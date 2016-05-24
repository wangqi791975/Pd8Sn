using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Com.Panduo.Common
{
    /// <summary>
    /// Url路径辅助类
    /// </summary>
    public static class UrlPathHelper
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        private static readonly string ParamRegexFromat = @"&?{0}(?<paramName>\d+)\s*=\s*(?<paramValue>\d+)";

        /// <summary>
        /// 获取协议部分：比如http或者https
        /// </summary>
        /// <returns></returns>
        public static string GetScheme()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Scheme);
        }
        /// <summary>
        /// 获取主机
        /// </summary>
        /// <param name="includeVisualPath">是否包含虚拟目录</param>
        /// <returns></returns>
        public static string GetHost(bool includeVisualPath)
        {
            var rootPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            // 如果网站是位于虚拟目录之后，就将虚拟目录路径加到从项目根开始的路径前面去
            if (includeVisualPath && !HttpContext.Current.Request.ApplicationPath.IsNullOrEmpty() && HttpContext.Current.Request.ApplicationPath != "/")
            {
                rootPath = string.Format("{0}/{1}", rootPath, HttpContext.Current.Request.ApplicationPath);
            }

            return rootPath;
        }
        /// <summary>
        /// 获取当前的基准Url地址(不包括参数)
        /// </summary>
        /// <returns></returns>
        public static string GetBaseUrl()
        {
            var uri = HttpContext.Current.Request.Url;

            return GetBaseUrl(uri.AbsoluteUri);
        }

        /// <summary>
        /// 获取指定URL基准Url地址(不包括参数)
        /// </summary>
        /// <returns></returns>
        public static string GetBaseUrl(this string url)
        {
            var uri = new Uri(url);

            url = string.IsNullOrEmpty(uri.Query) ? uri.AbsoluteUri : uri.AbsoluteUri.Replace(uri.Query, string.Empty);

            return url;
        }

        /// <summary>
        /// 获取当前原始地址
        /// </summary>
        /// <returns></returns>
        public static string GetRawUrl()
        {
            var uri = HttpContext.Current.Request.Url;

            return GetRawUrl(uri.AbsoluteUri);
        }

        /// <summary>
        /// 获取指定URL原始地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetRawUrl(this string url)
        {
            var uri = new Uri(url);

            return uri.AbsoluteUri;
        }

        /// <summary>
        /// 拼接Url地址
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string CombileUrl(this string baseUrl, string relativeUrl)
        {
            return VirtualPathUtility.Combine(baseUrl, relativeUrl);
        }

        /// <summary>
        /// 转变为绝对地址
        /// </summary>
        /// <param name="vartualPath"></param>
        /// <returns></returns>
        public static string ToAbsolteUrl(this string vartualPath)
        {
            return VirtualPathUtility.ToAbsolute(vartualPath);
        }

        /// <summary>
        /// 确保虚拟路径以斜杠结尾
        /// </summary>
        /// <param name="vartualPath"></param>
        /// <returns></returns>
        public static string EnSureTrailingSlash(this string vartualPath)
        {
            return VirtualPathUtility.AppendTrailingSlash(vartualPath);
        }

        /// <summary>
        /// 获取指定URL的查询字符串
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection GetQueryString()
        {
            var uri = HttpContext.Current.Request.Url;

            return GetQueryString(uri.AbsoluteUri);
        }

        /// <summary>
        /// 获取当前请求URl的查询字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(this string url)
        {
            var uri = new Uri(url);

            return HttpUtility.ParseQueryString(uri.Query, encoding);
        }

        /// <summary>
        /// 获取指定URL的查询字符串字典
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetQueryStringMap()
        {
            var uri = HttpContext.Current.Request.Url;

            return GetQueryStringMap(uri.AbsoluteUri);
        }

        /// <summary>
        /// 将指定的字典转换为查询字符串
        /// </summary>
        /// <param name="urlParams"></param>
        /// <returns></returns>
        public static string GetQueryString(this IDictionary<string, string> urlParams)
        {
            if (urlParams.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return urlParams.Select(c => c.Key + "=" + HttpUtility.UrlEncode(c.Value)).Aggregate((a, b) => a + "&" + b);
        }

        /// <summary>
        /// 将指定的字典转换为查询字符串
        /// </summary>
        /// <param name="urlParams"></param>
        /// <returns></returns>
        public static string GetQueryString(this IList<KeyValuePair<string, string>> urlParams)
        {
            if (urlParams.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return urlParams.Select(c => c.Key + "=" + HttpUtility.UrlEncode(c.Value)).Aggregate((a, b) => a + "&" + b);
        }

        /// <summary>
        /// 将指定的字典转换为查询字符串,假设前缀为p,则为p1=xxx&amp;p2=xxx
        /// </summary>
        /// <param name="urlParams">子类型数据列表</param>
        /// <param name="paramPrefix">参数前缀</param>
        /// <returns></returns>
        public static string GetQueryString<T>(this ICollection<T> urlParams, string paramPrefix) where T : struct
        {
            if (urlParams.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return GetQueryString(urlParams.ToList().ToDataIndexList().Select(c => new KeyValuePair<string, string>(string.Format("{0}{1}", paramPrefix, c.Key), c.Value.ToString())).ToList());
        }

        /// <summary>
        /// 解析字符串参数为列表信息，比如aaaa.html?p1=12&amp;p2=23&amp;p3=232得到一个包含12,23,232的list列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="paramPrefix"></param>
        /// <returns></returns>
        public static IList<T> GetQueryStringParamList<T>(this string queryString, string paramPrefix)
        {
            var list = new List<T>();

            var keywordMatches = Regex.Matches(queryString, string.Format(ParamRegexFromat, paramPrefix), RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            if (keywordMatches.Count > 0)
            {
                for (var i = 0; i < keywordMatches.Count; i++)
                {
                    if (keywordMatches[i].Groups["paramName"].Success && keywordMatches[i].Groups["paramValue"].Success)
                    {
                        list.Add(keywordMatches[i].Groups["paramValue"].Value.ParseTo<T>());
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取当前请求URl的查询字符串字典
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetQueryStringMap(this string url)
        {
            var map = new Dictionary<string, string>();
            var qs = GetQueryString(url);
            foreach (var key in qs.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    map.Add(key, HttpUtility.UrlDecode(qs.Get(key)));
                }
            }

            return map;
        }

        /// <summary>
        /// 设置当前请求URL的参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetParam(string paramName, string value)
        {
            var uri = HttpContext.Current.Request.Url;
            return SetParam(uri.AbsoluteUri, paramName, value);
        }

        /// <summary>
        /// 设置指定URL的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetParam(this string url, string paramName, string value)
        {
            return SetParam(url, new Dictionary<string, string> { { paramName, value } });
        }

        /// <summary>
        /// 设置指定URL的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramMap">Key为键,Value为值</param>
        /// <returns></returns>
        public static string SetParam(this string url, IDictionary<string, string> paramMap, List<string> ignoreParams = null)
        {
            var uri = new Uri(url);
            var qs = HttpUtility.ParseQueryString(uri.Query, encoding);
            foreach (var item in paramMap)
            {
                if (!ignoreParams.IsNullOrEmpty() && ignoreParams.Contains(item.Key))
                    continue;
                qs[item.Key] = HttpUtility.UrlEncode(item.Value, encoding);
            }

            if (uri.Query.IsNullOrEmpty())
            {
                url = string.Format("{0}{1}", uri.AbsoluteUri, qs.Count <= 0 ? string.Empty : "?" + qs);
            }
            else
            {
                url = uri.AbsoluteUri.Replace(uri.Query, qs.Count <= 0 ? string.Empty : "?" + qs);
            }

            return url;
        }

        /// <summary>
        /// 将当前URL剔除指定的查询参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string RemoveParam(string paramName)
        {
            var uri = HttpContext.Current.Request.Url;

            return RemoveParam(uri.AbsoluteUri, paramName);
        }

        /// <summary>
        /// 将当前URL剔除指定的查询参数
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public static string RemoveParam(IEnumerable<string> paramNames)
        {
            var uri = HttpContext.Current.Request.Url;

            return RemoveParam(uri.AbsoluteUri, paramNames);
        }

        /// <summary>
        /// 将指定URL剔除指定的查询参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string RemoveParam(this string url, string paramName)
        {
            var uri = new Uri(url);
            var qs = HttpUtility.ParseQueryString(uri.Query, encoding);

            foreach (var key in qs.AllKeys)
            {
                if (key.Equals(paramName, StringComparison.InvariantCultureIgnoreCase))
                {
                    qs.Remove(key);
                }
            }

            if (string.IsNullOrEmpty(uri.Query))
            {
                url = uri.AbsoluteUri + "?" + qs;
            }
            else
            {
                url = uri.AbsoluteUri.Replace(uri.Query, qs.Count <= 0 ? string.Empty : "?" + qs);
            }

            return url;
        }

        public static string RemoveParam(this string url, IEnumerable<string> paramNames)
        {
            var uri = new Uri(url);
            var qs = HttpUtility.ParseQueryString(uri.Query, encoding);

            foreach (var key in qs.AllKeys)
            {
                if (paramNames.Any(c => c.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    qs.Remove(key);
                }
            }

            if (string.IsNullOrEmpty(uri.Query))
            {
                url = uri.AbsoluteUri + "?" + qs;
            }
            else
            {
                url = uri.AbsoluteUri.Replace(uri.Query, qs.Count <= 0 ? string.Empty : "?" + qs);
            }

            return url;
        }

        /// <summary>
        /// 编码URl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string EncodeUrl(this string url)
        {
            var uri = new Uri(url);
            var qs = HttpUtility.ParseQueryString(uri.Query, encoding);

            foreach (var key in qs.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    qs.Set(key, HttpUtility.UrlEncode(qs.Get(key), encoding));
                }
            }

            if (string.IsNullOrEmpty(uri.Query))
            {
                url = uri.AbsoluteUri + (qs.Count <= 0 ? string.Empty : "?" + qs);
            }
            else
            {
                url = uri.AbsoluteUri.Replace(uri.Query, qs.Count <= 0 ? string.Empty : "?" + qs);
            }

            return url;
        }

        public static string DecodeUrl(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 转换不合法的url字符为短横线-
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToFriendlyUrl(string value)
        {
            return FilterUrlParam(value, replaceStr: "-").ToLowerInvariant();
        }

        /// <summary>
        /// 替换Url中的不合法字符
        /// </summary>
        /// <param name="url"></param>
        /// <param name="regex">要替换的正则表达式</param>
        /// <param name="replaceStr">替换的字符串</param>
        /// <returns></returns> 
        public static string FilterUrlParam(string url, string regex = "([/]{1,2})|([$=&*#%^<>{}()?×',.:\\s\\u005C\"]+)", string replaceStr = "")
        {
            url = Regex.Replace(string.IsNullOrEmpty(url) ? string.Empty : url.Trim(), regex, replaceStr, RegexOptions.IgnorePatternWhitespace);

            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 将文件格式的url转换为url本身的格式
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ToUrlFormat(this string filePath)
        {
            filePath = Regex.Replace(filePath, @"\:\\", "://");
            return Regex.Replace(filePath, @"\\", "/");
        }

        /// <summary>
        /// 转换为合法的url，结尾如果没有/则自动加上
        /// </summary>
        /// <param name="path">要转换的url</param>
        /// <returns></returns>
        public static string ToSafeUrlPath(this string path)
        {
            var newPath = path.IsNullOrEmpty() ? string.Empty : (path.EndsWith("/") ? path : path + "/");
            return newPath.ToUrlFormat();
        }

        /// <summary>
        /// 组合url路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ToFullUrlPath(this string path, string fileName)
        {
            return path.ToSafeUrlPath() + fileName;
        }
    }
}