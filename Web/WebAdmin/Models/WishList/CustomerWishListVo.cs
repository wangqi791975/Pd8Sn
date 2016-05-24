using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.WishList
{
    public class CustomerWishListVo
    {
        /// <summary>
        /// 产品扩展信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// wishlist产品信息
        /// </summary>
        public CustomerWishListProduct WishListInfo { get; set; }


    }
}