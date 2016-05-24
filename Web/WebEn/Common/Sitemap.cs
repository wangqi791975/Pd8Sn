using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SEO;

namespace Com.Panduo.Web.Common
{
    public class Sitemap
    {

        public static string ToHtmlText(IList<SitemapItem> sitemapItems)
        {
            var htmlText = new StringBuilder("<div class=\"nav\"><a href=\"/\">Home</a>");
            foreach (var item in sitemapItems)
            {
                if (!string.IsNullOrEmpty(item.Link))
                {
                    htmlText.AppendFormat(" &gt; <a href=\"{0}\">{1}</a>", item.Link, item.Text);
                }
                else
                    htmlText.AppendFormat(" &gt; {0}", item.Text);
            }
            htmlText.Append("</div>");
            return htmlText.ToString();
        }


        #region sitemap
        /// <summary>
        /// 获得类别展示页面的面包屑
        /// </summary>
        public static string GetCategoryDefaultSitemap(Category category)
        {
            var sitemap = new List<SitemapItem>();
            if (category.ParentId != 0)
            {
                CategoryLanguage parent = ServiceFactory.CategoryService.GetCategoryLanguageById(category.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId);
                if (parent != null)
                {
                    string strTitle = parent.CategoryLanguageName;
                    sitemap.Add(new SitemapItem(parent.CategoryLanguageName, UrlFuncitonHelper.GetHost(true) + UrlRewriteHelper.GetCategoryDefaultUrl(parent.CategoryId, parent.CategoryLanguageName)) { Title = strTitle });
                }
            }
            CategoryLanguage categoryLanguage = ServiceFactory.CategoryService.GetCategoryLanguageById(category.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            sitemap.Add(new SitemapItem("<strong>" + HttpUtility.HtmlEncode(categoryLanguage.CategoryLanguageName) + "</strong>", null, false));
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得产品列表面包屑导航
        /// </summary>
        public static string GetProductListSitemap(int categoryId)
        {
            var sitemap = new List<SitemapItem>();
            GetCategoryFamliy(categoryId, sitemap);
            return ToHtmlText(sitemap);
        }

        private static void GetCategoryFamliy(int categoryId, List<SitemapItem> sitemap, bool hasSublevel = false)
        {
            int i = 1;
            var categories = ServiceFactory.CategoryService.GetCategoryLanguageFamliy(categoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            SitemapItem categoryItem;
            foreach (var category in categories)
            {
                //  取SEO设置的面包屑Title，否则使用类别名称
                var metaList = ServiceFactory.MetaService.GetMetaListByType(category.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId, MetaListPageType.Home) ?? new MetaList();
                var categoryLanguageName = !metaList.Breadcrumb.IsNullOrEmpty() ? metaList.Breadcrumb : category.CategoryLanguageName;
                if (!hasSublevel && i == categories.Count)
                {
                    categoryItem = new SitemapItem()
                    {
                        Text = string.Format("<strong>{0}</strong>", HttpUtility.HtmlEncode(categoryLanguageName)),
                        IsHtmlEncode = false
                    };
                }
                else
                {
                    categoryItem = new SitemapItem()
                    {
                        Text = categoryLanguageName,
                        Link = UrlRewriteHelper.GetProductListUrl(category.CategoryId, category.CategoryLanguageName, 1)
                    };
                }
                categoryItem.Title = category.CategoryLanguageName;
                sitemap.Add(categoryItem);
                i++;
            }
        }


        /// <summary>
        /// 类别下搜索：获得搜索结果面包屑导航
        /// </summary>
        public static string GetProductSearchSitemap(string keyword, int categoryId = 0)
        {
            var sitemap = new List<SitemapItem>();
            if (categoryId > 0)
            {
                GetCategoryFamliy(categoryId, sitemap, true);
            }
            sitemap.Add(new SitemapItem() { Text = string.Format("Search: <strong>{0}</strong>", HttpUtility.HtmlEncode(keyword)) });
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 类别下搜索：产品明细页 根据搜索过来 可以跳回搜索页
        /// </summary>
        public static string GetProductSearchProductDetailSitemap(string keyword, string productCode)
        {
            var sitemap = new List<SitemapItem>();
            sitemap.Add(new SitemapItem() { Text = string.Format("Search: {0}", HttpUtility.HtmlEncode(keyword)), Link = UrlRewriteHelper.GetProductSearchUrl(keyword, 1) });
            sitemap.Add(new SitemapItem { Text = "<strong>" + HttpUtility.HtmlEncode(productCode) + "</strong>" });
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得专区页面面包屑导航
        /// </summary>
        public static string GetSpecialAreaSitemap(string areaName, int categoryId, string productCode)
        {
            var sitemap = new List<SitemapItem>();
            #region 生成类别部分的导航
            if (categoryId > 0)
            {
                GetCategoryFamliy(categoryId, sitemap, true);
            }
            #endregion

            sitemap.Add(new SitemapItem { Text = "<strong>" + HttpUtility.HtmlEncode(productCode) + "</strong>" });
            sitemap.Insert(0, new SitemapItem { Text = areaName });
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得专区页面面包屑导航
        /// </summary>
        public static string GetSimilarItemAreaSitemap(string areaName, string productCode, int productId, string productName)
        {
            var sitemap = new List<SitemapItem>();
            sitemap.Add(new SitemapItem { Text = productCode, Link = UrlRewriteHelper.GetProductDetailUrl(productId, productName) });
            sitemap.Add(new SitemapItem { Text = areaName });
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 公共
        /// </summary>
        public static string GetPublicSitemap(string name, bool isLastLink = true, string link = "")
        {
            var sitemap = new List<SitemapItem>
            {
                isLastLink
                    ? new SitemapItem {Text = "<strong>" + HttpUtility.HtmlEncode(name) + "</strong>"}
                    : new SitemapItem {Text = HttpUtility.HtmlEncode(name)}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得商品详细页面的面包屑导航数据
        /// </summary>
        public static string GetProductDetailSitemap(int categoryId, string productCode)
        {
            var sitemap = new List<SitemapItem>();

            #region 生成类别部分的导航
            if (categoryId > 0)
            {
                GetCategoryFamliy(categoryId, sitemap, true);
            }
            #endregion

            sitemap.Add(new SitemapItem("<strong>" + HttpUtility.HtmlEncode(productCode) + "</strong>", null, false));
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得商品详细页面的面包屑导航数据
        /// </summary>
        public static string GetMyAccountOrderSearchDetailsSitemap(string orderNo, int status, string statusName)
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem
                {
                    Text = HttpUtility.HtmlEncode(statusName),
                    Link = UrlRewriteHelper.GetMyAccountOrderSearchUrl(status)
                },
                new SitemapItem {Text = "<strong>" + HttpUtility.HtmlEncode(orderNo) + "</strong>"}
            };
            return ToHtmlText(sitemap);
        }


        /// <summary>
        /// 获得商品搜索页面的面包屑导航数据
        /// </summary>
        public static string GetMyAccountOrderSearchSitemap(string statusName)
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>" + HttpUtility.HtmlEncode(statusName) + "</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得WishList页面的面包屑导航数据
        /// </summary>
        public static string GetMyAccountWishListSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>Wishlist</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得Packing Slip页面的面包屑导航数据
        /// </summary>
        public static string GetMyAccountPackingSlipSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>Packing Slip</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得Address Book页面的面包屑导航数据
        /// </summary>
        public static string GetMyAccountAddressBookSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>Address Book</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获得MyAccount页面的面包屑导航数据
        /// </summary>
        public static string GetMyAccountSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "<strong>My Account</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取Account Setting页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetMyAccountSettingSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>Account Setting</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取My Preference页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetMyAccountPreferenceSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>My Preference</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取Newsletter Subscription页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetMyAccountNewsletterSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>Newsletter Subscription</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取My Coupon页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetMyAccountCouponSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Account"), Link = UrlRewriteHelper.GetMyAccount()},
                new SitemapItem {Text = "<strong>My Coupon</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取Testimonial页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetTestimonialSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "<strong>Testimonial</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取Testimonial页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetDetailReivewSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("My Orders"), Link = UrlRewriteHelper.GetMyAccountOrderSearchUrl(-1)},
                new SitemapItem {Text = "<strong>Reviews</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取Password Forgotten页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetForgetPasswordSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("Login"), Link = UrlRewriteHelper.GetLoginUrl()},
                new SitemapItem {Text = "<strong>Password Reset</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// 获取重置密码页面的面包屑导航
        /// </summary>
        /// <returns></returns>
        public static string GetResetPasswordSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = HttpUtility.HtmlEncode("Login"), Link = UrlRewriteHelper.GetLoginUrl()},
                new SitemapItem {Text = "<strong>Password Forgotten</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        public static string GetContactUsSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "<strong>Contact Us</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        #region Help Center 帮助中心
        /// <summary>
        /// 获取运费计算 面包屑导航
        /// </summary>
        /// <returns></returns>
        internal static string GetShippingCalculatorSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "<strong>Shipping Calculator</strong>"}
            };
            return ToHtmlText(sitemap);
        }
        /// <summary>
        /// 网站地图 面包屑导航
        /// </summary>
        /// <returns></returns>
        internal static string GetSitemapPageSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "<strong>Sitemap</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// Help Center 面包屑导航
        /// </summary>
        /// <returns></returns>
        internal static string GetHelpCenterSitemap()
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "<strong>Help Center</strong>"}
            };
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// Help文章 面包屑导航
        /// </summary>
        /// <returns></returns>
        internal static string GetHelpCenterSitemap(int categoryId)
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "Help Center",Link =UrlRewriteHelper.GetHelpCenterUrl() },
            };
            GetHelpCategoryFamliy(categoryId, sitemap);
            return ToHtmlText(sitemap);
        }

