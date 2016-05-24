using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var product = ServiceFactory.ProductService.GetProductById(1);
            ViewBag.Product = product;

            return View();
        }

        /// <summary>
        /// 脚本错误收集
        /// </summary>
        [ValidateInput(false)]
        public ActionResult ScriptError()
        {
            if (Request.UrlReferrer == null || Request.QueryString.Count == 0)
            {
                return null;
            }

            var adminUser = SessionHelper.CurrentAdminUser;
            var err = new StringBuilder();
            err.AppendLine(Environment.NewLine + "Referrer: " + Request.UrlReferrer.ToString());
            err.AppendLine("User Agent: " + Request.UserAgent ?? string.Empty);
            err.AppendLine("Cookie: " + Request.Browser.Cookies);
            err.AppendLine("IP: " + PageManager.GetClientIp());
            err.AppendLine("AdminUser Id: " + (adminUser == null ? string.Empty : adminUser.AdminId.ToString()));
            err.AppendLine("Error: ");
            err.AppendLine(Request.QueryString.AllKeys.Select(c => string.Format("{0}: {1}", c, HttpUtility.UrlDecode(Request.QueryString[c]))).Join(Environment.NewLine));

            LoggerHelper.GetLogger(LoggerType.ScriptError).Info(err.ToString());

            return null;
        }
    }
}
