using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;
using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V2
{
    /// <summary>
    /// 版本示例
    /// </summary>
    [ApiVersion("2.0")]
    public class DefaultController : ApiController
    {
        /// <summary>
        /// 测试版本1
        /// </summary>
        /// <returns></returns>
        public ResultContent<IEnumerable<string>> Test()
        {
            return new ResultContent<IEnumerable<string>>
            {
                Data = new[] { "版本2", "版本2" }
            };
        }
    }
}