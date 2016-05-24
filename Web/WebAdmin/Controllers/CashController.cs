using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.ServiceImpl.SiteConfigure;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class CashController : BaseController
    {
        //
        // GET: /Product/


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = 20;
            string keyWord = Request["keyword"] != null ? Request["keyword"].Trim() : string.Empty;

            IDictionary<CashItemSearchCriteria, object> searchDictionary = new Dictionary<CashItemSearchCriteria, object>();
            if (!keyWord.IsNullOrEmpty())
            {
                searchDictionary.Add(CashItemSearchCriteria.Keyword, keyWord);
            }

            PageData<CashItem> pageData = ServiceFactory.CashService.AdminFindAllCashItems(page, pageSize,
                searchDictionary, null);
            ViewData.Model = pageData;
            return View();
        }

        public ActionResult GetInfo(int id)
        {
            var currencies = ServiceFactory.ConfigureService.GetAllCurrencies();

            ViewBag.Currencies = currencies;
            return View();
        }

        public ActionResult Submit()
        {
            var hashtable = new Hashtable();
            hashtable.Add("getlist", true);
            hashtable.Add("msg", "添加成功!");
            hashtable.Add("error", false);
            try
            {
                string customerEmail = Request["customer_email"].Trim();
                int cashType = Request["cash_type"].ParseTo(-1);
                string currencyCode = Request["currency_code"].Trim();
                decimal amount = Request["amount"].Trim().ParseTo(0M);
                string comment = Request["comment"].Trim();
                bool notifyCustomer = Convert.ToBoolean(Request["notify_customer"]);
                if (customerEmail == string.Empty)
                {
                    hashtable["msg"] = "客户邮箱不能为空!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (cashType < 0)
                {
                    hashtable["msg"] = "请选择类型!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (currencyCode == string.Empty)
                {
                    hashtable["msg"] = "请选择币种!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (amount == 0)
                {
                    hashtable["msg"] = "金额不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (comment == string.Empty)
                {
                    hashtable["msg"] = "请填写备注!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                int adminId = 6;
                var customer = ServiceFactory.CustomerService.GetCustomerByEmail(customerEmail);
                if (customer == null)
                {
                    hashtable["msg"] = "客户不存在!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                var admin = ServiceFactory.AdminUserService.GetAdminUser(adminId);
                if (admin == null)
                {
                    hashtable["msg"] = "管理员不存在!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                var cashSections = ServiceFactory.CashService.GetAllCashSections();
                var currency = ServiceFactory.ConfigureService.GetCurrencyByCode(currencyCode);
                var amountUsd = PageHelper.ExchangeMoneyToUsd(amount, currency);
                if (cashSections.Count > 0 && amountUsd > cashSections[0].AmountMaximum || amountUsd < cashSections[0].AmountMinimum)
                {
                    hashtable["msg"] = string.Format("金额只能在{0}-{1}之间!", cashSections[0].AmountMinimum, cashSections[0].AmountMaximum);
                    hashtable["error"] = true;
                    return Json(hashtable);
                }

                if (EnumHelper.ToEnum<OperationType>(cashType) == OperationType.Income)
                {
                    ServiceFactory.CashService.AdminRecharge(customer.CustomerId, currencyCode, amount, comment, admin.AdminId, notifyCustomer);
                }
                else
                {
                    ServiceFactory.CashService.AdminWithdrawal(customer.CustomerId, currencyCode, amount, comment, admin.AdminId, notifyCustomer);
                }

            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                hashtable["msg"] = ActionJsonResult.Error;
                switch (bussinessException.GetError())
                {
                    case "ERROR_AMOUNT_NOT_ENOUGH":
                        hashtable["msg"] = "客户余额不足!";
                        break;
                }
                return Json(hashtable);
            }
            return Json(hashtable);
        }

    }
}
