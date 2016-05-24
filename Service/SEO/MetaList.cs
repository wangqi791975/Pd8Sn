using System;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// Meta-列表
    /// </summary>
    [Serializable]
    public class MetaList
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 类别id
        /// </summary>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public virtual MetaListPageType PageType { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 类别别名
        /// </summary>
        public virtual string Alias { get; set; }

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

        /// <summary>
        /// Title筛选
        /// </summary>
        public virtual string TitlePro { get; set; }

        /// <summary>
        /// Keywords筛选
        /// </summary>
        public virtual string KeywordsPro { get; set; }

        /// <summary>
        /// Description筛选
        /// </summary>
        public virtual string DescriptionPro { get; set; }

    }

    public enum MetaListPageType
    {
        /// <summary>
        /// Home区
        /// </summary>
        Home = 10,

        /// <summary>
        /// 新品区
        /// </summary>
        New = 20,

        /// <summary>
        /// Mix区
        /// </summary>
        Mix = 30,

        /// <summary>
        /// 促销区
        /// </summary>
        Promotion = 40,

        /// <summary>
        /// 商品专区
        /// </summary>
        Area = 50,

        /// <summary>
        /// 全站搜索区
        /// </summary>
        Search = 60
    }
}

