using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{

    /// <summary>
    /// 国家多语种
    /// </summary>
       [Serializable]
    public class CountryLanguage
    {
        /// <summary>
        /// 国家ID
        /// </summary>
        public virtual int CountryId
        {
            get;
            set;
        }

        /// <summary>
        /// 国家名称
        /// </summary>
        public virtual string CountryName
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
