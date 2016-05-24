using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{

    /// <summary>
    /// 省(州,郡)
    /// </summary>
    [Serializable]
    public class Province
    {
        /// <summary>
        /// 省名称
        /// </summary>
        public virtual string ProvinceName
        {
            get;
            set;
        }
        /// <summary>
        /// 省Id
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }

        /// <summary>
        /// 省编码
        /// </summary>
        public virtual string ProvinceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 国家ID
        /// </summary>
        public virtual int CountryId
        {
            get;
            set;
        }

    }
}
