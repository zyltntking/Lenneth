using Lenneth.WebApi.Core.Filter;
using Lenneth.WebApi.Models;
using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V2
{
    /// <summary>
    /// 版本化示例
    /// </summary>
    [ApiVersion("2.0")]
    [HeaderTokenAuth]
    public class DefaultController : ApiController
    {
        /// <summary>
        /// PostWithOutToken
        /// </summary>
        /// <returns></returns>
        public ResultContent<IEnumerable<string>> PostWithOutToken()
        {
            return new ResultContent<IEnumerable<string>>
            {
                Data = new[] { "版本2", "版本2" }
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