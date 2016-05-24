using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Configuration;

namespace System.Web.Mvc
{

    /// <summary>
    /// 雅虎YUI辅助
    /// </summary>
    public static class YuiHtmlHelper
    {
        /// <summary>
        /// 合并的文件名称与要合并的文件列表
        /// </summary>
        public static IDictionary<string, IEnumerable<string>> ResourcesMap = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        /// 处理动态Css对应的Action名称
        /// </summary>
        public static readonly string ActionNameCss = "Css";
        /// <summary>
        /// 处理动态Js对应的Action名称
        /// </summary>
        public static readonly string ActionNameJs = "Js";
        /// <summary>
        /// 程序版本号
        /// </summary>
        public static readonly string AppVersion = WebConfigurationManager.AppSettings["AppVersion"] ?? "1.0.0";

        /// <summary>
        /// 资源是否存在
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static bool ExistResource(string resourceName)
        {
            return ResourcesMap.ContainsKey(resourceName);
        }

        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static IEnumerable<string> TryGetResources(string resourceName)
        {
            if (ExistResource(resourceName))
            {
                return ResourcesMap[resourceName];
            }

            return new List<string>();
        }

        /// <summary>
        /// 合并的Css Html标签,href组成格式为:文件名_程序版本号_文件版本号.css
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFileName">合并后的css文件名</param>
        /// <param name="joinFilePaths">要合并的css文件用指定符号拼接起来的字符串</param>
        /// <param name="split">分隔符</param>
        /// <param name="fileVersion">合并的Css文件版本号</param>
        /// <returns></returns>
        public static MvcHtmlString CssFor(this HtmlHelper htmlHelper, string cssFileName, string joinFilePaths, string split = ",", string fileVersion = "1.0")
        {
            return htmlHelper.CssFor(cssFileName, Regex.Split(joinFilePaths, split, RegexOptions.IgnorePatternWhitespace), fileVersion);
        }

        /// <summary>
        /// 合并的Css Html标签,href组成格式为:文件名_程序版本号_文件版本号.css
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFileName">合并后的css文件名</param>
        /// <param name="combineCssFilePaths">要合并的css文件路径</param>
        /// <param name="fileVersion">合并的Css文件版本号</param>
        /// <returns>返回合并的css link文件</returns>
        public static MvcHtmlString CssFor(this HtmlHelper htmlHelper, string cssFileName, IEnumerable<string> combineCssFilePaths, string fileVersion = "1.0")
        {
            if (string.IsNullOrEmpty(cssFileName) || combineCssFilePaths == null || combineCssFilePaths.Count() == 0)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            var key = string.Format("{0}_{1}_{2}.css", Regex.Replace(cssFileName,".css",string.Empty,RegexOptions.IgnorePatternWhitespace), AppVersion, fileVersion);

            if (ResourcesMap.ContainsKey(key))
            {
                ResourcesMap[key] = combineCssFilePaths;
            }
            else
            {
                ResourcesMap.Add(key, combineCssFilePaths);
            }

            var tagBuilder = new TagBuilder("link");
            tagBuilder.MergeAttribute("rel", "stylesheet");
            tagBuilder.MergeAttribute("type", "text/css");
            tagBuilder.MergeAttribute("href", string.Format("{0}{1}/{2}", new UrlHelper(HttpContext.Current.Request.RequestContext).Content("~"), ActionNameCss, key));

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// 合并的Js Html标签,src组成格式为:文件名_程序版本号_文件版本号.js
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="jsFileName">合并后的js文件名</param>
        /// <param name="joinFilePaths">要合并的css文件用指定符号拼接起来的字符串</param>
        /// <param name="split">分隔符</param>
        /// <param name="fileVersion">合并的Js文件版本号</param>
        /// <returns></returns>
        public static MvcHtmlString JsFor(this HtmlHelper htmlHelper, string jsFileName, string joinFilePaths, string split = ",", string fileVersion = "1.0")
        {
            return htmlHelper.JsFor(jsFileName, Regex.Split(joinFilePaths, split, RegexOptions.IgnorePatternWhitespace), fileVersion);
        }

        /// <summary>
        /// 合并的Js Html标签,src组成格式为:文件名_程序版本号_文件版本号.js
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="jsFileName">合并后的js文件名</param>
        /// <param name="combineJsFilePaths">要合并的js文件路径</param>
        /// <param name="fileVersion">合并的Js文件版本号</param>
        /// <returns>返回合并的js script文件</returns>
        public static MvcHtmlString JsFor(this HtmlHelper htmlHelper, string jsFileName, IEnumerable<string> combineJsFilePaths, string fileVersion = "1.0")
        {
            if (string.IsNullOrEmpty(jsFileName) || combineJsFilePaths == null || combineJsFilePaths.Count()==0)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            var key = string.Format("{0}_{1}_{2}.js", Regex.Replace(jsFileName, ".js", string.Empty, RegexOptions.IgnorePatternWhitespace), AppVersion, fileVersion);

            if (ResourcesMap.ContainsKey(key))
            {
                ResourcesMap[key] = combineJsFilePaths;
            }
            else
            {
                ResourcesMap.Add(key, combineJsFilePaths);
            }

            var tagBuilder = new TagBuilder("script"); 
            tagBuilder.MergeAttribute("type" , "text/javascript");
            tagBuilder.MergeAttribute("src" , string.Format("{0}{1}/{2}", new UrlHelper(HttpContext.Current.Request.RequestContext).Content("~"), ActionNameJs, key));

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}