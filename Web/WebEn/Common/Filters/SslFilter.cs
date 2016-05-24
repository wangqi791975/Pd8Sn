using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Filters
{
    /// <summary>
    /// Https过滤
    /// </summary>
    public class SslFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (ConfigHelper.IsEnableHttps)
            {
                var controllerName = filterContext.RouteData.Values["controller"] as string;
                var actionName = filterContext.RouteData.Values["action"] as string;
                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;
                var scheme = request.Url.Scheme;

                if (scheme.Equals("http",StringComparison.InvariantCultureIgnoreCase))
                {
                    if (SslHelper.IsRequireHttps(controllerName,actionName))
                    {
                        var url = request.Url.OriginalString.Replace("http://"+request.Url.Authority,string.Format("https://{0}{1}" , request.Url.Host, ConfigHelper.HttpsPort == 443 ? string.Empty : ":" + ConfigHelper.HttpsPort));

                        response.Redirect(url);
                    }
                }
                else
                {
                    if (SslHelper.IsRequireHttp(controllerName, actionName))
                    {
                        var url = request.Url.OriginalString.Replace("https://" + request.Url.Authority, string.Format("https://{0}{1}", request.Url.Host, ConfigHelper.HttpPort == 80 ? string.Empty : ":" + ConfigHelper.HttpsPort));

                        response.Redirect(url);
                    }
                }

                return;
            }

            base.OnAuthorization(filterContext);
        } 
    }
}