using System;

namespace Com.Panduo.Service.Product.ProductArea
{
    /// <summary>
    /// 营销专区多语言
    /// </summary>
    [Serializable]
    public class ProductAreaLanguage
    {
        /// <summary>
        /// 专区Id
        /// </summary>
        public virtual int AreaId { set; get; }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId { set; get; }

        /// <summary>
        /// 专区名称
        /// </summary>
        public virtual string AreaName { set; get; }

        /// <summary>
        /// 专区首页 HTML
        /// </summary>
        public virtual string Home { set; get; }

    }
}
