using System;
using System.Collections.Generic;
using Com.Panduo.Service.Product.Property;

namespace Com.Panduo.Service.Product.Category
{
    public interface ICategoryService
    {

        /// <summary>
        /// 获取关联关键字
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>关键词列表</returns>
        IList<CategoryKeyword> GetCategoryKeywords(int categoryId, int languageId);

        /// <summary>
        /// 设置关联关键字
        /// </summary>
        /// <param name="keyword">关联关键字实体</param>
        void SetCategoryKeyword(CategoryKeyword keyword);

        /// <summary>
        /// 通过类别Id删除对应关键字
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        void DeleteCategoryKeywordByCategoryId(int categoryId);

        /// <summary>
        /// 通过类别Id和语言Id删除对应关键字
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="langId">语言Id</param>
        void DeleteCategoryKeywordByCategoryIdAndLangId(int categoryId,int langId);

        /// <summary>
        /// 通过关键字Id删除关键字
        /// </summary>
        /// <param name="keywordId">关键字Id</param>
        void DeleteCategoryKeywordById(int keywordId);

        /// <summary>
        /// 获取类别广告
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>返回CategoryAdvertisement对象，没有则为null</returns>
        CategoryAdvertisement GetCategoryAdvertisement(int categoryId, int languageId);

        /// <summary>
        /// 设置类别广告（忽略ProductMarketingArea设置）
        /// </summary>
        /// <param name="advertisement">类别广告实体</param>
        void SetCategoryAdvertisement(CategoryAdvertisement advertisement);

        /// <summary>
        /// 设置类别产品营销内容
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <param name="productMarketingArea">产品类别营销内容</param>
        void SetCategoryProductMarketingArea(int categoryId, int languageId, string productMarketingArea);

        /// <summary>
        /// 获取类别产品营销内容
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <remarks>返回类别产品营销内容</remarks>
        string GetCategoryProductMarketingArea(int categoryId, int languageId);

        /// <summary>
        /// 设置类别基本信息
        /// </summary>
        /// <param name="category">类别实体</param>
        /// <returns></returns>
        void SetCategoryBaseInfo(Category category);

        /// <summary>
        /// 设置类别是否隐藏
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="isHidden">是否隐藏</param>
        /// <returns></returns>
        void SetCategoryHidden(int categoryId, bool isHidden);


        /// <summary>
        /// 通过类别ID获取类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>有则返回Category，没有返回null</returns>
        Category GetCategoryById(int categoryId);

        /// <summary>
        /// 通过类别ID获取当前类别的父类别，如果没有父级返回NULL
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>有则返回Category，没有返回null</returns>
        Category GetParentCategoryById(int categoryId);

        /// <summary>
        /// 通过类别ID，语种ID获取类别多语种信息
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>有则返回Category，没有返回null</returns>
        CategoryLanguage GetCategoryLanguageById(int categoryId, int languageId);

        /// <summary>
        /// 通过类别ID获取类别多语种信息列表
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        IList<CategoryLanguage> GetCategoryLanguageById(int categoryId);

        /// <summary>
        /// 批量修改类别所有语种信息
        /// </summary>
        /// <param name="categoryLanguageList">类别多语种列表</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_CATEGORY_NOT_EXIST:类别不存在</value>
        /// </exception>
        /// <returns></returns>
        void UpdateCategoryLanguages(IList<CategoryLanguage> categoryLanguageList);

        /// <summary>
        /// 批量修改类别营销所有语种信息
        /// </summary>
        /// <param name="categoryAdvertisementList">类别营销多语种列表</param>
        /// <returns></returns>
        void UpdateCategoryAdvertisementLanguages(IList<CategoryAdvertisement> categoryAdvertisementList);

        /// <summary>
        /// 批量修改类别属性信息
        /// </summary>
        /// <param name="categoryPropertyList">类别属性列表</param>
        /// <returns></returns>
        void UpdateCategoryProperties(IList<CategoryProperty> categoryPropertyList);

        /// <summary>
        /// 获取类别绑定的所有属性
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <returns>有则返回属性列表，没有返回空IList</returns>
        IList<CategoryProperty> GetCategoryBindedAllProperties(int categoryId);

        /// <summary>
        /// 获取类别及该类别所有子孙类别绑定的所有属性
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <returns>有则返回属性列表，没有返回空IList</returns>
        IList<CategoryProperty> GetCategoryBindedAllPropertiesRecursive(int categoryId);

        /// <summary>
        /// 获取所有一级类别
        /// </summary>
        /// <returns>有则返回根类别列表，没有返回空IList</returns>
        IList<Category> GetAllRootCategories();

        /// <summary>
        /// 根据类别Id获取子类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>有则返回子类别列表，没有返回空IList</returns>
        IList<Category> GetAllSubCategories(int categoryId);

        /// <summary>
        /// 获取所有状态的类别
        /// </summary>
        /// <returns>所有类别</returns>
        IList<Category> GetAllCategories();

        /// <summary>
        /// 获取所有语种的类别
        /// </summary>
        /// <returns>所有类别</returns>
        IList<CategoryLanguage> GetAllCategoryLanguages();

        /// <summary>
        /// 获取当前语种的所有类别
        /// </summary>
        /// <param name="languageId">语种ID</param>
        /// <returns>所有类别</returns>
        IList<CategoryLanguage> GetAllCategoryLanguagesByLanguageId(int languageId);

        /// <summary>
        /// 获取所有末级类别
        /// </summary>
        /// <returns>末级类别列表，没有返回空IList</returns>
        IList<Category> GetAllLeafCategories();

        /// <summary>
        /// 判断类别是否根类别
        /// 根类别的parent_id为0
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        bool IsRootCategory(int categoryId);

        /// <summary>
        /// 判断类别是否末级类别
        /// 查找类别表中是否有parent_id为传入的categoryId，没有则为末级类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        bool IsLeafCategory(int categoryId);
        
        /// <summary>
        /// 通过类别ID获取topn子类别，按照排序字段升序排序
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="n">top n</param>
        /// <returns>类别子实体列表，没有则返回null</returns>
        IList<Category> GetTopSubCategoriesById(int categoryId, int n);
        //PageData<Category> findCategories(int currentPage, int pageSize, Map<T filterCriteria, object Object>, List<Soter<T>> sorterCriteria)

        /// <summary>
        /// 递归获取类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，入股为NUll则从根类别开始获取</param>
        /// <returns></returns>
        IList<RelatedData<Service.Product.Category.Category>> GetCategoryTreeRecursive(int? parentCategoryId);

        /// <summary>
        /// 递归获取类别树(缓存了所有的类别)
        /// </summary>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        IList<RelatedData<Service.Product.Category.CategoryLanguage>> GetCategoryTreeRecursiveCache(int languageId);

        /// <summary>
        /// 递归获取类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，入股为NUll则从根类别开始获取</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        IList<RelatedData<Service.Product.Category.CategoryLanguage>> GetCategoryTreeRecursive(int? parentCategoryId, int languageId);

        /// <summary>
        /// 加载所有类别属性到缓存
        /// </summary>
        void LoadCategoryProperties();

        /// <summary>
        /// 获取上下级类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        List<CategoryLanguage> GetCategoryLanguageFamliy(int categoryId, int? languageId);
    }

}
