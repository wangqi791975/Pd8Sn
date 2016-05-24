
using System;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Payment;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer.Club
{
    /// <summary>
    ///描述：club客户表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-28 14:29:29
    /// </summary>
    [Class(Table = "t_club_customer", Lazy = false, NameType = typeof(ClubCustomerPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ClubCustomerPo
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
        /// 经理id
        /// </summary>
        [Property(Column = "customer_manager_id")]
        public virtual int CustomerManagerId
        {
            get;
            set;
        }
        /// <summary>
        /// 0未激活，1已激活，2已享受优惠
        /// </summary>
        [Property(Column = "status")]
        public virtual ClubStatus Status
        {
            get;
            set;
        }
        /// <summary>
        /// 激活时间
        /// </summary>
        [Property(Column = "date_actived")]
        public virtual DateTime DateActived
        {
            get;
            set;
        }
        /// <summary>
        /// 费用
        /// </summary>
        [Property(Column = "fee")]
        public virtual decimal Fee
        {
            get;
            set;
        }
        /// <summary>
        /// club类型
        /// </summary>
        [Property(Column = "type")]
        public virtual ClubType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Property(Column = "added_date")]
        public virtual DateTime AddedDate
        {
            get;
            set;
        }
        /// <summary>
        /// 添加类型(自己注册/后台添加)
        /// </summary>
        [Property(Column = "added_type")]
        public virtual int? AddedType
        {
            get;
            set;
        }
        /// <summary>
        /// 付款状态
        /// </summary>
        [Property(Column = "pay_status")]
        public virtual PaymentStatus PaymentStatus
        {
            get;
            set;
        }
        /// <summary>
        ///支付类型
        /// </summary>
        [Property(Column = "pay_type")]
        public virtual PaymentType PayType
        {
            get;
            set;
        }
        /// <summary>
        /// 支付日志id
        /// </summary>
        [Property(Column = "pay_log_id")]
        public virtual int PayLogId
        {
            get;
            set;
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        [Property(Column = "ended_date")]
        public virtual DateTime EndedDate
        {
            get;
            set;
        }
        /// <summary>
        /// 最后一次加入Club节省运费
        /// </summary>
        [Property(Column = "saving_shipping_fee")]
        public virtual decimal SavingShippingFee
        {
            get;
            set;
        }

    }
}

