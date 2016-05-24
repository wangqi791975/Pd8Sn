using System;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// meta-首页
    /// </summary>
    [Serializable]
    public class MetaHome
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public virtual MetaHomePageType PageType { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 面包屑
        /// </summary>
        public virtual string Breadcrumb { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Keywords
        /// </summary>
        public virtual string Keywords { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }
    }

    public enum MetaHomePageType
    {
        /// <summary>
        /// 网站首页
        /// </summary>
        Home = 10,

        /// <summary>
        /// 新品首页
        /// </summary>
        NewIndex = 20,

        /// <summary>
        /// Mix首页
        /// </summary>
        MixIndex = 30,

        /// <summary>
        /// 促销首页
        /// </summary>
        PromotionIndex = 40,

        /// <summary>
        /// 产品详情页
        /// </summary>
        ProductDetail = 50
    }
}

