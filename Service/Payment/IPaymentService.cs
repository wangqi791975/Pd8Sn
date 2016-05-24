using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Payment.PayConfig;
using Com.Panduo.Service.Payment.PayInfo;

namespace Com.Panduo.Service.Payment
{
    /// <summary>
    /// 支付配置接口
    /// </summary>
    public interface IPaymentService
    {
        #region 读取支付配置
        /// <summary>
        /// 获取中国银行转账Bank Of China配置
        /// </summary>
        /// <returns></returns>
        BankOfChinaConfig GetBankOfChinaConfig(); 
        /// <summary>
        /// 获取中国银行转账 HSBC配置
        /// </summary>
        /// <returns></returns>
        HsbcConfig GetHsbcConfig();
        /// <summary>
        /// 获取西联汇款配置Western Union
        /// </summary>
        /// <returns></returns>
        WesternUnionConfig GetWesternUnionConfig(); 
        /// <summary>
        /// 获取Money Gram配置
        /// </summary>
        /// <returns></returns>
        MoneyGramConfig GetMoneyGramConfig();


        /// <summary>
        /// 获取Paypal支付配置
        /// </summary>
        /// <returns></returns>
        PaypalConfig GetPaypalConfig();
        /// <summary>
        /// 获取Paypal快速支付配置
        /// </summary>
        /// <returns></returns>
        PaypalExpressConfig GetPaypalExpressConfig();
        /// <summary>
        /// 获取GC配置
        /// </summary>
        /// <returns></returns>
        GlobalCollectConfig GetGlobalCollectConfig();
        /// <summary>
        /// 获取钱海支付 Ocean Payment配置
        /// </summary>
        /// <returns></returns>
        OceanPaymentConfig GetOceanPaymentConfig();
        #endregion

        #region 判断是否能够使用支付配置
        /// <summary>
        /// 是否能使用中国银行转账支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUseBankOfChina(int customerId, int countryId, int currencyId);
        /// <summary>
        /// 是否能够使用中国工商银行转账支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUseHsbc(int customerId, int countryId, int currencyId);
        /// <summary>
        /// 是否能够使用西联汇款支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUseWesternUnion(int customerId, int countryId, int currencyId);
        /// <summary>
        /// 是否能够使用Money Gram支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUseMoneyGram(int customerId, int countryId, int currencyId);

        /// <summary>
        /// 是否能够使用标准Paypal接口支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUsePaypal(int customerId, int countryId, int currencyId);
        /// <summary>
        /// 是否能够使用Paypal快速支付接口支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUsePaypalExpress(int customerId, int countryId, int currencyId);
        /// <summary>
        /// 是否能够使用GlobalCollect信用卡支付接口支付
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUseGlobalCollect(int customerId, int countryId, int currencyId);
        /// <summary>
        /// 是否能够使用eOceanPayment钱海支付接口支付
        /// </summary>
        /// <param name="method">支付渠道:Webmoney、Yandex、Credit Card、QiWi</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="countryId">账单地址国家ID</param>
        /// <param name="currencyId">使用的币种ID</param>
        /// <returns>true表示能够使用，false表示不能使用</returns>
        bool CanUseOceanPayment(string method, int customerId, int countryId, int currencyId);

        #endregion

        #region 支付方式屏蔽国家

        /// <summary>
        /// 得到GC屏蔽国家
        /// </summary>
        /// <param name="countryId">国家ID</param>
        GlobalCollectDisabledCountry GetGlobalCollectDisabledCountryById(int countryId);

        /// <summary>
        /// 是否是GC屏蔽国家
        /// </summary>
        /// <param name="countryId">国家ID</param>
        bool IsGlobalCollectDisabledCountry(int countryId);

        /// <summary>
        /// 添加GC屏蔽国家
        /// </summary>
        /// <param name="globalCollectDisabledCountry"></param>
        void AddGlobalCollectDisabledCountry(GlobalCollectDisabledCountry globalCollectDisabledCountry);

        /// <summary>
        /// 删除GC屏蔽国家
        /// </summary>
        /// <param name="countryId">国家ID</param>
        void DeleteGlobalCollectDisabledCountryById(int countryId);

        /// <summary>
        /// 搜索GC屏蔽国家列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的GC屏蔽国家列表</returns>
        PageData<GlobalCollectDisabledCountry> FindGlobalCollectDisabledCountries(int currentPage, int pageSize, IDictionary<DisabledCountrySearchCriteria, object> searchCriteria, IList<Sorter<DisabledCountrySorterCriteria>> sorterCriteria);
        #endregion

