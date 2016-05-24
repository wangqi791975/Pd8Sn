using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Category;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.ServiceImpl.Product.Category.Dao;

namespace Com.Panduo.ServiceImpl.Product.Category
{
    public class CategoryService : ICategoryService
    {
        public ICategoryDao CategoryDao { private get; set; }
        public ICategoryDescDao CategoryDescDao { private get; set; }
        public ICategoryKeywordDao CategoryKeywordDao { private get; set; }
        public ICategoryPropertyDao CategoryPropertyDao { private get; set; }
        public ICategoryAdDao CategoryAdDao { private get; set; }

        public IPropertyService PropertyService { get; set; }

        #region 业务异常
        /// <summary>
        /// 产品类别不存在
        /// </summary>
        public string ERROR_PRODUCT_CATEGORY_NOT_EXIST
        {
            get { return "ERROR_PRODUCT_CATEGORY_NOT_EXIST"; }
        }
        #endregion

        #region 类别关键字
        /// <summary>
        /// 获取关联关键字
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>关键词列表</returns>
        public IList<CategoryKeyword> GetCategoryKeywords(int categoryId, int languageId)
        {
            var lstCategoryKeywordPos = CategoryKeywordDao.GetCategoryKeywords(categoryId, languageId);
            if (lstCategoryKeywordPos.IsNullOrEmpty())
            {
                return new List<CategoryKeyword>();
            }
            var lstCategoryKeywords = lstCategoryKeywordPos.Select(x => new CategoryKeyword
            {
                Id = x.Id,
                Keyword = x.Keyword,
                Url = x.Link,
                LanguageId = x.LanguageId,
                CategoryId = x.CategoryId,
                DiplayOrder = x.SortIndex
            });
            return lstCategoryKeywords.ToList();
        }

        /// <summary>
        /// 设置关联关键字
        /// </summary>
        /// <param name="keyword">关联关键字实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_CATEGORY_NOT_EXIST:产品类别不存在</value>
        /// </exception>
        public void SetCategoryKeyword(CategoryKeyword keyword)
        {
            if (!CategoryDao.ExistObject("CategoryId", keyword.CategoryId))
                throw new BussinessException(ERROR_PRODUCT_CATEGORY_NOT_EXIST);

            var categoryKeywordPo = CategoryKeywordDao.GetCategoryKeyword(keyword.CategoryId, keyword.LanguageId, keyword.Keyword);
            //判断产品类别下关键字是否存在
            if (categoryKeywordPo.IsNullOrEmpty())
            {
                categoryKeywordPo = new CategoryKeywordPo
                {
                    CategoryId = keyword.CategoryId,
                    LanguageId = keyword.LanguageId,
                    Keyword = keyword.Keyword,
                    Link = keyword.Url,
                    SortIndex = keyword.DiplayOrder,
                };
                CategoryKeywordDao.AddObject(categoryKeywordPo);
            }
            else
            {
                categoryKeywordPo.LanguageId = keyword.LanguageId;
                categoryKeywordPo.Keyword = keyword.Keyword;
                categoryKeywordPo.Link = keyword.Url;
                categoryKeywordPo.SortIndex = keyword.DiplayOrder;
                CategoryKeywordDao.UpdateObject(categoryKeywordPo);
            }
        }

        public void DeleteCategoryKeywordByCategoryId(int categoryId)
        {
            CategoryKeywordDao.DeleteCategoryKeyword(categoryId);
        }

        public void DeleteCategoryKeywordByCategoryIdAndLangId(int categoryId, int langId)
        {
            CategoryKeywordDao.DeleteCategoryKeyword(categoryId, langId);
        }

        public void DeleteCategoryKeywordById(int keywordId)
        {
            CategoryKeywordDao.DeleteObjectById(keywordId);
        }

        #endregion

