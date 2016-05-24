using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment.PayConfig
{
    /// <summary>
    /// Paypal快速支付配置
    /// </summary>
    [Serializable]
    public class PaypalExpressConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 环境
        /// </summary>
        public virtual string Environment { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public virtual string ServiceUrl { get; set; }
        /// <summary>
        /// 主机头
        /// </summary>
        public virtual string Host { get; set; }
        /// <summary>
        /// Logo地址
        /// </summary>
        public virtual string LogoUrl { get; set; }
        /// <summary>
        /// 描述前缀
        /// </summary>
        public virtual string DescPrefix { get; set; }
        /// <summary>
        /// 描述后缀
        /// </summary>
        public virtual string DescSubfix { get; set; }
        /// <summary>
        /// 使用的认证方式
        /// </summary>
        public virtual PaypalExpressCredential Credential { get; set; }
        /// <summary>
        /// 支付限额,Key为币种标准码（USD、AUD),Value为对应的限额
        /// </summary>
        public virtual IDictionary<string, decimal> MaxAmounts { get; set; }
        /// <summary>
        /// 快速结账认证方式 - 证书配置
        /// </summary>
        public virtual PaypalCredentialCertificate PaypalCertificate { get; set; }
        /// <summary>
        /// 快速结账认证方式 - 签名配置
        /// </summary>
        public virtual PaypalCredentialSignature PaypalSignature { get; set; }
        /// <summary>
        /// 快速结账认证方式 - OAuth配置
        /// </summary>
        public virtual PaypalCredentialOAuth PaypalOAuth { get; set; }
    }

    /// <summary>
    /// Paypal快速结账认证方式
    /// </summary>
    public enum PaypalExpressCredential
    {
        /// <summary>
        /// 证书
        /// </summary>
        Certificate,
        /// <summary>
        /// 签名
        /// </summary>
        Signature,
        /// <summary>
        /// OAuth
        /// </summary>
        OAuth
    }

    /// <summary>
    /// Paypal快速结账认证方式 - 证书
    /// </summary>
    public class PaypalCredentialCertificate
    {
        /// <summary>
        /// Api用户名
        /// </summary>
        public virtual string ApiUsername { get; set; }
        /// <summary>
        ///  Api密码
        /// </summary>
        public virtual string ApiPassword { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public virtual string CertificateFile { get; set; }
        /// <summary>
        /// 证书秘钥
        /// </summary>
        public virtual string PrivateKeyPassword { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public virtual string Subject { get; set; }
    }

    /// <summary>
    /// Paypal快速结账认证方式 - 签名
    /// </summary>
    public class PaypalCredentialSignature
    {
        /// <summary>
        /// Api用户名
        /// </summary>
        public virtual string ApiUsername { get; set; }
        /// <summary>
        ///  Api密码
        /// </summary>
        public virtual string ApiPassword { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public virtual string Signature { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public virtual string Subject { get; set; }
    }

    /// <summary>
    /// Paypal快速结账认证方式 - OAuth
    /// </summary>
    public class PaypalCredentialOAuth
    {
        /// <summary>
        /// Api用户名
        /// </summary>
        public virtual string ApiUsername { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public virtual string AuthToken { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public virtual string AuthSignature { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public virtual string AuthTimestamp { get; set; }
        /// <summary>
        /// 证书
        /// </summary>
        public virtual string CertificateFile { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public virtual string PrivateKeyPassword { get; set; }
    }
}
