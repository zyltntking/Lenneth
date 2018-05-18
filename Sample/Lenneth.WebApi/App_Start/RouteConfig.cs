using System.Web.Mvc;
using System.Web.Routing;

namespace Lenneth.WebApi
{
    /// <summary>
    /// MVC路由配置
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// 全局路由注册
        /// </summary>
        /// <param name="routes">路由集合</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Root",
                url: "",
                defaults: new { controller = "RouteView", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetApiKey",
                url: "GetApiKey",
                defaults: new { controller = "RouteView", action = "GetApiKey", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Entity",
                url: "Entity",
                defaults: new { controller = "RouteView", action = "Entity", id = UrlParameter.Optional }
            );
        }
    }
}