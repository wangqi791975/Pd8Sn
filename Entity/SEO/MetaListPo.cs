
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SEO
{
    /// <summary>
    ///描述：meta-列表表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-09 13:58:08
    /// </summary>
    [Class(Table = "t_meta_list", Lazy = false, NameType = typeof(MetaListPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MetaListPo
    {
        /// <summary>
        /// 自增Id
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
        /// 页面类型
        /// </summary>
        [Property(Column = "page_type")]
        public virtual int PageType
        {
            get;
            set;
        }
        /// <summary>
        /// 语种Id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 类别别名
        /// </summary>
        [Property(Column = "alias")]
        public virtual string Alias
        {
            get;
            set;
        }
        /// <summary>
        /// 面包屑
        /// </summary>
        [Property(Column = "breadcrumb")]
        public virtual string Breadcrumb
        {
            get;
            set;
        }
        /// <summary>
        /// Title
        /// </summary>
        [Property(Column = "title")]
        public virtual string Title
        {
            get;
            set;
        }
        /// <summary>
        /// Keywords
        /// </summary>
        [Property(Column = "keywords")]
        public virtual string Keywords
        {
            get;
            set;
        }
        /// <summary>
        /// Description
        /// </summary>
        [Property(Column = "description")]
        public virtual string Description
        {
            get;
            set;
        }
        /// <summary>
        /// Title筛选
        /// </summary>
        [Property(Column = "title_pro")]
        public virtual string TitlePro
        {
            get;
            set;
        }
        /// <summary>
        /// Keywords筛选
        /// </summary>
        [Property(Column = "keywords_pro")]
        public virtual string KeywordsPro
        {
            get;
            set;
        }
        /// <summary>
        /// Description筛选
        /// </summary>
        [Property(Column = "description_pro")]
        public virtual string DescriptionPro
        {
            get;
            set;
        }
    }
}

