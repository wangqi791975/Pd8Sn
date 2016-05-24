using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Category;
using Com.Panduo.Entity.Product.Property;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.ServiceImpl.Product.Category.Dao;
using Com.Panduo.ServiceImpl.Product.Property.Dao;
using Memcached.ClientLibrary;

namespace Com.Panduo.ServiceImpl.Product.Property
{
    class PropertyService : IPropertyService
    {
        #region IOC注入
        public IPropertyDao PropertyDao { private get; set; }
        public IPropertyDescDao PropertyDescDao { private get; set; }
        public IPropertyValueGroupDao PropertyValueGroupDao { private get; set; }
        public IPropertyValueGroupDescDao PropertyValueGroupDescDao { private get; set; }
        public IPropertyValueDao PropertyValueDao { private get; set; }
        public IPropertyValueDescDao PropertyValueDescDao { private get; set; }

        public ICategoryPropertyDao CategoryPropertyDao { private get; set; }
        #endregion

        #region 属性
        public Service.Product.Property.Property GetPropertyById(int propertyId)
        {
            var property = GetPropertyFromCache((propertyId));
            return property;
        }

        public PropertyLanguage GetPropertyLanguageById(int propertyId, int languageId)
        {
            var propertyLanguages = GetPropertyLanguagesById(propertyId);
            if (!propertyLanguages.IsNullOrEmpty())
            {
                var list = propertyLanguages.Where(x => x.LanguageId == languageId).ToList();
                if (!list.IsNullOrEmpty())
                {
                    return list[0];
                }
            }
            var propertyLanguage = PropertyDescDao.GetPropertyDescByPropertyIdAndLanguageId(propertyId, languageId);
            if (!propertyLanguage.IsNullOrEmpty())
            {
                return new PropertyLanguage
                {
                    PropertyId = propertyId,
                    PropertyName = propertyLanguage.Name,
                    LanguageId = languageId
                };
            }
            return null;

        }

        public IList<PropertyLanguage> GetPropertyLanguagesById(int propertyId)
        {
            var propertyLanguages = ImplCacheHelper.GetPropertyLanguagesById(propertyId);
            if (propertyLanguages.IsNullOrEmpty())
            {
                var list = PropertyDescDao.FindObjectByHql("from PropertyDescPo where PropertyId = ?", new object[] { propertyId });
                List<PropertyLanguage> propertyLanguage = new List<PropertyLanguage>();
                if (!list.IsNullOrEmpty())
                {
                    propertyLanguages = list.Select(x => GetPropertyLanguageFromPo((PropertyDescPo)x)).ToList();
                    ImplCacheHelper.SetPropertyLanguagesById(propertyId, propertyLanguages);
                }
            }
            
            return propertyLanguages;

        }

        public IList<Service.Product.Property.Property> GetAllPropertiesByCategoryId(int categoryId)
        {
            var categoryPropertyList = new List<Service.Product.Property.Property>();
            IList<CategoryPropertyPo> categoryPropertyPoList = CategoryPropertyDao.GetCategoryBindedAllProperties(categoryId);
            foreach (var categoryPropertyPo in categoryPropertyPoList)
            {
                Service.Product.Property.Property property = new Service.Product.Property.Property();
                property = GetPropertyFromPo(PropertyDao.GetObject(categoryPropertyPo.PropertyId));
                //取出Po里的数据重新给Property里的这两项赋值，切记
                property.IsDisplay = categoryPropertyPo.IsShow;
                property.DisplayOrder = categoryPropertyPo.SortOrder;
                categoryPropertyList.Add(property);
            }
            return categoryPropertyList;
        }

