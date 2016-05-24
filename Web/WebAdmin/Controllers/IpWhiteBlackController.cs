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
    public class IpWhiteBlackController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            var whiteList = ServiceFactory.ConfigureService.GetWhiteList();
            ViewData.Model = whiteList;
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
                string ips = Request["ips"].Trim();
                if (ips == string.Empty)
                {
                    hashtable["msg"] = "IP地址不能为空!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                
                int adminId = 6;
                int count = 0;
                var ipsArr = ips.Split(new char[] { ',', '，' });
                foreach (var ip in ipsArr)
                {
                    if (!ip.Equals("") && ip.Length > 5)
                    {
                        var allow = ServiceFactory.ConfigureService.IsIpAllowVisit(ip);
                        if (!allow)
                        {
                            var whiteList = new WhiteList
                            {
                                IpAddress = ip,
                                CreateId = adminId,
                                CreateTime = DateTime.Now
                            };
                            ServiceFactory.ConfigureService.AddWhiteList(whiteList);
                            count++;
                        }
                    }
                    
                }
                hashtable["msg"] = string.Format("成功添加<b>{0}</b>个", count);

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
