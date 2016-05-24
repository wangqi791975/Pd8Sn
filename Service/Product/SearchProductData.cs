using System;
using System.Collections.Generic;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.Property;

namespace Com.Panduo.Service.Product
{
   /// <summary>
   /// 搜索产品数据
   /// </summary>
   [Serializable]
   public class SearchProductData
    { 
       /// <summary>
       /// 商品搜索数据
       /// </summary>
       public virtual PageData<Product> ProductPageData { get; set; }
       /// <summary>
       /// 类别搜索数据
       /// </summary>
       public virtual IList<SearchProductCategory> ProductCategories { get; set; }
       /// <summary>
       /// 属性搜索数据
       /// </summary>
       public virtual IList<SearchProductProperty> ProductProperties { get; set; }
    }
}
