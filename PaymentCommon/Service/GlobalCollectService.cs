using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Com.Panduo.Web.PaymentCommon.Common;
using Com.Panduo.Web.PaymentCommon.PayConfig;
using Com.Panduo.Web.PaymentCommon.PayInfo;
using Com.Panduo.Web.PaymentCommon.Service.Exception;
using Com.Panduo.Web.PaymentCommon.Service.Parm.GlobalCollect;
using log4net;

namespace Com.Panduo.Web.PaymentCommon.Service
{
    /// <summary>
    /// GC支付服务
    /// </summary>
    public class GlobalCollectService
    {
        #region 配置及常量信息
        /// <summary>
        /// GC支付日志
        /// </summary>
        private static ILog GcLogger = PaymentLoggerHelper.GetLogger(PaymentLoggerType.Gc); 

        private static readonly string RESULT_STATE_OK = "OK"; 

        /// <summary>
        /// 金额超限错误代码
        /// </summary>
        public static readonly string ERROR_CODE_OVERFLOWER_AMOUNT = "410120";

        /// <summary>
        /// GC配置参数
        /// </summary>
        private static GlobalCollectConfig GlobalCollectConfig = PaymentConfigHelper.GlobalCollectConfig;
        #endregion

        #region 支付步骤
        #region 步骤一：请求GC支付获得交易支付地址(GlobalCollectParm.payUrl)
        /// <summary>
        /// 请求GC支付，主要得到GC支付地址GlobalCollectParm.PayUrl
        /// </summary>
        /// <param name="parm"></param>
        public static GlobalCollectParm InsertOrderWithPayment(GlobalCollectParm parm)
        {
            GcLogger.InfoFormat("InsertOrderWithPayment - 步骤1 - 请求GC支付-开始验证。。。");
            #region 数据验证 
            if (parm == null)
            {
                throw new ArgumentNullException("parm");
            }
            if (string.IsNullOrEmpty(parm.WebOrderId))
            {
                throw new ArgumentNullException("WebOrderId");
            }
            if (string.IsNullOrEmpty(parm.GcOrderId))
            {
                throw new ArgumentNullException("GcOrderId");
            }
            if (string.IsNullOrEmpty(parm.CurrencyCode))
            {
                throw new ArgumentNullException("CurrencyCode");
            }

            if (parm.Amount <= 0)
            {
                throw new ArgumentOutOfRangeException("Amount");
            }

            CheckGcMaxAmount(parm.CurrencyCode, parm.Amount);
            
            GcLogger.InfoFormat("InsertOrderWithPayment - 步骤2 - GC支付-完成验证，主要参数:币种:{0},订单号:{1},GC订单号:{2}", parm.CurrencyCode, parm.WebOrderId, parm.GcOrderId);
            #endregion

            #region 组装请求参数
            //由于GC中的金额不允许小数点，需要将小数点右移2位，比如1234.56需要变成123456,数据返回的时候需要将123456左移2位变成1234.56
            var amountInCents = GetAmountToCents(parm.Amount);

            var sbXml = new StringBuilder(); 
            sbXml.Append("<XML><REQUEST>");
            sbXml.Append("<ACTION>INSERT_ORDERWITHPAYMENT</ACTION>");
            sbXml.Append("<META>");
            sbXml.AppendFormat("<MERCHANTID>{0}</MERCHANTID>", GlobalCollectConfig.MerchantId);
            sbXml.AppendFormat("<IPADDRESS>{0}</IPADDRESS>", GlobalCollectConfig.IpAddress);
            sbXml.AppendFormat("<VERSION>{0}</VERSION>", GlobalCollectConfig.Version);
            sbXml.Append("</META>");
            sbXml.Append("<PARAMS>");  

            #region 订单及地址信息
            sbXml.Append("<ORDER>");
            sbXml.AppendFormat("<ORDERID>{0}</ORDERID>", parm.GcOrderId);
            sbXml.AppendFormat("<AMOUNT>{0}</AMOUNT>", amountInCents);
            sbXml.AppendFormat("<CURRENCYCODE>{0}</CURRENCYCODE>", parm.CurrencyCode);
            sbXml.AppendFormat("<LANGUAGECODE>{0}</LANGUAGECODE>", GlobalCollectConfig.LanguageCode);
            sbXml.AppendFormat("<COUNTRYCODE>{0}</COUNTRYCODE>", parm.BillingAddress.CountryCode);
            sbXml.AppendFormat("<STREET>{0}</STREET>", XmlEncode(GetSubstring(parm.BillingAddress.Street, 50)));
            sbXml.AppendFormat("<CITY>{0}</CITY>", XmlEncode(GetSubstring(parm.BillingAddress.City, 40)));
            sbXml.AppendFormat("<ZIP>{0}</ZIP>", XmlEncode(GetSubstring(parm.BillingAddress.Zip, 10)));
            sbXml.AppendFormat("<SURNAME>{0}</SURNAME>", XmlEncode(GetSubstring(parm.BillingAddress.LastName, 35)));
            sbXml.AppendFormat("<FIRSTNAME>{0}</FIRSTNAME>", XmlEncode(GetSubstring(parm.BillingAddress.FirstName, 15)));
            sbXml.AppendFormat("<SHIPPINGFIRSTNAME>{0}</SHIPPINGFIRSTNAME>", XmlEncode(GetSubstring(parm.ShippingAddress.FirstName, 15)));
            sbXml.AppendFormat("<SHIPPINGSURNAME>{0}</SHIPPINGSURNAME>", XmlEncode(GetSubstring(parm.ShippingAddress.LastName, 35)));
            sbXml.AppendFormat("<SHIPPINGSTREET>{0}</SHIPPINGSTREET>", XmlEncode(GetSubstring(parm.ShippingAddress.Street, 50)));
            sbXml.AppendFormat("<SHIPPINGCITY>{0}</SHIPPINGCITY>", XmlEncode(GetSubstring(parm.ShippingAddress.City, 40)));
            sbXml.AppendFormat("<SHIPPINGSTATE>{0}</SHIPPINGSTATE>", XmlEncode(GetSubstring(parm.ShippingAddress.State, 35)));
            sbXml.AppendFormat("<SHIPPINGCOUNTRYCODE>{0}</SHIPPINGCOUNTRYCODE>", parm.ShippingAddress.CountryCode);
            sbXml.AppendFormat("<SHIPPINGZIP>{0}</SHIPPINGZIP>", XmlEncode(GetSubstring(parm.ShippingAddress.Zip, 10)));
            sbXml.AppendFormat("<PHONENUMBER>{0}</PHONENUMBER>", XmlEncode(GetSubstring(parm.ShippingAddress.Phone, 20)));
            sbXml.AppendFormat("<EMAIL>{0}</EMAIL>", XmlEncode(GetSubstring(parm.Email, 70)));
            sbXml.AppendFormat("<MERCHANTREFERENCE>{0}</MERCHANTREFERENCE>", parm.MerchantReference);
            sbXml.Append("</ORDER>"); 
            #endregion

            #region 支付信息
            sbXml.Append("<PAYMENT>");
            sbXml.AppendFormat("<CVVINDICATOR>{0}</CVVINDICATOR>", GlobalCollectConfig.Cvvindicator);
            sbXml.AppendFormat("<RETURNURL>{0}</RETURNURL>", parm.ReturnUrl);
            sbXml.AppendFormat("<PAYMENTPRODUCTID>{0}</PAYMENTPRODUCTID>", (int)parm.GlobalCollectType);
            sbXml.AppendFormat("<AMOUNT>{0}</AMOUNT>", amountInCents);
            sbXml.AppendFormat("<CURRENCYCODE>{0}</CURRENCYCODE>", parm.CurrencyCode);
            sbXml.AppendFormat("<LANGUAGECODE>{0}</LANGUAGECODE>", GlobalCollectConfig.LanguageCode);
            sbXml.AppendFormat("<HOSTEDINDICATOR>{0}</HOSTEDINDICATOR>", GlobalCollectConfig.HostedIndicator);

            sbXml.AppendFormat("<SURNAME>{0}</SURNAME>", XmlEncode(GetSubstring(parm.BillingAddress.LastName, 35)));
            sbXml.AppendFormat("<COMPANYNAME>{0}</COMPANYNAME>", XmlEncode(GetSubstring(parm.BillingAddress.CompanyName, 40)));
            sbXml.AppendFormat("<FISCALNUMBER>{0}</FISCALNUMBER>", XmlEncode(GetSubstring(parm.BillingAddress.VatCode, 15)));
            //sbXml.Append("<ATTEMPTID>").Append(attemptid).Append("</ATTEMPTID>");
            sbXml.Append("</PAYMENT>"); 
            #endregion

            sbXml.Append("</PARAMS>");
            sbXml.Append("</REQUEST></XML>");

            #endregion

            #region 发送支付请求到GC服务器 
            var xml = sbXml.ToString();

            GcLogger.InfoFormat("InsertOrderWithPayment - 步骤3 - GC支付-请求服务器...");
            var responseXml = RequestGc(xml); 
            #endregion

            #region 解析GC服务器数据并返回调用者
            GcLogger.InfoFormat("InsertOrderWithPayment - 步骤4 - GC支付-解析服务器返回xml数据...");

            var xmlDoc = GetXmlDocument(responseXml);

            var state = GetResult(xmlDoc); 
            var errorCode = GetErrorCode(xmlDoc);

            if (string.Equals(RESULT_STATE_OK,state.ToUpper(),StringComparison.InvariantCultureIgnoreCase))
            {
                parm.PayUrl = xmlDoc.SelectSingleNode(PayConstants.FORMACTION).InnerText; //得到支付地址
                parm.Ref = xmlDoc.SelectSingleNode(PayConstants.REF).InnerText;
                parm.Mac = xmlDoc.SelectSingleNode(PayConstants.MAC).InnerText;  //得到Mac地址

                GcLogger.InfoFormat("InsertOrderWithPayment - 步骤5 - GC支付-成功，主要信息:PayUrl-{0}\n\r,Ref-{1}\n\r,Mac-{2}", parm.PayUrl, parm.Ref, parm.Mac);
            }
            else if (errorCode == ERROR_CODE_OVERFLOWER_AMOUNT)
            {
                GcLogger.InfoFormat("InsertOrderWithPayment - 步骤5 - GC支付-超限,错误代码:{0}", errorCode);
                throw new MaxAmountOutOfRangeException(parm.CurrencyCode, GlobalCollectConfig.MaxAmounts.TryGet(parm.CurrencyCode), parm.Amount);
            }
            else
            {
                var errorMsg = GetErrorMessage(xmlDoc);

                GcLogger.InfoFormat("InsertOrderWithPayment - 步骤5 - GC支付-出错,错误代码:{0},错误消息:{1}", errorCode, errorMsg);
                throw new GlobalCollectException(errorMsg, errorCode, xml, responseXml);
            } 
            #endregion

            return parm;
        }
        #endregion

