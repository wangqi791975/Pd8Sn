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
using Com.Panduo.Web.Common.Config;
using Com.Panduo.Web.Common.Config.Hander;
using Com.Panduo.Web.Common.Mvc.Model;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// �����ļ�������
    /// </summary>
    public class ConfigHelper
    {
        private static readonly CustomConfigManager CustomConfigManager = (CustomConfigManager)WebConfigurationManager.GetSection("CustomConfig");

        static ConfigHelper()
        {
            //��½����
            _loginConfigMap = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/Login.xml").Elements("Controller").ToDictionary(k => ((string)k.Attribute("Name")).Trim().ToLower(),v=>new LoginFilterConfig
                {
                    ControllerName = ((string)v.Attribute("Name")).Trim().ToLower(),
                    IncludeActionList = ((string)v.Element("Include")).Split(new []{',',';',':','|'},StringSplitOptions.RemoveEmptyEntries).ToList(),
                    ExcludeActionList = ((string)v.Element("Exclude")).Split(new[] { ',', ';', ':', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                });

            //_loginConfigMap =
            //    XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/Login.xml").Elements("Controller").ToDictionary(k => ((string)k.Attribute("Name")).Trim().ToLower(), v => new LoginFilterConfig
            //    {
            //        ControllerName = ((string)v.Attribute("Name")).Trim().ToLower(),
            //        IncludeActionList = ((string)v.Element("Include")).Split(new[] { ',', ';', ':', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
            //        ExcludeActionList = ((string)v.Element("Exclude")).Split(new[] { ',', ';', ':', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
            //    });
        }

        /// <summary>
        /// ��վǰ̨��ַ
        /// </summary>
        public static readonly string WebDomainHost = WebConfigurationManager.AppSettings["WebDomainHost"] ?? string.Empty;

        /// <summary>
        /// ���վ�㻺����Կ
        /// </summary>
        public static readonly string ClearSiteCacheToken = WebConfigurationManager.AppSettings["Clear.SiteCache.Token"] ?? string.Empty;

        /// <summary>
        /// ���ͼƬ����URL
        /// </summary>
        public static string CateogryImageHost
        {
            get { return ConfigurationManager.AppSettings["CateogryImageHost"] ?? string.Empty; }
        }

        /// <summary>
        /// Mail Logo����URL
        /// </summary>
        public static string MailLogoHost
        {
            get { return ConfigurationManager.AppSettings["MailLogoHost"] ?? string.Empty; }
        }


        private static IDictionary<string, LoginFilterConfig> _loginConfigMap;
        /// <summary>
        /// ��ȡ��¼����
        /// </summary>
        public static IDictionary<string, LoginFilterConfig> LoginConfigMap
        {
            get
            {
                return _loginConfigMap;
            }
        }
    }
}
