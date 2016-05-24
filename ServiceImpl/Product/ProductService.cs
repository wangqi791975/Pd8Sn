using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using System;
using System.Collections.Generic;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.ClubProduct;
using Com.Panduo.Service.Product.DailyDeal;
using Com.Panduo.Service.Product.Promotion;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl.Product.Category.Dao;
using Com.Panduo.ServiceImpl.Product.Dao;
using Com.Panduo.ServiceImpl.Product.Property.Dao;
using Com.Panduo.ServiceImpl.Product.Solr;
using Com.Panduo.ServiceImpl.SiteConfigure;
using Com.Panduo.ServiceImpl.SiteConfigure.Dao;
using PropertyValueSortType = Com.Panduo.Service.Product.Property.PropertyValueSortType;

namespace Com.Panduo.ServiceImpl.Product
{
    public class ProductService : IProductService
    {

        #region IOC
        public ILanguageDao LanguageDao { private get; set; }
        public IProductDao ProductDao { private get; set; }
        public IProductDescDao ProductDescDao { private get; set; }
        public IProductPropertyValueDao ProductPropertyValueDao { private get; set; }
        public IProductStockDao ProductStockDao { private get; set; }
        public IProductQuantityPriceDao ProductQuantityPriceDao { private get; set; }
        public IProductImageDao ProductImageDao { private get; set; }
        public IUnitDao UnitDao { private get; set; }
        public IUnitDescDao UnitDescDao { private get; set; }
        public IProductSearchLogDao ProductSearchLogDao { private get; set; }
        public IProductsOtherpackDao ProductsOtherpackDao { private get; set; }
        public IProductAlsoboughtDao ProductAlsoboughtDao { private get; set; }
        public IHotProductDao HotProductDao { private get; set; }
        public IProductCategorieDao ProductCategorieDao { private get; set; }
        public IProductMatchDao ProductMatchDao { private get; set; }
        public IProductSimilarIgnoreViewDao ProductSimilarIgnoreViewDao { get; set; }
        public ICategoryDao CategoryDao { get; set; }
        public IPropertyValueDao PropertyValueDao { private get; set; }



        public ICategoryService CategoryService { get; set; }
        public IPropertyService PropertyService { get; set; }
        public IPromotionService PromotionService { get; set; }
        public IProductDailyDealService ProductDailyDealService { get; set; }
        public IClubProductService ClubProductService { get; set; }
        public IConfigureService ConfigureService { get; set; }

        public IProductPriceRiseDao ProductPriceRiseDao { get; set; }
        public IOemSourcingDao OemSourcingDao { get; set; }
        #endregion

        #region 常量
        public string ERROR_PRODUCT_NOT_EXIST { get; private set; }
        #endregion

        public ProductStatus? GetProductStatus(int productId)
        {
            var po = GetProductFromCache(productId);
            if (po.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                return (ProductStatus)po.Status;
            }
        }

        public Service.Product.Product GetProductById(int productId)
        {
            return GetProductFromCache(productId);
        }

        public Service.Product.Product GetProductByCode(string productCode)
        {
            return GetProductFromCache(productCode);
        }

        public void LoadAllProducts()
        {
            LoadAllProductToCache();
        }

        public void UpdateProduct(Service.Product.Product product)
        {
            var po = ProductDao.GetObject(product.ProductId);
            if (!po.IsNullOrEmpty())
            {
                po.ProductWeight = product.Weight;
                po.ProductVolumeWeight = product.VolumeWeight;
                po.PackQuantity = product.GroupQuantity;
                po.UnitId = product.UnitId;
                po.ProductPriceRmb = product.CostPriceRmb;
                po.IncreaseProportion = product.IncreaseProportion;
                po.Status = (int)product.Status;
                ProductDao.UpdateObject(po);
            }
        }