        #region 步骤二：获取支付信息,主要得到交易Id(GlobalCollectInfo.effortId)
        private static GlobalCollectInfo GetGcOrderStatus(string gcOrderId)
        { 
            GcLogger.InfoFormat("GetGcOrderStatus - 步骤1 - 请求获取GC支付状态-开始验证。。。");

            #region 数据验证
            if (string.IsNullOrEmpty(gcOrderId))
            {
                throw new ArgumentNullException("gcOrderId");
            }

            GcLogger.InfoFormat("GetGcOrderStatus - 步骤2 - 完成验证。。。");
            #endregion

            var sbXml = new StringBuilder();
            #region 组装数据
            sbXml.Append("<XML><REQUEST>");
            sbXml.Append("<ACTION>GET_ORDERSTATUS</ACTION>");
            sbXml.Append("<META>");
            sbXml.AppendFormat("<IPADDRESS>{0}</IPADDRESS>", GlobalCollectConfig.IpAddress);
            sbXml.AppendFormat("<MERCHANTID>{0}</MERCHANTID>", GlobalCollectConfig.MerchantId);
            sbXml.AppendFormat("<VERSION>{0}</VERSION>", GlobalCollectConfig.Version);
            sbXml.Append("</META>");
            sbXml.Append("<PARAMS><ORDER>");
            sbXml.AppendFormat("<ORDERID>{0}</ORDERID>", gcOrderId);
            sbXml.Append("</ORDER></PARAMS>");
            sbXml.Append("</REQUEST></XML>"); 
            #endregion

            #region 服务器请求数据  
            GcLogger.InfoFormat("GetGcOrderStatus - 步骤3 - 请求GC服务器。。。");
            var responseXml = RequestGc(sbXml.ToString()); 

            #endregion

            #region 解析服务返回的XML数据 
            GcLogger.InfoFormat("GetGcOrderStatus - 步骤4 - 解析GC服务器返回数据-{0}", responseXml);
            var xmlDoc = GetXmlDocument(responseXml);

            var state = GetResult(xmlDoc);
            var info = new GlobalCollectInfo
            {
                StatusId = "-1"
            }; 

            if (state.Equals(RESULT_STATE_OK, StringComparison.OrdinalIgnoreCase))
            {
                info.GcOrderId = gcOrderId;
                info.StatusId = GetStatusId(xmlDoc);
                info.EffortId = xmlDoc.SelectSingleNode(PayConstants.EFFORTID).InnerText;
                info.GlobalCollectType = EnumHelper.ToEnum<GlobalCollectType>(xmlDoc.SelectSingleNode(PayConstants.PAYMENTPRODUCTID).InnerText.ParseTo(1));
                info.MerchantReference = xmlDoc.SelectSingleNode(PayConstants.MERCHANTREFERENCE).InnerText;
                info.Currency = xmlDoc.SelectSingleNode(PayConstants.CURRENCYCODE).InnerText;
                var amount = decimal.Parse(xmlDoc.SelectSingleNode(PayConstants.AMOUNT).InnerText);//这里得到的还是GC格式的金额，需要处理小数点问题
                info.Amount = GetAmountFromCents(amount);
                var creditCardNoNode = xmlDoc.SelectSingleNode(PayConstants.CREDITCARDNUMBER);
                info.CreditCardNo = creditCardNoNode == null ? string.Empty : creditCardNoNode.InnerText;
                var codeNode = xmlDoc.SelectSingleNode(PayConstants.CODE);
                info.Code = codeNode == null ? string.Empty : codeNode.InnerText;
                var messageNode = xmlDoc.SelectSingleNode(PayConstants.MESSAGE);
                info.Message = messageNode == null ? string.Empty : messageNode.InnerText; 
            } 
            #endregion

            return info;
        }
        #endregion

