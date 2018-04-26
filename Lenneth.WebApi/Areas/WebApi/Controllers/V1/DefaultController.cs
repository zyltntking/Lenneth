using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;
using Lenneth.WebApi.Core.Filter;
using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V1
{
    /// <summary>
    /// 版本化示例
    /// </summary>
    [ApiVersion("1.0")]
    [HeaderTokenAuth]
    public class DefaultController : ApiController
    {
        /// <summary>
        /// PostWithToken
        /// </summary>
        /// <returns></returns>
        public ResultContent<IEnumerable<string>> PostWithToken()
        {
            return new ResultContent<IEnumerable<string>>
            {
                Data = new[] { "版本1", "版本1" }
            };
        }

        /// <summary>
        /// GetWithToken
        /// </summary>
        /// <param name="name">arg2</param>
        /// <returns></returns>
        [HttpGet]
        public ResultContent<string> GetWithToken(string name)
        {
            return new ResultContent<string>
            {
                Data = name
            };
        }
    }
}