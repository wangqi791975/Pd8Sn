using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Com.Panduo.Web.Common.Mvc.Error.Error404
{
    /// <summary>
    /// 控制器工厂包裹
    /// </summary>
    class ControllerFactoryWrapper : IControllerFactory
    {
        readonly IControllerFactory _factory;

        public ControllerFactoryWrapper(IControllerFactory factory)
        {
            _factory = factory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                var controller = _factory.CreateController(requestContext, controllerName);
                WrapControllerActionInvoker(controller);
                return controller;
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    return new PageNotFoundController();
                }

                throw;
            }
        }

        static void WrapControllerActionInvoker(IController controller)
        {
            var controllerWithInvoker = controller as Controller;
            if (controllerWithInvoker != null)
            {
                controllerWithInvoker.ActionInvoker = new ActionInvokerWrapper(controllerWithInvoker.ActionInvoker);
            }
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return _factory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            _factory.ReleaseController(controller);
        }
    }
}