        #region 步骤三：通知GC扣款
        public static bool ConfirmPayment(ref GlobalCollectInfo globalCollectInfo)
        {
            GcLogger.InfoFormat("ConfirmPayment - 步骤1 - 确认扣款-开始验证。。。");

            #region 数据验证
            if (globalCollectInfo == null)
            {
                return false;
            }

            if (globalCollectInfo.GcOrderId.IsNullOrEmpty())
            {
                return false;
            }


            GcLogger.InfoFormat("ConfirmPayment - 步骤2 - 确认扣款完成验证。。。");
            #endregion

            #region 从服务器获取最新的订单状态  
            globalCollectInfo = GetGcOrderStatus(globalCollectInfo.GcOrderId);

            GcLogger.InfoFormat("ConfirmPayment - 步骤3 - 请求最新的GC订单状态信息,返回的值是:{0}", globalCollectInfo.StatusId);
            #endregion

            #region 判断服务器返回的订单状态 
            if (globalCollectInfo.StatusId == GlobalCollectInfo.GC_PAY_STATUS_READY)
            {
                //支付成功
                return true;
            }
            else if (globalCollectInfo.StatusId == GlobalCollectInfo.GC_PAY_STATUS_CHALLENGED)
            {
                //可能存在欺诈的支付完成
                return true;
            }
            else if (globalCollectInfo.StatusId == GlobalCollectInfo.GC_PAY_STATUS_PENDING)
            { 
                GcLogger.InfoFormat("ConfirmPayment - 步骤3 - 支付准备的情况需要重新执行扣款操作。。。");
                //如果是等待支付的情况，还得执行扣款操作
                if (SetPayment(globalCollectInfo))
                {
                    //扣款成功
                    return true;
                }
            } 
            #endregion

            return false;
        }

