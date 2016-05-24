using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 页面辅助
    /// </summary>
    public static partial class PageManager
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            var ip = string.IsNullOrEmpty(CommonConfigHelper.CdnClintIpKey) ? string.Empty : HttpContext.Current.Request.ServerVariables[CommonConfigHelper.CdnClintIpKey];//先尝试获取CDN加速以后传递过来的原始客户IP

            if (ip.IsNullOrEmpty())
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {

                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IsNullOrEmpty())
                    {

                        if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        {
                            ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                        }
                        else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                        {
                            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                        }
                        else
                        {
                            ip = CommonConfigHelper.CompanyIp;//默认本公司IP
                        }
                    }
                    else
                    {
                        ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',', ';', '|').FirstOrDefault();
                    }

                }
                else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    ip = CommonConfigHelper.CompanyIp;//默认本公司IP
                }
            }

            return ip ?? string.Empty;
        }

        /// <summary>
        /// IP转换成整数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long ConvertIpToInt(string ip)
        {
            var separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }

        /// <summary>
        /// 获取当前网站跟路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, string.Empty);
        }

        /// <summary>
        /// 获取当前Url的分页参数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static string GetUrl(int pageIndex, string pageUrl = null)
        {
            pageUrl = string.IsNullOrEmpty(pageUrl) ? HttpContext.Current.Request.Url.AbsoluteUri : pageUrl;
            return UrlPathHelper.SetParam(pageUrl, CommonConst.PageIndex, pageIndex.ToString());
        }

        /// <summary>
        /// 转化为html格式的数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSafeHtml(this string value)
        {
            return value.ToHtml();
        }

        /// <summary>
        /// 转化为Url格式的数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSafeUrl(this string value)
        {
            return UrlPathHelper.FilterUrlParam(value, replaceStr: "-").ToLowerInvariant();
        }

    }
}
