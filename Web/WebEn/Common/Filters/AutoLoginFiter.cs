using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Common.Filters
{
    /// <summary>
    /// 自动登录过滤器
    /// </summary>
    public class AutoLoginFiter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session != null && SessionHelper.CurrentCustomer == null)
 	        {
                //todo 从Cookie读取用户名和密码然后调用业务方法给客户登陆
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //do nothing but log it;
        }
    }
}