using Lenneth.WebApi.Core.Filter;
using Lenneth.WebApi.Core.Utility;
using Lenneth.WebApi.Models;
using Lenneth.WebApi.Models.Mapper;
using Lenneth.WebApi.Models.Request;
using Lenneth.WebApi.Models.Response;
using Microsoft.Web.Http;
using System.Linq;
using System.Web.Http;

namespace Lenneth.WebApi.Areas.WebApi.Controllers.V1
{
    /// <summary>
    /// 服务端规模化数据查询
    /// </summary>
    [ApiVersion("1.0")]
    [HeaderTokenAuth]
    public class MassiveDataController : ApiController
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        public ResultContent<MassiveDataResponse<UserInfo>> UserInfo([FromBody] MassiveDataRquest request)
        {
            using (var context = new MapperContext())
            {
                var query = context.GetMapper<UserInfo, BIBOUser>();
                if (request.filters.Count > 0)
                {
                    var searchFilter = request.filters.First(t => string.Equals(t.type, "search"));
                    if (searchFilter.props.Count > 0 && searchFilter.vals.Count > 0)
                    {
                        query = query.Where(t => t.MobileNo.Contains(searchFilter.vals.First()));
                    }
                }

                var count = query.Count();
                var data = query.OrderBy(o => o.Uiid).Pagepagination(request.page,request.pageSize);
                return data.MassiveResult(count);
            }
        }
    }
}