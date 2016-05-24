using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 省(州,郡)多语种
    /// </summary>
    [Serializable]
    public class ProvinceLanguage
    {
        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 省ID
        /// </summary>
        public virtual int Province
        {
            get;
            set;
        }
        /// <summary>
        /// 对应语种省名称
        /// </summary>
        public virtual string ProvinceName
        {
            get;
            set;
        }

    }
}
