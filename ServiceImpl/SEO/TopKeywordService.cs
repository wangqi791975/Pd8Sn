using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.SEO;
using Com.Panduo.Service;
using Com.Panduo.Service.SEO;
using Com.Panduo.ServiceImpl.SEO.Dao;

namespace Com.Panduo.ServiceImpl.SEO
{
    public class TopKeywordService : ITopKeywordService
    {
        public ITopKeywordDao TopKeywordDao { private get; set; }
        public ITopKeywordSubjectDao TopKeywordSubjectDao { private get; set; }

        public string ERROR_SUBJECT_NOT_EXIST
        {
            get { return "ERROR_SUBJECT_NOT_EXIST"; }
        }

        public string ERROR_KEYWORD_NOT_EXIST
        {
            get { return "ERROR_KEYWORD_NOT_EXIST"; }
        }

        public string ERROR_SUBJECT_CAN_NOT_DUPLICATE
        {
            get { return "ERROR_SUBJECT_CAN_NOT_DUPLICATE"; }
        }

        public string ERROR_SUBJECT_CAN_NOT_DELETE
        {
            get { return "ERROR_SUBJECT_CAN_NOT_DELETE"; }
        }

        public string ERROR_KEYWORD_CAN_NOT_DUPLICATE
        {
            get { return "ERROR_KEYWORD_CAN_NOT_DUPLICATE"; }
        }

        public int AddKeywordSubject(TopKeywordSubject topKeywordSubject)
        {
            var topKeywordSubjectPo = TopKeywordSubjectDao.GeTopKeywordSubject(topKeywordSubject.TopKeywordSubjectName);
            if (!topKeywordSubjectPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_SUBJECT_CAN_NOT_DUPLICATE);
            }
            return TopKeywordSubjectDao.AddObject(GetTopKeywordSubjectPoFromVo(topKeywordSubject));
        }

        public void UpdateKeywordSubject(TopKeywordSubject topKeywordSubject)
        {
            int topKeywordSubjectId = TopKeywordSubjectDao.GetTopKeywordSubjectId(topKeywordSubject.TopKeywordSubjectId);
            if (topKeywordSubjectId == 0)
            {
                throw new BussinessException(ERROR_SUBJECT_NOT_EXIST);
            }
            topKeywordSubjectId = TopKeywordSubjectDao.GetTopKeywordSubjectId(topKeywordSubject.TopKeywordSubjectName);
            if (topKeywordSubjectId != 0)
            {
                throw new BussinessException(ERROR_SUBJECT_CAN_NOT_DUPLICATE);
            }
            TopKeywordSubjectDao.UpdateObject(GetTopKeywordSubjectPoFromVo(topKeywordSubject));
        }

        public void DeleteKeywordSubject(int keywordSubjectId)
        {
            TopKeywordSubjectDao.DeleteObjectById(keywordSubjectId);
        }

        public TopKeywordSubject GetKeywordSubjectById(int keywordSubjectId)
        {
            return GetTopKeywordSubjectVoFromPo(TopKeywordSubjectDao.GetObject(keywordSubjectId));
        }

        public PageData<TopKeywordSubject> FindKeywordSubjects(int currentPage, int pageSize, IDictionary<TopKeywordSubjectSearchCriteria, object> searchCriteria, IList<Sorter<TopKeywordSubjectSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT S FROM TopKeywordSubjectPo S");
            if (!searchCriteria.IsNullOrEmpty())
            {
                //todo 查询条件
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //todo 排序条件
            }
            var pageDataPo = TopKeywordSubjectDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql,
                hqlHelper.ParamMap);
            var pageDataVo = new PageData<TopKeywordSubject>();
            var voList = pageDataPo.Data.Select(GetTopKeywordSubjectVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public int AddKeyword(TopKeyword topKeyword)
        {
            var topKeywordPo = TopKeywordDao.GeTopKeyword(topKeyword.TopKeywordName);
            if (!topKeywordPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_KEYWORD_CAN_NOT_DUPLICATE);
            }
            return TopKeywordDao.AddObject(GetTopKeywordPoFromVo(topKeyword));
        }

