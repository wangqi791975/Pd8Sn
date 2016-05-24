using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.SEO;
using Com.Panduo.Service.ServiceConst;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Panduo.Service.Product.ProductArea;

namespace Com.Panduo.Web.Controllers
{
    public class MetaController : Controller
    {

        public ActionResult Index()
        {
            var type = Request["type"].ParseTo(0);
            if (Request.IsAjaxRequest())
            {
                switch (type)
                {
                    case 1:
                        return MetaHome();
                    case 2:
                        return MetaList();
                    case 3:
                        return MetaProductDetail();
                    case 4:
                        return MetaArea();
                    default:
                        return Content("请选择一项。。。");
                }
            }
            ViewBag.HidType = type;
            ViewBag.HidAreaId = Request["areaId"].ParseTo(0);
            return View("~/Views/SEO/Meta/Index.cshtml");
        }

        #region 设置首页Meta信息
        public ActionResult MetaHome()
        {
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            var metaHome = new Dictionary<int, List<MetaHome>>();
            foreach (var lang in languages)
            {
                metaHome[lang.LanguageId] = ServiceFactory.MetaService.GetMetaHomesByLanguageId(lang.LanguageId).ToList();
            }

            ViewBag.Languages = languages;
            return View("~/Views/SEO/Meta/MetaHome.cshtml", metaHome);
        }

        public ActionResult SetMetaHome()
        {
            var hashtable = new Hashtable();

            var metaHomeType = Request["HID_metaHomeType"] ?? string.Empty;
            var languageId = Request["HID_languageId"].ParseTo(0);
            if (!metaHomeType.IsNullOrEmpty() && languageId > 0)
            {
                var metaHomeTypeList = metaHomeType.Split(",");
                var metaHomes = new List<MetaHome>();
                foreach (var type in metaHomeTypeList)
                {
                    var metaHome = new MetaHome
                    {
                        LanguageId = languageId,
                        PageType = type.ToEnum<MetaHomePageType>(),
                        Breadcrumb = Request["FD_Breadcrumb_" + type] ?? string.Empty,
                        Title = Request["FD_Title_" + type] ?? string.Empty,
                        Keywords = Request["FD_Keywords_" + type] ?? string.Empty,
                        Description = Request["FD_Description_" + type] ?? string.Empty
                    };
                    metaHomes.Add(metaHome);
                }
                ServiceFactory.MetaService.SetMetaHome(metaHomes);
            }
            else
            {
                hashtable.Add("error", true);
            }

            return Json(hashtable);
        }
        #endregion

        #region 设置商品详情页Meta信息
        public ActionResult MetaProductDetail()
        {
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            var metaProductDetail = new Dictionary<int, MetaHome>();
            foreach (var lang in languages)
            {
                metaProductDetail[lang.LanguageId] = ServiceFactory.MetaService.GetMetaHomeByType(MetaHomePageType.ProductDetail, lang.LanguageId);
            }

            ViewBag.Languages = languages;
            return View("~/Views/SEO/Meta/MetaProductDetail.cshtml", metaProductDetail);
        }
        #endregion

        #region 设置各区商品列表Meta信息
        public ActionResult MetaList()
        {
            var categoryId = Request["HID_categoryId"].ParseTo(0);
            var languageId = Request["HID_languageId"].ParseTo(0);
            if (categoryId > 0 && languageId > 0)
            {
                var currentCategory = ServiceFactory.CategoryService.GetCategoryLanguageFamliy(categoryId, ServiceFactory.ConfigureService.EnglishLangId);
                ViewBag.CurrentCategory = currentCategory;
                ViewBag.CategoryId = categoryId;
                ViewBag.LanguageId = languageId;
                var metaList = ServiceFactory.MetaService.GetMetaListByType(categoryId, languageId).ToList();
                return View("~/Views/SEO/Meta/MetaListInfo.cshtml", metaList);
            }

            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            var categories = ServiceFactory.CategoryService.GetCategoryTreeRecursiveCache(ServiceFactory.ConfigureService.EnglishLangId);
            return View("~/Views/SEO/Meta/MetaList.cshtml", categories);
        }

        public ActionResult SetMetaList()
        {
            var hashtable = new Hashtable();

            var metaListType = Request["HID_metaListType"] ?? string.Empty;
            var categoryId = Request["HID_categoryId"].ParseTo(0);
            var languageId = Request["HID_languageId"].ParseTo(0);
            if (!metaListType.IsNullOrEmpty() && languageId > 0 && categoryId > 0)
            {
                var metaListTypeList = metaListType.Split(",");
                var metaLists = new List<MetaList>();
                foreach (var type in metaListTypeList)
                {
                    var metaHome = new MetaList
                    {
                        CategoryId = categoryId,
                        PageType = type.ToEnum<MetaListPageType>(),
                        LanguageId = languageId,
                        Alias = Request["FD_Alias"] ?? string.Empty,
                        Breadcrumb = Request["FD_Breadcrumb"] ?? string.Empty,
                        Title = Request["FD_Title_" + type] ?? string.Empty,
                        Keywords = Request["FD_Keywords_" + type] ?? string.Empty,
                        Description = Request["FD_Description_" + type] ?? string.Empty,
                        TitlePro = Request["FD_TitlePro_" + type] ?? string.Empty,
                        KeywordsPro = Request["FD_KeywordsPro_" + type] ?? string.Empty,
                        DescriptionPro = Request["FD_DescriptionPro_" + type] ?? string.Empty
                    };
                    metaLists.Add(metaHome);
                }
                ServiceFactory.MetaService.SetMetaList(metaLists);
            }
            else
            {
                hashtable.Add("error", true);
            }

            return Json(hashtable);
        }
        #endregion

        #region 设置专区首页SEO信息
        public ActionResult MetaArea()
        {
            var areaId = Request["HID_areaId"].ParseTo(0);
            if (areaId > 0 && Request.IsAjaxRequest())
            {
                var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                var metaArea = new Dictionary<int, MetaArea>();
                foreach (var lang in languages)
                {
                    metaArea[lang.LanguageId] = ServiceFactory.MetaService.GetMetaAreasByAreaId(areaId, lang.LanguageId);
                }
                ViewBag.Languages = languages;
                ViewBag.AreaId = areaId;
                return View("~/Views/SEO/Meta/MetaAreaInfo.cshtml", metaArea);
            }

            ViewBag.HidAreaId = Request["areaId"].ParseTo(0);
            var areas = ServiceFactory.ProductAreaService.FindProductAreas(1, 9999,
                new Dictionary<ProductAreaSearchCriteria, object>(), new List<Sorter<ProductAreaSorterCriteria>>());
            return View("~/Views/SEO/Meta/MetaArea.cshtml", areas);
        }

        public ActionResult SetMetaArea()
        {
            var hashtable = new Hashtable();

            var areaId = Request["HID_areaId"].ParseTo(0);
            var languageId = Request["HID_languageId"].ParseTo(0);
            if (languageId > 0 && areaId > 0)
            {
                var metaAreas = new List<MetaArea>();
                var metaarea = new MetaArea
                {
                    AreaId = areaId,
                    LanguageId = languageId,
                    PageName = Request["FD_PageName"] ?? string.Empty,
                    Title = Request["FD_Title"] ?? string.Empty,
                    Keywords = Request["FD_Keywords"] ?? string.Empty,
                    Description = Request["FD_Description"] ?? string.Empty
                };
                metaAreas.Add(metaarea);
                ServiceFactory.MetaService.SetMetaArea(metaAreas);
            }
            else
            {
                hashtable.Add("error", true);
            }

            return Json(hashtable);
        }
        #endregion
    }
}
