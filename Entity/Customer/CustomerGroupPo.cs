
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户等级表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:41
    /// </summary>
    [Class(Table = "t_customer_group", Lazy = false, NameType = typeof(CustomerGroupPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerGroupPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "CustomerGroupId", Column = "customer_group_id")]
        [Generator(2, Class = "native")]

        public virtual int CustomerGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 最小金额
        /// </summary>
        [Property(Column = "amount_min")]
        public virtual decimal AmountMin
        {
            get;
            set;
        }
        /// <summary>
        /// 最大金额
        /// </summary>
        [Property(Column = "amount_max")]
        public virtual decimal AmountMax
        {
            get;
            set;
        }
        /// <summary>
        /// %
        /// </summary>
        [Property(Column = "percentage")]
        public virtual decimal Percentage
        {
            get;
            set;
        }
        /// <summary>
        /// 合并RCD后折扣 %
        /// </summary>
        [Property(Column = "percentage_withrcd")]
        public virtual decimal PercentageWithRcd
        {
            get;
            set;
        }
    }
}

