using System.Collections.Generic;
using System.Web.Http;

using Lenneth.WebApi.Models;

using Microsoft.Web.Http;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V2
{
    /// <summary>
    /// 版本化示例
    /// </summary>
    [ApiVersion("2.0")]
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
    }
}