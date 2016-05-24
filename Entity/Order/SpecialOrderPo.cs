
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：t_special_order ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:27:45
    /// </summary>
    [Class(Table = "t_special_order", Lazy = false, NameType = typeof(SpecialOrderPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class SpecialOrderPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "SpecialId", Column = "special_id")]
        [Generator(2, Class = "native")]
        public virtual int SpecialId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 增加金额
        /// </summary>
        [Property(Column = "increase_money")]
        public virtual decimal IncreaseMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 币种编码
        /// </summary>
        [Property(Column = "currency_code")]
        public virtual string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "remark")]
        public virtual string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 是否通知客户
        /// </summary>
        [Property(Column = "is_notify_customer")]
        public virtual bool IsNotifyCustomer
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:废弃，1:活动)
        /// </summary>
        [Property(Column = "status")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

