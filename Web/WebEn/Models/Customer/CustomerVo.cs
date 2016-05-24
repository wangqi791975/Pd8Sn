using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.Customer
{
    public class CustomerVo
    {
        public Service.Customer.Customer Customer { get; set; }

        public decimal Balance { get; set; }

        /// <summary>
        /// 客户税号
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// ClubCoupon
        /// </summary>
        public CouponCustomer ClubCoupon { get; set; }

        /// <summary>
        /// 注册Coupon
        /// </summary>
        public CouponCustomer RegisterCoupon { get; set; }

        public ClubCustomer ClubCustomer { get; set; }

        public IList<CustomerGroup> CustomerGroup { get; set; }

        public decimal NextNeedCost { get; set; }

        public decimal NextDiscount { get; set; }

        public IList<ProductInfo> ProductInfoList { get; set; }
    }
}