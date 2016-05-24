using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShoppingCart
{
    /// <summary>
    /// 购物车
    /// </summary>
    [Serializable]
    public class ShoppingCartSummary
    {
        /// <summary>
        /// 购物车Id 未登录是临时ID(生成的负值）
        /// </summary>
        public virtual int ShoppingCartId { get; set; }

        /// <summary>
        /// 原始产品金额：‎所有产品原价总额
        /// </summary>
        public virtual decimal OriginalProductAmount { get; set; }

        /// <summary>
        /// 正价产品金额：‎没有促销折扣的产品原价总额‎
        /// </summary>
        public virtual decimal NoDiscountProductAmount { get; set; }

        /// <summary>
        /// 折扣前的总金额；包括正常的折扣商品以及一口价商品、Club商品；以正值显示
        /// </summary>
        public virtual decimal PromotionBeforeAmount { get; set; }
        /// <summary>
        /// 促销产品折扣优惠金额
        /// </summary>
        public virtual decimal PromotionDiscountAmount { get; set; }
        /// <summary>
        /// 促销折扣产品金额：有促销折扣的产品总额‎[sum(产品原价*促销折扣)]
        /// </summary>
        public virtual decimal PromotionAmount { get; set; }

        /// <summary>
        /// VIP折扣值（20%是0.8）
        /// </summary>
        public virtual decimal VipDiscount { get; set; }
        /// <summary>
        /// VIP折扣产品金额：正价产品金额*VIP折扣+促销折扣产品金额‎
        /// </summary>
        public virtual decimal VipDiscountAmount { get; set; }
        /// <summary>
        /// VIP and RCD折扣值（20%是0.8）
        /// </summary>
        public virtual decimal VipAndRcdDiscount { get; set; }
        /// <summary>
        /// VIP and RCD折扣产品金额：正价产品金额*VIP and RCD折扣+促销折扣产品金额‎
        /// </summary>
        public virtual decimal VipAndRcdDiscountAmount { get; set; }

        /// <summary>
        /// 订单折扣值（20%是0.8）
        /// </summary>
        public virtual decimal OrderDiscount { get; set; }

        /// <summary>
        /// 订单折扣产品金额‎：正价产品金额*订单折扣+促销折扣产品金额
        /// 订单折扣(营销模块设置)
        /// </summary>
        public virtual decimal OrderDiscountAmount { get; set; }

        /// <summary>
        /// 最终折扣类型（枚举）‎ : ‎订单折扣、VIP折扣‎
        /// </summary>
        public virtual DiscountType DiscountType { get; set; }
        
        /// <summary>
        /// 产品总数
        /// </summary>
        public virtual int TotalQuantity { get; set; }
        
        /// <summary>
        /// 总物理重 （单位：g）
        /// </summary>
        public virtual decimal GrossWeight { get; set; }

        /// <summary>
        /// 总体积重 （单位：g）
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }

        /// <summary>
        /// 发货重量(体积重与物理重哪个大取哪个) （单位：g）
        /// </summary>
        public virtual decimal ShippingWeight { get; set; }

        /// <summary>
        /// 包装箱重量 （单位：g）
        /// </summary>
        public virtual decimal PackageWeight { get; set; }

        /// <summary>
        /// Club计算运费重量 （单位：g）
        /// </summary>
        public virtual decimal ClubWeight { get; set; }

        /// <summary>
        /// 总金额
        /// 备注：默认取VipDiscount 
        /// </summary>
        public virtual decimal GrandTotal { get; set; }

        #region 当前折扣提醒
        public virtual bool HasCurrentDiscountTip { get; set; }
        public virtual string CurrentDiscountType { get; set; }
        public virtual decimal CurrentDiscount { get; set; }
        public virtual decimal ReplacingDiscount { get; set; }
        public virtual decimal ReplacingDiscountAmount { get; set; }
        #endregion

        #region 注释
        //Rcd折扣产品金额：正价产品金额*RCD折扣+促销折扣产品金额‎
        //public virtual decimal RcdDiscountProductAmount { get; set; }
        /// <summary>
        /// 产品促销优惠金额‎[计算列(界面显示)：原始产品金额-正价产品金额-促销折扣产品金额]
        /// </summary>
        //public virtual decimal PromotionLessAmount { get; set; }

        /// <summary>
        /// VIP折扣优惠金额‎[计算列(界面显示)：原始产品金额-VIP折扣产品金额]
        /// </summary>
        //public virtual decimal VipLessAmount { get; set; }

        /// <summary>
        /// 订单折扣优惠金额‎[计算列(界面显示)：原始产品金额-订单折扣产品金额]
        /// </summary>
        //public virtual decimal OrderDiscountLessAmount { get; set; }
        /// <summary>
        /// 基础运费（原始运费）
        /// </summary>
        //public virtual decimal BaseShippingCost { get; set; }

        /// <summary>
        /// 偏远费
        /// </summary>
        //public virtual decimal FarawayCost { get; set; }

        /// <summary>
        /// Club手续费
        /// </summary>
        //public virtual decimal ClubFee { get; set; }

        /// <summary>
        /// Club运费差额
        /// </summary>
        //public virtual decimal ClubDiffAmt { get; set; }

        /*/// <summary>
        /// 运费折扣
        /// </summary>
        public virtual decimal ShippingCostDiscount { get; set; }
        */

        /// <summary>
        /// 运费折扣金额
        /// </summary>
        //public virtual decimal ShippingCostDiscountAmount { get; set; }

        /// <summary>
        /// 促销产品数
        /// </summary>
        //public virtual int PromotionProductQty { get; set; }
        #endregion
    }
    /// <summary>
    /// 订单最终折扣类型
    /// </summary>
    public enum DiscountType
    {
        NoDiscount,
        OrderDiscount,
        VipDiscount
    }
}
