using System;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// meta-首页
    /// </summary>
    [Serializable]
    public class MetaArea
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 专区id
        /// </summary>
        public virtual int AreaId { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        public virtual string PageName { get; set; }

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
}

