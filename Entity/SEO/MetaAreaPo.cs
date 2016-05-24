
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SEO
{
    /// <summary>
    ///描述：meta-专区表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-09 13:58:07
    /// </summary>
    [Class(Table = "t_meta_area", Lazy = false, NameType = typeof(MetaAreaPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MetaAreaPo
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
        /// 专区id
        /// </summary>
        [Property(Column = "area_id")]
        public virtual int AreaId
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
        /// 页面名称
        /// </summary>
        [Property(Column = "page_name")]
        public virtual string PageName
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

