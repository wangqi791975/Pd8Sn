using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Com.Panduo.Web.PaymentCommon.Common;
using Com.Panduo.Web.PaymentCommon.PayConfig;
using Com.Panduo.Web.PaymentCommon.PayInfo;
using Com.Panduo.Web.PaymentCommon.Service.Exception;
using Com.Panduo.Web.PaymentCommon.Service.Parm;
using Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using com.paypal.sdk.util;
using log4net;

namespace Com.Panduo.Web.PaymentCommon.Service
{
    /// <summary>
    /// Paypal支付服务（标准支付/快速结账支付）
    /// </summary>
    public class PaypalService
    {
        #region 标准支付
        /// <summary>
        /// 标准支付日志
        /// </summary>
        private static ILog PaypalLogger = PaymentLoggerHelper.GetLogger(PaymentLoggerType.Paypal);
        /// <summary>
        /// Paypal标准支付配置
        /// </summary>
        private static readonly PaypalConfig PaypalConfig = PaymentConfigHelper.PaypalConfig;
        /// <summary>
        /// 验证“txn_id”是否有效
        /// 检查“txn_id”是否未重复，以防止欺诈者重复使用旧的已完成的交易
        /// </summary>
        /// <param name="txnId">交易ID</param>
        /// <returns></returns>
        public delegate bool ValidateTxnId(string txnId);

        /// <summary>
        /// 验证PayPal数据
        /// </summary>
        /// <param name="formData">Paypal Post过来的Form</param>
        /// <param name="validateTxnId">额外的支付宝交易号验证</param>
        /// <returns></returns>
        public static PaypalNotifyResult PaypalNotifyVerify(NameValueCollection formData, ValidateTxnId validateTxnId)
        {
            return PaypalNotifyVerify(formData, validateTxnId,Encoding.UTF8);
        }

        /// <summary>
        /// 验证PayPal数据
        /// </summary>
        /// <param name="formData">Paypal Post过来的Form</param>
        /// <param name="validateTxnId">额外的支付宝交易号验证</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static PaypalNotifyResult PaypalNotifyVerify(NameValueCollection formData, ValidateTxnId validateTxnId,Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var fromQueryString = formData.ToQueryString(encoding);
            PaypalLogger.InfoFormat("PaypalNotifyVerify - 步骤1 - Post的参数:{0}", fromQueryString);

            var paypalNotifyResult = new PaypalNotifyResult();

            /*如果返回值为验证通过标记则把传递过来的参数存入到数据库
            确认“payment_status”为“Completed”因为系统也会为其他结果（如“Pending”或“Failed”）发送 IPN。
            检查“txn_id”是否未重复，以防止欺诈者重复使用旧的已完成的交易
            验证“receiver_email”是已在您的PayPal账户中注册的电子邮件地址验证其他信息
             */

            //string postData = formData.ToString() + "&cmd=_notify-validate";

            var postData = fromQueryString + "&cmd=_notify-validate";
            var html = Request(PaypalConfig.SubmitUrl, postData, encoding);

