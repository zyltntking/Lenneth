using AutoMapper.QueryableExtensions;
using Lenneth.WebApi.Core.Session;
using Lenneth.WebApi.Models;
using Lenneth.WebApi.Models.Mapper;
using Newtonsoft.Json;
using StackExchange.Profiling;
using System.Linq;
using System.Web.Mvc;
using Unity;

namespace Lenneth.WebApi.Controllers
{
    /// <summary>
    /// 根视图路由
    /// </summary>
    public class RouteViewController : Controller
    {
        /// <summary>
        /// RedisSession
        /// </summary>
        private ISession RedisSession => UnityConfig.Container.Resolve<ISession>();

        /// <summary>
        /// 默认页
        /// </summary>
        /// <returns></returns>
        // GET: RouetView
        public ActionResult Index()
        {
            var apiKey = RedisSession.SessionId;
            RedisSession[Resources.AppResource.ApiKeyHeader] = apiKey;
            ViewBag.ApiKey = apiKey;

            return View();
        }

        /// <summary>
        /// 获取ApiKey
        /// </summary>
        /// <returns>apikey</returns>
        public string GetApiKey()
        {
            var apiKey = RedisSession.SessionId;
            RedisSession[Resources.AppResource.ApiKeyHeader] = apiKey;
            return apiKey;
        }

        /// <summary>
        /// Entity性能测试
        /// </summary>
        /// <returns></returns>
        public ActionResult Entity()
        {
            var profiler = MiniProfiler.Current;
            using (var db = new BIBOEntities())
            {
                var context = db.BIBOUsers;
                using (profiler.Step("query data"))
                {
                    var data = context.ProjectTo<UserInfo>().OrderBy(o => o.Uid).Where(c => c.UserName.Contains("张三")).Skip(0).Take(10).ToList();
                    ViewBag.data = JsonConvert.SerializeObject(data);
                }
            }

            return View();
        }
    }
}