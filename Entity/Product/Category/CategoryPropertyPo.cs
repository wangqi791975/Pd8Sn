
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Category
{
    /// <summary>
    ///描述：类别-属性关系表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:37:58
    /// </summary>
    [Class(Table = "t_category_property", Lazy = false, NameType = typeof(CategoryPropertyPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CategoryPropertyPo
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
        /// 类别id
        /// </summary>
        [Property(Column = "category_id")]
        public virtual int CategoryId
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
        /// 排序类型
        /// </summary>
        [Property(Column = "sort_type")]
        public virtual int SortType
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
        /// 排序顺序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
    }
}

