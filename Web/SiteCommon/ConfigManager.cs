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
    /// �����ļ�������
    /// </summary>
    public class ConfigManager
    { 
        static ConfigManager()
        { 
        } 

        /// <summary>
        /// �Ƿ�500���������
        /// </summary>
        public static bool IsSystemExceptionFilter
        {
            get { return ConfigurationManager.AppSettings["IsSystemExceptionFilter"].ParseTo(false); }
        }
        

        /// <summary>
        /// ��վCookie������׺
        /// </summary>
        public  static string CrossDomainCookie
        {
            get { return ConfigurationManager.AppSettings["CrossDomainCookie"]??string.Empty; }
        }

        /// <summary>
        /// ��վ����
        /// </summary>
        public static string SiteTheme
        {
            get { return ConfigurationManager.AppSettings["SiteTheme"] ?? "Default"; }
        }
    } 
}
