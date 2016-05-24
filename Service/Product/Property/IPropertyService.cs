using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Product.Property
{
    public interface IPropertyService
    {
        #region 属性
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <returns>返回属性对象，没有则返回null</returns>
        Property GetPropertyById(int propertyId);

        /// <summary>
        /// 通过属性ID,语种ID获取单个属性多语言
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>单个属性多语言</returns>
        PropertyLanguage GetPropertyLanguageById(int propertyId,int languageId);

        /// <summary>
        /// 通过属性ID获取所有属性多语言
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <returns>属性多语言列表</returns>
        IList<PropertyLanguage> GetPropertyLanguagesById(int propertyId);

        /// <summary>
        /// 根据类别ID获取所有属性
        /// </summary>
        /// <returns></returns>
        IList<Property> GetAllPropertiesByCategoryId(int categoryId);

        /// <summary>
        /// 根据产品ID获取所有属性
        /// </summary>
        /// <returns></returns>
        IList<Property> GetAllPropertiesByProductId(int productId);

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        IList<Property> GetAllProperties();

        /// <summary>
        /// 获取指定语种的所有属性
        /// </summary>
        /// <returns></returns>
        IList<Property> GetAllPropertiesOfLanguage(int languageId);

        /// <summary>
        /// 获取所有属性多语言
        /// </summary>
        /// <returns>所有属性多语言列表</returns>
        IList<PropertyLanguage> GetAllPropertyLanguages();

        /// <summary>
        /// 设置属性是否显示
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <param name="isDisplay">是否显示</param>
        void SetPropertyDisplay(int propertyId, bool isDisplay);

        /// <summary>
        /// 设置属性基本信息
        /// </summary>
        /// <param name="property">属性实体</param>
        void SetPropertyBaseInfo(Property property);
        
        /// <summary>
        /// 修改属性排序
        /// 只允许修改排序字段：DisplayOrder
        /// </summary>
        /// <param name="propertyOrderList">属性列表,key=属性ID,value=排序</param>
        void UpdatePropertyOrder(IList<KeyValuePair<int,int>> propertyOrderList);

        /// <summary>
        /// 批量修改属性所有语种信息
        /// </summary>
        /// <param name="propertyLanguageList">属性多语种列表</param>
        /// <returns></returns>
        void UpdatePropertyLanguages(IList<PropertyLanguage> propertyLanguageList);

        /// <summary>
        /// 批量修改属性值所有语种信息
        /// </summary>
        /// <param name="propertyValueLanguageList">属性值多语种列表</param>
        /// <returns></returns>
        void UpdatePropertyValueLanguages(IList<PropertyValueLanguage> propertyValueLanguageList);
        
        #endregion

        #region 属性值组
        /// <summary>
        /// 根据属性ID获取属性值组
        /// 1、先根据属性ID到属性值表t_property_value获取所有属性值组ID->property_value_group_id
        /// 2、然后排除重复的property_value_group_id
        /// 3、最后根据property_value_group_id去t_property_value_group表中得到所有属性值组的信息
        /// 备注：属性和属性值组是没有直接关系的，一个属性值组可以同时属于多个属性
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <returns>返回属性值组列表，没有则返回空List</returns>
        IList<PropertyValueGroup> GetAllPropertyValueGroupsOfProperty(int propertyId);


        /// <summary>
        /// 获取所有的属性值组
        /// </summary>
        /// <returns>属性值组列表</returns>
        IList<PropertyValueGroup> GetAllPropertyValueGroups();

        /// <summary>
        /// 获取指定语种的所有的属性值组
        /// </summary>
        /// <returns>属性值组列表</returns>
        IList<PropertyValueGroup> GetAllPropertyValueGroupLanguages(int languageId);

        /// <summary>
        /// 获取所有的属性值组多语言
        /// </summary>
        /// <returns>属性值组多语言列表</returns>
        IList<PropertyValueGroupLanguage> GetAllPropertyValueGroupLanguages();

        /// <summary>
        /// 通过属性值组Id获取所有的属性值组多语言
        /// </summary>
        /// <param name="propertyValueGroupId">属性值组Id</param>
        /// <returns>属性值组多语言列表</returns>
        IList<PropertyValueGroupLanguage> GetPropertyValueGroupLanguages(int propertyValueGroupId);

        /// <summary>
        /// 获取属性值组多语言
        /// </summary>
        /// <param name="propertyValueGroupId">属性值组Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>PropertyValueGroupLanguage</returns>
        PropertyValueGroupLanguage GetPropertyValueGroupLanguage(int propertyValueGroupId, int languageId);


        /// <summary>
        /// 通过属性值组ID获取属性值组
        /// </summary>
        /// <param name="propertyValueGroupId">属性值组Id</param>
        /// <returns></returns>
        PropertyValueGroup GetPropertyValueGroupById(int propertyValueGroupId);

        /// <summary>
        /// 直接查询数据库得到属性数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="keyWrod">关键词</param>
        /// <returns></returns>
        PageData<Service.Product.Property.Property> FindPropertiesForAdminList(int currentPage, int pageSize,
            string keyWrod);

        #endregion

        #region 属性值

        /// <summary>
        /// 根据属性ID获取属性值
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <returns>返回属性值列表，没有则返回空IList</returns>
        IList<PropertyValue> GetAllPropertyValuesOfProperty(int propertyId);


        /// <summary>
        /// 获取所有属性值
        /// </summary>
        /// <returns>返回属性值列表，没有则返回空IList</returns>
        IList<PropertyValue> GetAllPropertyValues();

        /// <summary>
        /// 获取指定语种的所有属性值
        /// </summary>
        /// <returns>返回属性值列表，没有则返回空IList</returns>
        IList<PropertyValue> GetAllPropertyValuesOfLanguage(int languageId);

        /// <summary>
        /// 获取当前语种所有的属性值列表
        /// </summary>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        IList<PropertyValueLanguage> GetAllPropertyValueLanguages(int languageId);
        
        /// <summary>
        /// 根据属性值组ID获取属性值
        /// </summary>
        /// <param name="propertyValueGroupId">属性值组ID</param>
        /// <returns>返回属性值列表，没有则返回空IList</returns>
        IList<PropertyValue> GetAllPropertyValuesOfPropertyValueGroup(int propertyValueGroupId);

        /// <summary>
        /// 通过属性ID获取属性值
        /// </summary>
        /// <param name="propertyValueId">属性值ID</param>
        /// <returns>返回属性值对象，没有则返回null</returns>
        PropertyValue GetPropertyValue(int propertyValueId);

        /// <summary>
        /// 获取所有属性值多语种
        /// </summary>
        /// <returns>属性值多语种列表</returns>
        IList<PropertyValueLanguage> GetAllPropertyValueLanguages();

        /// <summary>
        /// 通过属性值ID获取所有属性值多语种
        /// </summary>
        /// <returns>属性值多语种列表</returns>
        IList<PropertyValueLanguage> GetAllPropertyValueLanguagesById(int propertyValueId);


        /// <summary>
        /// 属性值多语种实体
        /// </summary>
        /// <param name="propertyValueId">属性值ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        PropertyValueLanguage GetPropertyValueLanguage(int propertyValueId, int languageId);

        /// <summary>
        /// 直接查询数据库得到属性值数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="keyWrod">关键词</param>
        /// <returns></returns>
        PageData<Service.Product.Property.PropertyValue> FindPropertyValuesForAdminList(int propertyId, int currentPage, int pageSize,
            string keyWrod);


        /// <summary>
        /// 修改属性值排序
        /// 只允许修改排序字段：DisplayOrder
        /// </summary>
        /// <param name="propertyValueOrderList">属性值列表,key=属性值ID,value=排序</param>
        void UpdatePropertyValueOrder(IList<KeyValuePair<int, int>> propertyValueOrderList);

        /// <summary>
        /// 设置属性值基本信息
        /// </summary>
        /// <param name="propertyValue">属性值实体</param>
        void SetPropertyValueBaseInfo(PropertyValue propertyValue);

        #endregion
    }
}
