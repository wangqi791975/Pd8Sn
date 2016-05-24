using System;

namespace Com.Panduo.Service.Product.Category
{
    /// <summary>
    /// 类别关键词
    /// </summary>
    [Serializable]
    public class CategoryKeyword
    {
        /// <summary>
        /// 关键词Id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public virtual string Keyword
        {
            get;
            set;
        }

        /// <summary>
        /// URL
        /// </summary>
        public virtual string Url
        {
            get;
            set;
        }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual string DiplayOrder
        {
            get;
            set;
        }
    }
}
