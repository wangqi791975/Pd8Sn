//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ImplCacheHelper.cs
//创 建 人：罗海明
//创建时间：2014/12/24 16:00:40 
//功能说明：产品相关的Cache
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.AdminUser;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.DailyDeal;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.ServiceConst;
using System.Linq;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.ServiceImpl
{
    public class ImplCacheHelper
    {
        #region 产品

        /// <summary>
        /// 设置产品Code缓存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool SetProductCode(int productId, string code)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_CODE, code), productId);
        }

        /// <summary>
        /// 获取产品Code对应的产品ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int GetProductCode(string code)
        {
            return MemcachedHelper.Instance.Get<int>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_CODE, code), 0);
        }

        /// <summary>
        /// 设置产品Cache
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns></returns>
        public static bool SetProduct(Service.Product.Product product)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, product.ProductId), product);
        }
        /// <summary>
        /// 从Cache获取产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public static Service.Product.Product GetProduct(int productId)
        {
            return MemcachedHelper.Instance.Get<Service.Product.Product>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, productId));
        }

        public static bool ClearProduct(int productId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, productId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 设置产品多语种Cache
        /// </summary>
        /// <param name="product">产品多语种</param>
        /// <returns></returns>
        public static bool SetProductLanguage(Service.Product.ProductLanguage product)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, product.ProductId, product.LanguageId), product);
        }
        /// <summary>
        /// 从Cache获取产品多语种
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public static Service.Product.ProductLanguage GetProductLanguage(int productId, int languageId)
        {
            return MemcachedHelper.Instance.Get<Service.Product.ProductLanguage>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, productId, languageId));
        }

        /// <summary>
        /// 设置产品主图片Cache
        /// </summary>
        /// <param name="productImages">产品图片</param>
        /// <returns></returns>
        public static bool SetProductImage(Service.Product.ProductImages productImages)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_IMG, productImages.ProductId), productImages);
        }

        /// <summary>
        /// 从Cache获取产品主图片
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public static Service.Product.ProductImages GetProductImage(int productId)
        {
            return MemcachedHelper.Instance.Get<Service.Product.ProductImages>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_IMG, productId));
        }


        /// <summary>
        /// 设置产品图片Cache
        /// </summary>
        /// <param name="productImages">产品图片</param>
        /// <returns></returns>
        public static bool SetProductImage(IList<Service.Product.ProductImages> productImages)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_IMG_LIST, productImages[0].ProductId), productImages);
        }

        /// <summary>
        /// 从Cache获取产品图片
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public static IList<Service.Product.ProductImages> GetProductImages(int productId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.ProductImages>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_IMG_LIST, productId));
        }
        #endregion

        #region 属性
        public static bool SetProperty(Service.Product.Property.Property property)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY, property.PropertyId), property);
        }

        public static Service.Product.Property.Property GetProperty(int propertyId)
        {
            return MemcachedHelper.Instance.Get<Service.Product.Property.Property>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY, propertyId));
        }

        public static bool ClearProperty(int propertyId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY, propertyId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetAllPropertyValues(IList<Service.Product.Property.PropertyValue> propertyValues)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE, MemcachedConst.KEY_ALL), MemcachedConst.KEY_ALL);
        }

        public static IList<Service.Product.Property.PropertyValue> GetAllPropertyValues()
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValue>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE, MemcachedConst.KEY_ALL));
        }

        public static bool ClearAllPropertyValues()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetPropertyValue(Service.Product.Property.PropertyValue propertyValue)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE, propertyValue.PropertyValueId), propertyValue);
        }

        public static Service.Product.Property.PropertyValue GetPropertyValue(int propertyValueId)
        {
            return MemcachedHelper.Instance.Get<Service.Product.Property.PropertyValue>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE, propertyValueId));
        }

        public static bool ClearPropertyValue(int propertyValueId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE, propertyValueId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetPropertyValueGroup(Service.Product.Property.PropertyValueGroup propertyValueGroup)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP, propertyValueGroup.GroupId), propertyValueGroup);
        }

        public static Service.Product.Property.PropertyValueGroup GetPropertyValueGroup(int groupId)
        {
            return MemcachedHelper.Instance.Get<Service.Product.Property.PropertyValueGroup>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP, groupId));
        }

        public static IList<Service.Product.Property.Property> GetAllProperties()
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.Property>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY));
        }

        public static IList<Service.Product.Property.Property> GetAllProperties(int langId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.Property>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY, langId));
        }

        public static bool SetAllProperties(IList<Service.Product.Property.Property> properties, int langId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY, langId), properties);
        }

        public static bool ClearAllProperties(int langId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY, langId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetAllPropertyValueGroupLanguages(IList<Service.Product.Property.PropertyValueGroup> propertyValueGroups, int langId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP_LANGUAGE, langId), propertyValueGroups);
        }

        public static IList<Service.Product.Property.PropertyValueGroup> GetAllPropertyValueGroupLanguages(int langId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValueGroup>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP_LANGUAGE, langId));
        }

        public static bool ClearAllPropertyValueGroupLanguages(int langId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP_LANGUAGE, langId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static IList<Service.Product.Property.PropertyValue> GetAllPropertyValues(int langId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValue>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY_VALUE, langId));
        }


        public static IList<Service.Product.Property.PropertyValueLanguage> GetAllPropertyValueLanguages(int langId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValueLanguage>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, langId));
        }

        public static bool SetAllPropertyValueLanguages(IList<Service.Product.Property.PropertyValueLanguage> propertyValueLanguages, int langId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, langId), propertyValueLanguages);
        }

        public static bool SetAllPropertyLanguages(IList<PropertyLanguage> propertyLanguage)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, MemcachedConst.KEY_ALL), propertyLanguage);
        }

        public static IList<PropertyLanguage> GetAllPropertyLanguages()
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyLanguage>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, MemcachedConst.KEY_ALL));
        }

        public static bool ClearGetAllPropertyLanguages()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetAllProperties(IList<Service.Product.Property.Property> properties)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY), properties);
        }

        public static bool SetAllPropertyValues(IList<Service.Product.Property.PropertyValue> propertyValues, int langId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PROPERTY_VALUE, langId), propertyValues);
        }

        public static bool SetPropertyLanguagesById(int propertyId, IList<PropertyLanguage> propertyLanguage)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, propertyId), propertyLanguage);
        }

        public static IList<PropertyLanguage> GetPropertyLanguagesById(int propertyId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyLanguage>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, propertyId));
        }
        public static bool ClearPropertyLanguagesById(int propertyId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, propertyId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static IList<Service.Product.Property.Property> GetAllPropertiesOfCategory(int categoryId, int langId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.Property>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_CATEGORY, string.Format("{0}_{1}", categoryId, langId)));
        }

        public static bool SetAllPropertiesOfCategory(int categoryId, int langId, IList<Service.Product.Property.Property> properties)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_CATEGORY, string.Format("{0}_{1}", categoryId, langId)), properties);
        }

        public static bool ClearAllPropertiesOfCategory(int categoryId, int langId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_CATEGORY, string.Format("{0}_{1}", categoryId, langId));
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetAllPropertyValueGroupLanguages(IList<PropertyValueGroupLanguage> propertyValueGroupLanguage)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP_LANGUAGE, MemcachedConst.KEY_ALL), propertyValueGroupLanguage);
        }

        public static bool ClearAllPropertyValueGroupLanguages()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_LANGUAGE, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static IList<PropertyValueGroupLanguage> GetAllPropertyValueGroupLanguages()
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValueGroupLanguage>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_GROUP_LANGUAGE, MemcachedConst.KEY_ALL));
        }

        public static bool SetAllPropertyValueLanguagesById(int propertyValueId, IList<PropertyValueLanguage> propertyValueLanguage)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, propertyValueId), propertyValueLanguage);
        }

        public static IList<PropertyValueLanguage> GetAllPropertyValueLanguagesById(int propertyValueId)
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValueLanguage>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, propertyValueId));
        }
        public static bool ClearAllPropertyValueLanguagesById(int propertyValueId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, propertyValueId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        public static bool SetAllPropertyValueLanguages(IList<PropertyValueLanguage> propertyValueLanguage)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, MemcachedConst.KEY_ALL), propertyValueLanguage);
        }

        public static IList<PropertyValueLanguage> GetAllPropertyValueLanguages()
        {
            return MemcachedHelper.Instance.Get<IList<Service.Product.Property.PropertyValueLanguage>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PROPERTY_VALUE_LANGUAGE, MemcachedConst.KEY_ALL));
        }

        #endregion

        #region 相似商品
        /// <summary>
        /// 设置相似商品属性值Cache
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="similarPropertyValues">相似商品属性值列表</param>
        /// <returns></returns>
        public static bool SetProductSimilarPropertyValue(int productId, IList<int> similarPropertyValues)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_SIMILAR_PROPERTY_VALUE, productId), similarPropertyValues);
        }
        /// <summary>
        /// 从Cache获取相似商品属性值
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>相似商品属性值Id列表</returns>
        public static IList<int> GetProductSimilarPropertyValues(int productId)
        {
            return MemcachedHelper.Instance.Get<IList<int>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_SIMILAR_PROPERTY_VALUE, productId));
        }

        /// <summary>
        /// 设置产品是否有相似商品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="hasSimilarProduct">是否有相似商品</param>
        /// <returns></returns>
        public static bool SetProductHasSimilar(int productId, bool hasSimilarProduct)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_HAS_SIMILAR, productId), hasSimilarProduct);
        }

        /// <summary>
        /// 获取产品是否有相似商品
        /// </summary>
        /// <param name="productId">产品ID</param> 
        /// <returns></returns>
        public static bool? GetProductHasSimilar(int productId)
        {
            return MemcachedHelper.Instance.Get<bool?>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_HAS_SIMILAR, productId));
        }

        #endregion

        #region 产品类别

        /// <summary>
        /// 清空产品类别Cache
        /// </summary>
        /// <returns></returns>
        public static bool ClearCategoryCache()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 添加产品类别Cache
        /// </summary>
        /// <param name="categories">产品类别集合</param>
        /// <returns></returns>
        public static bool AddAllCategories(IList<Category> categories)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                ClearCategoryCache();
                return MemcachedHelper.Instance.Add(key, categories);
            }
            return MemcachedHelper.Instance.Add(key, categories);
        }
        /// <summary>
        /// 设置产品类别Cache
        /// </summary>
        /// <param name="categories">产品类别集合</param>
        /// <returns></returns>
        public static bool SetAllCategories(IList<Category> categories)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                var lstCacheAllCategories = GetAllCategories().ToList();
                if (!lstCacheAllCategories.IsNullOrEmpty())
                {
                    //筛选出缓存中没有产品的类别
                    var lstLeakCategories =
                        categories.Where(x => !lstCacheAllCategories.Exists(y => y.CategoryId == x.CategoryId)).ToList();
                    lstCacheAllCategories.AddRange(lstLeakCategories);
                }
                else
                {
                    lstCacheAllCategories = categories.ToList();
                }

                return MemcachedHelper.Instance.Add(key, lstCacheAllCategories);
            }
            return MemcachedHelper.Instance.Add(key, categories);
        }

        /// <summary>
        /// 从Cache获取所有产品类别
        /// </summary>
        /// <returns></returns>
        public static IList<Category> GetAllCategories()
        {
            return MemcachedHelper.Instance.Get<IList<Category>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY, MemcachedConst.KEY_ALL));
        }

        /// <summary>
        /// 设置所有后台菜单
        /// </summary>
        /// <param name="adminMenus"></param>
        /// <returns></returns>
        public static bool SetAllAdminMenus(IList<AdminMenu> adminMenus)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_ADMIN_MENUS, MemcachedConst.KEY_ALL);
            return MemcachedHelper.Instance.Add(key, adminMenus);
        }

        /// <summary>
        /// 获取所有后台菜单
        /// </summary>
        /// <returns></returns>
        public static IList<AdminMenu> GetAllAdminMenus()
        {
            return MemcachedHelper.Instance.Get<IList<AdminMenu>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_ADMIN_MENUS, MemcachedConst.KEY_ALL));
        }

        /// <summary>
        /// 清空后台菜单
        /// </summary>
        /// <returns></returns>
        public static bool ClearAllAdminMenus()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_ADMIN_MENUS, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 设置所有根类别
        /// </summary>
        /// <returns></returns>
        public static bool SetAllRootCategories(IList<Category> categories)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_ROOT, MemcachedConst.KEY_ALL), categories);
        }





        /// <summary>
        /// 根据语种获取所有类别列表
        /// </summary>
        public static IList<CategoryLanguage> GetAllCategoryLanguagesByLanguageId(int languageId)
        {
            return MemcachedHelper.Instance.Get<IList<CategoryLanguage>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_LANGUAGE, languageId));
        }

        /// <summary>
        /// 设置语种获取所有类别列表
        /// </summary>
        public static bool SetAllCategoryLanguagesByLanguageId(IList<CategoryLanguage> categoryLanguages, int languageId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_LANGUAGE, languageId), categoryLanguages);
        }


        /// <summary>
        /// 获取所有根类别
        /// </summary>
        /// <returns></returns>
        public static IList<Category> GetAllRootCategories()
        {
            return MemcachedHelper.Instance.Get<IList<Category>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_ROOT, MemcachedConst.KEY_ALL));
        }

        /// <summary>
        /// 清空所有根类别
        /// </summary>
        /// <returns></returns>
        public static bool ClearAllRootCategories()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_ROOT, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 根据ID 获取子级类别
        /// </summary>
        public static IList<Category> GetAllSubCategories(int categoryId)
        {
            return MemcachedHelper.Instance.Get<IList<Category>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_SUB, categoryId));
        }

        /// <summary>
        /// 根据ID 获取子级类别
        /// </summary>
        public static bool SetAllSubCategories(IList<Category> categories, int categoryId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_SUB, categoryId), categories);
        }

        /// <summary>
        /// 清空子级类别缓存
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static bool ClearAllSubCategories(int categoryId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_SUB, categoryId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 从Cache获取单个产品类别
        /// </summary>
        /// <param name="categoryId">产品类别Id</param>
        /// <returns></returns>
        public static Category GetCategory(int categoryId)
        {
            var lstCacheAllCategories = GetAllCategories();
            if (!lstCacheAllCategories.IsNullOrEmpty())
            {
                return lstCacheAllCategories.FirstOrDefault(x => x.CategoryId == categoryId);
            }
            return null;
            //return MemcachedHelper.Instance.Get<Category>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, categoryId));
        }

        /// <summary>
        /// 设置产品类别树
        /// </summary>
        /// <param name="categoryTree">产品Id</param> 
        /// <returns></returns>
        public static bool SetCategoryTree(IList<RelatedData<Category>> categoryTree)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.KEY_CATEGORY_TREE, categoryTree);
        }

        /// <summary>
        /// 获取产品类别树
        /// </summary> 
        /// <returns></returns>
        public static IList<RelatedData<Category>> GetCategoryTree()
        {
            return MemcachedHelper.Instance.Get<IList<RelatedData<Category>>>(MemcachedConst.KEY_CATEGORY_TREE);
        }

        /// <summary>
        /// 根据ID 获取子级类别
        /// </summary>
        public static IList<RelatedData<Service.Product.Category.CategoryLanguage>> GetCategoryTreeRecursiveCache(int languageId)
        {
            return MemcachedHelper.Instance.Get<IList<RelatedData<Service.Product.Category.CategoryLanguage>>>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_TREE, languageId));
        }

        /// <summary>
        /// 根据ID 获取子级类别
        /// </summary>
        public static bool SetCategoryTreeRecursiveCache(IList<RelatedData<Service.Product.Category.CategoryLanguage>> categories, int languageId)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_TREE, languageId), categories);
        }

        /// <summary>
        /// 清空子级类别缓存
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public static bool ClearCategoryTreeRecursiveCache(int languageId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CATEGORY_TREE, languageId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }
        #endregion

        #region DailyDeal

        /// <summary>
        /// 清空DailyDeal标语库Cache
        /// </summary>
        /// <returns></returns>
        public static bool ClearDailyDealTitleCache(int languageId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_DAILYDEAL_TITLE,
                MemcachedConst.KEY_DAILYDEAL_TITLE_LANGUAGE, languageId);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 添加DailyDeal标语库Cache
        /// </summary>
        public static bool SetDailyDealTitles(int languageId, List<DailyDealTitle> dailyDealTitles)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_DAILYDEAL_TITLE,
                MemcachedConst.KEY_DAILYDEAL_TITLE_LANGUAGE, languageId);
            return MemcachedHelper.Instance.Add(key, dailyDealTitles);
        }

        /// <summary>
        /// 从Cache获取DailyDeal标语库
        /// </summary>
        /// <returns></returns>
        public static List<DailyDealTitle> GetDailyDealTitlesByLanguageId(int languageId)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_DAILYDEAL_TITLE,
                MemcachedConst.KEY_DAILYDEAL_TITLE_LANGUAGE, languageId);
            return MemcachedHelper.Instance.Get<List<DailyDealTitle>>(key);
        }
        #endregion

        #region 系统配置
        /// <summary>
        /// 存储系统配置到缓存
        /// </summary>
        /// <param name="key">配置的Key</param>
        /// <param name="value">配置的值</param>
        /// <returns></returns>
        public static bool SetSystemConfig(string key, object value)
        {
            return MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_SYSTEM_CONFIG, key), value);
        }

        /// <summary>
        /// 获取系统配置到缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSystemConfig<T>(string key)
        {
            return MemcachedHelper.Instance.Get<T>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT_CODE, key));
        }

        #endregion

        #region 币种 Currencies

        /// <summary>
        /// 设置网站币种
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public static bool SetAllValidCurrencies(List<Currency> currencies)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CURRENCY_VALID, MemcachedConst.KEY_CURRENCY);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                MemcachedHelper.Instance.Delete(key);
            }
            return MemcachedHelper.Instance.Add(key, currencies);
        }

        /// <summary>
        /// 得到网站币种
        /// </summary> 
        /// <returns></returns>
        public static List<Currency> GetAllValidCurrencies()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CURRENCY_VALID, MemcachedConst.KEY_CURRENCY);
            return MemcachedHelper.Instance.Get<List<Currency>>(key);
        }

        /// <summary>
        /// 清除网站币种
        /// </summary> 
        /// <returns></returns>
        public static bool ClearAllValidCurrencies()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CURRENCY_VALID, MemcachedConst.KEY_CURRENCY);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                return MemcachedHelper.Instance.Delete(key);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public static bool SetAllCurrencies(List<Currency> currencies)
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CURRENCY, MemcachedConst.KEY_ALL);
            if (MemcachedHelper.Instance.IsExists(key))
            {
                MemcachedHelper.Instance.Delete(key);
            }
            return MemcachedHelper.Instance.Add(key, currencies);
        }

        /// <summary>
        /// 
        /// </summary> 
        /// <returns></returns>
        public static List<Currency> GetAllCurrencies()
        {
            var key = MemcachedConst.ToObjectKey(MemcachedConst.KEY_CURRENCY_VALID, MemcachedConst.KEY_CURRENCY);
            return MemcachedHelper.Instance.Get<List<Currency>>(key);
        }
        #endregion

        public static bool ExistKey(string key)
        {
            return MemcachedHelper.Instance.IsExists(key);
        }
    }
}
