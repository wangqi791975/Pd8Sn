using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Service.Product;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// Solr商品比率辅助类
    /// </summary>
    public class SolrRateHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static SolrRateHelper()
        {
            SolrRateMap = new Dictionary<ProductSearchAreaType, int>();

            if (SolrRateConfig.IsNullOrEmpty())
            {
                throw new ConfigurationErrorsException(string.Format("Solr product rate config is wrong."));
            }

            var pageConfigs = SolrRateConfig.Split(new[] { '|' });
            if (pageConfigs.Any())
            { 
                foreach (var pageConfig in pageConfigs)
                {
                    var config = pageConfig.Split(":").ToArray();
                    SolrRateMap.Add(EnumHelper.ToEnum<ProductSearchAreaType>(config[0]), config[1].ParseTo(0)); 
                }
            }
        }

        /// <summary>
        /// Solr商品比率配置项
        /// </summary>
        public static readonly string SolrRateConfig = SolrConfigurer.SolrSettings["Product.DisplayRate"];
        /// <summary>
        /// Solr商品比率配置字典
        /// </summary>
        public static readonly IDictionary<ProductSearchAreaType,int> SolrRateMap = null; 

        /// <summary>
        /// 获取指定页码指定类型的数据比率值
        /// </summary> 
        /// <param name="areaType">数据类型</param>
        /// <returns></returns>
        public static int GetDataCount(ProductSearchAreaType areaType)
        {
            if (SolrRateMap.ContainsKey(areaType))
            {
                return SolrRateMap[areaType];
            }

            return 0;
        }

        /// <summary>
        /// 是否需要人工控制显示比率
        /// </summary>
        /// <param name="solrQueryParam"></param>
        /// <returns></returns>
        public static bool IsNeedDisplayControl(SolrQueryParam solrQueryParam)
        {
            return SolrRateMap.ContainsKey(solrQueryParam.AreaType) && solrQueryParam.Sorts.Any(c=>c == ProductSorterCriteria.BestMatch);
        }
    } 
}
