using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Result
{
    public class CssResult : ContentResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/css";

            base.ExecuteResult(context);
        }
    }
}