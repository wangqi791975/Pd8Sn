
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：搜索关键词表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:37
    /// </summary>
    [Class(Table = "t_search_keyword", Lazy = false, NameType = typeof(SearchKeywordPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class SearchKeywordPo
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
        /// 10搜索框内，20搜索框下，30底部
        /// </summary>
        [Property(Column = "`type`")]
        public virtual int Type
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
        /// 名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
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
        /// <summary>
        /// 排序顺序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int? SortOrder
        {
            get;
            set;
        }
    }
}