        #region 类别广告
        /// <summary>
        /// 获取类别广告
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>返回CategoryAdvertisement对象，没有则为null</returns>
        public CategoryAdvertisement GetCategoryAdvertisement(int categoryId, int languageId)
        {
            var categoryAdPo = CategoryAdDao.GetCategoryAdvertisement(categoryId, languageId);
            //判断产品是否存在
            if (categoryAdPo.IsNullOrEmpty())
            {
                var categoryPo = GetParentCategoryById(categoryId);
                if (!categoryPo.IsNullOrEmpty())
                {
                    var cAd2 = CategoryAdDao.GetCategoryAdvertisement(categoryPo.CategoryId, languageId);

                    if (cAd2.IsNullOrEmpty() && categoryPo.ParentId > 0)
                    {
                        var cAd1 = CategoryAdDao.GetCategoryAdvertisement(categoryPo.ParentId, languageId);
                        return CategoryAdvertisementPoConvertToVo(cAd1);
                    }
                    return CategoryAdvertisementPoConvertToVo(cAd2);
                }

            }
            return CategoryAdvertisementPoConvertToVo(categoryAdPo);
        }

        /// <summary>
        /// 设置类别广告（忽略ProductMarketingArea设置）
        /// </summary>
        /// <param name="advertisement">类别广告实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_CATEGORY_NOT_EXIST:产品类别不存在</value>
        /// </exception>
        public void SetCategoryAdvertisement(CategoryAdvertisement advertisement)
        {
            if (!CategoryDao.ExistObject("CategoryId", advertisement.CategoryId))
                throw new BussinessException(ERROR_PRODUCT_CATEGORY_NOT_EXIST);

            var categoryAdPo = CategoryAdDao.GetCategoryAdvertisement(advertisement.CategoryId, advertisement.LanguageId);
            //判断产品类别下关键字是否存在
            if (categoryAdPo.IsNullOrEmpty())
            {
                categoryAdPo = new CategoryAdPo
                {
                    CategoryId = advertisement.CategoryId,
                    LanguageId = advertisement.LanguageId,
                    AdImage = advertisement.AdvertisingImage,
                    AdWord = advertisement.AdvertisingWords,
                    MarketingText = advertisement.ProductMarketingArea,
                    Link = advertisement.Url
                };
                CategoryAdDao.AddObject(categoryAdPo);
            }
            else
            {
                //categoryAdPo.CategoryId = advertisement.CategoryId;
                //categoryAdPo.LanguageId = advertisement.LanguageId;
                categoryAdPo.AdImage = advertisement.AdvertisingImage;
                categoryAdPo.AdWord = advertisement.AdvertisingWords;
                categoryAdPo.MarketingText = advertisement.ProductMarketingArea;
                categoryAdPo.Link = advertisement.Url;
                CategoryAdDao.UpdateObject(categoryAdPo);
            }
        }

        /// <summary>
        /// 设置类别产品营销内容
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <param name="productMarketingArea">产品类别营销内容</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_CATEGORY_NOT_EXIST:产品类别不存在</value>
        /// </exception>
        public void SetCategoryProductMarketingArea(int categoryId, int languageId, string productMarketingArea)
        {
            if (!CategoryDao.ExistObject("CategoryId", categoryId))
                throw new BussinessException(ERROR_PRODUCT_CATEGORY_NOT_EXIST);

            var categoryAdPo = CategoryAdDao.GetCategoryAdvertisement(categoryId, languageId);
            //判断产品类别下关键字是否存在
            if (categoryAdPo.IsNullOrEmpty())
            {
                categoryAdPo = new CategoryAdPo
                {
                    CategoryId = categoryId,
                    LanguageId = languageId,
                    MarketingText = productMarketingArea
                };
                CategoryAdDao.AddObject(categoryAdPo);
            }
            else
            {
                categoryAdPo.MarketingText = productMarketingArea;
                CategoryAdDao.UpdateObject(categoryAdPo);
            }
        }