        public static bool SetPayment(GlobalCollectInfo globalCollectInfo)
        {

            GcLogger.InfoFormat("SetPayment - 步骤1 - 请求获取GC支付状态-开始验证。。。");

            #region 数据验证 
            if (globalCollectInfo == null)
            {
                throw new ArgumentNullException("globalCollectInfo");
            }
            if (string.IsNullOrEmpty(globalCollectInfo.OrderNo))
            {
                throw new ArgumentNullException("globalCollectInfo.OrderId");
            }
            if (string.IsNullOrEmpty(globalCollectInfo.EffortId))
            {
                throw new ArgumentNullException("globalCollectInfo.EffortId");
            }
            
            GcLogger.InfoFormat("SetPayment - 步骤2 -完成数据验证。。。");
            #endregion

            #region 组装数据
            var sbXml = new StringBuilder();
            sbXml.Append("<XML><REQUEST>");
            sbXml.Append("<ACTION>SET_PAYMENT</ACTION>");
            sbXml.Append("<META>");
            sbXml.AppendFormat("<IPADDRESS>{0}</IPADDRESS>", GlobalCollectConfig.IpAddress);
            sbXml.AppendFormat("<MERCHANTID>{0}</MERCHANTID>", GlobalCollectConfig.MerchantId);
            sbXml.AppendFormat("<VERSION>{0}</VERSION>", GlobalCollectConfig.Version);
            sbXml.Append("</META>");
            sbXml.Append("<PARAMS><PAYMENT>");
            sbXml.AppendFormat("<ORDERID>{0}</ORDERID>", globalCollectInfo.GcOrderId);
            sbXml.AppendFormat("<EFFORTID>{0}</EFFORTID>", globalCollectInfo.EffortId);
            sbXml.AppendFormat("<PAYMENTPRODUCTID>{0}</PAYMENTPRODUCTID>", (int)globalCollectInfo.GlobalCollectType);
            sbXml.Append("</PAYMENT></PARAMS>");
            sbXml.Append("</REQUEST></XML>"); 
            #endregion

            #region 请求GC服务器 
            var xml = sbXml.ToString();

            GcLogger.InfoFormat("SetPayment - 步骤3 - 请求GC服务器,参数:{0}", xml);

            var responseXml = RequestGc(xml); 
            #endregion

            #region 解析GC服务器并返回数据
            GcLogger.InfoFormat("SetPayment - 步骤4 - 解析GC服务器返回的数据,参数:{0}", responseXml);

            var xmlDoc = GetXmlDocument(responseXml);

            var result = GetResult(xmlDoc);
            if (result.ToUpper() == RESULT_STATE_OK)
            {
                var orderStatusInfo = GetGcOrderStatus(globalCollectInfo.GcOrderId);
                globalCollectInfo.StatusId = orderStatusInfo.StatusId;
                if (orderStatusInfo.StatusId == GlobalCollectInfo.GC_PAY_STATUS_READY || orderStatusInfo.StatusId == GlobalCollectInfo.GC_PAY_STATUS_CHALLENGED)
                {
                    globalCollectInfo.StatusId = orderStatusInfo.StatusId;
                    globalCollectInfo.EffortId = orderStatusInfo.EffortId;
                    globalCollectInfo.GlobalCollectType = orderStatusInfo.GlobalCollectType;
                    globalCollectInfo.MerchantReference = orderStatusInfo.MerchantReference;
                    globalCollectInfo.Currency = orderStatusInfo.Currency;
                    globalCollectInfo.Amount = orderStatusInfo.Amount;
                    globalCollectInfo.CreditCardNo = orderStatusInfo.CreditCardNo;
                    globalCollectInfo.Code = orderStatusInfo.Code;

                    return true;
                } 

                GcLogger.InfoFormat("SetPayment - 步骤5 - 扣款成功,主要信息:StatusId-{0},EffortId-{1},GlobalCollectType-{2},Currency-{3},Amount-{4},GcOrderId{5}", globalCollectInfo.StatusId, globalCollectInfo.EffortId, globalCollectInfo.GlobalCollectType, globalCollectInfo.Currency, globalCollectInfo.Amount, globalCollectInfo.GcOrderId);
            }
            else
            {
                var errorCode = GetErrorCode(xmlDoc);
                var errorMsg = GetErrorMessage(xmlDoc);

                GcLogger.ErrorFormat("SetPayment - 步骤5 - 扣款失败,错误代码:{0},错误原因:{1}", errorCode, errorMsg);

                throw new GlobalCollectException(errorMsg, errorCode, xml, responseXml);
            }
            return false; 
            #endregion

        }

