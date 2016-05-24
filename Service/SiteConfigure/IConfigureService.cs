using System;
using System.Collections.Generic;
using Com.Panduo.Service.SiteConfigure.Payment;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 站点配置相关的接口
    /// </summary>
    public interface IConfigureService
    {
        #region 常量
        /// <summary>
        /// 配置项不存在
        /// </summary>
        string ERROR_KEY_NOT_EXIST { get; }



        /// <summary>
        /// 白名单已经存在
        /// </summary>
        string ERROR_WHITELIST_EXIST { get; }


        /// <summary>
        /// 白名单不存在
        /// </summary>
        string ERROR_WHITELIST_NOT_EXIST { get; }


        /// <summary>
        /// 黑名单已经存在
        /// </summary>
        string ERROR_BLACKLIST_EXIST { get; }


        /// <summary>
        /// 黑名单不存在
        /// </summary>
        string ERROR_BLACKLIST_NOT_EXIST { get; }

        /// <summary>
        /// 币种不存在
        /// </summary>
        string ERROR_CURRENCY_NOT_EXIST { get; }

        /// <summary>
        /// 币种汇率不存在
        /// </summary>
        string ERROR_CURRENCY_RATE_NOT_EXIST { get; }

        /// <summary>
        /// 国家不存在
        /// </summary>
        string ERROR_COUNTRY_NOT_EXIST { get; }


        /// <summary>
        /// 关键词不存在
        /// </summary>
        string ERROR_SEACH_KEYWORD_NOT_EXIST { get; }

        /// <summary>
        /// 汇率小于零
        /// </summary>
        string ERROR_RATE_LESS_THAN_ZERO { get; }

        /// <summary>
        /// 美元的币种
        /// </summary>
        string CURRENCY_CODE_USD { get; }

        /// <summary>
        /// 英语的站点
        /// </summary>
        string LANGUAGE_CODE_EN { get; }
        #endregion

        #region 属性
        /// <summary>
        /// 是否IP限制
        /// </summary>
        bool IsIpAddressLimit { get; set; }

        /// <summary>
        /// 是否促销
        /// </summary>
        bool IsPromotion
        {
            get;
            set;
        }

        /// <summary>
        /// 促销开始时间
        /// </summary>
        DateTime? PromotionDateBegin
        {
            get;
            set;
        }

        /// <summary>
        /// 促销截止时间
        /// </summary>
        DateTime? PromotionDateEnd
        {
            get;
            set;
        }

        #region Dailydeal 有效期
        /// <summary>
        /// Dailydeal 有效期开始
        /// </summary>
        DateTime? DailydealsDateBegin
        {
            get;
            set;
        }

        /// <summary>
        /// Dailydeal 有效期结束
        /// </summary>
        DateTime? DailydealsDateEnd
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 匿名用户购物车数量
        /// </summary>
        int AnonymousUsersShoppingcartCount
        {
            get;
            set;
        }

        /// <summary>
        /// 原图地址
        /// </summary>
        string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 原图批量地址
        /// </summary>
        string ImageBatchUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 错误邮箱
        /// </summary>
        string ErrorMail
        {
            get;
            set;
        }

        /// <summary>
        ///  用户建议邮箱
        /// </summary>
        string SuggestionMail
        {
            get;
            set;
        }

        /// <summary>
        /// 系统邮箱
        /// </summary>
        string SystemMail
        {
            get;
            set;
        }

        /// <summary>
        /// 客户地址上限个数
        /// </summary>
        int CustomersAddressMaxCount
        {
            get;
            set;
        }


        int RecentlyViewedMaxCount
        {
            get;
            set;
        }

        /// <summary>
        /// 站点当前语种
        /// </summary>
        string SiteLanguageCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点当前语种Id
        /// </summary>
        int SiteLanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 站点当前语种Id
        /// </summary>
        int EnglishLangId
        {
            get;
            set;
        }

        /// <summary>
        /// 当前使用的IP库
        /// </summary>
        string SiteIpLibrary
        {
            get;
            set;
        }

        #endregion

        #region Coupon
        /// <summary>
        /// 非club客户的运费
        /// </summary>
        decimal ShippingFeeBefore
        {
            get;
            set;
        }

        /// <summary>
        /// 专属优惠券
        /// </summary>
        decimal ExclusiveCoupon
        {
            get;
            set;
        }

        /// <summary>
        /// club客户的运费
        /// </summary>
        decimal ShippingFeeAfter
        {
            get;
            set;
        }

        /// <summary>
        /// 手续费
        /// </summary>
        decimal HandlingFee
        {
            get;
            set;
        }
        #endregion

        #region IP管理
        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="whitelist"></param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_WHITELIST_EXIST:白名单已经存在</value>
        /// </exception>
        void AddWhiteList(WhiteList whitelist);

        /// <summary>
        /// 删除白名单
        /// </summary>
        /// <param name="whiteListId">白名单ID</param>
        void DeleteWhiteListById(int whiteListId);

        /// <summary>
        ///  获取白名单
        /// </summary>
        /// <returns>List</returns>
        IList<WhiteList> GetWhiteList();

        /// <summary>
        /// 删除黑名单
        /// </summary>
        /// <param name="blackListId">黑名单ID</param>
        void DeleteBlackListById(int blackListId);

        /// <summary>
        /// 添加黑名单
        /// </summary>
        /// <param name="blackList">黑名单</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_BLACKLIST_EXIST:黑名单已经存在</value>
        /// </exception>
        void AddBlackList(BlackList blackList);

        /// <summary>
        /// 获取黑名单
        /// </summary>
        /// <returns>列表</returns>
        IList<BlackList> GetBlackList();

        /// <summary>
        ///当前IP是否允许访问
        /// </summary>
        /// <param name="ipAddress">IP</param>
        /// <returns>是否允许访问</returns>
        bool IsIpAllowVisit(string ipAddress);

        #endregion

        #region 币种、汇率管理
        /// <summary>
        /// 设置币种显示状态
        /// </summary>
        /// <param name="currencyId">币种</param>
        /// <param name="status">状态值</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CURRENCY_NOT_EXIST:币种不存在</value>
        /// </exception>
        void SetCurrencyStatusById(int currencyId, bool status);

        /// <summary>
        /// 通过币种Id获取币种
        /// </summary>
        /// <returns>返回对应币种</returns>
        Currency GetCurrency(int currencyId);

        /// <summary>
        /// 获取所有的币种
        /// </summary>
        /// <returns>返回所有币种</returns>
        IList<Currency> GetAllCurrencies();

        /// <summary>
        /// 获取有效的币种
        /// </summary>
        /// <returns>返回有效的币种列表</returns>
        IList<Currency> GetAllValidCurrencies();

        /// <summary>
        /// 获取远程币种的汇率
        /// </summary>
        /// <returns>返回有效的币种列表</returns>
        IList<Currency> GetAllRemoteCurrencies();

        /// <summary>
        /// 修改汇率
        /// </summary>
        /// <param name="currency">币种实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_CURRENCY_NOT_EXIST:币种不存在</value>
        /// <value>ERROR_RATE_LESS_THAN_ZERO:汇率小于零</value>
        /// </exception>
        /// <returns></returns>
        /// <remarks>获取时间不能修改<br/>记录汇率日志ExchangeRateLog</remarks>
        void ModfiyRate(Currency currency);

        /// <summary>
        /// 批量修改汇率
        /// </summary>
        /// <param name="currencies"></param>
        /// <param name="adminId"></param>
        void ModfiyRates(IList<KeyValuePair<int, decimal>> currencies, int adminId);

        /// <summary>
        /// 获取单币种汇率
        /// </summary>
        /// <param name="currencyId">币种id</param>
        /// <returns>返回单币种汇率</returns>
        decimal GetSingleCurrencyRate(int currencyId);

        /// <summary>
        /// 获取单币种汇率
        /// </summary>
        /// <param name="currencyCode">币种编码</param>
        /// <returns>返回单币种汇率</returns>
        decimal GetSingleCurrencyRate(string currencyCode);

        /// <summary>
        /// 根据币种编号获取币种信息
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        Currency GetCurrencyByCode(string currencyCode);

        /// <summary>
        /// 获取汇率日志分页
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>返回带分页的汇率日志列表</returns>
        PageData<ExchangeRateLog> FindRateLog(int currentPage, int pageSize, IDictionary<ExchangeRateLogCriteria, object> searchCriteria, IList<Sorter<ExchangeRateLogSorterCriteria>> sorterCriteria);

        #endregion

        #region 大洲，国家，省，城市管理

        #region 大洲
        /// <summary>
        /// 获取所有大洲
        /// </summary>
        /// <returns>返回所有大洲列表</returns>
        IList<Continent> GetAllContinent();
        #endregion

        #region 国家
        /// <summary>
        /// 设置国家地址格式
        /// </summary>
        /// <param name="countryId">国家ID</param>
        /// <param name="format">格式</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_COUNTRY_NOT_EXIST:国家不存在</value>
        /// </exception>
        void SetCountryAddressFormat(int countryId, string format);

        /// <summary>
        /// 设置国家显示隐藏
        /// </summary>
        /// <param name="countryId">国家Id</param>
        /// <param name="isHidden">是否隐藏</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_COUNTRY_NOT_EXIST:国家不存在</value>
        /// </exception>
        void SetCountryHidden(int countryId, bool isHidden);

        /// <summary>
        /// 获取所有国家
        /// </summary>
        /// <returns>返回所有国家列表</returns>
        IList<Country> GetAllCountry();

        /// <summary>
        /// 根据大洲获取所有国家
        /// </summary>
        /// <param name="continentId">大洲id</param>
        /// <returns>返回所有国家列表</returns>
        IList<Country> GetAllCountryByContinent(int continentId);

        /// <summary>
        /// 获取常用国家
        /// </summary>
        /// <returns>返回所有常用国家列表</returns>
        IList<Country> GetCommonCountry();

        /// <summary>
        /// 获取非常用国家
        /// </summary>
        /// <returns>返回所有非常用国家列表</returns>
        IList<Country> GetUnCommonCountry();

        /// <summary>
        /// 获取所有有效国家
        /// </summary>
        /// <returns>返回所有有效国家列表</returns>
        IList<Country> GetAllValidCountry();

        /// <summary>
        /// 获取所有国家多语种列表
        /// </summary>
        /// <returns>返回所有国家多语种列表</returns>
        IList<CountryLanguage> GetAllCountryLanguages();

        /// <summary>
        /// 通过国家ID获取国家多语种列表
        /// </summary>
        /// <returns>返回国家多语种列表</returns>
        IList<CountryLanguage> GetCountryLanguages(int countryId);

        /// <summary>
        /// 获取单个国家多语种列表
        /// </summary>
        /// <param name="countryId">国家Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>返回国家多语种信息</returns>
        CountryLanguage GetCountryLanguage(int countryId, int languageId);

        /// <summary>
        /// 设置常用国家
        /// </summary>
        /// <param name="countryId">国家Id</param>
        /// <param name="flag">是否常用国家</param>
        /// <param name="displayOrder">排序</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_COUNTRY_NOT_EXIST:国家不存在</value>
        /// </exception>
        void SetCommonCountry(int countryId, bool flag, int displayOrder);

        /// <summary>
        /// 获取国家地址格式
        /// </summary>
        /// <param name="countryId">国家Id</param>
        /// <returns>获取国家地址格式</returns>
        string GetCountryAddressFormat(int countryId);

        /// <summary>
        /// 通过IP获取国家简码（调用IP库）
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns></returns>
        string GetCountrySimpleCode2ByIp(string ipAddress);

        /// <summary>
        /// 通过二位简码获取国家
        /// </summary>
        /// <param name="simpleCode2">二位简码</param>
        /// <returns>返回国家信息</returns>
        Country GetCountryBySimpleCode2(string simpleCode2);

        /// <summary>
        /// 通过IP获取国家
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>返回国家信息</returns>
        Country GetCountryByIp(string ipAddress);

        /// <summary>
        /// 获取单个国家信息
        /// </summary>
        /// <param name="countryId">国家ID</param>
        /// <returns>返回国家信息</returns>
        Country GetCountryById(int countryId);

        /// <summary>
        /// 通过国家Id获取国家
        /// </summary>
        /// <param name="countryIds">国家Ids</param>
        /// <param name="decollator">分隔符</param>
        /// <returns>对应的国家名称</returns>
        string GetCoutryNameByIds(string countryIds, string decollator);

        /// <summary>
        /// 通过语种Id获取语种
        /// </summary>
        /// <param name="languageIds">语种Ids</param>
        /// <param name="decollator">分隔符</param>
        /// <returns></returns>
        string GetLanguageNameByIds(string languageIds, string decollator);
        #endregion

        #region 省（州，郡）
        /// <summary>
        /// 获取国家所有的省（州，郡）
        /// </summary>
        /// <param name="countryId">国家ID</param>
        /// <returns></returns>
        IList<Province> GetAllProvinceByCountryId(int countryId);


        /// <summary>
        /// 获取所有省（州，郡）多语种列表
        /// </summary>
        /// <returns>返回所有省（州，郡）多语种列表</returns>
        IList<ProvinceLanguage> GetAllProvinceLanguages();


        /// <summary>
        /// 通过省ID获取省（州，郡）多语种列表
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <returns>返回省（州，郡）多语种列表</returns>
        IList<ProvinceLanguage> GetProvinceLanguages(int provinceId);


        /// <summary>
        /// 获取单个省（州，郡）多语种列表
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="languageid">语种ID</param>
        /// <returns>返回省（州，郡）多语种信息</returns>
        ProvinceLanguage GetProvinceLanguage(int provinceId, int languageid);
        #endregion

        #region 城市
        /// <summary>
        /// 获取州所有的城市
        /// </summary>
        /// <param name="provinceId">省Id</param>
        /// <returns></returns>
        IList<City> GetAllCityByProvinceId(int provinceId);


        /// <summary>
        /// 获取所有城市多语种列表
        /// </summary>
        /// <returns>返回所有城市多语种列表</returns>
        IList<CityLanguage> GetAllCityLanguages();

        /// <summary>
        /// 通过城市ID获取城市多语种列表
        /// </summary>
        /// <param name="cityId">城市Id</param>
        /// <returns>返回城市多语种列表</returns>
        IList<CityLanguage> GetCityLanguages(int cityId);


        /// <summary>
        /// 获取单个城市多语种列表
        /// </summary>
        /// <param name="cityId">城市Id</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>返回城市多语种信息</returns>
        CityLanguage GetCityLanguage(int cityId, int languageId);
        #endregion

        #region 高危国家

        /// <summary>
        /// 是否是高危国家
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        bool IsCountryHighRisk(int countryId);

        /// <summary>
        /// 批量添加高危国家
        /// </summary>
        /// <param name="countryHighRisks"></param>
        void AddCountryHighRisks(IList<CountryHighRisk> countryHighRisks);

        /// <summary>
        /// 得到所有的高危国家
        /// </summary>
        /// <returns></returns>
        IList<CountryHighRisk> GetAllCountryHighRisks();

            #endregion

        #endregion

        #region 网站联系方式
        /// <summary>
        /// 网站联系方式
        /// </summary>
        /// <returns>SiteContact</returns>
        SiteContact GetContact();
        #endregion

        #region 获取网站支付方式
        /// <summary>
        /// 获取GC支付方式配置
        /// </summary>
        /// <returns></returns>
        GlobalCollectConfig GetGlobalCollectPaymentConfig();

        /// <summary>
        /// 获取Alipay支付方式配置
        /// </summary>
        /// <returns></returns>
        AlipayConfig GetAlipayPaymentConfig();

        /// <summary>
        /// 获取PayPal支付方式配置
        /// </summary>
        /// <returns></returns>
        PayPalConfig GetPaypalPaymentConfig();

        /// <summary>
        /// 获取Qiwi支付方式配置
        /// </summary>
        /// <returns></returns>
        QiwiConfig GetQiwiPaymentConfig();
        #endregion

        #region 公共广告
        /// <summary>
        /// 获取公共广告，not found page的 
        /// </summary>
        /// <returns>返回公共广告内容</returns>
        string GetCommonAdvertisement();
        #endregion

        #region 搜索关键词
        /// <summary>
        /// 设置搜索关键词
        /// </summary>
        /// <param name="word">SearchKeyword</param>
        /// <remarks>修改或添加搜索关键词，根据语种id，类型和名称来判断是添加还是修改</remarks>
        void SetSearchKeyword(SearchKeyword word);

        /// <summary>
        /// 删除搜索关键词
        /// </summary>
        /// <param name="keywordId">搜索关键词Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_SEACH_KEYWORD_NOT_EXIST:搜索关键词不存在</value>
        /// </exception>
        /// <returns></returns>
        void DeleteSeachKeywordById(int keywordId);

        /// <summary>
        /// 获取搜索关键词
        /// </summary>
        /// <param name="keywordId">搜索关键词Id</param>
        /// <returns>搜索关键词</returns>
        SearchKeyword GetSearchKeyword(int keywordId);

        /// <summary>
        /// 根据关键字类型查询搜索关键字，2014-12-26 add
        /// </summary>
        /// <returns>搜索关键字列表</returns>
        IList<SearchKeyword> GetSearchKeywordByType(KeywordType type);

        /// <summary>
        /// 搜索关键词分页
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页码尺寸</param>
        /// <param name="searchCriteria">搜索条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<SearchKeyword> FindSearchKeyword(int currentPage, int pageSize, IDictionary<SearchKeywordCriteria, object> searchCriteria, IList<Sorter<SearchKeywordSorterCriteria>> sorterCriteria);
        #endregion

        #region Config
        void SetConfig(Config config);

        void SetConfig(List<Config> configs);

        void UpdateConfig(string key, string value);

        Config GetConfig(string key);
        #endregion

        /// <summary>
        /// 获取所有有效的语种
        /// </summary>
        /// <returns>返回所有有效的语种列表</returns>
        IList<Language> GetAllValidLanguage();
    }

    /// <summary>
    /// 汇率日志查询条件
    /// </summary>
    public enum ExchangeRateLogCriteria
    {
        /// <summary>
        /// 币种编码,精确查询
        /// </summary>
        CurrencyCode,
        /// <summary>
        /// 修改人,精确查询
        /// </summary>
        ModifiyId,

        /// <summary>
        /// 修改开始时间
        /// </summary>
        CreateDateFrom,

        /// <summary>
        /// 修改结束时间
        /// </summary>
        CreateDateTo,
    }

    /// <summary>
    /// 汇率日志排序条件
    /// </summary>
    public enum ExchangeRateLogSorterCriteria
    {
        /// <summary>
        /// 币种编码,精确查询
        /// </summary>
        CurrencyCode,
        /// <summary>
        /// 修改人,精确查询
        /// </summary>
        ModifiyId,
        /// <summary>
        /// 日志Id
        /// </summary>
        LogId,

        CreateDate,
    }

    public enum SearchKeywordCriteria
    {
        /// <summary>
        /// 搜索关键词类型
        /// </summary>
        KeywordType
    }

    public enum SearchKeywordSorterCriteria
    {
        
    }
}