        /// <summary>
        /// 获取类别产品营销内容
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <remarks>返回类别产品营销内容</remarks>
        public string GetCategoryProductMarketingArea(int categoryId, int languageId)
        {
            var ad = string.Empty;
            var categoryAdPo = CategoryAdDao.GetCategoryAdvertisement(categoryId, languageId);

            ad = categoryAdPo.IsNullOrEmpty() ? string.Empty : categoryAdPo.MarketingText;//判断产品类别下关键字是否存在

            if (ad.IsNullOrEmpty())
            {
                var categoryPo = this.GetParentCategoryById(categoryId);

                if (!categoryPo.IsNullOrEmpty())
                {
                    var cAd2 = CategoryAdDao.GetCategoryAdvertisement(categoryPo.CategoryId, languageId);
                    ad = cAd2.IsNullOrEmpty() ? string.Empty : cAd2.MarketingText;//判断产品类别下关键字是否存在
                    if (ad.IsNullOrEmpty() && categoryPo.ParentId > 0)
                    {
                        var cAd1 = CategoryAdDao.GetCategoryAdvertisement(categoryPo.ParentId, languageId);
                        ad = cAd1.IsNullOrEmpty() ? string.Empty : cAd1.MarketingText;//判断产品类别下关键字是否存在
                    }
                }

            }
            return ad;
        }

        #endregion

        #region 产品类别

        #region 产品类别多语种
        /// <summary>
        /// 通过类别ID，语种ID获取类别多语种信息
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns>有则返回CategoryLanguage，没有返回null</returns>
        public CategoryLanguage GetCategoryLanguageById(int categoryId, int languageId)
        {
            var categoryLanguagePo = CategoryDescDao.GetCategoryLanguage(categoryId, languageId);
            return CategoryLanguagePoConvertToVo(categoryLanguagePo);
        }

        /// <summary>
        /// 通过类别ID获取类别多语种信息列表
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        public IList<CategoryLanguage> GetCategoryLanguageById(int categoryId)
        {
            var lstCategoryLanguagePos = CategoryDescDao.GetCategoryLanguagesByCategoryId(categoryId);
            return CategoryLanguagePoConvertToVoList(lstCategoryLanguagePos);
        }

