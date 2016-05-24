using System.Collections;
using System.Collections.Generic;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// 顶部关键词服务接口
    /// </summary>
    public interface ITopKeywordService
    {
        #region 常量
        /// <summary>
        /// 主题不存在
        /// </summary>
        string ERROR_SUBJECT_NOT_EXIST { get; }

        /// <summary>
        /// 关键词不存在
        /// </summary>
        string ERROR_KEYWORD_NOT_EXIST { get; }

        /// <summary>
        /// 相同语种下主题不能重复
        /// </summary>
        string ERROR_SUBJECT_CAN_NOT_DUPLICATE { get; }

        /// <summary>
        /// 主题不能删除
        /// </summary>
        string ERROR_SUBJECT_CAN_NOT_DELETE { get; }

        /// <summary>
        /// 相同语种下关键词不能重复
        /// </summary>
        string ERROR_KEYWORD_CAN_NOT_DUPLICATE { get; }
        #endregion

        #region 方法

        #region 主题

        /// <summary>
        /// 添加主题
        /// </summary>
        /// <param name="topKeywordSubject">主题实体</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_SUBJECT_CAN_NOT_DUPLICATE:同种语言下主题不能重复</value>
        /// </exception>
        /// <returns>新建主题Id</returns>
        int AddKeywordSubject(TopKeywordSubject topKeywordSubject);

        /// <summary>
        /// 修改主题
        /// 1.忽略主题Id修改
        /// </summary>
        /// <param name="topKeywordSubject">主题实体</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_SUBJECT_NOT_EXIST:主题不存在</value>
        ///     <value>ERROR_SUBJECT_CAN_NOT_DUPLICATE:同种语言下主题不能重复</value>
        /// </exception>
        void UpdateKeywordSubject(TopKeywordSubject topKeywordSubject);

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="keywordSubjectId">主题Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_SUBJECT_NOT_EXIST:主题不存在</value>
        ///     <value>ERROR_SUBJECT_CAN_NOT_DELETE:主题下面有关键词存在不能删除</value>
        /// </exception>
        void DeleteKeywordSubject(int keywordSubjectId);

        /// <summary>
        /// 通过主题Id获取主题
        /// </summary>
        /// <param name="keywordSubjectId">主题Id</param>
        /// <returns>要获取的主题</returns>
        TopKeywordSubject GetKeywordSubjectById(int keywordSubjectId);


        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>所有主题集合</returns>
        PageData<TopKeywordSubject> FindKeywordSubjects(int currentPage, int pageSize, IDictionary<TopKeywordSubjectSearchCriteria, object> searchCriteria, IList<Sorter<TopKeywordSubjectSorterCriteria>> sorterCriteria);

        #endregion

        #region 关键词

        /// <summary>
        /// 添加关键词
        /// </summary>
        /// <param name="topKeyword">关键词实体</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_KEYWORD_CAN_NOT_DUPLICATE:相同语种下关键词不能重复</value>
        /// </exception>
        /// <returns>新添加的关键词Id</returns>
        int AddKeyword(TopKeyword topKeyword);

        /// <summary>
        /// 修改关键词
        /// 1.忽略关键词Id和主题Id修改
        /// </summary>
        /// <param name="topKeyword">关键词实体</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_KEYWORD_NOT_EXIST:关键词不存在</value>
        ///     <value>ERROR_KEYWORD_CAN_NOT_DUPLICATE:相同语种下关键词不能重复</value>
        /// </exception>
        void UpdateKeyword(TopKeyword topKeyword);

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="keywordId">关键词Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_KEYWORD_NOT_EXIST:关键词不存在</value>
        /// </exception>
        void DeleteKeyword(int keywordId);

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="subjectId">关键词主题Id</param>
        void DeleteKeywordBySubjectId(int subjectId);

        /// <summary>
        /// 通过关键词Id获取关键词
        /// </summary>
        /// <param name="keywordId">关键词Id</param>
        /// <returns>关键词实体</returns>
        TopKeyword GetKeyword(int keywordId);

        /// <summary>
        /// 通过主题Id获取对应的关键词
        /// </summary>
        /// <param name="subjectId">主题Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_SUBJECT_NOT_EXIST:主题不存在</value>
        /// </exception>
        /// <returns>关键词集合</returns>
        IList<TopKeyword> GetKeywordsBySubjectId(int subjectId);

        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="subjectId">主题Id</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>所有主题集合</returns>
        PageData<TopKeyword> FindKeywordsBySubjectId(int currentPage, int pageSize, int subjectId, IDictionary<TopKeywordSearchCriteria, object> searchCriteria, IList<Sorter<TopKeywordSorterCriteria>> sorterCriteria);

        #endregion

        #endregion
    }

    /// <summary>
    /// 关键词主题查询条件
    /// </summary>
    public enum TopKeywordSearchCriteria
    {
        //查询条件
    }

    /// <summary>
    /// 关键词主题排序条件
    /// </summary>
    public enum TopKeywordSorterCriteria
    {
        //排序条件
    }

    /// <summary>
    /// 关键词主题查询条件
    /// </summary>
    public enum TopKeywordSubjectSearchCriteria
    {
        //查询条件
    }

    /// <summary>
    /// 关键词主题排序条件
    /// </summary>
    public enum TopKeywordSubjectSorterCriteria
    {
        //排序条件
    }
}