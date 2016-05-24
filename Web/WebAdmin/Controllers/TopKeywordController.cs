using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.SEO;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.SEO.TopKeyword;

namespace Com.Panduo.Web.Controllers
{
    public class TopKeywordController : BaseController
    {
        #region TopKeyword
        public ActionResult TopKeyword()
        {
            var subjectId = Request["SubjectId"].ParseTo(0);
            if (subjectId > 0)
            {
                TopKeywordSubject subject = ServiceFactory.TopKeywordService.GetKeywordSubjectById(subjectId);
                ViewBag.TopKeywordSubjectId = subject.IsNullOrEmpty() ? 0 : subject.TopKeywordSubjectId;
                ViewBag.TopKeywordSubjectName = subject.IsNullOrEmpty() ? "" : subject.TopKeywordSubjectName;
                return View("~/Views/SEO/TopKeyword/TopKeyword.cshtml");
            }
            return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
        }

        public ActionResult TopKeywordList(int id)
        {
            var subjectId = id;
            int page = Request["page"].ParseTo(1);
            int pageSize = 10;
            string keyWord = Request["keyword"] ?? string.Empty;

            ViewBag.TopKeywordVos = new List<TopKeywordVo>();
            ViewData.Model = new PageData<TopKeywordVo>();
            if (subjectId > 0)
            {
                TopKeywordSubject subject = ServiceFactory.TopKeywordService.GetKeywordSubjectById(subjectId);
                var topKeywords = ServiceFactory.TopKeywordService.FindKeywordsBySubjectId(page, pageSize, subjectId, null, null);
                ViewBag.TopKeywordVos = topKeywords.Data.Select(x => new TopKeywordVo
                {
                    TopKeyworId = x.TopKeyworId,
                    TopKeywordName = x.TopKeywordName,
                    TopKeywordSubjectId = x.TopKeywordSubjectId,
                    TopKeywordSubjectName = subject.IsNullOrEmpty() ? string.Empty : subject.TopKeywordSubjectName
                }).ToList();
                ViewData.Model = topKeywords;
            }
            return View("~/Views/SEO/TopKeyword/TopKeywordList.cshtml");
        }

        public ActionResult TopKeywordInfo(int id)
        {
            if (id > 0)
            {
                var topKeyword = ServiceFactory.TopKeywordService.GetKeyword(id);
                TopKeywordSubject subject = ServiceFactory.TopKeywordService.GetKeywordSubjectById(topKeyword.TopKeywordSubjectId);
                ViewBag.Item = new TopKeywordVo
                {
                    TopKeyworId = topKeyword.TopKeyworId,
                    TopKeywordName = topKeyword.TopKeywordName,
                    TopKeywordSubjectId = topKeyword.TopKeywordSubjectId,
                    TopKeywordSubjectName = subject.IsNullOrEmpty() ? string.Empty : subject.TopKeywordSubjectName
                };
                return View("~/Views/SEO/TopKeyword/TopKeywordInfo.cshtml");
            }
            return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
        }

