using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Product;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Order;
using Resources;
using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate.Linq;

namespace Com.Panduo.Web.Controllers
{
    public class CheckoutController : BaseController
    {
        [HttpGet]
        public ActionResult Payment()
        {
            var currentCustomerId = SessionHelper.CurrentCustomer.CustomerId;
            string orderNumber = SessionHelper.LastOrderNumber;
            if (orderNumber.IsNullOrEmpty())
            {
                return Redirect(UrlRewriteHelper.GetShoppingCartUrl());
            }
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNumber);
            if (order.IsNullOrEmpty())
            {
                return RedirectToAction("Index", "Home");
            }
            if (order.OrderStatus != OrderStatusType.Pending)
            {
                return Redirect(UrlRewriteHelper.GetOrderDetail(orderNumber));
            }

            var orderShippingAddress = ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);
            int countryId = orderShippingAddress.IsNullOrEmpty() ? 0 : orderShippingAddress.Country;
            int currencyId = CacheHelper.GetCurrencyByCode(order.Currency).CurrencyId;
            ViewBag.Order = order;
            //  客户存在欠款
            ViewBag.debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(currentCustomerId);
            //  客户Cash
            ViewBag.cashBalanceUsd = ServiceFactory.CashService.GetCustomerBalance(currentCustomerId);
            //  错误信息
            ViewBag.ErrorMessage = null;
            //  是否paypal快速支付
            ViewBag.isPaypalExpress = false;

            #region paypal快速支付
            if (SessionHelper.PaypalExpressPayInfo != null && !SessionHelper.PaypalExpressPayInfo.Token.IsNullOrEmpty())
            {
                //  有出错信息，进行正常支付
                if (!RouteData.Values["PaypalExpressErrorType"].IsNullOrEmpty())
                {
                    ViewBag.ErrorMessage = RouteData.Values["PaypalExpressErrorType"];
                }
                else//否则只显示paypal快速支付
                {
                    ViewBag.isPaypalExpress = true;
                    return View(new Dictionary<string, object>());
                }
            }
            #endregion

            #region 判断是否能够使用支付配置
            ViewBag.canUseBankOfChina = ServiceFactory.PaymentService.CanUseBankOfChina(currentCustomerId, countryId, currencyId);
            ViewBag.canUseWesternUnion = ServiceFactory.PaymentService.CanUseWesternUnion(currentCustomerId, countryId, currencyId);
            ViewBag.canUseHsbc = ServiceFactory.PaymentService.CanUseHsbc(currentCustomerId, countryId, currencyId);
            ViewBag.canUseMoneyGram = ServiceFactory.PaymentService.CanUseMoneyGram(currentCustomerId, countryId, currencyId);
            ViewBag.canUsePaypal = ServiceFactory.PaymentService.CanUsePaypal(currentCustomerId, countryId, currencyId);
            //ViewBag.canUsePaypalExpress = ServiceFactory.PaymentService.CanUsePaypalExpress(currentCustomerId, countryId, currencyId);
            ViewBag.canUseGlobalCollect = ServiceFactory.PaymentService.CanUseGlobalCollect(currentCustomerId, countryId, currencyId);
            ViewBag.canUseWebmoney = ServiceFactory.PaymentService.CanUseOceanPayment("Webmoney", currentCustomerId, countryId, currencyId);
            ViewBag.canUseYandex = ServiceFactory.PaymentService.CanUseOceanPayment("Yandex", currentCustomerId, countryId, currencyId);
            ViewBag.canUseCreditCard = ServiceFactory.PaymentService.CanUseOceanPayment("Credit Card", currentCustomerId, countryId, currencyId);
            ViewBag.canUseQiWi = ServiceFactory.PaymentService.CanUseOceanPayment("QiWi", currentCustomerId, countryId, currencyId);
            ViewBag.canUseOceanPayment = ViewBag.canUseWebmoney || ViewBag.canUseYandex || ViewBag.canUseCreditCard || ViewBag.canUseQiWi;
            ViewBag.canUsePaypalCreditCard = !ViewBag.canUseGlobalCollect;
            #endregion

