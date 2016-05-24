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
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class CashSectionController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            var cashSections = ServiceFactory.CashService.GetAllCashSections();
            ViewData.Model = cashSections;
            return View();
        }

        public ActionResult Submit()
        {
            var hashtable = new Hashtable();
            hashtable.Add("getlist", true);
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                int code = Request["code"].ParseTo(0);
                int amountMinimum = Request["amount_minimum"].ParseTo(0);
                int amountMaximum = Request["amount_maximum"].ParseTo(0);
                if (code == 0)
                {
                    hashtable["msg"] = "数据非法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (amountMinimum == 0)
                {
                    hashtable["msg"] = "最小金额不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (amountMaximum <= 0)
                {
                    hashtable["msg"] = "最大金额不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (amountMinimum >= amountMaximum)
                {
                    hashtable["msg"] = "最小金额不能大于最大金额!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                
                int adminId = 6;
                var cashSection = new CashSection
                {
                    Code = code,
                    AmountMinimum = amountMinimum,
                    AmountMaximum = amountMaximum,
                    DateModified = DateTime.Now,
                    AdminId = adminId
                };
                ServiceFactory.CashService.UpdateCashSection(cashSection);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = ActionJsonResult.Error;
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
