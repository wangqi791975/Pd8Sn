using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.DailyDeal
{
    public interface IProductDailyDealService
    {

        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 一口价产品不存在
        /// </summary>
        string ERROR_DAILYDEAL_PRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 一口价产品已经存在
        /// </summary>
        string ERROR_DAILYDEAL_PRODUCT_IS_EXIST { get; }
        /// <summary>
        /// DailyDeal标语库为空
        /// </summary>
        string ERROR_DAILYDEAL_TITLE_IS_NULL { get; }
        #endregion

        /// <summary>
        /// 设置Dailydeal 有效期
        /// 保存到Config
        /// </summary>
        /// <param name="startTime">有效期开始</param>
        /// <param name="endTime">有效期结束</param>
        void SetDailydealValidityPeriod(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获取该语种所有标语库
        /// </summary>
        /// <param name="languageId">语种Id</param>
        List<DailyDealTitle> GetAllTitles(int languageId);


        /// <summary>
        /// 根据导入的Excel数据生成ProductDailyDeal对象
        /// <para>注意，该方法会做：</para>
        /// <para>1.生成随机数给SaledQuantity </para>
        /// <para>2.随机从标语库里匹配一个标语给Title</para>
        /// </summary>
        /// <param name="productNo">产品编号，货号 B00001</param>
        /// <param name="price">产品一口价价格</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="languageId">语种Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品数据不存在</value>
        /// <value>ERROR_DAILYDEAL_PRODUCT_IS_EXIST:该产品已经设置为Dailydeal</value>
        /// <value>ERROR_DAILYDEAL_TITLE_IS_NULL:Dailydeal标语库为空</value>
        /// </exception>
        /// <returns></returns>
        ProductDailyDeal CreatedProductDailyDealByImportData(string productNo, decimal price, DateTime startDateTime,
            int languageId = -1);

        /// <summary>
        /// 导入批量设置DailyDeal产品
        /// <para>注意：标语Id（TitleId）传进来已经是有值、已售数量（SoldQuantity）有值传进来
        /// 调用方法：CreatedProductDailyDealByImportData
        /// </para>
        /// </summary>
        /// <param name="list">ProductDailyDeal List</param>
        /// <param name="dailyDealLabels"></param>
        void SetDailyDealList(List<ProductDailyDeal> list, List<DailyDealLabel> dailyDealLabels);

        /// <summary>
        /// 设置产品DailyDeal状态
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="isEnable">是否DailyDeal</param>
        void SetProductDailyDealStatus(int productId, bool isEnable);

        /// <summary>
        /// 删除DailyDeal（物理删除）
        /// </summary>
        /// <param name="productId">产品Id</param>
        void DeleteProductDailyDeal(int productId);

        /// <summary>
        /// 获取产品DailyDea价格
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品一口价</returns>
        decimal GetProductDailyDealPrice(int productId);

        /// <summary>
        /// 获取产品DailyDeal信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品DailyDeal信息</returns>
        ProductDailyDeal GetProductDailyDeal(int productId);

        /// <summary>
        /// 批量获取一批产品DailyDeal信息
        /// </summary>
        /// <param name="productIds">产品Id列表</param>
        /// <returns>产品DailyDeal信息</returns>
        List<ProductDailyDeal> GetProductDailyDeals(IList<int> productIds);

        /// <summary>
        /// 判断产品是否DailyDeal产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>是否DailyDeal产品</returns>
        bool IsProductDailyDeal(int productId);

        /// <summary>
        /// 获取有效的一口价商品，前台显示
        /// </summary>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        List<ProductDailyDeal> GetValidDailyDealProducts(int languageId);

        /// <summary>
        /// 后台管理列表： 查询DailyDeal产品
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的DailyDeal产品列表</returns>
        PageData<ProductDailyDeal> FindProductDailyDeals(int currentPage, int pageSize, IDictionary<ProductDailyDealSearchCriteria, object> searchCriteria, IList<Sorter<ProductDailyDealSorterCriteria>> sorterCriteria);

        #region 头部底部DailyDeal广告

        void SetDailydealDesc(DailyDealDesc dailyDealDesc);

        DailyDealDesc GetDailydealDesc(int languageId);

        List<DailyDealDesc> GetAllDailydealDesc();

        #endregion

    }

    public enum ProductDailyDealSearchCriteria
    {
        LanguageId,
        /// <summary>
        /// 产品Code
        /// </summary>
        ProductCode,
        StartDateTime,
        /// <summary>
        /// 结束时间
        /// </summary>
        EndDateTime,
        /*/// <summary>
        /// 产品名称
        /// </summary>
        ProductName*/
    }

    public enum ProductDailyDealSorterCriteria
    {

    }
}
