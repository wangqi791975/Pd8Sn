using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service;
using Com.Panduo.Service.AdminUser;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Com.Panduo.Common;
using Com.Panduo.Service.Customer;
using Com.Panduo.Web.PaymentCommon.Service.Parm.GlobalCollect;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// Session辅助类
    /// </summary>
    public class SessionHelper
    {
        #region 公共信息
        /// <summary>
        /// 当前用户是否已检查过IP
        /// </summary>
        public const string SESSION_KEY_IS_IP_CHECKED = "SESSION_KEY_IS_IP_CHECKED";
        /// <summary>
        /// 当前登陆客户
        /// </summary>
        public static bool IsIpCheckd
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_IS_IP_CHECKED).ToString().ParseTo(false);
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_IS_IP_CHECKED, value);
            }
        }

        #endregion

        #region 后台用户相关
        /// <summary>
        /// 当前后台用户
        /// </summary>
        public const string SESSION_KEY_CURRENT_ADMINUSER = "SESSION_KEY_CURRENT_ADMINUSER";
        /// <summary>
        /// 当前后台用户
        /// </summary>
        //public static AdminUser CurrentAdminUser
        //{
        //    get
        //    {
        //        return SessionManager.GetSession(SESSION_KEY_CURRENT_ADMINUSER) as AdminUser;
        //    }
        //    set
        //    {
        //        SessionManager.SetSession(SESSION_KEY_CURRENT_ADMINUSER, value);
        //    }
        //}
        #endregion

        #region 客户信息相关
        /// <summary>
        /// 当前登陆客户
        /// </summary>
        public const string SESSION_KEY_CURRENT_USER = "SESSION_KEY_CURRENT_USER";
        /// <summary>
        /// 当前登陆客户
        /// </summary>
        public static Customer CurrentCustomer
        {
            get
            {
                //return new Customer { FullName = "LXF", CustomerId = 2 };
                return SessionManager.GetSession(SESSION_KEY_CURRENT_USER) as Customer;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_USER, value);
            }
        }

        /// <summary>
        /// 当前登陆客户的Preference
        /// </summary>
        public const string SESSION_KEY_CUSTOMER_PREFERENCE = "SESSION_KEY_CUSTOMER_PREFERENCE";
        /// <summary>
        /// 当前登陆客户的Preference
        /// <para>用户登陆成功之后 获取该客户的Preference</para>
        /// </summary>
        public static Preference CurrentCustomerPreference
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_CUSTOMER_PREFERENCE) as Preference;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CUSTOMER_PREFERENCE, value);
            }
        }

        /// <summary>
        /// 登陆失败验证码
        /// </summary>
        public const string SESSION_KEY_LOGIN_VALIDATECODE = "SESSION_KEY_LOGIN_VALIDATECODE";
        /// <summary>
        /// 登陆失败验证码
        /// </summary>
        public static string ValidateCode
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_LOGIN_VALIDATECODE) as string;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_LOGIN_VALIDATECODE, value);
            }
        }

        /// <summary>
        /// 当前登陆FaceBook信息
        /// </summary>
        public const string SESSION_KEY_CURRENT_FACEBOOK = "SESSION_KEY_CURRENT_FACEBOOK";
        /// <summary>
        /// 当前登陆FaceBook信息
        /// </summary>
        public static FacebookInfo CurrentFaceBookInfo
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_CURRENT_FACEBOOK) as FacebookInfo;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_FACEBOOK, value);
            }
        }
        #endregion

        #region 币种

        /// <summary>
        /// 当前客户选择的币种
        /// </summary>
        public const string SESSION_KEY_CURRENT_CURRENCY = "SESSION_KEY_CURRENT_CURRENCY";
        /// <summary>
        /// 当前客户选择的币种
        /// </summary>
        public static Currency CurrentCurrency
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_CURRENT_CURRENCY) as Currency;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_CURRENCY, value);
            }
        }
        #endregion

        #region 购物车
        /// <summary>
        /// 当前客户的ShoppingCartId
        /// </summary>
        public static int ShoppingCartId
        {
            get
            {
                //CheckCustomerIsLogined
                if (!CurrentCustomer.IsNullOrEmpty()) return CurrentCustomer.CustomerId;

                if (CookieHelper.CurrentShoppingCartId.HasValue) return CookieHelper.CurrentShoppingCartId.Value;

                var shoppingCartId = ServiceFactory.ShoppingCartService.GetCookIdforTempCustomerId();
                CookieHelper.CurrentShoppingCartId = shoppingCartId;
                return shoppingCartId;
            }
        }

        /// <summary>
        /// 当前客户购物车明细数量
        /// </summary>
        public const string SESSION_KEY_CURRENT_SHOPPINGCARTITEM_COUNT = "SESSION_KEY_CURRENT_SHOPPINGCARTITEM_COUNT";

        /// <summary>
        /// 当前客户购物车明细数量
        /// </summary>
        public static int CurrentShoppingCarItemCount
        {
            get
            {
                var itemCount = SessionManager.GetSession(SESSION_KEY_CURRENT_SHOPPINGCARTITEM_COUNT);
                var shoppingCarItemCount = itemCount.ParseTo(0);
                if (shoppingCarItemCount > 0) return shoppingCarItemCount;
                shoppingCarItemCount = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
                SessionManager.SetSession(SESSION_KEY_CURRENT_SHOPPINGCARTITEM_COUNT, shoppingCarItemCount);
                return shoppingCarItemCount;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_SHOPPINGCARTITEM_COUNT, value);
            }
        }


        /// <summary>
        /// 进入购物车之前的url
        /// </summary>
        private const string SESSION_KEY_LASTSHOPPING_URL = "SESSION_KEY_LASTSHOPPING_URL";
        /// <summary>
        /// 最后一个购物URL
        /// </summary>
        public static string LastShoppingUrl
        {
            get
            {
                //如果上一个页面是商品列表（包括正常商品列表，推荐商品列表，专区商品列表），商品详情，搜索时，点击Continue Shopping跳转回上一个页面
                //其他情况点击Continue Shopping跳转回网站首页
                var url = SessionManager.GetSession(SESSION_KEY_LASTSHOPPING_URL) as string;
                return url.IsNullOrEmpty() ? "/" : url;//"javascript:history.back()";
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_LASTSHOPPING_URL, value);
            }
        }

        /// <summary>
        /// 最后下单完成后的订单号
        /// </summary>
        private const string SESSION_KEY_LAST_ORDER_NUMBER = "SESSION_KEY_LAST_ORDER_NUMBER";
        /// <summary>
        /// 最后下单完成后的订单号
        /// </summary>
        public static string LastOrderNumber
        {
            get
            {
                var url = SessionManager.GetSession(SESSION_KEY_LAST_ORDER_NUMBER) as string;
                return url;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_LAST_ORDER_NUMBER, value);
            }
        }


        #endregion

        #region 支付相关信息
        public const string SESSION_KEY_PAYPAL_EXPRESS_TOKEN = "SESSION_KEY_PAYPAL_EXPRESS_TOKEN";
        /// <summary>
        /// Paypal快速支付Token值
        /// </summary>
        public static string PaypalExpressToken
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_PAYPAL_EXPRESS_TOKEN) as string;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_PAYPAL_EXPRESS_TOKEN, value);
            }
        }

        public const string SESSION_KEY_PAYPAL_EXPRESS_PAY_INFO = "SESSION_KEY_PAYPAL_EXPRESS_PAY_INFO";
        /// <summary>
        /// paypal快速支付返回信息
        /// </summary>
        public static PaymentCommon.PayInfo.PaypalInfo PaypalExpressPayInfo
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_PAYPAL_EXPRESS_PAY_INFO) as PaymentCommon.PayInfo.PaypalInfo;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_PAYPAL_EXPRESS_PAY_INFO, value);
            }
        }

        public const string SESSION_KEY_GC_INSERTORDERWITHPAYMENT_INFO = "SESSION_KEY_GC_INSERTORDERWITHPAYMENT_INFO";
        /// <summary>
        /// paypal快速支付返回信息
        /// </summary>
        public static GlobalCollectParm GlobalCollectParmInfo
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_GC_INSERTORDERWITHPAYMENT_INFO) as GlobalCollectParm;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_GC_INSERTORDERWITHPAYMENT_INFO, value);
            }
        }

        public const string SESSION_KEY_GC_ERROR_MESSAGE = "SESSION_KEY_GC_ERROR_MESSAGE";
        /// <summary>
        /// paypal快速支付返回信息
        /// </summary>
        public static string GlobalCollectErrorMessage
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_GC_ERROR_MESSAGE) as string;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_GC_ERROR_MESSAGE, value);
            }
        }
        #endregion
    }
}
