using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 汇率日志
    /// </summary>
    [Serializable]
    public class ExchangeRateLog
    {

        /// <summary>
        /// 汇率日志ID
        /// </summary>
        public virtual int LogId
        {
            get;
            set;
        }

        /// <summary>
        /// 币种编码
        /// </summary>
        public virtual string CurrencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public virtual int ModifiyId
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifiyTime
        {
            get;
            set;
        }

        /// <summary>
        /// 修改前汇率
        /// </summary>
        public virtual decimal OriginalRate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改后汇率
        /// </summary>
        public virtual decimal ModifiedRate
        {
            get;
            set;
        }



    }
}
