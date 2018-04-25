using Lenneth.WebApi.Core.Log;
using StackExchange.Profiling;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lenneth.WebApi
{
    /// <inheritdoc />
    /// <summary>
    /// 全局启动入口
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// 启动时运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // MiniProfiler权限验证
            MiniProfiler.Settings.Results_Authorize = IsUserAllowedToSeeMiniProfilerUI;
        }

        #region RequestLifeCycle

        /// <summary>
        /// 请求开始时
        /// </summary>
        private void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }

            if (/*!Request.IsLocal && */Request.Url.AbsolutePath.Contains(@"/WebApi/"))
            {
                ILogWapper log = new NLogWrapper(AppConfig.WebApiRequestLogConfig);
                log.Info(Request.Url.AbsolutePath);
            }
        }

        /// <summary>
        /// 获取请求用户的信息时
        /// </summary>
        private void Application_AuthenticateRequest()
        {
        }

        /// <summary>
        /// 已经获取请求用户的信息时
        /// </summary>
        private void Application_PostAuthenticateRequest()
        {
        }

        /// <summary>
        /// 用户请求授权时
        /// </summary>
        private void Application_AuthorizeRequest()
        {
        }

        /// <summary>
        /// 用户请求已经得到授权时
        /// </summary>
        private void Application_PostAuthorizeRequest()
        {
        }

        /// <summary>
        /// 获取缓存时
        /// </summary>
        private void Application_ResolveRequestCache()
        {
        }

        /// <summary>
        /// 已经取得缓存时
        /// </summary>
        private void Application_PostResolveRequestCache()
        {
        }

        /// <summary>
        /// 请求处理器对象已创建成功时
        /// </summary>
        private void Application_PostMapRequestHandler()
        {
        }

        /// <summary>
        /// 获取请求状态时
        /// </summary>
        private void Application_AcquireRequestState()
        {
        }

        /// <summary>
        /// 已获取请求状态时
        /// </summary>
        private void Application_PostAcquireRequestState()
        {
        }

        /// <summary>
        /// 准备执行处理程序时
        /// </summary>
        private void Application_PreRequestHandlerExecute()
        {
        }

        /// <summary>
        /// 已经执行处理程序时
        /// </summary>
        private void Application_PostRequestHandlerExecute()
        {
        }

        /// <summary>
        /// 释放请求状态时
        /// </summary>
        private void Application_ReleaseRequestState()
        {
        }

        /// <summary>
        /// 已释放请求状态时
        /// </summary>
        private void Application_PostReleaseRequestState()
        {
        }

        /// <summary>
        /// 更新缓存时
        /// </summary>
        private void Application_UpdateRequestCache()
        {
        }

        /// <summary>
        /// 已更新缓存时
        /// </summary>
        private void Application_PostUpdateRequestCache()
        {
        }

        /// <summary>
        /// 操作请求日志时
        /// </summary>
        private void Application_LogRequest()
        {
        }

        /// <summary>
        /// 已操作请求日志时
        /// </summary>
        private void Application_PostLogRequest()
        {
        }

        /// <summary>
        /// 请求结束时
        /// </summary>
        private void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        #endregion RequestLifeCycle

        #region MiniProfiler

        /// <summary>
        /// MiniProfiler权限
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        private bool IsUserAllowedToSeeMiniProfilerUI(HttpRequest httpRequest)
        {
            // Implement your own logic for who
            // should be able to access ~/mini-profiler-resources/results
            //var principal = httpRequest.RequestContext.HttpContext.User;
            //return principal.IsInRole("Developer");
            return true;
        }

        #endregion MiniProfiler
    }
}