using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Web.Common.Config;

namespace Com.Panduo.Web.Common.Filters
{
    /// <summary>
    /// 登陆检查过滤器
    /// </summary>
    public class LoginFilter : AuthorizeAttribute
    {
        /// <summary>
        /// 需要登录才能访问的资源
        /// </summary>
        private IDictionary<string, LoginFilterConfig> _loginActionMap = ConfigHelper.LoginConfigMap;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            if (filterContext.HttpContext.Session != null && SessionHelper.CurrentAdminUser == null)
            {
                var controllerNameString = controllerName.ToString().Trim().ToLower();
                if (_loginActionMap.ContainsKey(controllerNameString))
                {
                    var actionNameString = actionName.ToString();
                    var config = _loginActionMap[controllerNameString];
                    //1.如果排除项包括了当前Action那么就不需要判断了
                    //2.如果包含项配置为*或者All或者包含当前Action那么就需要登录
                    if (!config.ExcludeActionList.Any(c => string.Equals(c, actionNameString, StringComparison.InvariantCultureIgnoreCase)) && config.IncludeActionList.Any(c => c == "*" || string.Equals(c, "All", StringComparison.InvariantCultureIgnoreCase) || string.Equals(c, actionNameString, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        HandleUnauthorizedRequest(filterContext);
                        return;
                    }
                }
            }
            if (SessionHelper.CurrentAdminUser != null && (SessionHelper.CurrentAdminUser.UpdatePasswordTime.HasValue && (DateTime.Now - SessionHelper.CurrentAdminUser.UpdatePasswordTime.Value).Days > ServiceConfig.AdminModifyTime && "/AdminUser/ModifyPassword" != filterContext.HttpContext.Request.RawUrl))
            {
                var controllerNameString = controllerName.ToString().Trim().ToLower();
                if (_loginActionMap.ContainsKey(controllerNameString))
                {
                    var actionNameString = actionName.ToString();
                    var config = _loginActionMap[controllerNameString];
                    //1.如果排除项包括了当前Action那么就不需要判断了
                    //2.如果包含项配置为*或者All或者包含当前Action那么就需要登录
                    if (!config.ExcludeActionList.Any(c => string.Equals(c, actionNameString, StringComparison.InvariantCultureIgnoreCase)) && config.IncludeActionList.Any(c => c == "*" || string.Equals(c, "All", StringComparison.InvariantCultureIgnoreCase) || string.Equals(c, actionNameString, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("/AdminUser/ModifyPassword");
                        return;
                    }
                }
            }

            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Ajax登录的
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 488;
                //filterContext.HttpContext.Response.Status = "";//Unauthorized
                filterContext.HttpContext.Response.StatusDescription = "";//AjaxLogin
                return;
            }

            //非Ajax登陆的登陆以后跳转到请求的地址
            filterContext.Result = new RedirectResult("/AdminUser/Login");
        }
    }
}