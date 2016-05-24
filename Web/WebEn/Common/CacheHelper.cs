using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Marketing.Banner;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Common.Mvc.Model;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 网站缓存辅助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 加载IIS缓存
        /// </summary>
        public static void LoadCache()
        {
            
        }

        #region 网站展示公共数据

        #region 网站语种
        /// <summary>
        /// 页面公共数据:所有语种
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_LANGUAGE = "PAGE_KEY_PUBLICDATA_LANGUAGE";
        /// <summary>
        /// 页面公共数据:所有语种
        /// </summary>
        public static List<Language> Languages
        {
            get
            {
                var languagess = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_LANGUAGE) as IList<Language>;
                if (languagess == null)
                {
                    languagess = Service.ServiceFactory.ConfigureService.GetAllValidLanguage();
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_LANGUAGE, languagess ?? new List<Language>());
                }
                return languagess.IsNullOrEmpty() ? new List<Language>() : languagess.ToList();
            }
        }
        #endregion

        #region 网站币种
        /// <summary>
        /// 页面公共数据:网站币种
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_CURRENCY = "PAGE_KEY_PUBLICDATA_CURRENCY";
        /// <summary>
        /// 页面公共数据:网站币种
        /// </summary>
        public static List<Currency> Currencies
        {
            get
            {
                var currencies = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_CURRENCY) as IList<Currency>;
                if (currencies == null)
                {
                    currencies = Service.ServiceFactory.ConfigureService.GetAllValidCurrencies();
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_CURRENCY, currencies ?? new List<Currency>());
                }
                return currencies.IsNullOrEmpty() ? new List<Currency>() : currencies.ToList();
            }
        }



        /// <summary>
        /// 根据币种编码获取币种
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        public static Currency GetCurrencyByCode(string currencyCode)
        {
            if (Currencies.Any(c => c.CurrencyCode == currencyCode))
            {
                return Currencies.First(c => c.CurrencyCode == currencyCode);
            }

            return null;
        }
        #endregion

        #region 网站左侧类别
        /// <summary>
        /// 页面公共数据:网站类别
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE = "PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE";
        /// <summary>
        /// 页面公共数据:网站类别
        /// </summary>
        public static IList<RelatedData<CategoryLanguage>> CategoryLanguages
        {
            get
            {
                var categoryLanguages = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE) as IList<RelatedData<CategoryLanguage>>;
                if (categoryLanguages == null)
                {
                    categoryLanguages = Service.ServiceFactory.CategoryService.GetCategoryTreeRecursiveCache(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE, categoryLanguages ?? new List<RelatedData<CategoryLanguage>>());
                }
                return categoryLanguages.IsNullOrEmpty() ? new List<RelatedData<CategoryLanguage>>() : categoryLanguages.ToList();
            }
        }
        #endregion

        #region 网站左侧筛选属性
        /// <summary>
        /// 页面公共数据:网站属性
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_PROPERTY = "PAGE_KEY_PUBLICDATA_PROPERTY";
        /// <summary>
        /// 页面公共数据:网站属性
        /// </summary>
        public static IList<Property> Properties
        {
            get
            {
                var properties = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_PROPERTY) as IList<Property>;
                if (properties == null)
                {
                    properties = Service.ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_PROPERTY, properties ?? new List<Property>());
                }
                return properties.IsNullOrEmpty() ? new List<Property>() : properties.ToList();
            }
        }
        #endregion

        #region 网站左侧筛选属性值组
        /// <summary>
        /// 页面公共数据:网站属性值组
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP = "PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP";
        /// <summary>
        /// 页面公共数据:网站属性值组
        /// </summary>
        public static IList<PropertyValueGroup> PropertyValueGroups
        {
            get
            {
                var propertyValueGroups = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP) as IList<PropertyValueGroup>;
                if (propertyValueGroups == null)
                {
                    propertyValueGroups = Service.ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP, propertyValueGroups ?? new List<PropertyValueGroup>());
                }
                return propertyValueGroups.IsNullOrEmpty() ? new List<PropertyValueGroup>() : propertyValueGroups.ToList();
            }
        }
        #endregion

        #region 网站左侧筛选属性值
        /// <summary>
        /// 页面公共数据:网站属性值
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_PROPERTY_VALUE = "PAGE_KEY_PUBLICDATA_PROPERTY_VALUE";
        /// <summary>
        /// 页面公共数据:网站属性值
        /// </summary>
        public static IList<PropertyValue> PropertyValues
        {
            get
            {
                var propertyValues = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_PROPERTY_VALUE) as IList<PropertyValue>;
                if (propertyValues == null)
                {
                    propertyValues = Service.ServiceFactory.PropertyService.GetAllPropertyValuesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_PROPERTY_VALUE, propertyValues ?? new List<PropertyValue>());
                }
                return propertyValues.IsNullOrEmpty() ? new List<PropertyValue>() : propertyValues.ToList();
            }
        } 
        #endregion

        #region 全局搜索关键字
        /// <summary>
        /// 页面公共数据:搜索关键字
        /// </summary>
        public const string PAGE_KEY_PUBLICDATA_SEARCHKEYWORD = "PAGE_KEY_PUBLICDATA_SEARCHKEYWORD";
        /// <summary>
        /// 页面公共数据:搜索关键字
        /// </summary>
        public static List<SearchKeyword> SearchKeywords
        {
            get
            {
                var keywords = CacheManager.Instance.GetCache(PAGE_KEY_PUBLICDATA_SEARCHKEYWORD) as IList<SearchKeyword>;
                if (keywords == null)
                {
                    keywords = Service.ServiceFactory.ConfigureService.GetSearchKeywordByType(KeywordType.UnderBox);
                    CacheManager.Instance.SetCache(PAGE_KEY_PUBLICDATA_SEARCHKEYWORD, keywords ?? new List<SearchKeyword>());
                }
                return keywords.IsNullOrEmpty() ? new List<SearchKeyword>() : keywords.ToList();
            }
        }
        #endregion

        #endregion

        #region 网站路由配置
        /// <summary>
        /// 路由配置
        /// </summary>
        public const string CACHE_KEY_ROUTE_MAP = "CACHE_KEY_ROUTE_MAP";
        /// <summary>
        /// 获取
        /// </summary>
        public static IList<RouteObject> RountMaps
        {
            get
            {
                var rountMaps = CacheManager.Instance.GetCache(CACHE_KEY_ROUTE_MAP) as IList<RouteObject>;
                if (rountMaps == null)
                {
                    rountMaps = ConfigHelper.RountConfigs;
                    CacheManager.Instance.SetCache(CACHE_KEY_ROUTE_MAP, rountMaps ?? new List<RouteObject>());
                }
                return rountMaps;
            }
        }
        #endregion

        #region SSL缓存
        private const string CACHE_KEY_SSL = "CACHE_KEY_SSL";
        public static IDictionary<string, IDictionary<string, IList<string>>> SslConfigMap
        {
            get
            {
                var sslConfigMap = CacheManager.Instance.GetCache(CACHE_KEY_SSL) as IDictionary<string, IDictionary<string, IList<string>>>;
                if (sslConfigMap == null)
                {
                    sslConfigMap = ConfigHelper.SslConfigMap;
                    CacheManager.Instance.SetCache(CACHE_KEY_SSL, sslConfigMap ?? new Dictionary<string, IDictionary<string, IList<string>>>());
                }

                return sslConfigMap;
            }
        }

        #endregion

        #region 站点语言与默认货币对应关系
        private const string CACHE_KEY_DEFAULTCURRENCYWITHLANGUAGES = "CACHE_KEY_DEFAULTCURRENCYWITHLANGUAGES";
        /// <summary>
        /// 站点语言与默认货币对应关系
        /// <para>Key string: 币种编码 USD</para>
        /// <para>Value List string:语种集合 DE,FR,ES,IT,RU,JA</para>
        /// </summary>
        public static IDictionary<string, List<string>> DefaultCurrencyWithLanguages
        {
            get
            {
                var defaultCurrencyWithLanguages = CacheManager.Instance.GetCache(CACHE_KEY_DEFAULTCURRENCYWITHLANGUAGES) as Dictionary<string, List<string>>;
                if (defaultCurrencyWithLanguages == null)
                {
                    defaultCurrencyWithLanguages = ConfigHelper.DefaultCurrencyWithLanguages;
                    CacheManager.Instance.SetCache(CACHE_KEY_DEFAULTCURRENCYWITHLANGUAGES, defaultCurrencyWithLanguages ?? new Dictionary<string, List<string>>());
                }

                return defaultCurrencyWithLanguages;
            }
        }

        #endregion

        #region 国家 
        private const string CACHE_KEY_ALL_COUNTRIES = "CACHE_KEY_ALL_COUNTRIES";
        /// <summary>
        /// 所有国家
        /// </summary>
        public static IList<Country> AllCountries
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_COUNTRIES) as IList<Country>;
                if (list == null)
                {
                    list = ServiceFactory.ConfigureService.GetAllCountry();
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_COUNTRIES, list ?? new List<Country>());
                }

                return list;
            }
        }

        /// <summary>
        /// 根据ID获取国家编码
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static string GetCountryCode(int countryId)
        {
            if (AllCountries.Any(c=>c.CountryId == countryId))
            {
                return AllCountries.First(c => c.CountryId == countryId).SimpleCode2;
            }

            return string.Empty;
        }
        #endregion


        #region 客户订单状态
        private const string CACHE_KEY_ALL_CUSTOMER_ORDER_STATUS = "CACHE_KEY_ALL_CUSTOMER_ORDER_STATUS";
        public static IList<OrderStatus> GetAllCustomerOrderStatus
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_CUSTOMER_ORDER_STATUS) as IList<OrderStatus>;
                if (list == null)
                {
                    list = ServiceFactory.OrderService.GetAllCustomerOrderStatus(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_CUSTOMER_ORDER_STATUS, list ?? new List<OrderStatus>());
                }

                return list;
            }
        }
        #endregion


        #region 运送方式
        private const string CACHE_KEY_ALL_SHIPPINGS = "CACHE_KEY_ALL_SHIPPING";
        /// <summary>
        /// 所有运送方式
        /// </summary>
        public static IList<Shipping> GetAllShippings
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_SHIPPINGS) as IList<Shipping>;
                if (list == null)
                {
                    list = ServiceFactory.ShippingService.GetAllShippings();
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_SHIPPINGS, list ?? new List<Shipping>());
                }

                return list;
            }
        }

        /// <summary>
        /// 根据运送方式ID获取运送方式
        /// </summary>
        /// <param name="shippingId">运送方式ID</param>
        /// <returns>Shipping</returns>
        public static Shipping GetShippingById(int shippingId)
        {
            if (GetAllShippings.Any(c => c.ShippingId == shippingId))
            {
                return GetAllShippings.First(c => c.ShippingId == shippingId);
            }
            return null;
        }

        private const string CACHE_KEY_ALL_SHIPPING_DESCS = "CACHE_KEY_ALL_SHIPPING_DESCS";
        /// <summary>
        /// 当前语种的所有运送方式多语种信息
        /// </summary>
        public static IList<ShippingLanguage> GetAllShippingDescs
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_SHIPPING_DESCS) as IList<ShippingLanguage>;
                if (list == null)
                {
                    list = ServiceFactory.ShippingService.GetAllShippingDescs(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_SHIPPING_DESCS, list ?? new List<ShippingLanguage>());
                }

                return list;
            }
        }

        /// <summary>
        /// 根据运送方式ID获取运送方式名称
        /// </summary>
        /// <param name="shippingId">运送方式ID</param>
        /// <returns>运送方式名称</returns>
        public static string GetShippingName(int shippingId)
        {
            if (GetAllShippingDescs.Any(c => c.ShippingId == shippingId))
            {
                return GetAllShippingDescs.First(c => c.ShippingId == shippingId).Name;
            }

            return string.Empty;
        }
        #endregion

        #region IP检查 
        private const string CACHE_KEY_IS_IP_CHECK = "CACHE_KEY_IS_IP_CHECK";
        /// <summary>
        /// 是否检查IP
        /// </summary>
        public static bool IsIpCheck
        {
            get
            {
                var isIpCheck = CacheManager.Instance.GetCache(CACHE_KEY_IS_IP_CHECK) as bool?;
                if (isIpCheck == null)
                {
                    isIpCheck = ServiceFactory.ConfigureService.IsIpAddressLimit;
                    CacheManager.Instance.SetCache(CACHE_KEY_IS_IP_CHECK, isIpCheck);
                }

                return isIpCheck.Value;
            }
        }
        #endregion

        #region HomeAreaSetting
        private const string CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING = "CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING";
        /// <summary>
        /// 获取首页相关导航内容
        /// </summary>
        public static IList<HomeAreaSetting> AllNavigationHomeAreaSetting
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING) as IList<HomeAreaSetting>;
                if (list == null)
                {
                    list = ServiceFactory.BannerService.GetHomeAreaSetting(HomeAreaType.Navigation);
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING, list ?? new List<HomeAreaSetting>());
                }
                return list;
            }
        }

        /// <summary>
        /// 根据语种Id获取首页相关导航内容
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public static string GetHomeAreaNavigation(int languageId)
        {
            if (AllNavigationHomeAreaSetting.Any(c => c.LanguageId == languageId))
            {
                return AllNavigationHomeAreaSetting.First(c => c.LanguageId == languageId).Content;
            }

            return string.Empty;
        }


        private const string CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING = "CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING";

        /// <summary>
        /// 获取首页相关右边类别内容
        /// </summary>
        public static IList<HomeAreaSetting> AllRightCategoryHomeAreaSetting
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING) as IList<HomeAreaSetting>;
                if (list == null)
                {
                    list = ServiceFactory.BannerService.GetHomeAreaSetting(HomeAreaType.RightCategory);
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING, list ?? new List<HomeAreaSetting>());
                }
                return list;
            }
        }

        /// <summary>
        /// 根据语种Id获取首页相关右边类别内容
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public static string GetHomeAreaRightCategory(int languageId)
        {
            if (AllRightCategoryHomeAreaSetting.Any(c => c.LanguageId == languageId))
            {
                return AllRightCategoryHomeAreaSetting.First(c => c.LanguageId == languageId).Content;
            }
            return string.Empty;
        }

        private const string CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING = "CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING";

        /// <summary>
        /// 获取首页相关底部类别内容列表
        /// </summary>
        public static IList<HomeAreaSetting> AllBelowCategoryHomeAreaSetting
        {
            get
            {
                var list = CacheManager.Instance.GetCache(CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING) as IList<HomeAreaSetting>;
                if (list == null)
                {
                    list = ServiceFactory.BannerService.GetHomeAreaSetting(HomeAreaType.BelowCategory);
                    CacheManager.Instance.SetCache(CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING, list ?? new List<HomeAreaSetting>());
                }

                return list;
            }
        }

        /// <summary>
        /// 根据语种Id获取首页相关底部类别内容
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public static string GetHomeAreaBelowCategory(int languageId)
        {
            if (AllBelowCategoryHomeAreaSetting.Any(c => c.LanguageId == languageId))
            {
                return AllBelowCategoryHomeAreaSetting.First(c => c.LanguageId == languageId).Content;
            }
            return string.Empty;
        }
        #endregion
    }
}
