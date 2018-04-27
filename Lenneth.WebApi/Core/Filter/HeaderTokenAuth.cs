using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Core.Filter
{
    /// <summary>
    /// WebApi token验证拦截器
    /// </summary>
    internal class HeaderTokenAuth : FilterAttribute, IActionFilter
    {
        #region Implementation of IActionFilter

        /// <summary>异步执行筛选器操作。</summary>
        /// <returns>为此操作新建的任务。</returns>
        /// <param name="actionContext">操作上下文。</param>
        /// <param name="cancellationToken">为此任务分配的取消标记。</param>
        /// <param name="continuation">在调用操作方法之后，委托函数将继续。</param>
        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.Request.Headers.Contains(Resources.AppResource.Token))
            {
                var token = actionContext.Request.Headers.GetValues(Resources.AppResource.Token).First();

                if (TokenAuth(token))
                {
                    return continuation.Invoke();
                }
            }

            var errorToken = ResponseUtility.InitResult<object>(MessageCode.TokenAuthFail);

            var errorTokenMessage = new HttpResponseMessage
            {
                Content = new ObjectContent(errorToken.GetType(), errorToken, new JsonMediaTypeFormatter())
            };

            return Task.FromResult(errorTokenMessage);
        }

        /// <summary>
        /// Token验证主体
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool TokenAuth(string token)
        {
            return true;
        }

        #endregion Implementation of IActionFilter
    }
}