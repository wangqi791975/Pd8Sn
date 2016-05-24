using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    ///  区域
    /// </summary>
    [Serializable]
    public class Area
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


    }
}
