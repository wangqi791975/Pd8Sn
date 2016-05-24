using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// Solr商品比率辅助类
    /// </summary>
    public class SolrPageRateHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static SolrPageRateHelper()
        {
            SolrRateMap = new Dictionary<int, IDictionary<SolrRateType, decimal>>();

            if (SolrRateConfig.IsNullOrEmpty())
            {
                throw new ConfigurationErrorsException(string.Format("Solr product rate config is wrong."));
            }

            var pageConfigs = SolrRateConfig.Split(new[] { '|' });
            if (pageConfigs.Count() >0 )
            {
                var pageIndex = 0;
                foreach (var pageConfig in pageConfigs)
                {
                    pageIndex++;

                    var rates = pageConfig.Split(",");
                    if (rates.IsNullOrEmpty() || rates.Count != SolrRateTypes.Count)
                    {
                        throw new ConfigurationErrorsException(string.Format("Solr product rate({0}) config is wrong for page{1}.", rates, pageIndex));
                    }

                    var rateMap = new Dictionary<SolrRateType, decimal>
                    {
                        {SolrRateType.Promotion,rates[0].ParseTo(0.00M)},
                        {SolrRateType.BestSeller,rates[1].ParseTo(0.00M)},
                        {SolrRateType.FeaturedProduct,rates[2].ParseTo(0.00M)},
                        {SolrRateType.Normal,rates[3].ParseTo(0.00M)},
                    };

                    if (rateMap.Sum(c=>c.Value) != 1)
                    {
                         throw new ConfigurationErrorsException(string.Format("Solr product rate({0}) config is wrong for page{1},total rate must be 1.", rates, pageIndex));
                    }

                    SolrRateMap.Add(pageIndex , rateMap);
                }
            }
        }

        /// <summary>
        /// Solr商品比率配置项
        /// </summary>
        public static readonly string SolrRateConfig = SolrConfigurer.SolrSettings["Page.DisplayRate"];
        /// <summary>
        /// Solr商品比率配置字典
        /// </summary>
        public static readonly IDictionary<int, IDictionary<SolrRateType, decimal>> SolrRateMap = null;
        /// <summary>
        /// Solr商品比率枚举
        /// </summary>
        private static readonly IList<SolrRateType> SolrRateTypes = EnumHelper.ToEnums<SolrRateType>();

        /// <summary>
        /// 获取指定页码指定类型的数据比率值
        /// </summary>
        /// <param name="currentPage">页码</param>
        /// <param name="solrRateType">数据类型</param>
        /// <returns></returns>
        public static decimal GetDataRate(int currentPage, SolrRateType solrRateType)
        {
            if (SolrRateMap.ContainsKey(currentPage) && SolrRateMap[currentPage].ContainsKey(solrRateType))
            {
                return SolrRateMap[currentPage][solrRateType];
            }

            return 0;
        }

        /// <summary>
        /// 获取指定类型页面的数据条数
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="solrRateType"></param>
        /// <returns></returns>
        public static int GetDataCount(int currentPage,int pageSize,SolrRateType solrRateType)
        {
            var rate = GetDataRate(currentPage, solrRateType);
            if (rate >0 &&rate < 1)
            {
                return (int)Math.Floor(rate * pageSize);
            }

            return pageSize;
        }
    }

    /// <summary>
    /// 商品比率类型
    /// </summary>
    public enum SolrRateType
    {
        /// <summary>
        /// 促销商品
        /// </summary>
        Promotion = 10,
        /// <summary>
        /// 热销品
        /// </summary>
        BestSeller = 20,
        /// <summary>
        /// 推荐商品
        /// </summary>
        FeaturedProduct = 30,
        /// <summary>
        /// 正常商品
        /// </summary>
        Normal = 100
    }
}
