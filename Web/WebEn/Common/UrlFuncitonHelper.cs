using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// URL参数复制类
    /// </summary>
    public static class UrlFuncitonHelper
    { 
        /// <summary>
        /// 获取支付返回主机头
        /// </summary>
        /// <returns></returns>
        public static string GetPaymentReturnHost()
        { 
            if (!ConfigHelper.PaymentReturnHost.IsNullOrEmpty())
            {
                return Regex.Replace(ConfigHelper.PaymentReturnHost, "^(http[s]?://)", UrlPathHelper.GetScheme()).TrimEnd('/');
            }

            return GetCurrentHost().TrimEnd('/');
        }

        /// <summary>
        /// 获取当前主机头（处理好当前环境的https)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentHost()
        {
            return GetHost(true);
        } 

        /// <summary>
        /// 获取主机
        /// </summary>
        /// <param name="includeVisualPath">是否包含虚拟目录</param>
        /// <returns></returns>
        public static string GetHost(bool includeVisualPath)
        {
            return UrlPathHelper.GetHost(includeVisualPath);
        } 

        /// <summary>
        /// 获取当前的基准Url地址(不包括参数)
        /// </summary>
        /// <returns></returns>
        public static string GetBaseUrl()
        {
            return UrlPathHelper.GetBaseUrl();
        }

        /// <summary>
        /// 设置当前请求URL的参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetParam(string paramName, string value)
        {
            var url = GetFristPageIndexUrl();
            url = url.SetParam(paramName, value);
            return url;
        }

        /// <summary>
        /// 设置当前请求URL的参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetParam(string paramName, int value)
        {
            var url = GetFristPageIndexUrl();
            url = url.SetParam(paramName, value.ToString(CultureInfo.InvariantCulture));
            return url;
        }
        /// <summary>
        /// 将当前URL剔除指定的查询参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string RemoveParam(string paramName)
        {
            var url = GetFristPageIndexUrl();
            url = url.RemoveParam(paramName);
            return url;
        }

        /// <summary>
        /// 获取当前环境的原始地址URL
        /// </summary>
        /// <returns></returns>
        public static string GetFullUrl()
        {
            return UrlPathHelper.GetRawUrl();
        }

        /// <summary>
        /// 默认第一页
        /// </summary>
        /// <returns></returns>
        public static string GetFristPageIndexUrl()
        {
            var request = HttpContext.Current.Request;
            var url = request.Url.AbsoluteUri;
            var urlHelper = new UrlHelper(request.RequestContext);
            if (urlHelper.RequestContext.RouteData.Values.ContainsKey(UrlParameterKey.Page))
            {
                var routeDate = new RouteValueDictionary(urlHelper.RequestContext.RouteData.Values);
                var pageIndex = routeDate.TryGetValue(UrlParameterKey.Page);
                var currentPageIndex = pageIndex.IsNullOrEmpty() ? 1 : pageIndex.ParseTo(1);
                //routeDate.Set(PageIndexParmKey, _pageFormat);
                routeDate[UrlParameterKey.Page] = 1;
                url = urlHelper.Action(routeDate["action"].ToString(), routeDate);
                var baseUrl = UrlPathHelper.GetHost(true);
                url = string.Format("{0}{1}", baseUrl, url);
                url = url.SetParam(request.Url.AbsoluteUri.GetQueryStringMap());
            }
            else
            {
                url = url.SetParam(UrlParameterKey.Page, "1");
            }
            return url;
        }
        /// <summary>
        /// 默认第一页
        /// </summary>
        /// <returns></returns>
        public static string GetFristPageIndexUrl(this string url)
        {
            var request = HttpContext.Current.Request;
            var urlHelper = new UrlHelper(request.RequestContext);
            if (urlHelper.RequestContext.RouteData.Values.ContainsKey(UrlParameterKey.Page))
            {
                var routeDate = new RouteValueDictionary(urlHelper.RequestContext.RouteData.Values);
                var pageIndex = routeDate.TryGetValue(UrlParameterKey.Page);
                var currentPageIndex = pageIndex.IsNullOrEmpty() ? 1 : pageIndex.ParseTo(1);
                //routeDate.Set(PageIndexParmKey, _pageFormat);
                routeDate[UrlParameterKey.Page] = 1;
                url = urlHelper.Action(routeDate["action"].ToString(), routeDate);
                var baseUrl = UrlPathHelper.GetHost(true);
                url = string.Format("{0}{1}", baseUrl, url);
                url = url.SetParam(request.Url.AbsoluteUri.GetQueryStringMap());
            }
            else
            {
                url = url.SetParam(UrlParameterKey.Page, "1");
            }
            return url;
        }

        #region 属性值和属性值组
        /// <summary>
        /// 正则表达式格式串
        /// </summary>
        private static readonly string PropertyParamRegex = @"&?[{0}](?<paramName>\d+)\s*=\s*(?<paramValue>\d+)\s*";

        /// <summary>
        /// 将URL中涉及属性值和属性值组部分删除
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RemovePropertyValueAndGroup(string url)
        {
            var newUrl =  string.IsNullOrEmpty(url) ? string.Empty : Regex.Replace(url, string.Format(PropertyParamRegex, UrlParameterKey.PropertyValuePrefix + UrlParameterKey.PropertyValueGroupPrefix), string.Empty, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            return newUrl.Replace("?&", "?");
        }

        /// <summary>
        /// 将URL中涉及属性值部分删除
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RemovePropertyValue(string url)
        {
            var newUrl = string.IsNullOrEmpty(url) ? string.Empty : Regex.Replace(url, string.Format(PropertyParamRegex, UrlParameterKey.PropertyValuePrefix), string.Empty, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            return newUrl.Replace("?&", "?");
        }

        /// <summary>
        /// 将URL中涉及属性值组部分删除
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RemovePropertyValueGroup(string url)
        {
            var newUrl = string.IsNullOrEmpty(url) ? string.Empty : Regex.Replace(url, string.Format(PropertyParamRegex, UrlParameterKey.PropertyValueGroupPrefix), string.Empty, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            return newUrl.Replace("?&", "?");
        }

        /// <summary>
        /// 获取URL中的属性值参数列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IList<int> GetPropertyValueList(string url)
        {
            return UrlPathHelper.GetQueryStringParamList<int>(url, UrlParameterKey.PropertyValuePrefix);
        }

        /// <summary>
        /// 获取URL中的属性值组参数列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IList<int> GetPropertyValueGroupList(string url)
        {
            return UrlPathHelper.GetQueryStringParamList<int>(url, UrlParameterKey.PropertyValueGroupPrefix);
        }

        /// <summary>
        /// 组装属性值查询字符串的URL
        /// </summary>
        /// <param name="url">原始URL（剔除了属性值查询参数部分的）</param>
        /// <param name="propertyValudId">本次的属性值ID</param>
        /// <param name="selectedIds">之前已经选中的属性值ID</param>
        /// <param name="isAdd">true:本次是新增，false：本次是删除</param>
        /// <returns>完整的URL串</returns>
        public static string GetPropertyValueUrl(string url, int propertyValudId, IList<int> selectedIds, bool isAdd)
        {
            //url = url.GetFristPageIndexUrl();
            return GetSpecialUrl(url, propertyValudId, selectedIds, isAdd, UrlParameterKey.PropertyValuePrefix);
        }

        /// <summary>
        /// 组装属性值组查询字符串的URL
        /// </summary>
        /// <param name="url">原始URL（剔除了属性值组查询参数部分的）</param>
        /// <param name="propertyValudGroupId">本次的属性值组ID</param>
        /// <param name="selectedIds">之前已经选中的属性值组ID</param>
        /// <param name="isAdd">true:本次是新增，false：本次是删除</param>
        /// <returns>完整的URL串</returns>
        public static string GetPropertyValueGroupUrl(string url, int propertyValudGroupId, IList<int> selectedIds, bool isAdd)
        {
            //url = url.GetFristPageIndexUrl();
            return GetSpecialUrl(url, propertyValudGroupId, selectedIds, isAdd, UrlParameterKey.PropertyValueGroupPrefix);
        }
        /// <summary>
        /// 组装特殊的查询字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="selectedIds"></param>
        /// <param name="isAdd"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static string GetSpecialUrl(string url, int id, IList<int> selectedIds, bool isAdd, string prefix)
        {
            url = url.IsNullOrEmpty() ? string.Empty : url.TrimEnd('?', '&');
            url = url.Replace("?&", "?");
            selectedIds = selectedIds ?? new List<int>();
            if (isAdd)
            {
                selectedIds.Add(id);
            }
            else
            {
                selectedIds = selectedIds.Where(c => c != id).ToList();
            }

            var queryString = selectedIds.GetQueryString(prefix);

            if (queryString.IsNullOrEmpty())
            {
                return url;
            }

            return string.Format("{0}{1}{2}", url, url.Contains("?") ? "&" : "?", queryString);
        }

        #endregion
    }
}