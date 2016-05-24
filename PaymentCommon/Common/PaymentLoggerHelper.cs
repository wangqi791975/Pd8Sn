using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Com.Panduo.Web.PaymentCommon.Common
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum PaymentLoggerType
    {
        /// <summary>
        /// Payment日志
        /// </summary>
        Payment = 0,
        /// <summary>
        /// Paypal标准支付
        /// </summary>
        Paypal = 1,
        /// <summary>
        /// Paypal快速支付
        /// </summary>
        PaypalExpress=2,
        /// <summary>
        /// GC支付
        /// </summary>
        Gc=3,
        /// <summary>
        /// 钱海支付
        /// </summary>
        OceanPayment=4
    }

    /// <summary>
    /// 支付日志辅助类
    /// </summary>
    public class PaymentLoggerHelper
    {
        /// <summary>
        /// 获得日志Log
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static ILog GetLogger(PaymentLoggerType logger)
        {
            return LogManager.GetLogger(logger.ToString());
        }
    }
}
