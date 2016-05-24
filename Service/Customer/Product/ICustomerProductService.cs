using System.Collections.Generic;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Service.Customer.Product
{
    /// <summary>
    /// 客户产品专区服务
    /// </summary>
    public interface ICustomerProductService
    {
        #region 常量
        /// <summary>
        /// 客户产品不存在
        /// </summary>
        string ERROR_CUSTOMERPRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }
        #endregion

        #region 方法

        /// <summary>
        /// 添加客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns>新增Id</returns>
        int AddCustomerProduct(int customerId, int productId);

        /// <summary>
        /// 添加客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productCode">产品编号</param>
        /// <returns>新增Id</returns>
        int AddCustomerProduct(int customerId, string productCode);

        /// <summary>
        /// 批量添加客户产品
        /// </summary>
        /// <param name="customerProducts">产品Id key=customerId value=productId</param>
        void AddCustomerProducts(List<KeyValuePair<int,int>> customerProducts);

        /// <summary>
        /// 批量添加客户产品
        /// </summary>
        /// <param name="customerProducts">客户产品</param>
        void AddCustomerProducts(List<CustomerProduct> customerProducts);

        /// <summary>
        /// 删除客户产品
        /// </summary>
        /// <param name="id">自增Id</param>
        void DeleteCustomerProduct(int id);

        /// <summary>
        /// 移除客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        void RemoveCustomerProduct(int customerId, int productId);

        /// <summary>
        /// 批量移除客户产品
        /// </summary>
        /// <param name="customerProducts">产品Id key=customerId value=productId</param>
        void RemoveCustomerProducts(List<KeyValuePair<int, int>> customerProducts);

        /// <summary>
        /// 获取客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns>客户产品</returns>
        Service.Product.Product GetCustomerProduct(int customerId, int productId);

        /// <summary>
        /// 获取客户绑定产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户绑定产品</returns>
        List<CustomerProduct> GetCustomerProducts(int customerId);

        /// <summary>
        /// 搜索对应客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <returns>客户产品</returns>
        PageData<CustomerProduct> FindCustomerProducts(int customerId, int currentPage, int pageSize, IDictionary<CustomerProductSearchCriteria, object> searchCriteria, IList<Sorter<CustomerProductSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 搜索对应客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <returns>客户产品</returns>
        PageData<CustomerProductView> FindCustomerProductsView(int customerId, int currentPage, int pageSize,IDictionary<CustomerProductSearchCriteria, object> searchCriteria,IList<Sorter<CustomerProductSorterCriteria>> sorterCriteria);

        #endregion
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public enum CustomerProductSearchCriteria
    {
        KeyWrod
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public enum CustomerProductSorterCriteria { }
}