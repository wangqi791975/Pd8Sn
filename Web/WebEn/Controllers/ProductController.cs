using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.ClubProduct;
using Com.Panduo.Service.Review;
using Com.Panduo.ServiceImpl.Product.Solr;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Product;
using System.IO;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.SEO;
using Com.Panduo.Web.Models.SEO;

namespace Com.Panduo.Web.Controllers
{
    public class ProductController : BaseController
    {
        #region 产品搜索
        private static bool FilterKeyword(ref string keyword)
        {
            keyword = keyword.Trim();
            if (keyword.Length < 2)
            {
                return false;
            }
            if (keyword.Length > 200)
            {
                keyword = keyword.Substring(0, 200);
            }
            keyword = SolrExtendHelper.FilterKeyword(keyword.Trim().ToLower());
            if (keyword.Length < 2)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 商品搜索页面
        /// </summary>
        [OutputCache(CacheProfile = "ProductCacheProfile", Order = 100)]
        public ActionResult ProductSearch()
        {
            //var start = System.DateTime.Now;
            ViewBag.ProductSearchAreaType = ProductSearchAreaType.SearchArea;//客户当前处于哪个区（后续收集客户数据用）
            #region 关键字处理
            var keyword = Request[UrlParameterKey.ProductSearchKeyword] ?? string.Empty;
            //如果关键字为空，则跳到首页
            if (keyword == null || string.IsNullOrEmpty(keyword.Trim()))
            {
                return Redirect("/");
            }
            keyword = UrlRewriteHelper.FilterUrl(keyword);
            var model = new ProductVo();
            if (!FilterKeyword(ref keyword))
            {
                model.ProductInfo = new PageData<ProductInfo>();
                model.ProductInfo.Data = new List<ProductInfo>();
                model.ProductInfo.Pager = new Pager(0, 1, 60);
                goto lab_return;
            }
            if (KeywordRedirect.Instance.Redirect(keyword)) return null; //搜索特定关键字跳转如:hello kitty=>http://www.baidu.com
            int page = Request[UrlParameterKey.Page].ParseTo(1);//当前页
            page = page > 0 ? page : 1;
            #endregion

            #region 类别处理
            int categoryId;
            int.TryParse(Request[UrlParameterKey.CategoryId], out categoryId);
            Category category = new Category();
            if (categoryId > 0)
            {
                category = ServiceFactory.CategoryService.GetCategoryById(categoryId);
                if (category == null)
                {
                    return Redirect("/");
                }
            }
            #endregion

            //输入产品Code 跳到详情页
            var productById = ServiceFactory.ProductService.GetProductByCode(keyword.Trim());
            if (!productById.IsNullOrEmpty())
            {
                if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetProductDetailUrl(productById.ProductId, ServiceFactory.ProductService.GetProductNameByEn(productById.ProductId)), new List<string> { UrlParameterKey.ProductSearchKeyword, UrlParameterKey.Page }))
                {
                    return null;
                }
            }

            int pageSize = Request[UrlParameterKey.PageSize].ParseTo(-1);//页大小
            SetPageSize(pageSize);
            pageSize = PageHelper.GetCustomerPageSize();

            PageData<Product> pageData = GetProductItems(page, pageSize, categoryId, keyword, ProductSearchAreaType.SearchArea);
            page = pageData.Pager.PageCount >= page ? page : pageData.Pager.PageCount;
            if (UrlRewriteHelper.CheckSearchKeywordAndRedirect(keyword, page)) return null;

            //左侧类别树
            var categoryTree = new CategoryTreeVo();
            categoryTree.TreeType = CategoryTreeVo.CategoryTreeType.ProductSearch;
            categoryTree.CategoryRelatedDatas = null;//直接从搜索返回的结果里调取类别
            categoryTree.CurrentCategoryId = categoryId;
            categoryTree.CurrentParentCategoryId = category.ParentId;

            var viewMode = Request[UrlParameterKey.ViewMode] ?? "-1";
            SetShowMode(viewMode.ParseTo(-1));
            ViewBag.ShowMode = PageHelper.GetCustomerShowType();

            //model.Product = pageData;
            model.ProductInfo = new PageData<ProductInfo>();
            model.ProductInfo.Data = ServiceFactory.ProductService.GetProductInfos(pageData.Data, isIncludeProductStock: true, isIncludeProductImage: false,
                isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: true);
            model.ProductInfo.Pager = pageData.Pager;
            ViewBag.CategoryTree = categoryTree;
            ViewBag.Page = page;
            //var end = System.DateTime.Now;
            //ViewBag.time = DateTime.Now.Subtract(start).TotalSeconds;
            ViewBag.Sitemaps = Sitemap.GetProductSearchSitemap(keyword);
            SetProductListMetaInfo(category, MetaListPageType.Search, page, pageData.Pager.PageCount, ViewBag.ShowMode, keyword);
        lab_return:
            ViewBag.Keyword = keyword;
            ViewBag.ZoneName = string.Format("Search:{0}", keyword);
            SessionHelper.LastShoppingUrl = Request.RawUrl;
            return View(model);
        }

        #endregion

