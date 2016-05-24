using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// URL参数键
    /// </summary>
    public static class UrlParameterKey
    {
        public const string Page = "page";
        /// <summary>
        /// 浏览版式：list=0 \ gallery=1
        /// </summary>
        public const string ViewMode = "view";
        /// <summary>
        /// 排序筛选项名称
        /// </summary>
        public const string Sort = "sort";
        /// <summary>
        /// 每页展示商品数量
        /// </summary>
        public const string PageSize = "show";

        public const string CategoryId = "categoryid";
        public const string InStock = "instock";
        public const string BestSeller = "bestseller";
        public const string OnSale = "onsale";
        
        /// <summary>
        /// 参数名称前缀：例：进行了多个属性值筛选时，P{n}依次显示属性值的ID。如p1=属性值1的ID&p2=属性值2的ID&p3=属性值3的ID
        /// </summary>
        public const string PropertyValuePrefix = "p";

        public const string PropertyValueGroupPrefix = "g";

        public const string ProductSearchKeyword = "keyword";

        /// <summary>
        /// 跳转
        /// </summary>
        public const string RedirectUrl = "redirectUrl";
        
    }
}