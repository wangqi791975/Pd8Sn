using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// Url重写辅助类
    /// </summary>
    public static class UrlRewriteHelper
    {
        /// <summary>
        /// http://www.8seaons.com
        /// </summary>
        private static string _Host = ConfigHelper.WebDomainHost;

        private static string Host
        {
            get { return _Host.IsNullOrEmpty() ? string.Empty : _Host.TrimEnd('/'); }
        }

        /// <summary>
        /// 全站搜索
        /// </summary>
        public const string ProductSearch = "productsearch";
        /// <summary>
        /// 促销区
        /// </summary>
        public const string PromotionAreaUrl = "promotion";
        /// <summary>
        /// 新品区
        /// </summary>
        public const string NewArrivalsAreaUrl = "new-arrivals";
        /// <summary>
        /// Best Seller区
        /// </summary>
        public const string BestSellersAreaUrl = "best-sellers";
        /// <summary>
        /// 相似商品
        /// </summary>
        public const string SimiliarItemsAreaUrl = "similiar-items";

        /// <summary>
        /// BestMatch商品
        /// </summary>
        public const string BestMatchAreaUrl = "bestmatch-items";

        /// <summary>
        /// Mixed区
        /// </summary>
        public const string MixedAreaUrl = "mixed-products";
        /// <summary>
        /// 其他商品专区
        /// </summary>
        public const string SubjectAreaUrl = "subject-product";
        public const string MyAccountUrl = "account";


        #region 客户账户相关
        public static string GetLoginUrl()
        {
            return string.Format("{0}/{1}.html", Host, "login");
        }

        public static string GetRegisterUrl()
        {
            return string.Format("{0}/{1}.html", Host, "register");
        }

        public static string GetForgetPwdUrl()
        {
            return string.Format("{0}/{1}.html", Host, "forgetpassword");
        }

        public static string GetAccountSettingUrl()
        {
            return string.Format("{0}/{1}.html", Host, "accountsetting");
        }

        public static string GetAddressBookUrl()
        {
            return string.Format("{0}/{1}.html", Host, "addressbook");
        }

        public static string GetMyPreferenceUrl()
        {
            return string.Format("{0}/{1}.html", Host, "mypreference");
        }

        public static string GetNewsletterUrl()
        {
            return string.Format("{0}/{1}.html", Host, "newsletter");
        }

        public static string GetMyClubUrl()
        {
            return string.Format("{0}/{1}.html", Host, "myclub");
        }

        public static string GetMyProductsUrl()
        {
            return string.Format("{0}/{1}.html", Host, "myproducts");
        }

        public static string GetMyCouponUrl()
        {
            return string.Format("{0}/{1}.html", Host, "mycoupon");
        }
        #endregion

        #region 类别

        /// <summary>
        /// 获得类别展示页面Url 
        /// </summary>
        public static string GetCategoryDefaultUrl(int categoryId, string categoryName)
        {
            return string.Format("{0}/{1}/{2}.html", Host, FilterUrl(categoryName), categoryId);
        }

        #endregion

        #region 商品列表
        /// <summary>
        /// 获得类别商品列表Url
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="categoryName">类别名称</param>
        /// <param name="pageIndex">页码</param>
        public static string GetProductListUrl(int categoryId, string categoryName, int pageIndex)
        {
            if (pageIndex == 1)
            {
                return GetCategoryDefaultUrl(categoryId, categoryName);
            }
            return string.Format("{0}/{1}/{2}-{3}.html", Host, FilterUrl(categoryName), categoryId, pageIndex);
        }

        /// <summary>
        /// 获取相似商品列表Url
        /// </summary>
        /// <param name="productId">主商品ID</param>
        /// <param name="pageIndex">页码</param>
        public static string GetSimiliarItemsUrl(int productId, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}-{3}.html", Host, SimiliarItemsAreaUrl, productId, pageIndex);
        }

        /// <summary>
        /// 获取BestMatch商品列表Url
        /// </summary>
        /// <param name="productId">主商品ID</param>
        /// <param name="pageIndex">页码</param>
        public static string GetBestMatchUrl(int productId, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}-{3}.html", Host, BestMatchAreaUrl, productId, pageIndex);
        }


        #region NewArrivals
        /// <summary>
        /// 获取新品区商品列表Url
        /// </summary>
        /// <param name="pageIndex">页码</param>
        public static string GetNewArrivalsUrl(int pageIndex)
        {
            return string.Format("{0}/{1}/{2}.html", Host, NewArrivalsAreaUrl, pageIndex);
        }

        /// <summary>
        ///  获取新品区商品列表Url
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="categoryName">类别名称</param>
        /// <param name="pageIndex">页码</param>
        public static string GetNewArrivalsUrl(int categoryId, string categoryName, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}/{3}-{4}.html", Host, NewArrivalsAreaUrl, FilterUrl(categoryName), categoryId, pageIndex);
        }
        #endregion

        #region Best Seller
        /// <summary>
        /// 获取Best Seller首页
        /// </summary>
        /// <returns></returns>
        public static string GetBestSellerIndexUrl()
        {
            return string.Format("{0}/{1}.html", Host, BestSellersAreaUrl);
        }

        /// <summary>
        /// 获取Best Seller商品列表Url
        /// </summary>
        /// <param name="pageIndex">页码</param>
        public static string GetBestSellerUrl(int pageIndex)
        {
            return string.Format("{0}/{1}/{2}.html", Host, BestSellersAreaUrl, pageIndex);
        }

        /// <summary>
        /// 获取Best Seller商品列表Url
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="categoryName">类别名称</param>
        /// <param name="pageIndex">页码</param>
        public static string GetBestSellerUrl(int categoryId, string categoryName, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}/{3}-{4}.html", Host, BestSellersAreaUrl, FilterUrl(categoryName), categoryId, pageIndex);
        }
        #endregion

        #region Mixed区

        /// <summary>
        /// 获取Mixed区商品列表Url
        /// </summary>
        /// <param name="pageIndex">页码</param>
        public static string GetMixedUrl(int pageIndex)
        {
            return string.Format("{0}/{1}/{2}.html", Host, MixedAreaUrl, pageIndex);
        }

        /// <summary>
        /// 获取Mixed区商品列表Url
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="categoryName">类别名称</param>
        /// <param name="pageIndex">页码</param>
        public static string GetMixedUrl(int categoryId, string categoryName, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}/{3}-{4}.html", Host, MixedAreaUrl, FilterUrl(categoryName), categoryId, pageIndex);
        }
        #endregion

        #region 促销区

        /// <summary>
        /// 获取促销区首页
        /// </summary>
        /// <returns></returns>
        public static string GetPromotionIndexUrl()
        {
            return string.Format("{0}/{1}.html", Host, PromotionAreaUrl);
        }

        /// <summary>
        /// 获取促销区商品列表Url
        /// </summary>
        /// <param name="pageIndex">页码</param>
        public static string GetPromotionUrl(int pageIndex)
        {
            return string.Format("{0}/{1}/{2}.html", Host, PromotionAreaUrl, pageIndex);
        }

        /// <summary>
        /// 获取促销区商品列表Url
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="categoryName">类别名称</param>
        /// <param name="pageIndex">页码</param>
        public static string GetPromotionUrl(int categoryId, string categoryName, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}/{3}-{4}.html", Host, PromotionAreaUrl, FilterUrl(categoryName), categoryId, pageIndex);
        }
        #endregion

        #region 其他主题专区
        /// <summary>
        /// 获取其他主题专区商品列表Url
        /// </summary>
        /// <param name="areaId">专区ID</param>
        /// <param name="areaName">专区名称</param>
        /// <returns></returns>
        public static string GetSubjectAreaUrl(int areaId, string areaName)
        {
            return string.Format("{0}/{1}/{2}/{3}.html", Host, SubjectAreaUrl, FilterUrl(areaName), areaId);
        }

        /// <summary>
        /// 获取其他主题专区商品列表Url
        /// </summary>
        /// <param name="areaId">专区ID</param>
        /// <param name="areaName">专区名称</param>
        /// <param name="categoryId">类别ID</param>
        /// <param name="pageIndex">页码</param>
        public static string GetSubjectAreaUrl(int areaId, string areaName, int categoryId, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}/{3}/{4}-{5}.html", Host, SubjectAreaUrl, FilterUrl(areaName), areaId, categoryId, pageIndex);
        }
        #endregion


        /// <summary>
        /// 全站搜索Url
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string GetProductSearchUrl(string keyword, int pageIndex)
        {
            return string.Format("{0}/{1}?{2}={3}&{4}={5}", Host, ProductSearch, UrlParameterKey.ProductSearchKeyword, FilterUrl(keyword), UrlParameterKey.Page, pageIndex);
        }

        #endregion

        #region 商品详细信息

        /// <summary>
        /// 获取产品详情页Url
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productName">产品名称</param>
        public static string GetProductDetailUrl(int productId, string productName)
        {
            return string.Format("{0}/{1}-{2}.html", Host, FilterUrl(productName), productId);
        }

        #endregion

        #region URL特殊字符处理
        private static string ReplaceDataDotSpaceRegexFormat = @"(\d+)\.+(\w+?)";
        private static string ReplaceNumberSpaceRegexFormat = @"(\d+)\s+(\d+?)";
        private static string ReplaceUnitSpaceRegexFormat = @"(\d+)\s*(\w+)";
        private static string ReplaceMuliSpaceRegexFormat = @"(x+)(\d+?)";

        /// <summary>
        /// 过滤掉URL中特殊字符为合法参数
        /// </summary>
        /// <param name="paramValue">URL或者URL参数</param>
        /// <returns></returns>
        public static string FilterUrl(string paramValue)
        {
            if (paramValue.IsNullOrEmpty())
                return string.Empty;

            //替换数字之间的空格
            paramValue = Regex.Replace(paramValue, ReplaceNumberSpaceRegexFormat, "$1-$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换x数字格式为-x-数字的空格
            paramValue = Regex.Replace(paramValue, ReplaceMuliSpaceRegexFormat, "-$1-$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换数字之间的小数点
            paramValue = Regex.Replace(paramValue, ReplaceDataDotSpaceRegexFormat, "$1$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换单位的空格
            paramValue = Regex.Replace(paramValue, ReplaceUnitSpaceRegexFormat, "$1$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换特殊字符
            paramValue = Regex.Replace(paramValue, "([/]{1,2})|([-!！~`·&*#%^……《<>》{}（()）？?×'‘’,，.。:：\\s\\u005C\"【】\\[[\\]]+)", "-");

            //替换最后一个-
            return paramValue.TrimEnd('-').ToLower();
        }
        #endregion

        #region 正确页面跳转

        /// <summary>
        /// 301跳转
        /// </summary>
        public static void RedirectForever(string targetUrl)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.StatusCode = 301;
            response.AddHeader("Location", targetUrl);
        }

        /// <summary>
        /// 比较URL，如果不吻合(忽略大小写)，则301跳转
        /// </summary>
        /// <param name="targetUrl">跳转URL</param>
        public static bool CompareAndRedirect(string targetUrl, List<string> ignoreParams = null)
        {
            string rawUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            targetUrl = GetUrlWithoutQueryString(targetUrl);
            if (!GetUrlWithoutQueryString(rawUrl).Equals(targetUrl))
            {
                targetUrl = targetUrl.SetParam(rawUrl.GetQueryStringMap(), ignoreParams);
                //RedirectLog(rawUrl, targetUrl);
                RedirectForever(targetUrl);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 比较URL，如果不吻合(忽略大小写)，则301跳转
        /// </summary>
        public static bool CheckSearchKeywordAndRedirect(string keyword, int pageIndex)
        {
            string rawUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            var param = rawUrl.GetQueryStringMap();
            if (param.ContainsKey(UrlParameterKey.ProductSearchKeyword))
                param[UrlParameterKey.ProductSearchKeyword] = keyword;
            if (param.ContainsKey(UrlParameterKey.ProductSearchKeyword))
                param[UrlParameterKey.Page] = pageIndex.ToString();

            var targetUrl = GetUrlWithoutQueryString(rawUrl).SetParam(param);
            if (!rawUrl.Equals(targetUrl))
            {
                RedirectLog(rawUrl, targetUrl);
                RedirectForever(targetUrl);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 比较URL，如果不吻合(忽略大小写)false,否则：true
        /// </summary>
        /// <param name="targetUrl">跳转URL</param>
        public static bool CompareUrl(string targetUrl)
        {
            string rawUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            targetUrl = GetUrlWithoutQueryString(targetUrl);
            if (!GetUrlWithoutQueryString(rawUrl).Equals(targetUrl))
            {
                targetUrl = targetUrl.SetParam(rawUrl.GetQueryStringMap());
                RedirectLog(rawUrl, targetUrl);
                return false;
            }
            return true;
        }

        private static void RedirectLog(string oldUrl, string targetUrl)
        {
            //TODO:  记录301跳转日志
            /*HttpRequest request = HttpContext.Current.Request;
            StringBuilder err = new StringBuilder();
            err.AppendLine("301 Redirect.");
            err.AppendLine("Referrer: " + (request.UrlReferrer == null ? string.Empty : request.UrlReferrer.ToString()));
            err.AppendLine("Old URL: " + oldUrl);
            err.AppendLine("New URL: " + targetUrl);
            err.AppendLine("X-Requested-With:" + request.Headers["X-Requested-With"] ?? string.Empty);
            err.AppendLine("User Agent: " + request.UserAgent ?? string.Empty);
            err.AppendLine("Content-Type: " + request.ContentType ?? string.Empty);
            err.AppendLine("IP: " + request.UserHostAddress);
            Loger.Instance.SysLogger.Info(err.ToString());*/
        }

        public static string GetUrlWithoutQueryString(string url, bool isWithoutQueryString = true)
        {
            url = url.ToLower();
            int i = url.IndexOf('?');
            if (i < 0)
            {
                i = url.IndexOf('#');
            }
            if (i < 0) return url;
            return url.Substring(0, i);
        }
        #endregion

        /// <summary>
        /// 获取专区面包屑URL
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoryName"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string GetSpecialAreaUrl(string areaName, int categoryId, string categoryName, int pageIndex)
        {
            return string.Format("{0}/{1}/{2}/{3}-{4}.html", Host, areaName, FilterUrl(categoryName), categoryId, pageIndex);
        }

        #region 2期
        /// <summary>
        /// 获取购物车连接
        /// </summary>
        public static string GetShoppingCartUrl()
        {
            return string.Format("{0}/{1}.html", Host, "shoppingcart");
        }

        /// <summary>
        /// 获取Checkout页面
        /// </summary>
        public static string GetCheckoutUrl()
        {
            return string.Format("{0}/{1}.html", Host, "checkout");
        }

        /// <summary>
        /// 获取Checkout页面
        /// </summary>
        public static string GetPlaceOrderUrl()
        {
            return string.Format("{0}/{1}.html", Host, "placeorder");
        }



        /// <summary>
        /// 用户中心--〉订单明细
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        public static string GetOrderDetail(string orderNo)
        {
            return "/Order/OrderDetail?orderno=" + orderNo;
        }

        /// <summary>
        /// 订单明细item--〉订单快照
        /// </summary>
        /// <param name="detailId">明细Id</param>
        /// <returns></returns>
        public static string GetOrderSnapshot(string detailId)
        {
            return "/Order/OrderSnapshot?id=" + detailId;
        }

        /// <summary>
        /// 订单支付页面
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        public static string GetOrderDetailPayment(string orderNo)
        {
            return "/Order/OrderDetailPayment?orderno=" + orderNo;
        }

        /// <summary>
        /// QuickReorder
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        public static string GetQuickReorder(string orderNo)
        {
            return "/Order/QuickReorder?orderno=" + orderNo;
        }

        /// <summary>
        /// 我的订单入口
        /// </summary>
        /// <returns></returns>
        public static string GetMyOrderUrl()
        {
            return "/Order/OrderSearch";
        }

        /// <summary>
        /// 我的WishList入口
        /// </summary>
        /// <returns></returns>
        public static string GetMyWishList()
        {
            return "/Wishlist/MyWishlist";
        }

        /// <summary>
        /// WirteReview Order
        /// </summary>
        /// <returns></returns>
        public static string GetWriteReviews(int orderId)
        {
            return "/Reviews/Write?order_id=" + orderId;
        }

        /// <summary>
        /// WirteReview Product
        /// </summary>
        /// <returns></returns>
        public static string GetWriteReviews(int orderId, int orderDetailId)
        {
            return "/Reviews/Write?order_id=" + orderId + "&order_detail_id=" + orderDetailId;
        }

        /// <summary>
        /// WirteReview Order
        /// </summary>
        /// <returns></returns>
        public static string GetReadReviews(int orderId)
        {
            return "/Reviews/Read?order_id=" + orderId;
        }

        /// <summary>
        /// WirteReview Product
        /// </summary>
        /// <returns></returns>
        public static string GetReadReviews(int orderId, int orderDetailId)
        {
            return "/Reviews/Read?order_id=" + orderId + "&order_detail_id=" + orderDetailId;
        }

        public static string GetMyAccount()
        {
            return string.Format("{0}/{1}/{2}.html", Host, MyAccountUrl, "myaccount");
        }

        public static string GetMyAccountOrderSearchUrl(int status)
        {
            return string.Format("{0}/{1}/{2}.html?status={3}", Host, MyAccountUrl, "orderSearch", status);
        }

        /// <summary>
        /// Club注册页面URL
        /// </summary>
        /// <returns></returns>
        public static string GetClubJoinUrl()
        {
            return string.Format("{0}/{1}.html", Host, "welcomeinclub");
        }

        /// <summary>
        /// DailyDeal一口价
        /// </summary>
        /// <returns></returns>
        public static string GetDailyDealHomeUrl()
        {
            return string.Format("{0}/{1}.html", Host, "dailydeal");
        }

        /// <summary>
        /// Club
        /// </summary>
        /// <returns></returns>
        public static string GetClubHomeUrl()
        {
            return string.Format("{0}/{1}.html", Host, "club");
        }

        #endregion

        #region 3期

        public static string GetViewInvoiceUrl(string orderNo)
        {
            return string.Format("{0}/Order/OrderInvoice?orderno={1}", Host, orderNo);
        }

        /// <summary>
        /// Testimonial
        /// </summary>
        /// <returns></returns>
        public static string GetTestimonialUrl()
        {
            return string.Format("{0}/{1}.html", Host, "testimonial");
        }

        #endregion
    }
}
