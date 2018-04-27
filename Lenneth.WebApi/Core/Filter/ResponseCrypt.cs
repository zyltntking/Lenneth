using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Core.Filter
{
    /// <summary>
    /// 响应加密处理程序
    /// </summary>
    internal class ResponseCrypt : ActionFilterAttribute
    {
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            var responseBody = await actionExecutedContext.Response.Content.ReadAsAsync<ResultContent<object>>(cancellationToken);

            var content = ResponseUtility.EncryptContent(responseBody);

            actionExecutedContext.Response.Content = new ObjectContent(content.GetType(), content, new JsonMediaTypeFormatter());
        }
    }
}