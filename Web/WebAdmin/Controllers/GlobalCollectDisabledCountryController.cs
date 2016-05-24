using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class GlobalCollectDisabledCountryController : BaseController
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

            PageData<GlobalCollectDisabledCountry> pageData = ServiceFactory.PaymentService.FindGlobalCollectDisabledCountries(page, pageSize,
                null, null);
            ViewData.Model = pageData;
            return View();
        }

        public ActionResult GetInfo(int id)
        {
            var countries = ServiceFactory.ConfigureService.GetAllCountry();

            ViewData.Model = countries;
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
                int countryId = Request["country_id"].ParseTo(0);
                if (countryId <= 0)
                {
                    hashtable["msg"] = "请选择国家!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }

                int adminId = 6;
                var country = ServiceFactory.ConfigureService.GetCountryById(countryId);
                var admin = ServiceFactory.AdminUserService.GetAdminUser(adminId);
                if (country == null || admin == null)
                {
                    hashtable["msg"] = "数据错误!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                var globalCollectDisabledCountry = new GlobalCollectDisabledCountry
                {
                    CountryId = country.CountryId,
                    CountryName = country.CountryName,
                    DateCreated = DateTime.Now,
                    AdminId = admin.AdminId,
                    AccountEmail = admin.AccountEmail
                };
                ServiceFactory.PaymentService.AddGlobalCollectDisabledCountry(globalCollectDisabledCountry);

            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                hashtable["msg"] = ActionJsonResult.Error;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }
            return Json(hashtable);
        }

        public JsonResult Delete(FormCollection form)
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            hashtable.Add("getlist", true);
            hashtable.Add("url", "/GlobalCollectDisabledCountry/GetList");
            try
            {
                int id = form["ids"].ParseTo(0);
                ServiceFactory.PaymentService.DeleteGlobalCollectDisabledCountryById(id);
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



        #region 支付方式限制


        public ActionResult PaymentEnabledCustomer()
        {
            var qstpayment = Request["paymentType"].ParseTo(32);
            ViewBag.PaymentType = qstpayment;
            var type = qstpayment.ToEnum<PaymentType>();
            ViewBag.PaymentName = type.ToString();
            return View();
        }

        public ActionResult PaymentEnabledCustomerList(string keyword)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);
            var paymentType = Request["paymentType"].ParseTo(32).ToEnum<PaymentType>();

            var searchCriteria = new Dictionary<PaymentEnabledCustomerSearchCriteria, object>
            {
                {PaymentEnabledCustomerSearchCriteria.PaymentType,paymentType}
            };
            var sorterCriteria = new List<Sorter<PaymentEnabledCustomerSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(PaymentEnabledCustomerSearchCriteria.KeyWord, keyword);
            }

            var enabledCustomers = ServiceFactory.PaymentService.FindPaymentEnabledCustomers(page, pageSize, searchCriteria, sorterCriteria);
            return View(enabledCustomers);
        }

        public ActionResult PaymentEnabledCustomerEdit(int? id)
        {
            var reqstr = Request["paymentType"];
            reqstr = reqstr.Replace(@"/0", "");
            ViewBag.PaymentType = reqstr.ParseTo(32);
            return View();
        }

        public JsonResult PaymentEnabledCustomerSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var email = form["txtEmail"];
                var paymentType = form["paymentType"].ParseTo(32).ToEnum<PaymentType>();
                if (email.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "客户Email不能为空！";
                    return Json(hashtable);
                }
                var admin = SessionHelper.CurrentAdminUser;
                if (admin == null)
                {
                    hashtable["msg"] = "您的登录凭证已过期，请刷新页面重新登录.";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                var paymentDisabledCustomer = new PaymentEnabledCustomer
                {
                    PaymentType = paymentType,
                    CustomerEmail = email,
                    DateCreated = DateTime.Now,
                    AdminId = admin.AdminId,
                    AccountEmail = admin.AccountEmail
                };

                ServiceFactory.PaymentService.SetPaymentEnabledCustomer(paymentDisabledCustomer);

                hashtable["result"] = ActionJsonResult.Success;

            }
            catch (Exception exp)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

        public JsonResult PaymentEnabledCustomerDelete(FormCollection form)
        {
            var hashtable = new Hashtable
            {
                {"error", false},
                {"msg", string.Empty},
                {"getlist", true},
                {"url", "/GlobalCollectDisabledCountry/PaymentEnabledCustomerList"}
            };
            try
            {
                var id = form["ids"].ParseTo(0);
                ServiceFactory.PaymentService.DeletePaymentEnabledCustomerById(id);
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
    }
}
