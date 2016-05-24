using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    public static class UrlFuncitonHelper
    {
        /// <summary>
        /// 设置当前请求URL的参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetParam(string paramName, string value)
        {
           return UrlPathHelper.SetParam(paramName, value);
        }

        /// <summary>
        /// 将当前URL剔除指定的查询参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string RemoveParam(string paramName)
        {
            return UrlPathHelper.RemoveParam(paramName);
        }
    }
}