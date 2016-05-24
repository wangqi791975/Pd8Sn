using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Help;
using Com.Panduo.Service;
using Com.Panduo.Service.Help;
using Com.Panduo.ServiceImpl.Help.Dao;
using NHibernate.Linq;

namespace Com.Panduo.ServiceImpl.Help
{
    public class HelpService : IHelpService
    {
        #region IOC
        public IHelpCategoryDao HelpCategoryDao { private get; set; }
        public IHelpCategoryDescriptionDao HelpCategoryDescriptionDao { private get; set; }
        public IHelpArticleDao HelpArticleDao { private get; set; }
        public IHelpArticleDescriptionDao HelpArticleDescriptionDao { private get; set; }

        public IVHelpCategoryDao VHelpCategoryDao { private get; set; }
        public IVHelpArticleDao VHelpArticleDao { private get; set; }
        #endregion

        #region 前台(当前语种)
        /// <summary>
        /// 获取所有一级类别
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        /// </summary>
        public IList<VHelpCategory> GetRootHelpCategories(int languageId)
        {
            var lstVHelpCategoryPo = VHelpCategoryDao.FindDataByHql("from VHelpCategoryPo where ParentId=0 and LanguageId=?", new object[] { languageId });

            return lstVHelpCategoryPo.Select(VHelpCategoryPoToVo).ToList();
        }

        /// <summary>
        /// 所有二级类别 top
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="topn"></param>
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        public IList<VHelpCategory> GetSubHelpCategoryOfRootByTop(int languageId, int topn)
        {
            var lstVHelpCategoryPo = VHelpCategoryDao.FindDataByHql("from VHelpCategoryPo where ParentId<>0 and IsShowParent=1 and LanguageId=?", new object[] { languageId });

            return lstVHelpCategoryPo.Select(VHelpCategoryPoToVo).ToList();
        }

        /// <summary>
        /// 获取所有枝、叶类别（非一级类别）
        /// </summary>
        public List<HelpCategory> GetSubHelpCategoryNoRoot()
        {
            var lstHelpCategoryPo = HelpCategoryDao.FindDataByHql("from HelpCategoryPo where ParentId<>0");

            return lstHelpCategoryPo.Select(HelpCategoryPoToVo).ToList();
        }

        /// <summary>
        /// 获取根类别下的文章 top前5条 用于帮助中心首页显示
        /// </summary>
        public IList<VHelpArticle> GetHelpArticlesOfRootByTop(int languageId, int topn)
        {
            var lstHelpArticlePo = VHelpArticleDao.FindDataByHql("from VHelpArticlePo where IsShowParent=1 and LanguageId=?", new object[] { languageId });

            return lstHelpArticlePo.Select(VHelpArticlePoToVo).ToList();
        }

        public int GetRootHelpCategoryId(int helpCategoryId)
        {
            var category = GetHelpCategory(helpCategoryId);
            while (!category.IsNullOrEmpty())
            {
                if (category.ParentId == 0)
                    break;
                category = GetHelpCategory(category.ParentId);
            }
            return category.IsNullOrEmpty() ? 0 : category.HelpCategoryId;
        }

