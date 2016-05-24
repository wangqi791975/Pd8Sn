
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：如：是先显示国家还是先显示详情地址 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 16:53:09
    /// </summary>
    [Class(Table = "t_address_format", Lazy = false, NameType = typeof(AddressFormatPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AddressFormatPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "AddressFormatId", Column = "address_format_id")]
        [Generator(2, Class = "native")]
        public virtual int AddressFormatId
        {
            get;
            set;
        }
        /// <summary>
        /// 格式化
        /// </summary>
        [Property(Column = "address_format")]
        public virtual string AddressFormat
        {
            get;
            set;
        }
        /// <summary>
        /// 格式化1
        /// </summary>
        [Property(Column = "address_format1")]
        public virtual string AddressFormat1
        {
            get;
            set;
        }
    }
}

