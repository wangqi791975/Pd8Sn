
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SEO
{
    /// <summary>
    ///描述：meta-首页表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-09 13:58:07
    /// </summary>
    [Class(Table = "t_meta_home", Lazy = false, NameType = typeof(MetaHomePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MetaHomePo
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
    }
}

