
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Property
{
    /// <summary>
    ///描述：属性表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:22
    /// </summary>
    [Class(Table = "t_property", Lazy = false, NameType = typeof(PropertyPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PropertyPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "PropertyId", Column = "property_id")]
        [Generator(2, Class = "native")]
        public virtual int PropertyId
        {
            get;
            set;
        }
        /// <summary>
        /// code
        /// </summary>
        [Property(Column = "code")]
        public virtual string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
        /// <summary>
        /// 排序顺序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        [Property(Column = "sort_type")]
        public virtual int SortType
        {
            get;
            set;
        }
        /// <summary>
        /// 1是，0否
        /// </summary>
        [Property(Column = "is_basic")]
        public virtual bool IsBasic
        {
            get;
            set;
        }
        /// <summary>
        /// 1显示，0隐藏
        /// </summary>
        [Property(Column = "is_show")]
        public virtual bool IsShow
        {
            get;
            set;
        }
        /// <summary>
        /// 1有效，0无效
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
    }
}

