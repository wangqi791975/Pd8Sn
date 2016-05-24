using System.Web;
using System.Web.Routing;


namespace Com.Panduo.Web.Common.Mvc.Routing
{
    /// <summary>
    ///   Url小写转换辅助类 重写Route类的GetVirtualPath方法，把Url中非参数字符转换为小写
    /// </summary>
    public class RewriteUrlRoute : Route
    {
        #region 字段

        private static readonly string[] RequireKeys = new[] { "area", "controller", "action" };

        #endregion

        #region 构造函数

        /// <summary>
        ///   初始化一个LowerCaseUrlRoute类的新实例
        /// </summary>
        /// <param name="url"> </param>
        /// <param name="routeHandler"> </param>
        public RewriteUrlRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) { }

        /// <summary>
        ///   初始化一个LowerCaseUrlRoute类的新实例
        /// </summary>
        /// <param name="url"> </param>
        /// <param name="defaults"> </param>
        /// <param name="routeHandler"> </param>
        public RewriteUrlRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler) { }

        /// <summary>
        ///   初始化一个LowerCaseUrlRoute类的新实例
        /// </summary>
        /// <param name="url"> </param>
        /// <param name="defaults"> </param>
        /// <param name="constraints"> </param>
        /// <param name="routeHandler"> </param>
        public RewriteUrlRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler) { }

        /// <summary>
        ///   初始化一个LowerCaseUrlRoute类的新实例
        /// </summary>
        /// <param name="url"> </param>
        /// <param name="defaults"> </param>
        /// <param name="constraints"> </param>
        /// <param name="dataTokens"> </param>
        /// <param name="routeHandler"> </param>
        public RewriteUrlRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens,
            IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) { }

        #endregion

        #region 公有方法

        /// <summary>
        ///   解释分隔后的URL
        /// </summary>
        /// <param name="httpContext"> </param>
        /// <returns> </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var result = base.GetRouteData(httpContext);
            if (result == null)
            {
                return null;
            }
            var dict = result.Values;
            foreach (var key in RequireKeys)
            {
                if (!dict.ContainsKey(key))
                {
                    continue;
                }
                var value = dict[key];
                if (!(value is string))
                {
                    continue;
                }
                dict[key] = RewriteUrlHelper.Restore(value as string);
            }
            return result;
        }

        /// <summary>
        ///   把Url中非参数字符转换为小写分隔的形式
        /// </summary>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var valueDict = new RouteValueDictionary(values);
            foreach (var key in RequireKeys)
            {
                if (!valueDict.ContainsKey(key))
                {
                    continue;
                }
                var value = valueDict[key];
                if (!(value is string))
                {
                    continue;
                }
                valueDict[key] = RewriteUrlHelper.Spliter(value as string);
            }
            return base.GetVirtualPath(requestContext, valueDict);
        }

        #endregion
    }
}