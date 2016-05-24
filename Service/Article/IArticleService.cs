//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：IArticleService.cs
//创 建 人：罗海明
//创建时间：2015/04/07 16:10:40 
//功能说明：文章管理接口
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;

namespace Com.Panduo.Service.Article
{
    public interface IArticleService
    {

        /// <summary>
        /// 获取零散文章主表
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <returns>返回主键</returns>
        SomeArticle GetSomeArticle(int articleId);

        /// <summary>
        /// 设置零散文章主表
        /// </summary>
        /// <param name="someArticle">零散文章</param>
        /// <returns>返回主键</returns>
        int SetSomeArticle(SomeArticle someArticle);

        /// <summary>
        /// 设置零散文章多语种表
        /// </summary>
        /// <param name="language">SomeArticleLanguage</param>
        void SetSomeArticleLanguage(SomeArticleLanguage language);

        /// <summary>
        /// 设置零散文章多语种表
        /// </summary>
        /// <param name="language">SomeArticleLanguage</param>
        void SetSomeArticleLanguages(IList<SomeArticleLanguage> language);

        /// <summary>
        /// 根据文章Id获取零散文章多语种列表（编辑时候读取）
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <returns>SomeArticleLanguage列表</returns>
        IList<SomeArticleLanguage> GetSomeArticleLanguage(int articleId);

        
        /// <summary>
        /// 根据文章Id和语种Id获取零散文章多语种列表（前台读取）
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>SomeArticleLanguage列表</returns>
        SomeArticleLanguage GetSomeArticle(int articleId, int languageId);

        /// <summary>
        /// 获取零散文章列表
        /// </summary>
        /// <returns>SomeArticle列表</returns>
        PageData<SomeArticle> GetSomeArticle(int currentPage, int pageSize, IDictionary<SomeArticleCriteria, object> searchDictionary, IList<Sorter<SomeArticleSorterCriteria>> sorterCriteria);
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public enum SomeArticleCriteria
    {
        //标题
        Title
    }

    /// <summary>
    ///排序条件
    /// </summary>
    public enum SomeArticleSorterCriteria
    {
        //创建时间
        CreateTime
    }
}
