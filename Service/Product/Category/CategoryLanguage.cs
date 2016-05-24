using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Category
{
    /// <summary>
    /// 类别多语种
    /// </summary>
    [Serializable]
    public class CategoryLanguage
    {
        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int CategoryId
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
        /// 类别英文名称(用于重写，所有语种都要用到)
        /// </summary>
        public virtual string CategoryEnglishName
        {
            get;
            set;
        }

        /// <summary>
        /// 类别多语种名称
        /// </summary>
        public virtual string CategoryLanguageName
        {
            get;
            set;
        }

        /// <summary>
        /// 类别多语种描述
        /// </summary>
        public virtual string CategoryLanguageDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 样式名称
        /// </summary>
        public virtual string CssName
        {
            get;
            set;
        }
    }


}
