using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Com.Panduo.Web.PaymentCommon.Common;
using Com.Panduo.Web.PaymentCommon.PayConfig;
using Com.Panduo.Web.PaymentCommon.PayInfo;
using Com.Panduo.Web.PaymentCommon.Service.Parm.OceanPayment;
using log4net;

namespace Com.Panduo.Web.PaymentCommon.Service
{
    /// <summary>
    /// 钱海支付服务
    /// </summary>
    public class OceanPaymentService
    {
        #region 配置及常量
        /// <summary>
        /// 钱海支付日志
        /// </summary>
        private static ILog OceanPaymentLogger = PaymentLoggerHelper.GetLogger(PaymentLoggerType.OceanPayment);
        /// <summary>
        /// 钱海支付配置
        /// </summary>
        private static readonly OceanPaymentConfig OceanPaymentConfig = PaymentConfigHelper.OceanPaymentConfig;
        #endregion

        #region 支付方法
        #region 签名
        /// <summary>
        /// 获取请求参数的签名
        /// </summary>
        /// <param name="oceanPaymentParm"></param>
        /// <returns></returns>
        public static string GetRequestSignValue(OceanPaymentParm oceanPaymentParm)
        {
            var orginalValue = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", oceanPaymentParm.Account, oceanPaymentParm.Terminal, oceanPaymentParm.BackUrl, oceanPaymentParm.OrderNumber, oceanPaymentParm.OrderCurrency, oceanPaymentParm.OrderAmount, oceanPaymentParm.BillingAddress.FirstName, oceanPaymentParm.BillingAddress.LastName, oceanPaymentParm.BillingAddress.Email, OceanPaymentConfig.TryGetMethod(oceanPaymentParm.Methods).SecureCode);

            return EncryptData(orginalValue);
        }

        /// <summary>
        /// 获取响应数据签名
        /// </summary>
        /// <param name="oceanPaymentInfo"></param>
        /// <returns></returns>
        public static string GetResponseSignValue(OceanPaymentInfo oceanPaymentInfo)
        {
            var orginalValue = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", oceanPaymentInfo.Account, oceanPaymentInfo.Terminal, oceanPaymentInfo.OrderNumber, oceanPaymentInfo.OrderCurrency, oceanPaymentInfo.OrderAmount, oceanPaymentInfo.OrderNotes, oceanPaymentInfo.CardNumber, oceanPaymentInfo.PaymentId, oceanPaymentInfo.PaymentAuthType, oceanPaymentInfo.PaymentStatus, oceanPaymentInfo.PaymentRisk, OceanPaymentConfig.TryGetMethod(oceanPaymentInfo.Method).SecureCode);

            return EncryptData(orginalValue);
        }

        /// <summary>
        /// 使用SHA256加密算法
        /// </summary>
        /// <returns></returns>
        public static string EncryptData(string strData)
        {
            var orginalBytes = System.Text.Encoding.Default.GetBytes(strData);
            var sha256Service = new System.Security.Cryptography.SHA256Managed();
            var sha256Bytes = sha256Service.ComputeHash(orginalBytes);

            var resultString = sha256Bytes.Select(c => c.ToString("X2")).Join("");

            return resultString;
        }

        /// <summary>
        /// 使用SHA256加密算法
        /// </summary>
        /// <returns></returns>
        public static string EncryptData2(string strData)
        {
            System.Security.Cryptography.SHA256 sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] sha256Bytes = System.Text.Encoding.Default.GetBytes(strData);
            byte[] cryString = sha256.ComputeHash(sha256Bytes);
            string sha256Str = string.Empty;
            for (int i = 0; i < cryString.Length; i++)
            {
                sha256Str += cryString[i].ToString("X2");
            }

            return sha256Str;
        }
        #endregion

        #region 支付校验
        /// <summary>
        /// 验证交易号是否重复代理
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public delegate bool ValidatePaymentId(string paymentId);

        /// <summary>
        /// 钱海支付验证
        /// </summary>
        /// <param name="formData">Post过来的数据</param>
        /// <param name="validatePaymentId">钱海支付唯一交易号的额外校验</param>
        /// <returns></returns>
        public static OceanPaymentResult OceanPaymentVerify(NameValueCollection formData, ValidatePaymentId validatePaymentId)
        {
            var fromQueryString = formData.ToQueryString();
            OceanPaymentLogger.InfoFormat("OceanPaymentVerify - 步骤1 - Post的参数:{0}", fromQueryString);

            var oceanPaymentResult = new OceanPaymentResult();

            #region 获取组装数据
            var returnInfo = new OceanPaymentInfo
            {
                ResponseType = formData["response_type"] ?? string.Empty,
                Method = formData["methods"] ?? string.Empty,//Webmoney、Yandex、Credit Card、QiWi
                Account = formData["account"] ?? string.Empty,
                Terminal = formData["terminal"] ?? string.Empty,
                SignValue = formData["signValue"] ?? string.Empty,
                OrderNumber = formData["order_number"] ?? string.Empty,
                OrderCurrency = formData["order_currency"] ?? string.Empty,
                OrderAmount = formData["order_amount"].ParseTo(0.00M),
                OrderNotes = formData["order_notes"] ?? string.Empty,
                CardNumber = formData["card_number"] ?? string.Empty,
                PaymentId = formData["payment_id"] ?? string.Empty,
                PaymentAuthType = formData["payment_authType"] ?? string.Empty,
                PaymentStatus = formData["payment_status"] ?? string.Empty,
                PaymentDetails = formData["payment_details"] ?? string.Empty,
                PaymentRisk = formData["payment_risk"] ?? string.Empty,
                TotalInfo = fromQueryString
            };

            if (returnInfo.Method.IsNullOrEmpty())
            {
                returnInfo.Method = "QiWi";
            }

            returnInfo.ErrorCode = returnInfo.PaymentDetails.Trim().Left(5);

            oceanPaymentResult.OceanPaymentInfo = returnInfo;
            #endregion

            #region 验证数据
            //验证签名数据-判断是否被篡改过
            var signValue = GetResponseSignValue(returnInfo);
            if (string.Equals(signValue, returnInfo.SignValue, StringComparison.InvariantCultureIgnoreCase))
            { 
                OceanPaymentLogger.InfoFormat("OceanPaymentVerify - 步骤2 - 签名验证通过...");
                if (returnInfo.PaymentStatus == OceanPaymentInfo.OCEANPAYMENT_PAY_STATUS_SUCCESS && (validatePaymentId == null || validatePaymentId.Invoke(returnInfo.PaymentId)))
                {
                    oceanPaymentResult.IsValid = true;
                }
                else
                {
                    OceanPaymentLogger.InfoFormat("OceanPaymentVerify - 步骤2 - 校验状态校验失败,返回的交易状态是:{0}", returnInfo.PaymentStatus);
                }
            }
            else
            {
                OceanPaymentLogger.InfoFormat("OceanPaymentVerify - 步骤2 - 数据校验失败，原因是数据返回的签名被篡改,Post过来的签名为:{0},而数据签名为:{1}", returnInfo.SignValue, signValue);
            }
            #endregion

            OceanPaymentLogger.InfoFormat("OceanPaymentVerify - 步骤3 - 返回验证结果是:{0}", oceanPaymentResult.IsValid);

            return oceanPaymentResult;
        }
        
        #endregion
        #endregion
    }
}