        #endregion
        #endregion

        #region 辅助方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static int GetAmountToCents(decimal amount)
        {
            if (amount == 0)
            {
                return 0;
            }

            return (int)(Math.Round(amount, 2) * 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static decimal GetAmountFromCents(decimal amount)
        {
            return amount / 100;//小数点左移2位
        }


        /// <summary>
        /// 检查GC支付是否超额
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="payAmount"></param>
        private static void CheckGcMaxAmount(string currencyCode, decimal payAmount)
        {
            if (!GlobalCollectConfig.MaxAmounts.IsNullOrEmpty() && GlobalCollectConfig.MaxAmounts.ContainsKey(currencyCode))
            {
                var maxAmount = GlobalCollectConfig.MaxAmounts[currencyCode];
                if (payAmount > maxAmount)
                {
                    throw new PaymentException(string.Format("CheckGcMaxAmount - GC支付超额:币种:{0}最多允许支付:{1},当前支付金额:{2}", currencyCode, maxAmount, payAmount));
                }
            }
        }

        private static string GetSubstring(string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (s.Length > length) return s.Substring(0, length);
            return s;
        }

        private static string XmlEncode(string text)
        {
            return text.Replace(">", "&gt;").Replace("<", "&lt;");
        }

        private static string RequestGc(string postData)
        {
            GcLogger.InfoFormat("RequestGc - GC支付-请求支付信息，参数信息:{0}", postData);
            
            var html = string.Empty;
            HttpWebResponse response = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(GlobalCollectConfig.ServiceUrl);
                request.Method = "POST";
                request.Accept = "*/*";
                request.ContentType = "Content-Type:text/xml;charset=utf-8";
                var buffer = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);

                response = (HttpWebResponse)request.GetResponse();
                if ((int)response.StatusCode == 200)
                {
                    var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    html = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                }

                GcLogger.InfoFormat("RequestGc - GC支付-请求支付信息成功，返回的数据:{0}", html);
            }
            catch (System.Exception e)
            { 
                GcLogger.InfoFormat("RequestGc - 请求GC服务失败.地址：{0},数据:{1},错误信息:{2} ", GlobalCollectConfig.ServiceUrl, postData, e.Message);
                throw new PaymentException(string.Format("RequestGc - 请求GC服务失败.地址：{0},数据:{1},错误信息:{2} ", GlobalCollectConfig.ServiceUrl, postData, e.Message), e);
            }
            finally
            {
                if (response != null) response.Close();
            }

            return html;
        }

