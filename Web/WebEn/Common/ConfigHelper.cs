using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //_loginConfigMap = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/Login.xml").Elements("Controller").ToDictionary(c => (string)c.Attribute("Name"), c => c.Elements("Action").Select(d => (string)d).ToList());
            _loginConfigMap =
                XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/Login.xml").Elements("Controller").ToDictionary(k => ((string)k.Attribute("Name")).Trim().ToLower(), v => new LoginFilterConfig
                {
                    ControllerName = ((string)v.Attribute("Name")).Trim().ToLower(),
                    IncludeActionList = ((string)v.Element("Include")).Split(new []{',',';',':','|'},StringSplitOptions.RemoveEmptyEntries).ToList(),
                    ExcludeActionList = ((string)v.Element("Exclude")).Split(new[] { ',', ';', ':', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                });

            //SSL����
            _sslConfigMap = new Dictionary<string, IDictionary<string, IList<string>>>();
            var controllers = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/SslConfig.xml").Elements("Controller");
            var controllerName = String.Empty;
            foreach (var controller in controllers)
            {
                controllerName = (string)controller.Attribute("Name");
                _sslConfigMap.Add(controllerName, new Dictionary<string, IList<string>>());
                _sslConfigMap[controllerName].Add("Add", controller.Elements("Add").Select(c => (string)c).ToList());
                _sslConfigMap[controllerName].Add("Remove", controller.Elements("Remove").Select(c => (string)c).ToList());
            }

            //�Զ���·������
            _routeConfigs = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/Route.config").Elements("Route").Select(c => new
            {
                Name = (string)c.Attribute("Name"),
                Url = (string)c.Attribute("Url"),
                DefaultsValue = (string)c.Attribute("Defaults"),
                ConstraintsValue = (string)c.Attribute("Constraints"),
                NamespacesValue = (string)c.Attribute("Namespaces"),
            }).Select(c => new RouteObject
            {
                Name = c.Name,
                Url = c.Url,
                DefaultsValue = c.DefaultsValue ?? String.Empty,
                ConstraintsValue = c.ConstraintsValue ?? String.Empty,
                NamespacesValue = c.NamespacesValue ?? String.Empty,
                Defaults = c.DefaultsValue.IsNullOrEmpty() ? null : c.DefaultsValue.FromJsonAnonymous(),
                Constraints = c.ConstraintsValue.IsNullOrEmpty() ? null : c.ConstraintsValue.FromJsonAnonymous(),
                Namespaces = c.NamespacesValue.IsNullOrEmpty() ? null : c.NamespacesValue.Split(",").ToArray()
            }).ToList();


            var strDefualtCurrency = ConfigurationManager.AppSettings["DefualtCurrency"];
            if (!strDefualtCurrency.IsNullOrEmpty())
            {
                _defaultCurrencyWithLanguages = new Dictionary<string, List<string>>();
                foreach (var item in strDefualtCurrency.Split('|'))
                {
                    if (!item.IsNullOrEmpty() && item.Contains(":"))
                    {
                        var strKey = item.Split(':')[0];
                        var strValue = item.Split(':')[1];
                        if (!strKey.IsNullOrEmpty() && !strValue.IsNullOrEmpty())
                            _defaultCurrencyWithLanguages.Add(strKey, strValue.Split(',').ToList());
                    }
                }
            }

            //MailChimp
            var element = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Config/MailChimpServiceConfig.config").Element("MailChimpService");
            if (!element.IsNullOrEmpty())
                _mainChimpServiceConfig = new MainChimpConfig
                {
                    ApiKey = element.Attribute("apiKey").Value,
                    SubscriberListId = element.Attribute("subscriberListId").Value,
                    ServiceUrl = element.Attribute("serviceUrl").Value,
                    ListsRelatedSection = element.Attribute("listsRelatedSection").Value,
                    HelperRelatedSection = element.Attribute("helperRelatedSection").Value,
                };
        }

        //private static IDictionary<string, List<string>> _loginConfigMap;
        ///// <summary>
        ///// ��ȡ��¼����
        ///// </summary>
        //public static IDictionary<string, List<string>> LoginConfigMap
        //{
        //    get
        //    {
        //        return _loginConfigMap;
        //    }
        //}

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

        

        private static IDictionary<string, IDictionary<string, IList<string>>> _sslConfigMap;
        /// <summary>
        /// ��ȡHttps����
        /// </summary>
        public static IDictionary<string, IDictionary<string, IList<string>>> SslConfigMap
        {
            get
            {
                return _sslConfigMap;
            }
        }

        private static IList<RouteObject> _routeConfigs;
        /// <summary>
        /// ��ȡ·������
        /// </summary>
        public static IList<RouteObject> RountConfigs
        {
            get
            {
                return _routeConfigs;
            }
        }

        /// <summary>
        /// �Ƿ�����Https
        /// </summary>
        public static bool IsEnableHttps
        {
            get { return ConfigurationManager.AppSettings["IsEnableHttps"].ParseTo(false); }
        }

        /// <summary>
        /// Http�˿�
        /// </summary>
        public static int HttpPort
        {
            get { return ConfigurationManager.AppSettings["HttpPort"].ParseTo(80); }
        }

        /// <summary>
        /// Https�˿�
        /// </summary>
        public static int HttpsPort
        {
            get { return ConfigurationManager.AppSettings["HttpsPort"].ParseTo(443); }
        }


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

        #region վ��������Ĭ�ϻ��Ҷ�Ӧ��ϵ

        private static Dictionary<string, List<string>> _defaultCurrencyWithLanguages;

        /// <summary>
        /// վ��������Ĭ�ϻ��Ҷ�Ӧ��ϵ
        /// <para>Key string: ���ֱ��� USD</para>
        /// <para>Value List string:���ּ��� DE,FR,ES,IT,RU,JA</para>
        /// </summary>
        public static Dictionary<string, List<string>> DefaultCurrencyWithLanguages
        {
            get { return _defaultCurrencyWithLanguages; }
        }

        #endregion

        #region MainChimp

        private static MainChimpConfig _mainChimpServiceConfig;
        /// <summary>
        /// վ��Ĭ������
        /// </summary>
        public static MainChimpConfig MainChimpServiceConfig
        {
            get { return _mainChimpServiceConfig; }
        }
        public class MainChimpConfig
        {
            public string ApiKey { get; set; }

            public string SubscriberListId { get; set; }

            //https://us2.api.mailchimp.com/2.0
            public string ServiceUrl { get; set; }

            /// <summary>
            /// lists
            /// </summary>
            public string ListsRelatedSection { get; set; }
            /// <summary>
            /// lists
            /// </summary>
            public string HelperRelatedSection { get; set; }

        }
        #endregion

        #region ���ﳵ

        /// <summary>
        ///  �ͻ�δ��¼����add to cartʱ�����жϣ�����Ʒ�����Ѿ�������ֵ��������½��
        /// </summary>
        public static int MaxNotLoggedShoppingCartItemCount
        {
            get { return ConfigurationManager.AppSettings["MaxNotLoggedShoppingCartItemCount"].ParseTo(20); }
        }
        /// <summary>
        ///  ���ﳵһҳ�����ʾ���ٸ���Ʒ
        /// </summary>
        public static int MaxShoppingCartItemPageSize
        {
            get { return ConfigurationManager.AppSettings["MaxShoppingCartItemPageSize"].ParseTo(100); }
        }

        #endregion

        #region ֧�����
        /// <summary>
        /// ����֧����������ͷ��ַ����Ҫ��Ϊ�˼�������֧���Ժ��޷��������ݶ����ã���ʽ��������Ҫ����
        /// </summary>
        public static readonly string PaymentReturnHost = WebConfigurationManager.AppSettings["Payment.Return.Host"] ?? string.Empty;  
        #endregion

        /// <summary>
        /// ���վ�㻺����Կ
        /// </summary>
        public static readonly string ClearSiteCacheToken = WebConfigurationManager.AppSettings["Clear.SiteCache.Token"] ?? string.Empty; 
        
        /// <summary>
        ///  ��������һҳ�����ʾ����
        /// </summary>
        public static int MaxArticlePageSize
        {
            get { return ConfigurationManager.AppSettings["MaxArticlePageSize"].ParseTo(10); }
        }
    }
}
