
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SEO
{
    /// <summary>
    ///描述：热搜关键词主题表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 09:32:14
    /// </summary>
    [Class(Table = "t_top_keyword_subject", Lazy = false, NameType = typeof(TopKeywordSubjectPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class TopKeywordSubjectPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "TopKeywordSubjectId", Column = "top_keyword_subject_id")]
        [Generator(2, Class = "native")]
        public virtual int TopKeywordSubjectId
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
    }
}