        private static XmlDocument GetXmlDocument(string xml)
        { 
            GcLogger.InfoFormat("GetXmlDocument - GC支付-解析服务器返回的XML，参数信息:{0}", xml);

            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xml);
            }
            catch (XmlException ex)
            {
                GcLogger.InfoFormat("GetXmlDocument - GC支付-解析服务器返回的XML出错，错误信息:{0}", ex.Message);
                throw new GlobalCollectException("加载返回XML出错。" + ex.Message, string.Empty, string.Empty, xml, ex);
            }

            return xmlDoc;
        }

        #region 解析特殊节点取值 

        private static string GetResult(XmlDocument xmlDoc)
        {
            return xmlDoc.SelectSingleNode(PayConstants.RESULT).InnerText;
        }

        private static string GetStatusId(XmlDocument xmlDoc)
        {
            return xmlDoc.SelectSingleNode(PayConstants.STATUSID).InnerText;
        }

        private static string GetErrorCode(XmlDocument xmlDoc)
        {
            return GetNodeText(xmlDoc, PayConstants.CODE);
        }

        private static string GetErrorMessage(XmlDocument xmlDoc)
        {
            return GetNodeText(xmlDoc, PayConstants.MESSAGE);
        }

        private static string GetNodeText(XmlDocument xmlDoc, string path)
        {
            XmlNode node = xmlDoc.SelectSingleNode(path);
            if (node == null) return string.Empty;
            return node.InnerText;
        }
        #endregion

        /// <summary>
        /// 支付常量
        /// </summary>
        private class PayConstants
        {
            public static readonly string CODE = "XML/REQUEST/RESPONSE/STATUS/ERRORS/ERROR/CODE";
            public static readonly string MESSAGE = "XML/REQUEST/RESPONSE/STATUS/ERRORS/ERROR/MESSAGE";
            public static readonly string RESULT = "XML/REQUEST/RESPONSE/RESULT";
            public static readonly string STATUSID = "XML/REQUEST/RESPONSE/STATUS/STATUSID";
            public static readonly string EFFORTID = "XML/REQUEST/RESPONSE/STATUS/EFFORTID";
            public static readonly string FORMACTION = "XML/REQUEST/RESPONSE/ROW/FORMACTION";
            public static readonly string REF = "XML/REQUEST/RESPONSE/ROW/REF";
            public static readonly string MAC = "XML/REQUEST/RESPONSE/ROW/RETURNMAC";
            public static readonly string CONVERTEDAMOUNT = "XML/REQUEST/RESPONSE/ROW/CONVERTEDAMOUNT";
            public static readonly string PAYMENTPRODUCTID = "XML/REQUEST/RESPONSE/STATUS/PAYMENTPRODUCTID";
            public static readonly string MERCHANTREFERENCE = "XML/REQUEST/RESPONSE/STATUS/MERCHANTREFERENCE";
            public static readonly string CURRENCYCODE = "XML/REQUEST/RESPONSE/STATUS/CURRENCYCODE";
            public static readonly string AMOUNT = "XML/REQUEST/RESPONSE/STATUS/AMOUNT";
            public static readonly string CREDITCARDNUMBER = "XML/REQUEST/RESPONSE/STATUS/CREDITCARDNUMBER";
        }
        #endregion
    }
}