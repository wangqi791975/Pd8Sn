
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Property
{
    /// <summary>
    ///描述：属性值组表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:28
    /// </summary>
    [Class(Table = "t_property_value_group", Lazy = false, NameType = typeof(PropertyValueGroupPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PropertyValueGroupPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "PropertyValueGroupId", Column = "property_value_group_id")]
        [Generator(2, Class = "native")]

        public virtual int PropertyValueGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性ID
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
    }
}

