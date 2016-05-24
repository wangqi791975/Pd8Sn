using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Help;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Product;
using Resources;

namespace Com.Panduo.Web.Controllers
{
	public class HelpController : Controller
	{
		//
		// GET: /Help/

		private static int LanguageId { get { return ServiceFactory.ConfigureService.SiteLanguageId; } }

		#region Help Center

		/// <summary>
		/// 帮助中心 首页
		/// </summary>
		public ActionResult HelpIndex()
		{
			ViewBag.RootHelpCategories = ServiceFactory.HelpService.GetRootHelpCategories(LanguageId);
			ViewBag.SubHelpCategories = ServiceFactory.HelpService.GetSubHelpCategoryOfRootByTop(LanguageId, 5);
			ViewBag.RootHelpArticles = ServiceFactory.HelpService.GetHelpArticlesOfRootByTop(LanguageId, 5);
			ViewBag.CurrentHelpCategoryId = 0;
			ViewBag.Sitemaps = Sitemap.GetHelpCenterSitemap();
			return View();
		}

		public ActionResult HelpSearch()
		{
			var page = Request["page"].ParseTo(1);
			var keyword = Request["keyword"];
			var pageSize = ConfigHelper.MaxArticlePageSize; //页大小
			var sortMode = Request[UrlParameterKey.Sort].ParseTo<int>().ToEnum<ArticleSorterCriteria>();

			var sorter = new List<Sorter<ArticleSorterCriteria>> { new Sorter<ArticleSorterCriteria> { Key = sortMode, IsAsc = true } };

			var search = new Dictionary<ArticleSearchCriteria, object> { { ArticleSearchCriteria.LanguageId, LanguageId }, { ArticleSearchCriteria.Keyword, keyword } };

			var pageData = ServiceFactory.HelpService.FindHelpArticles(page, pageSize, search, sorter);
			ViewBag.Sitemaps = Sitemap.GetHelpSearchSitemap(keyword);

			ViewBag.SubTitle = string.Format("Search:{0}", keyword);
		    ViewBag.HelpSearchKeyword = keyword;
            ViewBag.CurrentHelpCategoryId = 0;
            ViewBag.IsLastLevel = true;
			return View("HelpCenter", pageData);
		}

		/// <summary>
		/// 帮助中心 列表、详情页
		/// </summary>
		/// <returns></returns>
		public ActionResult HelpCenter(int helpCategoryId, int page)
		{
			var showArticleList = Request["showArticleList"].ParseTo(0) > 0;
			var pageData = new PageData<VHelpArticle>();
			ViewBag.IsLastLevel = false;
			if (showArticleList || ServiceFactory.HelpService.IsLastLevel(helpCategoryId))
			{
				var pageSize = ConfigHelper.MaxArticlePageSize; //页大小
				var sortMode = Request[UrlParameterKey.Sort].ParseTo<int>().ToEnum<ArticleSorterCriteria>();

				var sorter = new List<Sorter<ArticleSorterCriteria>> { new Sorter<ArticleSorterCriteria> { Key = sortMode, IsAsc = true } };

				var search = new Dictionary<ArticleSearchCriteria, object>
				{
					{ ArticleSearchCriteria.LanguageId, LanguageId }, 
					{ ArticleSearchCriteria.CategoryId, helpCategoryId }
				};
				var currentHelpCategory = ServiceFactory.HelpService.GetHelpCategoryById(helpCategoryId, LanguageId);
				ViewBag.SubTitle = currentHelpCategory.CategoryName;
				pageData = ServiceFactory.HelpService.FindHelpArticles(page, pageSize, search, sorter);
				ViewBag.IsLastLevel = true;
			}
			else
			{
				ViewBag.HelpCategoriesOfTow = ServiceFactory.HelpService.GetSubHelpCategorById(helpCategoryId, LanguageId);
				ViewBag.SubHelpCategories = ServiceFactory.HelpService.GetSubHelpCategoryOfRootByTop(LanguageId, 5);
				ViewBag.HelpArticlesOfTow = ServiceFactory.HelpService.GetHelpArticlesOfRootByTop(LanguageId, 5);
			}

			ViewBag.CurrentHelpCategoryId = ServiceFactory.HelpService.GetRootHelpCategoryId(helpCategoryId);
			ViewBag.Sitemaps = Sitemap.GetHelpCenterSitemap(helpCategoryId);

			return Request.IsAjaxRequest() ? View("Partial/_HelpArticleList", pageData) : View(pageData);
		}

		public ActionResult ArticleDetail(int articleId)
		{
			var currentHelpArticle = ServiceFactory.HelpService.GetHelpArticleById(articleId);
			var currentHelpArticleDescription = ServiceFactory.HelpService.GetHelpArticleDescriptionById(articleId, LanguageId);
			ViewBag.PreHelpArticle = ServiceFactory.HelpService.GetPreviousHelpArticleById(articleId, LanguageId);
			ViewBag.NextHelpArticle = ServiceFactory.HelpService.GetNextHelpArticleById(articleId, LanguageId);

			ViewBag.CurrentHelpArticle = currentHelpArticleDescription;
			ViewBag.CurrentHelpCategoryId = ServiceFactory.HelpService.GetRootHelpCategoryId(currentHelpArticle.HelpCategoryId);
			ViewBag.Sitemaps = Sitemap.GetHelpArticleSitemap(currentHelpArticle.HelpCategoryId, currentHelpArticleDescription.ArticleName);
			return View();
		}

