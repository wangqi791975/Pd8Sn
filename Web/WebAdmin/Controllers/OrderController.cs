using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Payment;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Order;

namespace Com.Panduo.Web.Controllers
{
    public class OrderController : Controller
    {

        private int AdminId
        {
            get { return SessionHelper.CurrentAdminUser == null ? 0 : SessionHelper.CurrentAdminUser.AdminId; }
        }

        #region 订单
        public ActionResult Search()
        {
            var payments = new Dictionary<int, string>();
            payments.Add((int)PaymentType.BankOfChina, "BankOfChina");
            payments.Add((int)PaymentType.Paypal, "Paypal");

            var searchVo = new OrderSearchVo()
            {
                Languages = ServiceFactory.ConfigureService.GetAllValidLanguage(),
                Shippings = ServiceFactory.ShippingService.GetAllShippings(),
                Payments = payments,
                OrderStatus = ServiceFactory.OrderService.GetAdminOrderStatus(ServiceFactory.ConfigureService.EnglishLangId),
            };
            var customerid = Request["customer"].ParseTo<int>(0);
            var customer = ServiceFactory.CustomerService.GetCustomerById(customerid);
            if (!customer.IsNullOrEmpty())
            {
                ViewBag.Customer = customer.Email;
            }
            return View(searchVo);
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            var orderno = Request["orderno"] ?? string.Empty;
            var customer = Request["customer"] ?? string.Empty;
            var shippingId = Request["shippingId"] ?? string.Empty;
            var paymentId = Request["paymentId"] ?? string.Empty;
            var orderStatus = Request["orderStatus"] ?? string.Empty;
            var orderSource = Request["orderSource"] ?? string.Empty;

            var searchCriteria = new Dictionary<OrderSearchCriteria, object>();
            if (!orderno.IsNullOrEmpty())
            {
                searchCriteria.Add(OrderSearchCriteria.OrderNo, orderno);
            }
            if (!customer.IsNullOrEmpty())
            {
                searchCriteria.Add(OrderSearchCriteria.Customer, customer);
                ViewBag.Customer = customer;
            }
            if (shippingId.ParseTo<int>(-1) != -1)
            {
                searchCriteria.Add(OrderSearchCriteria.ShippingMethod, shippingId);
            }
            if (paymentId.ParseTo<int>(-1) != -1)
            {
                searchCriteria.Add(OrderSearchCriteria.PaymentMethod, paymentId);
            }
            if (orderStatus.ParseTo<int>(-1) != -1)
            {
                searchCriteria.Add(OrderSearchCriteria.OrderStatus, orderStatus);
            }
            if (orderSource.ParseTo<int>(-1) != -1)
            {
                searchCriteria.Add(OrderSearchCriteria.OrderSource, orderSource);
            }
            var sorterCriteria = new List<Sorter<OrderSorterCriteria>>();
            var order = ServiceFactory.OrderService.GetAdminOrdersList(page, pageSize, searchCriteria, sorterCriteria);

            IList<Order> orderList = new List<Order>();
            foreach (var o in order.Data)
            {
                o.OrderCost = ServiceFactory.OrderService.GetOrderCostByOrderId‎(o.OrderId);
                orderList.Add(o);
            }
            order.Data = orderList;
            return View(order);
        }

        public ActionResult Detail()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            order.OrderCost = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            var orderDetail = ServiceFactory.OrderService.GetOrderDetailListByOrderId(order.OrderId);


            if (orderDetail.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var orderDetailVo = new OrderDetailVo();
            orderDetailVo.Order = order;
            orderDetailVo.OrderShippingAddress =
                ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);//获取货运地址

            orderDetailVo.DefaultShippingAddress = ServiceFactory.CustomerService.GetDefaultShippingAddress(order.CustomerId);//获取默认地址

            orderDetailVo.PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>());

            var productInfos = ServiceFactory.ProductService.GetProductInfos(orderDetail.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: true, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = orderDetail.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();

            orderDetailVo.OrderDetailList = items;

            orderDetailVo.OrderRemarkList = ServiceFactory.OrderService.GetOrderRemarks(order.OrderId);

            //订单状态List
            orderDetailVo.OrderStatusHistoryList =
                ServiceFactory.OrderService.GetOrderAdminStatusHistoryByOrderId‎(order.OrderId);
            var payments = new Dictionary<int, string>();
            payments.Add((int)PaymentType.BankOfChina, "BankOfChina");
            payments.Add((int)PaymentType.Paypal, "Paypal");

            var searchVo = new OrderSearchVo()
            {
                Languages = ServiceFactory.ConfigureService.GetAllValidLanguage(),
                Shippings = ServiceFactory.ShippingService.GetAllShippings(),
                Payments = payments,
                OrderStatus = ServiceFactory.OrderService.GetAdminOrderStatus(ServiceFactory.ConfigureService.EnglishLangId),
            };
            orderDetailVo.OrderSearchVo = searchVo;


            ViewBag.OrderStatus = ServiceFactory.OrderService.GetAdminOrderStatus(1);
            return View(orderDetailVo);
        }


