using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class CacheController : Controller
    {
        public ActionResult CacheManager()
        {
            string updateType = Request["update_type"] ?? string.Empty;
            if (!updateType.IsNullOrEmpty())
            {
                var hashtable = new Hashtable();
                hashtable.Add("msg", "");
                hashtable.Add("error", false);
                //更新类别缓存
                if ("更新所有缓存".Equals(updateType))
                {
                    CacheHelper.ClearAllCache();
                }

                //更新网站语种缓存
                if ("更新网站语种缓存".Equals(updateType))
                {
                    CacheHelper.ClearLanguageIis();
                }

                //更新网站币种缓存
                if ("更新网站币种缓存".Equals(updateType))
                {
                    CacheHelper.ClearCurrencyIis();
                    ImplCacheHelper.ClearAllValidCurrencies();
                }

                //更新类别缓存
                if ("更新类别缓存".Equals(updateType))
                {
                    CacheHelper.ClearCategoryLanguageIis();
                    ImplCacheHelper.ClearAllRootCategories();
                    ImplCacheHelper.ClearCategoryCache();
                    var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                    var categories = ServiceFactory.CategoryService.GetAllCategories();
                    foreach (var language in languages)
                    {
                        ImplCacheHelper.ClearCategoryTreeRecursiveCache(language.LanguageId);
                    }
                    foreach (var category in categories)
                    {
                        ImplCacheHelper.ClearAllSubCategories(category.CategoryId);
                    }

                    
                }

                //更新属性缓存
                if ("更新属性缓存".Equals(updateType))
                {
                    var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                    var categories = ServiceFactory.CategoryService.GetAllCategories();
                    #region 属性
                    CacheHelper.ClearPropertyIis();
                    var properties = ServiceFactory.PropertyService.GetAllProperties();
                    ImplCacheHelper.ClearGetAllPropertyLanguages();
                    foreach (var property in properties)
                    {
                        ImplCacheHelper.ClearProperty(property.PropertyId);
                        ImplCacheHelper.ClearPropertyLanguagesById(property.PropertyId);
                    }
                    #endregion

                    #region 属性值组
                    CacheHelper.ClearPropertyValueGroupIis();
                    ImplCacheHelper.ClearAllPropertyValueGroupLanguages();
                    foreach (var language in languages)
                    {
                        ImplCacheHelper.ClearAllProperties(language.LanguageId);//属性
                        foreach (var category in categories)
                        {
                            ImplCacheHelper.ClearAllPropertiesOfCategory(category.CategoryId, language.LanguageId);//类别和属性间关系
                        }
                        ImplCacheHelper.ClearAllPropertyValueGroupLanguages(language.LanguageId);
                    }
                    #endregion

                    #region 属性值
                    CacheHelper.ClearPropertyValueIis();
                    ImplCacheHelper.ClearAllPropertyValues();
                    var propertyValues = ServiceFactory.PropertyService.GetAllPropertyValues();
                    foreach (var propertyValue in propertyValues)
                    {
                        ImplCacheHelper.ClearPropertyValue(propertyValue.PropertyValueId);
                        ImplCacheHelper.ClearAllPropertyValueLanguagesById(propertyValue.PropertyValueId);
                    }
                    #endregion
                }
                if ("更新类别展示右侧缓存".Equals(updateType))
                {
                    CacheHelper.ClearAllRightCategoryHomeAreaSetting();
                }
                if ("更新类别展示下方缓存".Equals(updateType))
                {
                    CacheHelper.ClearAllBelowCategoryHomeAreaSetting();
                }
                if ("首页横导航缓存".Equals(updateType))
                {
                    CacheHelper.ClearAllNavigationHomeAreaSetting();
                }

                //更新类别缓存
                if ("更新后台菜单缓存".Equals(updateType))
                {
                    ImplCacheHelper.ClearAllAdminMenus();
                }

                hashtable["msg"] = updateType + "成功!";
                return Json(hashtable);
            }
            return View();
        }

    }
}
