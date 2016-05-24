using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Panduo.Service.Order.ShippingOption
{
    public interface IShippingService
    {

        #region 常量

        /// <summary>
        /// 运送方式不存在
        /// </summary>
        string ERROR_SHIPPING_NOT_EXISTS { get; }
        #endregion

        #region 运费相关信息
        /// <summary>
        /// 通过国家二级简码和配送方式ID得到客户填写关税号的信息
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <param name="countryId">国家ID</param>
        /// <returns></returns>
        CustomsNo GetCustomsNo(int shippingId, int countryId);
        #endregion

        #region 运费基本信息
        /// <summary>
        /// 根据配送方式ID和国家二级编码得到配送ShippingDay对象
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <param name="countryIsoCode2">国家二级简码</param>
        /// <returns>ShippingDay实体</returns>
        ShippingDay GetShippingDay(int shippingId, string countryIsoCode2);

        /// <summary>
        /// 通过语种ID得到当前配送方式多语种（IIS缓存）
        /// </summary>
        /// <returns></returns>
        IList<ShippingLanguage> GetAllShippingDescs(int languageId);

        /// <summary>
        /// 通过语种ID得到当前配送方式shippingId的语种（IIS缓存）
        /// </summary>
        /// <returns></returns>
        ShippingLanguage GetShippingDescById(int languageId, int shippingId);

        /// <summary>
        /// 获取所有的运送方式信息（前台IIS缓存用）
        /// </summary>
        /// <returns></returns>
        IList<Shipping> GetAllShippings();

        /// <summary>
        /// 地址信息里是否包含P.O.BOX字符串
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        bool IsPoBox(string address);

        #endregion

        #region 运费不同计算方式

        /// <summary>
        /// 得到免运费活动基本运费信息
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <param name="shipppingCriteria">运费计算条件</param>
        /// <returns>ShippingAmount实体</returns>
        ShippingAmount GetShippingAmount(int shippingId, ShipppingCriteria shipppingCriteria);

        /// <summary>
        /// 得到免运费活动基本运费信息列表
        /// </summary>
        /// <param name="shipppingCriteria">运费计算条件</param>
        /// <returns>ShippingAmount实体列表</returns>
        IList<ShippingAmount> GetShippingAmounts(ShipppingCriteria shipppingCriteria);

        /// <summary>
        /// 得到免运费活动基本运费信息列表
        /// </summary>
        /// <param name="shipppingCriteria">运费计算条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>ShippingAmount实体列表</returns>
        List<ShippingAmount> GetShippingAmounts(ShipppingCriteria shipppingCriteria, List<Sorter<ShoppingAmountSorterCriteria>> sorterCriteria);

        ///// <summary>
        ///// 得到用户所有的配送方式信息(这里是通过返回ShippingOption返回当前所有各种情况的运费信息还是通过我上面的方法一项项得到不同的运费信息呢？)
        ///// </summary>
        ///// <param name="countryIsoCode2">国家二级简码</param>
        ///// <param name="city">城市</param>
        ///// <param name="postCode">邮编</param>
        ///// <param name="shippingWeight">发货重量</param>
        ///// <returns>ShippingOption实体列表</returns>
        //IList<ShippingOption> GetShippingOptions(string countryIsoCode2, string city, string postCode, decimal shippingWeight);


        #endregion

        #region 后台

        #region 配送方式
        /// <summary>
        /// 查询所有配送方式
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<Shipping> FindAllShippings(int currentPage, int pageSize, IDictionary<ShippingSearchCriteria, object> searchCriteria, IList<Sorter<ShippingSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 得到单个配送方式信息
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <returns></returns>
        ShippingBaseInfo GetShippingById(int shippingId);

        /// <summary>
        /// 设置配送方式
        /// </summary>
        /// <param name="shippingBaseInfo">配送方式(包含配送方式、配送方式多语种、配送方式到达国家天数)</param>
        void SetShipping(ShippingBaseInfo shippingBaseInfo);

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        //void DeleteShipping(int shippingId);
        #endregion

        #region 偏远城市和邮编
        /// <summary>
        /// 得到偏远城市列表
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchDictionary">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<RemoteCity> FindRemoteCities(int currentPage, int pageSize, IDictionary<RemoteCitySearchCriteria, object> searchDictionary, IList<Sorter<RemoteCitySorterCriteria>> sorterCriteria);

        /// <summary>
        /// 得到偏远邮编列表
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchDictionary">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<RemoteZip> FindRemoteZips(int currentPage, int pageSize, IDictionary<RemoteZipSearchCriteria, object> searchDictionary, IList<Sorter<RemoteZipSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 从ERP同步最新的偏远数据到网站
        /// </summary>
        void SyncRemoteData();
        #endregion

        #endregion


        #region 废弃

        #region 前台

        /*
        
        /// <summary>
        /// 下单页面 获取该客户地址 匹配的所有运送方式
        /// 业务实现里要处理营销活动
        /// </summary>
        /// <param name="customerId">客户</param>
        /// <param name="country">国家二位简码</param>
        /// <param name="city">城市</param>
        /// <param name="zipCode">邮编</param>
        /// <param name="weight">物理重</param>
        /// <param name="shippingWeight">ShippingWeight(体积重和物理重哪个大取哪个)</param>
        /// <param name="languageId">语种</param>
        /// <param name="currencyId">币种</param>
        /// <returns>运送方式运费集合</returns>
        IList<ShippingOption> GetAllShippingOptions(string customerId, string country, string city, string zipCode, decimal weight,
            decimal shippingWeight, int languageId, int currencyId);

        /// <summary>
        /// 获取单个运送方式运费
        /// 业务实现里要处理营销活动
        /// </summary>
        /// <param name="customerId">客户</param>
        /// <param name="shippingId">运送方式Id</param>
        /// <param name="country">国家二位简码</param>
        /// <param name="city">城市</param>
        /// <param name="zipCode">邮编</param>
        /// <param name="weight">物理重</param>
        /// <param name="shippingWeight">ShippingWeight(体积重和物理重哪个大取哪个)</param>
        /// <param name="languageId">语种</param>
        /// <param name="currencyId">币种</param>
        /// <returns>运送方式运费</returns>
        ShippingOption GetShippingOptionByShippingId(string customerId, int shippingId, string country, string city, string zipCode, decimal weight,
            decimal shippingWeight, int languageId, int currencyId);

        /// <summary>
        /// 获取客户默认地址匹配的运送方式运费
        /// 业务实现里要处理营销活动
        /// </summary>
        /// <param name="customerId">客户</param>
        /// <param name="weight">物理重</param>
        /// <param name="shippingWeight">ShippingWeight(体积重和物理重哪个大取哪个)</param>
        /// <param name="languageId">语种</param>
        /// <param name="currencyId">币种</param>
        /// <returns>运送方式运费</returns>
        ShippingOption GetCustomerDefaultAddressWithShippingOption(string customerId, decimal weight, decimal shippingWeight, int languageId, int currencyId);

        /// <summary>
        /// 通过配送ID和语种ID得到配送方式多语种
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>ShippingDesc实体</returns>
        ShippingLanguage GetShippingDesc(int shippingId, int languageId);

        */

        #endregion

        #region 后台操作

        /*
        /// <summary>
        /// 根据Id获取运送方式
        /// </summary>
        /// <param name="shippingId">运送方式Id</param>
        /// <returns>运送方式</returns>
        Shipping GetShippingById(int shippingId);
        /// <summary>
        /// 根据Id获取运送方式
        /// </summary>
        /// <param name="code">运送方式code</param>
        /// <returns>运送方式</returns>
        Shipping GetShippingByCode(string code);

        /// <summary>
        /// 获取所有运送方式
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<Shipping> FindAllShippings(int currentPage, int pageSize, IDictionary<ShippingSearchCriteria, object> searchCriteria, IList<Sorter<ShippingSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 维护单个运送方式
        /// </summary>
        /// <param name="shipping">运送方式对象</param>
        void UpdateShipping(Shipping shipping);
        
        */

        #endregion

        #endregion


    }

    public enum ShippingSearchCriteria
    {
        /// <summary>
        /// Code
        /// </summary>
        Code,

        /// <summary>
        /// 运送方式名称
        /// </summary>
        Name,

        /// <summary>
        /// 语种
        /// </summary>
        Language,

        /// <summary>
        /// 国家
        /// </summary>
        Country,

        /// <summary>
        /// 城市
        /// </summary>
        City,

        /// <summary>
        /// 邮编
        /// </summary>
        ZipCode,

    }

    public enum ShippingSorterCriteria
    {
        /// <summary>
        /// Code
        /// </summary>
        Code,

        /// <summary>
        /// 运送方式名称
        /// </summary>
        Name,
    }

    public enum RemoteCitySearchCriteria
    {
        ShippingCode,
    }

    public enum RemoteCitySorterCriteria
    {
        Id,
    }

    public enum RemoteZipSearchCriteria
    {
        ShippingCode,
    }

    public enum RemoteZipSorterCriteria
    {
        Id,
    }
}
