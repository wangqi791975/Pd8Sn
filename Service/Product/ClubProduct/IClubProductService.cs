using System;
using System.Collections.Generic;
using Com.Panduo.Service.Customer.Product;

namespace Com.Panduo.Service.Product.ClubProduct
{
    public interface IClubProductService
    {
        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// Club产品已经存在
        /// </summary>
        string ERROR_CLUBPRODUCT_EXIST { get; }
        /// <summary>
        /// Club产品不存在
        /// </summary>
        string ERROR_CLUBPRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 该产品为一口价产品
        /// </summary>
        string ERROR_EXIST_IN_DAILYDEAL_PRODUCT { get; }
        /// <summary>
        /// 该产品为促销产品
        /// </summary>
        string ERROR_EXIST_IN_PROMOTION_PRODUCT { get; }
        /// <summary>
        /// 重复存在
        /// </summary>
        string ERROR_CLUBPRODUCT_REPETITION { get; }
        #endregion

        #region Club产品

        /// <summary>
        /// 根据导入的Excel数据生成ProductClubProduct对象
        /// </summary>
        /// <param name="productNo">产品货号 B00001</param>
        /// <param name="discount">折扣</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品数据不存在</value>
        /// <value>ERROR_ClubProductAREA_NOT_EXIST:Club区不存在</value>
        /// <value>ERROR_ClubProductAREA_PRODUCT_EXIST:Club区中该产品已经存在</value>
        /// </exception>
        /// <returns></returns>
        ClubProduct CreatedClubProductByImportData(string productNo, decimal discount);

        /// <summary>
        /// 批量设置Club产品 key:错误编号 value：具体错误值
        /// </summary>
        /// <param name="list">Club产品集合</param>
        KeyValuePair<string,string> SetClubProductList(List<ClubProduct> list);

        /// <summary>
        /// 清空Club产品（物理删除）
        /// </summary>
        void ClearClubProduct();

        /// <summary>
        /// 根据产品Id获取单个产品Club信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ClubProduct GetVaildClubProductByProductId(int productId);

        /// <summary>
        /// 根据产品Id列表批量获取产品Club信息
        /// </summary>
        /// <param name="productIds">产品Id列表</param>
        /// <returns></returns>
        IList<ClubProduct> GetVaildClubProductByProductIds(IList<int> productIds);
 
        #region 方法
        /// <summary>
        /// 添加club产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>新增Id</returns>
        //int AddClubProduct(int productId);

        /// <summary>
        /// 移除club产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        void RemoveClubProduct(int productId);

        /// <summary>
        /// 批量移除club产品
        /// </summary>
        /// <param name="productIds">产品Id</param>
        //void RemoveClubProducts(List<int> productIds);

        /// <summary>
        /// 前台 根据大类获取所有Club产品
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<ClubProduct> GetClubProductsByType(ClubProductType type);

        /// <summary>
        /// 分页查询Club产品
        /// </summary>
        /// <returns></returns>
        PageData<ClubProductView> FindAllClubProducts(int currentPage, int pageSize,
            IDictionary<ClubProductSearchCriteria, object> searchCriteria, IList<Sorter<ClubProductSorterCriteria>> sorterCriteria);

        #endregion


        #endregion
    }


    /// <summary>
    /// Club产品搜索条件
    /// </summary>
    public enum ClubProductSearchCriteria
    {
        /// <summary>
        /// 产品货号
        /// </summary>
        ProductCode = 1,
        /// <summary>
        /// club会员产品类型
        /// </summary>
        ClubProductType = 2,
        /// <summary>
        /// 语言
        /// </summary>
        LanguageId = 3
    }

    /// <summary>
    /// Club产品排序条件
    /// </summary>
    public enum ClubProductSorterCriteria
    {

    }
}
