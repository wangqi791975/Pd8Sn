using System.Collections.Generic;

namespace Com.Panduo.Service.DataExport
{
    public interface IDataExportService
    {
        #region 普通客户
        /// <summary>
        /// 客户注册数统计
        /// </summary>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        IList<RegisterCustomer> GetRegisterCustomers(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        #endregion

        #region Club客户
        /// <summary>
        /// Club数据统计
        /// </summary>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        IList<ClubCustomer> GetClubCustomers(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);
        #endregion

        #region 产品
        /// <summary>
        /// 产品状态数据统计
        /// </summary>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        IList<ProductStatusLog> GetProductStatusLogs(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 导出网站所有产品编号
        /// </summary>
        /// <returns></returns>
        IList<string> GetProductModels();

        /// <summary>
        /// 网站上货信息
        /// </summary>
        /// <returns></returns>
        IList<ProductUpload> GetProductUploads(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        #endregion

        #region 订单
        /// <summary>
        /// 订单数据统计
        /// </summary>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        IList<OrderInfo> GetOrderDatas(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 平均订单金额统计
        /// </summary>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        IList<AverageOrderAmountInfo> GetAverageOrderAmounts(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        #endregion

        #region 销售数据
        /// <summary>
        /// 销售数据统计
        /// </summary>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        IList<SaleInfo> GetSaleInfos(IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        #endregion

    }

    /// <summary>
    /// 客户搜索条件
    /// </summary>
    public enum CustomerSearchCriteria
    {
        CustomerSource,
        CustomerOrderFirst,
        StartDateTime,
        EndDateTime
    }

    /// <summary>
    /// 客户排序条件
    /// </summary>
    public enum CustomerSorterCriteria
    {
        Number,
        DateTime
    }
}
