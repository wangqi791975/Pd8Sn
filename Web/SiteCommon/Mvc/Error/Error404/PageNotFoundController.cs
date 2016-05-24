using System.Web.Mvc;
using System.Web.Routing;

namespace Com.Panduo.Web.Common.Mvc.Error.Error404
{
    /// <summary>
    /// 404控制器
    /// </summary>
    public class PageNotFoundController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            ExecuteNotFound(requestContext);
        }

        public void ExecuteNotFound(RequestContext requestContext)
        {
            new PageNotFoundResult().ExecuteResult(
                new ControllerContext(requestContext, new FakeController())
            );
        }

        // ControllerContext requires an object that derives from ControllerBase.
        // NotFoundController does not do this.
        // So the easiest workaround is this FakeController.
        class FakeController : Controller { }
    }

}