
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Property
{
    /// <summary>
    ///描述：属性值表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:25
    /// </summary>
    [Class(Table = "t_property_value", Lazy = false, NameType = typeof(PropertyValuePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PropertyValuePo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "PropertyValueId", Column = "property_value_id")]
        [Generator(2, Class = "native")]

        public virtual int PropertyValueId
        {
            get;
            set;
        }
        /// <summary>
        /// 属性id
        /// </summary>
        [Property(Column = "property_id")]
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
        /// 有效1，无,0
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
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
        /// 属性值组id
        /// </summary>
        [Property(Column = "property_value_group_id")]
        public virtual int PropertyValueGroupId
        {
            get;
            set;
        }
    }
}

