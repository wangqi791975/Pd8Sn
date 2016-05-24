using System.Collections.Generic;

namespace Com.Panduo.Service.Customer.Product
{
    /// <summary>
    /// 心愿单服务接口
    /// </summary>
    public interface IWishListService
    {
        #region 常量
        /// <summary>
        /// 客户不存在
        /// </summary>
        string ERROR_CUSTOMER_NOT_EXIST { get; }

        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }
        #endregion

        #region 方法

        /// <summary>
        /// 添加心愿单产品（单个）
        /// </summary>
        /// <param name="wishListProduct">心愿单实体</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        /// <returns>新添加的心愿单Id</returns>
        int AddWishListProduct(WishListProduct wishListProduct);

        /// <summary>
        /// 批量添加心愿单产品（已经存在就更新）
        /// </summary>
        /// <param name="wishListProducts">多个心愿单</param>
        void AddWishListProducts(IList<WishListProduct> wishListProducts);

        /// <summary>
        /// 移除单个心愿单产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">要移除的心愿单产品Id</param>
        /// <param name="isHistory">是否历史心愿单产品</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        void RemoveWishListProduct(int customerId, int productId, bool isHistory);


        /// <summary>
        /// 批量移除心愿单产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productList">要移除的心愿单产品集合,key=产品Id value=是否历史记录</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        void RemoveWishListProduct(int customerId, IList<KeyValuePair<int, bool>> productList);


        /// <summary>
        /// 设置喜爱类型
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="wishListId">心愿单Id</param>
        /// <param name="wishlistType">喜爱类型</param> 
        /// <param name="isHistory">是否历史心愿单产品</param>       
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        void SetWishListType(int customerId, int wishListId, WishListType wishlistType, bool isHistory);


        /// <summary>
        /// 批量设置喜爱类型
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="wishtlistTypes">心愿单喜爱类型键值对，key：心愿单Id，value：喜爱类型</param>  
        /// <param name="isHistory">是否历史心愿单产品</param>    
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// </exception>
        void SetWishListType(int customerId, IDictionary<int, WishListType> wishtlistTypes, bool isHistory);


        /// <summary>
        /// 获取单个心愿单产品
        /// </summary>
        /// <param name="wishListId">心愿单Id</param>
        /// <param name="isHistory">是否历史心愿单产品</param> 
        /// <returns>商品实体</returns>
        WishListProduct GetWishListProductById(int wishListId, bool isHistory);


        /// <summary>
        /// 批量获取心愿单产品（分页）
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<WishListProduct> GetWishListProducts(int currentPage, int pageSize, int customerId,IDictionary<WishListSearchCriteria, object> searchCriteria, IList<Sorter<WishListSorterCriteria>> sorterCriteria);


        /// <summary>
        /// 批量获取历史心愿单产品（分页），[add By luohaiming]
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<WishListProduct> GetWishListHistoryProducts(int currentPage, int pageSize, int customerId, IDictionary<WishListSearchCriteria, object> searchCriteria, IList<Sorter<WishListSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 批量获取将要被移除的心愿单产品（分页），[add By luohaiming]
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<WishListProduct> GetWishListRemovedProducts(int currentPage, int pageSize, int customerId, IList<Sorter<WishListSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 根据客户Id获取客户历史wishList产品类别，[add By luohaiming]
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="isHistory">是否历史心愿单产品</param> 
        /// <returns></returns>
        IList<KeyValuePair<int, string>> GetWishListProductCategory(int customerId, bool isHistory);


        /// <summary>
        /// 根据语种id获取对应语种的wishlist类型列表，[add By luohaiming]
        /// </summary>
        /// <param name="languageId">语种id</param>
        /// <returns>WishListTypeDesc列表</returns>
        IList<WishListTypeDesc> GetWishListType(int languageId);

        /// <summary>
        /// 获取WishList个数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        int GetWishListCountByCustomerId(int customerId);


        /// <summary>
        /// 批量获取将要被移除的心愿单产品（分页），[add By luohaiming]
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<CustomerWishListProduct> GetAdminWishLists(int currentPage, int pageSize, IDictionary<WishListSearchCriteria, object> searchCriteria, IList<Sorter<WishListSorterCriteria>> sorterCriteria);

        #endregion
    }

    /// <summary>
    /// 心愿单查询条件
    /// </summary>
    public enum WishListSearchCriteria
    {
        /// <summary>
        /// 类别Id
        /// </summary>
        CategoryId,
        /// <summary>
        /// 心愿单类型
        /// </summary>
        ClassificationType,

        /// <summary>
        /// 客户邮箱（后台查询）
        /// </summary>
        CustomerEmail,

        /// <summary>
        /// 产品编号（后台查询）
        /// </summary>
        ProductNo,
    }

    /// <summary>
    /// 心愿单排序条件
    /// </summary>
    public enum WishListSorterCriteria
    {
        /// <summary>
        /// 添加时间
        /// </summary>
        AddDate=1
    }
}
