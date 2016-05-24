using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Error.Error404
{
    /// <summary>
    /// Wraps another IActionInvoker except it handles the case of an action method
    /// not being found and invokes the PageNotFoundController instead.
    /// </summary>
    class ActionInvokerWrapper : IActionInvoker
    {
        readonly IActionInvoker _actionInvoker;

        public ActionInvokerWrapper(IActionInvoker actionInvoker)
        {
            _actionInvoker = actionInvoker;
        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (InvokeActionWith404Catch(controllerContext, actionName))
                return true;

            // No action method was found, or it was, but threw a 404 HttpException.
            ExecuteNotFoundControllerAction(controllerContext);

            return true;
        }

        static void ExecuteNotFoundControllerAction(ControllerContext controllerContext)
        {
            var controller = new PageNotFoundController();
            controller.ExecuteNotFound(controllerContext.RequestContext);
        }

        bool InvokeActionWith404Catch(ControllerContext controllerContext, string actionName)
        {
            try
            {
                return _actionInvoker.InvokeAction(controllerContext, actionName);
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    return false;
                }
                throw;
            }
        }
    }
}
