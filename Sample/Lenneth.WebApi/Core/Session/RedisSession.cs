using System;
using System.Web;
using Lenneth.WebApi.Core.Hash;
using Lenneth.WebApi.Core.Redis;
using Unity;

namespace Lenneth.WebApi.Core.Session
{
    /// <summary>
    /// RedisSession
    /// </summary>
    internal class RedisSession : ISession
    {
        private HttpContext Context { get; }
        private bool Readonly { get; }
        private IRedisConnector Redis { get; } = UnityConfig.Container.Resolve<IRedisConnector>();
        private int TimeOut { get; }
        private static string  SessionName = "Lenneth_Session";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="timeOut"></param>
        public RedisSession(HttpContext context,bool isReadOnly = true,int timeOut = 120)
        {
            Context = context;
            Readonly = isReadOnly;
            TimeOut = timeOut;

            Redis.KeyExpire(SessionId, new TimeSpan(0, TimeOut, 0));
        }

        /// <summary>
        /// 计算SessionID
        /// </summary>
        public string SessionId
        {
            get
            {
                var hash = UnityConfig.Container.Resolve<IHash>();
                var cookie = Context.Request.Cookies.Get(SessionName);
                if (!string.IsNullOrEmpty(cookie?.Value)) return cookie.Value;
                var isessionid = hash.ComputeHash(Guid.NewGuid().ToString());
                var icookie = new HttpCookie(SessionName, isessionid)
                {
                    HttpOnly = Readonly,
                    Expires = DateTime.Now.AddMinutes(TimeOut)
                };
                Context.Response.Cookies.Add(icookie);
                return isessionid;

            }
        }

        /// <summary>
        /// 获取或设置session
        /// </summary>
        /// <param name="sessionKey"></param>
        public string this[string sessionKey]
        {
            get => Redis.HashGetStr(SessionId, sessionKey);
            set => Redis.HashSetStr(SessionId, sessionKey, value);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public bool Exist(string sessionKey) => Redis.HashExists(SessionId, sessionKey);

        /// <summary>
        /// 设置session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string sessionKey, T value) => Redis.HashSet(SessionId, sessionKey, value);

        /// <summary>
        /// 获取session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public T Get<T>(string sessionKey) => Redis.HashGet<T>(SessionId, sessionKey);
    }
}