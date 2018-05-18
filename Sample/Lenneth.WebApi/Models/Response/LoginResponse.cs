using System;

namespace Lenneth.WebApi.Models.Response
{
    /// <summary>
    /// 登陆响应
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expire { get; set; }
    }
}