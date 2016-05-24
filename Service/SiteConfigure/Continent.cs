using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{

    /// <summary>
    /// 洲
    /// </summary>
   [Serializable]
    public class Continent
    {

        /// <summary>
        /// 洲中文名称
        /// </summary>
        public virtual string ContinentName { get; set; }

        /// <summary>
        /// 洲ID
        /// </summary>
        public virtual int ContinentId { get; set; }

    }
}
