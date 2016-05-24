using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing
{
    /// <summary>
    /// 营销实体
    /// </summary>
    [Serializable]
    public class Marketing
    {
        /// <summary>
        /// 营销活动Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 营销活动名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 活动类型 枚举‎[注册、运费(折扣、升级、freeshipping)、下单支付、订单折扣、客户生日活动(单独的服务处理)]
        /// </summary>
        public virtual MarketingType MarketingType { get; set; }

        /// <summary>
        /// 是否强制排除Club
        /// </summary>
        public virtual bool IsExcludeClub { get; set; }

        /// <summary>
        /// 是否强制排除渠道商
        /// </summary>
        public virtual bool IsExcludeChannels { get; set; }

        /// <summary>
        /// 0：所有客户 ；1：VIP等级客户；2：导入客户
        /// </summary>
        public virtual MarketingCustomerType CustomerType { get; set; }

        /// <summary>
        /// 客户导入信息，key=客户Id，value=客户邮箱
        /// </summary>
        public virtual Dictionary<int, string> CustomerInfo { get; set; }

        /// <summary>
        /// 客户条件VIP等级，t_customer_group.Id 集合，为空则忽略该条件
        /// </summary>
        public virtual List<int> CustomerVipIds { get; set; }

        /// <summary>
        /// 存Club等级字符串，用|分隔。 为空表示所有
        /// </summary>
        public virtual List<int> ClubLevels { get; set; }

        /// <summary>
        /// 站点语种条件，t_language.Id 集合，为空则忽略该条件
        /// </summary>
        public virtual List<int> LanguageIds { get; set; }

        /// <summary>
        /// 币种条件，t_currency.Id 集合，为空则忽略该条件
        /// </summary>
        public virtual List<int> CurrencyIds { get; set; }

        /// <summary>
        /// 国家条件，t_country.Id 集合，为空则忽略该条件
        /// </summary>
        public virtual List<int> CountryIds { get; set; }

        /// <summary>
        /// default 0：物品总金额、1：正价商品金额
        /// </summary>
        public virtual MarketingAmountType? AmountType { get; set; }

        /// <summary>
        /// 有效期Begin
        /// </summary>
        public virtual DateTime? EffectiveBegin { get; set; }

        /// <summary>
        /// 有效期End
        /// </summary>
        public virtual DateTime? EffectiveEnd { get; set; }

        /// <summary>
        /// 状态:启用、禁用
        /// </summary>
        public virtual bool Status { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public virtual string LastModifyWho { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime? LastModifyTime { get; set; }
    }

    /// <summary>
    /// 活动类型枚举 单选：‎[注册、运费(折扣、升级、freeshipping)、下单支付、订单折扣、客户生日活动(单独的服务处理)]
    /// </summary>
    public enum MarketingType
    {
        /// <summary>
        /// 注册\生日等送Conpon活动
        /// </summary>
        SendCoupon,

        /// <summary>
        /// 运费活动(折扣、升级、freeshipping)
        /// </summary>
        Shipping,

        /// <summary>
        /// 下单送
        /// </summary>
        PlaceOrder,

        /// <summary>
        /// 订单折扣
        /// </summary>
        OrderDiscount,

        /// <summary>
        /// 送礼
        /// </summary>
        Gift,
    }

    /// <summary>
    /// default 0：物品总金额、1：正价商品金额
    /// </summary>
    public enum MarketingAmountType
    {
        /// <summary>
        /// 物品总金额
        /// </summary>
        TotalAmount,
        /// <summary>
        /// 正价商品金额
        /// </summary>
        NoDiscountTotalAmount,
    }
    /// <summary>
    /// 0：所有客户 ；1：VIP等级客户；2：Club客户；3：渠道商客户；4：导入客户
    /// </summary>
    public enum MarketingCustomerType
    {
        /// <summary>
        /// 所有客户
        /// </summary>
        AllCustomer = 0,
        /// <summary>
        /// VIP等级客户
        /// </summary>
        VipCustomer,
        /// <summary>
        /// Club客户
        /// </summary>
        ClubCustomer,
        /// <summary>
        /// 渠道商客户
        /// </summary>
        ChannelCustomer,
        /// <summary>
        /// 导入客户
        /// </summary>
        ImportCustomer,
    }
}
