using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order.ShippingOption;

namespace Com.Panduo.Web.Controllers
{
    public class RemoteController : Controller
    {
        
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var shipping=ServiceFactory.ShippingService.GetAllShippings();
            return View(shipping);
        }

        /// <summary>
        /// 偏远信息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            var shippingcode = Request["shippingcode"] ?? string.Empty;
            var displayId = Request["DisplayId"].ParseTo(1);
            if (displayId == 1)
            {
                var searchCriteria = new Dictionary<RemoteCitySearchCriteria, object>();
                if (!shippingcode.IsNullOrEmpty())
                {
                    searchCriteria.Add(RemoteCitySearchCriteria.ShippingCode, shippingcode);
                }
                var sorterCriteria = new List<Sorter<RemoteCitySorterCriteria>>();
                var remote = ServiceFactory.ShippingService.FindRemoteCities(page, pageSize, searchCriteria,
                    sorterCriteria);
                return View("GetRemoteCityList", remote);
            }
            else
            {
                var searchCriteria = new Dictionary<RemoteZipSearchCriteria, object>();
                if (!shippingcode.IsNullOrEmpty())
                {
                    searchCriteria.Add(RemoteZipSearchCriteria.ShippingCode, shippingcode);
                }
                var sorterCriteria = new List<Sorter<RemoteZipSorterCriteria>>();
                var remote = ServiceFactory.ShippingService.FindRemoteZips(page, pageSize, searchCriteria, sorterCriteria);
                return View("GetRemoteZipList", remote);
            }
        }
    }
}
