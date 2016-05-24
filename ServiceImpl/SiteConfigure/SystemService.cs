using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure
{
    public class SystemService : ISystemService
    {   
        public void LoadCacheAtferInit()
        {
            if (ServiceConfig.IsLoadCacheInit)
            {   
                LoadCache(ServiceConfig.IsLoadProductCacheInit);
            }
        }


        public void LoadCache(bool isLoadProductCache = false)
        {
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin LoadCache*************");

            var beginDate = DateTime.Now;
            var startDate = beginDate;

            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.CategoryService.GetAllCategories*************");
            //加载类别
            ServiceFactory.CategoryService.GetAllCategories();
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.CategoryService.GetAllCategories,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载英文类别树
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.CategoryService.GetCategoryTreeRecursive(null, ServiceFactory.ConfigureService.SiteLanguageId)*************");
            ServiceFactory.CategoryService.GetCategoryTreeRecursiveCache(ServiceFactory.ConfigureService.SiteLanguageId);
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.CategoryService.GetAllCategories,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载属性多语言
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertyLanguages*************");
            ServiceFactory.PropertyService.GetAllPropertyLanguages();
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertyLanguages,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载当前环境语言的所有属性
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId)*************");
            ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId),timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);


            //加载属性值组多语言
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages*************");
            ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages();
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertyLanguages,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载属性值多语言
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertyValueLanguages*************");
            ServiceFactory.PropertyService.GetAllPropertyValueLanguages();
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertyValueLanguages,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载当前环境语言的所有属性
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId)*************");
            ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertiesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId),timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载当前环境语言的所有属性值组
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages(ServiceFactory.ConfigureService.SiteLanguageId)*************");
            ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages(ServiceFactory.ConfigureService.SiteLanguageId);
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertyValueGroupLanguages(ServiceFactory.ConfigureService.SiteLanguageId),timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载当前环境语言的所有属性值
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.PropertyService.GetAllPropertyValuesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId)*************");
            ServiceFactory.PropertyService.GetAllPropertyValuesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId);
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.PropertyService.GetAllPropertyValuesOfLanguage(ServiceFactory.ConfigureService.SiteLanguageId),timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);


            //加载类别属性
            startDate = DateTime.Now;
            LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.CategoryService.LoadCategoryProperties*************");
            ServiceFactory.CategoryService.LoadCategoryProperties();
            LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.CategoryService.LoadCategoryProperties,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);

            //加载产品基本信息
            if (isLoadProductCache)
            {
                startDate = DateTime.Now;
                LoggerHelper.GetLogger(LoggerType.Service).Info("**************Begin ServiceFactory.ProductService.LoadAllProducts*************");
                ServiceFactory.ProductService.LoadAllProducts();
                LoggerHelper.GetLogger(LoggerType.Service).InfoFormat("**************End ServiceFactory.ProductService.LoadAllProducts,timer:{0} seconds.*************", DateTime.Now.Subtract(startDate).TotalSeconds);
            }

            LoggerHelper.GetLogger(LoggerType.Service).Info("**************End LoadCache*************");
        }

        public bool ClearAllCache()
        {
            return MemcachedHelper.Instance.FlushAll();
        }

        public bool ClearCache(string key)
        {
            return MemcachedHelper.Instance.Delete(key);
        }

        public bool IsExists(string key)
        {
            return MemcachedHelper.Instance.IsExists(key);
        }
    }
}
