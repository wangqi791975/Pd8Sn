using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Web.Common.Mvc;
using Com.Panduo.Web.Common.Mvc.Error.Error404;

namespace Com.Panduo.Web.Common.Mvc.Error
{
    /// <summary>
    /// 安装系统错误模块
    /// </summary>
    public class InstallerModule : IHttpModule
    {
        static bool _installed;
        static readonly object InstallerLock = new object();

        public void Init(HttpApplication application)
        {
            if (_installed) return;
            lock (InstallerLock)
            {
                if (_installed) return;
                Install();
                _installed = true;
            }
        }

        /// <summary>
        /// 安装模块
        /// </summary>
        static void Install()
        {
            WrapControllerBuilder();

            var routes = RouteTable.Routes;
            using (routes.GetWriteLock())
            {
                AddSystemRoute(routes);
                AddPageNotFoundRoute(routes);
                AddCatchAllRoute(routes);
            }
        } 
        /// <summary>
        /// 系统错误捕捉(500)
        /// </summary>
        /// <param name="routes"></param>
        static void AddSystemRoute(RouteCollection routes)
        {
            var route = new Route(
                CommonConfigHelper.SystemErrorRouteName,
                new RouteValueDictionary(new { controller = "SystemError", action = "SystemError" }),
                new RouteValueDictionary(new { incoming = new IncomingRequestRouteConstraint() }),
                new MvcRouteHandler()
            );
            
            routes.Insert(0, route);
        }

        /// <summary>
        /// 控制器404捕捉
        /// </summary>
        static void WrapControllerBuilder()
        {
            ControllerBuilder.Current.SetControllerFactory(
                new ControllerFactoryWrapper(
                    ControllerBuilder.Current.GetControllerFactory()
                )
            );
        }

        /// <summary>
        /// 路由器404捕捉
        /// </summary>
        /// <param name="routes"></param>
        static void AddPageNotFoundRoute(RouteCollection routes)
        { 
            var route = new Route(
                CommonConfigHelper.PageNotFoundRouteName,
                new RouteValueDictionary(new { controller = "PageNotFound", action = "PageNotFound" }),
                new RouteValueDictionary(new {incoming = new IncomingRequestRouteConstraint()}),
                new MvcRouteHandler()
            );
            
            routes.Insert(0, route); 
        }

        /// <summary>
        /// 所有不能识别路径404捕捉
        /// </summary>
        /// <param name="routes"></param>
        static void AddCatchAllRoute(RouteCollection routes)
        {
            routes.MapRoute(
                "NotNotFound-Catch-All",
                "{*any}",
                new { controller = "PageNotFound", action = "PageNotFound" }
            );
        }

        public void Dispose() { }

        class IncomingRequestRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return routeDirection == RouteDirection.IncomingRequest;
            }
        }
    }
}
