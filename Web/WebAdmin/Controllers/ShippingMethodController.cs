using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order.ShippingOption;

namespace Com.Panduo.Web.Controllers
{
    public class ShippingMethodController : BaseController
    {
        //
        // GET: /ShippingMethod/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(string keyword)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<ShippingSearchCriteria, object>
            {
            };
            var sorterCriteria = new List<Sorter<ShippingSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(ShippingSearchCriteria.Name, keyword);
            }

            var shippings = ServiceFactory.ShippingService.FindAllShippings(page, pageSize, searchCriteria, sorterCriteria);
            return View(shippings);
        }

        public ActionResult Show(int id)
        {
            var shippingBaseInfo = new ShippingBaseInfo
            {
                Shipping = new Shipping(),
                ShippingDay = new List<ShippingDay>(),
                ShippingLanguages = new List<ShippingLanguage>()
            };
            if (id > 0)
            {
                try
                {
                    shippingBaseInfo = ServiceFactory.ShippingService.GetShippingById(id);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_SHIPPING_NOT_EXISTS":
                            ViewBag.ErrorMsg = "运送方式不存在！";
                            break;
                    }
                }
            }
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();

            //ViewBag.Shippings = ServiceFactory.ShippingService.FindAllShippings(1,2,new Dictionary<ShippingSearchCriteria, object>(), new List<Sorter<ShippingSorterCriteria>>() );
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("运送方式", Url.Content("/ShippingMethod/Index")));
            return View(shippingBaseInfo);
        }

        public ActionResult Edit(int id)
        {
            var shippingBaseInfo = new ShippingBaseInfo
            {
                Shipping = new Shipping(),
                ShippingDay = new List<ShippingDay>(),
                ShippingLanguages = new List<ShippingLanguage>()
            };
            if (id > 0)
            {
                try
                {
                    shippingBaseInfo = ServiceFactory.ShippingService.GetShippingById(id);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_SHIPPING_NOT_EXISTS":
                            ViewBag.ErrorMsg = "运送方式不存在！";
                            break;
                    }
                }
            }
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.CommonCountry = ServiceFactory.ConfigureService.GetCommonCountry();
            ViewBag.UnCommonCountry = ServiceFactory.ConfigureService.GetUnCommonCountry();

            //ViewBag.Shippings = ServiceFactory.ShippingService.FindAllShippings(1,2,new Dictionary<ShippingSearchCriteria, object>(), new List<Sorter<ShippingSorterCriteria>>() );
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("运送方式", Url.Content("/ShippingMethod/Index")));
            return View(shippingBaseInfo);
        }

        public JsonResult ShippingSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var marketingName = form["ShippingName"];
                if (marketingName.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "运送方式名称不能为空！";
                    return Json(hashtable);
                }

                hashtable["result"] = ActionJsonResult.Success;

            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_SHIPPING_NOT_EXISTS":
                        hashtable["msg"] = "运送方式不存在.";
                        break;
                }
            }
            catch (Exception exp)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

    }
}
