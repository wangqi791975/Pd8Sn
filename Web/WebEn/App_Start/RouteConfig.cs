using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web
{
    /// <summary>
    /// 路由配置
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region 动态脚本
            //动态Css路由
            routes.MapRoute(
                "Css",
                YuiHtmlHelper.ActionNameCss + "/{resourceName}",
                new { controller = "Yui", action = YuiHtmlHelper.ActionNameCss },
                new[] { "Com.Panduo.Web.Common.Mvc.Controllers" }
            );

            //动态Js路由
            routes.MapRoute(
                "Js",
                YuiHtmlHelper.ActionNameJs + "/{resourceName}",
                new { controller = "Yui", action = YuiHtmlHelper.ActionNameJs },
                new[] { "Com.Panduo.Web.Common.Mvc.Controllers" }
            );
            #endregion

            #region 错误 
            //Js脚本错误
            routes.MapRoute(
                "JavaScriptError",
                "script-error.html",
                new { controller = "Home", action = "ScriptError" }
            );
            //系统错误
            routes.MapRoute(
                CommonConfigHelper.SystemErrorRouteName,
                CommonConfigHelper.SystemErrorRouteName,
                new { controller = "SystemError", action = "SystemError" },
                new[] { " Com.Panduo.Web.Common.Mvc.Error.Error500" }
            );

            //404错误
            routes.MapRoute(
                CommonConfigHelper.PageNotFoundRouteName,
                CommonConfigHelper.PageNotFoundRouteName,
                new { controller = "PageNotFound", action = "Execute" },
                new[] { " Com.Panduo.Web.Common.Mvc.Error.Error404" }
            );

            //ip检查错误
            routes.MapRoute(
                CommonConfigHelper.IpCheckErrorRouteName,
                CommonConfigHelper.IpCheckErrorUrl,
               new { controller = "Home", action = "AccessDenied" }
            );

            //授权错误
            routes.MapRoute(
               "Unauthorized", // 路由名称
               "Unauthorized", // 带有参数的 URL
               new { controller = "Home", action = "Unauthorized" } // 参数默认值
            );


            #endregion

            // routes.MapRoute(
            //    "StaticHtml",
            //    "static/{viewName}-{ulrInfo}.html",
            //    new { controller = "Product", action = "StaticHtml"},
            //    new{viewName="[a-zA-z]+"}
            //);

            #region 加载自定义路由
            foreach (var routeConfig in CacheHelper.RountMaps)
            {
                routes.MapRoute(routeConfig.Name, routeConfig.Url, routeConfig.Defaults, routeConfig.Constraints, routeConfig.Namespaces);
            }

            #region Url重写后路由

            #region 1期
            //商品搜索
            routes.MapRoute("ProductSearch", UrlRewriteHelper.ProductSearch, new { controller = "Product", action = "ProductSearch" });
            //Similar Item
            routes.MapRoute("SimilarItem", UrlRewriteHelper.SimiliarItemsAreaUrl + "/{productId}-{page}.html", new { controller = "Product", action = "SimilarItems", page = 1 }, new { productId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            //Best Matched
            routes.MapRoute("Bestmatch", UrlRewriteHelper.BestMatchAreaUrl + "/{productId}-{page}.html", new { controller = "Product", action = "BestMatch", page = 1 }, new { productId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            //New Arrvials
            routes.MapRoute("NewArrivalsCategory", UrlRewriteHelper.NewArrivalsAreaUrl + "/{categoryName}/{categoryId}-{page}.html", new { controller = "Product", action = "NewArrivals", categoryId = 0, page = 1 }, new { categoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            routes.MapRoute("NewArrivals", UrlRewriteHelper.NewArrivalsAreaUrl + "/{page}.html", new { controller = "Product", action = "NewArrivals", page = 1 });
            //Best Seller类页
            routes.MapRoute("BestSellerCategory", UrlRewriteHelper.BestSellersAreaUrl + "/{categoryName}/{categoryId}-{page}-1.html", new { controller = "Product", action = "BestSeller", categoryId = 0, page = 1 }, new { categoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            routes.MapRoute("BestSeller", UrlRewriteHelper.BestSellersAreaUrl + "/{page}.html", new { controller = "Product", action = "BestSeller", page = 1 }, new { page = "^\\d{1,6}|-page-" });
            routes.MapRoute("BestSellerIndex", string.Format("{0}.html", UrlRewriteHelper.BestSellersAreaUrl), new { controller = "Product", action = "StaticHtml" });
            //Mix Product
            routes.MapRoute("MixProductCategory", UrlRewriteHelper.MixedAreaUrl + "/{categoryName}/{categoryId}-{page}.html", new { controller = "Product", action = "MixProduct", categoryId = 0, page = 1 }, new { categoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            routes.MapRoute("MixProduct", UrlRewriteHelper.MixedAreaUrl + "/{page}.html", new { controller = "Product", action = "MixProduct", page = 1 }, new { page = "^\\d{1,6}|-page-" });
            //Promotion
            routes.MapRoute("PromotionCategory", UrlRewriteHelper.PromotionAreaUrl + "/{categoryName}/{categoryId}-{page}.html", new { controller = "Product", action = "Promotion", categoryId = 0, page = 1 }, new { categoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            routes.MapRoute("Promotion", UrlRewriteHelper.PromotionAreaUrl + "/{page}.html", new { controller = "Product", action = "Promotion", page = 1 }, new { page = "^\\d{1,6}|-page-" });
            routes.MapRoute("PromotionIndex", string.Format("{0}.html", UrlRewriteHelper.PromotionAreaUrl), new { controller = "Product", action = "StaticHtml" });
            //专区页面的Url-其他主题专区
            routes.MapRoute("AreaCategory", UrlRewriteHelper.SubjectAreaUrl + "/{areaName}/{areaId}/{categoryId}-{page}.html", new { controller = "Product", action = "Area", categoryId = 0, page = 1 }, new { areaId = "^\\d{1,8}$", categoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            routes.MapRoute("Area", UrlRewriteHelper.SubjectAreaUrl + "/{areaName}/{areaId}/{page}.html", new { controller = "Product", action = "Area", page = 1 }, new { areaId = "^\\d{1,8}$" });
            //产品List
            routes.MapRoute("ProductList", "{categoryName}/{categoryId}-{page}.html", new { controller = "Product", action = "ProductList", categoryName = "category", categoryId = 0, page = 1 }, new { categoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            //一级类别
            routes.MapRoute("CategoryDefault", "{categoryName}/{categoryId}.html", new { controller = "Product", action = "ProductList", categoryName = "category", categoryId = 0, page = 1 }, new { categoryId = "^\\d{1,8}$" });
            //产品Detail页面
            routes.MapRoute("ProductDetail", "{productName}-{productId}.html", new { controller = "Product", action = "Detail", productId = 0 }, new { productId = "^\\d{1,8}$" });
            routes.MapRoute("CustmoerLogin", "login.html", new { controller = "Account", action = "Login" });
            routes.MapRoute("CustmoerRegister", "register.html", new { controller = "Account", action = "Register" });
            routes.MapRoute("CustmoerForgetPwd", "password-forgotten.html", new { controller = "Account", action = "ForgetPwd" });
            #endregion

            #region 2期
            //购物车 页码在后面?page=2
            routes.MapRoute("ShoppingCart", "checkout/cart.html", new { controller = "ShoppingCart", action = "ShoppingCart" });
            routes.MapRoute("ItemsReview", "checkout/place-order.html", new { controller = "PlaceOrder", action = "ItemsReview" });
            routes.MapRoute("CheckOut", "checkout.html", new { controller = "PlaceOrder", action = "CheckOut" });
            //wishlist 列表页
            routes.MapRoute("Wishlist", UrlRewriteHelper.MyAccountUrl + "/wishlist.html", new { controller = "Wishlist", action = "MyWishlist" });
            routes.MapRoute("MyAccountIndex", "my-account.html", new { controller = "Account", action = "MyAccount" });
            routes.MapRoute("MyAccountOrderSearch", UrlRewriteHelper.MyAccountUrl + "/my-orders.html", new { controller = "Order", action = "OrderSearch" });
            routes.MapRoute("MyAccountOrderDetail", UrlRewriteHelper.MyAccountUrl + "/order-details.html", new { controller = "Order", action = "OrderDetail" });
            routes.MapRoute("MyAccountOrderDetailSnapshot", UrlRewriteHelper.MyAccountUrl + "/order-item-details.html", new { controller = "Order", action = "OrderSnapshot" });
            routes.MapRoute("MyAccountOrderDetailPayment", UrlRewriteHelper.MyAccountUrl + "/order-payment.html", new { controller = "Order", action = "OrderDetailPayment" });
            routes.MapRoute("MyAccountOrderReviews", UrlRewriteHelper.MyAccountUrl + "/product-reviews.html", new { controller = "Reviews", action = "Read" });
            routes.MapRoute("MyAccountOrderInvoice", UrlRewriteHelper.MyAccountUrl + "/order-invoice.html", new { controller = "Order", action = "OrderInvoice" });
            routes.MapRoute("MyAccountOrderInvoiceDownload", UrlRewriteHelper.MyAccountUrl + "/order-invoiceDownload.html", new { controller = "Order", action = "DownloadPdf" });
            routes.MapRoute("MyAccountPackingSlip", UrlRewriteHelper.MyAccountUrl + "/order-packingslip.html", new { controller = "Order", action = "PackingSlip" });


            //MyAccount
            routes.MapRoute("AccountSetting", UrlRewriteHelper.MyAccountUrl + "/"+UrlRewriteHelper.AcountService+"/account-setting.html", new { controller = "Account", action = "AccountSetting" });
            routes.MapRoute("AddressBook", UrlRewriteHelper.MyAccountUrl + "/" + UrlRewriteHelper.AcountService + "/address-book.html", new { controller = "Account", action = "AddressBook" });
            routes.MapRoute("MyPreference", UrlRewriteHelper.MyAccountUrl + "/" + UrlRewriteHelper.AcountService + "/my-preference.html", new { controller = "Account", action = "MyPreference" });
            routes.MapRoute("Newsletter", UrlRewriteHelper.MyAccountUrl + "/" + UrlRewriteHelper.AcountService + "/newsletter-subscription.html", new { controller = "Account", action = "Newsletter" });
            routes.MapRoute("MyClub", UrlRewriteHelper.MyAccountUrl + "/8seasons-club.html", new { controller = "Privilege", action = "Club" });
            routes.MapRoute("MyProducts", UrlRewriteHelper.MyAccountUrl + "/my-products.html", new { controller = "Privilege", action = "MyProducts" });
            routes.MapRoute("MyCoupon", UrlRewriteHelper.MyAccountUrl + "/my-coupon.html", new { controller = "Account", action = "MyCoupon" });
            //club
            routes.MapRoute("WelcomeInClub", "club-register.html", new { controller = "Account", action = "WelcomeInClub" });
            routes.MapRoute("Club", "club.html", new { controller = "Product", action = "ProductClub" });
            //dailydeal
            routes.MapRoute("ProductDailyDeal", "dailydeal.html", new { controller = "Product", action = "ProductDailyDeal" });
            #endregion

            #region 3期
            routes.MapRoute("HelpCenterSearch", UrlRewriteHelper.HelpCenterSearch, new { controller = "Help", action = "HelpSearch" });

            routes.MapRoute("Testimonial", "testimonial.html", new { controller = "Reviews", action = "Testimonial" });
            //帮助中心 运费计算
            routes.MapRoute("ShippingFee", "shippingfee.html", new { controller = "Help", action = "ShippingCalculator" });
            //网站地图 Sitemap
            routes.MapRoute("SitemapPage", "sitemap.html", new { controller = "Help", action = "SitemapPage" });
            //帮助中心 Sitemap
            routes.MapRoute("HelpIndex", "help-center.html", new { controller = "Help", action = "HelpIndex" });
            //帮助中心 主题列表ww.8easons.com/help-center/category-类别名称-类别ID.html
            routes.MapRoute("HelpCenter", UrlRewriteHelper.HelpCenter + "/{englishName}/{helpCategoryId}-{page}.html", new { controller = "Help", action = "HelpCenter", englishName = "faq", helpCategoryId = 1, page = 1 }, new { helpCategoryId = "^\\d{1,8}$", page = "^\\d{1,6}|-page-" });
            //帮助中心文章详情
            routes.MapRoute("ArticleDetail", UrlRewriteHelper.HelpCenter + "/{articleName}-{articleId}.html", new { controller = "Help", action = "ArticleDetail", articleId = 0 }, new { articleId = "^\\d{1,8}$" });

            routes.MapRoute("Sourcing", UrlRewriteHelper.HelpCenter + "/oem-sourcing.html", new { controller = "product", action = "Souring" });

            //联系我们
            routes.MapRoute("ContactUs", "contactus.html", new { controller = "ContactUs", action = "ContactUs" });

            //密码重置
            routes.MapRoute("ResetPassword", "resetpassword.html", new { controller = "Account", action = "ResetPwd" });
            #endregion

            #endregion

            #endregion

            //默认路由
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}