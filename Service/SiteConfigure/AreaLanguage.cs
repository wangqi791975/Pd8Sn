using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{

    /// <summary>
    /// 区域多语种
    /// </summary>
    [Serializable]
    public class AreaLanguage
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public virtual int AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        public virtual string AreaName
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
