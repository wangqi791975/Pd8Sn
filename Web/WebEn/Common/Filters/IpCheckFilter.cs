using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service; 

namespace Com.Panduo.Web.Common.Filters
{
    /// <summary>
    /// Ip地址检查过滤器
    /// </summary>
    public class IpCheckFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isValid = true;

            //启用IP检查的时候才需要判断
            if (CacheHelper.IsIpCheck && httpContext.Session != null)
            {
                //&& httpContext.Session != null
                if (httpContext.Session[SessionHelper.SESSION_KEY_IS_IP_CHECKED] == null)
                {
                    //Session中不存在的时候需要根据IP去判断是否允许访问
                    if (httpContext.Request.IsLocal)
                    {
                        //本地访问是允许的
                        httpContext.Session[SessionHelper.SESSION_KEY_IS_IP_CHECKED] = true;
                    }
                    else
                    {
                        //非本地访问需要判断ip
                        httpContext.Session[SessionHelper.SESSION_KEY_IS_IP_CHECKED] = ServiceFactory.ConfigureService.IsIpAllowVisit(PageManager.GetClientIp());
                    }
                }

                //Session中存在的时候根据Session中的数据判断
                isValid = CommonHelper.ParseTo(httpContext.Session[SessionHelper.SESSION_KEY_IS_IP_CHECKED].ToString(), false);

                //如果被拦截则记录日志
                if (!isValid)
                {
                    LoggerHelper.GetLogger(LoggerType.IpIntercept).InfoFormat("IP[{0}]于[{1}]尝试访问网站[{2}]由于IP不被允许而被拒绝!", PageManager.GetClientIp(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),httpContext.Request.RawUrl);
                }
            } 
            
            return isValid;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/" + CommonConfigHelper.IpCheckErrorUrl);
        }
    }
}