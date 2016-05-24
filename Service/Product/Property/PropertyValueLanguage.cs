using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 属性值多语种
    /// </summary>
    [Serializable]
    public class PropertyValueLanguage
    {
        /// <summary>
        /// 属性值ID
        /// </summary>
        public virtual int PropertyValueId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性值多语种名称
        /// </summary>
        public virtual string PropertyValueName
        {
            get;
            set;
        }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

    }
}
