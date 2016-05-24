using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Product.Promotion
{
    public interface IPromotionService
    {
        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 促销专区已经存在
        /// </summary>
        string ERROR_PROMOTIONAREA_NAME_EXIST { get; }
        /// <summary>
        /// 促销区不存在
        /// </summary>
        string ERROR_PROMOTIONAREA_NOT_EXIST { get; }
        /// <summary>
        /// 促销区中该产品已经存在
        /// </summary>
        string ERROR_PROMOTIONAREA_PRODUCT_EXIST { get; }
        /// <summary>
        /// 促销区中产品不存在
        /// </summary>
        string ERROR_PROMOTIONAREA_PRODUCT_NOT_EXIST { get; }
        #endregion

        #region 促销区

        /// <summary>
        /// 设置促销区
        /// 如果促销区下存在产品，不允许修改
        /// </summary>
        /// <param name="area">促销区实体</param>
        int SetPromotionArea(PromotionArea area);

        /// <summary>
        /// 设置促销区状态
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <param name="isValid">状态</param>
        void SetPromotionAreaStatus(int promotionAreaId, bool isValid);

        /// <summary>
        /// 删除促销区
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        void DeletePromotionArea(int promotionAreaId);

        /// <summary>
        /// 获取促销区
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <returns>促销区</returns>
        PromotionArea GetPromotionAreaById(int promotionAreaId);

        /// <summary>
        /// 后台管理列表用 查询促销区
        /// </summary>
        PageData<PromotionArea> FindPromotionAreas(int currentPage, int pageSize, IDictionary<PromotionAreaSearchCriteria, object> searchCriteria, IList<Sorter<PromotionAreaSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取该促销区下所有已经存在商品折扣
        /// 后台获取URL要用
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PROMOTIONAREA_NOT_EXIST:促销区不存在</value>
        /// </exception>
        List<decimal> GetPromotionAllDiscount(int promotionAreaId);

        /// <summary>
        /// 生成营销URL链接
        /// </summary>
        string ProduceSaleUrl(int promotionAreaId, int languageId, int categoryId, decimal discount);
        #endregion

        #region 促销产品

        /// <summary>
        /// 根据导入的Excel数据生成ProductPromotion对象
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <param name="productNo">产品货号 B00001</param>
        /// <param name="discount">折扣</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品数据不存在</value>
        /// <value>ERROR_PROMOTIONAREA_NOT_EXIST:促销区不存在</value>
        /// <value>ERROR_PROMOTIONAREA_PRODUCT_EXIST:促销区中该产品已经存在</value>
        /// </exception>
        /// <returns></returns>
        ProductPromotion CreatedProductPromotionByImportData(int promotionAreaId, string productNo, decimal discount);

        /// <summary>
        /// 批量设置促销产品
        /// </summary>
        /// <param name="list">促销产品集合</param>
        /// <param name="promotionAreaId">促销区Id</param>
        void SetPromotionProductList(List<ProductPromotion> list, int promotionAreaId = -1);

        /// <summary>
        /// 清空促销产品（物理删除）
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        void ClearProductPromotion(int promotionId);

        /// <summary>
        /// 获取该促销区下所有产品
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        /// <returns></returns>
        //IList<ProductPromotion> GetAllProductsByPromotionId(int promotionId);

        /// <summary>
        /// 根据产品Id获取单个产品促销信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ProductPromotion GetProductPromotionByProductId(int productId);

        /// <summary>
        /// 根据产品Id列表批量获取产品促销信息
        /// </summary>
        /// <param name="productIds">产品Id列表</param>
        /// <returns></returns>
        IList<ProductPromotion> GetProductPromotionByProductIds(IList<int> productIds);

        /// <summary>
        /// 获取该促销区内单个产品
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ProductPromotion GetProductPromotion(int promotionId, int productId);

        /// <summary>
        /// 获取促销折扣
        /// </summary>
        /// <returns>促销折扣列表</returns>
        IList<int> GetPromotionDiscount(); 

        /// <summary>
        /// 判断产品是否促销产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        bool IsPromotionProduct(int productId);

        /// <summary>
        /// 判断产品是否促销产品
        /// </summary>
        /// <param name="productPromotion"></param>
        /// <param name="stockQty"></param>
        /// <returns></returns>
        bool IsPromotionProduct(ProductPromotion productPromotion, int? stockQty);

        /// <summary>
        /// 分页查询促销产品
        /// </summary>
        //PageData<ProductPromotion> FindProductPromotions(int currentPage, int pageSize, IDictionary<ProductPromotionSearchCriteria, object> searchCriteria, IList<Sorter<ProductPromotionSorterCriteria>> sorterCriteria);

        #endregion
    }

    /// <summary>
    /// 促销区搜索条件
    /// </summary>
    public enum PromotionAreaSearchCriteria
    {
        /// <summary>
        /// 促销专区中文名称
        /// </summary>
        PromotionName,

        /// <summary>
        /// 促销开始时间
        /// </summary>
        SaleStartTime,

        /// <summary>
        /// 促销结束时间
        /// </summary>
        SaleEndTime
    }

    /// <summary>
    /// 促销区排序条件
    /// </summary>
    public enum PromotionAreaSorterCriteria
    {

    }
    /*
    /// <summary>
    /// 促销产品搜索条件
    /// </summary>
    public enum ProductPromotionSearchCriteria
    {
        /// <summary>
        /// 产品货号
        /// </summary>
        ProductCode
    }

    /// <summary>
    /// 促销产品排序条件
    /// </summary>
    public enum ProductPromotionSorterCriteria
    {

    }*/
}