            if (html.ToUpper() == "VERIFIED")
            {
                var returnInfo = new PaypalInfo
                {
                    IsExpressCheckOut = false,
                    McGross = formData["mc_gross"].ParseTo(0.00M),
                    AddressStatus = formData["address_status"] ?? string.Empty,
                    PayerId = formData["payer_id"] ?? string.Empty,
                    Tax = formData["tax"] != null ? Convert.ToDecimal(formData["tax"]) : 0,
                    AddressStreet1 = formData["address_street"] ?? string.Empty,
                    PaymentDate = formData["payment_date"] ?? string.Empty,
                    PaymentStatus = formData["payment_status"] ?? string.Empty,
                    Charset = formData["charset"] ?? string.Empty,
                    AddressZip = formData["address_zip"] ?? string.Empty,
                    FirstName = formData["first_name"] ?? string.Empty,
                    McFee = formData["mc_fee"] ?? string.Empty,
                    AddressCountryCode = formData["address_country_code"] ?? string.Empty,
                    AddressName = formData["address_name"] ?? string.Empty,
                    NotifyVersion = formData["notify_version"] ?? string.Empty,
                    Custom = formData["custom"] ?? string.Empty,
                    PayerStatus = formData["payer_status"] ?? string.Empty,
                    Business = formData["business"] ?? string.Empty,
                    AddressCountryName = formData["address_country"] ?? string.Empty,
                    AddressCity = formData["address_city"] ?? string.Empty,
                    Quantity = formData["quantity"] ?? string.Empty,
                    VerifySign = formData["verify_sign"] ?? string.Empty,
                    PayerEmail = formData["payer_email"] ?? string.Empty,
                    TxnId = formData["txn_id"] ?? string.Empty,
                    PaymentType = formData["payment_type"] ?? string.Empty,
                    LastName = formData["last_name"] ?? string.Empty,
                    AddressState = formData["address_state"] ?? string.Empty,
                    ReceiverEmail = formData["receiver_email"] ?? string.Empty,
                    PaymentFee = formData["payment_fee"] ?? string.Empty,
                    ReceiverId = formData["receiver_id"] ?? string.Empty,
                    TxnType = formData["txn_type"] ?? string.Empty,
                    ItemName = formData["item_name"] ?? string.Empty,
                    McCurrency = formData["mc_currency"] ?? string.Empty,
                    ItemNumber = formData["item_number"] ?? string.Empty,
                    ResidenceCountry = formData["residence_country"] ?? string.Empty,
                    PaymentGross = formData["payment_gross"] ?? string.Empty,
                    Shipping = formData["shipping"] ?? string.Empty,
                    PendingReason = formData["pending_reason"] ?? string.Empty,
                    ReasonCode = formData["reason_code"] ?? string.Empty,
                    Commonts = formData["memo"] ?? formData["invoice"] ?? string.Empty,
                    Descritpion = formData["memo"] ?? formData["invoice"] ?? string.Empty,
                    TotalInfo = postData
                };

                paypalNotifyResult.PaypalInfo = returnInfo;

                if (returnInfo.PaymentStatus.Trim().ToLower() == PaypalInfo.PAYPAL_STATUS_COMPLETED.ToLower() && (validateTxnId == null || validateTxnId.Invoke(returnInfo.TxnId)))
                {
                    paypalNotifyResult.IsValid = true;

                }
            }

            PaypalLogger.InfoFormat("PaypalNotifyVerify - 步骤3 - 返回验证结果是:{0}", paypalNotifyResult.IsValid);

            return paypalNotifyResult;
        }

        /// <summary>
        /// 请求Paypal验证数据
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <param name="postData">请求的参数</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        private static string Request(string url, string postData, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            PaypalLogger.InfoFormat("PaypalNotifyVerify - 步骤2 - 回调Paypal请求验证 - 请求的url:{0} - 请求参数:{1}",url , postData);

            var html = string.Empty;
            HttpWebResponse response = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                if (!string.IsNullOrEmpty(PaypalConfig.Host))
                {
                    request.Headers.Add("Host", PaypalConfig.Host);
                }
                //request.Accept = "*/*";
                request.ContentType = string.Format("application/x-www-form-urlencoded{0}", Encoding.GetEncoding("gb2312").Equals(encoding) ? ";charset=gb2312" : string.Empty) ;
                byte[] buffer = encoding.GetBytes(postData);
                request.ContentLength = buffer.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
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
                PaypalLogger.Error(string.Format("PaypalNotifyVerify - 步骤2 - 回调Paypal请求验证  - 发生异常 - Request - 请求的url:{0}\n\r - 请求参数:{1}\n\r - 异常信息:{2}\n\r,异常堆栈:{3}", url, postData, ex.Message, ex.StackTrace), ex);
                throw new PayPalException(ex.Message, null, null, null, ex); 
            }
            finally
            {
                if (response != null) response.Close();
            }

            PaypalLogger.InfoFormat("PaypalNotifyVerify - 步骤2 - 回调Paypal请求验证 - 返回验证结果:{0}", html);

