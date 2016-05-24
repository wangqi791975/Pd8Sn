using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Category;

//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductDetailVo.cs
//创 建 人：罗海明
//创建时间：2015/01/15 13:40:40 
//功能说明：商品明细Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
namespace Com.Panduo.Web.Models.Product
{
    public class ProductDetailVo
    {
        /// <summary>
        /// 产品扩展信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 产品多语言信息(删除)
        /// </summary>
        public ProductLanguage ProductLanguage { get; set; }

        /// <summary>
        /// 营销标题
        /// </summary>
        public string ProductMarketingTitle { get; set; }

        /// <summary>
        /// 营销内容
        /// </summary>
        public CategoryAdvertisement CategoryAdvertisement { get; set; }

        /// <summary>
        /// 产品绑定的所有属性&属性值
        /// </summary>
        public IList<KeyValuePair<string, string>> ProductPropertyAndPropertyValue { get; set; }

        /// <summary>
        /// 产品其他包装
        /// </summary>
        public IList<ProductInfo> OtherPack { get; set; }

        /// <summary>
        /// 营销内容（删除）
        /// </summary>
        public string MarketingArea { get; set; }
    }
}