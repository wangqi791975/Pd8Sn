using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Com.Panduo.Service.Suggestion
{
    /// <summary>
    /// 客户建议服务
    /// </summary>
    public interface ISuggestionService
    {

        #region 业务异常
        /// <summary>
        /// 建议不存在
        /// </summary>
        string ERROR_SUGGESTION_CONTENT_NOT_EXIST { get; }
        #endregion

        #region 方法

        /// <summary>
        /// 获取所有评分类型
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns>所有评分类型</returns>
        IList<SuggestionItem> GetAllSuggestionItems(int languageId);

        /// <summary>
        /// 通过评分类型Id获取类型对象
        /// </summary>
        /// <param name="itemId">评分类型Id</param>
        /// <returns>评分类型对应的评分对象</returns>
        IList<SuggestionObject> GetSuggestionObjectsByItemId(int itemId);

        /// <summary>
        /// 获取所有评分对象
        /// </summary>
        /// <returns>所有评分对象</returns>
        IList<SuggestionObject> GetAllSuggestionObjects();

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <returns></returns>
        int AddSuggestionContent(SuggestionContent suggestionContent);

        /// <summary>
        /// 修改评论（答复）
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="replyContent"></param>
        void ReplySuggestionContent(int detailId, string replyContent);

        /// <summary>
        /// 分页查找所有建议
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页面尺寸</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">搜索条件</param>
        /// <returns></returns>
        PageData<SuggestionContent> FindAllSuggestionContents(int currentPage, int pageSize,
            IDictionary<SuggestionContentSearchCriteria, object> searchCriteria,
            IList<Sorter<SuggestionContentSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 删除建议
        /// </summary>
        /// <param name="id"></param>
        void DeleteSuggestionContent(int id);

        /// <summary>
        /// 获取客户建议
        /// </summary>
        /// <param name="id">客户建议Id</param>
        /// <returns>客户建议</returns>
        SuggestionContent GetSuggestionContent(int id);

        /// <summary>
        /// 获取客户建议评分详细
        /// </summary>
        /// <param name="suggestionContentId">客户建议Id</param>
        /// <returns>客户建议评分详细</returns>
        IList<SuggestionDetail> GetSuggestionDetails(int suggestionContentId);

        /// <summary>
        /// 获取客户建议附件
        /// </summary>
        /// <param name="suggestionContentId">客户建议Id</param>
        /// <returns>客户建议附件</returns>
        IList<SuggestionAttachment> GetSuggestionAttachments(int suggestionContentId);

        //todo 答复（发送email）

        /// <summary>
        /// 客户建议反馈
        /// </summary>
        /// <param name="suggestionContent">客户建议实体</param>
        /// <returns>客户建议Id</returns>
        int SuggestionFeedback(SuggestionContent suggestionContent);

        #endregion

    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public enum SuggestionContentSearchCriteria
    {
        /// <summary>
        /// 关键词
        /// </summary>
        Keyword,
        /// <summary>
        /// 语言Id
        /// </summary>
        LanguageId,
        /// <summary>
        /// 邮箱
        /// </summary>
        Email,
        /// <summary>
        /// 姓名
        /// </summary>
        Name
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public enum SuggestionContentSorterCriteria
    {
        /// <summary>
        /// 评论时间
        /// </summary>
        DateCreated,
        /// <summary>
        /// 姓名
        /// </summary>
        Name
    }
}
