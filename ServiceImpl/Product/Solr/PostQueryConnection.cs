using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Com.Panduo.Common;
using HttpWebAdapters;
using HttpWebAdapters.Adapters;
using SolrNet;
using SolrNet.Exceptions;
using SolrNet.Impl;
using SolrNet.Utils;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// POST提交方式查询连接
    /// </summary>
    public class PostQueryConnection : ISolrConnection
    {
        private string serverURL;
        private string version = "";//2.2

        // Methods
        public PostQueryConnection(string serverURL)
        {
            this.ServerURL = serverURL;
            this.Timeout = -1;
            this.Cache = new NullCache();
            this.HttpWebRequestFactory = new HttpWebRequestFactory();
        }

        private static void CopyTo(Stream input, Stream output)
        {
            int num;
            byte[] buffer = new byte[0x1000];
            while ((num = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, num);
            }
        }

        public string Get(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return Post(relativeUrl, parameters);
        }

        //public string Get(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        //{
        //    string data;
        //    UriBuilder builder = new UriBuilder(this.serverURL);
        //    builder.Path = builder.Path + relativeUrl;
        //    builder.Query = this.GetQuery(parameters);
        //    IHttpWebRequest request = this.HttpWebRequestFactory.Create(builder.Uri);
        //    request.Method = HttpWebRequestMethod.GET;
        //    request.KeepAlive = true;
        //    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        //    SolrCacheEntity entity = this.Cache[builder.Uri.ToString()];
        //    if (entity != null)
        //    {
        //        request.Headers.Add(HttpRequestHeader.IfNoneMatch, entity.ETag);
        //    }
        //    if (this.Timeout > 0)
        //    {
        //        request.ReadWriteTimeout = this.Timeout;
        //        request.Timeout = this.Timeout;
        //    }
        //    try
        //    {
        //        SolrResponse response = this.GetResponse(request);
        //        if (response.ETag != null)
        //        {
        //            this.Cache.Add(new SolrCacheEntity(builder.Uri.ToString(), response.ETag, response.Data));
        //        }
        //        data = response.Data;
        //    }
        //    catch (WebException exception)
        //    {
        //        if (exception.Response != null)
        //        {
        //            using (exception.Response)
        //            {
        //                HttpWebResponseAdapter adapter = new HttpWebResponseAdapter(exception.Response);
        //                if (adapter.StatusCode == HttpStatusCode.NotModified)
        //                {
        //                    return entity.Data;
        //                }
        //                using (Stream stream = exception.Response.GetResponseStream())
        //                {
        //                    using (StreamReader reader = new StreamReader(stream))
        //                    {
        //                        throw new SolrConnectionException(reader.ReadToEnd(), exception, builder.Uri.ToString());
        //                    }
        //                }
        //            }
        //        }
        //        throw new SolrConnectionException(exception, builder.Uri.ToString());
        //    }
        //    return data;
        //}

        private string Get(UriBuilder builder)
        {
            string data;
            IHttpWebRequest request = this.HttpWebRequestFactory.Create(builder.Uri);
            request.Method = HttpWebRequestMethod.GET;
            request.KeepAlive = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            SolrCacheEntity entity = this.Cache[builder.Uri.ToString()];
            if (entity != null)
            {
                request.Headers.Add(HttpRequestHeader.IfNoneMatch, entity.ETag);
            }
            if (this.Timeout > 0)
            {
                request.ReadWriteTimeout = this.Timeout;
                request.Timeout = this.Timeout;
            }
            try
            {
                SolrResponse response = this.GetResponse(request);
                if (response.ETag != null)
                {
                    this.Cache.Add(new SolrCacheEntity(builder.Uri.ToString(), response.ETag, response.Data));
                }
                data = response.Data;
            }
            catch (WebException exception)
            {
                if (exception.Response != null)
                {
                    using (exception.Response)
                    {
                        HttpWebResponseAdapter adapter = new HttpWebResponseAdapter(exception.Response);
                        if (adapter.StatusCode == HttpStatusCode.NotModified)
                        {
                            return entity.Data;
                        }
                        using (Stream stream = exception.Response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                throw new SolrConnectionException(reader.ReadToEnd(), exception, builder.Uri.ToString());
                            }
                        }
                    }
                }
                throw new SolrConnectionException(exception, builder.Uri.ToString());
            }
            return data;
        }

        private string GetQuery(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            List<KeyValuePair<string, string>> source = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                source.AddRange(parameters);
            }

            if (!this.version.IsNullOrEmpty())
            { 
                source.Add(KV.Create<string, string>("version", this.version));
            }

            StringBuilder queryString = new StringBuilder();
            foreach (var kv in source)
            {
                queryString.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value));
            }
            return queryString.Remove(0, 1).ToString();
            //return string.Join("&", source.Select<KeyValuePair<string, string>, KeyValuePair<string, string>>(delegate(KeyValuePair<string, string> kv) {
            //    return KV.Create<string, string>(HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value));
            //}).Select<KeyValuePair<string, string>, string>(delegate(KeyValuePair<string, string> kv)
            //{
            //    return string.Format("{0}={1}", kv.Key, kv.Value);
            //}).ToArray<string>());
        }

        private SolrResponse GetResponse(IHttpWebRequest request)
        {
            using (IHttpWebResponse response = request.GetResponse())
            {
                string eTag = response.Headers[HttpResponseHeader.ETag];
                string str2 = response.Headers[HttpResponseHeader.CacheControl];
                if ((str2 != null) && str2.Contains("no-cache"))
                {
                    eTag = null;
                }
                return new SolrResponse(eTag, this.ReadResponseToString(response));
            }
        }

        public string Post2(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(GetQuery(parameters))))
            {
                return this.PostStream(relativeUrl, "application/x-www-form-urlencoded", stream, null);
            }
        }

        public string Post(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            UriBuilder builder = new UriBuilder(serverURL);
            builder.Path += relativeUrl;
            string q = GetQuery(parameters);

            /*URL的最大限制:
             * IE:2083
             * Firefox:65536
             * Safari:80000
             * Opera:190000
             * chrome:8182
             */
            if ((builder.Uri.ToString().Length + q.Length) <= 2000)
            {
                //使用GET方式
                builder.Query = q;
                return Get(builder);
            }

            IHttpWebRequest request = HttpWebRequestFactory.Create(builder.Uri);
            #region Added to set the post data

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = HttpWebRequestMethod.POST;

            byte[] data = Encoding.UTF8.GetBytes(q);
            request.ContentLength = data.Length;
            Stream os = request.GetRequestStream();
            os.Write(data, 0, data.Length);
            os.Close();
            #endregion

            request.KeepAlive = true;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            var cached = Cache[builder.Uri.ToString()];
            if (cached != null)
            {
                request.Headers.Add(HttpRequestHeader.IfNoneMatch, cached.ETag);
            }
            if (Timeout > 0)
            {
                request.ReadWriteTimeout = Timeout;
                request.Timeout = Timeout;
            }
            try
            {
                var response = GetResponse(request);
                if (response.ETag != null) Cache.Add(new SolrCacheEntity(builder.Uri.ToString(), response.ETag, response.Data));
                return response.Data;
            }
            catch (WebException e)
            {
                string message = e.Message;
                if (e.Response != null)
                {
                    using (Stream stream2 = e.Response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            message = reader.ReadToEnd();
                        }
                    }
                }
                throw new SolrConnectionException(message, e, request.RequestUri.ToString());
            }

        }
        public string Post(string relativeUrl, string s)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                return this.PostStream(relativeUrl, "text/xml; charset=utf-8", stream, null);
            }
        }

        public string PostStream(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            string data;
            UriBuilder builder = new UriBuilder(this.serverURL);
            builder.Path = builder.Path + relativeUrl;
            builder.Query = this.GetQuery(parameters);
            IHttpWebRequest request = this.HttpWebRequestFactory.Create(builder.Uri);
            request.Method = HttpWebRequestMethod.POST;
            request.KeepAlive = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            if (this.Timeout > 0)
            {
                request.ReadWriteTimeout = this.Timeout;
                request.Timeout = this.Timeout;
            }
            if (contentType != null)
            {
                request.ContentType = contentType;
            }
            request.ContentLength = content.Length;
            request.ProtocolVersion = HttpVersion.Version11;
            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    CopyTo(content, stream);
                }
                data = this.GetResponse(request).Data;
            }
            catch (WebException exception)
            {
                string message = exception.Message;
                if (exception.Response != null)
                {
                    using (Stream stream2 = exception.Response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            message = reader.ReadToEnd();
                        }
                    }
                }
                throw new SolrConnectionException(message, exception, request.RequestUri.ToString());
            }
            return data;
        }

        private string ReadResponseToString(IHttpWebResponse response)
        {
            string str;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream, this.TryGetEncoding(response)))
                {
                    str = reader.ReadToEnd();
                }
            }
            return str;
        }

        private Encoding TryGetEncoding(IHttpWebResponse response)
        {
            try
            {
                return Encoding.GetEncoding(response.CharacterSet);
            }
            catch
            {
                return Encoding.UTF8;
            }
        }

        // Properties
        public ISolrCache Cache
        {
            set;
            get;
        }

        public IHttpWebRequestFactory HttpWebRequestFactory { set; get; }

        public string ServerURL
        {
            get
            {
                return this.serverURL;
            }
            set
            {
                this.serverURL = UriValidator.ValidateHTTP(value);
            }
        }

        public int Timeout { get; set; }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }


        private struct SolrResponse
        {
            public string ETag { get; private set; }
            public string Data { get; private set; }
            public SolrResponse(string eTag, string data)
                : this()
            {
                ETag = eTag;
                Data = data;
            }
        }


    }


}
