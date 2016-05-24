using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Category
{
    /// <summary>
    /// 查询产品类别
    /// </summary>
   [Serializable]
   public  class SearchProductCategory
    {
        public SearchProductCategory()
        {
            SubSearchProductCategorys = new List<SearchProductCategory>();
        }
        
       /// <summary>
       /// 类别Id
       /// </summary>
        public Category Category { get; set; }

       /// <summary>
       /// 类别商品数量
       /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 子类别商品数量信息
        /// </summary>
        public IList<SearchProductCategory> SubSearchProductCategorys { get; set; }
    }
}
