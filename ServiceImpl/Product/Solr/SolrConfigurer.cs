using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// Solr配置管理类
    /// </summary>
    public class SolrConfigurer
    {
        public const string SectionName = "productSolrSettings";

        public static readonly NameValueCollection SolrSettings;

        static SolrConfigurer()
        {
            SolrSettings = (NameValueCollection)ConfigurationManager.GetSection(SectionName);
            if (SolrSettings == null)
            {
                throw new ConfigurationErrorsException(string.Format("Can't find the configuration section[{0}].", SectionName));
            }
        }
    }
}
