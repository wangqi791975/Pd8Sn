using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Web.PaymentCommon.PayInfo;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.OceanPayment
{
    [Serializable]
    public class OceanPaymentResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual bool IsValid { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public virtual OceanPaymentInfo OceanPaymentInfo { get; set; }
    }
}