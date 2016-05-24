using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Com.Panduo.Service;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// Solr查询返回结果
    /// </summary>
    [Serializable]
    public class SolrQueryResultData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SolrQueryResultData()
        {
            AllCategoryQtyMap = new Dictionary<int, int>();
            PropertyValueQtyMap = new Dictionary<int, int>();
            PropertyValueGroupQtyMap = new Dictionary<int, int>();
            FiterPropertyValueQtyMap = new Dictionary<int, IDictionary<int, int>>();
            FiterPropertyValueGroupQtyMap = new Dictionary<int, IDictionary<int, int>>();
        }
        /// <summary>
        /// 分页信息
        /// </summary>
        public virtual Pager Pager { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public virtual IList<ProductSolrInfo> DataList { get; set; }

        /// <summary>
        /// 根级类别数量字典
        /// </summary>
        public virtual ICollection<KeyValuePair<string, int>> RootCategoryQtyMap { get; set; }
        /// <summary>
        /// 上级类别数据字典
        /// </summary>
        public virtual ICollection<KeyValuePair<string, int>> ParentCategoryQtyMap { get; set; }
        /// <summary>
        /// 末级类别数据字典
        /// </summary>
        public virtual ICollection<KeyValuePair<string, int>> CategoryQtyMap { get; set; }
        /// <summary>
        /// 所有类别数量数据字典,Key为类别ID，value为对应产品数量
        /// </summary>
        public virtual IDictionary<int,int> AllCategoryQtyMap { get; set; }

        /// <summary>
        /// 属性数量数据字典,Key为属性ID，value为对应产品数量
        /// </summary>
        public virtual IDictionary<int, int> PropertyValueQtyMap { get; set; }
        /// <summary>
        /// 属性值数量数据字典,Key为属性值ID，value为对应产品数量
        /// </summary>
        public virtual IDictionary<int, int> PropertyValueGroupQtyMap { get; set; }
        /// <summary>
        /// 属性值过滤得到solr返回的属性值数量
        /// </summary>
        public virtual IDictionary<int, IDictionary<int, int>> FiterPropertyValueQtyMap { get; set; }
        /// <summary>
        /// 属性值过滤得到solr返回的属性值组数量
        /// </summary>
        public virtual IDictionary<int, IDictionary<int, int>> FiterPropertyValueGroupQtyMap { get; set; } 

    }
}
