using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

using Lenneth.WebApi.Core.Log;
using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Core.Filter
{
    /// <summary>
    /// WebApi 异常处理程序
    /// </summary>
    internal class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ILogWapper log = new NLogWrapper(AppConfig.WebApiExceptionLogConfig);
            log.Error(actionExecutedContext.Exception, actionExecutedContext.Request.RequestUri.LocalPath);

            var apiException = ResponseUtility.InitResult<object>(MessageCode.ApiInterfaceException);
            apiException.Data = actionExecutedContext.Exception.Message;

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, apiException);
        }
    }
}