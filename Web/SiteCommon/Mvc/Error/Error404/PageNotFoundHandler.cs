using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Com.Panduo.Web.Common.Mvc.Error.Error404
{
    /// <summary>
    /// 4040处理器
    /// </summary>
    public class PageNotFoundHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", CommonConfigHelper.PageNotFoundRouteName);
            var controllerContext = new ControllerContext(new HttpContextWrapper(context), routeData, new FakeController());
            var notFoundViewResult = new PageNotFoundResult();
            notFoundViewResult.ExecuteResult(controllerContext);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        // ControllerContext requires an object that derives from ControllerBase.
        class FakeController : Controller { }
    }
}