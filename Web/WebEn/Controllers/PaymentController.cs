using System.Web.Caching;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Payment.PayConfig;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Common.Mvc.Model;
using Com.Panduo.Web.Models.Payment;
using Com.Panduo.Web.PaymentCommon.Service;
using Com.Panduo.Web.PaymentCommon.Service.Exception;
using Com.Panduo.Web.PaymentCommon.Service.Parm.GlobalCollect;
using Com.Panduo.Web.PaymentCommon.Service.Parm.OceanPayment;
using Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PaymentLoggerHelper = Com.Panduo.Web.PaymentCommon.Common.PaymentLoggerHelper;
using PaymentLoggerType = Com.Panduo.Web.PaymentCommon.Common.PaymentLoggerType;

namespace Com.Panduo.Web.Controllers
{
    /// <summary>
    /// 支付控制器
    /// </summary>
    public class PaymentController : BaseController
    {
        /// <summary>
        /// 支付日志
        /// </summary>
        private static ILog _paymentLogger = PaymentLoggerHelper.GetLogger(PaymentLoggerType.Payment);

        #region 支付公共
        /// <summary>
        /// 更改支付方式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePaymentType()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var paymentTypeString = Request["paymentType"];
            if (paymentTypeString.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_PAYMENT_TYPE_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var paymentTypeValue = paymentTypeString.ParseTo(0);
            if (paymentTypeValue <= 0)
            {
                jsonData.Message = "ERROR_PAYMENT_TYPE_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var paymentType = EnumHelper.ToEnum<PaymentType>(paymentTypeValue);
            GlobalCollectType? gcType = null;

            if (paymentType == PaymentType.Gc)
            {
                var gcTypeString = Request["gcType"];
                if (gcTypeString.IsNullOrEmpty())
                {
                    jsonData.Message = "ERROR_GC_TYPE_CAN_NOT_EMPTY";
                    return Json(jsonData);
                }

                var gcTypeValue = gcTypeString.ParseTo(0);
                if (gcTypeValue <= 0)
                {
                    jsonData.Message = "ERROR_GC_TYPE_CAN_NOT_EMPTY";
                    return Json(jsonData);
                }

                gcType = EnumHelper.ToEnum<GlobalCollectType>(gcTypeValue);
            }

            #endregion

            #region 执行业务方法
            try
            {
                ServiceFactory.OrderService.CustomerChangePaymentType(orderNo, paymentType, gcType);

                jsonData.Succeed = true;
            }
            catch (BussinessException bussinessException)
            {
                jsonData.Message = bussinessException.GetError();
            }

            #endregion

            return Json(jsonData);
        }

        /// <summary>
        /// 下载支付文件信息
        /// </summary> 
        /// <returns></returns>
        public ActionResult DownloadPaymentFile()
        {
            #region 获取参数
            var orderNo = Request["orderNo"];
            var paymentTypeValue = Request["type"].ParseTo(0);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                return Content("");
            }

            if (paymentTypeValue <= 0)
            {
                return Content("");
            }

            var paymentType = EnumHelper.ToEnum<PaymentType>(paymentTypeValue, PaymentType.Paypal);
            if (paymentType != PaymentType.Hsbc && paymentType != PaymentType.BankOfChina && paymentType != PaymentType.WesternUnion && paymentType != PaymentType.MoneyGram)
            {
                return Content("");
            }
            #endregion

            #region 根据支付方式和订单号获取业务数据
            var fileName = GetPaymentFileName(paymentType);
            var content = GetPaymentInfo(orderNo, paymentType);
            #endregion

            #region 输出支付信息
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            { 
                writer.Write(content);
                writer.Flush(); 
            }
            #endregion

            return File(stream.ToArray(), "text/plain", fileName);
        }

        /// <summary>
        /// 阅读支付文件信息
        /// </summary>  
        /// <returns></returns>
        public ActionResult ReadPaymentFile()
        {
            #region 获取参数
            var orderNo = Request["orderNo"];
            var paymentTypeValue = Request["type"].ParseTo(0);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                return Content("");
            }

            if (paymentTypeValue <= 0)
            {
                return Content("");
            }

            var paymentType = EnumHelper.ToEnum<PaymentType>(paymentTypeValue, PaymentType.Paypal);
            if (paymentType != PaymentType.Hsbc && paymentType != PaymentType.BankOfChina && paymentType != PaymentType.WesternUnion && paymentType != PaymentType.MoneyGram)
            {
                return Content("");
            }
            #endregion

            #region 根据支付方式和订单号获取业务数据
            var content = GetPaymentInfo(orderNo, paymentType);
            #endregion

            return Content(content, "text/plain", Encoding.UTF8);
        }

        /// <summary>
        /// 获取支付文件内容
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        private string GetPaymentInfo(string orderNo, PaymentType paymentType)
        {
            var content = string.Empty;

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);
            //订单不存在或者订单不属于当前登陆客户的不允许下载
            if (order != null && order.CustomerId == SessionHelper.CurrentCustomer.CustomerId)
            {
                var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
                if (orderAmount != null)
                {
                    var currency = CacheHelper.GetCurrencyByCode(order.Currency);

                    //将待支付美元金额转换为对应币种金额显示，比如USD 123.36，这里不需要考虑Cash欠款
                    var payAmountInfo = orderAmount.NeedToPayAmt.ToExchangeCurrencyMoneyString(currency, order.ExchangeRate);//比如"USD 123.36"
                    var hasCash = ServiceFactory.CashService.GetCustomerBalance(order.CustomerId) > 0;

                    switch (paymentType)
                    {
                        case PaymentType.Paypal:
                            break;
                        case PaymentType.Hsbc:
                            content = this.RenderPartialViewToHtml("OfflinePaymentInfo/Hsbc", new OfflinePaymentInfo<HsbcConfig> { OrderNo = orderNo, Amount = payAmountInfo, HasCash = hasCash, PaymentInfo = ServiceFactory.PaymentService.GetHsbcConfig() });
                            break;
                        case PaymentType.BankOfChina:
                            content = this.RenderPartialViewToHtml("OfflinePaymentInfo/BankOfChina", new OfflinePaymentInfo<BankOfChinaConfig> { OrderNo = orderNo, Amount = payAmountInfo, HasCash = hasCash, PaymentInfo = ServiceFactory.PaymentService.GetBankOfChinaConfig() });
                            break;
                        case PaymentType.WesternUnion:
                            content = this.RenderPartialViewToHtml("OfflinePaymentInfo/WesternUnion", new OfflinePaymentInfo<WesternUnionConfig> { OrderNo = orderNo, Amount = payAmountInfo, HasCash = hasCash, PaymentInfo = ServiceFactory.PaymentService.GetWesternUnionConfig() });
                            break;
                        case PaymentType.Gc:
                            break;
                        case PaymentType.MoneyGram:
                            content = this.RenderPartialViewToHtml("OfflinePaymentInfo/MoneyGram", new OfflinePaymentInfo<MoneyGramConfig> { OrderNo = orderNo, Amount = payAmountInfo, HasCash = hasCash, PaymentInfo = ServiceFactory.PaymentService.GetMoneyGramConfig() });
                            break;
                        case PaymentType.Webmoney:
                            break;
                        case PaymentType.Yandex:
                            break;
                        case PaymentType.QiWi:
                            break;
                        case PaymentType.OceanCreditCard:
                            break;
                        default:
                            break;
                    }
                }
            }

