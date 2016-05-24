using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;


namespace Com.Panduo.Web.Common.Mvc.Routing
{
    /// <summary>
    ///   UrlRoute重写操作扩展类
    /// </summary>
    public static class RewriteUrlRouteExtensions
    {
        /// <summary>
        ///   把路由集合的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this RouteCollection routes, string name, string url)
        {
            return routes.RewriteCaseUrlRoute(name, url, null, null);
        }

        /// <summary>
        ///   把路由集合的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.RewriteCaseUrlRoute(name, url, defaults, null);
        }

        /// <summary>
        ///   把路由集合的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return routes.RewriteCaseUrlRoute(name, url, null, null, namespaces);
        }

        /// <summary>
        ///   把路由集合的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.RewriteCaseUrlRoute(name, url, defaults, constraints, null);
        }

        /// <summary>
        ///   把路由集合的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.RewriteCaseUrlRoute(name, url, defaults, null, namespaces);
        }

        /// <summary>
        ///   把路由集合的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, object constraints,
            string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            var route2 = new RewriteUrlRoute(url, new MvcRouteHandler()) {Defaults = new RouteValueDictionary(defaults), Constraints = new RouteValueDictionary(constraints), DataTokens = new RouteValueDictionary()};
            var item = route2;
            if ((namespaces != null) && (namespaces.Length > 0))
            {
                item.DataTokens["Namespaces"] = namespaces;
            }
            routes.Add(name, item);
            return item;
        }

        /// <summary>
        ///   把区域路由的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this AreaRegistrationContext context, string name, string url)
        {
            return context.RewriteCaseUrlRoute(name, url, null);
        }

        /// <summary>
        ///   把区域路由的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return context.RewriteCaseUrlRoute(name, url, defaults, null);
        }

        /// <summary>
        ///   把区域路由的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this AreaRegistrationContext context, string name, string url, string[] namespaces)
        {
            return context.RewriteCaseUrlRoute(name, url, null, namespaces);
        }

        /// <summary>
        ///   把区域路由的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            return context.RewriteCaseUrlRoute(name, url, defaults, constraints, null);
        }

        /// <summary>
        ///   把区域路由的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces)
        {
            return context.RewriteCaseUrlRoute(name, url, defaults, null, namespaces);
        }

        /// <summary>
        ///   把区域路由的UrlRoute重写
        /// </summary>
        public static RewriteUrlRoute RewriteCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints,
            string[] namespaces)
        {
            if ((namespaces == null) && (context.Namespaces != null))
            {
                namespaces = context.Namespaces.ToArray();
            }
            var route = context.Routes.RewriteCaseUrlRoute(name, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = context.AreaName;
            var flag = (namespaces == null) || (namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = flag;
            return route;
        }
    }
}