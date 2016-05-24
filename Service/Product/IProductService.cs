using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    public interface IProductService
    {

        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }

        #region 产品基本信息
        /// <summary>
        /// 根据产品ID获取产品状态
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>如果产品不存在，返回NULL</returns>
        ProductStatus? GetProductStatus(int productId);

        /// <summary>
        /// 通过产品ID获取产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品</returns>
        Product GetProductById(int productId);

        /// <summary>
        /// 通过产品编号获取产品
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>产品</returns>
        Product GetProductByCode(string productCode);

        /// <summary>
        /// 加载所有产品到缓存
        /// </summary>
        /// <returns></returns>
        void LoadAllProducts();

        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="product">产品实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        void UpdateProduct(Product product);

        /// <summary>
        /// 获取产品名称
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>产品名称</returns>
        string GetProductName(int productId, int languageId);

        /// <summary>
        /// 获取产品英文名称
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品名称</returns>
        string GetProductNameByEn(int productId);

        /// <summary>
        /// 获取产品描述
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>产品描述</returns>
        string GetProductDescription(int productId, int languageId);

        /// <summary>
        /// 获取产品单个语种信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>ProductLanguage</returns>
        ProductLanguage GetProductLanguage(int productId, int languageId);


        /// <summary>
        /// 获取产品所有语种信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>ProductLanguage</returns>
        IList<ProductLanguage> GetProductLanguages(int productId);


        /// <summary>
        /// 修改产品所有语种信息
        /// </summary>
        /// <param name="productLanguageList">产品多语种列表</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        /// <returns>ProductLanguage</returns>
        void UpdateProductLanguages(IList<ProductLanguage> productLanguageList);

        /// <summary>
        /// 获取其他包装方式
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>其他包装的产品列表</returns>
        IList<Product> GetOtherPackings(int productId);

        /// <summary>
        /// 获取产品绑定的所有属性值
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>属性值</returns>
        IList<ProductPropertyValue> GetProductBindedAllPropertyValues(int productId);

        /// <summary>
        /// 修改产品所有属性
        /// </summary>
        /// <param name="productPropertyValueList">属性值列表</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        /// <returns></returns>
        void UpdateProductPropertyValues(IList<ProductPropertyValue> productPropertyValueList);


        /// <summary>
        /// 获取产品体积重
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>体积重</returns>
        decimal GetProductSaleBatchVolume(int productId);

        /// <summary>
        /// 获取产品重量
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>重量</returns>
        decimal GetProductSaleBatchWeight(int productId);

        /// <summary>
        /// 获取产品库存信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>ProductStock</returns>
        ProductStock GetProductStock(int productId);

        /// <summary>
        /// 修改商品库存
        /// </summary>
        /// <param name="productStock">库存实体</param>
        void UpdateProductStock(ProductStock productStock);

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
        IList<ProductInfo> GetProductInfos(IList<Product> products, bool isIncludeProductStock = false, bool isIncludeProductImage = false, bool isIncludeProductProperty = false, bool isIncludeProductPrice = false, bool isJudgeHotSeller = false, bool isJudgeHasSimilarProuct = false);

        /// <summary>
        /// 批量加载产品详细信息
        /// </summary>
        /// <param name="productIds">产品ID列表</param>
        /// <param name="isIncludeProductStock">是否加载产品库存信息</param>
        /// <param name="isIncludeProductImage">是否加载产品图片列表信息</param>
        /// <param name="isIncludeProductProperty">是否加载产品属性信息</param>
        /// <param name="isIncludeProductPrice">是否加载产品价格信息</param>
        /// <param name="isJudgeHotSeller">是否判断产品是热销</param>
        /// <param name="isJudgeHasSimilarProuct">是否判读产品是否有相似商品</param>
        /// <returns></returns>
        IList<ProductInfo> GetProductInfos(IList<int> productIds, bool isIncludeProductStock = false, bool isIncludeProductImage = false, bool isIncludeProductProperty = false, bool isIncludeProductPrice = false, bool isJudgeHotSeller = false, bool isJudgeHasSimilarProuct = false);

        #endregion

        #region 价格
        /// <summary>
        /// 获取产品价格(一口价、促销价、vip价格等)
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品价格实体</returns>
        ProductPrice GetProductPrice(int productId);

        /// <summary>
        /// 获取产品阶梯价
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="costPriceFinal"></param>
        /// <returns>产品阶梯价</returns>
        IList<ProductStepPrice> GetProductSalePrices(int productId, decimal costPriceFinal);

        /// <summary>
        /// 根据购买数量获取产品阶梯价
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="purchaseQuantity">购买数量</param>
        /// <returns>产品阶梯价</returns>
        ProductStepPrice GetProductSalePriceByPurchaseQty(int productId, int purchaseQuantity);

        /// <summary>
        /// 修改产品所有阶梯价
        /// </summary>
        /// <param name="productStepPrices">阶梯价列表</param>
        /// <returns></returns>
        void UpdateProductStepPrices(IList<ProductStepPrice> productStepPrices);
        #endregion

        #region 图片
        /// <summary>
        /// 根据产品Id获取产品主图
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品主图</returns>
        string GetProductMainImage(int productId);

        /// <summary>
        /// 根据产品Id获取产品所有图片
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品图片</returns>
        IList<ProductImages> GetAllProductImagesById(int productId);
        #endregion

        #region 产品单位
        /// <summary>
        /// 获取产品单位
        /// </summary>
        /// <param name="unitId">单位ID</param>
        /// <returns>产品描述</returns>
        ProductUnit GetProductUnit(int unitId);

        /// <summary>
        /// 获取所有产品单位
        /// </summary>
        /// <returns>产品描述</returns>
        IList<ProductUnit> GetAllProductUnit();

        /// <summary>
        /// 获取产品单位单个语种信息
        /// </summary>
        /// <param name="unitId">单位ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>ProductLanguage</returns>
        string GetProductUnitLanguage(int unitId, int languageId);


        /// <summary>
        /// 获取产品单位所有语种信息
        /// </summary>
        /// <param name="unitId">单位ID</param>
        /// <returns>ProductLanguage</returns>
        IList<ProductUnitLanguage> GetProductUnitLanguages(int unitId);
        #endregion

        #region 相关产品
        /// <summary>
        /// 获取相关产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品</returns>
        IList<Product> GetMatchProductsById(int productId);

        /// <summary>
        /// 获取前N条的相关产品（根据相同属性值的个数）
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="topCount">TOP N</param>
        /// <returns>产品</returns>
        IList<Product> GetMatchProductTopNById(int productId, int topCount);


        /// <summary>
        /// 获取买了又买产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="topCount">TOP N</param>
        /// <returns>产品</returns>
        IList<Product> GetAlsoBuyProductsTopNById(int productId, int topCount);

        /// <summary>
        /// 获取前N条的相似产品（根据相同属性值的个数）
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="topCount">TOP N</param>
        /// <returns>产品</returns>
        IList<Product> GetSimilarProductTopNById(int productId, int topCount);

        /// <summary>
        /// 获取相似产品（根据相同属性值的个数）
        /// 1、排除一些属性值
        /// 2、与主产品的属性值相同的个数
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>产品分页</returns>
        SearchProductData GetSimilarProductById(int productId, int currentPage, int pageSize);

        #endregion

        /// <summary>
        /// 通过Solr调用
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">搜索条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <param name="isIncludeProperty">是否包含属性</param>
        /// <param name="isIncludeCategory">是否包含类别</param>
        /// <returns>SearchProductData</returns>
        SearchProductData SearchProducts(int currentPage, int pageSize, IDictionary<ProductSearchCriteria, object> searchCriteria, IList<Sorter<ProductSorterCriteria>> sorterCriteria, bool isIncludeProperty, bool isIncludeCategory);

        /// <summary>
        /// 记录查询日志
        /// </summary>
        /// <param name="log">ProductSearchLog</param>
        void RecordProductSearchLog(ProductSearchLog log);

        /// <summary>
        /// 添加找货信息
        /// </summary>
        /// <param name="sourcing"></param>
        void AddOemSouring(OemSourcing sourcing);

        #region 后台

        #region 产品调价上浮比例

        /// <summary>
        /// 添加产品调价上浮比例
        /// </summary>
        /// <param name="productPriceRise">调价上浮比例实体</param>
        void AddProductPriceRise(ProductPriceRise productPriceRise);

        /// <summary>
        /// 修改产品调价上浮比例
        /// </summary>
        /// <param name="productPriceRise">调价上浮比例实体</param>
        void UpdateProductPriceRise(ProductPriceRise productPriceRise);

        /// <summary>
        /// 删除产品调价上浮比例
        /// </summary>
        /// <param name="id">主键ID</param>
        void DeleteProductPriceRiseById(int id);

        /// <summary>
        /// 得到所有产品调价上浮比例
        /// </summary>
        IList<ProductPriceRise> GetAllProductPriceRise();

        /// <summary>
        /// 产品上货
        /// </summary>
        /// <param name="productExcelRows">产品基础信息</param>
        void ProductUpload(IList<ProductExcelRow> productExcelRows);
        #endregion

        /// <summary>
        /// 直接查询数据库得到产品数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchDictionary">查询条件</param>
        /// <returns></returns>
        PageData<Product> FindProductsForAdminList(int currentPage, int pageSize, IDictionary<ProductSearchCriteria, object> searchDictionary);

        /// <summary>
        /// 添加匹配产品
        /// </summary>
        /// <param name="list">key=产品Id,value=配产品Id</param>
        void SetBestMatch(IList<KeyValuePair<string,string>> list);


        /// <summary>
        /// 是否可以添加匹配产品
        /// </summary>
        /// <param name="item">key=产品Id,value=配产品Id</param>
        bool CanSetBestMatch(KeyValuePair<string, string> item);

        /// <summary>
        /// 更新产品库存
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void UpdateProductStockByOrderId(int orderId);


        #region 上货
        /// <summary>
        /// 上货
        /// </summary>
        /// <param name="uploadProduct"></param>
        /// <returns></returns>
        bool SaveUploadProduct(UploadProduct uploadProduct);
        /// <summary>
        /// 上货
        /// </summary>
        /// <param name="uploadProduct"></param>
        /// <returns></returns>
        bool SaveUploadProducts(List<UploadProduct> uploadProduct);

        #endregion

        #endregion

    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public enum ProductSearchCriteria
    {
        /// <summary>
        /// 商品区域,参数类型:枚举ProductSearchAreaType, 精确查询,比如搜索促销区商品应该传入的条件为:key = ProductSearchCriteria.ProductSearchAreaType,value = ProductSearchAreaType.Promotion
        /// </summary>
        ProductSearchAreaType,
        /// <summary>
        /// 键字搜索,参数类型:string
        /// </summary>
        Keyword,
        /// <summary>
        /// 搜索指定ID的产品(Or关系),参数类型:ICollection &lt;int&gt;,可以是ILst &lt;int&gt;也可以是int[]
        /// </summary>
        ProductIds,
        /// <summary>
        /// 排除的商品ID,参数类型:ICollection &lt;int&gt;,可以是ILst &lt;int&gt;也可以是int[]
        /// </summary>
        IgnoreProductIds,
        /// <summary>
        /// 相似商品ID,参数类型int,搜索与该商品相似的商品(同类别下属性值相似）
        /// </summary>
        SimilarProductId,
        /// <summary>
        /// BestMatch商品ID,参数类型int,搜索与该商品match的商品
        /// </summary>
        BestMatchProductId,
        /// <summary>
        /// 商品Sku(Or关系),参数类型:ICollection &lt;String&gt;,可以是ILst &lt;String&gt;也可以是string[]
        /// </summary>
        Skus,
        /// <summary>
        /// 上架开始时间(包含),参数类型:Datetime,如果值2014-12-12,表示上架时间从2014-12-12 00:00:00开始的商品
        /// </summary>
        JoinDateFrom,
        /// <summary>
        /// 上架截止时间(包含),参数类型:Datetime,如果值2014-12-12,表示上架时间截止2014-12-11 23:59:59的商品
        /// </summary>
        JoinDateTo,
        /// <summary>
        /// 创建开始时间(包含),参数类型:Datetime,如果值2014-12-12,表示创建时间从2014-12-12 00:00:00开始的商品
        /// </summary>
        CreateDateFrom,
        /// <summary>
        /// 创建截止时间(包含),参数类型:Datetime,如果值2014-12-12,表示创建时间截止2014-12-11 23:59:59的商品
        /// </summary>
        CreateDateTo,
        /// <summary>
        /// 最低售价开始金额(包含),美元金额,参数类型:decimal,如果值为4.5,表示最低段售价大于等于4.5的商品
        /// </summary>
        SalePriceFrom,
        /// <summary>
        /// 最低售价截止金额(包含),美元金额,参数类型:decimal,如果值为4.5,表示最低段售价小于等于4.5的商品
        /// </summary>
        SalePriceTo,
        /// <summary>
        /// 促销类型,参数类型:int,该值是促销区的ID
        /// </summary>
        PromotionId,
        /// <summary>
        /// 促销折扣,参数类型:decimal,如果该值为0.4,标示搜索促销折扣等于40%的商品
        /// </summary>
        PromotionDiscount,
        /// <summary>
        /// 产品专区,参数类型:int,该值是产品专区的ID
        /// </summary>
        ProductAreaId,
        /// <summary>
        /// 是否库存商品,参数类型:bool,如果该值为true表示只搜索有库存的商品,false表示搜索无库存的商品
        /// </summary>
        IsInStock,
        /// <summary>
        /// 是否热销商品,参数类型:bool,如果该值为true表示只搜索热销的商品,false表示搜索非热销的商品
        /// </summary>
        IsBestSeller,
        /// <summary>
        /// 是否正常销售商品,参数类型:bool,如果该值为true表示只搜索正常销售的商品,false表示搜索非正常销售的商品
        /// </summary>
        IsOnSale,
        /// <summary>
        /// 类别ID,参数类型:int,只搜索该类别，不搜索子孙类别下的商品
        /// </summary>
        CategoryId,
        /// <summary>
        /// 类别ID,参数类型:int,搜索该类别及其所有子孙类别下的商品
        /// </summary>
        CategoryPath,
        /// <summary>
        /// 属性值ID(And关系),参数类型:ICollection &lt;String&gt;,可以是ILst &lt;String&gt;也可以是string[]
        /// </summary>
        PropertyValueIds,
        /// <summary>
        /// 属性值组ID(And关系),参数类型:ICollection&lt;String&gt;,可以是ILst &lt;String&gt;也可以是string[]
        /// </summary>
        PropertyValueGroupIds
    }

    /// <summary>
    ///排序条件
    /// </summary>
    public enum ProductSorterCriteria
    {
        /// <summary>
        /// 无排序要求
        /// </summary>
        None = 0,
        /// <summary>
        /// 价格低到高,比如价格为4.5的排在价格为5.6的前面
        /// </summary>
        PriceLowToHigh = 10,
        /// <summary>
        /// 价格高到低,比如价格为4.5的排在价格为5.6的后面
        /// </summary>
        PriceHighToLow = 20,
        /// <summary>
        /// 最佳匹配高到低
        /// </summary>
        BestMatch = 30,
        /// <summary>
        /// 上架时间由近到远,比如2014-10-21上架的排在2014-11-12前面
        /// </summary>
        JoinDateNewToOld = 40,
        /// <summary>
        /// 上架时间由远到近,比如2014-10-21上架的排在2014-11-12后面
        /// </summary>
        JoinDateOldToNew = 50,
        /// <summary>
        /// 创建时间由近到远,比如2014-10-21上架的排在2014-11-12前面
        /// </summary>
        CreateDateNewToOld = 60,
        /// <summary>
        /// 创建时间由远到近,比如2014-10-21上架的排在2014-11-12后面
        /// </summary>
        CreateDateOldToNew = 70,
        /// <summary>
        /// 按照Sku的自然排序A-Z
        /// </summary>
        Sku = 80,
        /// <summary>
        /// 最后更新时间由近到远
        /// </summary>
        LastModifyDate = 90,
        /// <summary>
        /// 销量高到低排序
        /// </summary>
        SaleCount = 100,
        /// <summary>
        /// 点击量高到低排序
        /// </summary>
        ClickCount = 110,
        /// <summary>
        /// 好评度高到低排序
        /// </summary>
        HighComment = 120,
        /// <summary>
        /// 随机排序
        /// </summary>
        Random = 1000,
    }

    /// <summary>
    /// 商品区域
    /// </summary>
    public enum ProductSearchAreaType
    {
        /// <summary>
        /// 所有区域商品
        /// </summary>
        All = 0,
        /// <summary>
        /// 正常商品区
        /// </summary>
        NormalArea = 10,
        /// <summary>
        /// 新商品区
        /// </summary>
        NewArrival = 20,
        /// <summary>
        /// 热销商品区
        /// </summary>
        BestSeller = 30,
        /// <summary>
        /// 混装商品区
        /// </summary>
        MixProduct = 40,
        /// <summary>
        /// 促销商品区
        /// </summary>
        Promotion = 50,
        /// <summary>
        /// 搜索结果区
        /// </summary>
        SearchArea = 60,
        /// <summary>
        /// Daily Deals区
        /// </summary>
        DailyDeals = 70,
        /// <summary>
        /// 相似商品区
        /// </summary>
        SimilarItem = 80,
        /// <summary>
        /// 特色(推荐)商品区
        /// </summary>
        FeaturedProduct = 90,
        /// <summary>
        /// Club专区
        /// </summary>
        ClubProduct = 100,
        /// <summary>
        /// 专区商品
        /// </summary>
        ProductArea = 110,
        /// <summary>
        /// 替代品区
        /// </summary>
        Remplacement = 120,
        /// <summary>
        /// 特殊定价专区
        /// </summary>
        FixedPriceArea = 130,
        /// <summary>
        /// 清仓区
        /// </summary>
        Closeout = 140,

        /// <summary>
        /// BestMatch区
        /// </summary>
        BestMatch=150,
    }
}
