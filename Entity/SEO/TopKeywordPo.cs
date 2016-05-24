
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SEO
{
    /// <summary>
    ///描述：热搜关键词表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 09:32:13
    /// </summary>
    [Class(Table = "t_top_keyword", Lazy = false, NameType = typeof(TopKeywordPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class TopKeywordPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "TopKeywordId", Column = "top_keyword_id")]
        [Generator(2, Class = "native")]
        public virtual int TopKeywordId
        {
            get;
            set;
        }
        /// <summary>
        /// 所属主题id
        /// </summary>
        [Property(Column = "top_keyword_subject_id")]
        public virtual int TopKeywordSubjectId
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

