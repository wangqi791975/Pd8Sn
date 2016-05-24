using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using Microsoft.Practices.ServiceLocation;
using SolrNet;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// 产品Solr服务
    /// </summary>
    public class ProductSolrService
    {
        static ProductSolrService()
        {
            InitSolr();
        } 

        /// <summary>
        /// 产品Solr服务初始化
        /// </summary>
        private static void InitSolr()
        {
            Startup.Init<ProductSolrInfo>(new PostQueryConnection(SolrConfigurer.SolrSettings["SolrUrl.Product"]));
        }

        /// <summary>
        /// 获取产品Sol服务实例
        /// </summary>
        private static ISolrOperations<ProductSolrInfo> Instance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ISolrOperations<ProductSolrInfo>>();
            }
        }

        /// <summary>
        /// 获取产品Sol服务实例(只读)
        /// </summary>
        private static ISolrReadOnlyOperations<ProductSolrInfo> ReadOnlyInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ISolrReadOnlyOperations<ProductSolrInfo>>();
            }
        }
        

        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryParam">查询参数</param>
        /// <returns></returns>
        public static SolrQueryResultData SearchProduct(int currentPage,int pageSize,SolrQueryParam queryParam)
        { 
            var solrQueryResult = new SolrQueryResultData(); 
            var dataList = new List<ProductSolrInfo>();
            var totalNumFound = 0;

            if (queryParam.AreaType == ProductSearchAreaType.Promotion && queryParam.IsPromotionOn == false)
            {
                //请求促销区但是网站促销开关是关闭的时候不返回数据 
            }
            else
            {
                SolrQueryResults<ProductSolrInfo> result = null;
                if (SolrRateHelper.IsNeedDisplayControl(queryParam))
                {
                    #region 人工干预的查询模式
                    //只有配置了显示控制商品数量
                    #region 计算每种数据的数量配比
                    var totalQty = SolrRateHelper.GetDataCount(queryParam.AreaType); 
                    var divPageCount = decimal.Divide(totalQty, pageSize);//能够分页的倍数，可能会不能整除
                    var divMaxPageCount = (int)Math.Ceiling(divPageCount);//向上取整到最大的倍数
                    var divMaxTotalQty = divMaxPageCount * pageSize;//可能的最大数据，必须是整数
                    var avgQty = 0;
                    if (queryParam.IsPromotionOn)
                    {
                        avgQty = (int)Math.Floor(decimal.Divide(divMaxTotalQty, 3M));
                    }
                    else
                    {
                        avgQty = (int)Math.Floor(decimal.Divide(divMaxTotalQty, 2M));
                    } 
                    #endregion
                    
                    #region 取促销商品
                    var promotionResult = new SolrQueryResults<ProductSolrInfo>();
                    var promotionReturnQty = 0;
                    if (queryParam.IsPromotionOn)
                    {
                        var promotionQueryParam = queryParam.Clone();
                        promotionQueryParam.AreaType = ProductSearchAreaType.Promotion; 
                        var promotionSolrQueryHelper = new SolrQueryHelper(1, avgQty, promotionQueryParam);
                        promotionResult = ReadOnlyInstance.Query(promotionSolrQueryHelper.BaseQuery, promotionSolrQueryHelper.QueryOption);
                        promotionReturnQty = promotionResult.NumFound;
                    }
                    #endregion

                    #region 取热销商品
                    var bestSellserQueryParam = queryParam.Clone();
                    bestSellserQueryParam.AreaType = ProductSearchAreaType.BestSeller; 
                    //热销商品里面要排除已经获取的促销商品
                    if (bestSellserQueryParam.IgnoreProductIds.IsNullOrEmpty())
                    {
                        bestSellserQueryParam.IgnoreProductIds = promotionResult.Select(c => c.ProductId).ToList();
                    }
                    else
                    {
                        foreach (var item in promotionResult)
                        {
                            bestSellserQueryParam.IgnoreProductIds.Add(item.ProductId);
                        }
                    }
                    //促销商品不够的时候用热销商品补齐
                    var bestSellserSolrQueryHelper = new SolrQueryHelper(1, avgQty + (avgQty - promotionReturnQty), bestSellserQueryParam);

                    var bestSellserResult = ReadOnlyInstance.Query(bestSellserSolrQueryHelper.BaseQuery, bestSellserSolrQueryHelper.QueryOption);
                    var bestSellserReturnQty = bestSellserResult.NumFound; 
                    #endregion
                    
                    #region 取普通商品  
                    var normalQueryParam = queryParam.Clone();
                    normalQueryParam.AreaType = ProductSearchAreaType.NormalArea;
                    //促销+热销商品不够的时候用普通商品补齐
                    var normalSolrQueryHelper = new SolrQueryHelper(1, divMaxTotalQty - promotionReturnQty - bestSellserReturnQty, normalQueryParam);
                    //排除促销、热销商品
                    normalSolrQueryHelper.AddExtraFilterQuery(!new SolrQueryByField(ProductSolrField.IsPromotion, SolrConst.True));
                    normalSolrQueryHelper.AddExtraFilterQuery(!new SolrQueryByField(ProductSolrField.IsBestSeller, SolrConst.True));

                    var normalSellserResult = ReadOnlyInstance.Query(normalSolrQueryHelper.BaseQuery, normalSolrQueryHelper.QueryOption);
                    var normalReturnQty = normalSellserResult.NumFound; 
                    #endregion

                    #region 按照【促销商品】-【热销商品】-【普通商品】的顺序组装需要控制的N个商品
                    var controlResult = new SolrQueryResults<ProductSolrInfo>();
                    for (var i = 0; i < divMaxTotalQty; i++)
                    {
                        //添加促销商品
                        var promotionProduct = promotionResult.Where(c => !controlResult.Any(d => d.ProductId == c.ProductId)).FirstOrDefault();
                        if (promotionProduct != null)
                        {
                            controlResult.Add(promotionProduct);
                            promotionResult.Remove(promotionProduct);
                        }

                        //添加热销商品
                        var bestSellerProduct = bestSellserResult.Where(c => !controlResult.Any(d => d.ProductId == c.ProductId)).FirstOrDefault();
                        if (bestSellerProduct != null)
                        {
                            controlResult.Add(bestSellerProduct);
                            bestSellserResult.Remove(bestSellerProduct);
                        }

                        //添加普通商品
                        var normalProduct = normalSellserResult.Where(c => !controlResult.Any(d => d.ProductId == c.ProductId)).FirstOrDefault();
                        if (normalProduct != null)
                        {
                            controlResult.Add(normalProduct);
                            normalSellserResult.Remove(normalProduct);
                        } 
                    }
                    #endregion

                    #region 返回的商品数据
                    if (currentPage <= divMaxPageCount)
                    {
                        #region 请求页在需要控制的数据内 
                        var dataStartRowIndex = pageSize * (currentPage - 1);
                        var currentPageProducts = controlResult.Skip(dataStartRowIndex).Take(pageSize).ToList(); 
                        dataList.AddRange(currentPageProducts); 
                        #endregion
                    }
                    else
                    {
                        #region 请求页超过了控制的商品数量
                        var extraOverQueryParam = queryParam.Clone();

                        //把已经人工干预控制的商品排除掉
                        if (extraOverQueryParam.IgnoreProductIds.IsNullOrEmpty())
                        {
                            extraOverQueryParam.IgnoreProductIds = controlResult.Select(c => c.ProductId).ToList();
                        }
                        else
                        {
                            foreach (var item in controlResult)
                            {
                                extraOverQueryParam.IgnoreProductIds.Add(item.ProductId);
                            }
                        }

                        var extraSolrQueryHelper = new SolrQueryHelper(currentPage - divMaxPageCount, pageSize, extraOverQueryParam);//注意页码要减去前三页
                        var extraResult = ReadOnlyInstance.Query(extraSolrQueryHelper.BaseQuery, extraSolrQueryHelper.QueryOption);

                        dataList.AddRange(extraResult); 
                        #endregion
                    }
                    
                    //查询总数量以及属性值、类别统计信息但是不需要返回商品数据了
                    var solrQueryHelper = new SolrQueryHelper(1, 0, queryParam);
                    result = ReadOnlyInstance.Query(solrQueryHelper.BaseQuery, solrQueryHelper.QueryOption);

                    totalNumFound = result.NumFound; 
                    #endregion
                    #endregion
                }
                else
                {    
                    #region 非人工干预的查询模式
                    var solrQueryHelper = new SolrQueryHelper(currentPage, pageSize, queryParam); 
                    result = ReadOnlyInstance.Query(solrQueryHelper.BaseQuery, solrQueryHelper.QueryOption);

                    totalNumFound = result.NumFound; 
                    dataList.AddRange(result); 
                    #endregion
                } 

                #region 面统计
                //类别统计
                if (queryParam.IsStatisticsCategory)
                {

                    var categoryResult = result;
                    if (queryParam.AreaType == ProductSearchAreaType.SearchArea && (queryParam.CategoryId.HasValue || queryParam.CategoryPath.HasValue))
                    { 
                        //统计类别的时候需要情况类别过滤条件
                        var categoryParam = queryParam.Clone();
                        categoryParam.CategoryId = null;
                        categoryParam.CategoryPath = null;
                        var solrQueryHelper = new SolrQueryHelper(1, 0, categoryParam);
                        categoryResult = ReadOnlyInstance.Query(solrQueryHelper.BaseQuery, solrQueryHelper.QueryOption);
                    }

                    //根类别
                    ICollection<KeyValuePair<string, int>> rootCategoryQtyMap;
                    if (categoryResult.FacetFields.TryGetValue(ProductSolrField.RootCategoryId, out rootCategoryQtyMap))
                    {
                        solrQueryResult.RootCategoryQtyMap = rootCategoryQtyMap;

                        if (!rootCategoryQtyMap.IsNullOrEmpty())
                        {
                            foreach (var item in rootCategoryQtyMap)
                            {
                                solrQueryResult.AllCategoryQtyMap.Add(item.Key.ParseTo(0), item.Value);
                            }
                        }
                    }

                    //上级类别
                    ICollection<KeyValuePair<string, int>> parentCategoryQtyMap;
                    if (categoryResult.FacetFields.TryGetValue(ProductSolrField.ParentCategoryId, out parentCategoryQtyMap))
                    {
                        solrQueryResult.ParentCategoryQtyMap = parentCategoryQtyMap;

                        if (!parentCategoryQtyMap.IsNullOrEmpty())
                        {
                            foreach (var item in parentCategoryQtyMap)
                            {
                                if (!solrQueryResult.AllCategoryQtyMap.ContainsKey(item.Key.ParseTo(0)))
                                {
                                    solrQueryResult.AllCategoryQtyMap.Add(item.Key.ParseTo(0), item.Value);
                                }
                            }
                        }
                    }

                    //末级类别
                    ICollection<KeyValuePair<string, int>> categoryQtyMap;
                    if (categoryResult.FacetFields.TryGetValue(ProductSolrField.CategoryId, out categoryQtyMap))
                    {
                        solrQueryResult.CategoryQtyMap = categoryQtyMap;

                        if (!categoryQtyMap.IsNullOrEmpty())
                        {
                            foreach (var item in categoryQtyMap)
                            {
                                if (!solrQueryResult.AllCategoryQtyMap.ContainsKey(item.Key.ParseTo(0)))
                                { 
                                    solrQueryResult.AllCategoryQtyMap.Add(item.Key.ParseTo(0), item.Value);
                                }
                            }
                        }
                    }
                }

                //属性统计
                if (queryParam.IsStatisticsPropertyValue)
                {
                    IDictionary<int, int> resultPropertyValueQtyMap = new Dictionary<int, int>();
                    IDictionary<int, int> resultPropertyValueGroupQtyMap = new Dictionary<int, int>();

                    //属性值统计 
                    ICollection<KeyValuePair<string, int>> propertyValueQtyTotalMap;
                    if (result.FacetFields.TryGetValue(ProductSolrField.PropertyValueId, out propertyValueQtyTotalMap))
                    {
                        resultPropertyValueQtyMap = propertyValueQtyTotalMap.Select(c => new KeyValuePair<int, int>(c.Key.ParseTo(0), c.Value)).ToDictionary();
                    }

                    //属性值组统计
                    ICollection<KeyValuePair<string, int>> propertyValueGroupQtyTotalMap;
                    if (result.FacetFields.TryGetValue(ProductSolrField.PropertyValueGroupId, out propertyValueGroupQtyTotalMap))
                    {
                        resultPropertyValueGroupQtyMap = propertyValueGroupQtyTotalMap.Select(c => new KeyValuePair<int, int>(c.Key.ParseTo(0), c.Value)).ToDictionary();
                    }

                    if (!queryParam.FiterPropertyValueAndGroupIds.IsNullOrEmpty())
                    {
                        #region 存在属性值筛选的时候

                        var propertyParam = queryParam.Clone();
                        var fiterProperties = new Dictionary<int, IList<KeyValuePair<SolrPropertyType, int>>>();
                        foreach (var item in propertyParam.FiterPropertyValueAndGroupIds)
                        {
                            fiterProperties.Add(item.Key, item.Value);
                        }

                        foreach (var fiterProperty in fiterProperties)
                        {
                            propertyParam.FiterPropertyValueAndGroupIds = queryParam.FiterPropertyValueAndGroupIds.Where(c => c.Key != fiterProperty.Key).ToDictionary(k => k.Key, v => v.Value);

                            var solrQueryHelper = new SolrQueryHelper(1, 0, propertyParam);
                            result = ReadOnlyInstance.Query(solrQueryHelper.BaseQuery, solrQueryHelper.QueryOption);


                            //属性值统计
                            ICollection<KeyValuePair<string, int>> propertyValueQtyMap;
                            if (result.FacetFields.TryGetValue(ProductSolrField.PropertyValueId, out propertyValueQtyMap))
                            {
                                solrQueryResult.FiterPropertyValueQtyMap.Add(fiterProperty.Key, propertyValueQtyMap.ToDictionary(k => k.Key.ParseTo(0), v => v.Value));
                            }

                            //属性值组统计 
                           ICollection<KeyValuePair<string, int>> propertyValueGroupQtyMap;
                           if (result.FacetFields.TryGetValue(ProductSolrField.PropertyValueId, out propertyValueGroupQtyMap))
                            {
                                solrQueryResult.FiterPropertyValueGroupQtyMap.Add(fiterProperty.Key, propertyValueGroupQtyMap.ToDictionary(k => k.Key.ParseTo(0), v => v.Value));
                            }
                        }
                        #endregion
                    } 

                    solrQueryResult.PropertyValueQtyMap = resultPropertyValueQtyMap;
                    solrQueryResult.PropertyValueGroupQtyMap = resultPropertyValueGroupQtyMap;
                } 
                #endregion  
            }

            solrQueryResult.Pager = new Pager(totalNumFound, currentPage, pageSize);
            solrQueryResult.DataList = dataList;

            return solrQueryResult;
        }

        /// <summary>
        /// 根据关键字搜索商品
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>返回搜索到的商品个数</returns>
        public static int SearchProductCount(string keyword)
        {
            var param = new SolrQueryHelper(1, 0, new SolrQueryParam{Keyword = keyword,AreaType = ProductSearchAreaType.SearchArea});

            var result = ReadOnlyInstance.Query(param.BaseQuery, param.QueryOption);

            return result.NumFound;
        } 
    }
}
