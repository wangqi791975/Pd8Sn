using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Help
{
    public interface IHelpService
    {
        #region 前台(当前语种)
        /// <summary>
        /// 获取所有一级类别
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        /// </summary>
        IList<VHelpCategory> GetRootHelpCategories(int languageId);

        /// <summary>
        /// 所有二级类别 top
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="topn"></param>
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        IList<VHelpCategory> GetSubHelpCategoryOfRootByTop(int languageId, int topn);

        /// <summary>
        /// 获取根类别下的文章 top前5条 用于帮助中心首页显示
        /// </summary>
        /// <returns></returns>
        IList<VHelpArticle> GetHelpArticlesOfRootByTop(int languageId, int topn);

        IList<VHelpCategory> GetHelpCategoryFamliyByLanguageId(int categoryId, int languageId);

        /// <summary>
        /// 当前类别是否为末级类别
        /// </summary>
        bool IsLastLevel(int categoryId);
        /// <summary>
        /// 通过类别ID得到该类别下的子类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId"></param>
        /// <returns>有则返回根类别列表，没有返回空IList（对应语种）</returns>
        IList<VHelpCategory> GetSubHelpCategorById(int categoryId, int languageId);

        /// <summary>
        /// 通过类别ID得到类别实体
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        VHelpCategory GetHelpCategoryById(int categoryId, int languageId);

        IList<HelpArticle> GetHelpArticlesByCategoryId(int categoryId);

        /// <summary>
        /// 通过类别ID和语种ID得到类别多语种实体
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        HelpCategoryDescription GetHelpCategoryDescriptionById(int categoryId, int languageId);


        /// <summary>
        /// 通过类别ID得到类别文章实体
        /// </summary>
        /// <returns></returns>
        HelpArticle GetHelpArticleById(int articleId);

        /// <summary>
        /// 通过类别ID和语种ID得到类别文章多语种实体
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        HelpArticleDescription GetHelpArticleDescriptionById(int articleId, int languageId);

        /// <summary>
        /// 通过类别ID和语种ID得到类别文章多语种的上一个实体
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        VHelpArticle GetPreviousHelpArticleById(int articleId, int languageId);

        /// <summary>
        /// 通过类别ID和语种ID得到类别文章多语种的下一个实体
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        VHelpArticle GetNextHelpArticleById(int articleId, int languageId);

        /// <summary>
        /// 搜索帮助文章(前台、后台都调用该方法，搜索参数不同)
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的帮助文章列表</returns>
        PageData<HelpArticle> FindHelpArticlesOfAdmin(int currentPage, int pageSize, IDictionary<ArticleSearchCriteria, object> searchCriteria, IList<Sorter<ArticleSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 搜索帮助文章(前台、后台都调用该方法，搜索参数不同)
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的帮助文章列表</returns>
        PageData<VHelpArticle> FindHelpArticles(int currentPage, int pageSize, IDictionary<ArticleSearchCriteria, object> searchCriteria, IList<Sorter<ArticleSorterCriteria>> sorterCriteria);

        #endregion


        #region 后台

        #region 类别
        /// <summary>
        /// 递归获取帮助类别树
        /// </summary>
        /// <param name="parentCategoryId">上级类别ID，入股为NUll则从根类别开始获取</param>
        /// <returns></returns>
        IList<RelatedData<Service.Help.HelpCategory>> GetHelpCategoryTreeRecursive(int? parentCategoryId);

        IList<HelpCategory> GetRootHelpCategoriesByCn();
        /// <summary>
        /// 得到HelpCategory
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>HelpCategory实体</returns>
        HelpCategory GetHelpCategory(int categoryId);

        /// <summary>
        /// 得到HelpCategoryDescription
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns>HelpCategoryDescription实体列表</returns>
        IList<HelpCategoryDescription> GetHelpCategoryDescriptions(int categoryId);

        /// <summary>
        /// 添加帮助类别(不能超过3级)
        /// </summary>
        /// <param name="helpCategory">帮助类别实体</param>
        int SetHelpCategory(HelpCategory helpCategory);

        /// <summary>
        /// 删除帮助类别
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        void DeleteHelpCategory(int categoryId);

        /// <summary>
        /// 添加或修改帮助类别多语种
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="helpCategoryDescriptions"></param>
        void SetHelpCategoryDescription(int categoryId, IList<HelpCategoryDescription> helpCategoryDescriptions);
        #endregion

        #region 文章

        /// <summary>
        /// 得到HelpArticle
        /// </summary>
        /// <param name="articleId">类别ID</param>
        /// <returns>HelpArticle实体</returns>
        HelpArticle GetHelpArticle(int articleId);

        /// <summary>
        /// 得到HelpCategoryDescription
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <returns>HelpCategoryDescription实体列表</returns>
        IList<HelpArticleDescription> GetHelpArticleDescriptions(int articleId);

        /// <summary>
        /// 设置帮助文章
        /// </summary>
        /// <param name="helpArticle">帮助文章实体</param>
        int SetHelpArticle(HelpArticle helpArticle);

        /// <summary>
        /// 删除帮助文章
        /// </summary>
        /// <param name="articleId">文章ID</param>
        void DeleteHelpArticle(int articleId);

        /// <summary>
        /// 添加或修改帮助文章多语种
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="helpArticleDescriptions"></param>
        void SetHelpArticleDescription(int articleId, IList<HelpArticleDescription> helpArticleDescriptions);

        #endregion

        #endregion

        /// <summary>
        /// 获取当前分类 的根目录
        /// </summary>
        int GetRootHelpCategoryId(int helpCategoryId);

        PageData<HelpCategory> FindHelpCategories(int page, int pageSize, Dictionary<HelpCategorySearchCriteria, object> searchCriteria, List<Sorter<HelpCategorySorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取所有枝、叶类别（非一级类别）
        /// </summary>
        List<HelpCategory> GetSubHelpCategoryNoRoot();
    }

    public enum ArticleSearchCriteria
    {
        LanguageId,
        CategoryId,
        Keyword
    }

    public enum ArticleSorterCriteria
    {
        DateCreated
    }
    public enum HelpCategorySearchCriteria
    {
        LanguageId,
        ParentId,
        Keyword,
    }

    public enum HelpCategorySorterCriteria
    {
        DateCreated
    }
}
