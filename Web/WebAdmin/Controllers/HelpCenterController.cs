using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Help;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.Web.Controllers
{
    public class HelpCenterController : Controller
    {
        //
        // GET: /HelpCenter/

        #region 类别
        public ActionResult Index()
        {
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View();
        }

        public ActionResult List(string keyword)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<HelpCategorySearchCriteria, object>
            {
            };
            var sorterCriteria = new List<Sorter<HelpCategorySorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(HelpCategorySearchCriteria.Keyword, keyword);
            }
            searchCriteria.Add(HelpCategorySearchCriteria.ParentId, 0);

            //获取所有一级类别
            var categoriesNoRoot = ServiceFactory.HelpService.GetSubHelpCategoryNoRoot();
            var categories = ServiceFactory.HelpService.FindHelpCategories(page, pageSize, searchCriteria, sorterCriteria);
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.CategoriesNoRoot = categoriesNoRoot;
            return View(categories);
        }

        public ActionResult Edit(int id)
        {
            HelpCategory category = ServiceFactory.HelpService.GetHelpCategory(id);
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.ParentCategoryId = 0;//父类别
            return View(category);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CategorySave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty }, { "promotionid", string.Empty } };
            try
            {
                var categoryId = form["hidCategoryId"].ParseTo(0);
                var parentCategoryId = form["hidParentCategoryId"].ParseTo(0);
                var isShowParent = form["isShowParent"].ParseTo(0) > 0;
                var categoryName = form["txtCategoryName"];
                var lstLanguages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                var categoryDescriptions = (from language in lstLanguages
                                            where !form[string.Format("{0}{1}", "txtCategoryName_", language.LanguageId)].IsNullOrEmpty()
                                            select new HelpCategoryDescription
                                            {
                                                HelpCategoryId = categoryId,
                                                LanguageId = language.LanguageId,
                                                CategoryName = form[string.Format("{0}{1}", "txtCategoryName_", language.LanguageId)],
                                                Status = form[string.Format("{0}{1}", "Status_", language.LanguageId)].ParseTo(0) > 0,
                                                DateCreated = DateTime.Now,
                                                DateModified = DateTime.Now
                                            }).ToList();
                var helpCategoryId = ServiceFactory.HelpService.SetHelpCategory(new HelpCategory
                {
                    HelpCategoryId = categoryId,
                    ParentId = parentCategoryId,
                    CategoryName = categoryName,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsShowParent = isShowParent,
                    CategoryPath = "",
                    Status = true,
                    Descriptions = categoryDescriptions,
                });
                hashtable["hidCategoryId"] = helpCategoryId;
                hashtable["result"] = BaseController.ActionJsonResult.Success;
                hashtable["getlist"] = true;
            }
            catch (Exception exp)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }
        #endregion

        #region 文章

        public ActionResult ArticleIndex()
        {
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View();
        }

        public ActionResult ArticleList(string keyword)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<ArticleSearchCriteria, object>
            {
            };
            var sorterCriteria = new List<Sorter<ArticleSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(ArticleSearchCriteria.Keyword, keyword);
            }

            /*

             */

            //获取所有一级类别
            //var rootCategories = ServiceFactory.HelpService.GetRootHelpCategoriesByCn();
            var articles = ServiceFactory.HelpService.FindHelpArticlesOfAdmin(page, pageSize, searchCriteria, sorterCriteria);
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();

            //ViewBag.RootCategories = rootCategories;
            return View(articles);
        }

        [HttpGet]
        public ActionResult ArticleEdit()
        {
            var cid = Request["cid"].ParseTo(0);
            var id = Request["id"].ParseTo(0);
            // 取文章信息
            var articleById = new HelpArticle { HelpCategoryId = cid, DateCreated = DateTime.Now, DateModified = DateTime.Now };
            if (id > 0)
            {
                try
                {
                    articleById = ServiceFactory.HelpService.GetHelpArticleById(id);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_HELPARTICLE_NOT_EXIST":
                            ViewBag.ErrorMsg = "文章不存在！";
                            break;
                    }
                }
            }

            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("编辑文章", Url.Content("/HelpCenter/List")));

            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View(articleById);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ArticleSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty }, { "promotionid", string.Empty } };
            try
            {
                var categoryId = form["hidCategoryId"].ParseTo(0);
                if (categoryId <= 0)
                {
                    hashtable["result"] = BaseController.ActionJsonResult.Error;
                    hashtable["msg"] = "请选择文章主题";
                    return Json(hashtable);
                }
                var articleId = form["hidArticleId"].ParseTo(0);
                var title = form["txtArticleTitle"];
                var lstLanguages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                var articleDescriptions = (from language in lstLanguages
                                           where !form[string.Format("{0}{1}", "txtArticleName_", language.LanguageId)].IsNullOrEmpty()
                                           select new HelpArticleDescription
                                               {
                                                   LanguageId = language.LanguageId,
                                                   ArticleId = articleId,
                                                   ArticleName = form[string.Format("{0}{1}", "txtArticleName_", language.LanguageId)],
                                                   ArticleContent = form[string.Format("{0}{1}", "txtArticleContent_", language.LanguageId)],
                                                   Status = form[string.Format("{0}{1}", "Status_", language.LanguageId)].ParseTo(0) > 0,
                                                   IsShowParent = form[string.Format("{0}{1}", "IsShowParent_", language.LanguageId)].ParseTo(0) > 0,
                                                   DateCreated = DateTime.Now,
                                                   DateModified = DateTime.Now
                                               }).ToList();
                var helpArticleId = ServiceFactory.HelpService.SetHelpArticle(new HelpArticle
                {
                    HelpCategoryId = categoryId,
                    ArticleId = articleId,
                    Title = title,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = true,
                    SortOrder = 0,
                    Descriptions = articleDescriptions
                });

                hashtable["hidArticleId"] = helpArticleId;
                hashtable["result"] = BaseController.ActionJsonResult.Success;
            }
            catch (Exception exp)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

        #endregion
    }
}
