using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 搜索产品属性
    /// </summary>
    [Serializable]
   public class SearchProductProperty
    {
        public SearchProductProperty()
        {

        }

        /// <summary>
        /// 属性ID
        /// </summary>
        public virtual Property Property { get; set; }

        /// <summary>
        /// 属性商品数
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 属性值商品数键值对,Key为属性值ID,Value为该属性值对应的商品数
        /// </summary>
        public virtual IList<KeyValuePair<PropertyValue, int>> PropertyValueQtys { get; set; }

        /// <summary>
        /// 属性值组商品数量列表
        /// </summary>
        public virtual IList<SearchProductPropertyGroup> PropertyValueGroupQtys { get; set; }

    }
}
