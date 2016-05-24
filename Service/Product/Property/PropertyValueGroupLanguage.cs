using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 属性值组多语种
    /// </summary>
    [Serializable]
    public class PropertyValueGroupLanguage
    {
        /// <summary>
        /// 属性值组ID
        /// </summary>
        public virtual int GroupId
        {
            get;
            set;
        }


        /// <summary>
        /// 属性值组多语种名称
        /// </summary>
        public virtual string PropertyValueGroupName
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
