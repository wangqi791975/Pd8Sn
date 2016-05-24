using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Com.Panduo.Web.Common.Mvc.Result;

namespace Com.Panduo.Web.Common.Mvc.Helper
{
    public static  class ResultHelper
    {
        #region Jsonp
        /// <summary>
        /// 返回Jsonp结果
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="data">返回的json数据</param>
        /// <param name="callback">回调函数名称</param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data, string callback = "jsonpcallback")
        {
            return controller.Jsonp(data, callback, null);
        }

        /// <summary>
        /// 返回Jsonp结果
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="data">返回的json数据</param>
        /// <param name="callback">回调函数名称</param>
        /// <param name="jsonRequestBehavior">Json请求行为</param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data, string callback = "jsonpcallback", JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            return controller.Jsonp(data, callback, null, jsonRequestBehavior);
        }

        /// <summary>
        /// 返回Jsonp结果
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="data">返回的json数据</param>
        /// <param name="callback">回调函数名称</param>
        /// <param name="contentType">返回的Mimi类型</param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data, string callback = "jsonpcallback", string contentType = "text/javascript")
        {
            return controller.Jsonp(data, callback, contentType, null);
        }

        /// <summary>
        /// 返回Jsonp结果
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="data">返回的json数据</param>
        /// <param name="callback">回调函数名称</param>
        /// <param name="contentType">返回的Mimi类型</param>
        /// <param name="jsonRequestBehavior">Json请求行为</param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data, string callback = "jsonpcallback", string contentType = "text/javascript", JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            return controller.Jsonp(data, callback, contentType, null, jsonRequestBehavior);
        }

        /// <summary>
        /// 返回Jsonp结果
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="data">返回的json数据</param>
        /// <param name="callback">回调函数名称</param>
        /// <param name="contentType">返回的Mimi类型</param>
        /// <param name="contentEncoding">编码方式</param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data, string callback = "jsonpcallback", string contentType = "text/javascript", Encoding contentEncoding = null)
        {
            return controller.Jsonp(data, callback, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 返回Jsonp结果
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="data">返回的json数据</param>
        /// <param name="callback">回调函数名称</param>
        /// <param name="contentType">返回的Mimi类型</param>
        /// <param name="contentEncoding">编码方式</param>
        /// <param name="jsonRequestBehavior">Json请求行为</param>
        /// <returns></returns>
        public static JsonpResult Jsonp(this Controller controller, object data, string callback = "jsonpcallback", string contentType = "text/javascript", Encoding contentEncoding = null, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonpResult
            {
                Callback = callback,
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                Data = data,
                JsonRequestBehavior = jsonRequestBehavior
            };
        }
        #endregion

        #region Image
        /// <summary>
        /// 输出图形对象
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="image">图形</param>
        /// <param name="imageFormat">图形格式</param>
        /// <returns></returns>
        public static ImageResult Image(this Controller controller, Image image, ImageFormat imageFormat = null)
        {
            return new ImageResult
                {
                    Image  = image,
                    ImageFormat = imageFormat ?? ImageFormat.Png
                };
        }
        #endregion

        #region Chart
        /// <summary>
        /// 输出图表对象
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="chart">图表</param>
        /// <param name="imageFormat">图表格式</param>
        /// <returns></returns>
        public static ChartResult Chart(this Controller controller, Chart chart, ImageFormat imageFormat = null)
        {
            return new ChartResult
                {
                    Chart = chart,
                    ChartFormat = "png"
                };
        }
        #endregion

        #region Css
        /// <summary>
        /// 输出Css内容
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="content">内容</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static CssResult SendCss(this Controller controller, string content, Encoding encoding = null)
        {
            return new CssResult { Content = content, ContentEncoding = encoding ?? Encoding.UTF8 };
        }

        #endregion
    }
}