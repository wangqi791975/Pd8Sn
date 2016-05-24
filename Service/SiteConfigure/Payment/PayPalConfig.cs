using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure.Payment
{
    [Serializable]
   public class PayPalConfig
    {
        /// <summary>
        /// api账号
        /// </summary>
        public virtual string ApiUsername { get; set; }
        /// <summary>
        /// api密码
        /// </summary>
        public virtual string ApiPassword { get; set; }

        /// <summary>
        /// api验证key
        /// </summary>
        public virtual string ApiSignature { get; set; }


        /// <summary>
        /// 接口请求，如：https://api-3t.sandbox.paypal.com/nvp
        /// </summary>
       public virtual string ApiEndpoint { get; set; }

        /// <summary>
        /// 是否允许通过路由代理请求，默认false
        /// </summary>
        public virtual string UseProxy { get; set; }

        /// <summary>
        /// 代理host，默认127.0.0.1
        /// </summary>
        public virtual string ProxyHost { get; set; }

        /// <summary>
        /// 代理端口，默认808
        /// </summary>
        public virtual string ProxyPort { get; set; }

        /// <summary>
        /// 付款信息提交地址，如：https://www.sandbox.paypal.com/webscr&cmd=_express-checkout&token=
        /// </summary>
        public virtual string PaypalUrl { get; set; }

        /// <summary>
        /// 接口版本号，如：65.1
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// 付款成功后浏览器跳转URL(含有POST信息)，但用户可能直接关闭浏览器
        /// </summary>
        public virtual string ResponseUrl { get; set; }

        /// <summary>
        /// 付款成功后alipay异步通知地址（含有POST信息）
        /// </summary>
        public virtual string ResponseUrlNotify { get; set; }

    }
}