        #region 支付信息获取
        /// <summary>
        /// 查询Paypal支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        /// <returns></returns>
        PageData<PaypalInfo> FindPaypalInfos(int currentPage, int pageSize, IDictionary<PaypalInfoSearchCriteria, object> searchCriteria, IList<Sorter<PaypalInfoSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 查询GC支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        PageData<GlobalCollectInfo> FindGlobalCollectInfos(int currentPage, int pageSize, IDictionary<GlobalCollectInfoSearchCriteria, object> searchCriteria, IList<Sorter<GlobalCollectInfoSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 查询钱海支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        PageData<OceanPaymentInfo> FindOceanPaymentInfos(int currentPage, int pageSize, IDictionary<OceanPaymentInfoSearchCriteria, object> searchCriteria, IList<Sorter<OceanPaymentInfoSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 查询中国银行转账支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        PageData<BankOfChinaInfo> FindBankOfChinaInfos(int currentPage, int pageSize, IDictionary<BankOfChinaInfoSearchCriteria, object> searchCriteria, IList<Sorter<BankOfChinaInfoSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 查询HSBC支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        PageData<HsbcInfo> FindHsbcInfos(int currentPage, int pageSize, IDictionary<HsbcInfoSearchCriteria, object> searchCriteria, IList<Sorter<HsbcInfoSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 查询MoneyGram支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        PageData<MoneyGramInfo> FindMoneyGramInfos(int currentPage, int pageSize, IDictionary<MoneyGramInfoSearchCriteria, object> searchCriteria, IList<Sorter<MoneyGramInfoSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 查询西联汇款支付信息
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">分页条件</param>
        PageData<WesternUnionInfo> FindWesternUnionInfos(int currentPage, int pageSize, IDictionary<WesternUnionInfoSearchCriteria, object> searchCriteria, IList<Sorter<WesternUnionInfoSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 根据ID获取Paypal支付信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PaypalInfo GetPaypalInfo(int id);

        /// <summary>
        /// 根据ID获取GC支付信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GlobalCollectInfo GetGlobalCollectInfo(int id);

        /// <summary>
        /// 根据ID获取钱海支付信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OceanPaymentInfo GetOceanPaymentInfo(int id);

        /// <summary>
        /// 根据ID获取中国银行转账信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BankOfChinaInfo GetBankOfChinaInfo(int id);

        /// <summary>
        /// 根据ID获取HSBC转账信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HsbcInfo GetHsbcInfo(int id);

        /// <summary>
        /// 根据ID获取MoneyGram汇款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MoneyGramInfo GetMoneyGramInfo(int id);

        /// <summary>
        /// 根据ID获取西联汇款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WesternUnionInfo GetWesternUnionInfo(int id);

        /// <summary>
        /// 根据订单号获取Paypal支付信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<PaypalInfo> GetPaypalInfo(string orderNo);

        /// <summary>
        /// 根据订单号获取GC支付信息
        /// </summary> 
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<GlobalCollectInfo> GetGlobalCollectInfo(string orderNo);

        /// <summary>
        /// 根据订单号获取钱海支付信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<OceanPaymentInfo> GetOceanPaymentInfo(string orderNo);

        /// <summary>
        /// 根据订单号获取中国银行转账信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<BankOfChinaInfo> GetBankOfChinaInfo(string orderNo);

        /// <summary>
        /// 根据订单号获取HSBC转账信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<HsbcInfo> GetHsbcInfo(string orderNo);

        /// <summary>
        /// 根据订单号获取MoneyGram汇款信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<MoneyGramInfo> GetMoneyGramInfo(string orderNo);

        /// <summary>
        /// 根据订单号获取西联汇款信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        IList<WesternUnionInfo> GetWesternUnionInfo(string orderNo);

        #endregion

        #region 支付辅助
        /// <summary>
        ///生成GC唯一订单号
        /// </summary>
        /// <returns></returns>
        string GenerateGcOrderNo();
        /// <summary>
        /// 是否需要用美元来进行支付的币种
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        bool IsCurrencyUseUsdForPaypal(string currencyCode);

        #endregion

        #region 支付方式限制客户
        PageData<PaymentEnabledCustomer> FindPaymentEnabledCustomers(int page, int pageSize, Dictionary<PaymentEnabledCustomerSearchCriteria, object> searchCriteria, List<Sorter<PaymentEnabledCustomerSorterCriteria>> sorterCriteria);

        void SetPaymentEnabledCustomer(PaymentEnabledCustomer paymentEnabledCustomer);

        void DeletePaymentEnabledCustomerById(int id);
        #endregion

    }

    #region 支付查询排序条件

    public enum PaypalInfoSearchCriteria
    {

    }

    public enum PaypalInfoSorterCriteria
    {

    }

    public enum GlobalCollectInfoSearchCriteria
    {

    }

    public enum GlobalCollectInfoSorterCriteria
    {

    }

    public enum OceanPaymentInfoSearchCriteria
    {

    }

    public enum OceanPaymentInfoSorterCriteria
    {

    }

    public enum BankOfChinaInfoSearchCriteria
    {

    }

    public enum BankOfChinaInfoSorterCriteria
    {

    }

    public enum HsbcInfoSearchCriteria
    {

    }

    public enum HsbcInfoSorterCriteria
    {

    }

    public enum MoneyGramInfoSearchCriteria
    {

    }

    public enum MoneyGramInfoSorterCriteria
    {

    }

    public enum WesternUnionInfoSearchCriteria
    {

    }

    public enum WesternUnionInfoSorterCriteria
    {

    }

    public enum DisabledCountrySearchCriteria
    {
        KeyWord
    }

    public enum DisabledCountrySorterCriteria
    {
        DateCreated
    }


    public enum PaymentEnabledCustomerSearchCriteria
    {
        KeyWord,
        PaymentType
    }

    public enum PaymentEnabledCustomerSorterCriteria
    {
        DateCreated
    }
    #endregion
}
