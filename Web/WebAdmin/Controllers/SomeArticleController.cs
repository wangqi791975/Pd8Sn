using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Article;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Article;

namespace Com.Panduo.Web.Controllers
{
    public class SomeArticleController : Controller
    {

        private int AdminId
        {
            get { return SessionHelper.CurrentAdminUser == null ? 0 : SessionHelper.CurrentAdminUser.AdminId; }
        }

        public ActionResult Index()
        {
            var aticle = ServiceFactory.ArticleService.GetSomeArticle(1, 20, new Dictionary<SomeArticleCriteria, object>(), new List<Sorter<SomeArticleSorterCriteria>>());
           if (!aticle.IsNullOrEmpty()&&aticle.Data.Count>0)
              return View();
           else
              return View("Add");
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            var keyword = Request["keyword"] ?? string.Empty;

            var searchCriteria = new Dictionary<SomeArticleCriteria, object>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(SomeArticleCriteria.Title, keyword);
            }

            var sorterCriteria = new List<Sorter<SomeArticleSorterCriteria>>();
            var aticle = ServiceFactory.ArticleService.GetSomeArticle(page, pageSize, searchCriteria, sorterCriteria);
            return View(aticle);
        }

        [ValidateInput(false)]
        public ActionResult SetArticle()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                var aid = Request["HID_ArticleId"].ParseTo(0);
                var articleTitle = Request["article_title"] ?? string.Empty;
                var articleEnglishTitle = Request["article_english_title"] ?? string.Empty;
                if (articleTitle.IsNullOrEmpty())
                {
                    hashtable["msg"] = "中文标题不能为空";
                    hashtable["error"] = true;
                }
                else if (articleEnglishTitle.IsNullOrEmpty())
                {
                    hashtable["msg"] = "英文标题不能为空";
                    hashtable["error"] = true;
                }
                else
                {
                    var artice = new SomeArticle()
                    {
                        ArticleId = aid,
                        ChineseTitle = articleTitle,
                        EnglishTitle = articleEnglishTitle,
                        CreateId = AdminId,
                        CreateTime = DateTime.Now,
                    };
                    ServiceFactory.ArticleService.SetSomeArticle(artice);
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }

            return Json(hashtable);

        }


        [ValidateInput(false)]
        public ActionResult SetArticleLanguage()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                int aid = Request["ArticleId"].ParseTo(0);
                string[] languageIds = Request["HID_languageId"].Split(',');
                IList<SomeArticleLanguage> someArticleLanguages = new List<SomeArticleLanguage>();
                foreach (var languageId in languageIds)
                {
                    var title = Request["article_language_title_" + languageId] ?? string.Empty;
                    var content = Request["article_language_content_" + languageId] ?? string.Empty;
                       SomeArticleLanguage articleLanguage = new SomeArticleLanguage()
                        {
                            LanguageId = languageId.ParseTo(1),
                            Title = title,
                            ArticleId = aid,
                            CreateTime=DateTime.Now,
                            Status = Request["IsValid_" + languageId].ParseTo<bool>(true),
                            Content = content,
                        };
                        someArticleLanguages.Add(articleLanguage);
                }
                ServiceFactory.ArticleService.SetSomeArticleLanguages(someArticleLanguages);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }

            return Json(hashtable);
        }

        public ActionResult Edit()
        {
            var aid = Request["ArticleId"].ParseTo(0);
            if (aid < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            var article = ServiceFactory.ArticleService.GetSomeArticle(aid);
            if (article.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var list = ServiceFactory.ArticleService.GetSomeArticleLanguage(aid);
            var model = new SomeArticleVo()
            {
                SomeArticle = article,
                SomeArticleLanguage = list,
            };
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddArticle()
        {
            var articleTitle = Request["article_title"] ?? string.Empty;
            var articleEnglishTitle = Request["article_english_title"] ?? string.Empty;
            if (articleTitle.IsNullOrEmpty())
            {
                //hashtable["msg"] = "中文标题不能为空";
                //hashtable["error"] = true;
                return View("Add");
            }
            else if (articleEnglishTitle.IsNullOrEmpty())
            {
                //hashtable["msg"] = "英文标题不能为空";
                //hashtable["error"] = true;
                return View("Add");
            }
            else
            {
                var artice = new SomeArticle()
                {
                    ChineseTitle = articleTitle,
                    EnglishTitle = articleEnglishTitle,
                    CreateId = AdminId,
                    CreateTime = DateTime.Now,
                };
               var aid=ServiceFactory.ArticleService.SetSomeArticle(artice);
                var d = new RouteValueDictionary() { };
                d.Add("ArticleId", aid);
                return RedirectToAction("Edit", "SomeArticle", d);
            }
            
        }
    }
}
