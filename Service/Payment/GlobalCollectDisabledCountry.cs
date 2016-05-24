using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment
{
    /// <summary>
    /// GC屏蔽国家对象
    /// </summary>
    [Serializable]
    public class GlobalCollectDisabledCountry
    {

        /// <summary>
        /// 国家ID
        /// </summary>
        public virtual int CountryId { get; set; }

        /// <summary>
        ///国家名称
        /// </summary>
        public virtual string CountryName { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int AdminId { get; set; }

        /// <summary>
        /// 管理员Email
        /// </summary>
        public virtual string AccountEmail { get; set; }
    }
}
