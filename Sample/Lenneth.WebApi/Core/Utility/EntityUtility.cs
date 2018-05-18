using System.Collections.Generic;
using System.Linq;

namespace Lenneth.WebApi.Core.Utility
{
    internal static class EntityUtility
    {
        public static List<T> Pagepagination<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}