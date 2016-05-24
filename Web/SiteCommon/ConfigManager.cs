using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Xml.Linq; 
using Com.Panduo.Common;
using Com.Panduo.Web.Common;
namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigManager
    { 
        static ConfigManager()
        { 
        } 

        /// <summary>
        /// 是否500错误过滤器
        /// </summary>
        public static bool IsSystemExceptionFilter
        {
            get { return ConfigurationManager.AppSettings["IsSystemExceptionFilter"].ParseTo(false); }
        }
        

        /// <summary>
        /// 网站Cookie域名后缀
        /// </summary>
        public  static string CrossDomainCookie
        {
            get { return ConfigurationManager.AppSettings["CrossDomainCookie"]??string.Empty; }
        }

        /// <summary>
        /// 网站主题
        /// </summary>
        public static string SiteTheme
        {
            get { return ConfigurationManager.AppSettings["SiteTheme"] ?? "Default"; }
        }
    } 
}