            #region 读取支付配置
            var paymentConfig = new Dictionary<string, object>();
            if (ViewBag.canUseBankOfChina)
            {
                paymentConfig.Add("BankOfChina", ServiceFactory.PaymentService.GetBankOfChinaConfig());
            }
            if (ViewBag.canUseWesternUnion)
            {
                paymentConfig.Add("WesternUnion", ServiceFactory.PaymentService.GetWesternUnionConfig());
            }
            if (ViewBag.canUseHsbc)
            {
                paymentConfig.Add("Hsbc", ServiceFactory.PaymentService.GetHsbcConfig());
            }
            if (ViewBag.canUseMoneyGram)
            {
                paymentConfig.Add("MoneyGram", ServiceFactory.PaymentService.GetMoneyGramConfig());
            }
            if (ViewBag.canUsePaypal)
            {
                paymentConfig.Add("Paypal", ServiceFactory.PaymentService.GetPaypalConfig());
            }
            //if (ViewBag.canUsePaypalExpress)
            //{
            //    paymentConfig.Add("PaypalExpress", ServiceFactory.PaymentService.GetPaypalExpressConfig());
            //}
            if (ViewBag.canUseGlobalCollect)
            {
                paymentConfig.Add("GlobalCollect", ServiceFactory.PaymentService.GetGlobalCollectConfig());
            }
            if (ViewBag.canUseOceanPayment)
            {
                paymentConfig.Add("OceanPayment", ServiceFactory.PaymentService.GetOceanPaymentConfig());
            }
            #endregion

            return View(paymentConfig);
        }

        [HttpGet]
        public ActionResult Succeed()
        {
            string orderNumber = SessionHelper.LastOrderNumber;
            SessionHelper.LastOrderNumber = null;
            if (orderNumber.IsNullOrEmpty())
            {
                return Redirect(UrlRewriteHelper.GetShoppingCartUrl());
            }
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNumber);
            if (order.IsNullOrEmpty())
            {
                return RedirectToAction("Index", "Home");
            }

            var orderDetailVo = new OrderDetailVo
            {
                Order = order,
                OrderShippingAddress = ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId),
                PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>()),
                ShippingName = CacheHelper.GetShippingName(order.ShippingId)
            };
            if (!orderDetailVo.OrderShippingAddress.IsNullOrEmpty())
            {
                orderDetailVo.ShippingDay = ServiceFactory.ShippingService.GetShippingDay(order.ShippingId, CacheHelper.GetCountryCode(orderDetailVo.OrderShippingAddress.Country));
            }

            var list = ServiceFactory.ProductService.SearchProducts(1, 6, new Dictionary<ProductSearchCriteria, object>() { { ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.BestSeller } }, new List<Sorter<ProductSorterCriteria>>(), false, false);
            var productList = list.ProductPageData.Data;
            var productinfos = ServiceFactory.ProductService.GetProductInfos(productList,
                                                                     isIncludeProductStock: true,
                                                                     isIncludeProductImage: false,
                                                                     isIncludeProductProperty: false,
                                                                     isIncludeProductPrice: true,
                                                                     isJudgeHotSeller: true,
                                                                     isJudgeHasSimilarProuct: false);
            ViewBag.ProductInfoList = productinfos;

            return View(orderDetailVo);
        }

        private string GetPaymentName(PaymentType payment)
        {
            switch (payment)
            {
                case PaymentType.Paypal:
                    return Lang.TipPaymentNamePaypal;

                case PaymentType.Hsbc:
                    return Lang.TipPaymentNameHsbc;

                case PaymentType.BankOfChina:
                    return Lang.TipPaymentNameBankOfChina;

                case PaymentType.WesternUnion://西联汇款
                    return Lang.TipPaymentNameWesternUnion;

                case PaymentType.Gc://GC信用卡
                    return Lang.TipPaymentNameGc;

                case PaymentType.MoneyGram://MoneyGram汇款
                    return Lang.TipPaymentNameMoneyGram;

                case PaymentType.Webmoney://Webmoney支付
                    return Lang.TipPaymentNameWebmoney;

                case PaymentType.Yandex://Yandex支付
                    return Lang.TipPaymentNameYandex;

                case PaymentType.QiWi://QiWi支付
                    return Lang.TipPaymentNameQiWi;

                case PaymentType.OceanCreditCard://钱海信用卡支付
                    return Lang.TipPaymentNameOceanCreditCard;

                case PaymentType.Cash://Cash全额支付
                    return Lang.TipPaymentNameCash;

                default:
                    return "找不到对应";
            }
        }
    }
}
