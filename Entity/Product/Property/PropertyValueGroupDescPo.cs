
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Property
{
    /// <summary>
    ///描述：属性值组名称表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:29
    /// </summary>
    [Class(Table = "t_property_value_group_desc", Lazy = false, NameType = typeof(PropertyValueGroupDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PropertyValueGroupDescPo
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
        /// 属性值组id
        /// </summary>
        [Property(Column = "property_value_group_id")]
        public virtual int PropertyValueGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言别id
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

