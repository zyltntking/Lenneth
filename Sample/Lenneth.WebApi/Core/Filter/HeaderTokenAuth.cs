using Lenneth.WebApi.Core.Hash;
using Lenneth.WebApi.Core.Redis;
using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Unity;

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
            if (actionContext.Request.Headers.Contains(Resources.AppResource.TokenHeader))
            {
                var token = actionContext.Request.Headers.GetValues(Resources.AppResource.TokenHeader).First();

                var apiKey = actionContext.Request.Headers.GetValues(Resources.AppResource.ApiKeyHeader).First();

                if (TokenAuth(apiKey, token))
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
        /// <param name="apiKey"></param>
        /// <param name="clientToken"></param>
        /// <returns></returns>
        private bool TokenAuth(string apiKey, string clientToken)
        {
            var redis = UnityConfig.Container.Resolve<IRedisConnector>();
            var hash = UnityConfig.Container.Resolve<IHash>();

            var crypt = redis.HashGetStr(apiKey, Resources.AppResource.TokenHeader);

            if (string.IsNullOrWhiteSpace(crypt))
            {
                return false;
            }

            var serverToken = hash.ComputeHash(crypt);
            var isEqual = string.Equals(serverToken, clientToken);

            var date = Convert.ToDateTime(redis.HashGetStr(apiKey, Resources.AppResource.ExpireHeader),Resources.AppResource.Culture);
            var isVali = date > DateTime.Now;

            return isEqual && isVali;
        }

        #endregion Implementation of IActionFilter
    }
}