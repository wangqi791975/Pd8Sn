using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Com.Panduo.Web.PaymentCommon.PayConfig;

namespace Com.Panduo.Web.PaymentCommon.Common
{
    /// <summary>
    /// 支付配置辅助类
    /// </summary>
    internal static class PaymentConfigHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static PaymentConfigHelper()
        {
            //支付配置文件位置
            var configFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/Payment.config");

            //加载配置文件  
            var rootElement = XElement.Load(configFileFullPath);

            #region Paypal配置
            var paypalElement = rootElement.Element("Paypal"); 
            PaypalConfig = new PaypalConfig();
            PaypalConfig.IsEnable = ((string)paypalElement.Attribute("enable")).ParseTo(false);
            PaypalConfig.Account = (string)paypalElement.Element("Account");
            PaypalConfig.SubmitUrl = (string)paypalElement.Element("SubmitUrl"); 
            PaypalConfig.Host = (string)paypalElement.Element("Host"); 
            PaypalConfig.LogoUrl = (string)paypalElement.Element("LogoUrl");
            PaypalConfig.DescPrefix = (string)paypalElement.Element("DescPrefix");
            PaypalConfig.DescSubfix = (string)paypalElement.Element("DescSubfix");
            PaypalConfig.MaxAmounts = paypalElement.Descendants("MaxAmount").ToDictionary(k => (string)k.Attribute("currency"), v => ((string)v).ParseTo(decimal.MaxValue));
            #endregion

            #region Pyapal快速支付配置
            var paypalExpressElement = rootElement.Element("PaypalExpress"); 
            PaypalExpressConfig = new PaypalExpressConfig();
            PaypalExpressConfig.IsEnable = ((string)paypalExpressElement.Attribute("enable")).ParseTo(false);
            PaypalExpressConfig.Environment = (string)paypalExpressElement.Element("Environment");
            PaypalExpressConfig.ServiceUrl = (string)paypalExpressElement.Element("ServiceUrl");
            PaypalExpressConfig.Host = (string)paypalExpressElement.Element("Host"); 
            PaypalExpressConfig.LogoUrl = (string)paypalExpressElement.Element("LogoUrl");
            PaypalExpressConfig.DescPrefix = (string)paypalExpressElement.Element("DescPrefix");
            PaypalExpressConfig.DescSubfix = (string)paypalExpressElement.Element("DescSubfix");
            PaypalExpressConfig.Credential = EnumHelper.ToEnum<PaypalExpressCredential>((string)paypalExpressElement.Element("Credential")); 
            
            PaypalExpressConfig.PaypalCertificate = new PaypalCredentialCertificate();
            var certificateExpressElement = paypalExpressElement.Element("Credentials").Element("Certificate");
            PaypalExpressConfig.PaypalCertificate.ApiUsername = (string)certificateExpressElement.Element("ApiUsername");
            PaypalExpressConfig.PaypalCertificate.ApiPassword = (string)certificateExpressElement.Element("ApiPassword");
            PaypalExpressConfig.PaypalCertificate.CertificateFile = (string)certificateExpressElement.Element("CertificateFile");
            PaypalExpressConfig.PaypalCertificate.PrivateKeyPassword = (string)certificateExpressElement.Element("PrivateKeyPassword");
            PaypalExpressConfig.PaypalCertificate.Subject = (string)certificateExpressElement.Element("Subject");
            
            PaypalExpressConfig.PaypalSignature = new PaypalCredentialSignature();
            var signatureExpressElement = paypalExpressElement.Element("Credentials").Element("Signature");
            PaypalExpressConfig.PaypalSignature.ApiUsername = (string)signatureExpressElement.Element("ApiUsername");
            PaypalExpressConfig.PaypalSignature.ApiPassword = (string)signatureExpressElement.Element("ApiPassword");
            PaypalExpressConfig.PaypalSignature.Signature = (string)signatureExpressElement.Element("Signature");
            PaypalExpressConfig.PaypalSignature.Subject = (string)signatureExpressElement.Element("Subject");
     
