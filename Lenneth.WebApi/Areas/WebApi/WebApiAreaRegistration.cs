using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Web.Http.Routing;

namespace Lenneth.WebApi.Areas.WebApi
{
    public class WebApiAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 区域名
        /// </summary>
        public override string AreaName => "WebApi";

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