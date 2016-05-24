using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Service.Product;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    [Serializable]
    public class SolrQueryHelper
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SolrQueryHelper()
            : this(1, 10)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentPage">当前页码:从1开始</param>
        /// <param name="pageSize">分页大小</param>
        public SolrQueryHelper(int currentPage, int pageSize)
            : this(currentPage, pageSize, new SolrQueryParam())
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentPage">当前页码:从1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryParam">查询参数</param>
        public SolrQueryHelper(int currentPage, int pageSize, SolrQueryParam queryParam)
        {
            CurrentPage = currentPage < 1 ? 1 : currentPage;
            PageSize = pageSize < 0 ? 0 : pageSize;

            BuildSolrQuery(queryParam);
        }

        /// <summary>
        /// 查询允许的最大长度
        /// </summary>
        internal const int MaxQueryStringLength = SolrConst.MaxQueryStringLength;
        /// <summary>
        /// 要查询的字段
        /// </summary>
        private IList<string> _fields = new List<string>();

        /// <summary>
        /// 要过滤的条件
        /// </summary>
        private IList<ISolrQuery> _filterQueries = new List<ISolrQuery>();

        /// <summary>
        /// 排序条件
        /// </summary>
        private IList<SortOrder> _sortOrders = new List<SortOrder>();

        /// <summary>
        /// 面统计条件
        /// </summary>
        private IList<SolrFacetFieldQuery> _facetFileds = new List<SolrFacetFieldQuery>();

        /// <summary>
        /// 基础查询,参数q=xxx
        /// </summary>
        private ISolrQuery _baseQuery = SolrQuery.All;
        /// <summary>
        /// 当前查询区域
        /// </summary>
        private  ProductSearchAreaType AeaType = ProductSearchAreaType.All;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }


        /// <summary>
        /// 构建Solr查询
        /// </summary>
        /// <param name="queryParam"></param> 
        public void BuildSolrQuery(SolrQueryParam queryParam)
        {
            AeaType = queryParam.AreaType;

            // 1.返回的solr字段(fl=xxx)
            _fields.Add(ProductSolrField.ProductId);
            //最佳匹配商品需要返回匹配到的产品
            if (queryParam.AreaType == ProductSearchAreaType.BestMatch)
            {
                _fields.Add(ProductSolrField.ProductMatchId);
            }

            //2.主查询部分(q=xxx),这部分数据会参与匹配权重分值
            if (!queryParam.Keyword.IsNullOrEmpty())
            {
                //关键字查询为了得到分值需要放在q=xxx部分
                var keywordQuery = BuildKeywordQuery(queryParam.Keyword.Trim().ToLower());
                if (keywordQuery != null)
                {
                    _baseQuery = keywordQuery;
                }
            }

            //3.filterQueries过滤部分
            //商品搜索区域
            switch (queryParam.AreaType)
            {
                case ProductSearchAreaType.All:
                    //不需要特殊处理,可以搜索所有商品
                    break;
                case ProductSearchAreaType.NormalArea:
                    //不需要特殊处理
                    break;
                case ProductSearchAreaType.NewArrival:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsNewArrival, SolrConst.True));
                    break;
                case ProductSearchAreaType.BestSeller:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsBestSeller, SolrConst.True));
                    break;
                case ProductSearchAreaType.MixProduct:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsMixed, SolrConst.True));
                    break;
                case ProductSearchAreaType.Promotion:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsPromotion, SolrConst.True));
                    break;
                case ProductSearchAreaType.SearchArea:
                    //不需要特殊处理
                    break;
                case ProductSearchAreaType.DailyDeals:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsDailyDeal, SolrConst.True));
                    break;
                case ProductSearchAreaType.SimilarItem:
                    //不需要特殊处理,需要根据商品的属性值匹配
                    break;
                case ProductSearchAreaType.FeaturedProduct:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsRecommend, SolrConst.True));
                    break;
                case ProductSearchAreaType.ClubProduct:
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsClub, SolrConst.True));
                    break;
                case ProductSearchAreaType.ProductArea:
                    break;
                case ProductSearchAreaType.Remplacement:
                    break;
                case ProductSearchAreaType.FixedPriceArea:
                    break;
                case ProductSearchAreaType.Closeout:
                    break;
                case ProductSearchAreaType.BestMatch:
                    break;
            }

            //商品Id精确匹配(OR关系)
            if (!queryParam.ProductIds.IsNullOrEmpty())
            {
                _filterQueries.Add(new SolrQueryInList(ProductSolrField.ProductId, queryParam.ProductIds.Select(c => c.ToString())));
            }

            //排除的商品ID
            if (!queryParam.IgnoreProductIds.IsNullOrEmpty())
            {
                _filterQueries.Add(!new SolrQueryInList(ProductSolrField.ProductId, queryParam.IgnoreProductIds.Select(c => c.ToString())));
            }

            //商品Sku(OR关系)
            if (!queryParam.Skus.IsNullOrEmpty())
            {
                _filterQueries.Add(new SolrQueryInList(ProductSolrField.Sku, queryParam.Skus));
            }

            //上架开始-截止时间
            if (queryParam.JoinDateFrom.HasValue || queryParam.JoinDateTo.HasValue)
            {
                _filterQueries.Add(new SolrQueryByRange<DateTime?>(ProductSolrField.JoinDate, queryParam.JoinDateFrom, queryParam.JoinDateTo, true));
            }

            //创建开始-截止时间
            if (queryParam.CreateDateFrom.HasValue || queryParam.CreateDateTo.HasValue)
            {
                _filterQueries.Add(new SolrQueryByRange<DateTime?>(ProductSolrField.CreateDate, queryParam.CreateDateFrom, queryParam.CreateDateTo, true));
            }

            //最低售价开始-截止金额(包含),美元金额
            if (queryParam.SalePriceMinFrom.HasValue || queryParam.SalePriceMinTo.HasValue)
            {
                _filterQueries.Add(new SolrQueryByRange<decimal?>(ProductSolrField.SalePriceMin, queryParam.SalePriceMinFrom, queryParam.SalePriceMinTo, true));
            }

            //网站是否开启促销
            //IsPromotionOn

            //促销类型
            if (queryParam.PromotionId.HasValue)
            {
                _filterQueries.Add(new SolrQueryByField(ProductSolrField.PromotionId, queryParam.PromotionId.ToString()));
            }

            //促销折扣开始-截止
            if (queryParam.PromotionDiscountFrom.HasValue || queryParam.PromotionDiscountTo.HasValue)
            {
                _filterQueries.Add(new SolrQueryByRange<decimal?>(ProductSolrField.PromotionDiscount, queryParam.PromotionDiscountFrom, queryParam.PromotionDiscountTo, true));
            }

            //产品专区
            if (queryParam.ProductAreaId.HasValue)
            {
                _filterQueries.Add(new SolrQueryByField(ProductSolrField.ProductAreaId, queryParam.ProductAreaId.ToString()));
            }
            
            //查询库存的商品
            if (queryParam.IsInStock.HasValue)
            {
                if (queryParam.IsInStock.Value)
                {
                    _filterQueries.Add(new SolrQueryByRange<int?>(ProductSolrField.ProductStock, 1, null, true));
                }
                else
                {
                    _filterQueries.Add(!new SolrQueryByRange<int?>(ProductSolrField.ProductStock, 1, null, true));
                }
            }

            //查询热销的商品
            if (queryParam.IsBestSeller.HasValue)
            {
                _filterQueries.Add(new SolrQueryByField(ProductSolrField.IsBestSeller, queryParam.IsBestSeller.Value ? SolrConst.True : SolrConst.False));
            }

            //查询正常销售状态的商品
            if (queryParam.IsOnSale.HasValue)
            {
                if (queryParam.IsOnSale.Value)
                {
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.ProductStatus, ((int)ProductStatus.OnSale).ToString()));
                }
                else
                {
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.ProductStatus, ((int)ProductStatus.BackOrder).ToString()));
                }
            }

            //类别ID
            if (queryParam.CategoryId.HasValue)
            {
                _filterQueries.Add(new SolrQueryByField(ProductSolrField.CategoryId, queryParam.CategoryId.ToString()));
            }

            //类别路径
            if (queryParam.CategoryPath.HasValue)
            {
                _filterQueries.Add(new SolrQueryByField(ProductSolrField.CategoryPath, queryParam.CategoryPath.ToString()));
                //_filterQueries.Add(new SolrQueryByField(ProductSolrField.RootCategoryId, queryParam.CategoryPath.ToString()) || new SolrQueryByField(ProductSolrField.ParentCategoryId, queryParam.CategoryPath.ToString()) || new SolrQueryByField(ProductSolrField.CategoryId, queryParam.CategoryPath.ToString()));
            }

            //属性值ID(And关系)
            if (!queryParam.PropertyValueIds.IsNullOrEmpty())
            {
                foreach (var item in queryParam.PropertyValueIds)
                {
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.PropertyValueId, item.ToString()));
                    //_filterQueries.Add(new SolrQuery(string.Format("{0}:{1}", ProductSolrField.PropertyValueId, item)));
                }
            }

            //属性值ID(Or关系)
            if (!queryParam.OrPropertyValueIds.IsNullOrEmpty() )//&& queryParam.AreaType != ProductSearchAreaType.SimilarItem)
            {
                foreach (var item in queryParam.OrPropertyValueIds)
                {
                    _filterQueries.Add(new SolrQuery(string.Format("{0}:{1}", ProductSolrField.PropertyValueId, item)));
                }  
                //_filterQueries.Add(new SolrQueryInList(ProductSolrField.PropertyValueId, queryParam.OrPropertyValueIds.Select(c => c.ToString())));
            }

            //忽略的属性值ID
            if (!queryParam.IgnorePropertyValueIds.IsNullOrEmpty())
            {
                _filterQueries.Add(!new SolrQueryInList(ProductSolrField.PropertyValueId, queryParam.IgnorePropertyValueIds.Select(c => c.ToString())));
            }

            //属性值组ID(And关系)
            if (!queryParam.PropertyValueGroupIds.IsNullOrEmpty())
            {
                foreach (var item in queryParam.PropertyValueGroupIds)
                {
                    _filterQueries.Add(new SolrQueryByField(ProductSolrField.PropertyValueGroupId, item.ToString()));
                    //_filterQueries.Add(new SolrQuery(string.Format("{0}:{1}", ProductSolrField.PropertyValueGroupId, item)));
                }
            }

            //属性值组ID(Or关系)
            if (!queryParam.OrPropertyValueGroupIds.IsNullOrEmpty())
            {
                //foreach (var item in queryParam.OrPropertyValueGroupIds)
                //{
                //    _filterQueries.Add(new SolrQueryByField(ProductSolrField.PropertyValueGroupId, item.ToString()));
                //}  

                _filterQueries.Add(new SolrQueryInList(ProductSolrField.PropertyValueGroupId, queryParam.OrPropertyValueGroupIds.Select(c => c.ToString())));
            }

            //忽略的属性值组ID
            if (!queryParam.IgnorePropertyValueGroupIds.IsNullOrEmpty())
            {
                _filterQueries.Add(!new SolrQueryInList(ProductSolrField.PropertyValueGroupId, queryParam.IgnorePropertyValueGroupIds.Select(c => c.ToString())));
            }

            //属性值和属性值组过滤,(属性之间为And关系,属性内部的属性值和属性值组之间OR的关系)
            if (!queryParam.FiterPropertyValueAndGroupIds.IsNullOrEmpty())
            {
                foreach (var item in queryParam.FiterPropertyValueAndGroupIds)
                {
                    _filterQueries.Add(new SolrQuery(item.Value.Select(c => string.Format("{0}:{1}", c.Key == SolrPropertyType.PropertyValueGroup ? ProductSolrField.PropertyValueGroupId : ProductSolrField.PropertyValueId, c.Value)).Join(" OR ")));
                }
            }

            //4.Facet面统计部分
            //4.1.是否统计属性值
            if (queryParam.IsStatisticsPropertyValue)
            {
                //根级类别
                _facetFileds.Add(new SolrFacetFieldQuery(ProductSolrField.RootCategoryId) { MinCount = 1 });
                //上级类别
                _facetFileds.Add(new SolrFacetFieldQuery(ProductSolrField.ParentCategoryId) { MinCount = 1 });
                //末级类别
                _facetFileds.Add(new SolrFacetFieldQuery(ProductSolrField.CategoryId) { MinCount = 1 });
            }

            //4.2.是否统计类别
            if (queryParam.IsStatisticsCategory)
            {
                //属性值 
                _facetFileds.Add(new SolrFacetFieldQuery(ProductSolrField.PropertyValueId) { MinCount = 1 });

                //属性值组 
                _facetFileds.Add(new SolrFacetFieldQuery(ProductSolrField.PropertyValueGroupId) { MinCount = 1 });
            }

            //5.Sort排序部分
            if (!queryParam.Sorts.IsNullOrEmpty())
            {
                foreach (var sortOrder in queryParam.Sorts)
                {
                    switch (sortOrder)
                    {
                        case ProductSorterCriteria.None:
                            break;
                        case ProductSorterCriteria.PriceLowToHigh:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.SalePriceMin, SolrNet.Order.ASC));
                            break;
                        case ProductSorterCriteria.PriceHighToLow:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.SalePriceMin, SolrNet.Order.DESC));
                            break;
                        case ProductSorterCriteria.BestMatch:
                            //有BestMatch排序的时候还要返回分值字段，因为要在内存做比率值排序
                            //_fields.Add(ProductSolrField.Score);

                            //商品搜索区域
                            switch (queryParam.AreaType)
                            {
                                //全站搜索
                                case ProductSearchAreaType.SearchArea:
                                    //正常销售的商品(1)一定要排在预售(2)产品前面
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                                    _sortOrders.Add(new SortOrder(ProductSolrField.Score, SolrNet.Order.DESC));
                                    break;
                                //正常类别筛选和混装
                                case ProductSearchAreaType.NormalArea:
                                case ProductSearchAreaType.MixProduct:
                                    //1.促销商品
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortPromotion, SolrNet.Order.DESC));

                                    //2.热销商品 
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortBestSeller, SolrNet.Order.DESC));

                                    //3.普通商品 
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortNormal, SolrNet.Order.DESC));
                                    break;
                                //新品区域
                                case ProductSearchAreaType.NewArrival:
                                    //正常销售的商品(1)一定要排在预售(2)产品前面
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                                    _sortOrders.Add(new SortOrder(ProductSolrField.CreateDate, SolrNet.Order.DESC));
                                    break;
                                //热销品区域
                                case ProductSearchAreaType.BestSeller:
                                    //1.促销商品
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortPromotion, SolrNet.Order.DESC));

                                    //2.热销商品 
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortBestSeller, SolrNet.Order.DESC));

                                    break;
                                //促销
                                case ProductSearchAreaType.Promotion:
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortPromotion, SolrNet.Order.DESC));
                                    break;
                                //专区
                                case ProductSearchAreaType.ProductArea:
                                    //1.促销商品
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortPromotion, SolrNet.Order.DESC));

                                    //2.热销商品 
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortBestSeller, SolrNet.Order.DESC));

                                    //3.普通商品 
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortNormal, SolrNet.Order.DESC));
                                    break;
                                //相似商品
                                case ProductSearchAreaType.SimilarItem:
                                    //1.正常销售的商品(1)一定要排在预售(2)产品前面
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                                    //2.匹配度高的排在前面
                                    _sortOrders.Add(new SortOrder(ProductSolrField.Score, SolrNet.Order.DESC));
                                    //3.创建时间新的排列在前
                                    _sortOrders.Add(new SortOrder(ProductSolrField.CreateDate, SolrNet.Order.DESC));
                                    //4.库存量大的排列在前
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductStock, SolrNet.Order.DESC));
                                    break;
                                case ProductSearchAreaType.DailyDeals: 
                                case ProductSearchAreaType.FeaturedProduct:
                                case ProductSearchAreaType.ClubProduct:
                                case ProductSearchAreaType.Remplacement:
                                case ProductSearchAreaType.FixedPriceArea:
                                case ProductSearchAreaType.Closeout:
                                default:
                                    //1.正常销售的商品(1)一定要排在预售(2)产品前面
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                                    //2.匹配度高的排在前面
                                    _sortOrders.Add(new SortOrder(ProductSolrField.Score, SolrNet.Order.DESC));
                                    //3.创建时间新的排列在前
                                    _sortOrders.Add(new SortOrder(ProductSolrField.CreateDate, SolrNet.Order.DESC));
                                    //4.库存量大的排列在前
                                    _sortOrders.Add(new SortOrder(ProductSolrField.ProductStock, SolrNet.Order.DESC));
                                    break;
                            }

                            break;
                        case ProductSorterCriteria.JoinDateNewToOld:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.JoinDate, SolrNet.Order.DESC));
                            break;
                        case ProductSorterCriteria.JoinDateOldToNew:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.JoinDate, SolrNet.Order.ASC));
                            break;
                        case ProductSorterCriteria.CreateDateNewToOld:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.CreateDate, SolrNet.Order.DESC));
                            break;
                        case ProductSorterCriteria.CreateDateOldToNew:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.CreateDate, SolrNet.Order.ASC));
                            break;
                        case ProductSorterCriteria.Sku:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductCode, SolrNet.Order.ASC));
                            break;
                        case ProductSorterCriteria.LastModifyDate:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.LastModifyDate, SolrNet.Order.DESC));
                            break;
                        case ProductSorterCriteria.SaleCount:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductOrdered, SolrNet.Order.DESC));
                            break;
                        case ProductSorterCriteria.ClickCount:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductViewed, SolrNet.Order.DESC));
                            break;
                        case ProductSorterCriteria.Random:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(string.Format("{0}{1}", ProductSolrField.Random, CommonHelper.RandomInt(1000))));
                            break;
                        default:
                            //正常销售的商品(1)一定要排在预售(2)产品前面
                            _sortOrders.Add(new SortOrder(ProductSolrField.ProductSortStatus, SolrNet.Order.ASC));
                            _sortOrders.Add(new SortOrder(ProductSolrField.Score, SolrNet.Order.DESC));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加额外的过滤方式
        /// </summary>
        /// <param name="solrQuery"></param>
        public void AddExtraFilterQuery(ISolrQuery solrQuery)
        {
            _filterQueries.Add(solrQuery);
        }

        /// <summary>
        /// Solr查询参数q=xxx的xxx部分
        /// </summary>
        public ISolrQuery BaseQuery
        {
            get { return _baseQuery; }
        }

        /// <summary>
        /// 根据设置的条件获取查询选项
        /// </summary>
        public QueryOptions QueryOption
        {
            get
            {
                PageSize = PageSize <= 0 ? 10 : PageSize;

                var option = new QueryOptions
                                 {
                                     Rows = PageSize,
                                     Start = (CurrentPage - 1) * PageSize,
                                     Fields = _fields.ToArray(),
                                 };

                if (SolrConfigHelper.SolrEngine != SolrEngine.Standard && AeaType == ProductSearchAreaType.SearchArea)
                {
                    option.ExtraParams = new Dictionary<string, string>
                     {
                         {SolrConst.Qf,SolrConfigHelper.EDisMaxQf},
                         {SolrConst.Bf,SolrConfigHelper.EDisMaxBf},
                         {SolrConst.DefType,SolrConfigHelper.SolrEngine.ToString().ToLower()}
                     };
                }

                //过滤
                if (!_filterQueries.IsNullOrEmpty())
                {
                    option.FilterQueries = _filterQueries;
                }

                //排序
                if (!_sortOrders.IsNullOrEmpty())
                {
                    option.OrderBy = _sortOrders;
                }
                else
                {
                    option.OrderBy = new List<SortOrder>
                                         {
                                             new SortOrder(ProductSolrField.Score,SolrNet.Order.DESC)
                                         };
                }

                //面统计
                if (!_facetFileds.IsNullOrEmpty())
                {
                    option.Facet = new FacetParameters { Queries = _facetFileds.ToArray(), Limit = -1, MinCount = 1 };
                }

                return option;
            }
        }


        /// <summary>
        /// 组装关键字查询串
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private static ISolrQuery BuildKeywordQuery(string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            keyword = SolrExtendHelper.FilterKeyword(keyword);//过滤掉Solr关键字

            var keywordFromat = SolrConfigHelper.SolrEngine == SolrEngine.Standard ? SolrConfigHelper.SearchKeywordFormat : SolrConfigHelper.EDisMaxSearchKeywordFormat;
            var synonyms = SolrSynonym.GetSynonymsByVerbatim(keyword);

            //没有同义词的情况
            if (synonyms.IsNullOrEmpty())
            {
                return BuildKerywordSolrEngineQuery(string.Format(keywordFromat, SolrExtendHelper.KeywordEncode(keyword)));
            }

            //如果有同义词
            var keywordFormatOfSynonym = SolrConfigHelper.SearchKeywordFormatOfSynonym;
            var query = new StringBuilder(string.Format("({0})", string.Format(keywordFromat, SolrExtendHelper.KeywordEncode(keyword))));
            string synonymQuery;
            foreach (var s in synonyms)
            {
                synonymQuery = string.Format(" OR ({0})", string.Format(keywordFormatOfSynonym, SolrExtendHelper.KeywordEncode(s)));
                if ((query.Length + synonymQuery.Length) > MaxQueryStringLength)
                {
                    break;
                }
                query.Append(synonymQuery);
            }

            return BuildKerywordSolrEngineQuery(query.ToString());
        }

        private static ISolrQuery BuildKerywordSolrEngineQuery(string query)
        {
            return new SolrQuery(string.Format("{0}", query));
        }
    }
}
