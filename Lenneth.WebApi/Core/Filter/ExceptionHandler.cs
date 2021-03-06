﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

using Lenneth.WebApi.Core.Log;
using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;

using Unity;

namespace Lenneth.WebApi.Core.Filter
{
    /// <summary>
    /// WebApi 异常处理程序
    /// </summary>
    internal class ExceptionHandler : FilterAttribute, IExceptionFilter
    {
        #region Implementation of IExceptionFilter

        /// <summary>执行异步异常筛选器。</summary>
        /// <returns>异步异常筛选器。</returns>
        /// <param name="actionExecutedContext">操作执行的上下文。</param>
        /// <param name="cancellationToken">取消标记。</param>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var log = UnityConfig.Container.Resolve<ILogWapper>("WebApiExceptionLog");
                log.Error(actionExecutedContext.Exception, actionExecutedContext.Request.RequestUri.LocalPath);

                var apiException = ResponseUtility.InitResult<object>(MessageCode.ApiInterfaceException);
                apiException.Data = actionExecutedContext.Exception.Message;

                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, apiException);
            }, cancellationToken);
        }

        #endregion Implementation of IExceptionFilter
    }
}