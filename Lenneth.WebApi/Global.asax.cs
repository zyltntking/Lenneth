using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using StackExchange.Profiling;

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

        /// <summary>
        /// 请求开始时
        /// </summary>
        private void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        /// <summary>
        /// 请求结束时
        /// </summary>
        private void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

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
    }
}