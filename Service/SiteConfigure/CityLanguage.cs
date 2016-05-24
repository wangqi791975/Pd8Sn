using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 城市多语种
    /// </summary>
    [Serializable]
    public class CityLanguage
    {
        /// <summary>
        ///城市ID
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }

        /// <summary>
        /// 城市名称
        /// </summary>
        public virtual string CityName
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
