using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    
    /// <summary>
    /// 白名单
    /// </summary>
    [Serializable]
    public class WhiteList
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public virtual int CreateId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? ModifyTime { get; set; }

    }
}
