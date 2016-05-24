using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    internal class SolrConfigHelper
    {
        static SolrConfigHelper ()
        {
            switch (SolrConfigurer.SolrSettings["SolrEngine"].Trim().ToLower())
            {
                case "dismax":
                    SolrEngine = SolrEngine.DisMax;
                    break;
                case "edismax":
                    SolrEngine = SolrEngine.EDisMax;
                    break;
                default:
                    SolrEngine = SolrEngine.Standard;
                    break;
            }
           
        }

        /// <summary>
        /// Solr搜索引擎
        /// </summary>
        public static readonly SolrEngine SolrEngine = SolrEngine.Standard;

        /// <summary>
        /// 标准关键字搜索字段格式
        /// </summary>
        public static readonly string SearchKeywordFormat = SolrConfigurer.SolrSettings["Standard.SearchKeywordFormat"];

        public static readonly string EDisMaxSearchKeywordFormat = SolrConfigurer.SolrSettings["EDisMax.SearchKeywordFormat"];
        /// <summary>
        /// DisMax和EDisMax Solr引擎查询字段权重
        /// </summary>
        public static readonly string EDisMaxQf = SolrConfigurer.SolrSettings["EDisMax.QF"];
        /// <summary>
        ///  DisMax和EDisMax Solr引擎影响查询分值函数
        /// </summary>
        public static readonly string EDisMaxBf = SolrConfigurer.SolrSettings["EDisMax.BF"];
       
        /// <summary>
        /// 关键字同义词搜索字段格式
        /// </summary>
        public static readonly string SearchKeywordFormatOfSynonym = SolrConfigurer.SolrSettings["SearchKeywordFormatOfSynonym"] ?? SearchKeywordFormat;

        ///// <summary>
        ///// 每页显示的比例。促销类商品，商品池类商品，普通商品
        ///// </summary>
        //public static readonly float[,] DisplayRatePerPage = GetDisplayRateArr();

        //private static float[,] GetDisplayRateArr()
        //{
        //    string config = SolrConfigurer.SolrSettings["Page.DisplayRate"];
        //    if (string.IsNullOrEmpty(config)) return null;
        //    string[] pageArr = config.Trim(',').Split('|');
        //    float[,] data = new float[pageArr.Length, 3];
        //    string[] rateArr;
        //    for (int i = 0; i < pageArr.Length; i++)
        //    {
        //        rateArr = pageArr[i].Trim(',').Split(',');
        //        for (int j = 0; j < rateArr.Length; j++)
        //        {
        //            data[i, j] = float.Parse(rateArr[j]);
        //        }
        //        if ((data[i, 0] + data[i, 1] + data[i, 2]).CompareTo(1f) != 0)
        //        {
        //            throw new System.Configuration.ConfigurationErrorsException("比例设置有误。");
        //        }
        //    }
        //    return data;
        //}
    }
}
