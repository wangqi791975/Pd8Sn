using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    /// <summary>
    /// 视图辅助类
    /// </summary>
    public static class ViewHelper
    { 
        /// <summary>
        /// 渲染指定分部视图为Html字符串
        /// </summary>
        /// <typeparam name="T">强类型数据类型</typeparam>
        /// <param name="controller">视图所在Controller</param>
        /// <param name="viewName">视图名称或相对路径</param>
        /// <param name="model">模型数据</param>
        /// <returns></returns>
        public static string RenderPartialViewToHtml<T>(this Controller controller, string viewName,T model=default(T))
        {
            controller.ViewData.Model = model;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);
                    viewResult.View.Render(viewContext, stringWriter);

                    return stringWriter.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 渲染指定分部视图为Html字符串
        /// </summary>
        /// <typeparam name="T">强类型数据类型</typeparam>
        /// <param name="controller">视图所在Controller</param>
        /// <param name="viewName">视图名称或相对路径</param>
        /// <param name="layoutName">视图Layout名称或路径</param>
        /// <param name="model">模型数据</param>
        /// <returns></returns>
        public static string RenderViewToHtml<T>(this Controller controller, string viewName, string layoutName, T model = default(T))
        {
            controller.ViewData.Model = model;
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, layoutName);
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);
                    viewResult.View.Render(viewContext, stringWriter);

                    return stringWriter.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}