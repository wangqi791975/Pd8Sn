using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Article;
using Com.Panduo.Service;
using Com.Panduo.Service.Article;
using Com.Panduo.ServiceImpl.Article.Dao;

namespace Com.Panduo.ServiceImpl.Article
{
    public  class ArticleService : IArticleService
    {
        public ISpreadArticleDao SpreadArticleDao { private get; set; }
        public ISpreadArticleDescriptionDao SpreadArticleDescriptionDao { private get; set; }
        public SomeArticle GetSomeArticle(int articleId)
        {
            var po=SpreadArticleDao.GetObject(articleId);
            return GetSomeArticleVoFromPo(po);
        }

        public int SetSomeArticle(SomeArticle someArticle)
        {
            if (!someArticle.IsNullOrEmpty())
            {
                if (someArticle.ArticleId > 0)
                {
                    var po = SpreadArticleDao.GetObject(someArticle.ArticleId);
                    po.ChineseTitle = someArticle.ChineseTitle;
                    po.EnglishTitle = someArticle.EnglishTitle;
                    SpreadArticleDao.UpdateObject(po);
                    return someArticle.ArticleId;
                }
                else
                {
                    return SpreadArticleDao.AddObject(new SpreadArticlePo()
                    {
                        AdminId = someArticle.CreateId,
                        ChineseTitle=someArticle.ChineseTitle,
                        EnglishTitle = someArticle.EnglishTitle,
                        DateCreated = DateTime.Now,
                        
                    });
                }
            }
            return -1;
        }

        public void SetSomeArticleLanguage(SomeArticleLanguage language)
        {
            throw new NotImplementedException();
        }

        public void SetSomeArticleLanguages(IList<SomeArticleLanguage> language)
        {
            var listUpdate = new List<SpreadArticleDescriptionPo>();
            var listAdd = new List<SpreadArticleDescriptionPo>();
            foreach (var vo in language)
            {
                var po = SpreadArticleDescriptionDao.GetSpreadArticleDescription(vo.ArticleId,vo.LanguageId);
                if (!po.IsNullOrEmpty())
                {
                    po.Content = vo.Content;
                    po.Title = vo.Title;
                    po.Status = vo.Status;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.Title.IsNullOrEmpty())
                    {
                        po = new SpreadArticleDescriptionPo()
                        {
                            Title = vo.Title,
                            ArticleId = vo.ArticleId,
                            Content = vo.Content,
                            LanguageId = vo.LanguageId,
                            DateCreated = DateTime.Now,
                            Status = vo.Status,
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                SpreadArticleDescriptionDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                SpreadArticleDescriptionDao.AddObjects(listAdd);
            }
        }

        public IList<SomeArticleLanguage> GetSomeArticleLanguage(int articleId)
        {
           return SpreadArticleDescriptionDao.GetSpreadArticleDescriptions(articleId).Select(GetSomeArticleLanguageVoFromPo).ToList();
        }

        public SomeArticleLanguage GetSomeArticle(int articleId, int languageId)
        {
            var po= SpreadArticleDescriptionDao.GetSpreadArticleDescription(articleId, languageId);
            return GetSomeArticleLanguageVoFromPo(po);
        }

        public PageData<SomeArticle> GetSomeArticle(int currentPage, int pageSize, IDictionary<SomeArticleCriteria, object> searchDictionary, IList<Sorter<SomeArticleSorterCriteria>> sorterCriteria)
        {
           return  SpreadArticleDao.FindSomeArticle(currentPage, pageSize, searchDictionary, sorterCriteria);
        }

        internal static SomeArticle GetSomeArticleVoFromPo(SpreadArticlePo spreadArticlePo)
        {
            SomeArticle someArticle = null;
            if (!spreadArticlePo.IsNullOrEmpty())
            {
                someArticle = new SomeArticle
                {
                    ArticleId = spreadArticlePo.ArticleId,
                    ChineseTitle=spreadArticlePo.ChineseTitle,
                    EnglishTitle=spreadArticlePo.EnglishTitle,
                    CreateId=spreadArticlePo.AdminId,
                    CreateTime = spreadArticlePo.DateCreated,
                };
            }
            return someArticle;
        }

        internal static SomeArticleLanguage GetSomeArticleLanguageVoFromPo(SpreadArticleDescriptionPo spreadArticleDescriptionPo)
        {
            SomeArticleLanguage someArticleLanguage = null;
            if (!spreadArticleDescriptionPo.IsNullOrEmpty())
            {
                someArticleLanguage = new SomeArticleLanguage
                {
                    ArticleId=spreadArticleDescriptionPo.ArticleId,
                    Title = spreadArticleDescriptionPo.Title,
                    Content=spreadArticleDescriptionPo.Content,
                    LanguageId = spreadArticleDescriptionPo.LanguageId,
                    CreateTime=spreadArticleDescriptionPo.DateCreated,
                    Status=spreadArticleDescriptionPo.Status,
                };
            }
            return someArticleLanguage;
        }

    }
}