		#endregion

		#region 运费计算

		public ActionResult ShippingCalculator()
		{
			Country country = null;
			var shippingAddress = PageHelper.GetDefaultShippingAddress(out country, CookieHelper.CustomerCountryId);
			ViewBag.Country = country;
			ViewBag.ShippingAddress = shippingAddress;
			ViewBag.CommonCountry = ServiceFactory.ConfigureService.GetCommonCountry();
			ViewBag.UnCommonCountry = ServiceFactory.ConfigureService.GetUnCommonCountry();
			ViewBag.Sitemaps = Sitemap.GetShippingCalculatorSitemap();
			return View();
		}

		public ActionResult GetShippingItems(FormCollection form)
		{
			try
			{
				var countryId = form["countryId"].ParseTo<int>();
				var totalWeight = form["totalWeight"].ParseTo<decimal>();
				totalWeight += totalWeight * 0.1M;//需要注意,此处只是商品重量,实际计算的时候需要根据公式加上包装盒的重量作为最后的总重量再去计算运费

				var posatlCode = form["posatlCode"];
				var city = form["city"];

				Country country = null;
				var shippingAddress = PageHelper.GetDefaultShippingAddress(out country, countryId, posatlCode);

				if (!posatlCode.IsNullOrEmpty()) shippingAddress.ZipCode = posatlCode;


				var shipppingCriteria = new ShipppingCriteria
				{
					CountryIsoCode2 = country.SimpleCode2,
					City = city.IsNullOrEmpty() ? shippingAddress.City.IsNullOrEmpty() ? "" : shippingAddress.City : city,
					PostCode = shippingAddress.ZipCode,
					GrossWeight = totalWeight / 1000,
					VolumeWeight = totalWeight / 1000,
					ClubLevel = SessionHelper.CurrentCustomer.IsNullOrEmpty() ? 0 : SessionHelper.CurrentCustomer.ClubLevel,
					TotalAmount = 0
				};

				#region 排序 前台js已经实现
				var sorter = new List<Sorter<ShoppingAmountSorterCriteria>>
				{
					new Sorter<ShoppingAmountSorterCriteria> {Key = ShoppingAmountSorterCriteria.ShippingCostLowToHigh, IsAsc = true}
				};
				var shippingAmounts = ServiceFactory.ShippingService.GetShippingAmounts(shipppingCriteria, sorter).ToList();

				//Service：该列的显示内容不同运送方式不同 ，具体如下
				//ID为3、4、18、33、34、36、37、53、54、55、56、58、60、67、69、76、81、83、90 的运送方式显示Door to Door
				//ID为49的运送方式，显示：  Not Door to Door(ship to Moscow)
				//ID为51的运送方式，显示：Not Door to Door (ship to local post office)
				//ID为59的 运送方式，显示：Not Door to Door [?] 鼠标悬停在问号上显示提示文字Moscow, St. Petersburg, Krasnoyarsk, Novosibirsk, Ekaterinburg, Irkutsk, Omsk, Kamchatka, Sakhalin, Yakutsk.
				//ID为64的运送方式，显示：Not Door to Door [?] 鼠标悬停在问号上显示提示语St. Petersburg, Krasnoyarsk, Novosibirsk, Ekaterinburg, Irkutsk, Omsk, Yakutsk, Ussuri, Khabarovsk, Tyumen, Vladivostok, Yakutsk.
				//Ship parcel to my Agent in China：Door to Door
				foreach (var shippingAmount in shippingAmounts)
				{
					switch (shippingAmount.ShippingId)
					{
						case 3:
						case 4:
						case 18:
						case 33:
						case 34:
						case 36:
						case 37:
						case 53:
						case 54:
						case 55:
						case 56:
						case 58:
						case 60:
						case 67:
						case 69:
						case 76:
						case 81:
						case 83:
						case 90:
                            shippingAmount.Service = Lang.TipDoorToDoor;
							break;
						case 49:
                            shippingAmount.Service = Lang.TipNoDoToDoMo;
							break;
						case 51:
							shippingAmount.Service = Lang.TipNoDoToDof;
							break;
						case 59:
                            shippingAmount.Service = Lang.TipNoDoToDoMS;
							break;
						case 64:
                            shippingAmount.Service = Lang.TipNoDoToDoSK;
							break;
						default:
                            shippingAmount.Service = Lang.TipDoorToDoor;
							break;
					}

				}
				#endregion

				ViewBag.ShippingLanguages = ServiceFactory.ShippingService.GetAllShippingDescs(LanguageId);
				ViewBag.ShippingAmounts = shippingAmounts;
			}
			catch (Exception ex)
			{
			}
			return PartialView("Partial/_ShippingItems");
		}
		#endregion

		#region 网站地图 Sitemap

		public ActionResult SitemapPage()
		{
			var categoryTree = new CategoryTreeVo
			{
				TreeType = CategoryTreeVo.CategoryTreeType.ProductSearch,
				CategoryRelatedDatas = ServiceFactory.CategoryService.GetCategoryTreeRecursiveCache(LanguageId)
			};
			ViewBag.CategoryTree = categoryTree;

			ViewBag.Sitemaps = Sitemap.GetSitemapPageSitemap();
			return View();
		}
		#endregion
	}
}
