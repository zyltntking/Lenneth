using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Lenneth.WebApi.Core.Log;
using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;

namespace Lenneth.WebApi.Core.Filter
{
    /// <summary>
    /// WebApi 异常处理程序
    /// </summary>
    internal class ExceptionHandler : FilterAttribute, IExceptionFilter
    {
        //public override void OnException(HttpActionExecutedContext actionExecutedContext)
        //{
        //    ILogWapper log = new NLogWrapper(AppConfig.WebApiExceptionLogConfig);
        //    log.Error(actionExecutedContext.Exception, actionExecutedContext.Request.RequestUri.LocalPath);

        //    var apiException = ResponseUtility.InitResult<object>(MessageCode.ApiInterfaceException);
        //    apiException.Data = actionExecutedContext.Exception.Message;

        //    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, apiException);
        //}

        #region Implementation of IExceptionFilter

        /// <summary>执行异步异常筛选器。</summary>
        /// <returns>异步异常筛选器。</returns>
        /// <param name="actionExecutedContext">操作执行的上下文。</param>
        /// <param name="cancellationToken">取消标记。</param>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}