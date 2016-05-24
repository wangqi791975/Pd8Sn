using System;
using System.Collections.Generic;


namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 搜索产品属性值组
    /// </summary>
   [Serializable]
   public class SearchProductPropertyGroup
    {
        public SearchProductPropertyGroup()
        {
            PropertyValueQtys = new List<KeyValuePair<PropertyValue, int>>();
        }

       /// <summary>
        /// 属性值组ID
       /// </summary>
        public virtual PropertyValueGroup PropertyValueGroup { get; set; }

       /// <summary>
       /// 属性值组数量
       /// </summary>
       public virtual int Qty { get; set; }

       /// <summary>
       /// 属性值商品数量键值对，Key：属性值ID，Value：该属性值对应商品数量
       /// </summary>
       public virtual IList<KeyValuePair<PropertyValue, int>> PropertyValueQtys { get; set; }
    }
}
