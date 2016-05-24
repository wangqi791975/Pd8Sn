using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.Customer;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.ShoppingCart;
using Panduo.Com.Email.SaveXml;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.Web.Controllers
{
    public class ShoppingCartController : BaseController
    {

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Language = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            var keyword = Request["keyword"] ?? string.Empty;
            var languageId = Request["languageid"].ParseTo(-1);
            var searchCriteria = new Dictionary<CustomerSearchCriteria, object>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(CustomerSearchCriteria.KeyWord, keyword);
                ViewBag.Keyword = keyword;
            }
            if (languageId.ParseTo<int>(-1) != -1)
            {
                searchCriteria.Add(CustomerSearchCriteria.LanguageId, languageId);
            }
            var sorterCriteria = new List<Sorter<CustomerSorterCriteria>>();
            var data = ServiceFactory.CustomerService.FindLongTimeNotUpdatedCustomers(page, pageSize, searchCriteria, sorterCriteria);
            return View(data);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            int customerId = Request["customerId"].ParseTo<int>(0);


            var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);
            if (customer.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            ViewBag.CustomerEmail = customer.Email;
            var sorter = new List<Sorter<ShoppingCartSorterCriteria>>
			{
				new Sorter<ShoppingCartSorterCriteria> {Key = ShoppingCartSorterCriteria.AddedTimeNewToOld, IsAsc = true}
			};
            var search = new Dictionary<ShoppingCartSearchCriteria, object>()
			{
				{ShoppingCartSearchCriteria.LanguageId, ServiceFactory.ConfigureService.EnglishLangId}
			};
            var pageData = ServiceFactory.ShoppingCartService.FindVShoppingCartItems(customerId, page, pageSize, search, sorter);
            var currency = ServiceFactory.ConfigureService.GetCurrencyByCode(ServiceFactory.ConfigureService.CURRENCY_CODE_USD);
            var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(customerId,
            ServiceFactory.ConfigureService.EnglishLangId, currency.CurrencyId, 0);
            ViewBag.Language = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.GrandTotal = shoppingCartSummary.GrandTotal;
            ViewBag.VolumeWeight = shoppingCartSummary.VolumeWeight.ToString("N") + " g";
            ViewBag.Weight = shoppingCartSummary.ShippingWeight.ToString("N") + " g";
            return View(pageData);
        }


        /// <summary>
        /// 发邮件展示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMailView()
        {
            ViewBag.Language = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.CustomerEmail = Request["CustomerEmail"] ?? string.Empty; 
            return View();
        }

        /// <summary>
        /// 选择语种Id加载对应的模板
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMailContent()
        {
            var languageId = Request["languageId"].ParseTo(1);
            string customerEmail = Request["CustomerEmail"] ?? string.Empty;
            if (languageId > 0)
            {
                return RedirectToAction("SendMail", new { languageId = languageId, customerEmail = customerEmail });
            }
            else
            {
                return Content("请选择具体的语种");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMail(int languageId, string customerEmail)
        {
            var parm = new Dictionary<string, string>();
            parm.Add("name", SessionHelper.CurrentAdminUser.IsNullOrEmpty()?SessionHelper.CurrentAdminUser.Name:"");
            var emailType = MailType.ShoppingCartNotUpdate;
            var tplmailContent = ServiceFactory.MailService.AdminReadTemple(emailType, parm, languageId);
            var title = MailTemplateHelper.GetTagValue(tplmailContent, "<!--<tilte>(.*)</tilte>-->");//获取标题
            tplmailContent = MailTemplateHelper.RemoveTag(tplmailContent, "<!--<tilte>(.*)</tilte>-->");//干掉标题

            var model = new MailVo()
            {
                From = "notification@8seasons.com",
                To = customerEmail,
                Title = title,
                MailContent = tplmailContent,
            };
            ViewBag.languageId = languageId;
            return View(model);
        }

        [ValidateInput(false)]
        public JsonResult CreateSendMail()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var languageId = Request["languageid"].ParseTo(1);
            var mailfrom = Request["mailfrom"]??string.Empty;
            var mailto = Request["mailto"]??string.Empty;
            var mailtitle = Request["mailtitle"]??string.Empty;
            var mailcontent = Request["mailcontent"]??string.Empty;
            var list = mailto.Split(";").ToList();
            string msg = "";
            foreach (var e in list)
            {
              var c=  ServiceFactory.CustomerService.GetCustomerByEmail(e);
                if (c.IsNullOrEmpty())
                {
                    msg += e+" ";
                }
            }
            if (msg.Length >1)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "邮箱" + msg + "在系统中不存在";
            }
            else
            {
                try
                {
                    var mailId = Guid.NewGuid().ToString("N");
                    var xmlObj = new EmailXmlData
                    {
                        Type = MailType.ShoppingCartNotUpdate,
                        MailId = mailId,
                        Title = mailtitle,
                        MailContent = mailcontent,
                        SendDateTime = DateTime.Now,
                        Sender = mailfrom,
                        RecipientList = list,
                        LanguageId = languageId,
                    };
                    EmailService.SerializerXml(xmlObj);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "":
                            break;
                    }
                }
            }
            return Json(hashtable);
        }
    }
}