        public PageData<HelpCategory> FindHelpCategories(int page, int pageSize, Dictionary<HelpCategorySearchCriteria, object> searchCriteria, List<Sorter<HelpCategorySorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM HelpCategoryPo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case HelpCategorySearchCriteria.Keyword:
                            hqlHelper.AddWhere("CategoryName", HqlOperator.Like, "CategoryName", item.Value);
                            break;
                        case HelpCategorySearchCriteria.ParentId:
                            hqlHelper.AddWhere("ParentId", HqlOperator.Eq, "ParentId", item.Value);
                            break;
                        case HelpCategorySearchCriteria.LanguageId:
                            hqlHelper.AddWhere("LanguageId", HqlOperator.Eq, "LanguageId", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        //case HelpCategorySorterCriteria.Status:
                        //    hqlHelper.AddSorter("Status", sorter.IsAsc);
                        //    break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("DateCreated", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = HelpCategoryDao.FindPageDataByHql(page, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<HelpCategory>();
            var voList = pageDataPo.Data.Select(HelpCategoryPoToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public IList<VHelpCategory> GetHelpCategoryFamliyByLanguageId(int categoryId, int languageId)
        {
            var categoryLanguages = new List<VHelpCategory>();
            var category = GetHelpCategoryById(categoryId, languageId);
            while (!category.IsNullOrEmpty())
            {
                categoryLanguages.Insert(0, category);
                if (category.ParentId == 0)
                    break;
                category = GetHelpCategoryById(category.ParentId, languageId);
            }
            return categoryLanguages;
        }

        public bool IsLastLevel(int categoryId)
        {
            var category = HelpCategoryDao.GetOneObject("FROM HelpCategoryPo WHERE ParentId=?", new object[] { categoryId });
            return category.IsNullOrEmpty();
        }


        /// <summary>
        /// 通过类别ID得到类别实体
        /// </summary>
        public VHelpCategory GetHelpCategoryById(int categoryId, int languageId)
        {
            var helpCategoryPo = VHelpCategoryDao.GetOneObject("from VHelpCategoryPo where HelpCategoryId=? and LanguageId=?", new object[] { categoryId, languageId });

            return VHelpCategoryPoToVo(helpCategoryPo);
        }

        /// <summary>
        /// 通过类别ID得到该类别下的子类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId"></param>
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        public IList<VHelpCategory> GetSubHelpCategorById(int categoryId, int languageId)
        {
            var lstVHelpCategoryPo = VHelpCategoryDao.FindDataByHql("from VHelpCategoryPo where ParentId=? and LanguageId=?", new object[] { categoryId, languageId });

            return lstVHelpCategoryPo.Select(VHelpCategoryPoToVo).ToList();
        }

        /// <summary>
        /// 通过类别ID和语种ID得到类别多语种实体
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        public HelpCategoryDescription GetHelpCategoryDescriptionById(int categoryId, int languageId)
        {
            var helpCategoryDescriptionPo = HelpCategoryDescriptionDao.GetOneObject("from HelpCategoryDescriptionPo where HelpCategoryId=? and LanguageId=?", new object[] { categoryId, languageId });

            return HelpCategoryDescriptionPoToVo(helpCategoryDescriptionPo);
        }

        /// <summary>
        /// 通过类别ID得到类别文章实体
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public HelpArticle GetHelpArticleById(int articleId)
        {
            var helpArticlePo = HelpArticleDao.GetOneObject("from HelpArticlePo where ArticleId=?", new object[] { articleId });

            return HelpArticlePoToVo(helpArticlePo);
        }

        /// <summary>
        /// 通过类别ID得到类别文章实体
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<HelpArticle> GetHelpArticlesByCategoryId(int categoryId)
        {
            var lstHelpArticlePo = HelpArticleDao.FindDataByHql("from HelpArticlePo where HelpCategoryId=?", new object[] { categoryId });

            return lstHelpArticlePo.Select(HelpArticlePoToVo).ToList();
        }



        /// <summary>
        /// 通过类别ID和语种ID得到类别文章多语种实体
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        public HelpArticleDescription GetHelpArticleDescriptionById(int articleId, int languageId)
        {
            var helpArticleDescriptionPo = HelpArticleDescriptionDao.GetOneObject("from HelpArticleDescriptionPo where ArticleId=? and LanguageId=?", new object[] { articleId, languageId });

            return HelpArticleDescriptionPoToVo(helpArticleDescriptionPo);
        }

        /// <summary>
        /// 通过类别ID和语种ID得到类别文章多语种的上一个实体
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        public VHelpArticle GetPreviousHelpArticleById(int articleId, int languageId)
        {
            var lstHelpArticlePo = VHelpArticleDao.FindDataByHqlLimit("from VHelpArticlePo where ArticleId<? and LanguageId=? order by ArticleId desc", 1, articleId, languageId);

            return VHelpArticlePoToVo(lstHelpArticlePo.FirstOrDefault());
        }

        /// <summary>
        /// 通过类别ID和语种ID得到类别文章多语种的下一个实体
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        public VHelpArticle GetNextHelpArticleById(int articleId, int languageId)
        {
            var lstHelpArticlePo = VHelpArticleDao.FindDataByHqlLimit("from VHelpArticlePo where ArticleId>? and LanguageId=? order by ArticleId asc", 1, articleId, languageId);

            return VHelpArticlePoToVo(lstHelpArticlePo.FirstOrDefault());
        }

        /// <summary>
        /// 搜索帮助文章(前台、后台都调用该方法，搜索参数不同)
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的帮助文章列表</returns>
        public PageData<HelpArticle> FindHelpArticlesOfAdmin(int currentPage, int pageSize, IDictionary<ArticleSearchCriteria, object> searchCriteria, IList<Sorter<ArticleSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM HelpArticlePo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ArticleSearchCriteria.Keyword:
                            hqlHelper.AddWhere("Title", HqlOperator.Like, "Title", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        //case ShippingSorterCriteria.Status:
                        //    hqlHelper.AddSorter("Status", sorter.IsAsc);
                        //    break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("DateCreated", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = HelpArticleDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<HelpArticle>();
            var voList = pageDataPo.Data.Select(HelpArticlePoToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public PageData<VHelpArticle> FindHelpArticles(int currentPage, int pageSize, IDictionary<ArticleSearchCriteria, object> searchCriteria, IList<Sorter<ArticleSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VHelpArticlePo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ArticleSearchCriteria.CategoryId:
                            hqlHelper.AddWhere("HelpCategoryId", HqlOperator.Eq, "HelpCategoryId", item.Value);
                            break;
                        case ArticleSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("LanguageId", HqlOperator.Eq, "LanguageId", item.Value);
                            break;
                        case ArticleSearchCriteria.Keyword:
                            hqlHelper.AddWhere("ArticleName", HqlOperator.Like, "ArticleName", item.Value);
                            //hqlHelper.AddWhere("ArticleName", HqlOperator.Like, "ArticleName", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        case ArticleSorterCriteria.DateCreated:
                            hqlHelper.AddSorter("DateCreated", sorter.IsAsc);
                            break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("DateCreated", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = VHelpArticleDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<VHelpArticle>();
            var voList = pageDataPo.Data.Select(VHelpArticlePoToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        private static VHelpCategory VHelpCategoryPoToVo(VHelpCategoryPo po)
        {
            if (po.IsNullOrEmpty()) return null;

            return new VHelpCategory
            {
                HelpCategoryId = po.HelpCategoryId,
                CategoryType = po.CategoryType,
                ParentId = po.ParentId,
                CategoryPath = po.CategoryPath,
                SortOrder = po.SortOrder,
                DateCreated = po.DateCreated,
                DateModified = po.DateModified,
                CategoryName = po.CategoryName,
                EnCategoryName = po.EnCategoryName,
                LanguageId = po.LanguageId,
                IsShowParent = po.IsShowParent
            };
        }

        private HelpCategory HelpCategoryPoToVo(HelpCategoryPo po)
        {
            if (po.IsNullOrEmpty()) return null;
            var category = new HelpCategory
                        {
                            HelpCategoryId = po.HelpCategoryId,
                            CategoryType = po.CategoryType.ToEnum<CategoryType>(),
                            CategoryName = po.CategoryName,
                            ParentId = po.ParentId,
                            CategoryPath = po.CategoryPath,
                            SortOrder = po.SortOrder,
                            DateCreated = po.DateCreated,
                            DateModified = po.DateModified,
                            Status = po.Status,
                            IsShowParent = po.IsShowParent
                        };

            //获取类别多语种列表
            category.Descriptions = HelpCategoryDescriptionDao.FindDataByHql("from HelpCategoryDescriptionPo where HelpCategoryId=?", new object[] { po.HelpCategoryId }).Select(HelpCategoryDescriptionPoToVo).ToList();

            return category;
        }

        private static HelpCategoryDescription HelpCategoryDescriptionPoToVo(HelpCategoryDescriptionPo po)
        {
            if (po.IsNullOrEmpty()) return null;

            return new HelpCategoryDescription
            {
                DescriptionId = po.DescriptionId,
                HelpCategoryId = po.HelpCategoryId,
                LanguageId = po.LanguageId,
                CategoryName = po.CategoryName,
                DateCreated = po.DateCreated,
                DateModified = po.DateModified,
                Status = po.Status,
            };
        }

        private HelpArticle HelpArticlePoToVo(HelpArticlePo po)
        {
            if (po.IsNullOrEmpty()) return null;
            var article = new HelpArticle
                        {
                            ArticleId = po.ArticleId,
                            HelpCategoryId = po.HelpCategoryId,
                            CategoryPath = po.CategoryPath,
                            Title = po.Title,
                            Keywords = po.Keywords,
                            Content = po.Content,
                            Status = po.Status,
                            IsRecommend = po.IsRecommend,
                            SortOrder = po.SortOrder,
                            UsefulNumber = po.UsefulNumber,
                            UnusefulNumber = po.UnusefulNumber,
                            BrowseNumber = po.BrowseNumber,
                            DateCreated = po.DateCreated,
                            DateModified = po.DateModified,
                        };
            //获取文章多语种列表
            article.Descriptions = HelpArticleDescriptionDao.FindDataByHql("from HelpArticleDescriptionPo where ArticleId=?", new object[] { po.ArticleId }).Select(HelpArticleDescriptionPoToVo).ToList();

            return article;
        }
        private static VHelpArticle VHelpArticlePoToVo(VHelpArticlePo po)
        {
            if (po.IsNullOrEmpty()) return null;

            return new VHelpArticle
            {
                ArticleId = po.ArticleId,
                HelpCategoryId = po.HelpCategoryId,
                CategoryPath = po.CategoryPath,
                Keywords = po.Keywords,
                IsRecommend = po.IsRecommend,
                SortOrder = po.SortOrder,
                UsefulNumber = po.UsefulNumber,
                UnusefulNumber = po.UnusefulNumber,
                BrowseNumber = po.BrowseNumber,
                DateCreated = po.DateCreated,
                DateModified = po.DateModified,
                LanguageId = po.LanguageId,
                ArticleName = po.ArticleName,
                EnArticleName = po.EnArticleName,
                Status = po.Status,
                IsShowParent = po.IsShowParent
            };
        }
        private static HelpArticleDescription HelpArticleDescriptionPoToVo(HelpArticleDescriptionPo po)
        {
            if (po.IsNullOrEmpty()) return null;

            return new HelpArticleDescription
            {
                DescriptionId = po.DescriptionId,
                ArticleId = po.ArticleId,
                LanguageId = po.LanguageId,
                ArticleName = po.ArticleName,
                Keywords = po.Keywords,
                ArticleContent = po.Content,
                DateCreated = po.DateCreated,
                DateModified = po.DateModified,
                IsShowParent = po.IsShowParent
            };
        }
        #endregion


        #region 后台

        #region 类别
        /// <summary>
        /// 递归获取帮助类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，入股为NUll则从根类别开始获取</param>
        /// <returns></returns>
        public IList<RelatedData<HelpCategory>> GetHelpCategoryTreeRecursive(int? parentCategoryId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有一级类别
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        /// </summary>
        public IList<HelpCategory> GetRootHelpCategoriesByCn()
        {
            var lstVHelpCategoryPo = HelpCategoryDao.FindDataByHql("from HelpCategoryPo where ParentId=0");

            return lstVHelpCategoryPo.Select(HelpCategoryPoToVo).ToList();
        }

        /// <summary>
        /// 得到HelpCategory
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>HelpCategory实体</returns>
        public HelpCategory GetHelpCategory(int categoryId)
        {
            var helpCategoryPo = HelpCategoryDao.GetOneObject("from HelpCategoryPo where HelpCategoryId=?", new object[] { categoryId });

            return HelpCategoryPoToVo(helpCategoryPo);
        }

        /// <summary>
        /// 得到HelpCategoryDescription
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>HelpCategoryDescription实体列表</returns>
        public IList<HelpCategoryDescription> GetHelpCategoryDescriptions(int categoryId)
        {
            var lstHelpCategoryDescriptionPo = HelpCategoryDescriptionDao.FindDataByHql("from HelpCategoryDescriptionPo where HelpCategoryId=?", new object[] { categoryId });

            return lstHelpCategoryDescriptionPo.Select(HelpCategoryDescriptionPoToVo).ToList();
        }

        /// <summary>
        /// 添加帮助类别(不能超过3级)
        /// </summary>
        /// <param name="helpCategory">帮助类别实体</param>
        public int SetHelpCategory(HelpCategory helpCategory)
        {
            var helpCategoryPo = HelpCategoryDao.GetOneObject("from HelpCategoryPo where HelpCategoryId=?", new object[] { helpCategory.HelpCategoryId });
            if (helpCategoryPo.IsNullOrEmpty())
            {
                helpCategoryPo = new HelpCategoryPo
                {
                    CategoryType = (int)helpCategory.CategoryType,
                    CategoryName = helpCategory.CategoryName,
                    ParentId = helpCategory.ParentId,
                    CategoryPath = helpCategory.CategoryPath,
                    SortOrder = helpCategory.SortOrder,
                    DateCreated = helpCategory.DateCreated,
                    DateModified = helpCategory.DateModified,
                    Status = helpCategory.Status,
                    IsShowParent = helpCategory.IsShowParent
                };
                helpCategoryPo.HelpCategoryId = HelpCategoryDao.AddObject(helpCategoryPo);
            }
            else
            {
                helpCategoryPo.CategoryType = (int)helpCategory.CategoryType;
                helpCategoryPo.CategoryName = helpCategory.CategoryName;
                helpCategoryPo.ParentId = helpCategory.ParentId;
                helpCategoryPo.CategoryPath = helpCategory.CategoryPath;
                helpCategoryPo.SortOrder = helpCategory.SortOrder;
                helpCategoryPo.DateModified = helpCategory.DateModified;
                helpCategoryPo.Status = helpCategory.Status;
                helpCategoryPo.IsShowParent = helpCategory.IsShowParent;
                HelpCategoryDao.UpdateObject(helpCategoryPo);
            }
            SetHelpCategoryDescription(helpCategoryPo.HelpCategoryId, helpCategory.Descriptions);

            return helpCategoryPo.HelpCategoryId;
        }

        /// <summary>
        /// 删除帮助类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        public void DeleteHelpCategory(int categoryId)
        {
            HelpCategoryDao.DeleteObjectByHql("delete from HelpCategoryPo where HelpCategoryId=?", new object[] { categoryId });
        }

        /// <summary>
        /// 添加或修改帮助类别多语种
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="helpCategoryDescriptions"></param>
        public void SetHelpCategoryDescription(int categoryId, IList<HelpCategoryDescription> helpCategoryDescriptions)
        {
            HelpCategoryDescriptionDao.DeleteObjectByHql("delete from HelpCategoryDescriptionPo where HelpCategoryId=?", new object[] { categoryId });
            var lstHelpCategoryDescriptionPo = helpCategoryDescriptions.Select(x => new HelpCategoryDescriptionPo
            {
                DescriptionId = x.DescriptionId,
                HelpCategoryId = categoryId,//x.HelpCategoryId,
                LanguageId = x.LanguageId,
                CategoryName = x.CategoryName,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Status = x.Status,
            });
            HelpCategoryDescriptionDao.AddObjects(lstHelpCategoryDescriptionPo);
        }
        #endregion

        #region 文章

        /// <summary>
        /// 得到HelpArticle
        /// </summary>
        /// <param name="articleId">类别ID</param>
        /// <returns>HelpArticle实体</returns>
        public HelpArticle GetHelpArticle(int articleId)
        {
            var helpArticlePo = HelpArticleDao.GetOneObject("from HelpArticlePo where ArticleId=?", new object[] { articleId });

            return HelpArticlePoToVo(helpArticlePo);
        }

        /// <summary>
        /// 得到HelpCategoryDescription
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <returns>HelpCategoryDescription实体列表</returns>
        public IList<HelpArticleDescription> GetHelpArticleDescriptions(int articleId)
        {
            var lstHelpArticleDescriptionPo = HelpArticleDescriptionDao.FindDataByHql("from HelpArticleDescriptionPo where ArticleId=?", new object[] { articleId });

            return lstHelpArticleDescriptionPo.Select(HelpArticleDescriptionPoToVo).ToList();
        }

        /// <summary>
        /// 添加帮助文章
        /// </summary>
        /// <param name="helpArticle">帮助文章实体</param>
        public int SetHelpArticle(HelpArticle helpArticle)
        {
            var helpArticlePo = HelpArticleDao.GetOneObject("from HelpArticlePo where ArticleId=?", new object[] { helpArticle.ArticleId });
            if (helpArticlePo.IsNullOrEmpty())
            {
                helpArticlePo = new HelpArticlePo
                {
                    HelpCategoryId = helpArticle.HelpCategoryId,
                    CategoryPath = helpArticle.CategoryPath,
                    Title = helpArticle.Title,
                    Keywords = helpArticle.Keywords,
                    Content = helpArticle.Content,
                    Status = helpArticle.Status,
                    IsRecommend = helpArticle.IsRecommend,
                    SortOrder = helpArticle.SortOrder,
                    UsefulNumber = helpArticle.UsefulNumber,
                    UnusefulNumber = helpArticle.UnusefulNumber,
                    BrowseNumber = helpArticle.BrowseNumber,
                    DateCreated = helpArticle.DateCreated,
                    DateModified = helpArticle.DateModified
                };
                helpArticlePo.ArticleId = HelpArticleDao.AddObject(helpArticlePo);
            }
            else
            {
                helpArticlePo.HelpCategoryId = helpArticle.HelpCategoryId;
                helpArticlePo.CategoryPath = helpArticle.CategoryPath;
                helpArticlePo.Title = helpArticle.Title;
                helpArticlePo.Keywords = helpArticle.Keywords;
                helpArticlePo.Content = helpArticle.Content;
                helpArticlePo.Status = helpArticle.Status;
                helpArticlePo.IsRecommend = helpArticle.IsRecommend;
                helpArticlePo.SortOrder = helpArticle.SortOrder;
                helpArticlePo.UsefulNumber = helpArticle.UsefulNumber;
                helpArticlePo.UnusefulNumber = helpArticle.UnusefulNumber;
                helpArticlePo.BrowseNumber = helpArticle.BrowseNumber;
                helpArticlePo.DateModified = helpArticle.DateModified;
                HelpArticleDao.UpdateObject(helpArticlePo);
            }
            return helpArticlePo.ArticleId;
        }

        /// <summary>
        /// 删除帮助文章
        /// </summary>
        /// <param name="articleId">文章ID</param>
        public void DeleteHelpArticle(int articleId)
        {
            HelpArticleDao.DeleteObjectByHql("delete from HelpArticlePo where ArticleId=?", new object[] { articleId });
        }

        /// <summary>
        /// 添加或修改帮助文章多语种
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="helpArticleDescriptions"></param>
        public void SetHelpArticleDescription(int articleId, IList<HelpArticleDescription> helpArticleDescriptions)
        {
            HelpArticleDescriptionDao.DeleteObjectByHql("delete from HelpArticleDescriptionPo where ArticleId=?", new object[] { articleId });
            var lstHelpArticleDescriptionPo = helpArticleDescriptions.Select(x => new HelpArticleDescriptionPo
            {
                ArticleId = x.ArticleId,
                DescriptionId = x.DescriptionId,
                LanguageId = x.LanguageId,
                ArticleName = x.ArticleName,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                IsShowParent = x.IsShowParent,
                Status = x.Status,
            });
            HelpArticleDescriptionDao.AddObjects(lstHelpArticleDescriptionPo);
        }


        #endregion

        #endregion
    }
}