        public string GetProductName(int productId, int languageId)
        {
            var po = ProductDescDao.GetProductDescription(productId, languageId);
            if (!po.IsNullOrEmpty())
            {
                return po.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetProductNameByEn(int productId)
        {
            var language = LanguageDao.GetLanguageByCode("en");
            int languageId = 1;
            if (!language.IsNullOrEmpty())
                languageId = language.LanguageId;
            var po = ProductDescDao.GetProductDescription(productId, languageId);
            if (!po.IsNullOrEmpty())
                return po.Name;
            return string.Empty;
        }

        public string GetProductDescription(int productId, int languageId)
        {
            var po = GetProductLanguageFromCache(productId, languageId);
            if (!po.IsNullOrEmpty())
            {
                return po.ProductDescription;
            }
            else
            {
                return string.Empty;
            }
        }


        public ProductLanguage GetProductLanguage(int productId, int languageId)
        {
            var po = ProductDescDao.GetProductDescription(productId, languageId);
            return GetProductDescVoFromPo(po);
        }

        public IList<ProductLanguage> GetProductLanguages(int productId)
        {
            var productLanguages = new List<ProductLanguage>();
            var productDescPoList = ProductDescDao.GetProductDescriptions(productId);
            if (!productDescPoList.IsNullOrEmpty())
            {
                foreach (var countryPo in productDescPoList)
                {
                    productLanguages.Add(GetProductDescVoFromPo(countryPo));
                }
            }
            return productLanguages;
        }

        public void UpdateProductLanguages(IList<ProductLanguage> productLanguageList)
        {
            var listUpdate = new List<ProductDescPo>();
            var listAdd = new List<ProductDescPo>();
            foreach (var vo in productLanguageList)
            {

                var po = ProductDescDao.GetProductDescription(vo.ProductId, vo.LanguageId);
                if (!po.IsNullOrEmpty())
                {
                    po.Name = vo.ProductName != null ? vo.ProductName : po.Name;
                    po.MarketingTitle = vo.MarketingTitle != null ? vo.MarketingTitle : po.MarketingTitle;
                    po.Description = vo.ProductDescription != null ? vo.ProductDescription : po.Description;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.ProductName.IsNullOrEmpty())
                    {
                        po = new ProductDescPo()
                        {
                            ProductId = vo.ProductId,
                            LanguageId = vo.LanguageId,
                            Name = vo.ProductName,
                            Description = vo.ProductDescription,
                            MarketingTitle = vo.MarketingTitle ?? string.Empty
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                ProductDescDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                ProductDescDao.AddObjects(listAdd);
            }
        }


        public IList<Service.Product.Product> GetOtherPackings(int productId)
        {
            var list = new List<Service.Product.Product>();
            var po = ProductsOtherpackDao.GetProductOtherpack(productId);

            if (!po.IsNullOrEmpty())
            {
                if (po.OtherId1.HasValue && po.OtherId1.Value != 0)//判断小包装是否存在
                {
                    var vo = GetProductFromCache(po.OtherId1.Value);
                    if (vo != null)
                    {
                        list.Add(vo);
                    }
                }

                if (po.OtherId2.HasValue && po.OtherId2.Value != 0)//判断大包装是否存在
                {
                    var vo = GetProductFromCache(po.OtherId2.Value);
                    if (vo != null)
                    {
                        list.Add(vo);
                    }
                }
            }
            return list;
        }

        public IList<ProductPropertyValue> GetProductBindedAllPropertyValues(int productId)
        {
            var productPropertyValueList = new List<ProductPropertyValue>();
            var productPropertyValuePoList = ProductPropertyValueDao.GetProductBindedAllPropertyValues(productId);
            if (!productPropertyValuePoList.IsNullOrEmpty())
            {
                foreach (var productPropertyValuePo in productPropertyValuePoList)
                {
                    productPropertyValueList.Add(GetProductPropertyValueVoFromPo(productPropertyValuePo));
                }
            }
            return productPropertyValueList;
        }

        public decimal GetProductSaleBatchVolume(int productId)
        {
            var productPo = ProductDao.GetObject(productId);
            if (!productPo.IsNullOrEmpty())
            {
                return productPo.ProductVolumeWeight;
            }
            else
            {
                return 0.0M;
            }
        }

        public void UpdateProductPropertyValues(IList<ProductPropertyValue> productPropertyValueList)
        {
            var listUpdate = new List<ProductPropertyValuePo>();
            foreach (var productPropertyValue in productPropertyValueList)
            {
                var po = ProductPropertyValueDao.GetProductProductPropertyValue(productPropertyValue.ProductId, productPropertyValue.PropertyId);
                if (!po.IsNullOrEmpty())
                {
                    if (productPropertyValue.PropertyValueId > 0)
                    {
                        po.PropertyValueId = productPropertyValue.PropertyValueId;
                        listUpdate.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                ProductPropertyValueDao.UpdateObjects(listUpdate);
            }
        }

        public decimal GetProductSaleBatchWeight(int productId)
        {
            var productPo = ProductDao.GetObject(productId);
            if (!productPo.IsNullOrEmpty())
            {
                return productPo.ProductWeight;
            }
            else
            {
                return 0.0M;
            }
        }

        public ProductStock GetProductStock(int productId)
        {
            var po = ProductStockDao.GetProductStock(productId);
            return GetProductStockVoFromPo(po);
        }

        public void UpdateProductStock(ProductStock productStock)
        {
            var po = ProductStockDao.GetObject(productStock.StockId);
            po.LimitStock = productStock.BindStockType != null ? (int)productStock.BindStockType : po.LimitStock;
            po.Quantity = productStock.StockNumber != null ? productStock.StockNumber : po.Quantity;
            ProductStockDao.UpdateObject(po);
        }

        /// <summary>
        /// 批量加载产品详细信息
        /// </summary>
        /// <param name="products">产品基本信息列表</param>
        /// <param name="isIncludeProductStock">是否加载产品库存信息</param>
        /// <param name="isIncludeProductImage">是否加载产品图片列表信息</param>
        /// <param name="isIncludeProductProperty">是否加载产品属性信息</param>
        /// <param name="isIncludeProductPrice">是否加载产品价格信息</param>
        /// <param name="isJudgeHotSeller">是否判断产品是热销</param>
        /// <param name="isJudgeHasSimilarProuct">是否判读产品是否有相似商品</param>
        /// <returns></returns>
        public IList<ProductInfo> GetProductInfos(IList<Service.Product.Product> products, bool isIncludeProductStock = false, bool isIncludeProductImage = false, bool isIncludeProductProperty = false, bool isIncludeProductPrice = false, bool isJudgeHotSeller = false, bool isJudgeHasSimilarProuct = false)
        {
            var list = new List<ProductInfo>(products.Count);

            var beginDate = DateTime.Now;

            if (!products.IsNullOrEmpty())
            {
                var productIds = products.Select(c => c.ProductId).Join(",");


                //获取本次涉及产品的库存
                var allStock = isIncludeProductStock ? ProductStockDao.GetProductStockByIds(productIds) : new List<ProductStockPo>();

                //var stockTs = DateTime.Now - beginDate;

                //获取本次涉及产品的图片
                var allImages = isIncludeProductImage ? ProductImageDao.GetAllProductImagesByIds(productIds) : new List<ProductImagePo>();

                //var imageTs = DateTime.Now - beginDate;

                var allProperties = isIncludeProductProperty ? GetProductPropertyValues(products) : new Dictionary<int, IList<KeyValuePair<Service.Product.Property.Property, PropertyValue>>>();

                //获取本次涉及产品的价格， 
                var allPrices = isIncludeProductPrice ? GetProductPrices(products) : new List<ProductPrice>();

                //获取热销品信息
                var allHotSellers = isJudgeHotSeller ? HotProductDao.GetHotProductsByProductIds(productIds) : new List<HotProductPo>();

                //获取是否有相似商品信息
                var allSimilarProucts = isJudgeHasSimilarProuct ? GetProductHasSimilarItemInfos(products) : new Dictionary<int, bool>();

                //获取本次涉及产品的单位 
                var unitIds = products.Select(c => c.UnitId).Distinct().Join(",");
                var allProductUnits = UnitDescDao.GetAllUnitDescByUnitIdAndLangId(unitIds, ServiceFactory.ConfigureService.SiteLanguageId);

                //var unitTs = DateTime.Now - beginDate;
                //获取本次涉及产品的多语言信息（当前环境语种)
                var allProductDescs = ProductDescDao.GetProductDescriptionsByProductIdAndLangId(productIds, ServiceFactory.ConfigureService.SiteLanguageId);

                //var productDescTs = DateTime.Now - beginDate;
                //获取本次涉及产品的多语言信息(英语语种)
                var allEnglishProductDescs = ServiceFactory.ConfigureService.SiteLanguageId == ServiceFactory.ConfigureService.EnglishLangId ? allProductDescs : ProductDescDao.GetProductDescriptionsByProductIdAndLangId(productIds, ServiceFactory.ConfigureService.EnglishLangId);

                //var englishProductDescTs = DateTime.Now - beginDate;

                foreach (var product in products)
                {
                    var vo = new ProductInfo
                        {
                            Product = product,
                            ProductStock = isIncludeProductStock ? allStock.Select(c => GetProductStockVoFromPo(c)).FirstOrDefault(c => c.ProductId == product.ProductId) : null,
                            ProductImages = isIncludeProductImage ? allImages.Where(c => c.ProductId == product.ProductId).OrderByDescending(c => c.IsMain).ThenBy(c => c.SortOrder).Select(c => GetProductImageVoFromPo(c)).ToList() : null,
                            ProductProperties = isIncludeProductProperty ? allProperties.TryGet(product.ProductId) : null,
                            ProductPrice = isIncludeProductPrice ? allPrices.FirstOrDefault(c => c.ProductId == product.ProductId) : null,
                            IsHot = isJudgeHotSeller ? allHotSellers.Any(c => c.ProductId == product.ProductId) : false,
                            HasSimilarItems = isJudgeHasSimilarProuct ? allSimilarProucts.TryGet(product.ProductId) : false,

                            UnitName = allProductUnits.Where(c => c.UnitId == product.UnitId).Select(c => c.Name).FirstOrDefault(),
                            ProductDesName = allProductDescs.Where(c => c.ProductId == product.ProductId).Select(c => c.Description).FirstOrDefault(),
                            ProductName = allProductDescs.Where(c => c.ProductId == product.ProductId).Select(c => c.Name).FirstOrDefault(),
                            ProductEnName = allEnglishProductDescs.Where(c => c.ProductId == product.ProductId).Select(c => c.Name).FirstOrDefault(),
                        };

                    list.Add(vo);
                }
            }

            var all = DateTime.Now - beginDate;

            return list;
        }

        public IList<ProductInfo> GetProductInfos(IList<int> productIds, bool isIncludeProductStock = false, bool isIncludeProductImage = false, bool isIncludeProductProperty = false, bool isIncludeProductPrice = false, bool isJudgeHotSeller = false, bool isJudgeHasSimilarProuct = false)
        {
            var products = new List<Service.Product.Product>();
            foreach (var productId in productIds)
            {
                products.Add(GetProductById(productId));
            }

            return GetProductInfos(products, isIncludeProductStock, isIncludeProductImage, isIncludeProductProperty, isIncludeProductPrice, isJudgeHotSeller, isJudgeHasSimilarProuct);
        }

        public ProductPrice GetProductPrice(int productId)
        {
            var product = GetProductById(productId);
            if (!product.IsNullOrEmpty())
            {
                var promotion = ServiceFactory.PromotionService.GetProductPromotionByProductId(productId);
                var p = new ProductPrice()
                    {
                        ProductId = productId,
                        CostPrice = product.CostPriceFinal.HasValue ? product.CostPriceFinal.Value : 0.0M,
                        StepPrice = GetProductSalePrices(productId, (product.CostPriceFinal.HasValue ? product.CostPriceFinal.Value : 0.0M)),
                        PromotionalDiscount = promotion.IsNullOrEmpty() ? 1.0M : promotion.Discount,
                        ClubDiscount = ClubProductService.GetVaildClubProductByProductId(productId).IsNullOrEmpty() ? 0.0M : ClubProductService.GetVaildClubProductByProductId(productId).Discount,
                        IsNoHaggle = ServiceFactory.ProductDailyDealService.IsProductDailyDeal(productId),
                        NoHaggle = ServiceFactory.ProductDailyDealService.GetProductDailyDealPrice(productId),
                    };
                return p;
            }
            return null;
        }


        /// <summary>
        /// 获取产品阶梯价格
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="costPriceFinal">产品最终成本价格（美元）</param>
        /// <returns>产品阶梯价格列表</returns>
        public IList<ProductStepPrice> GetProductSalePrices(int productId, decimal costPriceFinal)
        {
            var list = new List<ProductStepPrice>();
            var productQuantityPriceList = ProductQuantityPriceDao.GetProductQuantityPrices(productId);
            if (!productQuantityPriceList.IsNullOrEmpty())
            {
                foreach (var productQuantityPricePo in productQuantityPriceList)
                {
                    var vo = GetProductStepPriceVoFromPo(productQuantityPricePo);
                    vo.CostPrice = costPriceFinal;
                    list.Add(vo);
                }
            }
            return list;
        }

        /// <summary>
        /// 批量获取产品几个信息
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private IList<ProductPrice> GetProductPrices(IList<Service.Product.Product> products)
        {
            var productPrices = new List<ProductPrice>();
            if (!products.IsNullOrEmpty())
            {
                var productIdList = products.Select(c => c.ProductId).ToList();
                var productIds = productIdList.Join(",");
                var promotions = PromotionService.GetProductPromotionByProductIds(productIdList);
                var productSalePricePos = ProductQuantityPriceDao.GetProductQuantityPricesBatch(productIds);
                var productSalePrices = GetProductSalePrices(productSalePricePos, products);
                var productDailyDeals = ProductDailyDealService.GetProductDailyDeals(productIdList);
                var clubProducts = ClubProductService.GetVaildClubProductByProductIds(productIdList);

                productPrices = products.Select(c => new ProductPrice()
                {
                    ProductId = c.ProductId,
                    CostPrice = c.CostPriceFinal.HasValue ? c.CostPriceFinal.Value : 0.0M,
                    StepPrice = productSalePrices.Where(p => p.ProductId == c.ProductId).OrderBy(d => d.Quantity).ToList(),
                    PromotionalDiscount = promotions.Any(d => d.ProductId == c.ProductId && PromotionService.IsPromotionProduct(d, null)) ? promotions.Where(p => p.ProductId == c.ProductId).Select(d => d.Discount).FirstOrDefault() : 1.00M,
                    ClubDiscount = clubProducts.Where(p => p.ProductId == c.ProductId).Select(d => d.Discount).FirstOrDefault(),
                    IsNoHaggle = productDailyDeals.Any(p => p.ProductId == c.ProductId && p.IsValid && DateTime.Now >= p.StartDateTime && DateTime.Now <= p.EndDateTime),
                    NoHaggle = productDailyDeals.Where(p => p.ProductId == c.ProductId).Select(d => d.Price).FirstOrDefault(),
                }).ToList();
            }

            return productPrices;
        }

        /// <summary>
        /// 批量获取产品关联的属性值
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private IDictionary<int, IList<KeyValuePair<Service.Product.Property.Property, Service.Product.Property.PropertyValue>>> GetProductPropertyValues(IList<Service.Product.Product> products)
        {
            var map = new Dictionary<int, IList<KeyValuePair<Service.Product.Property.Property, Service.Product.Property.PropertyValue>>>();

            if (!products.IsNullOrEmpty())
            {
                foreach (var product in products)
                {
                    //产品绑定属性值
                    var bindPropertyValue = GetProductBindedAllPropertyValues(product.ProductId);
                    var propertyandpropertyvalueList = new List<KeyValuePair<Service.Product.Property.Property, Service.Product.Property.PropertyValue>>();

                    foreach (var productPropertyValue in bindPropertyValue)
                    {
                        var property = ServiceFactory.PropertyService.GetPropertyById(productPropertyValue.PropertyId);
                        var propertyValue = ServiceFactory.PropertyService.GetPropertyValue(productPropertyValue.PropertyValueId);
                        var propertyLnaguage = ServiceFactory.PropertyService.GetPropertyLanguageById(productPropertyValue.PropertyId, ServiceFactory.ConfigureService.SiteLanguageId);
                        var propertyvalueLnaguage = ServiceFactory.PropertyService.GetPropertyValueLanguage(productPropertyValue.PropertyValueId, ServiceFactory.ConfigureService.SiteLanguageId);
                        if (property != null && propertyLnaguage != null && propertyValue != null && propertyvalueLnaguage != null && property.IsValid && propertyValue.IsValid && property.IsDisplay)
                        {
                            property.PropertyName = propertyLnaguage.PropertyName;
                            propertyValue.PropertyValueName = propertyvalueLnaguage.PropertyValueName;

                            propertyandpropertyvalueList.Add(new KeyValuePair<Service.Product.Property.Property, Service.Product.Property.PropertyValue>(property, propertyValue));
                        }
                    }

                    if (map.ContainsKey(product.ProductId))
                    {
                        map[product.ProductId] = propertyandpropertyvalueList;
                    }
                    else
                    {
                        map.Add(product.ProductId, propertyandpropertyvalueList);
                    }
                }
            }


            return map;
        }

        /// <summary>
        /// 获取产品相似商品信息
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private IDictionary<int, bool> GetProductHasSimilarItemInfos(IList<Service.Product.Product> products)
        {
            var map = new Dictionary<int, bool>();

            if (!products.IsNullOrEmpty())
            {
                IDictionary<ProductSearchCriteria, object> searchCriteria =
                    new Dictionary<ProductSearchCriteria, object>
                    {
                        {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.All},
                        {ProductSearchCriteria.SimilarProductId, 0}
                    };

                IList<Sorter<ProductSorterCriteria>> sorterCriteria = new List<Sorter<ProductSorterCriteria>>()
                {
                    new Sorter<ProductSorterCriteria>(){Key =ProductSorterCriteria.None,IsAsc = true}
                };

                SearchProductData result = null;

                foreach (var product in products)
                {
                    var hasSimilar = ImplCacheHelper.GetProductHasSimilar(product.ProductId);
                    if (hasSimilar == null)
                    {
                        searchCriteria[ProductSearchCriteria.SimilarProductId] = product.ProductId;

                        result = SearchProducts(1, 1, searchCriteria, sorterCriteria, false, false);

                        hasSimilar = result.ProductPageData.Pager.TotalRowCount > 0;

                        ImplCacheHelper.SetProductHasSimilar(product.ProductId, hasSimilar.Value);
                    }

                    if (map.ContainsKey(product.ProductId))
                    {
                        map[product.ProductId] = hasSimilar.Value;
                    }
                    else
                    {
                        map.Add(product.ProductId, hasSimilar.Value);
                    }
                }
            }


            return map;
        }

        private IList<ProductStepPrice> GetProductSalePrices(IList<ProductQuantityPricePo> pricePos, IList<Service.Product.Product> products)
        {
            var list = new List<ProductStepPrice>();
            if (!pricePos.IsNullOrEmpty())
            {
                foreach (var po in pricePos)
                {
                    var vo = GetProductStepPriceVoFromPo(po);
                    vo.CostPrice = products.Where(c => c.ProductId == po.ProductId).Select(c => c.CostPriceFinal.HasValue ? c.CostPriceFinal.Value : 0.0M).FirstOrDefault();
                    list.Add(vo);
                }
            }
            return list;
        }

        public ProductStepPrice GetProductSalePriceByPurchaseQty(int productId, int purchaseQuantity)
        {
            var productQuantityPricePo = ProductQuantityPriceDao.GetProductQuantityPriceByPurchaseQty(productId, purchaseQuantity);
            return GetProductStepPriceVoFromPo(productQuantityPricePo);
        }

        public void UpdateProductStepPrices(IList<ProductStepPrice> productStepPrices)
        {
            var listUpdate = new List<ProductQuantityPricePo>();
            foreach (var vo in productStepPrices)
            {
                var po = ProductQuantityPriceDao.GetProductQuantityPrice(vo.ProductId, vo.Quantity);
                if (!po.IsNullOrEmpty())
                {
                    po.ProfitRate = vo.ProfitCoefficient != null ? vo.ProfitCoefficient : po.ProfitRate;
                    listUpdate.Add(po);
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                ProductQuantityPriceDao.UpdateObjects(listUpdate);
            }
        }

        /// <summary>
        /// 获取产品主图
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public string GetProductMainImage(int productId)
        {
            var po = GetProductImagesFromCache(productId);
            if (!po.IsNullOrEmpty())
            {
                return po.ImageName;
            }
            else
            {
                return string.Empty;
            }
        }

        public IList<ProductImages> GetAllProductImagesById(int productId)
        {
            var list = new List<ProductImages>();
            var productImagePoList = ProductImageDao.GetAllProductImagesById(productId);
            if (!productImagePoList.IsNullOrEmpty())
            {
                foreach (var productImagePo in productImagePoList)
                {
                    list.Add(GetProductImageVoFromPo(productImagePo));
                }
            }
            return list;
        }

        #region 产品单位
        public ProductUnit GetProductUnit(int unitId)
        {
            throw new NotImplementedException();
        }

        public IList<ProductUnit> GetAllProductUnit()
        {
            var unitPo = UnitDao.GetAll();
            var productUnit = new List<ProductUnit>();
            if (unitPo != null)
            {
                productUnit = unitPo.Select(x => GetProductUnitFromPo((UnitPo)x)).ToList();
            }
            return productUnit;
        }

        public string GetProductUnitLanguage(int unitId, int languageId)
        {
            var unit = UnitDescDao.GetUnitDesc(unitId, languageId);
            if (!unit.IsNullOrEmpty())
            {
                return unit.Name;
            }
            return string.Empty;
        }

        public IList<ProductUnitLanguage> GetProductUnitLanguages(int unitId)
        {
            throw new NotImplementedException();
        }
        #endregion

        public IList<Service.Product.Product> GetMatchProductsById(int productId)
        {
            var list = new List<Service.Product.Product>();
            var productMatchPoList = ProductMatchDao.GetMatchProduct(productId);
            if (!productMatchPoList.IsNullOrEmpty())
            {
                foreach (var productMatchPo in productMatchPoList)
                {
                    list.Add(GetProductFromCache(productMatchPo.MatchProductId));
                }
            }
            return list;
        }

        public IList<Service.Product.Product> GetMatchProductTopNById(int productId, int topCount)
        {
            var list = new List<Service.Product.Product>();
            var productMatchPoList = ProductMatchDao.GetMatchProduct(productId, topCount);
            if (!productMatchPoList.IsNullOrEmpty())
            {
                foreach (var productMatchPo in productMatchPoList)
                {
                    list.Add(GetProductFromCache(productMatchPo.MatchProductId));
                }
            }
            return list;
        }

        public IList<Service.Product.Product> GetAlsoBuyProductsTopNById(int productId, int topCount)
        {
            var list = new List<Service.Product.Product>();
            var productAlsoboughtPoList = ProductAlsoboughtDao.GetProductAlsobought(productId, topCount);
            if (!productAlsoboughtPoList.IsNullOrEmpty())
            {
                foreach (var productAlsoboughtPo in productAlsoboughtPoList)
                {
                    list.Add(GetProductFromCache(productAlsoboughtPo.AlsoboughtProductId));
                }
            }
            return list;
        }

        public IList<Service.Product.Product> GetSimilarProductTopNById(int productId, int topCount)
        {
            IDictionary<ProductSearchCriteria, object> searchCriteria = new Dictionary<ProductSearchCriteria, object>();
            searchCriteria.Add(ProductSearchCriteria.SimilarProductId, productId);
            searchCriteria.Add(ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.SimilarItem);

            IList<Sorter<ProductSorterCriteria>> sorterCriteria = new List<Sorter<ProductSorterCriteria>>()
                {
                     new Sorter<ProductSorterCriteria>(){Key =ProductSorterCriteria.BestMatch,IsAsc = true}
                };
            var list = SearchProducts(1, topCount, searchCriteria, sorterCriteria, false, false);
            if (!list.IsNullOrEmpty())
            {
                return list.ProductPageData.Data;
            }
            return null;
        }

        public SearchProductData GetSimilarProductById(int productId, int currentPage, int pageSize)
        {
            IDictionary<ProductSearchCriteria, object> searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.SimilarItem},
                {ProductSearchCriteria.SimilarProductId, productId}
            };

            IList<Sorter<ProductSorterCriteria>> sorterCriteria = new List<Sorter<ProductSorterCriteria>>()
                {
                    new Sorter<ProductSorterCriteria>(){Key =ProductSorterCriteria.BestMatch,IsAsc = true}
                };
            return SearchProducts(currentPage, pageSize, searchCriteria, sorterCriteria, true, true);
        }

        public SearchProductData SearchProducts(int currentPage, int pageSize, IDictionary<ProductSearchCriteria, object> searchCriteria, IList<Sorter<ProductSorterCriteria>> sorterCriteria,
                                                bool isIncludeProperty, bool isIncludeCategory)
        {
            var beginDate = DateTime.Now;

            var solrQueryParm = new SolrQueryParam
            {
                IsPromotionOn = ConfigureService.IsPromotion,
                IsStatisticsCategory = isIncludeCategory,
                IsStatisticsPropertyValue = isIncludeProperty
            };

            #region 构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var criteria in searchCriteria)
                {
                    switch (criteria.Key)
                    {
                        //商品查询区域
                        case ProductSearchCriteria.ProductSearchAreaType:
                            solrQueryParm.AreaType = EnumHelper.ToEnum<ProductSearchAreaType>(criteria.Value.ToString());
                            break;
                        //关键字搜索
                        case ProductSearchCriteria.Keyword:
                            solrQueryParm.Keyword = criteria.Value.ToString().Trim();
                            break;
                        //商品ID搜索
                        case ProductSearchCriteria.ProductIds:
                            solrQueryParm.ProductIds = criteria.Value as ICollection<int>;
                            break;
                        //忽略的商品ID搜索
                        case ProductSearchCriteria.IgnoreProductIds:
                            solrQueryParm.IgnoreProductIds = criteria.Value as ICollection<int>;
                            break;
                        //产品匹配产品搜索
                        case ProductSearchCriteria.BestMatchProductId:
                            solrQueryParm.ProductIds = new List<int> { (int)criteria.Value };
                            break;
                        //相似商品搜索
                        case ProductSearchCriteria.SimilarProductId:
                            //相似商品时通过商品的属性值和规格值来确定的
                            var productId = criteria.Value.ToString().ParseTo(0);

                            if (productId != 0)
                            {
                                var similarProduct = GetProductFromCache(productId);
                                if (similarProduct != null)
                                {
                                    //获取该产品绑定的属性值(排除掉不需要匹配的属性值)，匹配是OR的关系
                                    solrQueryParm.OrPropertyValueIds = GetProductSimilarPropertyValuesFromCache(productId);

                                    //匹配的商品必须是同末级类别下的商品
                                    solrQueryParm.CategoryId = similarProduct.CategoryId;
                                }
                            }

                            //要排除自己本身
                            if (!solrQueryParm.IgnoreProductIds.IsNullOrEmpty())
                            {
                                solrQueryParm.IgnoreProductIds.Add(productId);
                            }
                            else
                            {
                                solrQueryParm.IgnoreProductIds = new List<int> { productId };
                            }
                            break;
                        //商品编码搜索
                        case ProductSearchCriteria.Skus:
                            solrQueryParm.Skus = criteria.Value as ICollection<string>;
                            break;
                        //上架时间开始
                        case ProductSearchCriteria.JoinDateFrom:
                            solrQueryParm.JoinDateFrom = criteria.Value as DateTime?;
                            break;
                        //上架时间截止
                        case ProductSearchCriteria.JoinDateTo:
                            solrQueryParm.JoinDateTo = criteria.Value as DateTime?;
                            break;
                        //创建时间开始
                        case ProductSearchCriteria.CreateDateFrom:
                            solrQueryParm.CreateDateFrom = criteria.Value as DateTime?;
                            break;
                        //创建时间截止
                        case ProductSearchCriteria.CreateDateTo:
                            solrQueryParm.CreateDateTo = criteria.Value as DateTime?;
                            break;
                        //最低售价开始
                        case ProductSearchCriteria.SalePriceFrom:
                            solrQueryParm.SalePriceMinFrom = criteria.Value as decimal?;
                            break;
                        //最低售价截止
                        case ProductSearchCriteria.SalePriceTo:
                            solrQueryParm.SalePriceMinTo = criteria.Value as decimal?;
                            break;
                        //促销类型（区域）
                        case ProductSearchCriteria.PromotionId:
                            solrQueryParm.PromotionId = criteria.Value as int?;
                            break;
                        //促销折扣
                        case ProductSearchCriteria.PromotionDiscount:
                            solrQueryParm.PromotionDiscountFrom = criteria.Value as decimal?;
                            solrQueryParm.PromotionDiscountTo = solrQueryParm.PromotionDiscountFrom;
                            break;
                        //产品专区
                        case ProductSearchCriteria.ProductAreaId:
                            solrQueryParm.ProductAreaId = criteria.Value as int?;
                            break;
                        //是否有库存
                        case ProductSearchCriteria.IsInStock:
                            solrQueryParm.IsInStock = criteria.Value as bool?;
                            break;
                        //是否热销商品
                        case ProductSearchCriteria.IsBestSeller:
                            solrQueryParm.IsBestSeller = criteria.Value as bool?;
                            break;
                        //是否正常销售商品
                        case ProductSearchCriteria.IsOnSale:
                            solrQueryParm.IsOnSale = criteria.Value as bool?;
                            break;
                        //类别精确搜索
                        case ProductSearchCriteria.CategoryId:
                            solrQueryParm.CategoryId = criteria.Value as int?;
                            break;
                        //类别模糊搜索
                        case ProductSearchCriteria.CategoryPath:
                            solrQueryParm.CategoryPath = criteria.Value as int?;
                            break;
                        //属性值搜索
                        case ProductSearchCriteria.PropertyValueIds:
                            solrQueryParm.PropertyValueIds = criteria.Value as ICollection<int>;
                            break;
                        //属性搜索
                        case ProductSearchCriteria.PropertyValueGroupIds:
                            solrQueryParm.PropertyValueGroupIds = criteria.Value as ICollection<int>;
                            break;
                    }
                }
            }

            //特殊处理属性值和属性值组
            if (!solrQueryParm.PropertyValueIds.IsNullOrEmpty() || !solrQueryParm.PropertyValueGroupIds.IsNullOrEmpty())
            {
                var fiterProperties = new Dictionary<int, IList<KeyValuePair<SolrPropertyType, int>>>();
                if (!solrQueryParm.PropertyValueIds.IsNullOrEmpty())
                {
                    foreach (var itemId in solrQueryParm.PropertyValueIds)
                    {
                        var item = PropertyService.GetPropertyValue(itemId);
                        if (item != null)
                        {
                            if (!fiterProperties.ContainsKey(item.PropertyId))
                            {
                                fiterProperties.Add(item.PropertyId, new List<KeyValuePair<SolrPropertyType, int>>());
                            }

                            fiterProperties[item.PropertyId].Add(new KeyValuePair<SolrPropertyType, int>(SolrPropertyType.PropertyValue, itemId));
                        }
                    }
                }

                if (!solrQueryParm.PropertyValueGroupIds.IsNullOrEmpty())
                {
                    foreach (var itemId in solrQueryParm.PropertyValueGroupIds)
                    {
                        var item = PropertyService.GetPropertyValueGroupById(itemId);
                        if (item != null)
                        {
                            if (!fiterProperties.ContainsKey(item.PropertyId))
                            {
                                fiterProperties.Add(item.PropertyId, new List<KeyValuePair<SolrPropertyType, int>>());
                            }

                            fiterProperties[item.PropertyId].Add(new KeyValuePair<SolrPropertyType, int>(SolrPropertyType.PropertyValueGroup, itemId));
                        }
                    }
                }

                solrQueryParm.FiterPropertyValueAndGroupIds = fiterProperties;
                solrQueryParm.PropertyValueIds = null;
                solrQueryParm.PropertyValueGroupIds = null;
            }
            #endregion

            #region 构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                solrQueryParm.Sorts = sorterCriteria.Select(c => c.Key).ToList();
            }
            else
            {
                //默认按照最佳匹配度排序
                solrQueryParm.Sorts = new List<ProductSorterCriteria> { ProductSorterCriteria.BestMatch };
            }
            #endregion

            #region 查询数据
            SolrQueryResultData solrResultData = null;
            try
            {

                //如果是查询最佳匹配产品的需要二次查询
                if (solrQueryParm.AreaType == ProductSearchAreaType.BestMatch)
                {
                    var bestMatchQueryParam = new SolrQueryParam
                    {
                        AreaType = ProductSearchAreaType.BestMatch,
                        ProductIds = solrQueryParm.ProductIds
                    };

                    solrResultData = ProductSolrService.SearchProduct(currentPage, pageSize, bestMatchQueryParam);

                    solrQueryParm.ProductIds =
                        solrResultData.DataList.Where(c => !c.ProductMatchId.IsNullOrEmpty())
                            .SelectMany(c => c.ProductMatchId)
                            .ToList();

                    if (!solrQueryParm.ProductIds.IsNullOrEmpty())
                    {
                        solrResultData = ProductSolrService.SearchProduct(currentPage, pageSize, solrQueryParm);
                    }
                    else
                    {
                        solrResultData = new SolrQueryResultData
                        {
                            Pager = new Pager(0, currentPage, pageSize),
                            DataList = new List<ProductSolrInfo>()
                        };
                    }
                }
                else
                {

                    solrResultData = ProductSolrService.SearchProduct(currentPage, pageSize, solrQueryParm);
                }

            }
            catch (Exception exception)
            {
                LoggerHelper.GetLogger(LoggerType.Solr).Error(string.Format("Solr查询异常,查询参数:{0}", ObjectHelper.GetProperyKeyValue(solrQueryParm)), exception);
            }
            catch
            {
                LoggerHelper.GetLogger(LoggerType.Solr).Error(string.Format("Solr查询异常,查询参数:{0}", ObjectHelper.GetProperyKeyValue(solrQueryParm)));
            }

            #endregion

            var searchProductData = new SearchProductData();

            var returnSearchDate = DateTime.Now.Subtract(beginDate).TotalSeconds;

            #region 组装数据
            if (solrResultData != null)
            {
                #region 组装商品数据
                var dataList = new List<Service.Product.Product>();
                foreach (var item in solrResultData.DataList)
                {
                    var vo = GetProductFromCache(item.ProductId);
                    if (!vo.IsNullOrEmpty())
                    {
                        //todo 这里是否还要加屏蔽商品的判断
                        dataList.Add(vo);
                    }
                }

                searchProductData.ProductPageData = new PageData<Service.Product.Product>
                {
                    Data = dataList,
                    Pager = solrResultData.Pager
                };
                #endregion

                #region 组装类别数据
                if (isIncludeCategory)
                {
                    searchProductData.ProductCategories = GetSolrProductCategories(solrResultData, solrQueryParm);
                }

                #endregion

                #region 组装属性值数据
                if (isIncludeProperty)
                {
                    searchProductData.ProductProperties = GetSolrProductProperties(solrResultData, solrQueryParm);
                }
                #endregion
            }
            else
            {
                //无搜索数据的情况下
                searchProductData.ProductPageData = new PageData<Service.Product.Product>()
                {
                    Pager = new Pager(0, currentPage, pageSize),
                    Data = new List<Service.Product.Product>()
                };

                searchProductData.ProductCategories = new List<SearchProductCategory>();
                searchProductData.ProductProperties = new List<SearchProductProperty>();
            }
            #endregion

            var endDate = DateTime.Now.Subtract(beginDate).TotalSeconds;

            return searchProductData;
        }

        public void RecordProductSearchLog(ProductSearchLog log)
        {
            var po = new ProductSearchLogPo()
                {
                    CustomerId = log.CustomerId,
                    Ip = log.Ip,
                    Keyword = log.Keywords,
                    DateSearched = DateTime.Now,
                };
            ProductSearchLogDao.AddObject(po);
        }

        /// <summary>
        /// 添加找货
        /// </summary>
        /// <param name="sourcing">OemSourcing</param>
        public void AddOemSouring(OemSourcing sourcing)
        {
            if (!sourcing.IsNullOrEmpty())
            {
                OemSourcingPo po = new OemSourcingPo()
                {
                    TitleLink = sourcing.TitleLink,
                    DetailContent = sourcing.DetailContent,
                    AttachmentLink = sourcing.AttachmentLink,
                    AttachmentName = sourcing.AttachmentName,
                    OriginalAttachmentName = sourcing.OriginalAttachmentName,
                    CustomerEmail = sourcing.CustomerEmail,
                    CustomerName = sourcing.CustomerName,
                    DateCreated = DateTime.Now,
                };
                OemSourcingDao.AddObject(po);
            }
        }

        #region 后台
        public void AddProductPriceRise(ProductPriceRise productPriceRise)
        {
            var po = new ProductPriceRisePo();
            ObjectHelper.CopyProperties(productPriceRise, po, new string[] { });
            if (po.RiseValue > 0)
            {
                ProductPriceRiseDao.AddObject(po);
            }
        }

        public void UpdateProductPriceRise(ProductPriceRise productPriceRise)
        {
            throw new NotImplementedException();
        }

        public void DeleteProductPriceRiseById(int id)
        {
            ProductPriceRiseDao.DeleteObjectById(id);
        }

        public IList<ProductPriceRise> GetAllProductPriceRise()
        {
            var pos = ProductPriceRiseDao.GetAll();
            var list = pos.Select(x => GetProductPriceRiseFromPo((ProductPriceRisePo)x)).ToList();
            return list;
        }

        public void ProductUpload(IList<ProductExcelRow> productExcelRows)
        {
            throw new NotImplementedException();
        }


        #endregion

        public PageData<Service.Product.Product> FindProductsForAdminList(int currentPage, int pageSize, IDictionary<ProductSearchCriteria, object> searchDictionary)
        {
            var list = ProductDao.FindProductsForAdminList(currentPage, pageSize, searchDictionary);

            PageData<Service.Product.Product> pageData = new PageData<Service.Product.Product>();
            pageData.Data = list.Data.Select(x => GetProductVoFromPo((ProductPo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        public void SetBestMatch(IList<KeyValuePair<string, string>> list)
        {
            bool cando = true;
            foreach (var item in list)
            {
                if (!ProductMatchDao.CanSetBestMatch(item))
                {
                    cando = false;
                    break;
                }
            }

            if (cando)
            {
                foreach (var item in list)
                {
                    ProductMatchDao.SetBestMatch(item);
                }
            }
        }

        public bool CanSetBestMatch(KeyValuePair<string, string> item)
        {
            return ProductMatchDao.CanSetBestMatch(item);
        }


        public void UpdateProductStockByOrderId(int orderId)
        {
            ProductStockDao.UpdateProductStockByOrderId(orderId);
        }

        #region Po Vo转换
        /// <summary>
        ///  产品Po转Vo
        /// </summary>
        /// <param name="productPo">产品Po</param>
        /// <returns>产品Vo</returns>
        public Service.Product.Product GetProductVoFromPo(ProductPo productPo)
        {
            Service.Product.Product product = null;
            if (!productPo.IsNullOrEmpty())
            {
                product = new Service.Product.Product
                {
                    ProductId = productPo.ProductId,
                    ProductCode = productPo.ProductModel,
                    IsOtherPack = productPo.IsOtherPack,
                    CostPriceRmb = productPo.ProductPriceRmb,
                    CostPriceFinal = productPo.ProductPriceFinal,
                    CostPrice = productPo.ProductPriceFinal,
                    Weight = productPo.ProductWeight,
                    VolumeWeight = productPo.ProductVolumeWeight,
                    GroupQuantity = productPo.PackQuantity,
                    UnitId = productPo.UnitId.HasValue ? productPo.UnitId.Value : 0,
                    IncreaseProportion = productPo.IncreaseProportion,
                    IsMixed = productPo.IsMixed,
                    CreateTime = productPo.DateCreated,
                    Status = productPo.Status.ToEnum<ProductStatus>(),
                    IsClub = productPo.IsClub,
                    IsRuClub = productPo.IsRuClub,
                };
                product.MainImage = GetProductMainImage(productPo.ProductId);//获取主图
                var category = ProductCategorieDao.GetProductCategorieByProductId(product.ProductId);
                if (!category.IsNullOrEmpty())
                {
                    product.CategoryId = category.CategoryId;
                }

                var hotProduct = HotProductDao.GetHotProductByProductId(product.ProductId);
                if (!hotProduct.IsNullOrEmpty())
                {
                    product.IsHot = hotProduct.SoldQuantity.HasValue;
                }
            }
            return product;
        }

        /// <summary>
        ///  产品多语种描述Po转Vo
        /// </summary>
        /// <param name="productDescPo">产品多语种描述Po</param>
        /// <returns>产品多语种描述Vo</returns>
        internal static ProductLanguage GetProductDescVoFromPo(ProductDescPo productDescPo)
        {
            ProductLanguage productLanguage = null;
            if (!productDescPo.IsNullOrEmpty())
            {
                productLanguage = new ProductLanguage
                {
                    ProductId = productDescPo.ProductId,
                    LanguageId = productDescPo.LanguageId,
                    ProductName = productDescPo.Name,//修复遗漏的赋值
                    ProductDescription = productDescPo.Description,
                    MarketingTitle = productDescPo.MarketingTitle,
                };
            }
            return productLanguage;
        }

        /// <summary>
        /// 产品多语种描述Vo转Po
        /// </summary>
        /// <param name="productLanguage">产品多语种描述Vo</param>
        /// <returns>产品多语种描述Po</returns>
        internal static ProductDescPo GetProductDescPoFromVo(ProductLanguage productLanguage)
        {
            ProductDescPo productDescPo = null;
            if (!productLanguage.IsNullOrEmpty())
            {
                productDescPo = new ProductDescPo
                {
                    ProductId = productLanguage.ProductId,
                    LanguageId = productLanguage.LanguageId,
                    Description = productLanguage.ProductDescription,
                    MarketingTitle = productLanguage.MarketingTitle,
                };
            }
            return productDescPo;
        }

        /// <summary>
        /// 产品属性值Po转Vo
        /// </summary>
        /// <param name="productPropertyValuePo">产品属性值Po</param>
        /// <returns>产品属性值Vo</returns>
        internal static ProductPropertyValue GetProductPropertyValueVoFromPo(ProductPropertyValuePo productPropertyValuePo)
        {
            ProductPropertyValue productPropertyValue = null;
            if (!productPropertyValuePo.IsNullOrEmpty())
            {
                productPropertyValue = new ProductPropertyValue
                {
                    ProductPropertyValueId = productPropertyValuePo.ProductPropertyValueId,
                    ProductId = productPropertyValuePo.ProductId,
                    PropertyId = productPropertyValuePo.PropertyId,
                    PropertyValueId = productPropertyValuePo.PropertyValueId,
                };
            }
            return productPropertyValue;
        }

        /// <summary>
        /// 产品库存Po转Vo
        /// </summary>
        /// <param name="productStockPo">产品库存Po</param>
        /// <returns>产品库存Vo</returns>
        internal static ProductStock GetProductStockVoFromPo(ProductStockPo productStockPo)
        {
            ProductStock productStock = null;
            if (!productStockPo.IsNullOrEmpty())
            {
                productStock = new ProductStock
                {
                    StockId = productStockPo.Id,
                    ProductId = productStockPo.ProductId,
                    StockNumber = productStockPo.Quantity,
                    BindStockType = productStockPo.LimitStock.ToEnum<StockStatus>(),
                    DateReturn = productStockPo.DateReturn,
                    DayReturn = productStockPo.DayReturn
                };
            }
            return productStock;
        }

        /// <summary>
        /// 产品梯度价格Po转Vo
        /// </summary>
        /// <param name="productQuantityPricePo">产品梯度价格Po</param>
        /// <returns>产品梯度价格Vo</returns>
        internal static ProductStepPrice GetProductStepPriceVoFromPo(ProductQuantityPricePo productQuantityPricePo)
        {
            ProductStepPrice productStock = null;
            if (!productQuantityPricePo.IsNullOrEmpty())
            {
                productStock = new ProductStepPrice
                {
                    ProductId = productQuantityPricePo.ProductId,
                    Quantity = productQuantityPricePo.Quantity,
                    ProfitCoefficient = productQuantityPricePo.ProfitRate,
                };
            }
            return productStock;
        }

        /// <summary>
        /// 产品图片Po转Vo
        /// </summary>
        /// <param name="productImagePo">产品图片Po</param>
        /// <returns>产品图片Vo</returns>
        internal static ProductImages GetProductImageVoFromPo(ProductImagePo productImagePo)
        {
            ProductImages productImages = null;
            if (!productImagePo.IsNullOrEmpty())
            {
                productImages = new ProductImages
                {
                    ProductId = productImagePo.ProductId,
                    IsMainImage = productImagePo.IsMain,
                    ImageName = productImagePo.Image,
                    DisplayOrder = productImagePo.SortOrder,
                };
            }
            return productImages;
        }

        /// <summary>
        /// 产品单位Po转Vo
        /// </summary>
        /// <param name="unitPo">产品单位Po</param>
        /// <returns>产品单位Vo</returns>
        internal static ProductUnit GetProductUnitFromPo(UnitPo unitPo)
        {
            ProductUnit productUnit = null;
            if (!unitPo.IsNullOrEmpty())
            {
                productUnit = new ProductUnit
                {
                    UnitId = unitPo.UnitId,
                    Code = unitPo.Code,
                    ChineseName = unitPo.ChineseName,
                    DateCreated = unitPo.DateCreated
                };
            }
            return productUnit;
        }

        /// <summary>
        /// 产品上浮比例Po转Vo
        /// </summary>
        /// <param name="po"> 产品上浮比例Po</param>
        /// <returns> 产品上浮比例Vo</returns>
        internal static ProductPriceRise GetProductPriceRiseFromPo(ProductPriceRisePo po)
        {
            ProductPriceRise productPriceRise = new ProductPriceRise();
            if (!po.IsNullOrEmpty())
            {
                ObjectHelper.CopyProperties(po, productPriceRise, new string[] { });
            }
            return productPriceRise;
        }

        #endregion

        #region Cache
        internal void LoadAllProductToCache()
        {
            var pos = ProductDao.GetAll();
            if (!pos.IsNullOrEmpty())
            {
                IDictionary<ProductSearchCriteria, object> searchCriteria =
                   new Dictionary<ProductSearchCriteria, object>
                    {
                        {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.All},
                        {ProductSearchCriteria.SimilarProductId, 0}
                    };

                IList<Sorter<ProductSorterCriteria>> sorterCriteria = new List<Sorter<ProductSorterCriteria>>()
                {
                    new Sorter<ProductSorterCriteria>(){Key =ProductSorterCriteria.None,IsAsc = true}
                };

                foreach (var po in pos)
                {
                    var vo = GetProductVoFromPo(po);

                    if (!vo.IsNullOrEmpty())
                    {
                        //产品产品基本信息
                        ImplCacheHelper.SetProduct(vo);

                        //缓存是否有相似商品
                        var result = SearchProducts(1, 1, searchCriteria, sorterCriteria, false, false);
                        ImplCacheHelper.SetProductHasSimilar(vo.ProductId, result.ProductPageData.Pager.TotalRowCount > 0);

                        //缓存产品Code
                        if (vo.ProductCode.IsNullOrEmpty())
                        {
                            ImplCacheHelper.SetProductCode(vo.ProductId, vo.ProductCode);
                        }
                    }
                }
            }
        }

        internal Service.Product.Product GetProductFromCache(int productId)
        {
            var vo = ImplCacheHelper.GetProduct(productId);
            if (vo.IsNullOrEmpty())
            {
                var po = ProductDao.GetObject(productId);

                vo = GetProductVoFromPo(po);

                if (!vo.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetProduct(vo);
                }
            }
            return vo;
        }

        internal Service.Product.Product GetProductFromCache(string sku)
        {
            Service.Product.Product vo = null;
            var productId = ImplCacheHelper.GetProductCode(sku);
            if (productId != 0)
            {
                vo = GetProductFromCache(productId);
            }
            else
            {
                var po = ProductDao.GetProductByCode(sku);
                vo = GetProductVoFromPo(po);
                if (!vo.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetProduct(vo);
                }
            }
            return vo;
        }

        internal ProductLanguage GetProductLanguageFromCache(int productId, int languageId)
        {
            var vo = ImplCacheHelper.GetProductLanguage(productId, languageId);
            if (vo.IsNullOrEmpty())
            {
                var po = ProductDescDao.GetProductDescription(productId, languageId);
                vo = GetProductDescVoFromPo(po);

                if (!vo.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetProductLanguage(vo);
                }
            }
            return vo;
        }

        internal ProductImages GetProductImagesFromCache(int productId)
        {
            var vo = ImplCacheHelper.GetProductImage(productId);
            if (vo.IsNullOrEmpty())
            {
                var po = ProductImageDao.GetProductMainImage(productId);

                vo = GetProductImageVoFromPo(po);

                if (!vo.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetProductImage(vo);
                }
            }
            return vo;
        }

        /// <summary>
        /// 从缓存获取一个类别关联的所有属性值列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        internal IList<int> GetProductSimilarPropertyValuesFromCache(int productId)
        {
            var list = ImplCacheHelper.GetProductSimilarPropertyValues(productId);
            if (list == null)
            {
                list = ProductSimilarIgnoreViewDao.GetProductSimilarIgnoreViewPos(productId).Select(c => c.PropertyValueId).ToList();

                ImplCacheHelper.SetProductSimilarPropertyValue(productId, list);
            }

            return list;
        }

        /// <summary>
        /// 从缓存中读取类别树
        /// </summary> 
        /// <returns></returns>
        internal IList<RelatedData<Service.Product.Category.Category>> GetCategoryTreeFromCache()
        {
            var categoryTree = ImplCacheHelper.GetCategoryTree();
            if (categoryTree.IsNullOrEmpty())
            {
                categoryTree = GetCategoryTreeRecursive(null);

                ImplCacheHelper.SetCategoryTree(categoryTree);
            }

            return categoryTree;
        }

        /// <summary>
        /// 递归获取类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，如果为NUll则从根类别开始获取</param>
        /// <returns></returns>
        internal IList<RelatedData<Service.Product.Category.Category>> GetCategoryTreeRecursive(int? parentCategoryId)
        {
            var categoryTree = new List<RelatedData<Service.Product.Category.Category>>();

            IList<Com.Panduo.Service.Product.Category.Category> categories = null;
            if (parentCategoryId.HasValue)
            {
                categories = CategoryService.GetAllSubCategories(parentCategoryId.Value);
            }
            else
            {
                categories = CategoryService.GetAllRootCategories();
            }

            //先过滤显示的类别再按照序号排序再按照A-Z排序
            categories = categories.Where(c => c.IsDisplay && c.IsValid).OrderBy(c => c.DiplayOrder).ThenBy(c => c.CategoryName).ToList();

            if (!categories.IsNullOrEmpty())
            {
                foreach (var category in categories)
                {
                    var relatedCategory = new RelatedData<Service.Product.Category.Category>
                    {
                        Data = category,
                        SubDataList = GetCategoryTreeRecursive(category.CategoryId)
                    };

                    categoryTree.Add(relatedCategory);
                }
            }

            return categoryTree;
        }

        internal IList<Service.Product.Property.Property> GetAllSolrPropertiesFromCache()
        {
            var allProperties = ImplCacheHelper.GetAllProperties(ServiceFactory.ConfigureService.SiteLanguageId);
            if (allProperties.IsNullOrEmpty())
            {
                allProperties = PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);

                ImplCacheHelper.SetAllProperties(allProperties, ServiceFactory.ConfigureService.SiteLanguageId);
            }

            return allProperties;
        }

        internal IList<Service.Product.Property.Property> GetAllCategorySolrPropertiesFromCache(int categoryId)
        {
            var allProperties = ImplCacheHelper.GetAllPropertiesOfCategory(categoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            if (allProperties.IsNullOrEmpty())
            {
                var categoryProperties = CategoryService.GetCategoryBindedAllPropertiesRecursive(categoryId);
                allProperties = new List<Service.Product.Property.Property>();
                if (!categoryProperties.IsNullOrEmpty())
                {
                    foreach (var categoryProperty in categoryProperties)
                    {
                        //必须是该类别允许显示的属性
                        if (categoryProperty.IsDisplay)
                        {
                            var property = PropertyService.GetPropertyById(categoryProperty.PropertyId);
                            //属性自身也必须是有效的
                            if (property != null && property.IsValid)
                            {
                                //取当前语种的名称以及类别属性的排序信息
                                var propertyLanguage = PropertyService.GetPropertyLanguageById(property.PropertyId, ServiceFactory.ConfigureService.SiteLanguageId);
                                property.PropertyName = propertyLanguage.IsNullOrEmpty() ? property.PropertyName : propertyLanguage.PropertyName;
                                if (categoryProperty.Id > 0)
                                {
                                    property.SortType = categoryProperty.SortType;
                                    property.DisplayOrder = categoryProperty.DisplayOrder;
                                }

                                allProperties.Add(property);
                            }
                        }
                    }
                }

                //属性的排序方式：先按照序号再按照名称A-Z
                allProperties = allProperties.OrderBy(c => c.DisplayOrder).ThenBy(c => c.PropertyName).ToList();

                ImplCacheHelper.SetAllPropertiesOfCategory(categoryId, ServiceFactory.ConfigureService.SiteLanguageId, allProperties);
            }

            return allProperties;
        }

        #endregion

        #region Solr搜索私有方法
        /// <summary>
        /// 组装Solr返回的类别数据
        /// </summary> 
        /// <param name="solrResultData">Solr返回结果数据</param>
        /// <param name="solrQueryParm">Solr查询参数</param>
        private IList<SearchProductCategory> GetSolrProductCategories(SolrQueryResultData solrResultData, SolrQueryParam solrQueryParm)
        {
            IList<SearchProductCategory> productCategories = null;

            if (solrResultData != null && !solrResultData.AllCategoryQtyMap.IsNullOrEmpty())
            {
                var categoryTree = GetCategoryTreeFromCache();

                productCategories = BuildCategoryQtyTreeRecursive(categoryTree, solrResultData.AllCategoryQtyMap);
            }

            return productCategories ?? new List<SearchProductCategory>();
        }

        /// <summary>
        /// 组装Solr返回的属性值数据
        /// </summary> 
        /// <param name="solrResultData">Solr返回结果数据</param>
        /// <param name="solrQueryParm">Solr查询参数</param>
        private IList<SearchProductProperty> GetSolrProductProperties(SolrQueryResultData solrResultData, SolrQueryParam solrQueryParm)
        {
            var productProperties = new List<SearchProductProperty>();
            if (solrResultData != null && !solrResultData.PropertyValueQtyMap.IsNullOrEmpty())
            {
                var propertyValueQtyMap = solrResultData.PropertyValueQtyMap;

                var allPropetyValues = PropertyService.GetAllPropertyValuesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);

                //1.先通过返回的属性值ID获取所有的属性值信息
                var allPropertyValueQtys = new Dictionary<PropertyValue, int>();
                foreach (var item in propertyValueQtyMap)
                {
                    var propertyValue = allPropetyValues.FirstOrDefault(c => c.PropertyValueId == item.Key);
                    if (propertyValue != null)
                    {
                        allPropertyValueQtys.Add(propertyValue, item.Value);
                    }
                }

                //1.1.再加载出来筛选属性值的属性值
                var fiterPropertyValueQtyMap = solrResultData.FiterPropertyValueQtyMap;
                if (!fiterPropertyValueQtyMap.IsNullOrEmpty())
                {
                    var choosedPropertyOfValues = allPropetyValues.Where(c => fiterPropertyValueQtyMap.Keys.Contains(c.PropertyId) && fiterPropertyValueQtyMap.Values.Any(d => d.Keys.Contains(c.PropertyValueId))).ToList();
                    foreach (var item in choosedPropertyOfValues)
                    {
                        if (!allPropertyValueQtys.ContainsKey(item))
                        {
                            allPropertyValueQtys.Add(item, fiterPropertyValueQtyMap[item.PropertyId].TryGet(item.PropertyValueId));
                        }
                    }
                }


                //2.获取属性信息
                var propertyValueGroups = PropertyService.GetAllPropertyValueGroupLanguages(ServiceFactory.ConfigureService.SiteLanguageId);

                var properties = GetSolrProperties(solrQueryParm);
                foreach (var property in properties)
                {
                    var productProperty = new SearchProductProperty
                    {
                        Property = property,
                        PropertyValueQtys = allPropertyValueQtys.Where(c => c.Key.PropertyId == property.PropertyId && c.Key.PropertyValueGroupId == 0).Select(c => new KeyValuePair<PropertyValue, int>(c.Key, c.Value)).ToList(),
                        PropertyValueGroupQtys = allPropertyValueQtys.Where(
                            c => c.Key.PropertyId == property.PropertyId && c.Key.PropertyValueGroupId > 0)
                            .GroupBy(c => c.Key.PropertyValueGroupId)
                            .Select(g => new SearchProductPropertyGroup
                            {
                                PropertyValueGroup = propertyValueGroups.FirstOrDefault(k => k.GroupId == g.Key),
                                PropertyValueQtys = allPropertyValueQtys.Where(d => d.Key.PropertyId == property.PropertyId && d.Key.PropertyValueGroupId == g.Key).Select(d => new KeyValuePair<PropertyValue, int>(d.Key, d.Value)).ToList(),
                            })
                            .Where(c => c.PropertyValueGroup != null)
                            .ToList()
                    };

                    //无效组下的属性值直接挂到属性下
                    var invalidGroupPropertyValues = productProperty.PropertyValueGroupQtys.Where(c => !c.PropertyValueGroup.IsValid).SelectMany(c => c.PropertyValueQtys);
                    foreach (var item in invalidGroupPropertyValues)
                    {
                        productProperty.PropertyValueQtys.Add(item);
                    }

                    //只保留有效的属性值组
                    productProperty.PropertyValueGroupQtys = productProperty.PropertyValueGroupQtys.Where(c => c.PropertyValueGroup.IsValid).ToList();

                    //属性值组的产品数量统计 
                    if (!productProperty.PropertyValueGroupQtys.IsNullOrEmpty())
                    {
                        foreach (var item in productProperty.PropertyValueGroupQtys)
                        {
                            item.Qty = item.PropertyValueQtys.Sum(c => c.Value);
                        }
                    }

                    //属性的产品数量统计 = 属性值数量 + 属性值组下属性值数量
                    productProperty.Qty = productProperty.PropertyValueQtys.Sum(c => c.Value) + productProperty.PropertyValueGroupQtys.Sum(c => c.Qty);

                    if (productProperty.Qty > 0)
                    {
                        #region 属性值和数值组排序
                        //排序属性和属性值
                        switch (productProperty.Property.SortType)
                        {
                            //名称A-Z
                            case PropertyValueSortType.NameAtoZ:
                                productProperty.PropertyValueQtys = productProperty.PropertyValueQtys.OrderBy(c => c.Key.PropertyValueName).ToList();
                                productProperty.PropertyValueGroupQtys = productProperty.PropertyValueGroupQtys.OrderBy(c => c.PropertyValueGroup.PropertyValueGroupName).ToList();
                                if (!productProperty.PropertyValueGroupQtys.IsNullOrEmpty())
                                {
                                    foreach (var item in productProperty.PropertyValueGroupQtys)
                                    {
                                        item.PropertyValueQtys = item.PropertyValueQtys.OrderBy(c => c.Key.PropertyValueName).ToList();
                                    }
                                }
                                break;
                            //序号升序
                            case PropertyValueSortType.SortOrderAscending:
                                productProperty.PropertyValueQtys = productProperty.PropertyValueQtys.OrderBy(c => c.Key.DisplayOrder).ToList();
                                productProperty.PropertyValueGroupQtys = productProperty.PropertyValueGroupQtys.OrderBy(c => c.PropertyValueGroup.PropertyValueGroupName).ToList();
                                if (!productProperty.PropertyValueGroupQtys.IsNullOrEmpty())
                                {
                                    foreach (var item in productProperty.PropertyValueGroupQtys)
                                    {
                                        item.PropertyValueQtys = item.PropertyValueQtys.OrderBy(c => c.Key.DisplayOrder).ToList();
                                    }
                                }
                                break;
                            //产品数量多到少（默认)
                            case PropertyValueSortType.ProductNumberMoreToLess:
                            default:
                                productProperty.PropertyValueQtys = productProperty.PropertyValueQtys.OrderByDescending(c => c.Value).ToList();
                                productProperty.PropertyValueGroupQtys = productProperty.PropertyValueGroupQtys.OrderByDescending(c => c.Qty).ToList();
                                if (!productProperty.PropertyValueGroupQtys.IsNullOrEmpty())
                                {
                                    foreach (var item in productProperty.PropertyValueGroupQtys)
                                    {
                                        item.PropertyValueQtys = item.PropertyValueQtys.OrderByDescending(c => c.Value).ToList();
                                    }
                                }
                                break;
                        }
                        #endregion

                        productProperties.Add(productProperty);
                    }
                }
            }

            return productProperties;
        }

        /// <summary>
        /// 是否正常的类别赛选查询
        /// </summary>
        /// <param name="solrQueryParm"></param>
        /// <returns></returns>
        private bool IsCategorySolrQuery(SolrQueryParam solrQueryParm)
        {
            return solrQueryParm.AreaType == ProductSearchAreaType.NormalArea && (solrQueryParm.CategoryId.HasValue || solrQueryParm.CategoryPath.HasValue);
        }

        /// <summary>
        /// 获取Solr属性值
        /// </summary>
        /// <param name="solrQueryParm"></param>
        /// <returns></returns>
        private IList<Service.Product.Property.Property> GetSolrProperties(SolrQueryParam solrQueryParm)
        {
            IList<Service.Product.Property.Property> properties = null;
            if (IsCategorySolrQuery(solrQueryParm))
            {
                properties = GetAllCategorySolrPropertiesFromCache(solrQueryParm.CategoryId.HasValue ? solrQueryParm.CategoryId.Value : solrQueryParm.CategoryPath.Value);
            }
            else
            {
                properties = GetAllSolrPropertiesFromCache();
            }

            return properties;
        }


        /// <summary>
        /// 递归构建类别-商品树
        /// </summary>
        /// <param name="categoryTree"></param>
        /// <param name="categoryQtyMap"></param>
        /// <returns></returns>
        private IList<SearchProductCategory> BuildCategoryQtyTreeRecursive(IList<RelatedData<Service.Product.Category.Category>> categoryTree, IDictionary<int, int> categoryQtyMap)
        {
            var productCategories = new List<SearchProductCategory>();

            if (!categoryTree.IsNullOrEmpty())
            {
                foreach (var relatedData in categoryTree)
                {
                    //一级类别
                    if (categoryQtyMap.ContainsKey(relatedData.Data.CategoryId))
                    {
                        var productCategory = new SearchProductCategory
                        {
                            Category = relatedData.Data,
                            Qty = categoryQtyMap[relatedData.Data.CategoryId],
                            SubSearchProductCategorys = BuildCategoryQtyTreeRecursive(relatedData.SubDataList, categoryQtyMap)
                        };

                        productCategories.Add(productCategory);
                    }
                }
            }

            return productCategories;
        }

        #endregion

        #region 后台上货

        public bool SaveUploadProduct(UploadProduct uploadProduct)
        {
            throw new NotImplementedException();
        }

        public bool SaveUploadProducts(List<UploadProduct> uploadProduct)
        {
            var lstProductCodes = uploadProduct.Select(x => x.ProductCode).ToList();
            lstProductCodes.RemoveAll(code => !ProductDao.IsExistsByCode(code));
            if (lstProductCodes.Any())
                throw new BussinessException(string.Format("货号[{0}]产品已经存在", string.Join(",", lstProductCodes)));

            #region 产品线 关联 网站类别
            var lstCategories = CategoryDao.GetAllCategories();
            var lstUploadCategoryCode = uploadProduct.Select(x => x.ProductClass).ToList();
            lstUploadCategoryCode.RemoveAll(x => lstCategories.Exists(y => y.Code == x));
            if (lstUploadCategoryCode.Any())
                throw new BussinessException(string.Format("产品线[{0}]不存在", string.Join(",", lstUploadCategoryCode)));
            #endregion

            #region 产品属性 关联 网站属性值
            var lstPropertys = PropertyValueDao.GetAll().ToList();
            var lstUploadProperty = new List<string>();
            foreach (var lst in uploadProduct.Select(uprod => uprod.PropertyValues.ToList()))
            {
                lst.RemoveAll(lstUploadProperty.Contains);
                lstUploadProperty.AddRange(lst);
            }
            lstUploadProperty.RemoveAll(x => lstPropertys.Exists(y => y.Code == x));
            if (lstUploadProperty.Any())
                throw new BussinessException(string.Format("属性值[{0}]不存在", string.Join(",", lstUploadProperty)));
            #endregion

            foreach (var uploadProd in uploadProduct)
            {
                var productPo = new ProductPo
                {
                    ProductModel = uploadProd.ProductCode,
                    ProductPriceRmb = uploadProd.Price,
                    ProductWeight = uploadProd.Weight,
                    ProductVolumeWeight = uploadProd.VolumeWeight,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = 1,
                    IsMixed = false
                };
                productPo.ProductId = ProductDao.AddObject(productPo);

                #region 多语种处理
                var lstProductDescPos = new List<ProductDescPo>();
                if (!string.IsNullOrEmpty(uploadProd.ProductEnName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductEnName,
                        Description = uploadProd.ProductEnDesc,
                        LanguageId = 1,
                        MarketingTitle = ""
                    });
                if (!string.IsNullOrEmpty(uploadProd.ProductDeName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductDeName,
                        Description = uploadProd.ProductDeDesc,
                        LanguageId = 2,
                        MarketingTitle = ""
                    });
                if (!string.IsNullOrEmpty(uploadProd.ProductRuName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductRuName,
                        Description = uploadProd.ProductRuDesc,
                        LanguageId = 3,
                        MarketingTitle = ""
                    });
                if (!string.IsNullOrEmpty(uploadProd.ProductFrName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductFrName,
                        Description = uploadProd.ProductFrDesc,
                        LanguageId = 4,
                        MarketingTitle = ""
                    });
                if (!string.IsNullOrEmpty(uploadProd.ProductEsName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductEsName,
                        Description = uploadProd.ProductEsDesc,
                        LanguageId = 5,
                        MarketingTitle = ""
                    });
                if (!string.IsNullOrEmpty(uploadProd.ProductJpName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductJpName,
                        Description = uploadProd.ProductJpDesc,
                        LanguageId = 6,
                        MarketingTitle = ""
                    });
                if (!string.IsNullOrEmpty(uploadProd.ProductLtName))
                    lstProductDescPos.Add(new ProductDescPo
                    {
                        ProductId = productPo.ProductId,
                        Name = uploadProd.ProductLtName,
                        Description = uploadProd.ProductLtDesc,
                        LanguageId = 7,
                        MarketingTitle = ""
                    });

                ProductDescDao.AddObjects(lstProductDescPos);
                #endregion

                var productCategory = lstCategories.Find(x => x.Code == uploadProd.ProductClass);
                var productCategoriePo = new ProductCategoriePo
                {
                    ProductId = productPo.ProductId,
                    CategoryId = productCategory.CategoryId,
                    CategoryPath = string.Format(",{0},{1}", productCategory.ParentId, productCategory.CategoryId)
                };
                ProductCategorieDao.AddObject(productCategoriePo);

                var lstproductPropertyValuePo = uploadProd.PropertyValues.Select(x => new ProductPropertyValuePo
                {
                    ProductId = productPo.ProductId,
                    PropertyValueId = lstPropertys.Find(y => y.Code == x).PropertyValueId,
                    PropertyId = lstPropertys.Find(y => y.Code == x).PropertyId,
                }).ToList();
                ProductPropertyValueDao.AddObjects(lstproductPropertyValuePo);

                #region 梯断价
                var lstProductQuantityPricePo = new[] {
                    new ProductQuantityPricePo
                    {
                        ProductId = productPo.ProductId,
                        Quantity = 1,
                        ProfitRate = uploadProd.PriceStep
                    },
                    new ProductQuantityPricePo
                    {
                        ProductId = productPo.ProductId,
                        Quantity = 3,
                        ProfitRate = uploadProd.PriceStep
                    },
                    new ProductQuantityPricePo
                    {
                        ProductId = productPo.ProductId,
                        Quantity = 5,
                        ProfitRate = uploadProd.PriceStep*0.8M
                    },
                    new ProductQuantityPricePo
                    {
                        ProductId = productPo.ProductId,
                        Quantity = 7,
                        ProfitRate = uploadProd.PriceStep*0.6M
                    },
                    new ProductQuantityPricePo
                    {
                        ProductId = productPo.ProductId,
                        Quantity = 10,
                        ProfitRate = uploadProd.PriceStep*0.4M
                    },
                    new ProductQuantityPricePo
                    {
                        ProductId = productPo.ProductId,
                        Quantity = 50,
                        ProfitRate = uploadProd.PriceStep*0.2M
                    }
                };
                ProductQuantityPriceDao.AddObjects(lstProductQuantityPricePo);
                #endregion
            }


            return true;
        }
        #endregion
    }
}
