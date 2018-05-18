// ReSharper disable InconsistentNaming

using System.Collections.Generic;

namespace Lenneth.WebApi.Models.Request
{
    /// <summary>
    /// 大规模数据服务端渲染请求
    /// </summary>
    public class MassiveDataRquest
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public string type { get; set; }
        
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 页面尺寸
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 排序参数
        /// </summary>
        public SortInfo sortinfo { get; set; }

        /// <summary>
        /// 过滤参数
        /// </summary>
        public List<Filter> filters { get; set; }

    }

    /// <summary>
    /// 过滤参数
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// 过滤类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 关联属性
        /// </summary>
        public List<string> props { get; set; }

        /// <summary>
        /// 关联值
        /// </summary>
        public List<string> vals { get; set; }
    }

    /// <summary>
    /// 排序信息
    /// </summary>
    public class SortInfo
    {
        /// <summary>
        /// 排序属性
        /// </summary>
        public string prop { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string order { get; set; }
    }
}