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
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class CustomerHighRiskController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            var whiteList = ServiceFactory.ConfigureService.GetWhiteList();
            ViewData.Model = whiteList;
            return View();
        }

        [HttpGet]
        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = 20;

            var customers = ServiceFactory.CustomerService.FindCustomerHighRisks(page, pageSize, null, null);
            return View(customers);
        }

        public ActionResult GetInfo()
        {
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
                if (customerEmail == string.Empty)
                {
                    hashtable["msg"] = "IP地址不能为空!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                
                int adminId = 6;
                var admin = ServiceFactory.AdminUserService.GetAdminUser(adminId);
                var customer = ServiceFactory.CustomerService.GetCustomerByEmail(customerEmail);
                if (customer == null)
                {
                    hashtable["msg"] = "客户不存在!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                var customerHighRisk = new CustomerHighRisk
                {
                    CustomerId = customer.CustomerId,
                    CustomerEmail = customer.Email,
                    AdminId = adminId,
                    AdminEmail = admin.AccountEmail,
                    DateCreated = DateTime.Now
                };
                ServiceFactory.CustomerService.AddCustomerHighRisk(customerHighRisk);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = ActionJsonResult.Error;
                switch (bussinessException.GetError())
                {
                    case "ERROR_CUSTOMER_NOT_EXIST":
                        hashtable["msg"] = "客户不存在!";
                        hashtable["error"] = true;
                        break;

                    case "ERROR_EMAIL_EXIST":
                        hashtable["msg"] = "该客户已添加进高危客户!";
                        hashtable["error"] = true;
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
            hashtable.Add("url", "/CustomerHighRisk/GetList");
            try
            {
                int id = form["ids"].ParseTo(0);
                ServiceFactory.CustomerService.DelCustomerHighRisk(id);
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

    }
}
