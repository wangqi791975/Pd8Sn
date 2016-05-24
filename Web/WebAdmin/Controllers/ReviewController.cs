using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Review;
using Com.Panduo.Service.Suggestion;
using Com.Panduo.Service.SystemMail;
using Com.Panduo.Web.Common;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.Web.Controllers
{
    public class ReviewController : BaseController
    {
        #region 综合评价管理
        [HttpGet]
        public ActionResult Testimonials()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TestimonialsList(string keyword, int? lang, CustomerReviewSorterCriteria? sorter, bool? isasc)
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<CustomerReviewSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<CustomerReviewSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(CustomerReviewSearchCriteria.Keyword, keyword);
            }
            if (!keyword.IsNullOrEmpty() && keyword.Length <= 2 && keyword.Length >= 1)
            {
                searchCriteria.Add(CustomerReviewSearchCriteria.LanguageId, -1);
            }
            else
                if (lang.HasValue)
                {
                    if (lang.Value != 0)
                        searchCriteria.Add(CustomerReviewSearchCriteria.LanguageId, lang.Value);
                }
            if (sorter.HasValue && isasc.HasValue)
            {
                var sort = new Sorter<CustomerReviewSorterCriteria>(sorter.Value, isasc.Value);
                sorterCriteria.Add(sort);
            }

            var webSiteReviews = ServiceFactory.ReviewService.FindCustomerWebSiteReviewsByType(page, pageSize, ReviewType.Web, searchCriteria, sorterCriteria);
            return View(webSiteReviews);
        }

        [HttpGet]
        public ActionResult TestimonialsEdit(int id)
        {
            ViewBag.WebSiteReview = ServiceFactory.ReviewService.GetReviewWebsiteCustomerViewById(id);
            return View();
        }

        [HttpPost]
        public ActionResult TestimonialsEdit(int id, int recommend, string replycontent)
        {
            var admin = SessionHelper.CurrentAdminUser;
            ServiceFactory.ReviewService.ReplyWebSiteReview(id, admin.AdminId, DateTime.Now, replycontent);
            ServiceFactory.ReviewService.UpdateIsRecommend(id, Convert.ToBoolean(recommend));
            var message = new Dictionary<string, object> { { "TestimonialsList", true }, { "msg", "修改成功!" }, { "getlist", true } };
            //发送邮件
            var customer = ServiceFactory.CustomerService.GetCustomerById(ServiceFactory.ReviewService.GetWebSiteReviewById(id).CustomerId);
            if (!customer.IsNullOrEmpty())
                ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                {
                    Attachment = replycontent,
                    DateAdded = DateTime.Now,
                    DateCreatedXml = DateTime.Now,
                    EmailType = MailType.TestimonialReply,
                    LanguageId = customer.RegisterLanguageId.HasValue ? customer.RegisterLanguageId.Value : ServiceFactory.ConfigureService.EnglishLangId,
                    HasAttachment = false,
                    IsCreatedXml = false,
                    KeyNo = customer.CustomerId.ToString(CultureInfo.InvariantCulture)
                });

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool UpdateTestimonialsValid(int id, bool isvalid)
        {
            try
            {
                ServiceFactory.ReviewService.UpdateWebSiteReviewStatus(id, !isvalid);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region 产品评价管理

        [HttpGet]
        public ActionResult ProductReview()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProductReviewList(string keyword, int? lang, CustomerReviewSorterCriteria? sorter, bool? isasc)
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<CustomerReviewSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<CustomerReviewSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(CustomerReviewSearchCriteria.Keyword, keyword);
            }
            if (!keyword.IsNullOrEmpty() && keyword.Length <= 2 && keyword.Length >= 1)
            {
                searchCriteria.Add(CustomerReviewSearchCriteria.LanguageId, -1);
            }
            else
                if (lang.HasValue)
                {
                    if (lang.Value != 0)
                        searchCriteria.Add(CustomerReviewSearchCriteria.LanguageId, lang.Value);
                }
            if (sorter.HasValue && isasc.HasValue)
            {
                var sort = new Sorter<CustomerReviewSorterCriteria>(sorter.Value, isasc.Value);
                sorterCriteria.Add(sort);
            }

            var productReviews = ServiceFactory.ReviewService.FindCustomerProductReviewsByProductId(page, pageSize, searchCriteria, sorterCriteria, null);
            return View(productReviews);
        }

        [HttpGet]
        public ActionResult ProductReviewEdit(int id)
        {
            ViewBag.ProductReview = ServiceFactory.ReviewService.GetReviewProductCustomerViewById(id);
            return View();
        }

        [HttpPost]
        public ActionResult ProductReviewEdit(int id, string replycontent)
        {
            var admin = SessionHelper.CurrentAdminUser;
            ServiceFactory.ReviewService.ReplyProductReview(id, admin.AdminId, DateTime.Now, replycontent);
            var message = new Dictionary<string, object> { { "ProductReviewList", true }, { "msg", "修改成功!" } };
            //发送邮件
            var customer = ServiceFactory.CustomerService.GetCustomerById(ServiceFactory.ReviewService.GetProductReviewById(id).CustomerId);
            if (!customer.IsNullOrEmpty())
                ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                {
                    Attachment = replycontent,
                    DateAdded = DateTime.Now,
                    DateCreatedXml = DateTime.Now,
                    EmailType = MailType.ReviewReply,
                    LanguageId = customer.RegisterLanguageId.HasValue ? customer.RegisterLanguageId.Value : ServiceFactory.ConfigureService.EnglishLangId,
                    HasAttachment = false,
                    IsCreatedXml = false,
                    KeyNo = customer.CustomerId.ToString(CultureInfo.InvariantCulture)
                });
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool UpdateProductValid(int id, bool isvalid)
        {
            try
            {
                ServiceFactory.ReviewService.UpdateProductReviewStatus(id, !isvalid);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Suggestion
        [HttpGet]
        public ActionResult Suggestion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SuggestionList(string keyword, int? lang, SuggestionContentSorterCriteria? sorter, bool? isasc, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<SuggestionContentSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<SuggestionContentSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(SuggestionContentSearchCriteria.Keyword, keyword);
            }
            if (!keyword.IsNullOrEmpty() && keyword.Length <= 2 && keyword.Length >= 1)
            {
                searchCriteria.Add(SuggestionContentSearchCriteria.LanguageId, -1);
            }
            else
                if (lang.HasValue)
                {
                    if (lang.Value != 0)
                        searchCriteria.Add(SuggestionContentSearchCriteria.LanguageId, lang.Value);
                }
            if (sorter.HasValue && isasc.HasValue)
            {
                var sort = new Sorter<SuggestionContentSorterCriteria>(sorter.Value, isasc.Value);
                sorterCriteria.Add(sort);
            }
            var suggestionContents = ServiceFactory.SuggestionService.FindAllSuggestionContents(page, pageSize, searchCriteria, sorterCriteria);
            return View(suggestionContents);
        }

        [HttpPost]
        public JsonResult DeleteSuggestion(FormCollection form)
        {
            var hashtable = new Hashtable
            {
                {"error", false},
                {"msg", string.Empty},
                {"getlist", true},
                {"url", "/Review/SuggestionList"}
            };
            try
            {
                var ids = form["ids"];
                foreach (var id in ids.Split(','))
                {
                    ServiceFactory.SuggestionService.DeleteSuggestionContent(Convert.ToInt32(id));
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_SUGGESTION_CONTENT_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "该建议不存在";
                        hashtable["getlist"] = false;
                        break;
                }
            }

            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult SuggestionDetail(int id, int page = 1)
        {
            ViewBag.Page = page;
            var suggestionContent = ServiceFactory.SuggestionService.GetSuggestionContent(id);
            return View(suggestionContent);
        }

        [HttpPost]
        public ActionResult SuggestionSendEmail(int detailId, string replyContent)
        {
            var hashtable = new Hashtable
            {
                {"error", false},
                {"msg", string.Empty}
            };

            ServiceFactory.SuggestionService.ReplySuggestionContent(detailId, replyContent);
            var suggestionContent = ServiceFactory.SuggestionService.GetSuggestionContent(detailId);
            var customer = ServiceFactory.CustomerService.GetCustomerByEmail(suggestionContent.Email);
            if (!customer.IsNullOrEmpty())
                ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                {
                    Attachment = "",
                    DateAdded = DateTime.Now,
                    DateCreatedXml = DateTime.Now,
                    EmailType = MailType.SuggestionReply,
                    LanguageId = suggestionContent.LanguageId,
                    HasAttachment = false,
                    IsCreatedXml = false,
                    KeyNo = customer.CustomerId.ToString(CultureInfo.InvariantCulture)
                });
            else
            {
                hashtable["error"] = true;
                hashtable["msg"] = "客户不存在！";
            }
            return Json(hashtable);
        }
        #endregion
    }
}
