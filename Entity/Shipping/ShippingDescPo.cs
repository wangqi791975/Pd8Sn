
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Shipping
{
    /// <summary>
    ///描述：t_shipping_desc ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 14:06:55
    /// </summary>
    [Class(Table = "t_shipping_desc", Lazy = false, NameType = typeof(ShippingDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ShippingDescPo
    {
        /// <summary>
        /// id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// shipping_id
        /// </summary>
        [Property(Column = "shipping_id")]
        public virtual int ShippingId
        {
            get;
            set;
        }
        /// <summary>
        /// language_id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// name
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 配送方式简述
        /// </summary>
        [Property(Column = "shipping_description")]
        public virtual string ShippingDescription
        {
            get;
            set;
        }
    }
}

