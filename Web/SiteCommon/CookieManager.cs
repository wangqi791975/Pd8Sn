using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Common
{
    public partial class CookieManager
    {
        #region 公共设置
        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expireDays">cookie可用天数</param>
        public static void SetCookie(string key, string value, double expireDays)
        {
            SetCookie(key, value, DateTime.Now.AddDays(expireDays));
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param> 
        /// <param name="expireDateTime">失效绝对时间</param>
        public static void SetCookie(string key, string value, DateTime? expireDateTime = null)
        {
            if (expireDateTime.HasValue)
            {

                SetCookie(key, value, expireDateTime.Value);
            }
            else
            {

                SetCookie(key, value, DateTime.MaxValue);
            }
        }


        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param> 
        /// <param name="expireDateTime">失效绝对时间</param>
        public static void SetCookie(string key, string value, DateTime expireDateTime)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                if (!ConfigManager.CrossDomainCookie.IsNullOrEmpty())
                { 
                    cookie.Domain = ConfigManager.CrossDomainCookie;
                }
                cookie.Values.Add(key, value);
            }
            else
            {
                if (cookie[key] != null && cookie[key].IndexOf(value, StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    cookie.Values.Remove(key);
                    cookie.Values.Add(key, value);
                }
            }

            cookie.Expires = expireDateTime;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        /// <summary>
        /// 强制清除cookie
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveCookie(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                HttpContext.Current.Request.Cookies.Remove(key);
            }
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];

            return cookie != null ? cookie[key] : string.Empty;
        } 
        #endregion 
    }
}