        public JsonResult Edit(FormCollection form)
        {
            var hashtable = new Hashtable();
            hashtable.Add("Result", ActionJsonResult.Failing);
            hashtable.Add("Msg", string.Empty);

            try
            {
                var topKeywordId = 0;
                var strTopKeywordId = form["txtTopKeywordId"] as string;
                var topKeywordSubjectId = form["hidTopKeywordSubjectId"] as string;
                var topKeywordName = form["txtTopKeywordName"] as string;
                if (strTopKeywordId.IsNullOrEmpty() || strTopKeywordId == "0")
                {
                    topKeywordId = ServiceFactory.TopKeywordService.AddKeyword(new TopKeyword
                    {
                        TopKeywordSubjectId = topKeywordSubjectId.ParseTo(0),
                        TopKeywordName = topKeywordName
                    });
                }
                else
                {
                    topKeywordId = strTopKeywordId.ParseTo(0);
                    ServiceFactory.TopKeywordService.UpdateKeyword(new TopKeyword
                    {
                        TopKeyworId = topKeywordId,
                        TopKeywordSubjectId = topKeywordSubjectId.ParseTo(0),
                        TopKeywordName = topKeywordName

                    });
                }
                hashtable["Result"] = ActionJsonResult.PageRefresh;
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }

        public JsonResult DeleteTopKeyword(FormCollection form)
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            hashtable.Add("getlist", true);
            hashtable.Add("url", "/TopKeyword/TopKeywordList");
            try
            {
                var topKeywordIds = form["ids"] as string;
                if (!topKeywordIds.IsNullOrEmpty())
                {
                    foreach (var id in topKeywordIds.Split(','))
                    {
                        ServiceFactory.TopKeywordService.DeleteKeyword(id.ParseTo(0));
                    }
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable.Add("error", true);
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }
        #endregion

        #region TopKeywordSubject
        public ActionResult TopKeywordSubject()
        {
            return View("~/Views/SEO/TopKeyword/TopKeywordSubject.cshtml");
        }

        public ActionResult TopKeywordSubjectList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = 10;
            string keyWord = Request["keyword"] ?? string.Empty;
            var topKeywordSubjects = ServiceFactory.TopKeywordService.FindKeywordSubjects(page, pageSize, null, null);
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();
            ViewData.Model = topKeywordSubjects;
            ViewBag.TopKeywordSubjectVos = topKeywordSubjects.Data.Select(x => TopKeywordSubjectConvertToVo(x, languages)).ToList();

            return View("~/Views/SEO/TopKeyword/TopKeywordSubjectList.cshtml");
        }

        private static TopKeywordSubjectVo TopKeywordSubjectConvertToVo(TopKeywordSubject subject, List<Language> languages)
        {
            if (subject.IsNullOrEmpty())
                return null;
            var language = languages.Find(x => x.LanguageId == subject.LanguageId);
            return new TopKeywordSubjectVo
            {
                TopKeywordSubjectId = subject.TopKeywordSubjectId,
                LanguageId = subject.LanguageId,
                LanguageName = language.IsNullOrEmpty() ? string.Empty : language.LanguageName,
                TopKeywordSubjectName = subject.TopKeywordSubjectName,
            };
        }

        public ActionResult TopKeywordSubjectInfo(int id)
        {
            //var topKeywordSubjectId = Request["Id"].ParseTo(0);
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();
            if (id > 0)
            {
                //Update
                var topKeywordSubject = ServiceFactory.TopKeywordService.GetKeywordSubjectById(id);
                ViewBag.Item = TopKeywordSubjectConvertToVo(topKeywordSubject, languages);
                ViewData["ddlLanguage"] = new SelectList(languages, "LanguageId", "ChineseName", topKeywordSubject.LanguageId);//语种
            }
            else
            {
                //Add
                ViewBag.Item = new TopKeywordSubjectVo();
                ViewData["ddlLanguage"] = new SelectList(languages, "LanguageId", "ChineseName");//语种
            }

            return View("~/Views/SEO/TopKeyword/TopKeywordSubjectInfo.cshtml");
        }

        public JsonResult SubjectEdit(FormCollection form)
        {
            var hashtable = new Hashtable();
            hashtable.Add("Result", ActionJsonResult.Failing);
            hashtable.Add("Msg", string.Empty);

            try
            {
                var topKeywordSubjectId = 0;
                var strTopKeywordSubjectId = form["txtTopKeywordSubjectId"] as string;
                var languageId = form["ddlLanguage"] as string;
                var topKeywordSubjectName = form["txtTopKeywordSubjectName"] as string;

                if (strTopKeywordSubjectId.IsNullOrEmpty() || strTopKeywordSubjectId == "0")
                {
                    topKeywordSubjectId = ServiceFactory.TopKeywordService.AddKeywordSubject(new TopKeywordSubject
                    {
                        LanguageId = languageId.ParseTo(0),
                        TopKeywordSubjectName = topKeywordSubjectName
                    });
                }
                else
                {
                    topKeywordSubjectId = strTopKeywordSubjectId.ParseTo(0);
                    ServiceFactory.TopKeywordService.UpdateKeywordSubject(new TopKeywordSubject
                    {
                        TopKeywordSubjectId = topKeywordSubjectId,
                        LanguageId = languageId.ParseTo(0),
                        TopKeywordSubjectName = topKeywordSubjectName

                    });
                }
                hashtable["Result"] = ActionJsonResult.PageRefresh;
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }

        public JsonResult DeleteSubject(FormCollection form)
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            hashtable.Add("getlist", true);
            hashtable.Add("url", "/TopKeyword/TopKeywordSubjectList");
            try
            {
                var topKeywordIds = form["ids"] as string;
                if (!topKeywordIds.IsNullOrEmpty())
                {
                    foreach (var id in topKeywordIds.Split(','))
                    {
                        ServiceFactory.TopKeywordService.DeleteKeywordBySubjectId(id.ParseTo(0));
                        ServiceFactory.TopKeywordService.DeleteKeywordSubject(id.ParseTo(0));
                    }
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }
        #endregion

    }
}
