using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    public static class CommonConfigHelper
    {
        static CommonConfigHelper()
        {
            Config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
        }

        private static readonly Configuration Config = null;
        
        /// <summary>
        /// 是否要清除Header的Server头
        /// </summary>
        public static bool IsRemoveServerReponseHeader { get { return bool.Parse(WebConfigurationManager.AppSettings["IsRemoveServerReponseHeader"] ?? "false"); } }
        
        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public static bool IsCompressOn { get { return bool.Parse(WebConfigurationManager.AppSettings["IsCompressOn"]??"false"); } }
       
        /// <summary>
        /// 是否启用Js服务端Cache缓存
        /// </summary>
        public static bool IsJsCacheEnable
        {
            get
            {
                var outputCacheSettings = (OutputCacheSettingsSection)Config.GetSection("system.web/caching/outputCacheSettings");
                return outputCacheSettings.OutputCacheProfiles["JsCacheProfile"] == null ? true : outputCacheSettings.OutputCacheProfiles["JsCacheProfile"].Enabled;
            }
        }

        /// <summary>
        /// 是否启用Css服务端Cache缓存
        /// </summary>
        public static bool IsCssCacheEnable
        {
            get
            { 
                var outputCacheSettings = (OutputCacheSettingsSection)Config.GetSection("system.web/caching/outputCacheSettings");
                return outputCacheSettings.OutputCacheProfiles["CssCacheProfile"] == null ? true : outputCacheSettings.OutputCacheProfiles["CssCacheProfile"].Enabled;
            }
        }   
        /// <summary>
        /// 最新多大的文件才压缩
        /// </summary>
        public static int CompressionMinSize { get { return 2048; } }

        /// <summary>
        /// 压缩文件的Mime类型
        /// </summary>
        public static IList<string> CompressableMimeTypes = Regex.Split(WebConfigurationManager.AppSettings["CompressableMimeType"] ?? string.Empty, ",", RegexOptions.IgnorePatternWhitespace).ToList();

        /// <summary>
        /// 404错误路由URL
        /// </summary>
        public static readonly string PageNotFoundRouteName = WebConfigurationManager.AppSettings["PageNotFoundRouteName"] ?? "PageNotFound";
        /// <summary>
        /// 404错误显示页面名称
        /// </summary>
        public static readonly string PageNotFoundViewName = WebConfigurationManager.AppSettings["PageNotFoundViewName"] ?? "PageNotFound";
        /// <summary>
        /// 500错误路由URL
        /// </summary>
        public static readonly string SystemErrorRouteName = WebConfigurationManager.AppSettings["SystemErrorRouteName"] ?? "SystemError";
        /// <summary>
        /// 500错误显示页面名称
        /// </summary>
        public static readonly string SystemErrorViewName = WebConfigurationManager.AppSettings["SystemErrorViewName"] ?? "SystemError";
        /// <summary>
        /// IP检查错误路由名称
        /// </summary>
        public static readonly string IpCheckErrorRouteName = WebConfigurationManager.AppSettings["IpCheckErrorRouteName"] ?? "IpCheckError";
        /// <summary>
        /// IP检查错误路由URL
        /// </summary>
        public static readonly string IpCheckErrorUrl = WebConfigurationManager.AppSettings["IpCheckErrorUrl"] ?? "IpCheckError";
        /// <summary>
        /// 使用CDN加速以后CDN节点通过节点传递过来的客户原始IPKey名称
        /// </summary>
        public static readonly string CdnClintIpKey = WebConfigurationManager.AppSettings["CdnClintIpKey"] ?? "";
        /// <summary>
        /// 公司外网IP
        /// </summary>
        public static readonly string CompanyIp = WebConfigurationManager.AppSettings["CompanyIp"] ?? "127.0.0.1";
    }
}