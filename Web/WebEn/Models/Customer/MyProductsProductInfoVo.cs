//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：WishListProductInfoVo.cs
//创 建 人：罗海明
//创建时间：2015/01/29 13:40:40 
//功能说明：WishListVo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.Customer
{
    public class MyProductsProductInfoVo
    {
        /// <summary>
        /// 产品扩展信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// wishlist产品信息
        /// </summary>
        public CustomerProduct CustomerProductInfo { get; set; }
    }
}