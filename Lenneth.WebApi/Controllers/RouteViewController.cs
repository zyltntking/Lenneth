using System.Web.Mvc;

namespace Lenneth.WebApi.Controllers
{
    /// <summary>
    /// 根视图路由
    /// </summary>
    public class RouteViewController : Controller
    {
        /// <summary>
        /// 默认页
        /// </summary>
        /// <returns></returns>
        // GET: RouetView
        public ActionResult Index()
        {
            return View();
        }
    }
}