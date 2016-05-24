using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 工具辅助类
    /// </summary>
    public static class ToolHelper
    {
        /// <summary>
        /// 创建GET方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string CreateHttpGet(string url, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            var html = string.Empty;
            HttpWebResponse response = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                //request.Accept = "*/*";
                request.ContentType = string.Format("application/x-www-form-urlencoded{0}", Encoding.GetEncoding("gb2312").Equals(encoding) ? ";charset=gb2312" : string.Empty);

                response = (HttpWebResponse)request.GetResponse();
                if ((int)response.StatusCode == 200)
                {
                    var reader = new StreamReader(response.GetResponseStream(), encoding);
                    html = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                LoggerHelper.GetLogger(LoggerType.Exception).Error(ex.Message, ex);
            }
            finally
            {
                if (response != null) response.Close();
            }

            return html;
        }

        /// <summary>
        /// 创建GET方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <param name="parameters">请求的参数</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string CreateHttpPost(string url, IDictionary<string, string> parameters, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            var html = string.Empty;
            HttpWebResponse response = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                //request.Accept = "*/*";
                request.ContentType = string.Format("application/x-www-form-urlencoded{0}", Encoding.GetEncoding("gb2312").Equals(encoding) ? ";charset=gb2312" : string.Empty);
                //byte[] buffer = encoding.GetBytes(postData);
                //request.ContentLength = buffer.Length;
                //using (Stream reqStream = request.GetRequestStream())
                //{
                //    reqStream.Write(buffer, 0, buffer.Length);
                //}
                //发送POST数据  
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                            i++;
                        }
                    }
                    byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                if ((int)response.StatusCode == 200)
                {
                    var reader = new StreamReader(response.GetResponseStream(), encoding);
                    html = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                LoggerHelper.GetLogger(LoggerType.Exception).Error(ex.Message, ex);
            }
            finally
            {
                if (response != null) response.Close();
            }

            return html;
        }

    }
}
