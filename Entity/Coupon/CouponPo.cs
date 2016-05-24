using System;
using Com.Panduo.Service.Coupon;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Coupon
{
    /// <summary>
    ///描述：coupon表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-28 14:29:24
    /// </summary>
    [Class(Table = "t_coupon", Lazy = false, NameType = typeof(CouponPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CouponPo
    {
        /// <summary>
        /// id
        /// </summary>
        [Id(1, Name = "CouponId", Column = "coupon_id")]
        [Generator(2, Class = "native")]
        public virtual int CouponId
        {
            get;
            set;
        }
        /// <summary>
        /// coupon号
        /// </summary>
        [Property(Column = "coupon_code")]
        public virtual string CouponCode
        {
            get;
            set;
        }
        /// <summary>
        /// 面额
        /// </summary>
        [Property(Column = "amount")]
        public virtual decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 面额币种
        /// </summary>
        [Property(Column = "amount_currency")]
        public virtual int AmountCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 起用金额
        /// </summary>
        [Property(Column = "min_amount")]
        public virtual decimal MinAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 起用金额币种
        /// </summary>
        [Property(Column = "min_amount_currency")]
        public virtual int MinAmountCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 1有效，0无效
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 领取开始时间
        /// </summary>
        [Property(Column = "get_date_started")]
        public virtual DateTime? GetDateStarted
        {
            get;
            set;
        }
        /// <summary>
        /// 领取结束时间
        /// </summary>
        [Property(Column = "get_date_ended")]
        public virtual DateTime? GetDateEnded
        {
            get;
            set;
        }
        /// <summary>
        /// 0：相对时间，1：绝对时间
        /// </summary>
        [Property(Column = "expired_type")]
        public virtual LimitType ExpiredType
        {
            get;
            set;
        }
        /// <summary>
        /// 使用期限
        /// </summary>
        [Property(Column = "expired_day")]
        public virtual int? ExpiredDay
        {
            get;
            set;
        }
        /// <summary>
        /// 使用开始时间
        /// </summary>
        [Property(Column = "use_date_started")]
        public virtual DateTime? UseDateStarted
        {
            get;
            set;
        }
        /// <summary>
        /// 使用结束时间
        /// </summary>
        [Property(Column = "use_date_ended")]
        public virtual DateTime? UseDateEnded
        {
            get;
            set;
        }
        /// <summary>
        /// 1允许，0不允许
        /// </summary>
        [Property(Column = "get_able")]
        public virtual bool GetAble
        {
            get;
            set;
        }
        /// <summary>
        /// 0表示不限制
        /// </summary>
        [Property(Column = "get_person_limit")]
        public virtual int GetPersonLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 1允许，0不允许
        /// </summary>
        [Property(Column = "get_allow_duplicate")]
        public virtual bool GetAllowDuplicate
        {
            get;
            set;
        }
        /// <summary>
        /// 领取币种，存id，多个逗号隔开
        /// </summary>
        [Property(Column = "get_currency_code")]
        public virtual string GetCurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 领取语言
        /// </summary>
        [Property(Column = "get_language_id")]
        public virtual string GetLanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 多个id以逗号隔开
        /// </summary>
        [Property(Column = "get_country_id")]
        public virtual string GetCountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 金额对象 default 0：物品总金额、1：正价商品金额
        /// </summary>
        [Property(Column = "amount_type")]
        public virtual AmountType? AmountType
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
    }
}

