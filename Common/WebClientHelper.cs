using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Com.Panduo.Common
{
    /// <summary>
    /// WebClient重载
    /// </summary>
    public class WebClientHelper :WebClient
    {
        private int _timeOut = 10;

        /// <summary>
        /// 过期时间(单位：秒)
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeOut;
            }
            set
            {
                if (value <= 0)
                    _timeOut = 10;
                _timeOut = value;
            }
        }

        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = 1000 * Timeout;
            request.ReadWriteTimeout = 1000 * Timeout;
            return request;
        }

    }
}
