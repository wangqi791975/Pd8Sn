using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Order.ShoppingCart;

namespace Com.Panduo.Web.Models.ShoppingCart
{
    public class ShoppingCartItemVo : ShoppingCartItem
    {

        /// <summary>
        /// 产品当前语种名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string ProductEnName { get; set; }

        /// <summary>
        /// 产品主图
        /// </summary>
        public virtual string MainImage { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// 产品类别ID
        /// </summary>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 重量(g)
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 体积重(g)
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }

        public virtual int Tip { get; set; }
        public virtual ProdDiscountType ProdDiscountType { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal OriginalPrice { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string BackorderDays { get; set; }
        public virtual decimal ProductSubTotal { get; set; }

    }

}