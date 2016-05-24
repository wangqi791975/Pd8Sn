
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Shipping
{
    /// <summary>
    ///描述：配送方式对应国家到达天数表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 14:06:54
    /// </summary>
    [Class(Table = "t_shipping_day", Lazy = false, NameType = typeof(ShippingDayPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ShippingDayPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "ShippingDayId", Column = "shipping_day_id")]
        [Generator(2, Class = "native")]
        public virtual int ShippingDayId
        {
            get;
            set;
        }
        /// <summary>
        /// 配送方式ID
        /// </summary>
        [Property(Column = "shipping_id")]
        public virtual int ShippingId
        {
            get;
            set;
        }
        /// <summary>
        /// 国家二级编码
        /// </summary>
        [Property(Column = "country_iso_code_2")]
        public virtual string CountryIsoCode2
        {
            get;
            set;
        }
        /// <summary>
        /// 最少天数
        /// </summary>
        [Property(Column = "day_low")]
        public virtual int DayLow
        {
            get;
            set;
        }
        /// <summary>
        /// 最大天数
        /// </summary>
        [Property(Column = "day_high")]
        public virtual int DayHigh
        {
            get;
            set;
        }
    }
}

