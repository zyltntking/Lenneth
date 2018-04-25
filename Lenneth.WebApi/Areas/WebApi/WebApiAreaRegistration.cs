using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Web.Http.Routing;

namespace Lenneth.WebApi.Areas.WebApi
{
    /// <summary>
    /// WebApi区域配置
    /// </summary>
    public class WebApiAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 区域名
        /// </summary>
        public override string AreaName => "WebApi";

        /// <summary>
        /// 注册区域
        /// </summary>
        /// <param name="context">注册实体上下文</param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "WebApi_default",
            //    "WebApi/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            context.Routes.MapHttpRoute(
                name: "WebApi_default",
                routeTemplate: "WebApi/v{apiVersion}/{controller}/{action}/{id}",
                //routeTemplate: "WebApi/v{apiVersion}/{controller}/{id}", // RESTful Style
                defaults: new { id = RouteParameter.Optional },
                constraints: new { apiVersion = new ApiVersionRouteConstraint() });
        }
    }
}