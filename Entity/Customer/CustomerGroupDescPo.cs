
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户等级名称表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:43
    /// </summary>
    [Class(Table = "t_customer_group_desc", Lazy = false, NameType = typeof(CustomerGroupDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerGroupDescPo
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
        /// 客户等级id
        /// </summary>
        [Property(Column = "customer_group_id")]
        public virtual int CustomerGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言别
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
    }
}