            return content;
        }

        /// <summary>
        /// 获取支付文件名
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        private string GetPaymentFileName(PaymentType paymentType)
        {
            var paymentFileName = string.Empty;

            switch (paymentType)
            {
                case PaymentType.Paypal:
                    break;
                case PaymentType.Hsbc:
                    paymentFileName = "Bank_Wire_Transfer_(HSBC).txt";
                    break;
                case PaymentType.BankOfChina:
                    paymentFileName = "Bank_Wire_Transfer_(Bank_of_China).txt";
                    break;
                case PaymentType.WesternUnion:
                    paymentFileName = "Westerm_Union_Money_Transfer.txt";
                    break;
                case PaymentType.Gc:
                    break;
                case PaymentType.MoneyGram:
                    paymentFileName = "Money_Gram.txt";
                    break;
                case PaymentType.Webmoney:
                    break;
                case PaymentType.Yandex:
                    break;
                case PaymentType.QiWi:
                    break;
                case PaymentType.OceanCreditCard:
                    break;
            }

            return paymentFileName;
        }

        /// <summary>
        /// iframe顶层跳转
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static ContentResult RedirectByScript(string url)
        {
            var result = new ContentResult();
            result.Content = string.Format("<html><head><script type=\"text/javascript\">top.location.href=\"{0}\";</script></head><body></body></html>", url);
            result.ContentType = "text/html";
            return result;
        }

        private static ContentResult RedirectErrorPage(string orderId,string statusId,string errorMessage)
        {
            SessionHelper.GlobalCollectErrorMessage = errorMessage;

            return RedirectByScript(string.Format(UrlRewriteHelper.GetOrderDetailPayment(orderId) + "&payMethod={0}&statusId={1}", (int)PaymentType.Gc, statusId));
        }

        private static ContentResult RedirectSignInByScript()
        {
            return RedirectByScript(UrlRewriteHelper.GetLoginUrl());
        }
        #endregion

        #region Cash支付
        /// <summary>
        /// 使用Cash支付订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult PayOrderByCash()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var isFullPay = Request["isFullPay"].ParseTo(false);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }
            #endregion

            #region 组装业务参数

            #endregion

            #region 执行业务方法
            try
            {
                ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, isFullPay);

                jsonData.Succeed = true;
            }
            catch (BussinessException bussinessException)
            {
                jsonData.Message = bussinessException.GetError();
            }
            #endregion

            return Json(jsonData);
        }
        #endregion

        #region Paypal标准支付
        #region 请求支付
        /// <summary>
        /// 请求用Paypal支付订单验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult RequestPayOrderByPaypalVertify()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            #endregion

            #region 验证基本参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);
            if (order == null || order.OrderStatus != OrderStatusType.Pending)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var orderBillingAddress = ServiceFactory.OrderService.GetOrderBillingAddressByOrderId‎(order.OrderId);
            if (orderBillingAddress == null)
            {
                jsonData.Message = "ERROR_ORDER_BILLING_ADDRESS_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }
            #endregion

            var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            if (orderAmount == null)
            {
                jsonData.Message = "ERROR_ORDER_AMOUNT_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (orderAmount.NeedToPayAmt <= 0)
            {
                jsonData.Message = "ERROR_ORDER_AMOUNT_NOT_NEED_TO_PAY";
                return Json(jsonData);
            }

            jsonData.Succeed = true;

            return Json(jsonData);
        }

        /// <summary>
        /// 请求用Paypal支付订单
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        public ActionResult RequestPayOrderByPaypal()
        {
            #region 获取参数
            var orderNo = Request["orderNo"];
            #endregion

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);

            var orderBillingAddress = ServiceFactory.OrderService.GetOrderBillingAddressByOrderId‎(order.OrderId);

            var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);

            var model = new PaypalSubmitParm
            {
                IsPayForClubFee = false,
                PaypalConfig = ServiceFactory.PaymentService.GetPaypalConfig(),
                Customer = SessionHelper.CurrentCustomer,
                BillingAddress = orderBillingAddress,
                Order = order
            };

            var debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(model.Customer.CustomerId);

            var payAmountUsd = orderAmount.NeedToPayAmt + debtCashUsd;

            //汇率转换为订单币种对应的金额进行支付
            var payCurrencyCode = ServiceFactory.PaymentService.IsCurrencyUseUsdForPaypal(order.Currency) ? PageHelper.CURRENCY_CODE_USD : order.Currency;

            var currency = CacheHelper.GetCurrencyByCode(payCurrencyCode);

            var paymentAmount = payAmountUsd * order.ExchangeRate;//汇率转换为订单币种对应的金额

            paymentAmount = PageHelper.GetRoundValue(paymentAmount, currency.DecimalPlaces);

            model.CurrencyCode = payCurrencyCode;
            model.PaymentAmount = paymentAmount;


            return View("SubmitToPaypal", model);
        }

        /// <summary>
        /// 请求用Paypal支付Club年费验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult RequestPayClubByPaypalVertify()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var customerId = SessionHelper.CurrentCustomer.IsNullOrEmpty() ? 0 : SessionHelper.CurrentCustomer.CustomerId;

            #endregion

            #region 验证基本参数
            //客户不存在
            var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);
            if (customer == null)
            {
                jsonData.Message = "ERROR_CUSTOMER_NOT_EXIST";
                return Json(jsonData);
            }

            //客户已经是club会员
            if (customer.ClubLevel > 0)
            {
                jsonData.Message = "ERROR_CUSTOMER_IS_CLUB_ALREADY";
                return Json(jsonData);
            }

            //客户支付的club年费不存在或者小于等于0
            var clubShippingFee = ServiceFactory.ClubService.GetClubShippingFee(customerId);
            if (clubShippingFee == null || clubShippingFee.ShippingFeeAfter <= 0)
            {
                jsonData.Message = "ERROR_CUSTOMER_CLUB_FEE_WRONG";
                return Json(jsonData);
            }

            #endregion

            #region 执行业务方法

            #endregion

            jsonData.Succeed = true;

            return Json(jsonData);
        }

        /// <summary>
        /// 请求用Paypal支付Club年费
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        public ActionResult RequestPayClubByPaypal()
        {
            var model = new PaypalSubmitParm
            {
                IsPayForClubFee = true,
                PaypalConfig = ServiceFactory.PaymentService.GetPaypalConfig(),
                Customer = SessionHelper.CurrentCustomer,
            };

            var clubShippingFee = ServiceFactory.ClubService.GetClubShippingFee(model.Customer.CustomerId);


            //就用美元支付
            model.CurrencyCode = ServiceFactory.ConfigureService.CURRENCY_CODE_USD;
            var currency = CacheHelper.GetCurrencyByCode(model.CurrencyCode);

            var payAmount = PageHelper.GetRoundValue(clubShippingFee.ShippingFeeAfter, currency.DecimalPlaces);

            model.PaymentAmount = payAmount;

            return View("SubmitToPaypal", model);
        }
        #endregion

        #region 支付服务器返回
        /// <summary>
        /// Paypal响应支付订单(Notify)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public void PaypalNotifyForPayOrder()
        {
            var qs = Request.Form.ToQueryString(Request.ContentEncoding);

            _paymentLogger.InfoFormat("PaypalNotifyForPayOrder - Paypal返回进入,编码方式:{0},参数:{1}", Request.ContentEncoding, qs);

            #region 支付返回验证
            PaypalNotifyResult paypalNotifyResult = null;
            try
            {
                paypalNotifyResult = PaypalService.PaypalNotifyVerify(Request.Form, null, Request.ContentEncoding);

                if (paypalNotifyResult == null || !paypalNotifyResult.IsValid)
                {
                    _paymentLogger.ErrorFormat("PaypalNotifyForPayOrder - PayPal支付失败。支付验证返回Null。\r\n");
                }
            }
            catch (Exception ex)
            {
                _paymentLogger.Error(string.Format("PaypalNotifyForPayOrder - PayPal支付处理出错。\r\nRequestForm:{0}", qs), ex);
            }
            #endregion

            #region 业务方法执行
            if (paypalNotifyResult != null && paypalNotifyResult.IsValid)
            {
                var paypalInfo = ConvertPaymentInfo(paypalNotifyResult.PaypalInfo);

                paypalInfo.PaypalTargetType = PaypalTargetType.Order;
                var order = ServiceFactory.OrderService.GetOrderByOrderNo(paypalInfo.ItemNumber);

                try
                {
                    //由于标准paypal支付的时候网站是不允许客户修改账单地址的，所以paypal返回的信息里面是不会有地址的，但是为了兼容需要把账单赋值给paypal日志
                    var orderBillingAddress = ServiceFactory.OrderService.GetOrderBillingAddressByOrderId‎(order.OrderId);
                    if (orderBillingAddress != null)
                    {
                        paypalInfo.AddressCity = paypalInfo.AddressCity.IsNullOrEmpty() ? orderBillingAddress.City : paypalInfo.AddressCity;
                        paypalInfo.AddressCountryCode = paypalInfo.AddressCountryCode.IsNullOrEmpty() ? CacheHelper.GetCountryCode(orderBillingAddress.Country) : paypalInfo.AddressCountryCode;
                        paypalInfo.AddressCountryName = paypalInfo.AddressCity.IsNullOrEmpty() ? orderBillingAddress.City : paypalInfo.AddressCity;
                        paypalInfo.AddressState = paypalInfo.AddressState.IsNullOrEmpty() ? orderBillingAddress.Province : paypalInfo.AddressState;
                        paypalInfo.AddressStreet1 = paypalInfo.AddressStreet1.IsNullOrEmpty() ? orderBillingAddress.Street1 : paypalInfo.AddressStreet1;
                        paypalInfo.AddressStreet2 = paypalInfo.AddressStreet2.IsNullOrEmpty() ? orderBillingAddress.Street2 : paypalInfo.AddressStreet2;
                        paypalInfo.AddressZip = paypalInfo.AddressZip.IsNullOrEmpty() ? orderBillingAddress.ZipCode : paypalInfo.AddressZip;
                        paypalInfo.FirstName = paypalInfo.FirstName.IsNullOrEmpty() ? orderBillingAddress.FirstName : paypalInfo.FirstName;
                        paypalInfo.LastName = paypalInfo.LastName.IsNullOrEmpty() ? orderBillingAddress.LastName : paypalInfo.LastName;
                    }

                    ServiceFactory.OrderService.CustomerPayOrderByPaypal(paypalInfo.ItemNumber, paypalInfo);

                    _paymentLogger.InfoFormat("PaypalNotifyForPayOrder - PayPal支付订单处理成功。\n\r订单号:{0},Paypal交易号:{1}", paypalInfo.ItemNumber, paypalInfo.TxnId);
                }
                catch (BussinessException ex)
                {
                    _paymentLogger.Error(string.Format("PaypalNotifyForPayOrder - PayPal支付处理出错。\r\nRequestForm:{0}", qs), ex);
                }
            }
            #endregion
        }

        /// <summary>
        /// Paypal响应支付Club年费(Notify)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public void PaypalNotifyForPayClub()
        {
            var qs = Request.Form.ToQueryString(Request.ContentEncoding);

            _paymentLogger.InfoFormat("PaypalNotifyForPayClub - Paypal返回进入,编码方式:{0},参数:{1}", Request.ContentEncoding, qs);

            #region 支付返回验证
            PaypalNotifyResult paypalNotifyResult = null;
            try
            {
                paypalNotifyResult = PaypalService.PaypalNotifyVerify(Request.Form, null, Request.ContentEncoding);

                if (paypalNotifyResult == null || !paypalNotifyResult.IsValid)
                {
                    _paymentLogger.ErrorFormat("PaypalNotifyForPayClub - PayPal支付失败。支付验证返回Null。\r\n");
                }
            }
            catch (Exception ex)
            {
                _paymentLogger.Error(string.Format("PaypalNotifyForPayClub - PayPal支付处理出错。\r\nRequestForm:{0}", qs), ex);
            }
            #endregion

            #region 业务方法执行
            if (paypalNotifyResult != null && paypalNotifyResult.IsValid)
            {
                var paypalInfo = ConvertPaymentInfo(paypalNotifyResult.PaypalInfo);

                var customerId = 0;

                if (SessionHelper.CurrentCustomer == null)
                {
                    //var customer = ServiceFactory.CustomerService.GetCustomerByEmail(paypalInfo.ItemNumber);
                    //customerId = customer == null ?customerId : customer.CustomerId;
                    customerId = paypalInfo.Custom.ParseTo(0);//从传递过去的Custom取值
                }
                else
                {
                    customerId = SessionHelper.CurrentCustomer.CustomerId;
                }

                paypalInfo.PaypalTargetType = PaypalTargetType.ClubFee;
                paypalInfo.TargetId = customerId;

                try
                {
                    ServiceFactory.ClubService.JoinClubByPaypal(customerId, paypalInfo);

                    _paymentLogger.InfoFormat("PaypalNotifyForPayClub - PayPal支付Club年费处理成功。\n\r客户Email:{0},Paypal交易号:{1}", paypalInfo.ItemNumber, paypalInfo.TxnId);
                }
                catch (BussinessException ex)
                {
                    _paymentLogger.Error(string.Format("PaypalNotifyForPayClub - PayPal支付处理出错。\r\nRequestForm:{0}", qs), ex);
                }
            }
            #endregion
        }

        /// <summary>
        /// 将支付组件的信息转换为业务对象
        /// </summary>
        /// <param name="paypalInfo"></param>
        /// <returns></returns>
        private static Com.Panduo.Service.Payment.PayInfo.PaypalInfo ConvertPaymentInfo(
            Com.Panduo.Web.PaymentCommon.PayInfo.PaypalInfo paypalInfo)
        {
            PaypalInfo paypal = null;
            if (paypalInfo != null)
            {
                paypal = new PaypalInfo();
                ObjectHelper.CopyProperties(paypalInfo, paypal, new[] { "PAYPAL_STATUS_COMPLETED", "PaypalTargetType", "IsCompleted" });
            }

            return paypal;
        }
        #endregion

        #region 支付完成（成功或者失败)响应
        /// <summary>
        /// Paypal支付订单成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayOrderByPaypalSuccess()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            #endregion

            #region 验证基本参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }
            #endregion

            if (!SessionHelper.LastOrderNumber.IsNullOrEmpty() && SessionHelper.LastOrderNumber.Equals(orderNo))
            {
                return RedirectToAction("Succeed", "Checkout");
            }
            else
            {
                return Redirect(UrlRewriteHelper.GetOrderDetail(orderNo));
            }
        }

        /// <summary>
        /// Paypal支付Club Fee成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayClubFeeByPaypalSuccess()
        {
            //客户信息通过session获取
            return Redirect(UrlRewriteHelper.GetMyClubUrl());
            //return View();
        }

        #endregion
        #endregion

        #region Paypal快速支付
        #region 步骤一:调用SetExpressCheckout取得paypal通信token
        /// <summary>
        /// 步骤一:调用SetExpressCheckout取得paypal通信token
        /// </summary> 
        /// <returns></returns> 
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult SetPaypalExpressCheckout()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var totalAmount = Request["totalAmount"].ParseTo(0.00M);
            var shippingAmount = Request["shippingAmount"].ParseTo(0.00M);
            var productCount = Request["productCount"].ParseTo(0);
            var currency = SessionHelper.CurrentCurrency;
            if (ServiceFactory.PaymentService.IsCurrencyUseUsdForPaypal(currency.CurrencyCode))
            {
                currency = CacheHelper.GetCurrencyByCode(PageHelper.CURRENCY_CODE_USD);
            }
            
            var customer = SessionHelper.CurrentCustomer;
            var paypalExpressConfig = ServiceFactory.PaymentService.GetPaypalExpressConfig();
            #endregion

            #region 参数验证
            if (totalAmount <= 0)
            {
                jsonData.Message = "ERROR_TOTAL_AMOUNT_CAN_NOT_BELOW_ZERO";
                return Json(jsonData);
            }

            if (productCount <= 0)
            {
                jsonData.Message = "ERROR_PRODUCT_COUNT_CAN_NOT_BELOW_ZERO";
                return Json(jsonData);
            }

            if (currency == null)
            {
                jsonData.Message = "ERROR_CURRENCY_CAN_NOT_NULL";
                return Json(jsonData);
            }
            #endregion

            #region 组装请求参数
            var scheme = UrlFuncitonHelper.GetCurrentHost();
            var parm = new PaypalExpressCheckoutParm();
            #region 主信息
            parm.SuccessUrl = scheme + "/Payment/GetPaypalExpressCheckoutDetails";
            parm.CancelUrl = UrlRewriteHelper.GetShoppingCartUrl();
            parm.LocalCode = string.Empty;//暂时不设置
            parm.LogoUrl = paypalExpressConfig.LogoUrl;
            parm.CurrencyCode = currency.CurrencyCode;
            parm.ShippingAmount = shippingAmount;
            parm.PayAmount = totalAmount;//todo 是否需要币种转换?
            #endregion

            #region 购物车商品明细(暂时不提交明细）

            parm.OrderItems = new List<PayPalOrderItem>();

            #endregion

            //账单地址
            var billingAddress = ServiceFactory.CustomerService.GetDefaultBillingAddress(customer.CustomerId);
            if (billingAddress != null)
            {
                parm.Address = new PayPalAddress();
                parm.Address.Email = customer.Email;
                parm.Address.City = billingAddress.City;
                parm.Address.CountryCode = CacheHelper.GetCountryCode(billingAddress.Country);
                parm.Address.FirstName = billingAddress.FirstName;
                parm.Address.LastName = billingAddress.LastName;
                parm.Address.Phone = billingAddress.Telphone;
                parm.Address.State = billingAddress.Province;
                parm.Address.Street1 = billingAddress.Street1;
                parm.Address.Street2 = billingAddress.Street2;
                parm.Address.Zip = billingAddress.ZipCode;
            }

            #endregion

            #region 请求服务
            try
            {
                var result = PaypalService.SetExpressCheckout(parm);
                if (result.IsSuccess())
                {
                    //需要记住使用的Token，每次都更新这个Token
                    SessionHelper.PaypalExpressToken = result.Token;

                    jsonData.Succeed = true;
                    jsonData.Data = result.PayUrl;
                }
                else
                {
                    jsonData.Message = "ERROR_SETEXPRESSCHECKOUT_ASK_FAIL";
                }
            }
            catch (MaxAmountOutOfRangeException rex)
            {
                jsonData.Message = "ERROR_SETEXPRESSCHECKOUT_MAXAMOUNTOUTOFRANGE";
                jsonData.Data = rex.Currency;
                jsonData.ExtraData = rex.MaxAmount;

                _paymentLogger.Error(string.Format("PayPal快速支付请求出错!,币种:{0},最大允许金额:{1},支付金额:{2},\n\r错误消息:{3}", rex.Currency, rex.MaxAmount, rex.PayAmount, rex.Message), rex);
            }
            catch (PayPalException pex)
            {
                jsonData.Message = "ERROR_SETEXPRESSCHECKOUT_PAYPALEXCEPTION";

                _paymentLogger.Error(string.Format("PayPal快速支付请求出错!,错误编码:{0}\n\r,错误消息:{1}", pex.Code, pex.Message), pex);
            }
            #endregion


            return Json(jsonData);
        }
        #endregion

        #region 步骤二:调用GetExpressCheckoutDetails获取支付信息
        /// <summary>
        /// 根据Token获取支付信息
        /// </summary> 
        /// <returns></returns>
        public ActionResult GetPaypalExpressCheckoutDetails()
        {
            var token = SessionHelper.PaypalExpressToken;
            if (string.IsNullOrEmpty(token))
            {
                return Redirect(UrlRewriteHelper.GetShoppingCartUrl());
            }

            PaymentCommon.PayInfo.PaypalInfo paypalInfo = null;
            try
            {
                paypalInfo = PaypalService.GetExpressCheckoutDetails(token);
            }
            catch (PayPalException pex)
            {
                _paymentLogger.Error(string.Format("GetPaypalExpressCheckoutDetails - PayPal快速支付获取支付信息出错!,错误编码:{0}\n\r,错误消息:{1}", pex.Code, pex.Message), pex);
                return new EmptyResult();
            }

            if (paypalInfo == null)
            {
                _paymentLogger.ErrorFormat("GetPaypalExpressCheckoutDetails - PayPal快速支付获取支付信息出错!,返回信息为null");
                return new EmptyResult();
            }

            SessionHelper.PaypalExpressPayInfo = paypalInfo;

            return Redirect(UrlRewriteHelper.GetCheckoutUrl());
        }
        #endregion

        #region 步骤三:调用DoExpressCheckoutPayment扣款
        /// <summary>
        /// 支付中心页面扣款按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult DoPaypalExpressCheckoutPayment()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            var customer = SessionHelper.CurrentCustomer;

            #endregion

            #region 验证基本参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            //支付token失效
            if (SessionHelper.PaypalExpressPayInfo == null || string.IsNullOrEmpty(SessionHelper.PaypalExpressPayInfo.Token) || string.IsNullOrEmpty(SessionHelper.PaypalExpressPayInfo.PayerId))
            {
                jsonData.Message = "ERROR_PAYPAL_EXPRESS_EXPIRE";
                return Json(jsonData);
            }

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);
            if (order == null || order.OrderStatus != OrderStatusType.Pending)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }

            #endregion

            var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            if (orderAmount == null)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(customer.CustomerId);

            var payAmountUsd = orderAmount.NeedToPayAmt + debtCashUsd;

            //汇率转换为订单币种对应的金额进行支付
            var payCurrencyCode = ServiceFactory.PaymentService.IsCurrencyUseUsdForPaypal(order.Currency) ? PageHelper.CURRENCY_CODE_USD : order.Currency;

            var currency = CacheHelper.GetCurrencyByCode(payCurrencyCode);

            var paymentAmount = payAmountUsd * (string.Equals(PageHelper.CURRENCY_CODE_USD, payCurrencyCode, StringComparison.InvariantCultureIgnoreCase) ? 1.00M : order.ExchangeRate);//汇率转换为订单币种对应的金额
             
            paymentAmount = PageHelper.GetRoundValue(paymentAmount, currency.DecimalPlaces);

            var paymentParm = new DoPaypalExpressCheckoutPaymentParm
            {
                CurrencyCode = payCurrencyCode,
                PaymentAmount = paymentAmount,
                OrderNo = orderNo,
                OrderId = order.OrderId
            };

            var result = DoPaypalExpressCheckoutPayment(paymentParm);

            //扣款结果处理
            switch (result.PaypalExpressResultType)
            {
                case PaypalExpressResultType.Success:
                    jsonData.Succeed = true;
                    break;
                case PaypalExpressResultType.MaxAmountOutOfRange:
                case PaypalExpressResultType.PayPalException:
                case PaypalExpressResultType.PaySuccess:
                default:
                    jsonData.Data = (int)result.PaypalExpressResultType;
                    jsonData.ExtraData = result.PayPalExpressCheckoutUrl;
                    break;
            }

            return Json(jsonData);
        }

        /// <summary>
        /// Paypal快速支付扣款
        /// </summary>
        /// <param name="paymentParm"></param>
        /// <returns></returns>
        internal static DoPaypalExpressCheckoutPaymentResult DoPaypalExpressCheckoutPayment(DoPaypalExpressCheckoutPaymentParm paymentParm)
        {
            #region 获取数据
            var sessionPaypalInfo = SessionHelper.PaypalExpressPayInfo;

            #endregion

            #region 验证数据

            #endregion

            #region 请求扣款
            var result = new DoPaypalExpressCheckoutPaymentResult
                {
                    PaypalExpressResultType = PaypalExpressResultType.UnKnowError
                };

            PaymentCommon.PayInfo.PaypalInfo payInfo = null;

            try
            {
                payInfo = PaypalService.DoExpressCheckoutPayment(sessionPaypalInfo.Token, sessionPaypalInfo.PayerId, paymentParm.CurrencyCode, paymentParm.PaymentAmount, paymentParm.OrderNo, paymentParm.OrderId);

                //以下数据在第二步获取的，第三步需要赋值过来
                payInfo.PayerEmail = payInfo.PayerEmail.IsNullOrEmpty() ? sessionPaypalInfo.PayerEmail : payInfo.PayerEmail;
                payInfo.PayerId = payInfo.PayerId.IsNullOrEmpty() ? sessionPaypalInfo.PayerId : payInfo.PayerId;
                payInfo.ReceiverId = payInfo.ReceiverId.IsNullOrEmpty() ? sessionPaypalInfo.ReceiverId : payInfo.ReceiverId;
                payInfo.ReceiverEmail = payInfo.ReceiverEmail.IsNullOrEmpty() ? sessionPaypalInfo.ReceiverEmail : payInfo.ReceiverEmail;
                payInfo.ResidenceCountry = payInfo.ResidenceCountry.IsNullOrEmpty() ? sessionPaypalInfo.ResidenceCountry : payInfo.ResidenceCountry;
                payInfo.FirstName = payInfo.FirstName.IsNullOrEmpty() ? sessionPaypalInfo.FirstName : payInfo.FirstName;
                payInfo.LastName = payInfo.LastName.IsNullOrEmpty() ? sessionPaypalInfo.LastName : payInfo.LastName;
                payInfo.AddressName = payInfo.AddressName.IsNullOrEmpty() ? sessionPaypalInfo.AddressName : payInfo.AddressName;
                payInfo.AddressStatus = payInfo.AddressStatus.IsNullOrEmpty() ? sessionPaypalInfo.AddressStatus : payInfo.AddressStatus;
                payInfo.AddressCountryCode = payInfo.AddressCountryCode.IsNullOrEmpty() ? sessionPaypalInfo.AddressCountryCode : payInfo.AddressCountryCode;
                payInfo.AddressCountryName = payInfo.AddressCountryName.IsNullOrEmpty() ? sessionPaypalInfo.AddressCountryName : payInfo.AddressCountryName;
                payInfo.AddressState = payInfo.AddressState.IsNullOrEmpty() ? sessionPaypalInfo.AddressState : payInfo.AddressState;
                payInfo.AddressCity = payInfo.AddressCity.IsNullOrEmpty() ? sessionPaypalInfo.AddressCity : payInfo.AddressCity;
                payInfo.AddressStreet1 = payInfo.AddressStreet1.IsNullOrEmpty() ? sessionPaypalInfo.AddressStreet1 : payInfo.AddressStreet1;
                payInfo.AddressStreet2 = payInfo.AddressStreet2.IsNullOrEmpty() ? sessionPaypalInfo.AddressStreet2 : payInfo.AddressStreet2;
                payInfo.AddressZip = payInfo.AddressZip.IsNullOrEmpty() ? sessionPaypalInfo.AddressZip : payInfo.AddressZip;
                payInfo.PhoneNumber = payInfo.PhoneNumber.IsNullOrEmpty() ? sessionPaypalInfo.PhoneNumber : payInfo.PhoneNumber;
                payInfo.Custom = payInfo.Custom.IsNullOrEmpty() ? sessionPaypalInfo.Custom : payInfo.Custom;
                payInfo.Commonts = payInfo.Commonts.IsNullOrEmpty() ? sessionPaypalInfo.Commonts : payInfo.Commonts;

                result.PaypalExpressResultType = PaypalExpressResultType.PaySuccess;
            }
            catch (MaxAmountOutOfRangeException rex)
            {
                result.PaypalExpressResultType = PaypalExpressResultType.MaxAmountOutOfRange;
            }
            catch (PayPalException pex)
            {
                result.PaypalExpressResultType = PaypalExpressResultType.PayPalException;

                if (!string.IsNullOrEmpty(pex.Code) && pex.Code == "10486")
                {
                    result.PayPalExpressCheckoutUrl = PaypalService.GetExpressCheckoutRedirectUrl(sessionPaypalInfo.Token) + "&useraction=commit";
                }
            }
            #endregion

            #region 执行网站业务逻辑
            if (result.PaypalExpressResultType == PaypalExpressResultType.PaySuccess)
            {
                try
                {
                    var paypalInfo = ConvertPaymentInfo(payInfo);
                    paypalInfo.IsExpressCheckOut = true;

                    ServiceFactory.OrderService.CustomerPayOrderByPaypal(paypalInfo.ItemNumber, paypalInfo);

                    result.PaypalExpressResultType = PaypalExpressResultType.Success;
                }
                catch (BussinessException bussinessException)
                {
                    result.ErrorMessage = bussinessException.GetError();
                }
            }
            #endregion

            #region 清空缓存数据
            //支付成功或者失败都需要清空掉
            SessionHelper.PaypalExpressToken = null;
            SessionHelper.PaypalExpressPayInfo = null;
            #endregion

            return result;
        }
        #endregion
        #endregion

        #region Gc支付
        #region 步骤一：请求GC支付获得交易支付地址(GlobalCollectParm.payUrl)
        public ActionResult LoadGcIframe()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            var globalCollectType = (PaymentCommon.PayConfig.GlobalCollectType)Request["globalCollectType"].ParseTo(0);
            var customer = SessionHelper.CurrentCustomer;
            #endregion

            #region 验证基本参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);
            if (order == null || order.OrderStatus != OrderStatusType.Pending)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var orderBillingAddress = ServiceFactory.OrderService.GetOrderBillingAddressByOrderId‎(order.OrderId);
            if (orderBillingAddress == null)
            {
                jsonData.Message = "ERROR_ORDER_BILLING_ADDRESS_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var orderShippingAddress = ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);
            if (orderShippingAddress == null)
            {
                jsonData.Message = "ERROR_ORDER_SHIPPING_ADDRESS_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }
            #endregion

            var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            if (orderAmount == null)
            {
                jsonData.Message = "ERROR_ORDER_AMOUNT_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(customer.CustomerId);

            var payAmountUsd = orderAmount.NeedToPayAmt + debtCashUsd;

            var paymentAmount = payAmountUsd * order.ExchangeRate;//汇率转换为订单币种对应的金额

            var currency = CacheHelper.GetCurrencyByCode(order.Currency);

            paymentAmount = PageHelper.GetRoundValue(paymentAmount, currency.DecimalPlaces);

            string gcOrderId = null;
            try
            {
                gcOrderId = ServiceFactory.PaymentService.GenerateGcOrderNo();
            }
            catch (Exception ex)
            {
                _paymentLogger.Error(string.Format("LoadGcIframe - 生成GC订单号异常，异常信息：{0}", ex.Message), ex);
                jsonData.Message = "ERROR_GENERATE_GC_ORDERNO";
                return Json(jsonData);
            }

            var scheme = UrlFuncitonHelper.GetCurrentHost();
            var gcParm = new GlobalCollectParm();
            gcParm.GcOrderId = gcOrderId;
            gcParm.WebOrderId = orderNo;
            gcParm.Amount = paymentAmount;
            gcParm.CurrencyCode = order.Currency;
            gcParm.EffortId = string.Empty;
            gcParm.Email = customer.Email;
            gcParm.GlobalCollectType = globalCollectType;
            gcParm.ReturnUrl = scheme + "/Payment/DoGlobalCollect?orderNo=" + orderNo;
            gcParm.BillingAddress = ConvertBillingAddress(orderBillingAddress);
            gcParm.ShippingAddress = ConvertShipingAddress(orderShippingAddress);
            gcParm.MerchantReference = CreateMerchantReference(orderNo);

            try
            {
                gcParm = GlobalCollectService.InsertOrderWithPayment(gcParm);

                jsonData.Succeed = true;
                jsonData.Data = gcParm.PayUrl;

                //保存最后一次GC请求的信息
                SessionHelper.GlobalCollectParmInfo = gcParm;
            }
            catch (MaxAmountOutOfRangeException rex)
            {
                jsonData.Message = "ERROR_GC_MAXAMOUNTOUTOFRANGE";
                jsonData.Data = rex.Currency;
                jsonData.ExtraData = rex.MaxAmount;

                _paymentLogger.Error(string.Format("LoadGcIframe - 加载GC支付页面错误,币种:{0},最大允许金额:{1},支付金额:{2},\n\r错误消息:{3}", rex.Currency, rex.MaxAmount, rex.PayAmount, rex.Message), rex);
            }
            catch (GlobalCollectException gx)
            {
                jsonData.Message = "ERROR_GC_GLOBALCOLLECTEXCEPTION";

                jsonData.Data = gx.ErrorCode;
                jsonData.ExtraData = gx.Message;

                _paymentLogger.Error(string.Format("LoadGcIframe - 加载GC支付页面错误!,错误编码:{0}\n\r,错误消息:{1}", gx.ErrorCode, gx.Message), gx);
            }
            catch (Exception ex)
            {
                jsonData.Message = "ERROR_GC_EXCEPTION";

                jsonData.Data = ex.Message;
            }

            return this.Json(jsonData);
        }

        private static GlobalCollectBillingAddress ConvertBillingAddress(OrderBillingAddress address)
        {
            var item = new GlobalCollectBillingAddress();
            item.City = address.City;
            item.CountryCode = CacheHelper.GetCountryCode(address.Country);
            item.Fax = string.Empty;
            item.FirstName = address.FirstName;
            item.LastName = address.LastName;
            item.State = address.Province;
            item.Street = string.Format("{0}\n\r{1}", address.Street1, address.Street2);
            item.Zip = address.ZipCode;

            item.CompanyName = address.CompanyName ?? string.Empty;
            item.VatCode = item.VatCode ?? string.Empty;
            return item;
        }

        private static GlobalCollectShipingAddress ConvertShipingAddress(OrderShippingAddress address)
        {
            var item = new GlobalCollectShipingAddress();
            item.City = address.City;
            item.CountryCode = CacheHelper.GetCountryCode(address.Country);
            item.Fax = string.Empty;
            item.FirstName = address.FirstName;
            item.LastName = address.LastName;
            item.State = address.Province;
            item.Street = string.Format("{0}\n\r{1}", address.Street1, address.Street2);
            item.Zip = address.ZipCode;
            item.Phone = address.Telphone;

            item.CompanyName = address.CompanyName ?? string.Empty;
            item.VatCode = item.VatCode ?? string.Empty;
            return item;
        }



        #endregion

        #region 步骤二：获取支付信息,主要得到交易Id(GlobalCollectInfo.effortId),该步骤内部调用

        #endregion

        #region 步骤三：通知GC扣款
        public ActionResult DoGlobalCollect()
        {
            _paymentLogger.ErrorFormat("DoGlobalCollect - 进入方法 -IP地址:{0},\n\r订单号:{1},\n\rREF:{2},\n\rRETURNMAC:{3}", PageManager.GetClientIp(), Request["orderNo"], Request["REF"], Request["RETURNMAC"]);

            #region 获取参数
            var orderNo = Request["orderNo"] ?? string.Empty;
            var strRef = Request["REF"];
            var strMac = Request["RETURNMAC"] ?? string.Empty;
            var globalCollectParmInfo = SessionHelper.GlobalCollectParmInfo;
            var customer = SessionHelper.CurrentCustomer;
            var statusId = string.Empty;
            var errorMesage = string.Empty;
            #endregion

            #region 验证参数
            if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(strRef) || strRef.Length < 20)
            {
                return RedirectErrorPage(orderNo, statusId, errorMesage);
            }
            #endregion

            var gcOrderId = strRef.Substring(10, 10);
            var scheme = UrlFuncitonHelper.GetPaymentReturnHost();
            try
            {
                var gcInfo = new PaymentCommon.PayInfo.GlobalCollectInfo
                {
                    GcOrderId = gcOrderId
                };

                //执行GC扣款操作
                var isOk = GlobalCollectService.ConfirmPayment(ref gcInfo);

                gcInfo.OrderNo = string.IsNullOrEmpty(orderNo) ? GetWebOrderIdFromMerchantReference(gcInfo.MerchantReference) : orderNo;

                if (isOk)
                {
                    //GC扣款成功后执行网站内部业务支付操作
                    var globalCollectInfo = ConvertGlobalCollectInfo(gcInfo);
                    if (globalCollectParmInfo != null)
                    {
                        globalCollectInfo.RefData = globalCollectParmInfo.Ref;
                        globalCollectInfo.Mac = globalCollectParmInfo.Mac;
                        globalCollectInfo.PayUrl = globalCollectParmInfo.PayUrl;

                        SessionHelper.GlobalCollectParmInfo = null;
                    }

                    ServiceFactory.OrderService.CustomerPayOrderByGc(globalCollectInfo.OrderNo, globalCollectInfo);

                    //todo iframe跳转到GC支付完成地址，显示支付成功信息
                    if (!SessionHelper.LastOrderNumber.IsNullOrEmpty() && SessionHelper.LastOrderNumber.Equals(orderNo))
                    {
                        return RedirectByScript("/Checkout/Succeed");
                    }
                    else
                    {
                        return RedirectByScript(UrlRewriteHelper.GetOrderDetail(gcInfo.OrderNo));
                    }
                }
                else
                {
                    if (gcInfo.StatusId == "25" || gcInfo.CreditCardNo.IsNullOrEmpty())
                    {
                        statusId = "-1";
                        errorMesage = ""; //CANCAL_PAYMENT
                    }
                    else
                    {
                        statusId = gcInfo.StatusId;
                        errorMesage = gcInfo.Message; //CANCAL_PAYMENT
                    }
                    
                    _paymentLogger.ErrorFormat("DoGlobalCollect - GC扣款失败 - 订单号:{0},GC订单号:{1}", gcInfo.OrderNo, gcInfo.GcOrderId);
                }
            }
            catch (GlobalCollectException gx)
            {
                statusId = gx.ErrorCode;
                errorMesage = gx.Message;

                _paymentLogger.Error(string.Format("DoGlobalCollect - GC扣款异常!,错误编码:{0}\n\r,错误消息:{1}", gx.ErrorCode, gx.Message), gx);
            }
            catch (BussinessException bussinessException)
            {
                statusId = "-888";
                errorMesage = "ERROR_PAY_SUCCESS_HANDLE_FAIL";

                _paymentLogger.Error(string.Format("DoGlobalCollect - GC扣款成功-执行业务操作异常!,错误编码:{0}\n\r,错误消息:{1}", bussinessException.GetError()), bussinessException);
            }

            return RedirectErrorPage(orderNo, statusId, errorMesage);
        }

        private Com.Panduo.Service.Payment.PayInfo.GlobalCollectInfo ConvertGlobalCollectInfo(PaymentCommon.PayInfo.GlobalCollectInfo gcInfo)
        {
            Com.Panduo.Service.Payment.PayInfo.GlobalCollectInfo globalCollectInfo = null;
            if (gcInfo != null)
            {
                globalCollectInfo = new GlobalCollectInfo();
                ObjectHelper.CopyProperties(gcInfo, globalCollectInfo, new[] { "GlobalCollectType" });
                globalCollectInfo.GlobalCollectType = (Com.Panduo.Service.Payment.PayConfig.GlobalCollectType)(int)gcInfo.GlobalCollectType;

            }

            return globalCollectInfo;
        }


        #endregion

        private static string CreateMerchantReference(string orderNo)
        {
            return string.Format("{0}#{1}", orderNo, Guid.NewGuid());
        }

        /// <summary>
        /// 从MerchantReference获取网站订单号
        /// </summary>
        /// <param name="merchantReference"></param>
        /// <returns></returns>
        private static string GetWebOrderIdFromMerchantReference(string merchantReference)
        {
            if (string.IsNullOrEmpty(merchantReference))
            {
                return string.Empty;
            }
            //截取merchantReference得到网站原始订单号
            return merchantReference.IndexOf("#", StringComparison.InvariantCultureIgnoreCase) > 0 ? merchantReference.Substring(0, merchantReference.IndexOf("#", StringComparison.InvariantCultureIgnoreCase)) : merchantReference;
        }
        #endregion

        #region 钱海支付(Webmoney、Yandex、QiWi、Credit Card)
        #region 请求支付
        /// <summary>
        /// 请求用钱海支付订单验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult RequestPayOrderByOceanPaymentVertify()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var oceanPaymentType = (PaymentType)Request["oceanPaymentType"].ParseTo(0);
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            var customer = SessionHelper.CurrentCustomer;
            #endregion

            #region 验证基本参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);
            if (order == null || order.OrderStatus != OrderStatusType.Pending)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            var orderBillingAddress = ServiceFactory.OrderService.GetOrderBillingAddressByOrderId‎(order.OrderId);
            if (orderBillingAddress == null)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            //非钱海支付接口数据类型
            if (oceanPaymentType != PaymentType.OceanCreditCard && oceanPaymentType != PaymentType.QiWi && oceanPaymentType != PaymentType.Webmoney && oceanPaymentType != PaymentType.Yandex)
            {
                jsonData.Message = "ERROR_PAYMENTTYPE_WRONG";
                return Json(jsonData);
            }

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }
            #endregion

            var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            if (orderAmount == null)
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            jsonData.Succeed = true;

            return Json(jsonData);
        }

        /// <summary>
        /// 请求用钱海支付方式支付订单
        /// </summary>
        /// <returns></returns> 
        [OutputCache(Duration = 0)]
        public ActionResult RequestPayOrderByOceanPayment()
        { 
            #region 获取参数
            var orderNo = Request["orderNo"];
            var oceanPaymentType = (PaymentType)Request["oceanPaymentType"].ParseTo(0); 
            var customer = SessionHelper.CurrentCustomer;
            #endregion

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);

            var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            
            var orderBillingAddress = ServiceFactory.OrderService.GetOrderBillingAddressByOrderId‎(order.OrderId);

            var scheme = UrlFuncitonHelper.GetPaymentReturnHost();

            var debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(customer.CustomerId);

            var payAmountUsd = orderAmount.NeedToPayAmt + debtCashUsd;

            var paymentAmount = payAmountUsd * order.ExchangeRate;//汇率转换为订单币种对应的金额

            var currency = CacheHelper.GetCurrencyByCode(order.Currency);

            paymentAmount = PageHelper.GetRoundValue(paymentAmount, currency.DecimalPlaces);

            var oceanPaymentConfig = ServiceFactory.PaymentService.GetOceanPaymentConfig();
            var method = GetOceanPaymentMethod(oceanPaymentType);
            var methodConfig = oceanPaymentConfig.TryGetMethod(method);

            var oceanPaymentParm = new OceanPaymentParm
            {
                Account = methodConfig.Account,
                Terminal = methodConfig.Terminal,
                SignValue = string.Empty,
                BackUrl = scheme + "/Payment/OceanPaymentNotifyForPayOrder",
                Methods = method,
                Pages = "0",
                OrderNumber = orderNo,
                OrderCurrency = order.Currency,
                OrderAmount = paymentAmount,
                OrderNotes = string.Empty,
                BillingAddress = ConvertOceanPaymentBillingAddress(orderBillingAddress),
                ShippingAddress = null//目前用不到
            };

            //获取签名
            oceanPaymentParm.SignValue = OceanPaymentService.GetRequestSignValue(oceanPaymentParm);

            var model = new OceanPaymentSubmitParm
            {
                ServiceUrl = oceanPaymentConfig.ServiceUrl,
                OceanPaymentConfig = oceanPaymentParm,
                BillingAddress = orderBillingAddress,
                Order = order

            };

            return View("SubmitToOceanPayment", model);
        }

        private static OceanPaymentBillingAddress ConvertOceanPaymentBillingAddress(OrderBillingAddress billingAddress)
        {
            OceanPaymentBillingAddress paymentBillingAddress = null;
            if (billingAddress != null)
            {
                paymentBillingAddress = new OceanPaymentBillingAddress();
                ObjectHelper.CopyProperties(billingAddress, paymentBillingAddress, new[] { "Country" });
                paymentBillingAddress.Email = SessionHelper.CurrentCustomer.Email;
                paymentBillingAddress.Phone = billingAddress.Telphone;
                paymentBillingAddress.State = billingAddress.Province;
                paymentBillingAddress.Zip = billingAddress.ZipCode;
                paymentBillingAddress.Address = string.Format("{0}{1}", billingAddress.Street1, billingAddress.Street2);

                paymentBillingAddress.CountryCode = CacheHelper.GetCountryCode(billingAddress.Country);
            }

            return paymentBillingAddress;
        }
        #endregion

        #region 支付返回
        /// <summary>
        /// OceanPayment响应支付订单(Notify)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult OceanPaymentNotifyForPayOrder()
        {
            var qs = Request.Form.ToQueryString();

            _paymentLogger.InfoFormat("OceanPaymentNotifyForPayOrder - 钱海支付返回进入,参数:{0}", qs);

            var isSuccess = false;
            var errorCode = string.Empty;
            var errorMessage = string.Empty;
            var orderNo = string.Empty;
            var method = Request["methods"];
            
            #region 支付返回验证
            OceanPaymentResult oceanPaymentResult = null;
            try
            {
                oceanPaymentResult = OceanPaymentService.OceanPaymentVerify(Request.Form, null);

                if (oceanPaymentResult == null)
                {
                    _paymentLogger.ErrorFormat("OceanPaymentNotifyForPayOrder - 钱海支付失败。支付验证返回Null。\r\n");
                }
                else
                {
                    if (!oceanPaymentResult.IsValid)
                    {
                        _paymentLogger.ErrorFormat("OceanPaymentNotifyForPayOrder - 钱海支付失败。支付验证返回False。\r\n");
                    }

                    errorCode = oceanPaymentResult.OceanPaymentInfo.ErrorCode;
                    errorMessage = oceanPaymentResult.OceanPaymentInfo.PaymentDetails;
                } 
            }
            catch (Exception ex)
            {
                errorCode = "-1";
                errorMessage = ex.Message;

                _paymentLogger.Error(string.Format("OceanPaymentNotifyForPayOrder -钱海支付处理出错。\r\nRequestForm:{0}", qs), ex);
            }
            #endregion

            #region 业务方法执行
            if (oceanPaymentResult != null && oceanPaymentResult.IsValid)
            {
                var oceanPaymentInfo = ConvertOceanPaymentInfo(oceanPaymentResult.OceanPaymentInfo);
                orderNo = oceanPaymentInfo.OrderNumber; 
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByOceanPayment(oceanPaymentInfo.OrderNumber, oceanPaymentInfo);
                }
                catch (BussinessException ex)
                {
                    _paymentLogger.Error(string.Format("OceanPaymentNotifyForPayOrder - PayPal钱海支付处理出错。\r\nRequestForm:{0}", qs), ex);
                }

                isSuccess = true;
            }
            #endregion

            if (isSuccess)
            {
                //todo 跳转到支付成功页面
                return Redirect(UrlRewriteHelper.GetOrderDetail(orderNo));
            }

            //todo 跳转到支付失败页面
            return RedirectOceanPaymentErrorPage(orderNo, method, errorCode, errorMessage);
        }

        private static ActionResult RedirectOceanPaymentErrorPage(string orderId,string method,string errorCode, string errorMessage)
        {
            var paymentType = PaymentType.QiWi;
            if (string.Equals("Webmoney", method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.Webmoney; ;
            }
            else if (string.Equals("Yandex", method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.Yandex; ;
            }
            else if (string.Equals("Credit Card", method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.OceanCreditCard; ;
            }
            else if (string.Equals("QiWi", method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.QiWi; ;
            }
            
            SessionHelper.GlobalCollectErrorMessage = errorMessage;

            return RedirectByScript(string.Format(UrlRewriteHelper.GetOrderDetailPayment(orderId) + "&payMethod={0}&errorCode={1}", (int)paymentType, errorCode));
        }

        private static Com.Panduo.Service.Payment.PayInfo.OceanPaymentInfo ConvertOceanPaymentInfo(
            Com.Panduo.Web.PaymentCommon.PayInfo.OceanPaymentInfo paymentInfo)
        {
            OceanPaymentInfo oceanPaymentInfo = null;
            if (paymentInfo != null)
            {
                oceanPaymentInfo = new OceanPaymentInfo();
                ObjectHelper.CopyProperties(paymentInfo, oceanPaymentInfo, new string[] { });
            }

            return oceanPaymentInfo;
        }
        #endregion

        public static string GetOceanPaymentMethod(PaymentType paymentType)
        {
            switch (paymentType)
            {
                case PaymentType.Paypal:
                    break;
                case PaymentType.Hsbc:
                    break;
                case PaymentType.BankOfChina:
                    break;
                case PaymentType.WesternUnion:
                    break;
                case PaymentType.Gc:
                    break;
                case PaymentType.MoneyGram:
                    break;
                case PaymentType.Webmoney:
                    return "Webmoney";
                case PaymentType.Yandex:
                    return "Yandex";
                case PaymentType.QiWi:
                    return "Qiwi";
                case PaymentType.OceanCreditCard:
                    return "Credit Card";
            }
            return string.Empty;
        }

        public static PaymentType? GetPaymentTypeFromMethod(string method)
        {
            if (string.Equals("Webmoney", method, StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentType.Webmoney;
            }
            else if (string.Equals("Yandex", method, StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentType.Yandex;
            }
            else if (string.Equals("Qiwi", method, StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentType.QiWi;
            }
            else if (string.Equals("Credit Card", method, StringComparison.InvariantCultureIgnoreCase) || string.Equals("CreditCard", method, StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentType.OceanCreditCard;
            }

            return null;
        }
        #endregion

        #region HSBC支付
        /// <summary>
        /// 通过中国工商银行支付订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult PayOrderByHsbc()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var currencyCode = Request["currencyCode"];
            var isStandardCurrency = Request["isStandard"].ParseTo(false);
            var amount = Request["amount"].ParseTo(0.00M);
            var paymentDate = Request["paymentDate"].ParseTo(DateTime.MinValue);
            var paymentReceipt = Request["paymentReceipt"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (currencyCode.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_CURRENCY_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (amount <= 0)
            {
                jsonData.Message = "ERROR_AMOUNT_MAST_ABOVE_ZERO";
                return Json(jsonData);
            }

            if (paymentDate == DateTime.MinValue)
            {
                jsonData.Message = "ERROR_PAYMENT_DATE_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 组装业务参数
            var hsbcInfo = new HsbcInfo
            {
                OrderNo = orderNo,
                IsStandardCurrency = isStandardCurrency,
                CurrencyCode = currencyCode,
                Amount = amount,
                PaymentDate = paymentDate,
                PaymentReceipt = paymentReceipt
            };

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }

            try
            {
                ServiceFactory.OrderService.CustomerPayOrderByHsbc(orderNo, hsbcInfo);

                jsonData.Succeed = true;
            }
            catch (BussinessException bussinessException)
            {
                jsonData.Message = bussinessException.GetError();
            }
            #endregion

            return Json(jsonData);
        }

        #endregion

        #region Bank Of China支付
        /// <summary>
        /// 通过中国银行支付订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult PayOrderByBankOfChina()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var currencyCode = Request["currencyCode"];
            var isStandardCurrency = Request["isStandard"].ParseTo(false);
            var amount = Request["amount"].ParseTo(0.00M);
            var paymentDate = Request["paymentDate"].ParseTo(DateTime.MinValue);
            var paymentReceipt = Request["paymentReceipt"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (currencyCode.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_CURRENCY_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (amount <= 0)
            {
                jsonData.Message = "ERROR_AMOUNT_MAST_ABOVE_ZERO";
                return Json(jsonData);
            }

            if (paymentDate == DateTime.MinValue)
            {
                jsonData.Message = "ERROR_PAYMENT_DATE_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 组装业务参数
            var bankOfChinaInfo = new BankOfChinaInfo
            {
                OrderNo = orderNo,
                IsStandardCurrency = isStandardCurrency,
                CurrencyCode = currencyCode,
                Amount = amount,
                PaymentDate = paymentDate,
                PaymentReceipt = paymentReceipt
            };

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }

            try
            {
                ServiceFactory.OrderService.CustomerPayOrderByBankOfChina(orderNo, bankOfChinaInfo);

                jsonData.Succeed = true;
            }
            catch (BussinessException bussinessException)
            {
                jsonData.Message = bussinessException.GetError();
            }
            #endregion

            return Json(jsonData);
        }
        #endregion

        #region WesternUnion支付
        /// <summary>
        /// 通过西联汇款支付订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult PayOrderByWesternUnion()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var currencyCode = Request["currencyCode"];
            var isStandardCurrency = Request["isStandard"].ParseTo(false);
            var amount = Request["amount"].ParseTo(0.00M);
            var controlNo = Request["controlNo"];
            var paymentReceipt = Request["paymentReceipt"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (currencyCode.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_CURRENCY_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (amount <= 0)
            {
                jsonData.Message = "ERROR_AMOUNT_MAST_ABOVE_ZERO";
                return Json(jsonData);
            }

            if (controlNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_CONTROL_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 组装业务参数
            var westernUnionInfo = new WesternUnionInfo
            {
                OrderNo = orderNo,
                IsStandardCurrency = isStandardCurrency,
                CurrencyCode = currencyCode,
                Amount = amount,
                ControlNo = controlNo,
                PaymentReceipt = paymentReceipt
            };

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }

            try
            {
                ServiceFactory.OrderService.CustomerPayOrderByWesternUnion(orderNo, westernUnionInfo);

                jsonData.Succeed = true;
            }
            catch (BussinessException bussinessException)
            {
                jsonData.Message = bussinessException.GetError();
            }
            #endregion

            return Json(jsonData);
        }

        #endregion

        #region MoneyGram支付
        /// <summary>
        /// 通过MoneyGram支付订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult PayOrderByMoneyGram()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderNo = Request["orderNo"];
            var fullNameOfRemitter = Request["fullNameOfRemitter"];
            var countryId = Request["countryId"].ParseTo(0);
            var currencyCode = Request["currencyCode"];
            var isStandardCurrency = Request["isStandard"].ParseTo(false);
            var amount = Request["amount"].ParseTo(0.00M);
            var controlNo = Request["controlNo"];
            var paymentReceipt = Request["paymentReceipt"];
            var isUseCashPartPay = Request["isUseCashPartPay"].ParseTo(false);
            #endregion

            #region 验证参数
            if (orderNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_ORDER_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (fullNameOfRemitter.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_FULL_NAME_OF_REMITTER_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (countryId <= 0)
            {
                jsonData.Message = "ERROR_COUNTRY_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (currencyCode.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_CURRENCY_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            if (amount <= 0)
            {
                jsonData.Message = "ERROR_AMOUNT_MAST_ABOVE_ZERO";
                return Json(jsonData);
            }

            if (controlNo.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_CONTROL_NO_CAN_NOT_EMPTY";
                return Json(jsonData);
            }

            #endregion

            #region 组装业务参数
            var moneyGramInfo = new MoneyGramInfo
            {
                OrderNo = orderNo,
                FullNameOfRemitter = fullNameOfRemitter,
                CountryId = countryId,
                IsStandardCurrency = isStandardCurrency,
                CurrencyCode = currencyCode,
                Amount = amount,
                ControlNo = controlNo,
                PaymentReceipt = paymentReceipt
            };

            #endregion

            #region 执行业务方法
            //Cash部分支付
            if (isUseCashPartPay)
            {
                try
                {
                    ServiceFactory.OrderService.CustomerPayOrderByCash(orderNo, false);
                }
                catch (BussinessException bussinessException)
                {
                    jsonData.Message = bussinessException.GetError();
                    return Json(jsonData);
                }
            }

            try
            {

                ServiceFactory.OrderService.CustomerPayOrderByMoneyGram(orderNo, moneyGramInfo);

                jsonData.Succeed = true;
            }
            catch (BussinessException bussinessException)
            {
                jsonData.Message = bussinessException.GetError();
            }
            #endregion

            return Json(jsonData);
        }
        #endregion

        #region 支付文件上传

        /// <summary>
        /// 上传支付信息的文件
        /// </summary>
        /// <param name="fileData">上传的文件</param>
        /// <param name="folder"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult UploadPaymentReceipt(HttpPostedFileBase fileData, string folder)
        {
            string extend = "";
            if (null != fileData)
            {
                try
                {
                    string fileName = Path.GetFileName(fileData.FileName);
                    if (fileName != null)
                    {
                        var savePath = ConfigurationManager.AppSettings["PaymentReceipt.Path"] ?? string.Empty;
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }
                        extend = Guid.NewGuid() + fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
                        try
                        {
                            //当天上传的文件放到以当天日期命名的文件夹中     
                            var dateFolder = DateTime.Now.Date.ToString("yyyy-MM-dd");
                            var saveFelder = Path.Combine(savePath, dateFolder);
                            if (!Directory.Exists(saveFelder))
                            {
                                Directory.CreateDirectory(saveFelder);
                            }
                            fileData.SaveAs(Path.Combine(saveFelder, extend));
                            extend = Path.Combine(dateFolder, extend);
                        }
                        catch (Exception e)
                        {
                            throw new ApplicationException(e.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    extend = ex.ToString();
                }
            }
            return Content(extend);
        }
        #endregion
    }
}
