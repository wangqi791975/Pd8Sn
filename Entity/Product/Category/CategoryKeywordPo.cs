
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Category
{
    /// <summary>
    ///描述：类别热搜词表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:37:57
    /// </summary>
    [Class(Table = "t_category_keyword", Lazy = false, NameType = typeof(CategoryKeywordPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CategoryKeywordPo
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
        /// 语言别
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 关键词
        /// </summary>
        [Property(Column = "keyword")]
        public virtual string Keyword
        {
            get;
            set;
        }
        /// <summary>
        /// 链接
        /// </summary>
        [Property(Column = "link")]
        public virtual string Link
        {
            get;
            set;
        }
        
         ///<summary>
         ///排序
         ///</summary>
        [Property(Column = "sort_index")]
        public virtual string SortIndex
        {
            get;
            set;
        }
    }
}

