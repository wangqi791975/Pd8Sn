
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：订单支付日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-03-03 13:54:19
    /// </summary>
    [Class(Table = "t_order_payment_log", Lazy = false, NameType = typeof(OrderPaymentLogPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderPaymentLogPo
    {
        /// <summary>
        /// 主键,自增长
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 支付类型:Paypal = 0,Hsbc = 1,BankOfChina=2,WesternUnion = 4,Gc=8,MoneyGram =16,Webmoney=32,Yandex=64,QiWi=128,OceanCreditCard=256
        /// </summary>
        [Property(Column = "payment_type")]
        public virtual int PaymentType
        {
            get;
            set;
        }
        /// <summary>
        /// 支付日志对应的ID
        /// </summary>
        [Property(Column = "log_object_id")]
        public virtual int LogObjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已通讯：1-已通讯，0-未通讯
        /// </summary>
        [Property(Column = "is_communicated")]
        public virtual bool IsCommunicated
        {
            get;
            set;
        }
        /// <summary>
        /// 通讯时间
        /// </summary>
        [Property(Column = "communicated_date")]
        public virtual DateTime? CommunicatedDate
        {
            get;
            set;
        }
        /// <summary>
        /// 支付时间
        /// </summary>
        [Property(Column = "payment_date")]
        public virtual DateTime PaymentDate
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
    }
}