        public ActionResult Detail(int productId)
        {
            #region 判断产品是否存在
            if (productId < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var productById = ServiceFactory.ProductService.GetProductById(productId);
            if (productById.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            //下架产品跳出
            if (productById.Status == ProductStatus.OffLine || productById.Status == ProductStatus.Delete)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            #endregion
            if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetProductDetailUrl(productId, ServiceFactory.ProductService.GetProductNameByEn(productId))))
            {
                return null;
            }

            var detailVo = new ProductDetailVo();
            var productLanguage = ServiceFactory.ProductService.GetProductLanguage(productId, ServiceFactory.ConfigureService.SiteLanguageId);
            if (!productLanguage.IsNullOrEmpty() && !productLanguage.MarketingTitle.IsNullOrEmpty())
            {
                detailVo.ProductMarketingTitle = productLanguage.MarketingTitle;
            }
            else
            {
                var adinfo = ServiceFactory.CategoryService.GetCategoryAdvertisement(productById.CategoryId,
                    ServiceFactory.ConfigureService.SiteLanguageId);
                if (!adinfo.IsNullOrEmpty())
                    detailVo.ProductMarketingTitle = adinfo.MarketingTitle;
            }

            detailVo.CategoryAdvertisement =
                ServiceFactory.CategoryService.GetCategoryAdvertisement(productById.CategoryId,
                    ServiceFactory.ConfigureService.SiteLanguageId);

            //detailVo.MarketingArea = ServiceFactory.CategoryService.GetCategoryProductMarketingArea(productById.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            var productinfos = ServiceFactory.ProductService.GetProductInfos(new List<Product>() { productById },
                                                                             isIncludeProductStock: true,
                                                                             isIncludeProductImage: true,
                                                                             isIncludeProductProperty: true,
                                                                             isIncludeProductPrice: true,
                                                                             isJudgeHotSeller: true,
                                                                             isJudgeHasSimilarProuct: true);
            if (!productinfos.IsNullOrEmpty())
            {
                detailVo.ProductInfo = productinfos[0];
            }
            if (productById.IsOtherPack)
            { //判断是否有大小包装
                var hasOther = ServiceFactory.ProductService.GetOtherPackings(productId);
                productById.IsOtherPack = !hasOther.IsNullOrEmpty();
                if (productById.IsOtherPack)
                {
                    detailVo.OtherPack = ServiceFactory.ProductService.GetProductInfos(hasOther,
                                                                            isIncludeProductStock: true,
                                                                            isIncludeProductImage: true,
                                                                            isIncludeProductProperty: true,
                                                                            isIncludeProductPrice: true,
                                                                            isJudgeHotSeller: true,
                                                                            isJudgeHasSimilarProuct: true);
                }

            }

            Category productCategory = ServiceFactory.CategoryService.GetCategoryById(productById.CategoryId);
            //左侧类别树
            var categoryTree = new CategoryTreeVo();
            categoryTree.TreeType = CategoryTreeVo.CategoryTreeType.ProductDatail;
            categoryTree.CategoryRelatedDatas = CacheHelper.CategoryLanguages;
            categoryTree.CurrentCategoryId = productById.CategoryId;
            categoryTree.CurrentParentCategoryId = productCategory != null ? productCategory.ParentId : 0;

            //产品绑定属性值 
            var propertyandpropertyvalueList = detailVo.ProductInfo == null ? new List<KeyValuePair<Service.Product.Property.Property, Service.Product.Property.PropertyValue>>() : detailVo.ProductInfo.ProductProperties;

            detailVo.ProductPropertyAndPropertyValue = propertyandpropertyvalueList.OrderBy(c => c.Key.DisplayOrder).ThenBy(c => c.Key.PropertyName).Select(c => new KeyValuePair<string, string>(c.Key.PropertyName, c.Value.PropertyValueName)).ToList();
            //获取产品评分分值和数量
            var rating = ServiceFactory.ReviewService.GetRatingByProductId(productId);
            ViewBag.RatingAvg = rating[Rating.AvgScore];
            ViewBag.RatingCount = rating[Rating.ReviewCount];
            ViewBag.CategoryTree = categoryTree;

            #region Meta信息
            SetProductDetailMetaInfo(productById, productLanguage, detailVo);
            #endregion

