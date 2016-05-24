using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Service.Customer;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Common
{
    public partial class CookieHelper
    {
        #region 客户信息相关
        /// <summary>
        /// 客户登陆错误次数
        /// </summary>
        private static readonly string COOKIE_KEY_LOGINERRORCOUNT = "COOKIE_KEY_LOGINERRORCOUNT";
        /// <summary>
        /// 客户登陆错误次数
        /// </summary>
        public static int LoginErrorCount
        {
            get
            {
                var loginErrCount = CookieManager.GetCookie(COOKIE_KEY_LOGINERRORCOUNT);
                return loginErrCount.ParseTo(0);
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_LOGINERRORCOUNT, value.ToString(), DateTime.Now.AddDays(1));
            }
        }

        /// <summary>
        /// Cookie记录当前登陆客户的Preference
        /// </summary>
        private static readonly string COOKIE_KEY_CUSTOMER_PREFERENCE = "COOKIE_KEY_CUSTOMER_PREFERENCE";
        /// <summary>
        /// Cookie记录当前登陆客户的Preference
        /// </summary>
        public static Preference CurrentCustomerPreference
        {
            get
            {
                var strPreference = CookieManager.GetCookie(COOKIE_KEY_CUSTOMER_PREFERENCE);
                if (!strPreference.IsNullOrEmpty())
                {
                    var preference = strPreference.FromJson<Preference>();
                    return preference;
                }
                return new Preference();
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_CUSTOMER_PREFERENCE, value.ToJson(), DateTime.Now.AddDays(7));
            }
        }
        #endregion

        #region Product页面Cookie

        /// <summary>
        /// Cookie记录当前登陆客户最近看的商品编号
        /// </summary>
        private static readonly string COOKIE_KEY_PRODUCT_RECENTLYVIEWED = "COOKIE_KEY_PRODUCT_RECENTLYVIEWED";
        /// <summary>
        /// Cookie记录当前登陆客户最近看的商品编号
        /// </summary>
        public static List<int> RecentlyViewedProductList
        {
            get
            {
                var recentlyViewedProduct = CookieManager.GetCookie(COOKIE_KEY_PRODUCT_RECENTLYVIEWED);
                if (!recentlyViewedProduct.IsNullOrEmpty())
                {
                    var recentlyViewedProductList = recentlyViewedProduct.FromJson<List<int>>();
                    return recentlyViewedProductList;
                }
                return null;
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_PRODUCT_RECENTLYVIEWED, value.ToJson(), DateTime.Now.AddDays(7));
            }
        }
        #endregion

        #region Product PageSize页面Cookie

        /// <summary>
        /// Cookie记录当前登陆客户最近看的商品编号
        /// </summary>
        private static readonly string COOKIE_KEY_PRODUCT_PAGESIZE = "COOKIE_KEY_PRODUCT_PAGESIZE";
        /// <summary>
        /// Cookie记录当前登陆客户最近看的Product PageSize页面Cookie
        /// </summary>
        public static int ProductPageSize
        {
            get
            {
                var recentlyViewedProduct = CookieManager.GetCookie(COOKIE_KEY_PRODUCT_PAGESIZE);
                if (!recentlyViewedProduct.IsNullOrEmpty())
                {
                    var productPageSize = recentlyViewedProduct.FromJson<int>();
                    return productPageSize;
                }
                return default(int);
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_PRODUCT_PAGESIZE, value.ToJson(), DateTime.Now.AddDays(7));
            }
        }
        #endregion


        #region 购物车
        /// <summary>
        /// 当前客户购物车
        /// </summary>
        private const string COOKIE_KEY_CURRENT_SHOPPINGCART_ID = "COOKIE_KEY_CURRENT_SHOPPINGCART_ID";
        /// <summary>
        /// 当前客户购物车Id : 登陆用户未用户ID，未登陆用户未临时ID
        /// </summary>
        public static int? CurrentShoppingCartId
        {
            get
            {
                var shoppingCartId = CookieManager.GetCookie(COOKIE_KEY_CURRENT_SHOPPINGCART_ID);
                if (!shoppingCartId.IsNullOrEmpty())
                    return shoppingCartId.FromJson<int>();

                return null;
            }
            set { CookieManager.SetCookie(COOKIE_KEY_CURRENT_SHOPPINGCART_ID, value.ToJson(), DateTime.Now.AddDays(15)); }
        }

        /// <summary>
        /// 当前客户购物车排序模式
        /// </summary>
        private const string COOKIE_KEY_CURRENT_SHOPPINGCART_LISTSORTMODE = "COOKIE_KEY_CURRENT_SHOPPINGCART_LISTSORTMODE";
        public static int CurrentShoppingCartListSortMode
        {
            get
            {
                var shoppingCartListSortMode = CookieManager.GetCookie(COOKIE_KEY_CURRENT_SHOPPINGCART_LISTSORTMODE);
                return !shoppingCartListSortMode.IsNullOrEmpty() ? shoppingCartListSortMode.FromJson<int>() : 0;
            }
            set { CookieManager.SetCookie(COOKIE_KEY_CURRENT_SHOPPINGCART_LISTSORTMODE, value.ToJson(), DateTime.Now.AddDays(15)); }
        }
        #endregion

        #region PlaceOrder页面

        /// <summary>
        /// 记录客户选择的国家ID
        /// </summary>
        private static readonly string COOKIE_KEY_CUSTOMER_COUNTRY_ID = "COOKIE_KEY_CUSTOMER_COUNTRY_ID";
        /// <summary>
        /// 客户选择的国家ID
        /// </summary>
        public static int CustomerCountryId
        {
            get
            {
                var customerAddressId = CookieManager.GetCookie(COOKIE_KEY_CUSTOMER_COUNTRY_ID);
                return customerAddressId.ParseTo(0);
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_CUSTOMER_COUNTRY_ID, value.ToString(), DateTime.Now.AddDays(15));
            }
        }

        /// <summary>
        /// 记录客户选择的配送地址ID
        /// </summary>
        private static readonly string COOKIE_KEY_CUSTOMER_ADDRESS_ID = "COOKIE_KEY_CUSTOMER_ADDRESS_ID";
        /// <summary>
        /// 客户选择的配送地址ID
        /// </summary>
        public static int CustomerAddressId
        {
            get
            {
                var customerAddressId = CookieManager.GetCookie(COOKIE_KEY_CUSTOMER_ADDRESS_ID);
                return customerAddressId.ParseTo(0);
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_CUSTOMER_ADDRESS_ID, value.ToString(), DateTime.Now.AddDays(15));
            }
        }

        /// <summary>
        /// 记录客户选择的配送方式ID
        /// </summary>
        private static readonly string COOKIE_KEY_CUSTOMER_SHIPPING_ID = "COOKIE_KEY_CUSTOMER_SHIPPING_ID";
        /// <summary>
        /// 客户选择的配送方式ID
        /// </summary>
        public static int CustomerShippingId
        {
            get
            {
                var customerShippingId = CookieManager.GetCookie(COOKIE_KEY_CUSTOMER_SHIPPING_ID);
                return customerShippingId.ParseTo(0);
            }
            set
            {
                CookieManager.SetCookie(COOKIE_KEY_CUSTOMER_SHIPPING_ID, value.ToString(), DateTime.Now.AddDays(15));
            }
        }
        #endregion

    }
}