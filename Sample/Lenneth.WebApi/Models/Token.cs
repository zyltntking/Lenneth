using System;

namespace Lenneth.WebApi.Models
{
    internal class Token
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expire { get; set; }
    }
}