using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace Lenneth.WebApi.Models.Response
{
    /// <summary>
    /// 大规模数据服务端响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MassiveDataResponse<T>
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public List<T> data { get; set; }

        /// <summary>
        /// 数组总规模
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime timestamp { get; set; }
    }
}