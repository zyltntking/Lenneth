using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Lenneth.WebApi.Core.Redis;
using Unity;

namespace Lenneth.WebApi.Core.Filter
{
    /// <summary>
    /// AppKey验证程序
    /// </summary>
    internal class AppKeyValidate : DelegatingHandler
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpConfiguration">webapi配置</param>
        public AppKeyValidate(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration);
        }

        /// <summary>
        /// 拦截任务
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains(Resources.AppResource.ApiKeyHeader))
            {
                var apiKey = request.Headers.GetValues(Resources.AppResource.ApiKeyHeader).First();

                if (Validate(apiKey))
                {
                    return await base.SendAsync(request, cancellationToken);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            var errorkey = ResponseUtility.InitResult<object>(MessageCode.ApiKeyValiFail);

            response.Content = new ObjectContent(errorkey.GetType(), errorkey, new JsonMediaTypeFormatter());

            return response;
        }

        /// <summary>
        /// 验证apiKey权限
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <returns></returns>
        private bool Validate(string apiKey)
        {
            var redis = UnityConfig.Container.Resolve<IRedisConnector>();
            if (redis.KeyExists(apiKey))
            {
                return true;
            }
            return false;
        }
    }
}