using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common.Mvc.Helper
{
    /// <summary>
    /// Url辅助
    /// </summary>
    public static class CommonUrlHelper
    {
        /// <summary>
        /// 返回Url完整路径，包括http,主机、端口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string ContentFullPath(this UrlHelper url, string virtualPath)
        {
            if (virtualPath.StartsWith("http"))
            {
                return virtualPath;
            }

            var rootPath = UrlPathHelper.GetHost(false);

            var result = string.Format("{0}{1}",rootPath, VirtualPathUtility.ToAbsolute(virtualPath));

            return result;
        }

        public static string ContentFullPath(string virtualPath)
        {
            if (virtualPath.StartsWith("http"))
            {
                return virtualPath;
            }

            var rootPath = UrlPathHelper.GetHost(false);

            var result = string.Format("{0}{1}", rootPath, VirtualPathUtility.ToAbsolute(virtualPath));

            return result;
        }

        /// <summary>
        /// 在Controller中根据路由生成url
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public static string RouteUrl(this Controller controller, string routeName,RouteValueDictionary routeValueDictionary=null)
        {
            if (routeValueDictionary==null)
            {

                return new UrlHelper(HttpContext.Current.Request.RequestContext).RouteUrl(routeName);
            }

            return new UrlHelper(HttpContext.Current.Request.RequestContext).RouteUrl(routeName, routeValueDictionary);
        }

        public static string Action(this Controller c, string controller, string action)
        {
            var rvd = new RouteValueDictionary();
            rvd.Add("controller", controller);
            rvd.Add("action", action);
            return RouteTable.Routes.GetVirtualPath(c.Request.RequestContext, rvd).VirtualPath;
        }
    }
}
