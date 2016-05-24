using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.ServiceConst;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 订单费用
    /// </summary>
    [Serializable]
    public class OrderCost
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 原始产品金额：‎所有产品原价总额
        /// </summary>
        public virtual decimal OriginalProductAmount { get; set; }

        /// <summary>
        /// 正价产品金额：‎没有促销折扣的产品原价总额‎
        /// </summary>
        public virtual decimal NoDiscountProductAmount { get; set; }

        /// <summary>
        /// 促销折扣产品金额：有促销折扣的产品总额‎[sum(产品原价*促销折扣)]
        /// </summary>
        public virtual decimal DiscountProductAmount { get; set; }
        
        /// <summary>
        /// 订单产品促销优惠金额‎
        /// </summary>
        public virtual decimal PromotionLessAmount { get; set; }

        /// <summary>
        /// VIP折扣：0.0
        /// </summary>
        public virtual decimal VipDiscount { get; set; }
        /// <summary>
        /// VIP折扣优惠金额
        /// </summary>
        public virtual decimal VipLessAmount { get; set; }

        /// <summary>
        /// 订单折扣:0.0 营销模块设置
        /// </summary>
        public virtual decimal OrderDiscount { get; set; }
        /// <summary>
        /// 订单折扣优惠金额
        /// </summary>
        public virtual decimal OrderDiscountLessAmount { get; set; }
        
        /*/// <summary>
        /// RCD折扣
        /// </summary>
        public virtual decimal RcdDiscount { get; set; }*/
        
        /// <summary>
        /// 基础运费
        /// </summary>
        public virtual decimal BaseShippingCost { get; set; }

        /// <summary>
        /// 偏远费
        /// </summary>
        public virtual decimal FarawayCost { get; set; }

        /// <summary>
        /// Club手续费
        /// </summary>
        public virtual decimal ClubFee { get; set; }

        /// <summary>
        /// Club运费差额
        /// </summary>
        public virtual decimal ClubDiffAmt { get; set; }

        /// <summary>
        /// 运费折扣：0.0
        /// </summary>
        public virtual decimal ShippingCostDiscount { get; set; }
        
        /// <summary>
        /// 手续费：FreeShipping手续费等
        /// </summary>
        public virtual decimal FreeShippingFee { get; set; }
        /// <summary>
        /// FreeShipping差额
        /// </summary>
        public virtual decimal FreeShippingDiffAmt { get; set; }

        /// <summary>
        /// 业务优惠金额
        /// </summary>
        public virtual decimal BusinessDerateAmount { get; set; }

        /// <summary>
        /// 业务附加费
        /// </summary>
        public virtual decimal BusinessSurcharge { get; set; }

        /// <summary>
        /// 退款
        /// </summary>
        public virtual decimal Refund { get; set; }
        
        /// <summary>
        /// 使用Coupon(优惠卷)的额度
        /// </summary>
        public virtual decimal CouponAmt { get; set; }

        /// <summary>
        /// 使用Cash支付的额度
        /// </summary>
        public virtual decimal CashAmt { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastModifyTime { get; set; }

        #region 动态列属性
        /// <summary>
        /// 总运费:计算列[基础运费+偏远费+FreeShipping差额+Club运费差额]
        /// </summary>
        public virtual decimal TotalShippingCost { get; set; }

        /// <summary>
        /// 产品总额:计算列[正价产品金额+促销折扣产品金额]
        /// </summary>
        public virtual decimal TotalProductAmt { get; set; }

        /// <summary>
        /// 订单总额：计算列[正价产品金额+折扣产品金额+基础运费+偏远费+FreeShipping差额+Club运费差额+Club手续费+业务附加费-业务优惠金额-退款]
        /// </summary>
        public virtual decimal TotalOrderAmt { get; set; }

        #endregion

        /// <summary>
        /// 实际已支付
        /// </summary> 
        public virtual decimal PaymentAmount { get; set; }

        /// <summary>
        /// 待支付金额 = 订单总额- 优惠券金额 - Cash支付金额 - 实际已支付金额 - 实际已支付金额
        /// </summary>
        public virtual decimal NeedToPayAmt
        {
            get
            {
                var needToPayAmt = TotalOrderAmt - CouponAmt - CashAmt -PaymentAmount;
                return needToPayAmt >0 && needToPayAmt > ServiceConfig.AmountTolerance ? needToPayAmt :0;
            }
        } 
        

    }

}
