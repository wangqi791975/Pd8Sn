using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// Https辅助方法
    /// </summary>
    public static class SslHelper
    {
        /// <summary>
        /// 指定方法是否需要Https加密
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool IsRequireHttps(string controllerName, string actionName)
        {
            return CacheHelper.SslConfigMap.Any(c =>c.Key.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase) && c.Value != null &&c.Value["Add"].Any(d => d.Equals("*") || d.Equals(actionName, StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// 指定方法是否需要Http解密
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool IsRequireHttp(string controllerName, string actionName)
        {
            return CacheHelper.SslConfigMap.Any(c => c.Key.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase) && c.Value != null && c.Value["Remove"].Any(d => d.Equals("*") || d.Equals(actionName, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}