        public IList<Service.Product.Property.Property> GetAllPropertiesByProductId(int productId)
        {
            var productPropertyList = new List<Service.Product.Property.Property>();
            /*
            IList<CategoryPropertyPo> categoryPropertyPoList = CategoryPropertyDao.GetCategoryBindedAllProperties(productId);
            foreach (var categoryPropertyPo in categoryPropertyPoList)
            {
                productPropertyList.Add(GetPropertyFromPo(PropertyDao.GetObject(categoryPropertyPo.PropertyId)));
            }
            */
            IList<ProductPropertyValue> productPropertyValues =
                ServiceFactory.ProductService.GetProductBindedAllPropertyValues(productId);
            foreach (var productPropertyValue in productPropertyValues)
            {
                productPropertyList.Add(GetPropertyFromPo(PropertyDao.GetObject(productPropertyValue.PropertyId)));
            }
            return productPropertyList;
        }

        public IList<Service.Product.Property.Property> GetAllProperties()
        {
            var propertyList = new List<Service.Product.Property.Property>();
            var propertyPoList = PropertyDao.GetAll();
            if (!propertyPoList.IsNullOrEmpty())
            {
                propertyList = propertyPoList.Select(x => GetPropertyFromPo(x)).ToList();
            }
            return propertyList;
        }
        
        public IList<Service.Product.Property.Property> GetAllPropertiesOfLanguage(int languageId)
        {
            var list = ImplCacheHelper.GetAllProperties(languageId);
            if (list.IsNullOrEmpty())
            {
                list = GetAllProperties();
                foreach (var item in list)
                {
                    var itemLanguage = GetPropertyLanguageById(item.PropertyId, languageId);
                    item.PropertyName = itemLanguage == null ? item.PropertyName : itemLanguage.PropertyName;
                }

                ImplCacheHelper.SetAllProperties(list, languageId);
            }
            return list;
        }

        public IList<PropertyLanguage> GetAllPropertyLanguages()
        {
            var propertyLanguages = ImplCacheHelper.GetAllPropertyLanguages();
            if (propertyLanguages.IsNullOrEmpty())
            {
                var list = PropertyDescDao.GetAll();
                if (!list.IsNullOrEmpty())
                {
                    propertyLanguages = list.Select(x => GetPropertyLanguageFromPo(x)).ToList();
                    ImplCacheHelper.SetAllPropertyLanguages(propertyLanguages);
                }
            }
            return propertyLanguages;
        }

        public void SetPropertyDisplay(int propertyId, bool isDisplay)
        {
            //var propertyDao = GetPropertyPoFromVo(GetPropertyFromCache(propertyId));
            var propertyDao = PropertyDao.GetObject(propertyId);
            propertyDao.IsShow = isDisplay;
            PropertyDao.UpdateObject(propertyDao);
        }

        public void SetPropertyBaseInfo(Service.Product.Property.Property property)
        {
            var propertyDao = PropertyDao.GetObject(property.PropertyId);
            propertyDao.ChineseName = property.PropertyName;
            propertyDao.IsShow = property.IsDisplay;
            propertyDao.Status = property.IsValid;
            //propertyDao.IsBasic = property.IsBasicProperty;
            propertyDao.SortOrder = property.DisplayOrder;
            propertyDao.SortType = Convert.ToInt32(property.SortType);
            propertyDao.IsShow = property.IsDisplay;
            PropertyDao.UpdateObject(propertyDao);
        }

        public void UpdatePropertyOrder(IList<KeyValuePair<int, int>> propertyOrderList)
        {
            foreach (var keyValuePair in propertyOrderList)
            {
                //var propertyDao = GetPropertyPoFromVo(GetPropertyFromCache(keyValuePair.Key));
                var propertyDao = PropertyDao.GetObject(keyValuePair.Key);
                if (!propertyDao.IsNullOrEmpty())
                {
                    propertyDao.SortOrder = keyValuePair.Value;
                    PropertyDao.UpdateObject(propertyDao);
                }
            }
        }

