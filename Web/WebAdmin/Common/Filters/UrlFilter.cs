using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;

namespace Com.Panduo.Web.Common.Filters
{
    /// <summary>
    /// url路径过滤器
    /// </summary>
    public class UrlFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            if (!SessionHelper.CurrentAdminUserModules.IsNullOrEmpty())
            {
                var controllerNameString = controllerName.ToString().Trim().ToLower();
                var actionNameString = actionName.ToString().Trim().ToLower();
                var adminModule = ServiceFactory.AdminUserService.GetAdminModule(controllerNameString, actionNameString);
                if (!(controllerNameString == "home" && actionNameString == "index") && !adminModule.IsNullOrEmpty() && SessionHelper.CurrentAdminUserModules.Find(m => m.ModuleCode == adminModule.ModuleCode).IsNullOrEmpty())
                {
                    HandleUnauthorizedRequest(filterContext);
                    return;
                }
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //非Ajax登陆的登陆以后跳转到请求的地址
            filterContext.Result = new RedirectResult("/AdminUser/Login");
        }
    }
}