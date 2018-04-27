using System.Threading;
using System.Web.Mvc;
using Lenneth.WebApi.Core.Crypt;
using StackExchange.Profiling;
using Unity;

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
            // it's ok if this is null
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Set page title"))
            {
                ViewBag.Title = "Home Page";
            }
            using (profiler.Step("Doing complex stuff"))
            {
                using (profiler.Step("Step A"))
                { // something more interesting here
                    Thread.Sleep(100);
                }
                using (profiler.Step("Step B"))
                { // and here
                    Thread.Sleep(250);
                }
            }

            return View();
        }
    }
}