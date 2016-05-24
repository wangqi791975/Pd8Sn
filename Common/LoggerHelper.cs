using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// 服务日志
        /// </summary>
        Service = 0,
        /// <summary>
        /// 后台日志
        /// </summary>
        Admin=1,
        /// <summary>
        /// 邮件日志
        /// </summary>
        SendMail=2,
        /// <summary>
        /// 异常日志
        /// </summary>
        Exception = 4,
        /// <summary>
        /// 前台日志
        /// </summary>
        Web=5,
        /// <summary>
        /// 支付日志
        /// </summary>
        Payment=6,
        /// <summary>
        /// Solr日志
        /// </summary>
        Solr = 7,
        /// <summary>
        /// IP拦截日志
        /// </summary>
        IpIntercept,
        /// <summary>
        /// Js脚本错误日志
        /// </summary>
        ScriptError
    }

    public class LoggerHelper
    {
        /// <summary>
        /// 获得日志Log
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static ILog GetLogger(LoggerType logger)
        {
            return LogManager.GetLogger(logger.ToString());
        }
    }
}
