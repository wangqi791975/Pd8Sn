using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web
{
    /// <summary>
    /// 路由配置
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            #region 动态脚本
            //Js脚本错误
            routes.MapRoute(
                "JavaScriptError",
                "script-error.html",
                new { controller = "Home", action = "ScriptError" }
            );

            //动态Css路由
            routes.MapRoute(
                "Css",
                YuiHtmlHelper.ActionNameCss + "/{resourceName}",
                new { controller = "Yui", action = YuiHtmlHelper.ActionNameCss },
                new[] { "Com.Panduo.Web.Common.Mvc.Controllers" }
            );

            //动态Js路由
            routes.MapRoute(
                "Js",
                YuiHtmlHelper.ActionNameJs + "/{resourceName}",
                new { controller = "Yui", action = YuiHtmlHelper.ActionNameJs },
                new[] { "Com.Panduo.Web.Common.Mvc.Controllers" }
            );
            #endregion

            #region 错误
            //系统错误
            routes.MapRoute(
                CommonConfigHelper.SystemErrorRouteName,
                CommonConfigHelper.SystemErrorRouteName,
                new { controller = "SystemError", action = "SystemError" },
                new[] { " Com.Panduo.Web.Common.Mvc.Error.Error500" }
            );

            //404错误
            routes.MapRoute(
                CommonConfigHelper.PageNotFoundRouteName,
                CommonConfigHelper.PageNotFoundRouteName,
                new { controller = "PageNotFound", action = "Execute" },
                new[] { " Com.Panduo.Web.Common.Mvc.Error.Error404" }
            );

            //授权错误
            routes.MapRoute(
               "Unauthorized", // 路由名称
               "Unauthorized", // 带有参数的 URL
               new { controller = "Home", action = "Unauthorized" } // 参数默认值
            );
            #endregion
             
            
            //默认路由
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}