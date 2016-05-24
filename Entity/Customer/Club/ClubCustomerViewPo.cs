
using System;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Payment;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer.Club
{
    /// <summary>
    ///描述：club客户视图 ORM 映射类 
    ///创建人:王琦
    ///创建时间：2015-03-12 14:29:29
    /// </summary>
    [Class(Table = "v_club_customer", Lazy = false, NameType = typeof(ClubCustomerViewPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ClubCustomerViewPo
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
        /// 客户邮箱
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 客户等级
        /// </summary>
        [Property(Column = "customer_level")]
        public virtual string CustomerLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 客服经理名称
        /// </summary>
        [Property(Column = "manager_name")]
        public virtual string ManagerName
        {
            get;
            set;
        }

        /// <summary>
        /// transaction_id
        /// </summary>
        [Property(Column = "transaction_id")]
        public virtual string TransactionId
        {
            get;
            set;
        }

        /// <summary>
        /// website
        /// </summary>
        [Property(Column = "website")]
        public virtual string Website
        {
            get;
            set;
        }
    }
}

