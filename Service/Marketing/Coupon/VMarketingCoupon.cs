using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.Coupon
{
    public class VMarketingCoupon
    {
        /// <summary>
        /// MarketingId
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 营销活动名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 0：所有客户 ；1：VIP等级客户；2：导入客户
        /// </summary>
        public virtual MarketingCustomerType CustomerType
        {
            get;
            set;
        }
        /// <summary>
        /// 存VIP等级ID字符串，用|分隔。 为空表示所有
        /// </summary>
        public virtual string Targetcustomerviplevel
        {
            get;
            set;
        }
        /// <summary>
        /// 存Club等级字符串，用|分隔。 为空表示所有
        /// </summary>
        public virtual string TargetClubLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制排除Club
        /// </summary>
        public virtual bool IsExcludeclub
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制排除渠道商
        /// </summary>
        public virtual bool IsExcludeChannels
        {
            get;
            set;
        }
        /// <summary>
        /// 存站点ID字符串，用|分隔。 为空表示所有
        /// </summary>
        public virtual string TargetLanguages
        {
            get;
            set;
        }
        /// <summary>
        /// 存币种ID字符串，用|分隔。 为空表示所有
        /// </summary>
        public virtual string TargetCurrencies
        {
            get;
            set;
        }
        /// <summary>
        /// 存国家ID字符串，用|分隔。 为空表示所有
        /// </summary>
        public virtual string TargetCountry
        {
            get;
            set;
        }
        /// <summary>
        /// default 0：物品总金额、1：正价商品金额
        /// </summary>
        public virtual MarketingAmountType? AmountType
        {
            get;
            set;
        }
        /// <summary>
        /// 有效期Begin
        /// </summary>
        public virtual DateTime? EffectiveBegin
        {
            get;
            set;
        }
        /// <summary>
        /// 有效期End
        /// </summary>
        public virtual DateTime? EffectiveEnd
        {
            get;
            set;
        }
        /// <summary>
        /// 单选枚举[1：注册活动、2：订单折扣、3：运费活动(折扣、升级、freeshipping)、4：订单支付活动]
        /// </summary>
        public virtual MarketingType MarketingType
        {
            get;
            set;
        }
        /// <summary>
        /// 启用、禁用
        /// </summary>
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public virtual string Lastmodifywho
        {
            get;
            set;
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime? Lastmodifytime
        {
            get;
            set;
        }
        /// <summary>
        /// default 0：注册送Coupon、1：生日送Coupon 
        /// </summary>
        public virtual int RewardType
        {
            get;
            set;
        }
    }
}