            WriteViewdProduct(productId);
            SessionHelper.LastShoppingUrl = Request.RawUrl;
            return View(detailVo);
        }

        public ActionResult StaticHtml(string viewName)
        {
            return View(viewName);
        }

        private void SetShowMode(int mode)
        {
            int type = 0;
            var p = SessionHelper.CurrentCustomerPreference;
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                if (!p.IsNullOrEmpty() && p.ListShowType.HasValue)
                {
                    switch (p.ListShowType.Value)
                    {
                        case ListShowType.Gallery:
                            type = 1;
                            break;
                        case ListShowType.List:
                            type = 0;
                            break;
                    }
                    if (type != mode && mode != -1)
                    {
                        p.ListShowType = mode.ToEnum<ListShowType>();
                        SessionHelper.CurrentCustomerPreference = p;
                        //ServiceFactory.CustomerService.SetPreference(p);
                    }
                }
            }
        }

        private void SetPageSize(int pageSize)
        {
            var sessionpageSize = 60;//默认60
            var p = SessionHelper.CurrentCustomerPreference;
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                if (!p.IsNullOrEmpty() && p.ListShowCount.HasValue)
                {
                    switch (p.ListShowCount.Value)
                    {
                        case ListShowCount.T:
                            sessionpageSize = 30;
                            break;
                        case ListShowCount.S:
                            sessionpageSize = 60;
                            break;
                        case ListShowCount.N:
                            sessionpageSize = 90;
                            break;
                    }
                    if (sessionpageSize != pageSize && pageSize != -1)
                    {
                        p.ListShowCount = pageSize.ToEnum<ListShowCount>();
                        SessionHelper.CurrentCustomerPreference = p;
                        //  ServiceFactory.CustomerService.SetPreference(p);
                    }
                }
            }
        }

        [OutputCache(CacheProfile = "ProductCacheProfile", Order = 100)]
        public ActionResult ProductList(int categoryId, int page)
        {
            ViewBag.ProductSearchAreaType = ProductSearchAreaType.NormalArea;//客户当前处于哪个区（后续收集客户数据用）
            if (categoryId < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var category = ServiceFactory.CategoryService.GetCategoryById(categoryId);
            if (category.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var categoryLanguage = ServiceFactory.CategoryService.GetCategoryLanguageById(categoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            if (categoryLanguage.IsNullOrEmpty())
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);


            int pageSize = Request[UrlParameterKey.PageSize].ParseTo(-1);//页大小
            var viewMode = Request[UrlParameterKey.ViewMode] ?? "-1";
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                SetPageSize(pageSize);
                pageSize = PageHelper.GetCustomerPageSize();
                SetShowMode(viewMode.ParseTo(-1));
                ViewBag.ShowMode = PageHelper.GetCustomerShowType();
            }
            else
            {
                pageSize =pageSize==-1?60:pageSize.ParseTo<int>(60);
                ViewBag.ShowMode = (viewMode == "1" ? true : false);
            }



            page = page > 0 ? page : 1;
            PageData<Product> pageData = GetProductItems(page, pageSize, categoryId, string.Empty, ProductSearchAreaType.NormalArea);
            page = pageData.Pager.PageCount >= page ? page : pageData.Pager.PageCount;
            if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetProductListUrl(categoryId, categoryLanguage.CategoryEnglishName, page)))
            {
                return null;
            }

            //左侧类别树
            var categoryTree = new CategoryTreeVo();
            categoryTree.TreeType = CategoryTreeVo.CategoryTreeType.ProductList;
            categoryTree.CategoryRelatedDatas = CacheHelper.CategoryLanguages;
            categoryTree.CurrentCategoryId = category.CategoryId;
            categoryTree.CurrentParentCategoryId = category.ParentId;

            var model = new ProductVo();
            model.Category = category;
            model.CategoryLanguage = categoryLanguage;
            model.CategoryAdvertisement = ServiceFactory.CategoryService.GetCategoryAdvertisement(categoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            model.IsRoot = ServiceFactory.CategoryService.IsRootCategory(categoryId);
            model.IsLeaf = ServiceFactory.CategoryService.IsLeafCategory(categoryId);
            model.ProductInfo = new PageData<ProductInfo>();
            model.ProductInfo.Data = ServiceFactory.ProductService.GetProductInfos(pageData.Data, isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: true);
            model.ProductInfo.Pager = pageData.Pager;

            if (model.IsRoot)//读取hot Items
            {
                var list = ServiceFactory.ProductService.SearchProducts(1, 10,
                    new Dictionary<ProductSearchCriteria, object>()
                    {
                        {ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.BestSeller},
                        {ProductSearchCriteria.CategoryId, category.CategoryId}
                    }, new List<Sorter<ProductSorterCriteria>>(), false, false);
                var productList = list.ProductPageData.Data;
                var productinfos = ServiceFactory.ProductService.GetProductInfos(productList,
                    isIncludeProductStock: true,
                    isIncludeProductImage: false,
                    isIncludeProductProperty: false,
                    isIncludeProductPrice: true,
                    isJudgeHotSeller: true,
                    isJudgeHasSimilarProuct: false);
                model.HotItems = new List<ProductInfo>();
                model.HotItems = productinfos;
            }

            ViewBag.CategoryTree = categoryTree;
            ViewBag.ZoneName = model.CategoryLanguage.CategoryLanguageName;//分页条显示名称，当前的类别名称/专区名称/搜索关键词
            ViewBag.Page = page;

            #region Meta信息
            SetProductListMetaInfo(category, MetaListPageType.Home, page, pageData.Pager.PageCount, ViewBag.ShowMode, string.Empty);
            ViewBag.Sitemaps = Sitemap.GetProductListSitemap(categoryId);
            #endregion

            SessionHelper.LastShoppingUrl = Request.RawUrl;
            return View(model);
        }

        /// <summary>
        /// 大小包装弹窗内容,ajax调用
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns></returns>
        public ActionResult OtherPackage(int id)
        {
            var p = new ProductPackVo();
            var product = ServiceFactory.ProductService.GetProductById(id);
            if (!product.IsNullOrEmpty() & product.IsOtherPack)
            {
                var otherpack = ServiceFactory.ProductService.GetOtherPackings(id);

                p.OtherPack = ServiceFactory.ProductService.GetProductInfos(otherpack,
                                                                             isIncludeProductStock: true,
                                                                             isIncludeProductImage: true,
                                                                             isIncludeProductProperty: true,
                                                                             isIncludeProductPrice: true,
                                                                             isJudgeHotSeller: true,
                                                                             isJudgeHasSimilarProuct: true);
            }
            var productInfos = ServiceFactory.ProductService.GetProductInfos(new List<Product>() { product },
                                                                               isIncludeProductStock: true,
                                                                               isIncludeProductImage: true,
                                                                               isIncludeProductProperty: true,
                                                                               isIncludeProductPrice: true,
                                                                               isJudgeHotSeller: true,
                                                                               isJudgeHasSimilarProuct: true);
            if (productInfos.Count == 1)
            {
                p.ProductInfo = productInfos[0];
            }
            return View(p);
        }

        #region ProductDetail 推荐商品AJAX
        /// <summary>
        /// 产品详细页面相似商品(异步调用)
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ProductDetailSimilar()
        {
            int productId = Request["productId"].ParseTo(0);//产品ID
            var similarProductList = ServiceFactory.ProductService.GetSimilarProductTopNById(productId, 6);
            var productinfos = ServiceFactory.ProductService.GetProductInfos(similarProductList,
                                                                        isIncludeProductStock: true,
                                                                        isIncludeProductImage: false,
                                                                        isIncludeProductProperty: false,
                                                                        isIncludeProductPrice: true,
                                                                        isJudgeHotSeller: true,
                                                                        isJudgeHasSimilarProuct: false);
            var model = new RecommendItemVo() { ProductInfo = productinfos, MainProductId = productId };
            return View(model);
        }

        /// <summary>
        ///  产品详细页面BestMatch商品(异步调用)
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ProductDetailBestMatch()
        {
            int productId = Request["productId"].ParseTo(0);//产品ID
            var similarProductList = ServiceFactory.ProductService.GetMatchProductTopNById(productId, 6);
            var productinfos = ServiceFactory.ProductService.GetProductInfos(similarProductList,
                                                                        isIncludeProductStock: true,
                                                                        isIncludeProductImage: false,
                                                                        isIncludeProductProperty: false,
                                                                        isIncludeProductPrice: true,
                                                                        isJudgeHotSeller: true,
                                                                        isJudgeHasSimilarProuct: false);
            var model = new RecommendItemVo() { ProductInfo = productinfos, MainProductId = productId };
            return View(model);
        }

        /// <summary>
        /// 产品详细页面买了又买商品(异步调用)
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductDetailAlsoBuy()
        {
            int productId = Request["productId"].ParseTo(0);//产品ID
            var similarProductList = ServiceFactory.ProductService.GetAlsoBuyProductsTopNById(productId, 5);
            var productinfos = ServiceFactory.ProductService.GetProductInfos(similarProductList,
                                                                           isIncludeProductStock: true,
                                                                           isIncludeProductImage: false,
                                                                           isIncludeProductProperty: false,
                                                                           isIncludeProductPrice: true,
                                                                           isJudgeHotSeller: true,
                                                                           isJudgeHasSimilarProuct: false);
            var model = new RecommendItemVo() { ProductInfo = productinfos, MainProductId = productId };
            return View(model);
        }

        /// <summary>
        /// 产品详细页面最近浏览商品(异步调用)
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductDetailRecentViewed()
        {
            int id = Request["productId"].ParseTo(0);//产品ID
            var productList = new List<Product>();
            var recentlyViewedProductIdList = CookieHelper.RecentlyViewedProductList;
            if (!recentlyViewedProductIdList.IsNullOrEmpty())
            {
                foreach (var productId in recentlyViewedProductIdList)
                {
                    if (!(productId == id))
                    {
                        var product = ServiceFactory.ProductService.GetProductById(productId);
                        if (!product.IsNullOrEmpty())
                        {
                            productList.Add(product);
                        }
                    }
                }
            }
            if (productList.Count == ServiceFactory.ConfigureService.RecentlyViewedMaxCount)
            {
                productList.RemoveAt(productList.Count - 1);
            }
            var productinfoList = ServiceFactory.ProductService.GetProductInfos(productList,
                                                                   isIncludeProductStock: true,
                                                                   isIncludeProductImage: false,
                                                                   isIncludeProductProperty: false,
                                                                   isIncludeProductPrice: true,
                                                                   isJudgeHotSeller: true,
                                                                   isJudgeHasSimilarProuct: false);
            return View(productinfoList);
        }

        /// <summary>
        /// 产品浏览记录
        /// </summary>
        /// <param name="pId">产品Id</param>
        private void WriteViewdProduct(int pId)
        {
            var viewd = CookieHelper.RecentlyViewedProductList;
            if (viewd.IsNullOrEmpty())
            {
                viewd = new List<int>() { pId };
            }
            else
            {
                if (viewd.LastIndexOf(pId) >= 0)
                {
                    viewd.RemoveAt(viewd.LastIndexOf(pId));
                    viewd.Insert(0, pId);
                }
                else
                {
                    if (viewd.Count >= ServiceFactory.ConfigureService.RecentlyViewedMaxCount)
                    {
                        viewd.RemoveAt(viewd.Count - 1);
                    }
                    viewd.Insert(0, pId);
                }

            }
            CookieHelper.RecentlyViewedProductList = viewd;
        }

        #endregion

        /// <summary>
        /// 相似商品
        /// </summary>
        /// <returns></returns>
        [OutputCache(CacheProfile = "ProductCacheProfile", Order = 100)]
        public ActionResult SimilarItems(int productId, int page)
        {
            if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetSimiliarItemsUrl(productId, page)))
            {
                return null;
            }
            int pageSize = Request[UrlParameterKey.PageSize].ParseTo(-1);//页大小
            var viewMode = Request[UrlParameterKey.ViewMode] ?? "-1";
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                SetPageSize(pageSize);
                pageSize = PageHelper.GetCustomerPageSize();
                SetShowMode(viewMode.ParseTo(-1));
                ViewBag.ShowMode = PageHelper.GetCustomerShowType();
            }
            else
            {
                pageSize = pageSize == -1 ? 60 : pageSize.ParseTo<int>(60);
                ViewBag.ShowMode = (viewMode == "1" ? true : false);
            }

            if (productId < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var product = ServiceFactory.ProductService.GetProductById(productId);
            if (product.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var productinfos = ServiceFactory.ProductService.GetProductInfos(new List<Product>() { product },
                                                                         isIncludeProductStock: true,
                                                                         isIncludeProductImage: false,
                                                                         isIncludeProductProperty: false,
                                                                         isIncludeProductPrice: true,
                                                                         isJudgeHotSeller: true,
                                                                         isJudgeHasSimilarProuct: false);
            //var similarProducts = ServiceFactory.ProductService.GetSimilarProductById(productId, page, pageSize);

            PageData<Product> pageData = GetProductItems(page, pageSize, null, null, ProductSearchAreaType.SimilarItem, productId);

            page = pageData.Pager.PageCount >= page ? page : pageData.Pager.PageCount;
            if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetSimiliarItemsUrl(productId, page)))
            {
                return null;
            }

            var similarItem = new SimilarItemVo();
            similarItem.SimilarProductInfo = new PageData<ProductInfo>();
            similarItem.ProductInfo = productinfos[0];

            var similarProductinfos = ServiceFactory.ProductService.GetProductInfos(pageData.Data,
                                                                         isIncludeProductStock: true,
                                                                         isIncludeProductImage: false,
                                                                         isIncludeProductProperty: false,
                                                                         isIncludeProductPrice: true,
                                                                         isJudgeHotSeller: true,
                                                                         isJudgeHasSimilarProuct: true);
            if (!similarProductinfos.IsNullOrEmpty())
            {
                similarItem.SimilarProductInfo.Data = similarProductinfos;
                similarItem.SimilarProductInfo.Pager = pageData.Pager;
            }

            var fullUrl = UrlFuncitonHelper.GetFullUrl();
            var propertyValueList = UrlFuncitonHelper.GetPropertyValueList(fullUrl);//属性值ID
            var propertyValueGroupList = UrlFuncitonHelper.GetPropertyValueGroupList(fullUrl);//属性值组ID

            ViewBag.FullUrl = fullUrl;
            ViewBag.CurrentyUrlExcludeProperty = UrlFuncitonHelper.RemovePropertyValueAndGroup(fullUrl);
            ViewBag.PropertyValueList = propertyValueList;
            ViewBag.PropertyValueGroupList = propertyValueGroupList;
            ViewBag.Sitemaps = Sitemap.GetSimilarItemAreaSitemap(UrlRewriteHelper.SimiliarItemsAreaUrl, product.ProductCode, product.ProductId, ServiceFactory.ProductService.GetProductNameByEn(product.ProductId));
            return View(similarItem);
        }

        /// <summary>
        /// BestMatch商品
        /// </summary>
        /// <returns></returns>
        [OutputCache(CacheProfile = "ProductCacheProfile", Order = 100)]
        public ActionResult BestMatch(int productId, int page)
        {
            if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetBestMatchUrl(productId, page)))
            {
                return null;
            }

            int pageSize = Request[UrlParameterKey.PageSize].ParseTo(-1);//页大小
            var viewMode = Request[UrlParameterKey.ViewMode] ?? "-1";
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                SetPageSize(pageSize);
                pageSize = PageHelper.GetCustomerPageSize();
                SetShowMode(viewMode.ParseTo(-1));
                ViewBag.ShowMode = PageHelper.GetCustomerShowType();
            }
            else
            {
                pageSize = pageSize == -1 ? 60 : pageSize.ParseTo<int>(60);
                ViewBag.ShowMode = (viewMode == "1" ? true : false);
            }

            if (productId < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var product = ServiceFactory.ProductService.GetProductById(productId);
            if (product.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var productinfos = ServiceFactory.ProductService.GetProductInfos(new List<Product>() { product },
                                                                         isIncludeProductStock: true,
                                                                         isIncludeProductImage: false,
                                                                         isIncludeProductProperty: false,
                                                                         isIncludeProductPrice: true,
                                                                         isJudgeHotSeller: true,
                                                                         isJudgeHasSimilarProuct: false);
            //var similarProducts = ServiceFactory.ProductService.GetSimilarProductById(productId, page, pageSize);

            PageData<Product> pageData = GetProductItems(page, pageSize, null, null, ProductSearchAreaType.BestMatch, productId);

            page = pageData.Pager.PageCount >= page ? page : pageData.Pager.PageCount;
            if (UrlRewriteHelper.CompareAndRedirect(UrlRewriteHelper.GetBestMatchUrl(productId, page)))
            {
                return null;
            }

            var similarItem = new SimilarItemVo();
            similarItem.SimilarProductInfo = new PageData<ProductInfo>();
            similarItem.ProductInfo = productinfos[0];

            var similarProductinfos = ServiceFactory.ProductService.GetProductInfos(pageData.Data,
                                                                         isIncludeProductStock: true,
                                                                         isIncludeProductImage: false,
                                                                         isIncludeProductProperty: false,
                                                                         isIncludeProductPrice: true,
                                                                         isJudgeHotSeller: true,
                                                                         isJudgeHasSimilarProuct: true);
            if (!similarProductinfos.IsNullOrEmpty())
            {
                similarItem.SimilarProductInfo.Data = similarProductinfos;
                similarItem.SimilarProductInfo.Pager = pageData.Pager;
            }

            var fullUrl = UrlFuncitonHelper.GetFullUrl();
            var propertyValueList = UrlFuncitonHelper.GetPropertyValueList(fullUrl);//属性值ID
            var propertyValueGroupList = UrlFuncitonHelper.GetPropertyValueGroupList(fullUrl);//属性值组ID

            ViewBag.FullUrl = fullUrl;
            ViewBag.CurrentyUrlExcludeProperty = UrlFuncitonHelper.RemovePropertyValueAndGroup(fullUrl);
            ViewBag.PropertyValueList = propertyValueList;
            ViewBag.PropertyValueGroupList = propertyValueGroupList;
            ViewBag.Sitemaps = Sitemap.GetSimilarItemAreaSitemap(UrlRewriteHelper.BestMatchAreaUrl, product.ProductCode, product.ProductId, ServiceFactory.ProductService.GetProductNameByEn(product.ProductId));
            return View(similarItem);
        }


        public PageData<Product> GetProductItems(int currentPage, int pageSize, int? categoryId, string keyword, ProductSearchAreaType searchAreaType, int? productId = null)
        {
            var fullUrl = UrlFuncitonHelper.GetFullUrl();
            var propertyValueList = UrlFuncitonHelper.GetPropertyValueList(fullUrl);//属性值ID
            var propertyValueGroupList = UrlFuncitonHelper.GetPropertyValueGroupList(fullUrl);//属性值组ID

            ViewBag.FullUrl = fullUrl;
            ViewBag.CurrentyUrlExcludeProperty = UrlFuncitonHelper.RemovePropertyValueAndGroup(fullUrl);
            ViewBag.PropertyValueList = propertyValueList;
            ViewBag.PropertyValueGroupList = propertyValueGroupList;

            var search = new Dictionary<ProductSearchCriteria, object>() { };

            if (ProductSearchAreaType.SimilarItem == searchAreaType)
            {
                search.Add(ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.SimilarItem);
                if (productId.HasValue && productId > 0)
                {
                    search.Add(ProductSearchCriteria.SimilarProductId, productId.Value);
                }
            }
            else if (ProductSearchAreaType.BestMatch == searchAreaType)
            {
                search.Add(ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.BestMatch);
                if (productId.HasValue && productId > 0)
                {
                    search.Add(ProductSearchCriteria.BestMatchProductId, productId.Value);
                }
            }
            else if (ProductSearchAreaType.SearchArea == searchAreaType)
            {
                search.Add(ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.SearchArea);
            }
            else
            {
                search.Add(ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.NormalArea);
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                search.Add(ProductSearchCriteria.CategoryPath, categoryId.Value);
            }
            if (!keyword.IsNullOrEmpty())
            {
                search.Add(ProductSearchCriteria.Keyword, keyword);
            }
            if (!propertyValueList.IsNullOrEmpty())
            {
                search.Add(ProductSearchCriteria.PropertyValueIds, propertyValueList);
            }
            if (!propertyValueGroupList.IsNullOrEmpty())
            {
                search.Add(ProductSearchCriteria.PropertyValueGroupIds, propertyValueGroupList);
            }

            #region onSale&bestSeller&inStock
            var onSale = Request[UrlParameterKey.OnSale] ?? "0";
            if (onSale == "1")
            {
                search.Add(ProductSearchCriteria.IsOnSale, true);
            }

            var bestSeller = Request[UrlParameterKey.BestSeller] ?? "0";
            if (bestSeller == "1")
            {
                search.Add(ProductSearchCriteria.IsBestSeller, true);
            }
            var inStock = Request[UrlParameterKey.InStock] ?? "0";
            if (inStock == "1")
            {
                search.Add(ProductSearchCriteria.IsInStock, true);
            }
            #endregion

            var sorter = new List<Sorter<ProductSorterCriteria>>();
            var sort = Request[UrlParameterKey.Sort] ?? "1";
            //sort，对应的网站的排序筛选sort by。目前已知的有四种排序：1:best match，2:商品价格从低到高，3:商品价格从高到低，4:添加时间从新到旧
            switch (sort)
            {
                case "1":
                    sorter.Add(new Sorter<ProductSorterCriteria> { Key = ProductSorterCriteria.BestMatch, IsAsc = true });
                    break;
                case "2":
                    sorter.Add(new Sorter<ProductSorterCriteria> { Key = ProductSorterCriteria.PriceLowToHigh, IsAsc = true });
                    break;
                case "3":
                    sorter.Add(new Sorter<ProductSorterCriteria> { Key = ProductSorterCriteria.PriceHighToLow, IsAsc = true });
                    break;
                case "4":
                    sorter.Add(new Sorter<ProductSorterCriteria> { Key = ProductSorterCriteria.CreateDateNewToOld, IsAsc = true });
                    break;
                default://默认排序
                    sorter.Add(new Sorter<ProductSorterCriteria> { Key = ProductSorterCriteria.BestMatch, IsAsc = true });
                    break;

            }
            var list = ServiceFactory.ProductService.SearchProducts(currentPage, pageSize, search, sorter, true, true);
            ViewBag.ProductProperties = list.ProductProperties;
            ViewBag.ProductCategories = list.ProductCategories;
            return list.ProductPageData;

        }

        #region 客户评论
        public ActionResult ProductDetailCustomerReview(int productId)
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(20);//页大小

            PageData<ReviewProductCustomerView> productPageData = ServiceFactory.ReviewService.FindCustomerProductReviewsByProductId(page, pageSize, null, null, productId);

            ViewBag.ProductId = productId;
            return View(productPageData);
        }

        [HttpPost]
        public ActionResult WriteReview(int productId, int rating, string name, string rviewcontent)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            try
            {
                int customerId = 0;
                if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
                    customerId = SessionHelper.CurrentCustomer.CustomerId;
                var reviewProductView = ServiceFactory.ReviewService.GetNotReviewProductView(customerId, productId);
                if (!reviewProductView.IsNullOrEmpty())
                {
                    var productReview = new ProductReview
                    {
                        CustomerId = customerId,
                        OrderProductId = reviewProductView.OrderProductId,
                        ProductId = productId,
                        LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                        Rating = rating,
                        ReviewContent = rviewcontent,
                        CreateDateTime = DateTime.Now,
                        AuditStatus = AuditStatus.AuditPass,
                        IsValid = true
                    };
                    ServiceFactory.ReviewService.AddProductReview(productReview);
                    //todo 发送邮件（产品评论发送邮件给销售）
                    //var customer = SessionHelper.CurrentCustomer;
                    //if (!customer.IsNullOrEmpty())
                    //    ServiceFactory.MailService.ReviewEmail(customer.FullName, customer.Email, DateTime.Now, rviewcontent);
                }
                else
                {
                    hashtable["error"] = true;
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {

                }
                hashtable["error"] = true;
            }
            return Json(hashtable);
        }

        [HttpPost]
        public JsonResult CheckProductReivew(int productId)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            int customerId = 0;
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
                customerId = SessionHelper.CurrentCustomer.CustomerId;

            if (ServiceFactory.ReviewService.GetNotReviewProductView(customerId, productId).IsNullOrEmpty())
            {
                hashtable["error"] = true;
            }
            return Json(hashtable);
        }
        #endregion

        #region ask a question
        /// <summary>
        /// ask a question 发送邮件
        /// </summary>
        [HttpPost]
        public void EmailUs()
        {
            string email = Request["email"] ?? string.Empty;//email
            string fullname = Request["fullname"] ?? string.Empty;//fullname
            string question = Request["question"] ?? string.Empty;//question
            string attachmentfile = Request["attachmentfile"] ?? string.Empty;
            ServiceFactory.MailService.AskQuestionEmail(fullname, email, question, attachmentfile);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Upload(HttpPostedFileBase fileData, string folder)
        {
            string extend = "";
            if (null != fileData)
            {
                try
                {
                    string filename = Path.GetFileName(fileData.FileName);
                    extend = Guid.NewGuid() + filename.Substring(filename.LastIndexOf('.'),
                                                      filename.Length - filename.LastIndexOf('.'));
                    saveFile(fileData, extend, UploadFileType.CustomerAskQuestion);
                }
                catch (Exception ex)
                {
                    extend = ex.ToString();
                }
            }
            return Content(extend);
        }

        [NonAction]
        private void saveFile(HttpPostedFileBase postedFile, string saveName, UploadFileType uploadFileType)
        {
            string dirFolder = UploadFileHelper.GetUploadFileSavePath(uploadFileType);
            //创建顶级目录 
            if (!Directory.Exists(dirFolder))
            {
                Directory.CreateDirectory(dirFolder);
            }
            try
            {
                postedFile.SaveAs(Path.Combine(dirFolder, saveName));
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }
        #endregion

        #region Product一口价

        public ActionResult ProductDailyDeal()
        {
            var languageId = PageHelper.GetCurrentLanguage().IsNullOrEmpty() ? ServiceFactory.ConfigureService.SiteLanguageId : PageHelper.GetCurrentLanguage().LanguageId;
            ViewBag.DailyDeals = ServiceFactory.ProductDailyDealService.GetValidDailyDealProducts(languageId);
            var desc = ServiceFactory.ProductDailyDealService.GetDailydealDesc(languageId);
            return View(desc);
        }

        #endregion

        #region 获取Meta信息
        /// <summary>
        /// 设置详情页的Meta信息
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="productLanguage">产品语言</param>
        /// <param name="detailVo">VO</param>
        private void SetProductDetailMetaInfo(Product product, ProductLanguage productLanguage, ProductDetailVo detailVo)
        {
            var metaProductDetail = ServiceFactory.MetaService.GetMetaHomeByType(MetaHomePageType.ProductDetail, ServiceFactory.ConfigureService.SiteLanguageId);
            var code = product.ProductCode;
            if (!metaProductDetail.IsNullOrEmpty())
            {
                code = metaProductDetail.Breadcrumb.IsNullOrEmpty() ? code : metaProductDetail.Breadcrumb.Replace("{product_code}", code);
                var parentCategory = ServiceFactory.CategoryService.GetCategoryLanguageById(product.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId);
                var parentCategoryName = parentCategory.CategoryLanguageName ?? string.Empty;
                var propertyValues = detailVo.ProductPropertyAndPropertyValue.Aggregate(string.Empty, (current, pv) => current + ((current.IsNullOrEmpty() ? "" : ",") + pv.Value + " " + parentCategoryName));
                ViewBag.MetaInfo = new MetaInfoVo
                {
                    Title = metaProductDetail.Title.Replace("{product_name}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(productLanguage.ProductName)),
                    Keywords = metaProductDetail.Keywords.Replace("{category_name}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(parentCategoryName)).Replace("{attribute_list}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(propertyValues)),
                    Description = metaProductDetail.Description.Replace("{product_name}", productLanguage.ProductName.ToLower())
                };
            }
            ViewBag.Sitemaps = Sitemap.GetProductDetailSitemap(product.CategoryId, code);
        }

        /// <summary>
        /// 设置列表首页SEO信息
        /// </summary>
        /// <param name="homePageType">页面类型</param>
        private void SetProductListOtherInfo(MetaHomePageType homePageType)
        {
            var metaHome = ServiceFactory.MetaService.GetMetaHomeByType(MetaHomePageType.ProductDetail, ServiceFactory.ConfigureService.SiteLanguageId);
            if (!metaHome.IsNullOrEmpty())
            {
                ViewBag.MetaInfo = new MetaInfoVo
                {
                    Title = metaHome.Title,
                    Keywords = metaHome.Keywords,
                    Description = metaHome.Description
                };
            }
        }

        /// <summary>
        /// 设置列表的Meta信息
        /// </summary>
        /// <param name="category">当前类别</param>
        /// <param name="listPageType">页面类型</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="showMode">页面显示也行</param>
        /// <param name="keyword">搜索关键字</param>
        private void SetProductListMetaInfo(Category category, MetaListPageType listPageType, int page, int pageCount, bool showMode, string keyword)
        {
            var metaList = ServiceFactory.MetaService.GetMetaListByType(category.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId, listPageType) ?? new MetaList();
            var categoryLanguage = ServiceFactory.CategoryService.GetCategoryLanguageById(category.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId) ?? new CategoryLanguage();
            var allParentLanguage = ServiceFactory.CategoryService.GetCategoryLanguageFamliy(category.ParentId, ServiceFactory.ConfigureService.SiteLanguageId);
            var parentCategory = allParentLanguage.LastOrDefault() ?? new CategoryLanguage();
            var topCategory = allParentLanguage.FirstOrDefault() ?? new CategoryLanguage();
            var parentMetaList = ServiceFactory.MetaService.GetMetaListByType(parentCategory.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId, listPageType) ?? new MetaList();
            var topMetaList = ServiceFactory.MetaService.GetMetaListByType(topCategory.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId, listPageType) ?? new MetaList();
            var propertyValueList = UrlFuncitonHelper.GetPropertyValueList(UrlFuncitonHelper.GetFullUrl());
            var propertyValueNames = string.Empty;
            foreach (var propertyValueId in propertyValueList)
            {
                var propertyValueLanguage = ServiceFactory.PropertyService.GetPropertyValueLanguage(propertyValueId, ServiceFactory.ConfigureService.SiteLanguageId);
                propertyValueNames += (propertyValueNames.IsNullOrEmpty() ? "" : " ") + (propertyValueLanguage.IsNullOrEmpty() ? string.Empty : propertyValueLanguage.PropertyValueName);
            }

            IDictionary<string, string> dic = new Dictionary<string, string>
            {
                {"{1}", !metaList.Alias.IsNullOrEmpty() ? metaList.Alias : (categoryLanguage.CategoryLanguageName ?? string.Empty)},
                {"{2}", !parentMetaList.Alias.IsNullOrEmpty() ? parentMetaList.Alias : (parentCategory.CategoryLanguageName ?? string.Empty)},
                {"{3}", !topMetaList.Alias.IsNullOrEmpty() ? topMetaList.Alias : (topCategory.CategoryLanguageName ?? string.Empty)},
                {"{4}", page>1 ? "Page "+page.ToString() : ""},
                {"{5}", page>1 ? pageCount.ToString() : ""},
                {"{6}", page>1 ? (showMode ? "Gallery" : "List") : ""},
                {"{7}", propertyValueNames},
                {"{8}", keyword ?? string.Empty}
            };
            var isPro = propertyValueList.Count > 0;
            var metaInfo = new MetaInfoVo
            {
                Title = isPro ? metaList.TitlePro : metaList.Title,
                Keywords = isPro ? metaList.KeywordsPro : metaList.Keywords,
                Description = isPro ? metaList.DescriptionPro : metaList.Description
            };
            foreach (var item in dic)
            {
                if (!metaInfo.Title.IsNullOrEmpty()) metaInfo.Title = metaInfo.Title.Replace(item.Key, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Value));
                if (!metaInfo.Keywords.IsNullOrEmpty()) metaInfo.Keywords = metaInfo.Keywords.Replace(item.Key, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Value));
                if (!metaInfo.Description.IsNullOrEmpty()) metaInfo.Description = metaInfo.Description.Replace(item.Key, item.Value.ToLower());
            }
            ViewBag.MetaInfo = metaInfo;
        }
        #endregion

        #region 找货OEM
        [HttpGet]
        public ActionResult Souring()
        {
            int source = Request["source"].ParseTo<int>(0);
            int productid = Request["productid"].ParseTo<int>(0);
            ProductInfo info = null;
            if (source == 1)
            {
                ViewBag.Source = true;
                var product = ServiceFactory.ProductService.GetProductById(productid);
                if (!product.IsNullOrEmpty())
                {
                    info = ServiceFactory.ProductService.GetProductInfos(new List<Product>() { product })[0];
                }
            }
            else
            {
                ViewBag.Source = false;
            }
            return View(info);
        }

        [HttpPost]
        public ActionResult AddSouring()
        {
            string productName = Request["ProductName"] ?? string.Empty;
            string content = Request["content"] ?? string.Empty;
            string fullname = "";
            string email = "";
            string attachmentfile = Request["attachmentfile"] ?? string.Empty;
            string ofiles = "";
            string nfiles = "";
            var customer = SessionHelper.CurrentCustomer;
            if (customer.IsNullOrEmpty())
            {
                fullname = Request["fullname"] ?? string.Empty;
                email = Request["email"] ?? string.Empty;
            }
            else
            {
                fullname = customer.FullName;
                email = customer.Email;
            }

            var files = attachmentfile.Split(",");
            var filedir = UploadFileHelper.GetUploadFileSavePath(UploadFileType.AddSource);
            foreach (var f in files)
            {
                var a = f.Split("|");
                if (a.Count == 2)
                {
                    var attachmentfilepath = Path.Combine(filedir, a[0]);
                    if (System.IO.File.Exists(attachmentfilepath))
                    {
                        nfiles += a[0] + ",";
                        ofiles += a[1] + ",";
                    }
                }
            }

            if (!fullname.IsNullOrEmpty() && !email.IsNullOrEmpty() && !content.IsNullOrEmpty() &&
                !productName.IsNullOrEmpty())
            {
                ServiceFactory.ProductService.AddOemSouring(new OemSourcing()
                {
                    TitleLink = productName,
                    DetailContent = content,
                    CustomerEmail = email,
                    CustomerName = fullname,
                    OriginalAttachmentName = ofiles,
                    AttachmentName = nfiles,
                });
                //发邮件了
                ServiceFactory.MailService.CustomerSourcingInformation(productName, content, fullname, email, nfiles);
                ServiceFactory.MailService.SourcingInformationToSales(productName, content, fullname, email, nfiles);
            }

            return RedirectToAction("SouringSuccess", "Product");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult SouringUpload(HttpPostedFileBase fileData, string folder)
        {
            string extend = "";
            if (null != fileData)
            {
                try
                {
                    string filename = Path.GetFileName(fileData.FileName);
                    extend = Guid.NewGuid() + filename.Substring(filename.LastIndexOf('.'),
                                                      filename.Length - filename.LastIndexOf('.'));
                    saveFile(fileData, extend, UploadFileType.AddSource);
                }
                catch (Exception ex)
                {
                    extend = ex.ToString();
                }
            }
            return Content(extend);
        }

        public ActionResult SouringField()
        {
            return View();
        }

        public ActionResult SouringSuccess()
        {
            return View();
        }
        #endregion

        #region ProductClub
        [HttpGet]
        public ActionResult ProductClub()
        {
            ViewBag.NewClubProducts = ServiceFactory.ClubProductService.FindAllClubProducts(1, 10000, new Dictionary<ClubProductSearchCriteria, object> { { ClubProductSearchCriteria.ClubProductType, ClubProductType.New } }, null).Data.ToList();
            ViewBag.HotClubProducts = ServiceFactory.ClubProductService.FindAllClubProducts(1, 10000, new Dictionary<ClubProductSearchCriteria, object> { { ClubProductSearchCriteria.ClubProductType, ClubProductType.Hot } }, null).Data.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult HowJoinClub()
        {
            return RedirectToAction("WelcomeInClub", "Account");
        }
        #endregion
    }


}
