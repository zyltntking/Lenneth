﻿using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V1
{
    /// <summary>
    /// 版本示例
    /// </summary>
    [ApiVersion("1.0")]
    public class DefaultController : ApiController
    {
        /// <summary>
        /// 测试版本1
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Test()
        {
            return new[] { "版本1", "版本1" };
        }
    }
}