        /// <summary>
        /// Help文章 面包屑导航
        /// </summary>
        /// <returns></returns>
        internal static string GetHelpSearchSitemap(string keyword)
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "Help Center",Link =UrlRewriteHelper.GetHelpCenterUrl() },
                new SitemapItem { Text = "<strong>Search: " + HttpUtility.HtmlEncode(keyword) + "</strong>" }
            };
            return ToHtmlText(sitemap);
        }
        /// <summary>
        /// Help文章 面包屑导航
        /// </summary>
        /// <returns></returns>
        internal static string GetHelpArticleSitemap(int categoryId, string articleName)
        {
            var sitemap = new List<SitemapItem>
            {
                new SitemapItem {Text = "Help Center",Link =UrlRewriteHelper.GetHelpCenterUrl() },
            };
            GetHelpCategoryFamliy(categoryId, sitemap, true);
            sitemap.Add(new SitemapItem { Text = "<strong>" + HttpUtility.HtmlEncode(string.Format("{0}", articleName.Length > 40 ? articleName.Left(40) + "..." : articleName)) + "</strong>" });
            return ToHtmlText(sitemap);
        }

        private static void GetHelpCategoryFamliy(int categoryId, ICollection<SitemapItem> sitemap, bool hasSublevel = false)
        {
            var i = 1;
            var categories = ServiceFactory.HelpService.GetHelpCategoryFamliyByLanguageId(categoryId, ServiceFactory.ConfigureService.SiteLanguageId);
            foreach (var category in categories)
            {
                var categoryLanguageName = category.CategoryName;
                SitemapItem categoryItem;
                if (!hasSublevel && i == categories.Count)
                {
                    categoryItem = new SitemapItem
                    {
                        Text = string.Format("<strong>{0}</strong>", HttpUtility.HtmlEncode(categoryLanguageName)),
                        IsHtmlEncode = false
                    };
                }
                else
                {
                    categoryItem = new SitemapItem
                    {
                        Text = categoryLanguageName,
                        Link = UrlRewriteHelper.GetHelpCenterListUrl(category.HelpCategoryId, category.EnCategoryName, 1)
                    };
                }
                sitemap.Add(categoryItem);
                i++;
            }
        }
        #endregion



        #endregion


    }
}
