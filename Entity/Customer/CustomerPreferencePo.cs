
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户偏好表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:45
    /// </summary>
    [Class(Table = "t_customer_preference", Lazy = false, NameType = typeof(CustomerPreferencePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerPreferencePo
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
        /// key
        /// </summary>
        [Property(Column = "`key`")]
        public virtual string Key
        {
            get;
            set;
        }
        /// <summary>
        /// value
        /// </summary>
        [Property(Column = "value")]
        public virtual string Value
        {
            get;
            set;
        }
    }
}

