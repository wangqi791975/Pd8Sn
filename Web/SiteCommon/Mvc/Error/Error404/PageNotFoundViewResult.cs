using System;
using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Error.Error404
{
    /// <summary>
    ///  名为PageNotFound的视图被呈现并且设置响应状态码为404
    ///  ViewData返回当前请求Url：RequestedUrl和上一个地址Url: ReferrerUrl
    /// </summary>
    public class PageNotFoundResult : HttpNotFoundResult
    {
        public PageNotFoundResult()
        {
            ViewName = CommonConfigHelper.PageNotFoundViewName;
            ViewData = new ViewDataDictionary();
        }

        /// <summary>
        /// 要呈现视图的名称,默认为PageNotFound
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// 要传递到视图的数据
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// 执行Result并返回视图数据
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;

            ViewData["RequestedUrl"] = GetRequestedUrl(request);
            ViewData["ReferrerUrl"] = GetReferrerUrl(request, request.Url.OriginalString);

            //设置404状态码
            response.Status = "404 Not Found";
            response.StatusCode = 404;
            response.TrySkipIisCustomErrors = true;

            var viewResult = new ViewResult
            {
                ViewName = ViewName,
                ViewData = ViewData
            };

            response.Clear();
            viewResult.ExecuteResult(context);
        }

        /// <summary>
        /// 获取请求URL地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        static string GetRequestedUrl(HttpRequestBase request)
        {
            return request.AppRelativeCurrentExecutionFilePath == "~/" + CommonConfigHelper.PageNotFoundRouteName
                       ? ExtractOriginalUrlFromExecuteUrlModeErrorRequest(request.Url) 
                       : request.Url.OriginalString;
        }

        /// <summary>
        /// 获取来源URL地址
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        static string GetReferrerUrl(HttpRequestBase request, string url)
        {
            return request.UrlReferrer != null && request.UrlReferrer.OriginalString != url
                       ? request.UrlReferrer.OriginalString 
                       : null;
        }

        /// <summary>
        /// Handles the case when a web.config &lt;error statusCode="404" path="/NotNotFound" responseMode="ExecuteURL" /&gt; is triggered.
        /// The original URL is passed via the querystring.
        /// </summary>
        static string ExtractOriginalUrlFromExecuteUrlModeErrorRequest(Uri url)
        {

            var start = url.Query.IndexOf(';');
            if (0 <= start && start < url.Query.Length - 1)
            {
                return url.Query.Substring(start + 1);
            }
            else
            {
                // Unexpected format, so just return the full URL!
                return url.ToString();
            }
        }
    }
}