            return html;
        }
        #endregion

        #region 快速结账 
        #region 凭证和配置信息
        /// <summary>
        /// 快速支付日志
        /// </summary>
        private static ILog PaypalExpressLogger = PaymentLoggerHelper.GetLogger(PaymentLoggerType.PaypalExpress);
        /// <summary>
        /// Paypal快速支付配置
        /// </summary>
        private static readonly PaypalExpressConfig PaypalExpressConfig = PaymentConfigHelper.PaypalExpressConfig;
        /// <summary>
        /// 快速支付凭证
        /// </summary>
        private static readonly IAPIProfile DefaultProfile = GetPaypalProfile(); 
        /// <summary>
        /// 获取快速支付API接口服务
        /// </summary>
        /// <returns></returns>
        private static NVPCallerServices PaypalExpressApiService = new NVPCallerServices
        {
            APIProfile = DefaultProfile
        };

        /// <summary>
        /// 获取交互使用的凭证
        /// </summary>
        /// <returns></returns>
        private static IAPIProfile GetPaypalProfile()
        {
            IAPIProfile profile = null;
            switch (PaypalExpressConfig.Credential)
            {
                //证书
                case PaypalExpressCredential.Certificate:
                    profile = ProfileFactory.createSSLAPIProfile();
                    profile.APIUsername = PaypalExpressConfig.PaypalCertificate.ApiUsername;
                    profile.APIPassword = PaypalExpressConfig.PaypalCertificate.ApiPassword;
                    profile.Environment = PaypalExpressConfig.Environment;
                    profile.CertificateFile = PaypalExpressConfig.PaypalCertificate.CertificateFile;
                    profile.PrivateKeyPassword = PaypalExpressConfig.PaypalCertificate.PrivateKeyPassword;
                    profile.Subject = PaypalExpressConfig.PaypalCertificate.Subject;
                    break;
                //签名
                case PaypalExpressCredential.Signature:
                    profile = ProfileFactory.createSignatureAPIProfile();
                    profile.APIUsername = PaypalExpressConfig.PaypalSignature.ApiUsername;
                    profile.APIPassword = PaypalExpressConfig.PaypalSignature.ApiPassword;
                    profile.APISignature = PaypalExpressConfig.PaypalSignature.Signature;
                    profile.Environment = PaypalExpressConfig.Environment;
                    profile.Subject = PaypalExpressConfig.PaypalSignature.Subject;
                    break;
                //OAuth
                case PaypalExpressCredential.OAuth:
                    profile = ProfileFactory.createPermissionAPIProfile();
                    profile.APIUsername = PaypalExpressConfig.PaypalOAuth.ApiUsername;
                    profile.Oauth_Signature = PaypalExpressConfig.PaypalOAuth.AuthSignature;
                    profile.Oauth_Token = PaypalExpressConfig.PaypalOAuth.AuthToken;
                    profile.Oauth_Timestamp = PaypalExpressConfig.PaypalOAuth.AuthTimestamp;
                    profile.Environment = PaypalExpressConfig.Environment;
                    profile.CertificateFile = PaypalExpressConfig.PaypalOAuth.CertificateFile;
                    profile.PrivateKeyPassword = PaypalExpressConfig.PaypalOAuth.PrivateKeyPassword;
                    break;
                default:
                    PaypalExpressLogger.ErrorFormat("GetPaypalProfile - unknow PaypalExpress Credential - {0}", PaypalExpressConfig.Credential);
                    throw new PaymentException(string.Format("GetPaypalProfile - unknow PaypalExpress Credential - {0}", PaypalExpressConfig.Credential));
                    break;
            }

            return profile;
        }
        #endregion
        
        #region Pyapal快速结账API接口
        /*
         * 1.调用SetExpressCheckout获取Token
         * 2.调用GetExpressCheckoutDetails获取支付信息
         * 3.调用DoExpressCheckoutPayment扣款
         */
        #region 步骤一:调用SetExpressCheckout取得paypal通信token
        /// <summary>
        /// 步骤一:调用SetExpressCheckout取得paypal通信token
        /// </summary> 
        /// <returns></returns>
        public static PayPalExpressCheckoutResult SetExpressCheckout(PaypalExpressCheckoutParm parm)
        { 
            PaypalExpressLogger.InfoFormat("SetExpressCheckout - 步骤1 - 请求快速支付-开始验证。。。");

            #region 参数校验
            if (parm == null)
            {
                throw new ArgumentNullException("parm");
            }

            //parm.Check(); 
            
            //Paypal限额校验
            CheckPayPalExpressMaxAmount(parm.CurrencyCode, parm.PayAmount);


            PaypalExpressLogger.InfoFormat("SetExpressCheckout - 步骤2 - 快速支付-完成验证，主要参数:币种:{0},支付金额:{1}", parm.CurrencyCode, parm.PayAmount);
            #endregion

            var encoder = new NVPCodec();

            #region 主数据(必填)
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["PAYMENTACTION"] = "Order";//PAYMENTREQUEST_0_PAYMENTACTION
            encoder["REQCONFIRMSHIPPING"] = "1";
            encoder["CALLBACKTIMEOUT"] = "4";
            encoder["RETURNURL"] = parm.SuccessUrl;
            encoder["CANCELURL"] = parm.CancelUrl;
            encoder["HDRIMG"] = parm.LogoUrl ?? string.Empty;
            encoder["CURRENCYCODE"] = parm.CurrencyCode;//PAYMENTREQUEST_0_CURRENCYCODE
            if (!string.IsNullOrEmpty(parm.LocalCode))
            {
                encoder["LOCALECODE"] = parm.LocalCode;
            }

            if (parm.ShippingAmount > 0)
            {
                encoder["SHIPPINGAMT"] = parm.ShippingAmount.ToString("0.00");//PAYMENTREQUEST_0_SHIPPINGAMT
                encoder["SHIPDISCAMT"] = "0.00";//PAYMENTREQUEST_0_SHIPDISCAMT

                encoder["ITEMAMT"] = (parm.PayAmount - parm.ShippingAmount).ToString("0.00");//PAYMENTREQUEST_0_ITEMAMT
            }
            encoder["AMT"] = parm.PayAmount.ToString("0.00"); //PAYMENTREQUEST_0_AMT
            #endregion

            #region 明细数据(可空)
            if (!parm.OrderItems.IsNullOrEmpty())
            {
                var index = 0;
                var indexStr = string.Empty;
                foreach (var item in parm.OrderItems)
                {
                    indexStr = index.ToString();
                    encoder["L_NAME" + indexStr] = item.Name ?? string.Empty;//L_PAYMENTREQUEST_n_NAMEm
                    encoder["L_AMT" + indexStr] = item.Amount.ToString("0.00");//L_PAYMENTREQUEST_n_AMTm
                    encoder["L_QTY" + indexStr] = item.Qty.ToString();//L_PAYMENTREQUEST_n_QTYm
                    if (!string.IsNullOrEmpty(item.Number)) encoder["L_NUMBER" + indexStr] = item.Number;//L_PAYMENTREQUEST_n_NUMBERm
                    if (!string.IsNullOrEmpty(item.Description)) encoder["L_DESC" + indexStr] = item.Description;//L_PAYMENTREQUEST_n_DESCm
                    index++;
                }  
            }
            #endregion

            #region 货运地址数据(可空)
            if (parm.Address != null)
            {
                //encoder["ADDROVERRIDE"] = "1"; //如果不打算使用PayPal返回的地址 
                encoder["SHIPTOCOUNTRYCODE"] = parm.Address.CountryCode ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE
                encoder["SHIPTONAME"] = parm.Address.AddressName ?? string.Empty;//PAYMENTREQUEST_0_SHIPTONAME
                encoder["SHIPTOSTREET"] = parm.Address.Street1 ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTREET
                encoder["SHIPTOSTREET2"] = parm.Address.Street2 ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTREET2
                encoder["SHIPTOPHONENUM"] = parm.Address.Phone ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOPHONENUM
                encoder["SHIPTOCITY"] = parm.Address.City ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOCITY
                encoder["SHIPTOSTATE"] = parm.Address.State ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTATE
                encoder["SHIPTOZIP"] = parm.Address.Zip ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOZIP
                encoder["EMAIL"] = parm.Address.Email ?? string.Empty;
                encoder["ADDRESSSTATUS"] = parm.Address.AddressStatus ?? string.Empty;
            }
            #endregion

            #region 请求并组装返回的数据
            var requestNvp = encoder.Encode();
            NVPCodec decoder = null;
            try
            {
                var responseNvp = PaypalExpressApiService.Call(requestNvp);
                decoder = new NVPCodec();
                decoder.Decode(responseNvp);
            }
            catch (System.Exception ex)
            {
                PaypalExpressLogger.Error(string.Format("SetExpressCheckout - 步骤3 - 快速支付-发生异常 - 异常信息:{0},\n\r异常堆栈:{1}", ex.Message, ex.StackTrace), ex);
                throw new PayPalException(ex.Message, null, encoder, decoder, ex);
            }

            PaypalExpressLogger.InfoFormat("SetExpressCheckout - 步骤3 - 快速支付 - 服务器响应完成,结果是：{0}。", decoder["ACK"]);

            var result = new PayPalExpressCheckoutResult();
            result.Ack = decoder["ACK"];
            if (result.IsSuccess())
            {
                result.Token = decoder["TOKEN"];
                result.PayUrl = GetExpressCheckoutRedirectUrl(result.Token);
                PaypalExpressLogger.InfoFormat("SetExpressCheckout - 步骤4 - 快速支付 - 验证成功,获取到数据：TOKEN:{0}\n\r,Paypal跳转地址是:{1}!", result.Token, result.PayUrl);
            }
            else
            {
                string errorCode;
                var errorMessage = GetErrorMsg(decoder, out errorCode);
                PaypalExpressLogger.ErrorFormat("SetExpressCheckout - 步骤4 - 快速支付 - 验证失败,信息如下:{0}", errorMessage);
                throw new PayPalException(errorMessage, errorCode, encoder, decoder);
            } 
            #endregion

            return result;
        }
        
        /// <summary>
        /// 获取SetExpressCheckout获取Token后重定向到Paypal的Url
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetExpressCheckoutRedirectUrl(string token)
        {
            return string.Format("{0}?cmd=_express-checkout&token={1}", PaypalExpressConfig.ServiceUrl, token);
        }
        /// <summary>
        /// 检查Paypal快速支付是否超额
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="payAmount"></param>
        private static void CheckPayPalExpressMaxAmount(string currencyCode, decimal payAmount)
        {
            if (!PaypalExpressConfig.MaxAmounts.IsNullOrEmpty() && PaypalExpressConfig.MaxAmounts.ContainsKey(currencyCode))
            {
                var maxAmount = PaypalExpressConfig.MaxAmounts[currencyCode];
                if (payAmount > maxAmount)
                {
                    throw new MaxAmountOutOfRangeException(currencyCode, maxAmount,payAmount, string.Format("CheckPayPalExpressMaxAmount - Paypal快速支付超额:币种:{0}最多允许支付:{1},当前支付金额:{2}", currencyCode, maxAmount, payAmount));
                }
            }
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="decoder"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private static string GetErrorMsg(NVPCodec decoder, out string errorCode)
        {
            errorCode = decoder["L_ERRORCODE0"];
            string msg2 = decoder["L_SHORTMESSAGE0"];
            string msg = decoder["L_LONGMESSAGE0"];
            return string.Concat(msg2, " --> ", msg);
        } 

        #endregion

        #region 步骤二:调用GetExpressCheckoutDetails获取支付信息
        /// <summary>
        /// 根据Token获取支付信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static PaypalInfo GetExpressCheckoutDetails(string token)
        {
            PaypalExpressLogger.InfoFormat("GetExpressCheckoutDetails - 步骤1 - 请求根据token获取客户地址信息。。。");

            #region 验证参数
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            } 
            #endregion

            #region 请求数据
            var encoder = new NVPCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            var requestNvp = encoder.Encode();
            NVPCodec decoder = null;
            try
            {
                string responseNvp = PaypalExpressApiService.Call(requestNvp);
                decoder = new NVPCodec();
                decoder.Decode(responseNvp);
            }
            catch (System.Exception ex)
            {
                PaypalExpressLogger.Error(string.Format("GetExpressCheckoutDetails - 步骤2 - 获取客户地址信息-发生异常 - 异常信息:{0},\n\r异常堆栈:{1}", ex.Message, ex.StackTrace), ex);
                throw new PayPalException(ex.Message, null, encoder, decoder, ex);
            }
            
            PaypalExpressLogger.InfoFormat("GetExpressCheckoutDetails - 步骤2 - 获取客户地址信息 - 服务器响应完成,结果是：{0}。", decoder["ACK"]);
            #endregion

            #region 返回数据并组装
            var ack = decoder["ACK"];
            if (IsExpressAckSuccess(ack))
            { 
                PaypalExpressLogger.InfoFormat("SetExpressCheckout - 步骤3 - 快速支付 - 验证成功,组装获取到的数据!");
                return GetExpressCheckoutInfo(token, decoder);
            }
            else
            {
                string errorCode;
                var errorMessage = GetErrorMsg(decoder, out errorCode);
                PaypalExpressLogger.ErrorFormat("GetExpressCheckoutDetails  - 步骤3 - 获取客户地址信息 - 服务器验证失败, 参数:{0}", errorMessage);
                throw new PayPalException(errorMessage, errorCode, encoder, decoder);
            }  
            #endregion
        }
        #endregion

        #region 步骤三:调用DoExpressCheckoutPayment扣款
        public static PaypalInfo DoExpressCheckoutPayment(string token, string payerId, string currencyCode, decimal amount, string orderNo,int? orderId =null)
        {
            PaypalExpressLogger.InfoFormat("DoExpressCheckoutPayment - 步骤1 - 请求扣款,参数:token:{0},payerId:{1},currencyCode:{2},amount:{3},orderNo:{4},orderId:{5}。。。", token, payerId, currencyCode, amount, orderNo,orderId);

            #region 数据验证
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrEmpty(payerId))
            {
                throw new ArgumentNullException("payerId");
            }
            if (amount <= 0) throw new ArgumentOutOfRangeException("amount");

            //Paypal限额校验
            CheckPayPalExpressMaxAmount(currencyCode, amount);  
            #endregion
            
            #region 组装请求参数
            var encoder = new NVPCodec();
            encoder["METHOD"] = "DoExpressCheckoutPayment";
            encoder["PAYMENTACTION"] = "Sale";//PAYMENTREQUEST_0_PAYMENTACTION
            encoder["TOKEN"] = token;
            encoder["PAYERID"] = payerId;
            encoder["CURRENCYCODE"] = currencyCode;//PAYMENTREQUEST_0_CURRENCYCODE
            encoder["AMT"] = amount.ToString("0.00");//PAYMENTREQUEST_0_AMT
            encoder["INVNUM"] = orderNo;//PAYMENTREQUEST_0_INVNUM
            var itemName = string.Format("{0}{1}{2}", PaypalExpressConfig.DescPrefix, orderNo, PaypalExpressConfig.DescSubfix);//PAYMENTREQUEST_0_DESC
            encoder["DESC"] = itemName;
            if (orderId.HasValue)
            {
                encoder["CUSTOM"] = orderId.ToString();// 	PAYMENTREQUEST_0_CUSTOM
            }
            var requestNvp = encoder.Encode(); 
            #endregion

            NVPCodec decoder = null;
            try
            {
                string responseNvp = PaypalExpressApiService.Call(requestNvp);
                decoder = new NVPCodec();
                decoder.Decode(responseNvp);
            }
            catch (System.Exception ex)
            {
                PaypalExpressLogger.Error(string.Format("GetExpressCheckoutDetails - 步骤2 - 请求扣款-发生异常 - 异常信息:{0},\n\r异常堆栈:{1}", ex.Message, ex.StackTrace), ex);
                throw new PayPalException(ex.Message, null, encoder, decoder, ex);
            } 

            var ack = decoder["ACK"];
            PaypalExpressLogger.InfoFormat("GetExpressCheckoutDetails - 步骤2 - 请求扣款 - 服务器响应完成,结果是：{0}。", ack);

            if (IsExpressAckSuccess(ack))
            {
                var item = GetPayPalPayInfo(token,decoder);
                item.IsExpressCheckOut = true;
                item.Token = token;
                item.PayerId = payerId;
                item.ItemName = itemName;
                item.ItemNumber = orderNo; 

                PaypalExpressLogger.InfoFormat("SetExpressCheckout - 步骤3 - 快速支付 - 验证成功,组装获取到的数据!");

                return item;
            }
            else
            {
                string errorCode;
                var errorMessage = GetErrorMsg(decoder, out errorCode);
                PaypalExpressLogger.ErrorFormat("GetExpressCheckoutDetails  - 步骤3 - 请求扣款 - 服务器验证失败, 参数:{0}", errorMessage);
                throw new PayPalException(errorMessage, errorCode, encoder, decoder);
            }
        }
        #endregion

        private static bool IsExpressAckSuccess(string ack)
        {
            if (string.IsNullOrEmpty(ack)) return false;
            ack = ack.ToLower();
            return ack == "success" || ack == "successwithwarning";
        }
        #endregion

        #region 返回结果处理
        /// <summary>
        /// 返回的客户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="decoder"></param>
        /// <returns></returns>
        internal static PaypalInfo GetExpressCheckoutInfo(string token, NVPCodec decoder)
        {
            var info = new PaypalInfo();
            info.IsExpressCheckOut = true;
            info.AddressCountryCode = decoder["SHIPTOCOUNTRYCODE"] ?? decoder["PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE
            info.AddressName = decoder["SHIPTONAME"] ?? decoder["PAYMENTREQUEST_0_SHIPTONAME"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTONAME
            info.AddressStreet1 = decoder["SHIPTOSTREET"] ?? decoder["PAYMENTREQUEST_0_SHIPTOSTREET"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTREET
            info.AddressStreet2 = decoder["SHIPTOSTREET2"] ?? decoder["PAYMENTREQUEST_0_SHIPTOSTREET2"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTREET2
            info.PhoneNumber = decoder["SHIPTOPHONENUM"] ?? decoder["PAYMENTREQUEST_0_SHIPTOPHONENUM"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOPHONENUM
            info.AddressCity = decoder["SHIPTOCITY"] ?? decoder["PAYMENTREQUEST_0_SHIPTOCITY"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOCITY
            info.AddressState = decoder["SHIPTOSTATE"] ?? decoder["PAYMENTREQUEST_0_SHIPTOSTATE"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTATE
            info.AddressZip = decoder["SHIPTOZIP"] ?? decoder["PAYMENTREQUEST_0_SHIPTOZIP"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOZIP
            info.AddressStatus = decoder["ADDRESSSTATUS"] ?? decoder["PAYMENTREQUEST_0_ADDRESSSTATUS"] ?? string.Empty;// 	PAYMENTREQUEST_0_ADDRESSSTATUS
            info.FirstName = decoder["FIRSTNAME"] ?? string.Empty;
            info.LastName = decoder["LASTNAME"] ?? string.Empty; 

            info.Descritpion = decoder["NOTETEXT"] ?? decoder["PAYMENTREQUEST_0_NOTETEXT"] ?? decoder["NOTE"] ?? string.Empty;
            info.Custom = decoder["CUSTOM"] ?? string.Empty;

            info.PayerId = decoder["PAYERID"] ?? string.Empty;
            info.PayerEmail = decoder["EMAIL"] ?? string.Empty;
            info.PayerStatus = decoder["PAYERSTATUS"] ?? string.Empty;

            info.ReceiverEmail = decoder["PAYMENTINFO_0_SELLERPAYPALACCOUNTID"] ?? string.Empty;//PAYMENTINFO_0_SELLERPAYPALACCOUNTID 
            info.ReceiverId = decoder["PAYMENTINFO_0_SELLERID"] ?? string.Empty;//PAYMENTINFO_0_SELLERID
            
            info.Ack = decoder["ACK"] ?? string.Empty;
            info.Token = token;
            info.TotalInfo = GetExpressCheckoutTotalInfo(decoder);

            return info;
        }

        /// <summary>
        /// 获取用拼接起来的所有支付信息
        /// </summary>
        /// <param name="decoder"></param>
        /// <returns></returns>
        internal static string GetExpressCheckoutTotalInfo(NVPCodec decoder)
        {
            if (decoder != null && !decoder.AllKeys.IsNullOrEmpty())
            {
                return decoder.AllKeys.Select(c => string.Format("{0}={1}", c, decoder[c])).Join("&");
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取支付信息
        /// </summary>
        internal static PaypalInfo GetPayPalPayInfo(string token, NVPCodec decoder)
        {
            var info = new PaypalInfo();

            info.McCurrency = decoder["CURRENCYCODE"] ?? string.Empty;//PAYMENTINFO_0_CURRENCYCODE 
            info.McGross = decoder["AMT"].ParseTo(0.00M);//PAYMENTINFO_0_AMT 
            info.McFee = decoder["FEEAMT"] ?? string.Empty;//PAYMENTINFO_0_FEEAMT
            info.Tax = decoder["TAXAMT"].ParseTo(0.00M);//PAYMENTINFO_0_TAXAMT
            info.Shipping = decoder["SHIPPINGAMT"]??string.Empty;//PAYMENTINFO_0_SHIPPINGAMT
            info.PaymentType = decoder["PAYMENTTYPE"] ?? string.Empty;//PAYMENTINFO_0_PAYMENTTYPE
            info.PaymentDate = decoder["ORDERTIME"] ?? string.Empty;//PAYMENTINFO_0_ORDERTIME
            info.PaymentStatus = decoder["PAYMENTSTATUS"] ?? string.Empty;//PAYMENTINFO_0_PAYMENTSTATUS
            info.TxnId = decoder["TRANSACTIONID"] ?? string.Empty;//PAYMENTINFO_0_TRANSACTIONID
            info.TxnType = decoder["TRANSACTIONTYPE"] ?? string.Empty;//PAYMENTINFO_0_TRANSACTIONTYPE
            info.Business = decoder["PAYMENTINFO_0_SECUREMERCHANTACCOUNTID"] ?? string.Empty; //PAYMENTINFO_0_SECUREMERCHANTACCOUNTID

            info.FirstName = decoder["FIRSTNAME"] ?? string.Empty;
            info.LastName = decoder["LASTNAME"] ?? string.Empty;
            info.AddressName = decoder["SHIPTONAME"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTONAME
            info.AddressStatus = decoder["ADDRESSSTATUS"] ?? string.Empty;// 	PAYMENTREQUEST_0_ADDRESSSTATUS
            info.AddressCountryCode = decoder["SHIPTOCOUNTRYCODE"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE
            info.AddressStreet1 = decoder["SHIPTOSTREET"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTREET
            info.AddressStreet2 = decoder["SHIPTOSTREET2"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTREET2
            info.PhoneNumber = decoder["SHIPTOPHONENUM"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOPHONENUM
            info.AddressCity = decoder["SHIPTOCITY"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOCITY
            info.AddressState = decoder["SHIPTOSTATE"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOSTATE
            info.AddressZip = decoder["SHIPTOZIP"] ?? string.Empty;//PAYMENTREQUEST_0_SHIPTOZIP
            
            info.ItemName = decoder["DESC"] ?? string.Empty;//PAYMENTREQUEST_0_DESC
            info.ItemNumber = decoder["INVNUM"] ?? string.Empty;//PAYMENTREQUEST_0_INVNUM
            info.Quantity = "1";
            info.PayerId = decoder["PAYERID"] ?? string.Empty;
            info.PayerEmail = decoder["EMAIL"] ?? string.Empty;
            info.PayerStatus = decoder["PAYERSTATUS"] ?? string.Empty;
            info.ReceiverEmail = decoder["PAYMENTINFO_0_SELLERPAYPALACCOUNTID"] ?? string.Empty;//PAYMENTINFO_0_SELLERPAYPALACCOUNTID 
            info.ReceiverId = decoder["PAYMENTINFO_0_SELLERID"] ?? string.Empty;//PAYMENTINFO_0_SELLERID

            info.Custom = decoder["CUSTOM"] ?? string.Empty;//PAYMENTREQUEST_0_CUSTOM
            info.Commonts = decoder["NOTETEXT"] ?? string.Empty;
            info.Descritpion = decoder["NOTETEXT"] ?? decoder["PAYMENTREQUEST_0_NOTETEXT"] ?? decoder["NOTE"] ?? string.Empty; ;//PAYMENTREQUEST_0_NOTETEXT
            info.PendingReason = decoder["PENDINGREASON"] ?? string.Empty;//PAYMENTINFO_0_PENDINGREASON
            info.ReasonCode = decoder["REASONCODE"] ?? string.Empty;//PAYMENTINFO_0_REASONCODE

            info.Ack = decoder["ACK"] ?? string.Empty;//PAYMENTINFO_0_ACK
            info.Token = token;
            info.TotalInfo = GetExpressCheckoutTotalInfo(decoder);

            return info;
        } 
        #endregion

        #endregion
    }
}