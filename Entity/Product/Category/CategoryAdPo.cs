
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Category
{
    /// <summary>
    ///描述：类别广告图 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:37:53
    /// </summary>
    [Class(Table = "t_category_ad", Lazy = false, NameType = typeof(CategoryAdPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CategoryAdPo
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
        /// 广告图
        /// </summary>
        [Property(Column = "ad_image")]
        public virtual string AdImage
        {
            get;
            set;
        }
        /// <summary>
        /// 广告词
        /// </summary>
        [Property(Column = "ad_word")]
        public virtual string AdWord
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
        /// 营销标题
        /// </summary>
        [Property(Column = "marketing_title")]
        public virtual string MarketingTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 产品详情页营销区域
        /// </summary>
        [Property(Column = "marketing_text")]
        public virtual string MarketingText
        {
            get;
            set;
        }
    }
}

