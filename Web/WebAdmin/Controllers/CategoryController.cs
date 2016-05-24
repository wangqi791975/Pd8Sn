using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;
using Newtonsoft.Json.Linq;

namespace Com.Panduo.Web.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /Category/

        public ActionResult Index(int? id)
        {
            ViewBag.Id = id;
            return View();
        }

        //[ChildActionOnly]
        public ActionResult GetList(string id)
        {
            IList<KeyValuePair<string, string>> breadcrumbs = new List<KeyValuePair<string, string>>();
            breadcrumbs.Add(new KeyValuePair<string, string>("类别管理", "/Category/GetList"));

            var categoryLanguages = ServiceFactory.CategoryService.GetAllCategoryLanguagesByLanguageId(ServiceFactory.ConfigureService.EnglishLangId);
            int idFinal = 0;
            if (!id.IsNullOrEmpty() && !id.Equals("0"))
            {
                if (id.Contains("_"))
                {
                    string[] idArray = id.Split('_');
                    idFinal = Convert.ToInt32(idArray[idArray.Length - 1]);
                    string backId = "";
                    string value = "";
                    for (int i = 0; i < idArray.Length; i++)
                    {
                        if (!idArray[i].Equals("0"))
                        {
                            value = (!backId.IsNullOrEmpty() ? backId + "_" : "") + idArray[i];
                            Category categoryTemp = ServiceFactory.CategoryService.GetCategoryById(Convert.ToInt32(idArray[i]));
                            if (categoryTemp != null)
                            {
                                Category category = new Category { CategoryId = categoryTemp.CategoryId, CategoryImage = categoryTemp.CategoryImage, ParentId = categoryTemp.ParentId, IsDisplay = categoryTemp.IsDisplay, DiplayOrder = categoryTemp.DiplayOrder, CategoryName = categoryLanguages.FirstOrDefault(w => w.CategoryId == categoryTemp.CategoryId).CategoryLanguageName };
                                if (i != idArray.Length - 1)
                                {
                                    breadcrumbs.Add(new KeyValuePair<string, string>(category.CategoryName, "/Category/GetList/" + value));
                                }
                                else
                                {
                                    breadcrumbs.Add(new KeyValuePair<string, string>(category.CategoryName, ""));
                                }
                            }

                            backId = (!backId.IsNullOrEmpty() ? backId + "_" : "") + idArray[i];
                        }
                    }
                }
                else
                {
                    idFinal = Convert.ToInt32(id);
                    Category categoryTemp = ServiceFactory.CategoryService.GetCategoryById(idFinal);
                    if (categoryTemp != null)
                    {
                        Category category = new Category { CategoryId = categoryTemp.CategoryId, CategoryImage = categoryTemp.CategoryImage, ParentId = categoryTemp.ParentId, IsDisplay = categoryTemp.IsDisplay, DiplayOrder = categoryTemp.DiplayOrder, CategoryName = categoryLanguages.FirstOrDefault(w => w.CategoryId == categoryTemp.CategoryId).CategoryLanguageName };
                        breadcrumbs.Add(new KeyValuePair<string, string>(category.CategoryName, ""));
                    }

                }
            }

            IList<Category> categories = null;
            if (idFinal != 0)
            {
                categories = ServiceFactory.CategoryService.GetAllSubCategories(Convert.ToInt32(idFinal));
            }
            else
            {
                categories = ServiceFactory.CategoryService.GetAllRootCategories();
            }

            categories = categories.Select(x => new Category { CategoryId = x.CategoryId, CategoryImage = x.CategoryImage, ParentId = x.ParentId, IsDisplay = x.IsDisplay, DiplayOrder = x.DiplayOrder, CategoryName = categoryLanguages.FirstOrDefault(w => w.CategoryId == x.CategoryId).CategoryLanguageName }).OrderBy(x => x.DiplayOrder).ToList();

            ViewBag.Id = id;
            ViewBag.Breadcrumbs = breadcrumbs;
            ViewBag.Categories = categories;
            return View();
        }

        public ActionResult GetInfo(int id)
        {
            Category category = ServiceFactory.CategoryService.GetCategoryById(id);
            ViewBag.Category = category;
            return View("GetInfo");
        }

        [HttpPost]
        public JsonResult Update()
        {
            var hashtable = new Hashtable();
            hashtable.Add("getlist", true);
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                int categoryId = Request["id"].ParseTo(0);
                int parentId = Request["parent_id"].ParseTo(0);
                bool isDisplay = Convert.ToBoolean(Request["is_display"]);
                int displayOrder = Request["display_order"].ParseTo(-1);
                if (categoryId < 1)
                {
                    hashtable["msg"] = "类别不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (displayOrder < 0)
                {
                    hashtable["msg"] = "前台排序不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                Category category = new Category();
                category.CategoryId = categoryId;
                category.CategoryName = Request["category_name"] ?? string.Empty;
                category.IsDisplay = isDisplay;
                category.DiplayOrder = displayOrder;
                //如果设置为隐藏，要判断该类别下的所有子类别是否已全部隐藏
                if (category.IsDisplay == false)
                {
                    IList<RelatedData<Service.Product.Category.Category>> relatedDatas =
                        ServiceFactory.CategoryService.GetCategoryTreeRecursive(categoryId);
                    int displays = GetCategoryDisplays(relatedDatas, 0);
                    if (displays > 0)
                    {
                        hashtable["getlist"] = false;
                        hashtable["msg"] = string.Format("当前类别有{0}个开启状态的子类别，不允许关闭!", displays);
                        hashtable["error"] = true;
                        return Json(hashtable);
                    }

                }
                ServiceFactory.CategoryService.SetCategoryBaseInfo(category);

                #region 清除缓存
                CacheHelper.ClearCategoryLanguageIis();
                ImplCacheHelper.ClearAllSubCategories(parentId);
                ImplCacheHelper.ClearAllRootCategories();
                ImplCacheHelper.ClearCategoryCache();
                #endregion
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = ActionJsonResult.Error;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public ActionResult Edit(int id)
        {

            Category category = ServiceFactory.CategoryService.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            IList<Language> languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            IList<CategoryLanguage> categoryLanguage = ServiceFactory.CategoryService.GetCategoryLanguageById(id);

            IList<KeyValuePair<string, string>> breadcrumbs = new List<KeyValuePair<string, string>>();
            breadcrumbs.Add(new KeyValuePair<string, string>("类别管理", "/Category/Index"));
            breadcrumbs.Add(new KeyValuePair<string, string>(string.Format("{0}", categoryLanguage.FirstOrDefault(x => x.LanguageId == ServiceFactory.ConfigureService.EnglishLangId).CategoryLanguageName.ToString()), ""));

            Boolean isLeafCategory = ServiceFactory.CategoryService.IsLeafCategory(id);
            IList<Property> properties = null;
            if (isLeafCategory)
            {
                properties = ServiceFactory.PropertyService.GetAllPropertiesByCategoryId(id);
            }

            ViewBag.Id = id;
            ViewBag.Category = category;
            ViewBag.Languages = languages;
            ViewBag.CategoryLanguage = categoryLanguage;
            ViewBag.IsLeafCategory = isLeafCategory;
            ViewBag.Properties = properties;
            ViewBag.Breadcrumbs = breadcrumbs;
            return View();
        }

        public JsonResult ValidateDisplayOrder()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            int categoryId = Request["category_id"].ParseTo(0);
            int displayOrder = Request["display_order"].ParseTo(-1);
            if (categoryId < 1 || displayOrder < 0)
            {
                hashtable["msg"] = "前台排序不合法!";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            var categoryParent = ServiceFactory.CategoryService.GetParentCategoryById(categoryId);
            int parentId = 0;
            if (categoryParent != null)
            {
                parentId = categoryParent.CategoryId;
            }
            var categories = ServiceFactory.CategoryService.GetTopSubCategoriesById(parentId, 100);
            foreach (var category in categories)
            {
                if (category.CategoryId != categoryId && category.DiplayOrder == displayOrder)
                {
                    var currentCategory = ServiceFactory.CategoryService.GetCategoryLanguageById(category.CategoryId, ServiceFactory.ConfigureService.EnglishLangId);
                    hashtable["msg"] = string.Format("当前排序与同级【{0}，ID：{1}】前台排序编号重复，请修改！", currentCategory.CategoryLanguageName, category.CategoryId);
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
            }
            return Json(hashtable);
        }

        [ValidateInput(false)]
        public JsonResult Submit()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                #region 修改Category
                int categoryId = Request["id"].ParseTo(0);
                int parentId = Request["parent_id"].ParseTo(0);
                bool isDisplay = Convert.ToBoolean(Request["is_display"]);
                int displayOrder = Request["display_order"].ParseTo(-1);
                if (categoryId < 1)
                {
                    hashtable["msg"] = "类别不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (displayOrder < 0)
                {
                    hashtable["msg"] = "前台排序不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                Category category = new Category();
                category.CategoryId = categoryId;
                category.IsDisplay = isDisplay;
                category.DiplayOrder = displayOrder;
                //如果设置为隐藏，要判断该类别下的所有子类别是否已全部隐藏
                if (category.IsDisplay == false)
                {
                    IList<RelatedData<Service.Product.Category.Category>> relatedDatas =
                        ServiceFactory.CategoryService.GetCategoryTreeRecursive(categoryId);
                    int displays = GetCategoryDisplays(relatedDatas, 0);
                    if (displays > 0)
                    {
                        hashtable["getlist"] = false;
                        hashtable["msg"] = string.Format("当前类别有{0}个开启状态的子类别，不允许关闭!", displays);
                        hashtable["error"] = true;
                        return Json(hashtable);
                    }

                }
                var categoryImageFolder = "Upload/Category";
                var categoryImageFile = Request.Files["category_image"];
                string categoryImage = string.Empty;
                if (categoryImageFile != null && categoryImageFile.ContentLength != 0)
                {
                    string uploadName = Guid.NewGuid().ToString();
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../" + categoryImageFolder),
                        uploadName + Path.GetExtension(categoryImageFile.FileName));
                    categoryImage = "/" + categoryImageFolder + "/" + uploadName + Path.GetExtension(categoryImageFile.FileName);
                    categoryImageFile.SaveAs(filePath);
                    if (!Request["category_image_hidden"].IsNullOrEmpty())
                    {
                        try
                        {
                            System.IO.File.Delete(Path.Combine(HttpContext.Server.MapPath(Request["category_image_hidden"]), ""));
                        }
                        catch (DirectoryNotFoundException)
                        {

                        }
                    }
                }
                else
                {
                    categoryImage = Request["category_image_hidden"] ?? string.Empty;
                }
                category.CategoryImage = categoryImage;

                ServiceFactory.CategoryService.SetCategoryBaseInfo(category);
                #endregion

                #region 修改CategoryLanguage和CategoryAdvertisement
                string[] languageIds = Request["language_ids"].Split(',');
                IList<CategoryLanguage> categoryLanguages = new List<CategoryLanguage>();
                IList<CategoryAdvertisement> categoryAdvertisements = new List<CategoryAdvertisement>();
                string advertisingImageFolder = "Upload/CagegoryAd";
                string advertisingImagePath = "";
                foreach (var languageId in languageIds)
                {
                    CategoryLanguage categoryLanguage = new CategoryLanguage();
                    categoryLanguage.CategoryId = categoryId;
                    categoryLanguage.LanguageId = languageId.ParseTo(1);
                    categoryLanguage.CategoryLanguageName = Request["category_language_name_" + languageId] ?? string.Empty;
                    categoryLanguage.CategoryLanguageDescription = Request["category_language_description_" + languageId] ?? string.Empty;
                    categoryLanguages.Add(categoryLanguage);

                    CategoryAdvertisement categoryAdvertisement = new CategoryAdvertisement();
                    var filePost = Request.Files["advertising_image_" + languageId];
                    if (filePost != null && filePost.ContentLength != 0)
                    {
                        string uploadName = Guid.NewGuid().ToString();
                        string filePath = Path.Combine(HttpContext.Server.MapPath("../" + advertisingImageFolder),
                            uploadName + Path.GetExtension(filePost.FileName));
                        advertisingImagePath = "/" + advertisingImageFolder + "/" + uploadName + Path.GetExtension(filePost.FileName);
                        filePost.SaveAs(filePath);
                        if (!Request["advertising_image_hidden_" + languageId].IsNullOrEmpty())
                        {
                            try
                            {
                                System.IO.File.Delete(Path.Combine(HttpContext.Server.MapPath(Request["advertising_image_hidden_" + languageId]), ""));
                            }
                            catch (DirectoryNotFoundException)
                            {

                            }
                        }
                    }
                    else
                    {
                        advertisingImagePath = Request["advertising_image_hidden_" + languageId] ?? string.Empty;
                    }

                    categoryAdvertisement.CategoryId = categoryId;
                    categoryAdvertisement.LanguageId = languageId.ParseTo(1);
                    categoryAdvertisement.MarketingTitle = Request["marketing_title_" + languageId] ?? string.Empty;
                    categoryAdvertisement.AdvertisingImage = advertisingImagePath;
                    categoryAdvertisement.AdvertisingWords = Request["advertising_words_" + languageId] ?? string.Empty;
                    categoryAdvertisement.Url = Request["url_" + languageId] ?? string.Empty;
                    categoryAdvertisement.ProductMarketingArea = Request["product_marketing_area_" + languageId] ?? string.Empty;
                    categoryAdvertisements.Add(categoryAdvertisement);

                }
                ServiceFactory.CategoryService.UpdateCategoryLanguages(categoryLanguages);
                ServiceFactory.CategoryService.UpdateCategoryAdvertisementLanguages(categoryAdvertisements);
                #endregion

                #region 修改CategoryProperty
                string[] propertyIds = Request["property_ids"].Split(',');
                IList<CategoryProperty> categoryProperties = new List<CategoryProperty>();
                foreach (var propertyId in propertyIds)
                {
                    CategoryProperty categoryProperty = new CategoryProperty();
                    categoryProperty.CategoryId = categoryId;
                    categoryProperty.PropertyId = propertyId.ParseTo(0);
                    categoryProperty.IsDisplay = Request["property_id_" + propertyId] != null ? true : false;
                    categoryProperty.DisplayOrder = Request["display_order_" + propertyId].ParseTo(0);
                    categoryProperties.Add(categoryProperty);
                }
                ServiceFactory.CategoryService.UpdateCategoryProperties(categoryProperties);
                #endregion

                #region 清除缓存
                CacheHelper.ClearCategoryLanguageIis();
                ImplCacheHelper.ClearAllSubCategories(parentId);
                ImplCacheHelper.ClearAllRootCategories();
                ImplCacheHelper.ClearCategoryCache();
                foreach (var languageId in languageIds)
                {
                    ImplCacheHelper.ClearCategoryTreeRecursiveCache(languageId.ParseTo(1));
                }
                #endregion
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }

            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult PopularSearch()
        {
            ViewBag.ParentCategory = ServiceFactory.CategoryService.GetAllRootCategories();
            return View();
        }

        [HttpGet]
        public ActionResult GetPopularSearch(int id)
        {
            ViewBag.CategoryId = id;
            return View();
        }

        public ActionResult NodeItem()
        {
            return View();
        }

        [HttpPost]
        public bool UpdateKeyword(int langId, int categoryId, string keywordobjs)
        {
            try
            {
                ServiceFactory.CategoryService.DeleteCategoryKeywordByCategoryIdAndLangId(categoryId, langId);
                var categoryKeywords = new List<CategoryKeyword>();
                var keywords = Regex.Split(keywordobjs, "{,}");
                foreach (var keyword in keywords)
                {
                    JObject json = JObject.Parse(keyword);
                    var categoryKeyword = new CategoryKeyword
                    {
                        CategoryId = categoryId,
                        Keyword = json["Keyword"].ToString(),
                        Url = json["Url"].ToString(),
                        DiplayOrder = json["DiplayOrder"].ToString(),
                        LanguageId = langId,
                    };
                    ServiceFactory.CategoryService.SetCategoryKeyword(categoryKeyword);
                    categoryKeywords.Add(categoryKeyword);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        [HttpPost]
        public bool DeleteKeyword(int keywordId)
        {
            try
            {
                ServiceFactory.CategoryService.DeleteCategoryKeywordById(keywordId);
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 调用类别递归对查询类别下为显示类别树的数量
        /// </summary>
        /// <param name="relatedDatas">类别递归树</param>
        /// <returns></returns>
        private int GetCategoryDisplays(IList<RelatedData<Service.Product.Category.Category>> relatedDatas, int displays = 0)
        {
            foreach (var data in relatedDatas)
            {
                if (data.Data.IsDisplay == true)
                {
                    displays++;
                }
                if (!data.SubDataList.IsNullOrEmpty())
                {
                    displays = GetCategoryDisplays(data.SubDataList, displays);
                }
            }
            return displays;
        }
    }
}