        /// <summary>
        /// 删除订单显示页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDelete(int id)
        {
            var order = ServiceFactory.OrderService.GetOrderByOrderId(id);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            order.OrderCost = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);

            var customer = ServiceFactory.CustomerService.GetCustomerById(order.CustomerId);
            if (!customer.IsNullOrEmpty())
            {
                ViewBag.FullName = customer.FullName;
            }
            return View(order);
        }

        public ActionResult Delete()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var cId = Request["ids"].ParseTo(-1);//订单Id
            var selct = Request["ifadd"].ParseTo<bool>(false);
            try
            {
                if (selct)
                {
                    ServiceFactory.ProductService.UpdateProductStockByOrderId(cId);
                }
                ServiceFactory.OrderService.DeleteOrder(cId, 1);
                hashtable.Add("getlist", true);
                hashtable.Add("url", "/order/GetList");
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }

        public ActionResult UpdateBusinessDiscount()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var orderno = Request["HID_orderNo"] ?? string.Empty;
            var amount = Request["discountamount"] ?? string.Empty;
            var remark = Request["discount_remark"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            order.OrderCost = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            try
            {
                ServiceFactory.OrderService.UpdateOrderBusinessDiscountAmount(order.OrderId, amount.ParseTo<decimal>(),
                    remark, 1);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }

        public ActionResult UpdateBusinessSurcharge()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var orderno = Request["HID_orderNo"] ?? string.Empty;
            var amount = Request["surchargeamount"] ?? string.Empty;
            var remark = Request["surcharge_remark"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            order.OrderCost = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);
            try
            {
                ServiceFactory.OrderService.UpdateOrderBusinessSurcharge(order.OrderId, amount.ParseTo<decimal>(),
                    remark, 1);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public ActionResult UpdatePaymentAmount()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var orderno = Request["HID_orderNo"] ?? string.Empty;
            var amount = Request["OrderpaymentAmount"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            try
            {
                ServiceFactory.OrderService.AddOrderPaymentAmountLog(order.OrderId, amount.ParseTo<decimal>(), 1);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public ActionResult Change()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var orderno = Request["HID_orderNo"] ?? string.Empty;
            var status = Request["FD_OrderStatus"] ?? string.Empty;
            var comment = Request["order_comment"] ?? string.Empty;
            var notifyCustomer = Request["FD_NotifyCustomer"].ParseTo<bool>(false);
            var notifyEmailWithComments = Request["FD_NotifyEmailWithComments"].ParseTo<bool>(false);
            var remark = Request["seller_memo"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            try
            {
                OrderStatusHistory os = new OrderStatusHistory()
                {
                    AdminId = AdminId,
                    ChangeDate = DateTime.Now,
                    Comments = comment,
                    NotifyCustomer = notifyCustomer,
                    NotifyEmailWithComments = notifyEmailWithComments,
                    OrderId = order.OrderId,
                    Status = status.ParseTo<int>(0),
                };
                ServiceFactory.OrderService.ChangeOrderStatus‎(os, remark);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public ActionResult OrderInvoice()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);

            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var orderDetail = ServiceFactory.OrderService.GetOrderDetailListByOrderId(order.OrderId);

            if (orderDetail.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var orderInvoiceVo = new OrderInvoiceVo();
            orderInvoiceVo.Order = order;
            orderInvoiceVo.OrderShippingAddress =
                ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);//获取货运地址
            orderInvoiceVo.PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>());
            orderInvoiceVo.ShippingName = CacheHelper.GetShippingName(order.ShippingId);//运送方式名称
            var productInfos = ServiceFactory.ProductService.GetProductInfos(orderDetail.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = orderDetail.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();
            orderInvoiceVo.OrderDetailList = items;
            return View(orderInvoiceVo);
        }

        public ActionResult PaymentAmountLog(int id)
        {
            var model = ServiceFactory.OrderService.GetOrderPaymentAmountLogList(id);
            return View(model);
        }

        /// <summary>
        /// 查看线下支付信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewInfo()
        {
            var orderId = Request["id"].ParseTo<int>(0);
            if (orderId < 1)
            {
                return View("ViewInfo/Default", null);
            }
            var order = ServiceFactory.OrderService.GetOrderByOrderId(orderId);
            if (order != null)
            {
                switch (order.PaymentMethod.ToEnum<PaymentType>())
                {
                    case PaymentType.Paypal:
                        break;
                    case PaymentType.Webmoney:
                        break;
                    case PaymentType.Yandex:
                        break;
                    case PaymentType.QiWi:
                        break;
                    case PaymentType.OceanCreditCard:
                        break;
                    case PaymentType.Gc:
                        break;
                    case PaymentType.Hsbc:
                        var hsbcinfos = ServiceFactory.PaymentService.GetHsbcInfo(order.OrderNo);
                        return View("ViewInfo/HSBCInfo", hsbcinfos);
                    case PaymentType.BankOfChina:
                        var bankOfChinaInfos = ServiceFactory.PaymentService.GetBankOfChinaInfo(order.OrderNo);
                        return View("ViewInfo/BankOfChinaInfo", bankOfChinaInfos);
                    case PaymentType.WesternUnion:
                        var westernUnionInfos = ServiceFactory.PaymentService.GetWesternUnionInfo(order.OrderNo);
                        return View("ViewInfo/WesternUnionInfo", westernUnionInfos);
                    case PaymentType.MoneyGram:
                        var moneyGramInfos = ServiceFactory.PaymentService.GetMoneyGramInfo(order.OrderNo);
                        return View("ViewInfo/MoneyGramInfo", moneyGramInfos);
                    default:
                        return View("ViewInfo/Default", null);
                }
            }
            return View("ViewInfo/Default", null);
        }

        /// <summary>
        /// 获取支付方式对应名称
        /// </summary>
        /// <param name="payment">支付类型</param>
        /// <returns>对应名称</returns>
        public static string GetPaymentName(PaymentType payment)
        {
            switch (payment)
            {
                case PaymentType.Paypal:
                    return "paypalmanually或paypalwpp";

                case PaymentType.Hsbc:
                    return "Hsbc";

                case PaymentType.BankOfChina:
                    return "BankOfChina";

                case PaymentType.WesternUnion://西联汇款
                    return "westernunion";

                case PaymentType.Gc://GC信用卡
                    return "gcCreditCard";

                case PaymentType.MoneyGram://MoneyGram汇款
                    return "moneygram";

                case PaymentType.Webmoney://Webmoney支付
                    return "webmoney";

                case PaymentType.Yandex://Yandex支付
                    return "Yandex";

                case PaymentType.QiWi://QiWi支付
                    return "QIWI";

                case PaymentType.OceanCreditCard://钱海信用卡支付
                    return "OceanCreditCard";

                case PaymentType.Cash://Cash全额支付
                    return "Cash";

                default:
                    return "找不到对应";

            }
        } 
        #endregion

        #region 特殊订单

        public ActionResult SpecialOrder()
        {
           int page = Request["page"].ParseTo(1);
           int pageSize = Request["pageSize"].ParseTo(20);
           var model= ServiceFactory.OrderService.GetSpecialOrder(page, pageSize, null, null);
           return View(model);
        }

        public ActionResult SpecialOrderDelete()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var cId = Request["ids"].ParseTo(-1);//Id
            try
            {
                ServiceFactory.OrderService.DeleteSpecialOrder(cId);
                hashtable.Add("getlist", true);
                hashtable.Add("url", "/Order/SpecialOrder");
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public ActionResult AddSpecialOrder()
        {
            return View(ServiceFactory.ConfigureService.GetAllCurrencies());
        }


        [HttpPost]
        public JsonResult Add()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", string.Empty);
            hashtable.Add("error", false);
            try
            {
                var strCustomerEmail = Request["txtCustomerEmail"] as string;
                var strPrice = Request["txtPrice"].ParseTo<int>(0);
                var strCurrencyCode = Request["txtCurrencyCode"] as string;
                var notifyCustomer = Request["if_notify_customer"].ParseTo<bool>(false);
                var strRemark = Request["txtRemark"]??string.Empty;
                if (strPrice <= 0)
                {
                    hashtable["msg"] = "增加金额限制只能输入正数";
                    hashtable["error"] = true;
                }
                else
                {
                    if (!strCustomerEmail.IsNullOrEmpty())
                    {
                        var order = new SpecialOrder()
                        {
                            CurrencyCode = strCurrencyCode,
                            CustomerMail = strCustomerEmail,
                            Increase=strPrice,
                            IsNotifyCustomer = notifyCustomer,
                            Remark = strRemark,
                            Creator = AdminId,
                        };
                        ServiceFactory.OrderService.AddSpecialOrder(order);
                        hashtable["getlist"] = true;
                        hashtable["url"] = "/Order/SpecialOrder";
                    }
                    else
                    {
                        hashtable["msg"] = "请输入客户邮箱";
                        hashtable["error"] = true;
                    }
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_CUSTOMER_EMAIL_NOT_EXIST":
                        hashtable["msg"] = "系统中找不到该客户，请确认";
                        hashtable["error"] = true;
                        break;
                }
            }

            return Json(hashtable);
        }



        public ActionResult ViewSpecialOrder(int id)
        {
           var model= ServiceFactory.OrderService.GetSpecialOrder(id);
            if (!model.IsNullOrEmpty())
            {
                var admin=  ServiceFactory.AdminUserService.GetAdminUser(model.Creator);
                if (!admin.IsNullOrEmpty())
                {
                    model.CreateAccount = admin.AccountEmail;
                }
                var customer = ServiceFactory.CustomerService.GetCustomerById(model.CustomerId);
                if (!customer.IsNullOrEmpty())
                {
                    model.CustomerMail = customer.Email;
                }
            }
            return View(model);
        }

        #endregion
    }
}