        public void UpdatePropertyLanguages(IList<PropertyLanguage> propertyLanguageList)
        {
            var listUpdate = new List<PropertyDescPo>();
            var listAdd = new List<PropertyDescPo>();
            foreach (var vo in propertyLanguageList)
            {
                var po = PropertyDescDao.GetPropertyDescByPropertyIdAndLanguageId(vo.PropertyId, vo.LanguageId);
                if (!po.IsNullOrEmpty())
                {
                    po.Name = vo.PropertyName ?? po.Name;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.PropertyName.IsNullOrEmpty())
                    {
                        po = new PropertyDescPo
                        {
                            PropertyId = vo.PropertyId,
                            LanguageId = vo.LanguageId,
                            Name = vo.PropertyName
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                PropertyDescDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                PropertyDescDao.AddObjects(listAdd);
            }
        }

        public void UpdatePropertyValueLanguages(IList<PropertyValueLanguage> propertyValueLanguageList)
        {
            var listUpdate = new List<PropertyValueDescPo>();
            var listAdd = new List<PropertyValueDescPo>();
            foreach (var vo in propertyValueLanguageList)
            {
                var po = PropertyValueDescDao.GetPropertyDescPo(vo.PropertyValueId, vo.LanguageId);
                if (!po.IsNullOrEmpty())
                {
                    po.Name = vo.PropertyValueName ?? po.Name;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.PropertyValueName.IsNullOrEmpty())
                    {
                        po = new PropertyValueDescPo
                        {
                            PropertyValueId = vo.PropertyValueId,
                            LanguageId = vo.LanguageId,
                            Name = vo.PropertyValueName
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                PropertyValueDescDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                PropertyValueDescDao.AddObjects(listAdd);
            }
        }

        public PageData<Service.Product.Property.Property> FindPropertiesForAdminList(int currentPage, int pageSize, string keyWrod)
        {
            var list = PropertyDao.FindPropertiesForAdminList(currentPage, pageSize, keyWrod);

            PageData<Service.Product.Property.Property> pageData = new PageData<Service.Product.Property.Property>();
            pageData.Data = list.Data.Select(x => GetPropertyFromPo((PropertyPo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        #endregion

        #region 属性值组

        public IList<PropertyValueGroup> GetAllPropertyValueGroupsOfProperty(int propertyId)
        {
            var list = PropertyValueGroupDao.GetPropertyValueGroupByProertytyId(propertyId);
            var propertyValueGroup = new List<PropertyValueGroup>();
            if (!list.IsNullOrEmpty())
            {
                propertyValueGroup = list.Select(x => GetPropertyValueGroupFromPo(x)).ToList();
            }
            return propertyValueGroup;
        }

        public IList<PropertyValueGroup> GetAllPropertyValueGroups()
        {
            var list = PropertyValueGroupDao.GetAll();
            List<PropertyValueGroup> propertyValueGroup = new List<PropertyValueGroup>();
            if (!list.IsNullOrEmpty())
            {
                propertyValueGroup = list.Select(x => GetPropertyValueGroupFromPo(x)).ToList();
            }
            return propertyValueGroup;
        }
         
        public IList<PropertyValueGroup> GetAllPropertyValueGroupLanguages(int languageId)
        {
            var list = ImplCacheHelper.GetAllPropertyValueGroupLanguages(languageId);
            if (list.IsNullOrEmpty())
            {
                list = GetAllPropertyValueGroups();
                foreach (var item in list)
                {
                    var itemLanguage = GetPropertyValueGroupLanguage(item.GroupId, languageId);
                    item.PropertyValueGroupName = itemLanguage == null ? item.PropertyValueGroupName : itemLanguage.PropertyValueGroupName;
                }

                ImplCacheHelper.SetAllPropertyValueGroupLanguages(list, languageId);
            }
            return list;
        }

        public IList<PropertyValueGroupLanguage> GetAllPropertyValueGroupLanguages()
        {
            var propertyValueGroupLanguages = ImplCacheHelper.GetAllPropertyValueGroupLanguages();
            if (propertyValueGroupLanguages.IsNullOrEmpty())
            {
                var list = PropertyValueGroupDescDao.GetAll();
                if (!list.IsNullOrEmpty())
                {
                    propertyValueGroupLanguages = list.Select(x => GetPropertyValueGroupLanguageFromPo(x)).ToList();
                    ImplCacheHelper.SetAllPropertyValueGroupLanguages(propertyValueGroupLanguages);
                }
            }

            return propertyValueGroupLanguages;
        }

        public IList<PropertyValueGroupLanguage> GetPropertyValueGroupLanguages(int groupId)
        {
            var list = new List<PropertyValueGroupLanguage>();
            var propertyValueGroupLanguages = GetAllPropertyValueGroupLanguages();
            if (!propertyValueGroupLanguages.IsNullOrEmpty())
            {
                list =
                    propertyValueGroupLanguages.Where(
                        x => x.GroupId == groupId).ToList();
                if (!list.IsNullOrEmpty())
                {
                    return list;
                }
            }
            return list;
        }

        public PropertyValueGroupLanguage GetPropertyValueGroupLanguage(int propertyValueGroupId, int languageId)
        {
            var propertyValueGroupLanguages = GetAllPropertyValueGroupLanguages();
            if (!propertyValueGroupLanguages.IsNullOrEmpty())
            {
                var list =
                    propertyValueGroupLanguages.Where(
                        x => x.GroupId == propertyValueGroupId && x.LanguageId == languageId).ToList();
                if (!list.IsNullOrEmpty())
                {
                    return list[0];
                }
            }
            return null;
        }

        public PropertyValueGroup GetPropertyValueGroupById(int propertyValueGroupId)
        {
            return GetPropertyValueGroupFromCache(propertyValueGroupId);
        } 
        #endregion

        #region 属性值
        public IList<PropertyValue> GetAllPropertyValuesOfProperty(int propertyId)
        {
            var list = new List<PropertyValue>();
            var propertyValues = GetAllPropertyValues();
            if (!propertyValues.IsNullOrEmpty())
            {
                list = propertyValues.Where(x => x.PropertyId == propertyId).ToList();
                if (!list.IsNullOrEmpty())
                {
                    return list;
                }
            }
            return list;
        }

        public IList<PropertyValue> GetAllPropertyValues()
        {
            var propertyValues = ImplCacheHelper.GetAllPropertyValues();
            if (propertyValues.IsNullOrEmpty())
            {
                var list = PropertyValueDao.GetAll();
                if (!list.IsNullOrEmpty())
                {
                    propertyValues = list.Select(x => GetPropertyValueFromPo((PropertyValuePo)x)).ToList();
                    ImplCacheHelper.SetAllPropertyValues(propertyValues);
                }
            }
            return propertyValues;
        }

        public IList<PropertyValue> GetAllPropertyValuesOfLanguage(int languageId)
        {
            var list = ImplCacheHelper.GetAllPropertyValues(languageId);
            if (list.IsNullOrEmpty())
            {
                list = GetAllPropertyValues();
                foreach (var item in list)
                {
                    var itemLanguage = GetPropertyValueLanguage(item.PropertyValueId, languageId);
                    item.PropertyValueName = itemLanguage == null ? item.PropertyValueName : itemLanguage.PropertyValueName;
                }

                ImplCacheHelper.SetAllPropertyValues(list, languageId);
            }
            return list;
        }

        public IList<PropertyValueLanguage> GetAllPropertyValueLanguages(int languageId)
        {
            var list = ImplCacheHelper.GetAllPropertyValueLanguages(languageId);
            if (list.IsNullOrEmpty())
            {
                list = PropertyValueDescDao.GetPropertyDescPos(languageId).Select(x => GetPropertyValueLanguageFromPo((PropertyValueDescPo)x)).ToList();
                ImplCacheHelper.SetAllPropertyValueLanguages(list, languageId);
            }
            return list;
        }


        public IList<PropertyValue> GetAllPropertyValuesOfPropertyValueGroup(int propertyValueGroupId)
        {
            var list = new List<PropertyValue>();
            var propertyValues = GetAllPropertyValues();
            if (!propertyValues.IsNullOrEmpty())
            {
                list = propertyValues.Where(x => x.PropertyValueGroupId == propertyValueGroupId).ToList();
                if (!list.IsNullOrEmpty())
                {
                    return list;
                }
            }
            return list;
        }

        public PropertyValue GetPropertyValue(int propertyValueId)
        {
            return GetPropertyValueFromCache(propertyValueId);
        }

        public IList<PropertyValueLanguage> GetAllPropertyValueLanguages()
        {
            var propertyValueLanguages = ImplCacheHelper.GetAllPropertyValueLanguages();
            if (propertyValueLanguages.IsNullOrEmpty())
            {
                var list = PropertyValueDescDao.GetAll();
                if (!list.IsNullOrEmpty())
                {
                    propertyValueLanguages = list.Select(x => GetPropertyValueLanguageFromPo(x)).ToList();
                    ImplCacheHelper.SetAllPropertyValueLanguages(propertyValueLanguages);
                }
            }
            
            return propertyValueLanguages;
        }

        public IList<PropertyValueLanguage> GetAllPropertyValueLanguagesById(int propertyValueId)
        {
            var propertyValueLanguages = ImplCacheHelper.GetAllPropertyValueLanguagesById(propertyValueId);
            if (propertyValueLanguages.IsNullOrEmpty())
            {
                var list = PropertyValueDescDao.FindObjectByHql("from PropertyValueDescPo where PropertyValueId = ?", new object[] { propertyValueId });
                List<PropertyValueLanguage> propertyValueLanguage = new List<PropertyValueLanguage>();
                if (!list.IsNullOrEmpty())
                {
                    propertyValueLanguages = list.Select(x => GetPropertyValueLanguageFromPo((PropertyValueDescPo)x)).ToList();
                    ImplCacheHelper.SetAllPropertyValueLanguagesById(propertyValueId, propertyValueLanguages);
                }
            }
            return propertyValueLanguages;
        }

        public PropertyValueLanguage GetPropertyValueLanguage(int propertyValueId, int languageId)
        {
            var propertyValueLanguages = GetAllPropertyValueLanguagesById(propertyValueId);
            if (!propertyValueLanguages.IsNullOrEmpty())
            {
                var list = propertyValueLanguages.FirstOrDefault(x => x.LanguageId == languageId);
                if (!list.IsNullOrEmpty())
                {
                    return list;
                }
            }
            return null;
        }

        public PageData<Service.Product.Property.PropertyValue> FindPropertyValuesForAdminList(int propertyId, int currentPage, int pageSize,
            string keyWrod)
        {
            var list = PropertyValueDao.FindPropertyValuesForAdminList(propertyId, currentPage, pageSize, keyWrod);

            PageData<Service.Product.Property.PropertyValue> pageData = new PageData<Service.Product.Property.PropertyValue>();
            pageData.Data = list.Data.Select(x => GetPropertyValueFromPo((PropertyValuePo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        public void UpdatePropertyValueOrder(IList<KeyValuePair<int, int>> propertyValueOrderList)
        {
            foreach (var keyValuePair in propertyValueOrderList)
            {
                var propertyValue = PropertyValueDao.GetObject(keyValuePair.Key);
                if (!propertyValue.IsNullOrEmpty())
                {
                    propertyValue.SortOrder = keyValuePair.Value;
                    PropertyValueDao.UpdateObject(propertyValue);
                }
            }
        }

        public void SetPropertyValueBaseInfo(PropertyValue propertyValue)
        {
            var propertyValueDao = PropertyValueDao.GetObject(propertyValue.PropertyValueId);
            propertyValueDao.ChineseName = propertyValue.PropertyValueName;
            propertyValueDao.SortOrder = propertyValue.DisplayOrder;
            propertyValueDao.Status = propertyValue.IsValid;

            PropertyValueDao.UpdateObject(propertyValueDao);
        }

        #endregion

        #region 辅助方法
        /// <summary>
        /// 属性PO转换为VO
        /// </summary>
        /// <param name="propertyPo">PO</param>
        /// <returns>Property实体</returns>
        internal static Service.Product.Property.Property GetPropertyFromPo(PropertyPo propertyPo)
        {
            Service.Product.Property.Property property = null;
            if (!propertyPo.IsNullOrEmpty())
            {
                property = new Service.Product.Property.Property
                {
                    PropertyId = propertyPo.PropertyId,
                    PropertyName = propertyPo.ChineseName,
                    PropertyCode = propertyPo.Code,
                    DisplayOrder = propertyPo.SortOrder,
                    SortType = EnumHelper.ToEnum<PropertyValueSortType>(Convert.ToInt32(propertyPo.SortType)),
                    IsBasicProperty = propertyPo.IsBasic,
                    IsDisplay = propertyPo.IsShow,
                    IsValid = propertyPo.Status
                    
                };
            }
            return property;
        }

        /// <summary>
        /// 属性VO转换为PO
        /// </summary>
        /// <param name="propertyVo">VO</param>
        /// <returns>Property实体</returns>
        internal static Entity.Product.Property.PropertyPo GetPropertyPoFromVo(Service.Product.Property.Property propertyVo)
        {
            Entity.Product.Property.PropertyPo property = null;
            if (!propertyVo.IsNullOrEmpty())
            {
                property = new Entity.Product.Property.PropertyPo
                {
                    PropertyId = propertyVo.PropertyId,
                    ChineseName = propertyVo.PropertyName,
                    Code = propertyVo.PropertyCode,
                    SortOrder = propertyVo.DisplayOrder,
                    IsBasic = propertyVo.IsBasicProperty,
                    IsShow = propertyVo.IsDisplay,
                    Status = propertyVo.IsValid

                };
            }
            return property;
        }

        /// <summary>
        /// 属性多语种PO转换成PropertyLanguage
        /// </summary>
        /// <param name="propertyDescPo">PO</param>
        /// <returns>PropertyLanguage实体</returns>
        internal static PropertyLanguage GetPropertyLanguageFromPo(PropertyDescPo propertyDescPo)
        {
            PropertyLanguage propertyLanguage = null;
            if (!propertyDescPo.IsNullOrEmpty())
            {
                propertyLanguage = new Service.Product.Property.PropertyLanguage
                {
                    PropertyId = propertyDescPo.PropertyId,
                    LanguageId = propertyDescPo.LanguageId,
                    PropertyName = propertyDescPo.Name

                };
            }
            return propertyLanguage;
        }

        /// <summary>
        /// 属性值组PO转换成PropertyValueGroup
        /// </summary>
        /// <returns>PropertyValueGroup</returns>
        internal static PropertyValueGroup GetPropertyValueGroupFromPo(PropertyValueGroupPo propertyValueGroupPo)
        {
            PropertyValueGroup propertyValueGroup = null;
            if (!propertyValueGroupPo.IsNullOrEmpty())
            {
                propertyValueGroup = new Service.Product.Property.PropertyValueGroup
                {
                    PropertyValueId = propertyValueGroupPo.PropertyValueGroupId,
                    GroupId = propertyValueGroupPo.PropertyValueGroupId,
                    PropertyId = propertyValueGroupPo.PropertyId,
                    IsValid = propertyValueGroupPo.Status,
                    PropertyValueGroupName = propertyValueGroupPo.ChineseName,
                    PropertyValueGroupCode = propertyValueGroupPo.Code
                };
            }
            return propertyValueGroup;
        }

        internal static PropertyValueGroupLanguage GetPropertyValueGroupLanguageFromPo(PropertyValueGroupDescPo propertyValueGroupDescPo)
        {
            PropertyValueGroupLanguage propertyValueGroupLanguage = null;
            if (!propertyValueGroupDescPo.IsNullOrEmpty())
            {
                propertyValueGroupLanguage = new Service.Product.Property.PropertyValueGroupLanguage
                {
                    GroupId = propertyValueGroupDescPo.PropertyValueGroupId,
                    PropertyValueGroupName = propertyValueGroupDescPo.Name,
                    LanguageId = propertyValueGroupDescPo.LanguageId
                };
            }
            return propertyValueGroupLanguage;
        }

        internal static PropertyValue GetPropertyValueFromPo(PropertyValuePo propertyValuePo)
        {
            PropertyValue propertyValue = null;
            if (!propertyValuePo.IsNullOrEmpty())
            {
                propertyValue = new Service.Product.Property.PropertyValue
                {
                    PropertyValueId = propertyValuePo.PropertyValueId,
                    PropertyId = propertyValuePo.PropertyId,
                    PropertyValueGroupId = propertyValuePo.PropertyValueGroupId,
                    PropertyValueName = propertyValuePo.ChineseName,
                    DisplayOrder = propertyValuePo.SortOrder,
                    PropertyValueCode = propertyValuePo.Code,
                    IsValid = propertyValuePo.Status
                };
            }
            return propertyValue;
        }

        internal static PropertyValueLanguage GetPropertyValueLanguageFromPo(PropertyValueDescPo propertyValueDescPo)
        {
            PropertyValueLanguage propertyValueLanguage = null;
            if (!propertyValueDescPo.IsNullOrEmpty())
            {
                propertyValueLanguage = new Service.Product.Property.PropertyValueLanguage
                {
                    PropertyValueId = propertyValueDescPo.PropertyValueId,
                    PropertyValueName = propertyValueDescPo.Name,
                    LanguageId = propertyValueDescPo.LanguageId
                };
            }
            return propertyValueLanguage;
        }

        /// <summary>
        /// 从memcache中得到属性信息
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <returns>属性VO</returns>
        internal Service.Product.Property.Property GetPropertyFromCache(int propertyId)
        {
            var vo = ImplCacheHelper.GetProperty(propertyId);
            if (vo.IsNullOrEmpty())
            {
                var po = PropertyDao.GetObject(propertyId);

                if (!po.IsNullOrEmpty())
                {
                    vo = GetPropertyFromPo(po);
                    ImplCacheHelper.SetProperty(vo);
                }
            }
            return vo;
        }

        /// <summary>
        /// 从memcache中得到属性组信息
        /// </summary>
        /// <param name="propertyValueGroupId">属性值组ID</param>
        /// <returns>属性值组VO</returns>
        internal Service.Product.Property.PropertyValueGroup GetPropertyValueGroupFromCache(int propertyValueGroupId)
        {
            var vo = ImplCacheHelper.GetPropertyValueGroup(propertyValueGroupId);
            if (vo.IsNullOrEmpty())
            {
                var po = PropertyValueGroupDao.GetObject(propertyValueGroupId);
                if (!po.IsNullOrEmpty())
                {
                    vo = GetPropertyValueGroupFromPo(po);
                    ImplCacheHelper.SetPropertyValueGroup(vo);
                }
            }
            return vo;
        }

        /// <summary>
        /// 从memcache中得到属性值信息
        /// </summary>
        /// <param name="propertyValueId">属性值ID</param>
        /// <returns>属性值VO</returns>
        internal Service.Product.Property.PropertyValue GetPropertyValueFromCache(int propertyValueId)
        {
            var vo = ImplCacheHelper.GetPropertyValue(propertyValueId);
            if (vo.IsNullOrEmpty())
            {
                var po = PropertyValueDao.GetObject(propertyValueId);
                if (!po.IsNullOrEmpty())
                {
                    vo = GetPropertyValueFromPo(po);
                    ImplCacheHelper.SetPropertyValue(vo);
                }
            }
            return vo;
        }

        #endregion


    }
}