            PaypalExpressConfig.PaypalOAuth = new PaypalCredentialOAuth();
            var oAuthExpressElement = paypalExpressElement.Element("Credentials").Element("OAuth");
            PaypalExpressConfig.PaypalOAuth.ApiUsername = (string)oAuthExpressElement.Element("ApiUsername");
            PaypalExpressConfig.PaypalOAuth.AuthToken = (string)oAuthExpressElement.Element("AuthToken");
            PaypalExpressConfig.PaypalOAuth.AuthSignature = (string)oAuthExpressElement.Element("AuthSignature");
            PaypalExpressConfig.PaypalOAuth.AuthTimestamp = (string)oAuthExpressElement.Element("AuthTimestamp");
            PaypalExpressConfig.PaypalOAuth.CertificateFile = (string)oAuthExpressElement.Element("CertificateFile");
            PaypalExpressConfig.PaypalOAuth.PrivateKeyPassword = (string)oAuthExpressElement.Element("PrivateKeyPassword");
            
            PaypalExpressConfig.MaxAmounts = paypalExpressElement.Descendants("MaxAmount").ToDictionary(k => (string)k.Attribute("currency"), v => ((string)v).ParseTo(decimal.MaxValue));
            #endregion

            #region GC支付配置
            var gcElement = rootElement.Element("GlobalCollect"); 
            GlobalCollectConfig = new GlobalCollectConfig();
            GlobalCollectConfig.IsEnable = ((string)gcElement.Attribute("enable")).ParseTo(false);
            GlobalCollectConfig.ServiceUrl = (string)gcElement.Element("ServiceUrl");
            GlobalCollectConfig.Version = (string)gcElement.Element("Version");
            GlobalCollectConfig.LanguageCode = (string)gcElement.Element("LanguageCode");
            GlobalCollectConfig.HostedIndicator = (string)gcElement.Element("HostedIndicator");
            GlobalCollectConfig.Cvvindicator =  (string)gcElement.Element("Cvvindicator");
            GlobalCollectConfig.MerchantId = (string)gcElement.Element("MerchantId");
            GlobalCollectConfig.IpAddress =  (string)gcElement.Element("IpAddress");

            GlobalCollectConfig.MaxAmounts = gcElement.Descendants("MaxAmount").ToDictionary(k => (string)k.Attribute("currency"), v => ((string)v).ParseTo(decimal.MaxValue));
            #endregion

            #region 钱海支付配置
            var oceanPaymentElement = rootElement.Element("OceanPayment"); 
            OceanPaymentConfig = new OceanPaymentConfig();
            OceanPaymentConfig.ServiceUrl = (string)oceanPaymentElement.Element("ServiceUrl");
            OceanPaymentConfig.MethodMap = oceanPaymentElement.Descendants("Add").Select(c => new OceanPaymentMethod
            {
                Method = (string) c.Attribute("Method"),
                Account = (string) c.Attribute("Account"),
                Terminal = (string) c.Attribute("Terminal"),
                SecureCode = (string) c.Attribute("SecureCode"),
                IsEnable = ((string) c.Attribute("Enable")).ParseTo(false),

            }).ToDictionary(k=>k.Method,v=>v);
            
            OceanPaymentConfig.MaxAmounts = oceanPaymentElement.Descendants("MaxAmount").ToDictionary(k => (string)k.Attribute("currency"), v => ((string)v).ParseTo(decimal.MaxValue));
            #endregion 
        }

        /// <summary>
        /// Paypal标准支付配置
        /// </summary>
        public static PaypalConfig PaypalConfig = null;
        /// <summary>
        /// Paypal快速支付配置
        /// </summary>
        public static PaypalExpressConfig PaypalExpressConfig = null;
        /// <summary>
        /// GC支付配置
        /// </summary>
        public static GlobalCollectConfig GlobalCollectConfig = null;
        /// <summary>
        /// 钱海支付配置
        /// </summary>
        public static OceanPaymentConfig OceanPaymentConfig = null;
    }
}
