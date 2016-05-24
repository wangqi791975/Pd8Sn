namespace Com.Panduo.Web.Models.SEO
{
    public class MetaInfoVo
    {
        /// <summary>
        /// 面包屑
        /// </summary>
        public virtual string Breadcrumb { get; set; }

        /// <summary>
        /// 类别别名
        /// </summary>
        public virtual string Alias { get; set; }

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

        /// <summary>
        /// 页面名称
        /// </summary>
        public virtual string PageName { get; set; }
    }
}