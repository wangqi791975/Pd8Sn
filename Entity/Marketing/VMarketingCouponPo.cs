
using System;
using Com.Panduo.Service.Marketing;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    [Class(Table = "v_marketing_coupon", Lazy = false, NameType = typeof(VMarketingCouponPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class VMarketingCouponPo
    {
        /// <summary>
        /// MarketingId
        /// </summary>
        [Id(1, Name = "Id", Column = "Id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 营销活动名称
        /// </summary>
        [Property(Column = "`Name`")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 0：所有客户 ；1：VIP等级客户；2：导入客户
        /// </summary>
        [Property(Column = "CustomerType")]
        public virtual int CustomerType
        {
            get;
            set;
        }
        /// <summary>
        /// 存VIP等级ID字符串，用|分隔。 为空表示所有
        /// </summary>
        [Property(Column = "TargetCustomerVipLevel")]
        public virtual string Targetcustomerviplevel
        {
            get;
            set;
        }
        /// <summary>
        /// 存Club等级字符串，用|分隔。 为空表示所有
        /// </summary>
        [Property(Column = "TargetClubLevel")]
        public virtual string TargetClubLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制排除Club
        /// </summary>
        [Property(Column = "IsExcludeClub")]
        public virtual bool IsExcludeclub
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制排除渠道商
        /// </summary>
        [Property(Column = "IsExcludeChannels")]
        public virtual bool IsExcludeChannels
        {
            get;
            set;
        }
        /// <summary>
        /// 存站点ID字符串，用|分隔。 为空表示所有
        /// </summary>
        [Property(Column = "TargetLanguages")]
        public virtual string TargetLanguages
        {
            get;
            set;
        }
        /// <summary>
        /// 存币种ID字符串，用|分隔。 为空表示所有
        /// </summary>
        [Property(Column = "TargetCurrencies")]
        public virtual string TargetCurrencies
        {
            get;
            set;
        }
        /// <summary>
        /// 存国家ID字符串，用|分隔。 为空表示所有
        /// </summary>
        [Property(Column = "TargetCountry")]
        public virtual string TargetCountry
        {
            get;
            set;
        }
        /// <summary>
        /// default 0：物品总金额、1：正价商品金额
        /// </summary>
        [Property(Column = "AmountType")]
        public virtual MarketingAmountType? AmountType
        {
            get;
            set;
        }
        /// <summary>
        /// 有效期Begin
        /// </summary>
        [Property(Column = "EffectiveBegin")]
        public virtual DateTime? EffectiveBegin
        {
            get;
            set;
        }
        /// <summary>
        /// 有效期End
        /// </summary>
        [Property(Column = "EffectiveEnd")]
        public virtual DateTime? EffectiveEnd
        {
            get;
            set;
        }
        /// <summary>
        /// 单选枚举[1：注册活动、2：订单折扣、3：运费活动(折扣、升级、freeshipping)、4：订单支付活动]
        /// </summary>
        [Property(Column = "MarketingType")]
        public virtual int MarketingType
        {
            get;
            set;
        }
        /// <summary>
        /// 启用、禁用
        /// </summary>
        [Property(Column = "Status")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 最后修改人
        /// </summary>
        [Property(Column = "LastModifyWho")]
        public virtual string Lastmodifywho
        {
            get;
            set;
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Property(Column = "LastModifyTime")]
        public virtual DateTime? Lastmodifytime
        {
            get;
            set;
        }
        /// <summary>
        /// default 0：注册送Coupon、1：生日送Coupon 
        /// </summary>
        [Property(Column = "RewardType")]
        public virtual int RewardType
        {
            get;
            set;
        }
    }
}

