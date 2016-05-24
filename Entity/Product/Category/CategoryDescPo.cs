
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Category
{
    /// <summary>
    ///描述：类别名称表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:37:55
    /// </summary>
    [Class(Table = "t_category_desc", Lazy = false, NameType = typeof(CategoryDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CategoryDescPo
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
        /// <summary>
        /// 描述
        /// </summary>
        [Property(Column = "description")]
        public virtual string Description
        {
            get;
            set;
        }
        
    }
}

