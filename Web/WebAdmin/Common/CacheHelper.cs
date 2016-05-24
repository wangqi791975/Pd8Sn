using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 网站缓存辅助类
    /// </summary>
    public class CacheHelper
    {

        #region 后台缓存
        /// <summary>
        /// 页面公共数据:所有语种
        /// </summary>
        public const string ADMIN_KEY_PUBLICDATA_LANGUAGE = "ADMIN_KEY_PUBLICDATA_LANGUAGE";
        /// <summary>
        /// 页面公共数据:所有语种
        /// </summary>
        public static List<Language> Languages
        {
            get
            {
                var languagess = CacheManager.Instance.GetCache(ADMIN_KEY_PUBLICDATA_LANGUAGE) as IList<Language>;
                if (languagess == null)
                {
                    languagess = ServiceFactory.ConfigureService.GetAllValidLanguage();
                    CacheManager.Instance.SetCache(ADMIN_KEY_PUBLICDATA_LANGUAGE, languagess ?? new List<Language>());
                }
                return languagess.IsNullOrEmpty() ? new List<Language>() : languagess.ToList();
            }
        }


        private const string ADMIN_KEY_ALL_SHIPPING_DESCS = "ADMIN_KEY_ALL_SHIPPING_DESCS";
        /// <summary>
        /// 当前语种的所有运送方式多语种信息
        /// </summary>
        public static IList<ShippingLanguage> GetAllShippingDescs
        {
            get
            {
                var list = CacheManager.Instance.GetCache(ADMIN_KEY_ALL_SHIPPING_DESCS) as IList<ShippingLanguage>;
                if (list == null)
                {
                    list = ServiceFactory.ShippingService.GetAllShippingDescs(ServiceFactory.ConfigureService.SiteLanguageId);
                    CacheManager.Instance.SetCache(ADMIN_KEY_ALL_SHIPPING_DESCS, list ?? new List<ShippingLanguage>());
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

        #region 前台缓存清除

        #region 所有缓存（服务层(Memcache)和IIS)
        public static void ClearAllCache()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearAllCache?token={1}", lang.Host,
                        Web.Common.ConfigHelper.ClearSiteCacheToken),
                    null);
            }
        }
        #endregion

        #region 所有语种
        public const string PAGE_KEY_PUBLICDATA_LANGUAGE = "PAGE_KEY_PUBLICDATA_LANGUAGE";
        public static void ClearLanguageIis()
        {
            CacheManager.Instance.ClearCache(ADMIN_KEY_PUBLICDATA_LANGUAGE);//后台本身语种缓存先清除
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                        lang.Host,
                        Web.Common.ConfigHelper.ClearSiteCacheToken, PAGE_KEY_PUBLICDATA_LANGUAGE),
                    null);
            }
        }
        #endregion

        #region AllNavigationHomeAreaSetting
        public const string CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING = "CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING";
        public static void ClearAllNavigationHomeAreaSetting()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}", lang.Host,
                    Web.Common.ConfigHelper.ClearSiteCacheToken, CACHE_KEY_ALL_NAVIGATION_HOME_AREA_SETTING),
                null);
            }
        }
        #endregion

        #region RightCategoryHomeAreaSetting
        public const string CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING = "CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING";
        public static void ClearAllRightCategoryHomeAreaSetting()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                        lang.Host,
                        Web.Common.ConfigHelper.ClearSiteCacheToken, CACHE_KEY_ALL_RIGHT_HOME_AREA_SETTING),
                    null);
            }
        }
        #endregion

        #region BelowCategoryHomeAreaSetting
        public const string CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING = "CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING";
        public static void ClearAllBelowCategoryHomeAreaSetting()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                         lang.Host,
                        Web.Common.ConfigHelper.ClearSiteCacheToken, CACHE_KEY_ALL_BELOW_HOME_AREA_SETTING),
                    null);
            }
        }
        #endregion

        #region 网站币种
        public const string PAGE_KEY_PUBLICDATA_CURRENCY = "PAGE_KEY_PUBLICDATA_CURRENCY";
        public static void ClearCurrencyIis()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                          lang.Host,
                        Web.Common.ConfigHelper.ClearSiteCacheToken, PAGE_KEY_PUBLICDATA_CURRENCY),
                    null);
            }
        }
        #endregion

        #region 网站左侧类别
        public const string PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE = "PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE";
        public static void ClearCategoryLanguageIis()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                        lang.Host,
                        Web.Common.ConfigHelper.ClearSiteCacheToken, PAGE_KEY_PUBLICDATA_CATEGORY_LANGUAGE),
                    null);
            }
        }
        #endregion

        #region 属性、属性值组、属性值
        public const string PAGE_KEY_PUBLICDATA_PROPERTY = "PAGE_KEY_PUBLICDATA_PROPERTY";
        public static void ClearPropertyIis()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                       lang.Host, Web.Common.ConfigHelper.ClearSiteCacheToken,
                        PAGE_KEY_PUBLICDATA_PROPERTY), null);
            }
        }

        public const string PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP = "PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP";

        public static void ClearPropertyValueGroupIis()
        {
            foreach (var lang in Languages)
            {
                ToolHelper.CreateHttpGet(
                    string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                        lang.Host, Web.Common.ConfigHelper.ClearSiteCacheToken,
                        PAGE_KEY_PUBLICDATA_PROPERTY_VALUE_GROUP), null);
            }
        }

        public const string PAGE_KEY_PUBLICDATA_PROPERTY_VALUE = "PAGE_KEY_PUBLICDATA_PROPERTY_VALUE";

        public static void ClearPropertyValueIis()
        {
            foreach (var lang in Languages)
            {
              ToolHelper.CreateHttpGet(
                        string.Format("{0}/WebSite/ClearWebCache?token={1}&cacheKey={2}",
                            lang.Host, Web.Common.ConfigHelper.ClearSiteCacheToken,
                            PAGE_KEY_PUBLICDATA_PROPERTY_VALUE), null);
            }
        }

        #endregion

        #endregion

    }
}
