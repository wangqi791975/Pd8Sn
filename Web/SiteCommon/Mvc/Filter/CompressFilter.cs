using System.IO.Compression;

using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Filter
{
    /// <summary>
    /// gzip/deflate压缩过滤
    /// </summary>
    public class CompressFilterAttribute: ActionFilterAttribute
    {

        public override void  OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Ajax不进行压缩
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            //异常类不进行压缩
            if (filterContext.Exception != null)
            {
                return;
            }

            var request = filterContext.HttpContext.Request;

            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToLowerInvariant();

            var response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("gzip"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }

            //清除响应头中的Service属性
            if (CommonConfigHelper.IsRemoveServerReponseHeader)
            {
                filterContext.HttpContext.Response.Headers.Remove("Server");
            }
        }
    }
}