        /// <summary>
        /// 批量修改类别所有语种信息
        /// </summary>
        /// <param name="categoryLanguageList">类别多语种列表</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_CATEGORY_NOT_EXIST:类别不存在</value>
        /// </exception>
        /// <returns></returns>
        public void UpdateCategoryLanguages(IList<CategoryLanguage> categoryLanguageList)
        {
            var listUpdate = new List<CategoryDescPo>();
            var listAdd = new List<CategoryDescPo>();
            foreach (var vo in categoryLanguageList)
            {
                var po = CategoryDescDao.GetCategoryLanguage(vo.CategoryId, vo.LanguageId);
                if (!po.IsNullOrEmpty())
                {
                    po.Name = vo.CategoryLanguageName != null ? vo.CategoryLanguageName : po.Name;
                    po.Description = vo.CategoryLanguageDescription != null ? vo.CategoryLanguageDescription : po.Description;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.CategoryLanguageName.IsNullOrEmpty())
                    {
                        po = new CategoryDescPo
                        {
                            CategoryId = vo.CategoryId,
                            LanguageId = vo.LanguageId,
                            Name = vo.CategoryLanguageName,
                            Description = vo.CategoryLanguageDescription
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                CategoryDescDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                CategoryDescDao.AddObjects(listAdd);
            }
        }

        /// <summary>
        /// 批量修改类别营销所有语种信息
        /// </summary>
        /// <param name="categoryAdvertisementList">类别营销多语种列表</param>
        /// <returns></returns>
        public void UpdateCategoryAdvertisementLanguages(IList<CategoryAdvertisement> categoryAdvertisementList)
        {
            var listUpdate = new List<CategoryAdPo>();
            var listAdd = new List<CategoryAdPo>();
            foreach (var vo in categoryAdvertisementList)
            {
                var po = CategoryAdDao.GetCategoryAdvertisement(vo.CategoryId, vo.LanguageId);
                if (!po.IsNullOrEmpty())
                {
                    po.CategoryId = vo.CategoryId != null ? vo.CategoryId : po.CategoryId;
                    po.LanguageId = vo.LanguageId != null ? vo.LanguageId : po.LanguageId;
                    po.AdImage = vo.AdvertisingImage != null ? vo.AdvertisingImage : po.AdImage;
                    po.AdWord = vo.AdvertisingWords != null ? vo.AdvertisingWords : po.AdWord;
                    po.Link = vo.Url != null ? vo.Url : po.Link;
                    po.MarketingTitle = vo.MarketingTitle != null ? vo.MarketingTitle : po.MarketingTitle;
                    po.MarketingText = vo.ProductMarketingArea != null ? vo.ProductMarketingArea : po.MarketingText;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.MarketingTitle.IsNullOrEmpty())
                    {
                        po = new CategoryAdPo
                        {
                            CategoryId = vo.CategoryId,
                            LanguageId = vo.LanguageId,
                            AdImage = vo.AdvertisingImage,
                            AdWord = vo.AdvertisingWords,
                            Link = vo.Url,
                            MarketingTitle = vo.MarketingTitle,
                            MarketingText = vo.ProductMarketingArea
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                CategoryAdDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                CategoryAdDao.AddObjects(listAdd);
            }
        }

        /// <summary>
        /// 批量修改类别属性信息
        /// </summary>
        /// <param name="categoryPropertyList">类别属性列表</param>
        /// <returns></returns>
        public void UpdateCategoryProperties(IList<CategoryProperty> categoryPropertyList)
        {
            var listUpdate = new List<CategoryPropertyPo>();
            foreach (var vo in categoryPropertyList)
            {
                var po = CategoryPropertyDao.GetCategoryProperty(vo.CategoryId, vo.PropertyId);
                if (!po.IsNullOrEmpty())
                {
                    po.CategoryId = vo.CategoryId != null ? vo.CategoryId : po.CategoryId;
                    po.PropertyId = vo.PropertyId != null ? vo.PropertyId : po.PropertyId;
                    po.SortOrder = vo.DisplayOrder != null ? vo.DisplayOrder : po.SortOrder;
                    po.IsShow = vo.IsDisplay != null ? vo.IsDisplay : po.IsShow;
                    listUpdate.Add(po);
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                CategoryPropertyDao.UpdateObjects(listUpdate);
            }
        }

        /// <summary>
        /// 获取所有语种的类别
        /// </summary>
        /// <returns>所有类别</returns>
        public IList<CategoryLanguage> GetAllCategoryLanguages()
        {
            var lstCategoryLanguagePos = CategoryDescDao.GetAllCategoryLanguages();
            return CategoryLanguagePoConvertToVoList(lstCategoryLanguagePos);
        }

        public IList<CategoryLanguage> GetAllCategoryLanguagesByLanguageId(int languageId)
        {
            var categoryLanguages = ImplCacheHelper.GetAllCategoryLanguagesByLanguageId(languageId);
            if (categoryLanguages.IsNullOrEmpty())
            {
                categoryLanguages = CategoryLanguagePoConvertToVoList(CategoryDescDao.GetAllCategoryLanguagesByLanguageId(languageId));
                ImplCacheHelper.SetAllCategoryLanguagesByLanguageId(categoryLanguages, languageId);
            }
            return categoryLanguages;

        }

        /// <summary>
        /// CategoryLanguage Po转换Vo
        /// </summary>
        /// <param name="categoryLanguagePo">CategoryLanguage Po</param>
        /// <returns></returns>
        private CategoryLanguage CategoryLanguagePoConvertToVo(CategoryDescPo categoryLanguagePo)
        {
            if (categoryLanguagePo.IsNullOrEmpty())
            {
                return null;
            }
            string categoryEnglishName = categoryLanguagePo.Name;
            if (categoryLanguagePo.LanguageId != ServiceFactory.ConfigureService.EnglishLangId)
            {
                CategoryDescPo categoryDescPo = CategoryDescDao.GetCategoryLanguage(categoryLanguagePo.LanguageId, ServiceFactory.ConfigureService.EnglishLangId);
                if (categoryDescPo != null)
                {
                    categoryEnglishName = categoryDescPo.Name;
                }
            }
            return new CategoryLanguage
            {
                CategoryId = categoryLanguagePo.CategoryId,
                LanguageId = categoryLanguagePo.LanguageId,
                CategoryEnglishName = categoryEnglishName,
                CategoryLanguageName = categoryLanguagePo.Name,
                CategoryLanguageDescription = categoryLanguagePo.Description
            };
        }

        /// <summary>
        /// CategoryLanguage Po集合转换 Vo集合
        /// </summary>
        /// <param name="lstCategoryLanguagePos">CategoryLanguage Po集合</param>
        private IList<CategoryLanguage> CategoryLanguagePoConvertToVoList(IList<CategoryDescPo> lstCategoryLanguagePos)
        {
            if (lstCategoryLanguagePos.IsNullOrEmpty())
                return new List<CategoryLanguage>();

            return lstCategoryLanguagePos.Select(CategoryLanguagePoConvertToVo).ToList();
        }

        #endregion

        #region 类别属性
        /// <summary>
        /// 获取类别绑定的所有属性
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <returns>有则返回属性列表，没有返回空IList</returns>
        public IList<CategoryProperty> GetCategoryBindedAllProperties(int categoryId)
        {
            var lstCategoryPropertyPos = CategoryPropertyDao.GetCategoryBindedAllProperties(categoryId);
            if (lstCategoryPropertyPos.IsNullOrEmpty())
                return new List<CategoryProperty>();

            return lstCategoryPropertyPos.Select(x => new CategoryProperty
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                PropertyId = x.PropertyId,
                DisplayOrder = x.SortOrder,
                SortType = EnumHelper.ToEnum<PropertyValueSortType>(x.SortType),
                IsDisplay = x.IsShow
            }).ToList();
        }

        public IList<CategoryProperty> GetCategoryBindedAllPropertiesRecursive(int categoryId)
        {
            var list = GetCategoryBindedAllProperties(categoryId);
            if (list.IsNullOrEmpty())
            {
                //由于只有末级类别才绑定了属性所有如果是非末级类别还需要取这个类别所有子孙类别绑定的属性
                var pos = CategoryPropertyDao.GetCategoryBindedAllPropertiesRecursive(categoryId);
                if (!pos.IsNullOrEmpty())
                {
                    list = pos.Select(c => new CategoryProperty
                    {
                        Id = 0,
                        CategoryId = categoryId,
                        PropertyId = c,
                        DisplayOrder = 0,
                        SortType = PropertyValueSortType.Undefined,
                        IsDisplay = true
                    }).ToList();
                }
            }

            return list;
        }

        #endregion


        /// <summary>
        /// 设置类别基本信息
        /// </summary>
        /// <param name="category">类别实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_CATEGORY_NOT_EXIST:产品类别不存在</value>
        /// </exception>
        public void SetCategoryBaseInfo(Service.Product.Category.Category category)
        {
            var categoryPo = CategoryDao.GetCategoryById(category.CategoryId);
            if (categoryPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PRODUCT_CATEGORY_NOT_EXIST);
            else
            {
                categoryPo.ChineseName = category.CategoryName ?? categoryPo.ChineseName;
                categoryPo.Image = category.CategoryImage ?? categoryPo.Image;
                categoryPo.IsShow = category.IsDisplay;
                categoryPo.SortOrder = category.DiplayOrder;
                CategoryDao.UpdateObject(categoryPo);
            }
        }

        /// <summary>
        /// 设置类别是否隐藏
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="isHidden">是否隐藏</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_CATEGORY_NOT_EXIST:产品类别不存在</value>
        /// </exception>
        public void SetCategoryHidden(int categoryId, bool isHidden)
        {
            var categoryPo = CategoryDao.GetCategoryById(categoryId);
            if (categoryPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PRODUCT_CATEGORY_NOT_EXIST);
            else
            {
                categoryPo.IsShow = isHidden;
                CategoryDao.UpdateObject(categoryPo);
            }
        }

        /// <summary>
        /// 通过类别ID获取类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>有则返回Category，没有返回null</returns>
        public Service.Product.Category.Category GetCategoryById(int categoryId)
        {
            var categoryVo = ImplCacheHelper.GetCategory(categoryId);
            if (categoryVo.IsNullOrEmpty())
            {
                var categoryPo = CategoryDao.GetCategoryById(categoryId);
                return CategoryPoConvertToVo(categoryPo);
            }
            return categoryVo;
        }

        public Service.Product.Category.Category GetParentCategoryById(int categoryId)
        {
            var lstCategoryVos = ImplCacheHelper.GetAllCategories();
            if (lstCategoryVos.IsNullOrEmpty())
            {
                var categoryPo = CategoryDao.GetParentCategoryById(categoryId);
                return CategoryPoConvertToVo(categoryPo);
            }
            else
            {
                if (lstCategoryVos.ToList().Exists(x => x.ParentId == categoryId))
                    return lstCategoryVos.First(x => x.ParentId == categoryId);
                else
                {
                    var categoryPo = CategoryDao.GetParentCategoryById(categoryId);
                    return CategoryPoConvertToVo(categoryPo);
                }
            }
        }

        /// <summary>
        /// 获取所有一级类别
        /// </summary>
        /// <returns>有则返回根类别列表，没有返回空IList</returns>
        public IList<Service.Product.Category.Category> GetAllRootCategories()
        {
            var lstCategoryVos = ImplCacheHelper.GetAllRootCategories();
            if (lstCategoryVos.IsNullOrEmpty())
            {
                var lstCategoryPos = CategoryDao.GetAllRootCategories();
                lstCategoryVos = CategoryPoConvertToVoList(lstCategoryPos);

                if (!lstCategoryVos.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetAllRootCategories(lstCategoryVos);
                }
            }
            return lstCategoryVos;
        }

        /// <summary>
        /// 根据类别Id获取子类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>有则返回子类别列表，没有返回空IList</returns>
        public IList<Service.Product.Category.Category> GetAllSubCategories(int categoryId)
        {
            var lstCategoryVos = ImplCacheHelper.GetAllSubCategories(categoryId);
            if (lstCategoryVos.IsNullOrEmpty())
            {
                var lstCategoryPos = CategoryDao.GetAllSubCategories(categoryId);
                lstCategoryVos = CategoryPoConvertToVoList(lstCategoryPos);
                if (!lstCategoryVos.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetAllSubCategories(lstCategoryVos, categoryId);
                }
            }
            return lstCategoryVos;
        }

        /// <summary>
        /// 获取所有状态的类别
        /// </summary>
        /// <returns>所有类别</returns>
        public IList<Service.Product.Category.Category> GetAllCategories()
        {
            var lstCategoryVos = ImplCacheHelper.GetAllCategories();
            if (lstCategoryVos.IsNullOrEmpty())
            {
                var lstCategoryPos = CategoryDao.GetAllCategories();
                lstCategoryVos = CategoryPoConvertToVoList(lstCategoryPos);

                if (!lstCategoryVos.IsNullOrEmpty())
                {
                    ImplCacheHelper.SetAllCategories(lstCategoryVos);
                }
            }
            return lstCategoryVos;
        }

        /// <summary>
        /// 获取所有末级类别
        /// </summary>
        /// <returns>末级类别列表，没有返回空IList</returns>
        public IList<Service.Product.Category.Category> GetAllLeafCategories()
        {
            var lstCategoryPos = CategoryDao.GetAllLeafCategories();
            return CategoryPoConvertToVoList(lstCategoryPos);
        }

        /// <summary>
        /// 通过类别ID获取topn子类别，按照排序字段升序排序
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="topN">top n</param>
        /// <returns>类别子实体列表，没有则返回null</returns>
        public IList<Service.Product.Category.Category> GetTopSubCategoriesById(int categoryId, int topN)
        {
            var lstCategoryPos = CategoryDao.GetTopSubCategoriesById(categoryId, topN);
            return CategoryPoConvertToVoList(lstCategoryPos);
        }

        /// <summary>
        /// 判断类别是否根类别
        /// 根类别的parent_id为0
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        public bool IsRootCategory(int categoryId)
        {
            var categoryPo = CategoryDao.GetCategoryById(categoryId);
            if (categoryPo.IsNullOrEmpty())
                return false;
            return categoryPo.ParentId == 0;
        }

        /// <summary>
        /// 判断类别是否末级类别
        /// 查找类别表中是否有parent_id为传入的categoryId，没有则为末级类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        public bool IsLeafCategory(int categoryId)
        {
            return CategoryDao.IsLeafCategory(categoryId);
        }

        /// <summary>
        /// 递归获取类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，入股为NUll则从根类别开始获取</param>
        /// <returns></returns>
        public IList<RelatedData<Service.Product.Category.Category>> GetCategoryTreeRecursive(int? parentCategoryId)
        {
            var categoryTree = new List<RelatedData<Service.Product.Category.Category>>();

            IList<Com.Panduo.Service.Product.Category.Category> categories = null;
            if (!parentCategoryId.HasValue)
            {
                categories = GetAllRootCategories();
            }
            else
            {
                categories = GetAllSubCategories(parentCategoryId.Value);
            }

            //先过滤显示的类别再按照序号排序再按照A-Z排序
            categories = categories.Where(c => c.IsDisplay && c.IsValid).OrderBy(c => c.DiplayOrder).ThenBy(c => c.CategoryName).ToList();

            if (!categories.IsNullOrEmpty())
            {
                foreach (Service.Product.Category.Category category in categories)
                {
                    var relatedCategory = new RelatedData<Service.Product.Category.Category>
                    {
                        Data = category,
                        SubDataList = GetCategoryTreeRecursive(category.CategoryId)
                    };

                    categoryTree.Add(relatedCategory);
                }
            }

            return categoryTree;
        }

        /// <summary>
        /// 递归获取类别树(缓存了所有的类别)
        /// </summary>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        public IList<RelatedData<Service.Product.Category.CategoryLanguage>> GetCategoryTreeRecursiveCache(int languageId)
        {
            var categories = ImplCacheHelper.GetCategoryTreeRecursiveCache(languageId);
            if (categories.IsNullOrEmpty())
            {
                categories = GetCategoryTreeRecursive(null, languageId);
                ImplCacheHelper.SetCategoryTreeRecursiveCache(categories, languageId);
            }
            return categories;
        }

        /// <summary>
        /// 递归获取类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，入股为NUll则从根类别开始获取</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        public IList<RelatedData<Service.Product.Category.CategoryLanguage>> GetCategoryTreeRecursive(int? parentCategoryId, int languageId)
        {
            var categoryTree = new List<RelatedData<Service.Product.Category.CategoryLanguage>>();

            IList<Com.Panduo.Service.Product.Category.Category> categories = null;
            if (!parentCategoryId.HasValue)
            {
                categories = GetAllRootCategories();
            }
            else
            {
                categories = GetAllSubCategories(parentCategoryId.Value);
            }

            //先过滤显示的类别再按照序号排序再按照A-Z排序
            categories = categories.Where(c => c.IsDisplay && c.IsValid).OrderBy(c => c.DiplayOrder).ThenBy(c => c.CategoryName).ToList();

            if (!categories.IsNullOrEmpty())
            {
                foreach (Service.Product.Category.Category category in categories)
                {
                    var isLeaf = ServiceFactory.CategoryService.IsLeafCategory(category.CategoryId);
                    if (isLeaf)
                    {
                        int result = SqlHelper.ExecuteScalar(SqlHelper.CONN_STRING, CommandType.Text, "select dbo.uf_category_product_count(" + category.CategoryId + ", '1,2')", null).ParseTo(0);
                        if (result <= 0)
                        {
                            continue;
                        }
                    }
                    var categoryLanguage = GetCategoryLanguageById(category.CategoryId, languageId);
                    categoryLanguage.CssName = category.CssName;
                    var relatedCategory = new RelatedData<Service.Product.Category.CategoryLanguage>
                    {
                        Data = categoryLanguage,
                        SubDataList = GetCategoryTreeRecursive(category.CategoryId, languageId)
                    };

                    categoryTree.Add(relatedCategory);
                }
            }

            return categoryTree;
        }

        /// <summary>
        /// 加载类别属性到缓存
        /// </summary>
        public void LoadCategoryProperties()
        {
            var allCategories = GetAllCategories();
            foreach (var category in allCategories)
            {
                var categoryProperties = GetCategoryBindedAllProperties(category.CategoryId);
                var allProperties = new List<Service.Product.Property.Property>();
                if (!categoryProperties.IsNullOrEmpty())
                {
                    foreach (var categoryProperty in categoryProperties)
                    {
                        //必须是该类别允许显示的属性
                        if (categoryProperty.IsDisplay)
                        {
                            var property = PropertyService.GetPropertyById(categoryProperty.PropertyId);
                            //属性自身也必须是有效的
                            if (property != null && property.IsValid)
                            {
                                //取当前语种的名称以及类别属性的排序信息
                                var propertyLanguage = PropertyService.GetPropertyLanguageById(property.PropertyId, ServiceFactory.ConfigureService.SiteLanguageId);
                                property.PropertyName = propertyLanguage.IsNullOrEmpty() ? property.PropertyName : propertyLanguage.PropertyName;
                                property.SortType = categoryProperty.SortType;
                                property.DisplayOrder = categoryProperty.DisplayOrder;

                                allProperties.Add(property);
                            }
                        }
                    }
                }

                //属性的排序方式：先按照序号再按照名称A-Z
                allProperties = allProperties.OrderBy(c => c.DisplayOrder).ThenBy(c => c.PropertyName).ToList();

                ImplCacheHelper.SetAllPropertiesOfCategory(category.CategoryId, ServiceFactory.ConfigureService.SiteLanguageId, allProperties);
            }
        }

        public List<CategoryLanguage> GetCategoryLanguageFamliy(int categoryId, int? languageId)
        {
            var categoryLanguages = new List<CategoryLanguage>();
            var category = GetCategoryById(categoryId);
            if (category.IsNullOrEmpty()) return categoryLanguages;
            CategoryLanguage categoryLanguage;
            do
            {
                categoryLanguage = GetCategoryLanguageById(category.CategoryId, languageId ?? ServiceFactory.ConfigureService.SiteLanguageId);
                categoryLanguages.Insert(0, categoryLanguage);
                if (category.ParentId == 0)
                    break;
                category = GetCategoryById(category.ParentId);
            } while (!category.IsNullOrEmpty());
            return categoryLanguages;
        }


        /// <summary>
        /// CategoryAdvertisement Po转Vo
        /// </summary>
        /// <param name="categoryAdPo">CategoryAdPo</param>
        /// <returns>CategoryVo</returns>
        private CategoryAdvertisement CategoryAdvertisementPoConvertToVo(CategoryAdPo categoryAdPo)
        {
            if (categoryAdPo.IsNullOrEmpty())
                return null;
            return new CategoryAdvertisement
            {
                LanguageId = categoryAdPo.LanguageId,
                CategoryId = categoryAdPo.CategoryId,
                AdvertisingImage = categoryAdPo.AdImage,
                AdvertisingWords = categoryAdPo.AdWord,
                MarketingTitle = categoryAdPo.MarketingTitle,
                Url = categoryAdPo.Link,
                ProductMarketingArea = categoryAdPo.MarketingText
            };
        }

        /// <summary>
        /// Category Po转Vo
        /// </summary>
        /// <param name="categoryPo">CategoryPo</param>
        /// <returns>CategoryVo</returns>
        private Service.Product.Category.Category CategoryPoConvertToVo(CategoryPo categoryPo)
        {
            if (categoryPo.IsNullOrEmpty())
                return null;

            return new Service.Product.Category.Category
            {
                CategoryId = categoryPo.CategoryId,
                ParentId = categoryPo.ParentId,
                CategoryCode = categoryPo.Code,
                CategoryImage = categoryPo.Image,
                CategoryName = categoryPo.ChineseName,
                CssName = categoryPo.ClassName,
                CreateTime = categoryPo.DateCreated,
                DiplayOrder = categoryPo.SortOrder,
                IsDisplay = categoryPo.IsShow,
                IsValid = categoryPo.Status,
                ModfiyTime = categoryPo.DateModified
            };
        }

        /// <summary>
        /// Category Po集合转Vo集合
        /// <para>顺便将缓存中没有产品的类别填充进去</para>
        /// </summary>
        /// <param name="lstCategoryPos">CategoryPo List</param>
        private List<Service.Product.Category.Category> CategoryPoConvertToVoList(IList<CategoryPo> lstCategoryPos)
        {
            if (lstCategoryPos.IsNullOrEmpty())
                return new List<Service.Product.Category.Category>();
            var lstCategoryVos = lstCategoryPos.Select(CategoryPoConvertToVo).ToList();

            return lstCategoryVos;
        }

        #endregion

    }
}
