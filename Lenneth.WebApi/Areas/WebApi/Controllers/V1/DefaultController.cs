using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;
using Lenneth.WebApi.Core.Filter;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V1
{
    /// <summary>
    /// 版本示例
    /// </summary>
    [ApiVersion("1.0")]
    [HeaderTokenAuth]
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

        /// <summary>
        /// strtest
        /// </summary>
        /// <param name="token">arg1</param>
        /// <param name="test">arg2</param>
        /// <returns></returns>
        [HttpGet]
        public string StrTest(string test)
        {
            return $"{test}";
        }
    }
}