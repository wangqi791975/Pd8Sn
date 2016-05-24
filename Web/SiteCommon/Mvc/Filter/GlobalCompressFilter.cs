using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Filter
{
    /// <summary>
    /// 全局压缩过滤器
    /// </summary>
    public class GlobalCompressFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
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
            var response = filterContext.HttpContext.Response;

            //压缩设置
            if (CommonConfigHelper.IsCompressOn)
            {

                if (CommonConfigHelper.CompressableMimeTypes.Contains(response.ContentType.ToLower()) && response.Filter != null)
                {
                    var acceptEncoding = request.Headers["Accept-Encoding"];

                    if (string.IsNullOrEmpty(acceptEncoding)) return;

                    acceptEncoding = acceptEncoding.ToLowerInvariant();

                    if (acceptEncoding.Contains("gzip"))
                    {
                        response.AppendHeader("Content-encoding", "gzip");
                        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate"))
                    {
                        response.AppendHeader("Content-encoding", "deflate");
                        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    }
                }  
            }

            //清除响应头中的Service属性
            if (CommonConfigHelper.IsRemoveServerReponseHeader)
            {
                filterContext.HttpContext.Response.Headers.Remove("Server");
            }

            base.OnResultExecuted(filterContext);
        }
    }
}