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

            routes.IgnoreRoute("RouteView/{action}");

            routes.IgnoreRoute("{controller}/index");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "RouteView", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}