using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Coupon
{
    /// <summary>
    ///描述：coupon-客户关联表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-28 14:29:28
    /// </summary>
    [Class(Table = "t_coupon_customer", Lazy = false, NameType = typeof(CouponCustomerPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CouponCustomerPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 客户id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// coupon id
        /// </summary>
        [Property(Column = "coupon_id")]
        public virtual int CouponId
        {
            get;
            set;
        }
        /// <summary>
        /// Coupon编号
        /// </summary>
        [Property(Column = "coupon_code")]
        public virtual string CouponCode
        {
            get;
            set;
        }
        /// <summary>
        /// 使用开始时间
        /// </summary>
        [Property(Column = "use_date_started")]
        public virtual DateTime UseDateStarted
        {
            get;
            set;
        }
        /// <summary>
        /// 使用结束时间
        /// </summary>
        [Property(Column = "use_date_ended")]
        public virtual DateTime UseDateEnded
        {
            get;
            set;
        }
        /// <summary>
        /// 面额
        /// </summary>
        [Property(Column = "amount")]
        public virtual decimal? Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 面额币种
        /// </summary>
        [Property(Column = "amount_currency")]
        public virtual int? AmountCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 起用金额
        /// </summary>
        [Property(Column = "min_amount")]
        public virtual decimal? MinAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 起用金额币种
        /// </summary>
        [Property(Column = "min_amount_currency")]
        public virtual int? MinAmountCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 使用币种，存id，多个逗号隔开
        /// </summary>
        [Property(Column = "use_currency_code")]
        public virtual string UseCurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 使用语言
        /// </summary>
        [Property(Column = "use_language_id")]
        public virtual string UseLanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 多个id以逗号隔开
        /// </summary>
        [Property(Column = "use_country_id")]
        public virtual string UseCountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 0未使用，1已使用，3已删除
        /// </summary>
        [Property(Column = "status")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 0未使用，1已使用，2已过期，3已删除（无用）
        /// </summary>
        [Property(Column = "final_status")]
        public virtual int FinalStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 10,20,30...
        /// </summary>
        [Property(Column = "source")]
        public virtual int Source
        {
            get;
            set;
        }
        /// <summary>
        /// 来源说明
        /// </summary>
        [Property(Column = "source_desc")]
        public virtual string SourceDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 使用时间
        /// </summary>
        [Property(Column = "date_used")]
        public virtual DateTime? DateUsed
        {
            get;
            set;
        }
        /// <summary>
        /// 使用订单号
        /// </summary>
        [Property(Column = "use_order_id")]
        public virtual int UseOrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 使用说明
        /// </summary>
        [Property(Column = "use_desc")]
        public virtual string UseDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 状态变更原因
        /// </summary>
        [Property(Column = "update_reason")]
        public virtual string UpdateReason
        {
            get;
            set;
        }
        /// <summary>
        /// 金额对象 default 0：物品总金额、1：正价商品金额
        /// </summary>
        [Property(Column = "amount_type")]
        public virtual int? AmountType
        {
            get;
            set;
        }
        /// <summary>
        /// 停用时间
        /// </summary>
        [Property(Column = "date_disabled")]
        public virtual DateTime? DateDisabled
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员id
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int? AdminId
        {
            get;
            set;
        }
    }
}

