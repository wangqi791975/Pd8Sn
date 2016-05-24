using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 属性多语种
    /// </summary>
    [Serializable]
    public class PropertyLanguage
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public virtual int PropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性多语种名称
        /// </summary>
        public virtual string PropertyName
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

    }

}
