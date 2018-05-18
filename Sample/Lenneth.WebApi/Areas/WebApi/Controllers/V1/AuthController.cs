using Lenneth.WebApi.Core.Crypt;
using Lenneth.WebApi.Core.Hash;
using Lenneth.WebApi.Core.Redis;
using Lenneth.WebApi.Core.Session;
using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;
using Lenneth.WebApi.Models.Request;
using Lenneth.WebApi.Models.Response;
using Microsoft.Web.Http;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using Unity;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V1
{
    /// <summary>
    /// 认证控制器
    /// </summary>
    [ApiVersion("1.0")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// RedisSession
        /// </summary>
        private ISession RedisSession => UnityConfig.Container.Resolve<ISession>();

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="request">登陆请求</param>
        /// <returns>登陆结果</returns>
        public ResultContent<LoginResponse> Login([FromBody]LoginRequest request)
        {
            // todo Check UserName And PassWord

            if (string.Equals(request.UserName, @"super") && string.Equals(request.PassWord, @"123456"))
            {
                var result = ResponseUtility.InitResult<LoginResponse>(MessageCode.Success);

                var expire = DateTime.Now.AddDays(1);

                var token = new Token
                {
                    Id = 1,
                    UserName = "super",
                    Expire = expire,
                    Type = Resources.AppResource.AccessToken
                };

                var crypt = UnityConfig.Container.Resolve<ICrypt>();

                var hash = UnityConfig.Container.Resolve<IHash>();

                var tokenString = crypt.Encrypt(JsonConvert.SerializeObject(token, Formatting.None));

                var response = new LoginResponse
                {
                    Token = hash.ComputeHash(tokenString),
                    Expire = expire
                };

                result.Data = response;

                RedisSession[Resources.AppResource.TokenHeader] = tokenString;
                RedisSession[Resources.AppResource.ExpireHeader] = expire.ToString(Resources.AppResource.Culture);

                return result;
            }

            return ResponseUtility.InitResult<LoginResponse>(MessageCode.UserNameOrPassWordError);
        }

        /// <summary>
        /// 静默授权
        /// </summary>
        /// <returns></returns>
        public ResultContent<LoginResponse> SilentAuth()
        {
            var redis = UnityConfig.Container.Resolve<IRedisConnector>();

            var sessionId = RedisSession.SessionId;

            var isSetApiKey = redis.HashExists(sessionId, Resources.AppResource.ApiKeyHeader);
            var isSetToken = redis.HashExists(sessionId, Resources.AppResource.TokenHeader);
            var isSetExpire = redis.HashExists(sessionId, Resources.AppResource.ExpireHeader);

            if (isSetApiKey && isSetToken && isSetExpire)
            {
                var expire = Convert.ToDateTime(redis.HashGetStr(sessionId, Resources.AppResource.ExpireHeader), Resources.AppResource.Culture);
                var tokenString = redis.HashGetStr(sessionId, Resources.AppResource.TokenHeader);
                var hash = UnityConfig.Container.Resolve<IHash>();
                if (expire > DateTime.Now)
                {
                    var result = ResponseUtility.InitResult<LoginResponse>(MessageCode.Success);

                    var response = new LoginResponse
                    {
                        Token = hash.ComputeHash(tokenString),
                        Expire = expire
                    };

                    result.Data = response;

                    return result;
                }
            }

            return ResponseUtility.InitResult<LoginResponse>(MessageCode.SilentAuthFail);
        }
    }
}