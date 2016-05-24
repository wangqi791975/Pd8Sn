using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Engine
{
    /// <summary>
    /// 自定义Razor视图引擎
    /// </summary>
    public class CustomRazorViewEngine : RazorViewEngine
    {
        private static readonly string[] ViewSearchFormats = new[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/Partial/{0}.cshtml",
                "~/Views/Shared/Layout/{0}.cshtml",
                "~/Views/Shared/Error/{0}.cshtml",
            };

        private static readonly string[] AreaViewSearchFormats = new[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/Partial/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/Layout/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/Error/{0}.cshtml",
            };

        public CustomRazorViewEngine()
        {
            //视图
            ViewLocationFormats = ViewSearchFormats;
            //模板
            MasterLocationFormats = ViewSearchFormats;
            //部分视图
            PartialViewLocationFormats = ViewSearchFormats;

            //区域视图
            AreaViewLocationFormats = AreaViewSearchFormats;
            //区域模板
            AreaMasterLocationFormats = AreaViewSearchFormats;
            //区域部分视图
            AreaPartialViewLocationFormats = AreaViewSearchFormats;
        }
    }
}
