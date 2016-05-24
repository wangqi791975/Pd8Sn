using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common.Filters
{
    /// <summary>
    /// 系统异常过滤器
    /// </summary>
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //记录错误日志
            Logger(filterContext);

            //跳转
            if (ConfigManager.IsSystemExceptionFilter)
            { 
                filterContext.ExceptionHandled = true; 
                filterContext.Result = new RedirectResult("/" + CommonConfigHelper.SystemErrorRouteName); 
            }
            else
            {
                base.OnException(filterContext);
            }
           
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="filterContext"></param> 
        private static void Logger(ExceptionContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            var message = string.Format("错误处理过滤:controller:{0},action:{1},错误消息:{2},错误跟踪信息:{3}", controllerName, actionName, filterContext.Exception.Message, filterContext.Exception.StackTrace);
            
            LoggerHelper.GetLogger(LoggerType.Exception).Error(message,filterContext.Exception);
        }
    }


}