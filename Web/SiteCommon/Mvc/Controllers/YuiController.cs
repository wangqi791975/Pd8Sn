using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Web.Mvc;
using Com.Panduo.Web.Common.Mvc.Filter;
using Com.Panduo.Web.Common.Mvc.Helper;
using Yahoo.Yui.Compressor;

namespace Com.Panduo.Web.Common.Mvc.Controllers
{
    /// <summary>
    /// 动态Css/Js处理控制器
    /// </summary>
    public class YuiController : Controller
    {
        /// <summary>
        /// 动态获取Css文件
        /// <para>1.使用Yahoo.Yui.Compressor.CssCompressor压缩要合并的Css文件</para>
        /// <para>2.根据浏览器接受的压缩格式进行压缩过滤后输出</para>
        /// <para>3.根据缓存策略通知客户端、服务端、代理服务器进行缓存</para> 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        [CompressFilter(Order = 1)]
        [OutputCache(CacheProfile = "CssCacheProfile", Order = 2)]
        public ActionResult Css(string resourceName)
        {
            var value = GetResource(resourceName, true);
            if (!string.IsNullOrEmpty(value) && CommonConfigHelper.IsCompressOn)
            {
                var compressor = new CssCompressor
                {
                    CompressionType = CompressionType.Standard,
                    RemoveComments = true,
                    LineBreakPosition = int.MaxValue
                };

                value = compressor.Compress(value);
            }

            return Content(value, "text/css");
        }

        /// <summary>
        /// 动态获取Js文件
        /// <para>1.使用Yahoo.Yui.Compressor.JavaScriptCompressor压缩要合并的Js文件</para>
        /// <para>2.根据浏览器接受的压缩格式进行压缩过滤后输出</para>
        /// <para>3.根据缓存策略通知客户端、服务端、代理服务器进行缓存</para>  
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        [CompressFilter(Order = 1)]
        [OutputCache(CacheProfile = "JsCacheProfile", Order = 2)]
        public ActionResult Js(string resourceName)
        {
            var value = GetResource(resourceName, false);
            if (!string.IsNullOrEmpty(value) && CommonConfigHelper.IsCompressOn)
            {
                var compressor = new JavaScriptCompressor
                {
                    CompressionType = CompressionType.Standard,
                    Encoding = Encoding.UTF8,
                    ThreadCulture = System.Globalization.CultureInfo.CurrentCulture,
                    IgnoreEval = false,
                    DisableOptimizations = false,
                    LineBreakPosition = int.MaxValue,
                    ObfuscateJavascript = true,
                    PreserveAllSemicolons = true
                };

                value = compressor.Compress(value);
            }

            return JavaScript(value);
        }

        /// <summary>
        /// 读取资源并合并资源
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="isCss">是否Css资源</param>
        /// <returns></returns>
        private string GetResource(string resourceName, bool isCss)
        {
            //尝试从缓存池读取
            if ((isCss && CommonConfigHelper.IsCssCacheEnable) || (!isCss && CommonConfigHelper.IsJsCacheEnable))
            {
                var cacheValue = CacheManager.Instance.GetCache(resourceName);
                if (cacheValue != null)
                {
                    return cacheValue.ToString();
                }
            }

            //如果没有缓存则读取并合并文件
            var resources = YuiHtmlHelper.TryGetResources(resourceName);
            if (resources != null && resources.Count() > 0)
            {
                var files = resources.Select(c => Server.MapPath(c.Trim())).Select(c => System.IO.File.Exists(c) ? System.IO.File.ReadAllText(c) : string.Empty).Where(c => !string.IsNullOrEmpty(c)).ToList();

                //将合并后的文件字符串加入缓存
                CacheManager.Instance.SetCache(resourceName, files.Count > 0 ? files.Aggregate((a, b) => a + " " + b) : string.Empty);
            }

            return CacheManager.Instance.GetCache(resourceName) as string;
        }
    }
}