        public void UpdateKeyword(TopKeyword topKeyword)
        {
            var topKeywordPo = TopKeywordDao.GeTopKeyword(topKeyword.TopKeywordName);
            if (!topKeywordPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_KEYWORD_CAN_NOT_DUPLICATE);
            }
            TopKeywordDao.UpdateObject(GetTopKeywordPoFromVo(topKeyword));
        }

        public void DeleteKeyword(int keywordId)
        {
            var topKeywordPo = TopKeywordDao.GetObject(keywordId);
            if (topKeywordPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_KEYWORD_NOT_EXIST);
            }
            TopKeywordDao.DeleteObjectById(keywordId);
        }

        public void DeleteKeywordBySubjectId(int subjectId)
        {
            try
            {
                if (!TopKeywordSubjectDao.ExistObject("TopKeywordSubjectId", subjectId))
                {
                    throw new BussinessException(ERROR_SUBJECT_NOT_EXIST);
                }
                TopKeywordDao.DeleteKeywordBySubjectId(subjectId);
            }
            catch (System.Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        public TopKeyword GetKeyword(int keywordId)
        {
            return GetTopKeywordVoFromPo(TopKeywordDao.GetObject(keywordId));
        }

        public IList<TopKeyword> GetKeywordsBySubjectId(int subjectId)
        {
            return TopKeywordDao.GetTopKeywords(subjectId).Select(GetTopKeywordVoFromPo).ToList();
        }

        public PageData<TopKeyword> FindKeywordsBySubjectId(int currentPage, int pageSize, int subjectId, IDictionary<TopKeywordSearchCriteria, object> searchCriteria,
            IList<Sorter<TopKeywordSorterCriteria>> sorterCriteria)
        {
            try
            {
                var hqlHelper = new HqlHelper("SELECT S FROM TopKeywordPo S");
                hqlHelper.AddWhere("TopKeywordSubjectId", HqlOperator.Eq, "SubjectId", subjectId);
                if (!searchCriteria.IsNullOrEmpty())
                {
                    //todo 查询条件
                }
                if (!sorterCriteria.IsNullOrEmpty())
                {
                    //todo 排序条件
                }
                var pageDataPo = TopKeywordDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql,
                    hqlHelper.ParamMap);
                var pageDataVo = new PageData<TopKeyword>();
                var voList = pageDataPo.Data.Select(GetTopKeywordVoFromPo).ToList();

                pageDataVo.Pager = pageDataPo.Pager;
                pageDataVo.Data = voList;
                return pageDataVo;
            }
            catch (Exception ex)
            {
                throw new BussinessException("System error.");
            }
        }

        #region 辅助方法

        internal static TopKeyword GetTopKeywordVoFromPo(TopKeywordPo topKeywordPo)
        {
            TopKeyword topKeyword = null;
            if (!topKeywordPo.IsNullOrEmpty())
            {
                topKeyword = new TopKeyword
                {
                    TopKeyworId = topKeywordPo.TopKeywordId,
                    TopKeywordSubjectId = topKeywordPo.TopKeywordSubjectId,
                    TopKeywordName = topKeywordPo.Name
                };
            }
            return topKeyword;
        }

        internal static TopKeywordPo GetTopKeywordPoFromVo(TopKeyword topKeyword)
        {
            TopKeywordPo topKeywordPo = null;
            if (!topKeyword.IsNullOrEmpty())
            {
                topKeywordPo = new TopKeywordPo
                {
                    TopKeywordId = topKeyword.TopKeyworId,
                    TopKeywordSubjectId = topKeyword.TopKeywordSubjectId,
                    Name = topKeyword.TopKeywordName
                };
            }
            return topKeywordPo;
        }

        internal static TopKeywordSubject GetTopKeywordSubjectVoFromPo(TopKeywordSubjectPo topKeywordSubjectPo)
        {
            TopKeywordSubject topKeywordSubject = null;
            if (!topKeywordSubjectPo.IsNullOrEmpty())
            {
                topKeywordSubject = new TopKeywordSubject
                {
                    TopKeywordSubjectId = topKeywordSubjectPo.TopKeywordSubjectId,
                    LanguageId = topKeywordSubjectPo.LanguageId,
                    TopKeywordSubjectName = topKeywordSubjectPo.Name
                };
            }
            return topKeywordSubject;
        }

        internal static TopKeywordSubjectPo GetTopKeywordSubjectPoFromVo(TopKeywordSubject topKeywordSubject)
        {
            TopKeywordSubjectPo topKeywordSubjectPo = null;
            if (!topKeywordSubject.IsNullOrEmpty())
            {
                topKeywordSubjectPo = new TopKeywordSubjectPo
                {
                    TopKeywordSubjectId = topKeywordSubject.TopKeywordSubjectId,
                    LanguageId = topKeywordSubject.LanguageId,
                    Name = topKeywordSubject.TopKeywordSubjectName
                };
            }
            return topKeywordSubjectPo;
        }
        #endregion
    }
}