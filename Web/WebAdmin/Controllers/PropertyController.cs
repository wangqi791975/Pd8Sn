using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class PropertyController : BaseController
    {
        //
        // GET: /Property/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = 20;
            string keyWord = Request["keyword"] != null ? Request["keyword"].Trim() : string.Empty;

            PageData<Property> pageData = ServiceFactory.PropertyService.FindPropertiesForAdminList(page, pageSize, keyWord);

            ViewData.Model = pageData;
            return View();
        }

        public ActionResult GetInfo(int id)
        {
            Property property = ServiceFactory.PropertyService.GetPropertyById(id);
            if (property == null)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            IList<Language> languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            IList<PropertyLanguage> propertyLanguages = ServiceFactory.PropertyService.GetPropertyLanguagesById(id);

            ViewBag.Id = id;
            ViewBag.Property = property;
            ViewBag.Languages = languages;
            ViewBag.PropertyLanguages = propertyLanguages;
            return View();
        }

        public JsonResult ValidateDisplayOrder()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            int propertyId = Request["property_id"].ParseTo(0);
            int displayOrder = Request["display_order"].ParseTo(-1);
            if (propertyId < 1 || displayOrder < 0)
            {
                hashtable["msg"] = "前台排序不合法!";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            var properties = ServiceFactory.PropertyService.GetAllProperties();
            foreach (var property in properties)
            {
                if (property.PropertyId != propertyId && property.DisplayOrder == displayOrder)
                {
                    hashtable["msg"] = string.Format("该排序值和【{0}，ID：{1}】属性相同，请修改！", property.PropertyName, property.PropertyId);
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
            }
            return Json(hashtable);
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
                int propertyId = Request["id"].ParseTo(0);
                int displayOrder = Request["display_order"].ParseTo(-1);
                if (propertyId < 1)
                {
                    hashtable["msg"] = "属性不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (displayOrder < 0)
                {
                    hashtable["msg"] = "前台排序不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }

                Property property = new Property();
                property.PropertyId = propertyId;
                property.PropertyName = Request["property_name"] ?? string.Empty; ;
                property.DisplayOrder = displayOrder;
                //property.IsBasicProperty = Convert.ToBoolean(Request["is_basic_property"]);
                property.IsValid = Convert.ToBoolean(Request["is_valid"]);
                property.IsDisplay = Convert.ToBoolean(Request["is_display"]);
                property.SortType = EnumHelper.ToEnum<PropertyValueSortType>(Request["sort_type"].ParseTo(0));
                ServiceFactory.PropertyService.SetPropertyBaseInfo(property);

                #region 修改PropertyLanguage
                string[] languageIds = Request["language_ids"].Split(',');
                IList<PropertyLanguage> propertyLanguages = new List<PropertyLanguage>();
                foreach (var languageId in languageIds)
                {
                    var propertyLanguage = new PropertyLanguage();
                    propertyLanguage.PropertyId = propertyId;
                    propertyLanguage.PropertyName = Request["property_name_" + languageId] ?? string.Empty;
                    propertyLanguage.LanguageId = languageId.ParseTo(1);
                    propertyLanguages.Add(propertyLanguage);
                }
                if (propertyLanguages.Count > 0)
                {
                    ServiceFactory.PropertyService.UpdatePropertyLanguages(propertyLanguages);
                    ImplCacheHelper.ClearPropertyLanguagesById(propertyId);
                }
                #endregion
                #region 清除缓存
                var categories = ServiceFactory.CategoryService.GetAllCategories();
                CacheHelper.ClearPropertyIis();
                ImplCacheHelper.ClearGetAllPropertyLanguages();
                ImplCacheHelper.ClearAllPropertyValues();
                ImplCacheHelper.ClearProperty(propertyId);
                ImplCacheHelper.ClearPropertyLanguagesById(property.PropertyId);
                foreach (var languageId in languageIds)
                {
                    ImplCacheHelper.ClearAllProperties(languageId.ParseTo(1));
                    foreach (var category in categories)
                    {
                        ImplCacheHelper.ClearAllPropertiesOfCategory(category.CategoryId, languageId.ParseTo(1));//类别和属性间关系
                    }
                }
                #endregion
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = ActionJsonResult.Error;
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

    }
}
