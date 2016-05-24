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
    public class PropertyValueController : BaseController
    {
        //
        // GET: /PropertyValue/

        public ActionResult Index(int id)
        {
            if (id < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            Property property = ServiceFactory.PropertyService.GetPropertyById(id);
            IList<KeyValuePair<string, string>> breadcrumbs = new List<KeyValuePair<string, string>>();
            breadcrumbs.Add(new KeyValuePair<string, string>("属性管理", "/Property/Index"));
            breadcrumbs.Add(new KeyValuePair<string, string>(property.PropertyName, ""));

            ViewBag.Id = id;
            ViewBag.Breadcrumbs = breadcrumbs;
            return View();
        }

        public ActionResult GetList(int id)
        {
            int propertyId = id.ParseTo(0);
            int page = Request["page"].ParseTo(1);
            int pageSize = 20;
            string keyWord = Request["keyword"] != null ? Request["keyword"].Trim() : string.Empty;

            PageData<PropertyValue> pageData = ServiceFactory.PropertyService.FindPropertyValuesForAdminList(propertyId, page, pageSize, keyWord);

            ViewData.Model = pageData;
            return View();
        }

        public ActionResult GetInfo(int id)
        {
            PropertyValue propertyValue = ServiceFactory.PropertyService.GetPropertyValue(id);
            if (propertyValue == null)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            IList<Language> languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            IList<PropertyValueLanguage> propertyValueLanguages =
                ServiceFactory.PropertyService.GetAllPropertyValueLanguagesById(id);
            PropertyValueGroup propertyValueGroup = ServiceFactory.PropertyService.GetPropertyValueGroupById(propertyValue.PropertyValueGroupId);

            ViewBag.Id = id;
            ViewBag.PropertyValue = propertyValue;
            ViewBag.Languages = languages;
            ViewBag.PropertyValueLanguages = propertyValueLanguages;
            ViewBag.PropertyValueGroup = propertyValueGroup;

            return View();
        }

        public JsonResult ValidateDisplayOrder()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            int propertyValueId = Request["property_value_id"].ParseTo(0);
            int displayOrder = Request["display_order"].ParseTo(-1);
            if (propertyValueId < 1 || displayOrder < 0)
            {
                hashtable["msg"] = "前台排序不合法!";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            var property = ServiceFactory.PropertyService.GetPropertyValue(propertyValueId);
            var propertyValues = ServiceFactory.PropertyService.GetAllPropertyValuesOfProperty(property.PropertyId);
            foreach (var value in propertyValues)
            {
                if (value.PropertyValueId != propertyValueId && value.DisplayOrder == displayOrder)
                {
                    hashtable["msg"] = string.Format("该排序值和【{0}，ID：{1}】属性值相同，请修改！", value.PropertyValueName, value.PropertyValueId);
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult Update()
        {
            var hashtable = new Hashtable();
            hashtable.Add("getlist", true);
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                int propertyValueId = Request["id"].ParseTo(0);
                int displayOrder = Request["display_order"].ParseTo(-1);
                if (propertyValueId < 1)
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

                PropertyValue propertyValue = new PropertyValue();
                propertyValue.PropertyValueId = propertyValueId;
                propertyValue.PropertyValueName = Request["property_value_name"];
                propertyValue.DisplayOrder = displayOrder;
                propertyValue.IsValid = Convert.ToBoolean(Request["is_valid"]);
                ServiceFactory.PropertyService.SetPropertyValueBaseInfo(propertyValue);

                #region 修改PropertyLanguage
                string[] languageIds = Request["language_ids"].Split(',');
                IList<PropertyValueLanguage> propertyLanguages = new List<PropertyValueLanguage>();
                foreach (var languageId in languageIds)
                {
                    var propertyValueLanguage = new PropertyValueLanguage();
                    propertyValueLanguage.PropertyValueId = propertyValueId;
                    propertyValueLanguage.PropertyValueName = Request["property_value_name_" + languageId] ?? string.Empty;
                    propertyValueLanguage.LanguageId = languageId.ParseTo(1);
                    propertyLanguages.Add(propertyValueLanguage);
                }
                if (propertyLanguages.Count > 0)
                {
                    ServiceFactory.PropertyService.UpdatePropertyValueLanguages(propertyLanguages);
                    ImplCacheHelper.ClearAllPropertyValueLanguagesById(propertyValueId);
                }
                #endregion
                #region 清除缓存
                CacheHelper.ClearPropertyValueIis();
                ImplCacheHelper.ClearAllPropertyValues();
                ImplCacheHelper.ClearPropertyValue(propertyValueId);
                ImplCacheHelper.ClearAllPropertyValueLanguagesById(propertyValueId);
                foreach (var languageId in languageIds)
                {
                    ImplCacheHelper.ClearAllPropertyValueGroupLanguages(languageId.ParseTo(1));
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
