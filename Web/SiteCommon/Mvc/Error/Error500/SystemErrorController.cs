using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Com.Panduo.Web.Common.Mvc.Error.Error500
{
    /// <summary>
    /// 系统错误控制器
    /// </summary>
    public class SystemErrorController : Controller
    {
        public ActionResult SystemError()
        {
            Response.Status = "500 Internal Server Error";
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            var dataa = RouteData.Values;

            return View(CommonConfigHelper.SystemErrorViewName);
        }
    }
}
