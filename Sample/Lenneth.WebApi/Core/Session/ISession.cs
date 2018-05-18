namespace Lenneth.WebApi.Core.Session
{
    internal interface ISession
    {
        /// <summary>
        /// 获取SessionID
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// 设置和取得Session
        /// </summary>
        /// <param name="sessionKey">key</param>
        /// <returns></returns>
        string this[string sessionKey] { get; set; }

        /// <summary>
        /// 判断session是否存在
        /// </summary>
        /// <param name="sessionKey">key</param>
        /// <returns></returns>
        bool Exist(string sessionKey);

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sessionKey">key</param>
        /// <param name="value">val</param>
        /// <returns></returns>
        bool Set<T>(string sessionKey, T value);

        /// <summary>
        /// 取得session
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sessionKey">key</param>
        /// <returns>value</returns>
        T Get<T>(string sessionKey);
    }
}