using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Entity.Order;
using Com.Panduo.Entity.Payment;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Com.Panduo.ServiceImpl.Order.Dao;
using Com.Panduo.ServiceImpl.Payment.Dao;
using ImageLibrary;

namespace Com.Panduo.ServiceImpl.Order
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// 用于图片下载
        /// </summary>
        private static WebClientHelper _webClientHelper = new WebClientHelper { Timeout = ServiceConfig.RequestTimeout };

        public IOrderDao OrderDao { private get; set; }
        public IOrderDetailDao OrderDetailDao { private get; set; }
        public IOrderStatusDescriptionDao OrderStatusDescriptionDao { private get; set; }
        public IOrderPayStatusDescriptionDao OrderPayStatusDescriptionDao { private get; set; }
        public IOrderPriceDao OrderPriceDao { private get; set; }
        public IOrderSnapshotDao OrderSnapshotDao { private get; set; }
        public IPackageDao PackageDao { private get; set; }
        public IPackageDetailDao PackageDetailDao { private get; set; }
        public IOrderStatusHistoryDao OrderStatusHistoryDao { private get; set; }
        public IOrderAddressDao OrderAddressDao { private get; set; }
        public IOrderStatusDao OrderStatusDao { private get; set; }
        public IOrderPaymentAmountLogDao OrderPaymentAmountLogDao { private get; set; }
        public IOrderRemarkByAdminDao OrderRemarkByAdminDao { private get; set; }
        public ISpecialOrderDao SpecialOrderDao { private get; set; }
        #region IOC

        public IOrderPaymentLogDao OrderPaymentLogDao { private get; set; }
        public IPaymentLogBankOfChinaDao PaymentLogBankOfChinaDao { private get; set; }
        public IPaymentLogGcDao PaymentLogGcDao { private get; set; }
        public IPaymentLogHsbcDao PaymentLogHsbcDao { private get; set; }
        public IPaymentLogMoneyGramDao PaymentLogMoneyGramDao { private get; set; }
        public IPaymentLogOceanPaymentDao PaymentLogOceanPaymentDao { private get; set; }
        public IPaymentLogPaypalDao PaymentLogPaypalDao { private get; set; }
        public IPaymentLogWesternUnionDao PaymentLogWesternUnionDao { private get; set; }

        public ICashService CashService { private get; set; }
        public IConfigureService ConfigureService { private get; set; }
        public ICustomerService CustomerService { private get; set; }
        public IPaymentService PaymentService { private get; set; }
        #endregion

        public string ERROR_ADDRESS_HAVA_NO_CHOICE { get; private set; }
        public string ERROR_SHIPPING_MOTHOD_HAVA_NO_CHOICE { get; private set; }
        public string ERROR_CUSTOMERS_NO_IS_NULL { get; private set; }
        public string ERROR_TARIFF_IS_NULL { get; private set; }
        public string ERROR_ORDER_REMARK_TOO_LONG { get; private set; }
        public string ERROR_COUPON_IS_DISABLED { get; private set; }
        public string ERROR_NEW_PRODUCT_HAS_BEEN_NOT_ON_SALE { get; private set; }
        public string ERROR_PRODUCT_NOT_EXIST { get; private set; }

        public string ERROR_TO_SUBMIT
        {
            get { return "ERROR_TO_SUBMIT"; }
        }

        public string ERROR_ORDER_NOT_EXIST
        {
            get { return "ERROR_ORDER_NOT_EXIST"; }
        }

        public string ERROR_ORDER_NOT_UPDATE
        {
            get { return "ERROR_ORDER_NOT_UPDATE"; }
        }

        public string ERROR_ORDER_STATUS_WRONG
        {
            get { return "ERROR_ORDER_STATUS_WRONG"; }
        }

        public string ERROR_ORDER_AMOUNT_WRONG
        {
            get { return "ERROR_ORDER_AMOUNT_WRONG"; }
        }

        public string ERROR_ORDER_PAYMENT_STATUS_WRONG
        {
            get { return "ERROR_ORDER_PAYMENT_STATUS_WRONG"; }
        }

        public string ERROR_PAYMENT_TYPE_GC_TYPE_EMPTY
        {
            get { return "ERROR_PAYMENT_TYPE_GC_TYPE_EMPTY"; }
        }

        public string ERROR_PAYMENT_NOT_ENOUGH_TO_PAY_BY_CASH
        {
            get { return "ERROR_PAYMENT_NOT_ENOUGH_TO_PAY_BY_CASH"; }
        }

        public string ERROR_PAYMENT_CASH_DEBT
        {
            get { return "ERROR_PAYMENT_CASH_DEBT"; }
        }


        public string ERROR_PAYMENT_ORDER_NOT_SAME_AS_PAYPAL
        {
            get { return "ERROR_PAYMENT_ORDER_NOT_SAME_AS_PAYPAL"; }
        }

        public string ERROR_PAYMENT_PAY_CURRENCY_ERROR
        {
            get { return "ERROR_PAYMENT_PAY_CURRENCY_ERROR"; }
        }

        public string ERROR_PAYMENT_PAY_AMOUNT_ERROR
        {
            get { return "ERROR_PAYMENT_PAY_AMOUNT_ERROR"; }
        }

        public string ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR
        {
            get { return "ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR"; }
        }

        public string ERROR_PAYMENT_PAYPAL_DUPLICATE
        {
            get { return "ERROR_PAYMENT_PAYPAL_DUPLICATE"; }
        }

        public string ERROR_PAYMENT_ORDER_NOT_SAME_AS_GC
        {
            get { return "ERROR_PAYMENT_ORDER_NOT_SAME_AS_GC"; }
        }

        public string ERROR_PAYMENT_CREDITCARD_TYPE_ERROR
        {
            get { return "ERROR_PAYMENT_CREDITCARD_TYPE_ERROR"; }
        }

        public string ERROR_PAYMENT_CREDITCARD_STATUS_ERROR
        {
            get { return "ERROR_PAYMENT_CREDITCARD_STATUS_ERROR"; }
        }

        public string ERROR_PAYMENT_CREDITCARD_DUPLICATE
        {
            get { return "ERROR_PAYMENT_CREDITCARD_DUPLICATE"; }
        }

        public string ERROR_PAYMENT_ORDER_NOT_SAME_AS_OCEANPAYMENT
        {
            get { return "ERROR_PAYMENT_ORDER_NOT_SAME_AS_OCEANPAYMENT"; }
        }

        public string ERROR_PAYMENT_OCEANPAYMENT_DUPLICATE
        {
            get { return "ERROR_PAYMENT_OCEANPAYMENT_DUPLICATE"; }
        }

        public string ERROR_CUSTOMER_EMAIL_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_EMAIL_NOT_EXIST"; }
        }

        public Service.Order.Order GetCustomerLatestOrder(int customerId, Sorter<LatestOrderSorterCriteria> critria)
        {
            var po = OrderDao.GetCustomerLatestOrder(customerId, critria);
            return GetOrderVoFromPo(po);
        }

        public PageData<Service.Order.Order> FindOrders(int currentPage, int pageSize,
            IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria)
        {
            return OrderDao.FindOrders(currentPage, pageSize, searchCriteria, sorterCriteria);
        }

        public OrderCost GetOrderCostByOrderId‎(int orderId)
        {
            var po = OrderPriceDao.GetOrderCostByOrderId‎(orderId);
            return GetOrderCostVoFromPo(po);
        }

        public PageData<OrderDetail> GetOrderDetsById(int customerId, int orderId, int currentPage, int pageSize, IDictionary<OrderDetailSearchCriteria, object> searchCriteria)
        {
            var pageData = new PageData<OrderDetail>();
            var dataList = new List<OrderDetail>();
            if (OrderDao.IsCustomerOrder(customerId, orderId))
            {
                var p = OrderDetailDao.GetOrderDetsById(orderId, currentPage, pageSize, searchCriteria);
                foreach (var x in p.Data)
                {
                    dataList.Add(GetOrderDetailVoFromPo(x));
                }
                pageData.Pager = p.Pager;
                pageData.Data = dataList;
                return pageData;
            }
            pageData.Data = dataList;
            pageData.Pager = new Pager(0, currentPage, pageSize);
            return pageData;
        }

        public IList<OrderDetail> GetOrderDetailListByOrderId(int orderId)
        {
            var dataList = new List<OrderDetail>();
            var p = OrderDetailDao.GetOrderDetsById(orderId, 1, 100000, null);
            foreach (var x in p.Data)
            {
                dataList.Add(GetOrderDetailVoFromPo(x));
            }
            return dataList;
        }

        public OrderDetail GetOrderDetsById(int customerId, int orderDetsId)
        {
            var po = OrderDetailDao.GetObject(orderDetsId);

            if (!po.IsNullOrEmpty())
            {
                if (OrderDao.IsCustomerOrder(customerId, po.OrderId))
                {
                    return GetOrderDetailVoFromPo(po);
                }
            }
            return null;
        }

        public IDictionary<int, int> GetOrderItemsCountByOrderId(int orderId)
        {
            return OrderDetailDao.GetOrderDetsCountByOrderId(orderId);
        }

        public string RequestDownloadImage(string imageName)
        {
            return string.Format(ServiceConfig.ImageUrl, 0, imageName);
        }

        public string RequestDownloadImageBatch(int orderId)
        {
            var batchFileUrl = string.Empty;

            var url = string.Format(ServiceConfig.ImageBatchUrl, 0, orderId);

            try
            {
                var fileName = _webClientHelper.DownloadString(url);

                if (!fileName.IsNullOrEmpty())
                {
                    batchFileUrl = string.Format(ServiceConfig.ImageUrl, 1, fileName);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.GetLogger(LoggerType.Exception).Error(string.Format("RequestDownloadImageBatch-异常-{0}", exception.Message), exception);
            }
            return batchFileUrl;
        }

        public void UpdateOrderItemStatusById(string orderItemId, int orderItemStatus)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderItemIsReviewedById(int orderItemId, bool isReviewed)
        {
            var po = OrderDetailDao.GetObject(orderItemId);
            po.IsReviewed = isReviewed;

            OrderDetailDao.UpdateObject(po);
        }

        public IList<OrderStatusHistory> GetOrderStatusHistoryByOrderId‎(int orderId)
        {
            return OrderStatusHistoryDao.GetOrderStatusHistoryByOrderId‎(orderId,
                                                                         ServiceFactory.ConfigureService.SiteLanguageId);
        }

        public OrderBillingAddress GetOrderBillingAddressById‎(int orderBillingAddressId)
        {
            var address = OrderAddressDao.GetObject(orderBillingAddressId);
            if (!address.IsNullOrEmpty())
            {
                if ((int)AddressType.Billing == address.AddressType)
                {
                    return GetOrderBillingAddressVoFromPo(address);
                }
                return null;
            }
            return null;
        }

        public OrderBillingAddress GetOrderBillingAddressByOrderId‎(int orderId)
        {
            var address = OrderAddressDao.GetOrderAddressByOrderId(orderId, (int)AddressType.Billing);
            if (address == null)
            {
                //老版本没有账单地址，需要将货运地址作为账单地址
                address = OrderAddressDao.GetOrderAddressByOrderId(orderId, (int)AddressType.Freight);
            }

            return GetOrderBillingAddressVoFromPo(address);
        }

        public OrderShippingAddress GetOrderShippingAddressById‎(int orderShippingAddressId)
        {
            var address = OrderAddressDao.GetObject(orderShippingAddressId);
            if (!address.IsNullOrEmpty())
            {
                if ((int)AddressType.Freight == address.AddressType)
                {
                    return GetOrderShippingAddressVoFromPo(address);
                }
                return null;
            }
            return null;
        }

        public OrderShippingAddress GetOrderShippingAddressByOrderId‎(int orderId)
        {
            var address = OrderAddressDao.GetOrderAddressByOrderId(orderId, (int)AddressType.Freight);
            return GetOrderShippingAddressVoFromPo(address);
        }

        public IList<Package> GetOrderPackageByOrderId(int orderId)
        {
            IList<Package> list = new List<Package>();
            var polist = PackageDao.GetOrderPackageByOrderId(orderId);
            foreach (var orderPackagePo in polist)
            {
                list.Add(GetPackageVoFromPo(orderPackagePo));
            }
            return list;
        }

        public PageData<PackageDetail> FindOrderPackages(int currentPage, int pageSize,
            IDictionary<PackageSearchCriteria, object> searchCriteria,
            IList<Sorter<PackageSorterCriteria>> sorterCriteria)
        {
            return PackageDetailDao.FindOrderPackages(currentPage, pageSize, searchCriteria, sorterCriteria);
        }

        public OrderImpression GetOrderImpressionByOrderId‎(int orderDetId)
        {
            OrderImpression orderImpression = null;
            var po = OrderSnapshotDao.GetOrderSnapshot(orderDetId);

            if (!po.IsNullOrEmpty())
            {
                orderImpression = new OrderImpression
                    {
                        Id = po.OrderDetailId,
                        OrderItemId = po.OrderProductId,
                        ProductName = po.ProductName,
                        CategoryId = po.CategoryId,
                        CategoryName = po.CategoryName,
                        ProductPropertyStr =
                        po.KeyValue.IsNullOrEmpty()
                            ? new Dictionary<string, string>()
                            : po.KeyValue.FromJson<IDictionary<string, string>>(),
                        Description = po.Description,
                        LastModifyTime = po.DateModified,
                        ProductImages =
                        po.Images.IsNullOrEmpty() ? new List<string>() : po.Images.Split(new char[] { ',', '，' }).ToList()
                    };
            }
            return orderImpression;
        }

        public void UpdateOrderForSale(Service.Order.Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderStatusForSale(string orderId, string status)
        {
            throw new NotImplementedException();
        }

        public void SignOrderStatusForSale(string orderId, string shippingId, string trackingNumber,
            DateTime shippedDate)
        {
            throw new NotImplementedException();
        }

        public void CheckOrderStatusForSale(string orderId, bool isPass)
        {
            throw new NotImplementedException();
        }

        public PageData<Service.Order.Order> GetAdminOrdersList(int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria)
        {
            return OrderDao.GetAdminOrdersList(currentPage, pageSize, searchCriteria, sorterCriteria);
        }

        public void DeleteOrder(int orderId, int adminId)
        {
            var orderpo = OrderDao.GetObject(orderId);
            if (!orderpo.IsNullOrEmpty())
            {
                orderpo.OrderStatus = (int)OrderStatusType.Deleted;
                OrderDao.AddObject(orderpo);
            }
        }


        public IList<OrderPaymentAmountLog> GetOrderPaymentAmountLogList(int orderId)
        {
            IList<OrderPaymentAmountLog> list = new List<OrderPaymentAmountLog>();
            var polist = OrderPaymentAmountLogDao.GetOrderPaymentAmountLogList(orderId);
            foreach (var orderPaymentAmountLog in polist)
            {
                list.Add(GetOrderPaymentAmountLogVoFromPo(orderPaymentAmountLog));
            }
            return list;
        }

        public void AddOrderPaymentAmountLog(int orderId, decimal paymentAmount, int adminId)
        {
            var cost = OrderPriceDao.GetOrderCostByOrderId‎(orderId);
            if (!cost.IsNullOrEmpty() && paymentAmount > 0)
            {
                var po = new OrderPaymentAmountLogPo
                {
                    OrderId = orderId,
                    NewAmount = paymentAmount,
                    AdminId = adminId,
                    OriginalAmount = cost.PaymentAmount.HasValue ? cost.PaymentAmount.Value : 0.0M,
                    DateCreated = DateTime.Now,
                };
                int id = OrderPaymentAmountLogDao.AddObject(po);
                if (id > 0)
                {
                    cost.PaymentAmount = paymentAmount;
                    OrderPriceDao.UpdateObject(cost);
                }
            }
        }

        public void UpdateOrderBusinessDiscountAmount(int orderId, decimal amount, string remark, int adminId)
        {
            var cost = OrderPriceDao.GetOrderCostByOrderId‎(orderId);
            if (!cost.IsNullOrEmpty() && amount > 0)
            {
                cost.BusinessDerateAmount = amount;
                OrderPriceDao.UpdateObject(cost);
                var orderremark = OrderRemarkByAdminDao.GetOrderRemark(orderId, (int)OrderRemarkType.BusinessDiscount);
                if (!orderremark.IsNullOrEmpty())
                {
                    orderremark.AdminId = adminId;
                    orderremark.RemarkContent = remark;
                    OrderRemarkByAdminDao.UpdateObject(orderremark);
                }
                else
                {
                    OrderRemarkByAdminDao.AddObject(new OrderRemarkByAdminPo()
                    {
                        AdminId = adminId,
                        OrderId = orderId,
                        RemarkContent = remark,
                        RemarkType = (int)OrderRemarkType.BusinessDiscount,
                        DateCreated = DateTime.Now,
                    });
                }
            }
        }

        public void UpdateOrderBusinessSurcharge(int orderId, decimal amount, string remark, int adminId)
        {
            var cost = OrderPriceDao.GetOrderCostByOrderId‎(orderId);
            if (!cost.IsNullOrEmpty() && amount > 0)
            {
                cost.BusinessAddedAmount = amount;
                OrderPriceDao.UpdateObject(cost);
                var orderremark = OrderRemarkByAdminDao.GetOrderRemark(orderId, (int)OrderRemarkType.BusinessSurcharge);
                if (!orderremark.IsNullOrEmpty())
                {
                    orderremark.AdminId = adminId;
                    orderremark.RemarkContent = remark;
                    OrderRemarkByAdminDao.UpdateObject(orderremark);
                }
                else
                {
                    OrderRemarkByAdminDao.AddObject(new OrderRemarkByAdminPo()
                    {
                        AdminId = adminId,
                        OrderId = orderId,
                        RemarkContent = remark,
                        RemarkType = (int)OrderRemarkType.BusinessSurcharge,
                        DateCreated = DateTime.Now,
                    });
                }
            }
        }

        public IList<OrderRemark> GetOrderRemarks(int orderId)
        {
            IList<OrderRemark> list = new List<OrderRemark>();
            var polist = OrderRemarkByAdminDao.GetOrderRemarkList(orderId);
            foreach (var po in polist)
            {
                list.Add(GetOrderRemarkVoFromPo(po));
            }
            return list;
        }

        public IList<OrderStatusHistory> GetOrderAdminStatusHistoryByOrderId‎(int orderId)
        {
            return OrderStatusHistoryDao.GetOrderAdminStatusHistoryByOrderId‎(orderId);
        }

        public void ChangeOrderStatus‎(OrderStatusHistory orderStatusHistory,string remark)
        {
            if (!orderStatusHistory.IsNullOrEmpty())
            {
                OrderStatusHistoryPo po = new OrderStatusHistoryPo()
                {
                    ChangeDate=orderStatusHistory.ChangeDate,
                    Comments=orderStatusHistory.Comments,
                    NotifyCustomer=orderStatusHistory.NotifyCustomer,
                    NotifyEmailWithComments=orderStatusHistory.NotifyEmailWithComments,
                    Status=orderStatusHistory.Status,
                    OrderId=orderStatusHistory.OrderId,
                };
                OrderStatusHistoryDao.AddObject(po);

                var orderremark = OrderRemarkByAdminDao.GetOrderRemark(orderStatusHistory.OrderId, (int)OrderRemarkType.SellerMemo);
                if (!orderremark.IsNullOrEmpty())
                {
                    orderremark.AdminId = 1;
                    orderremark.RemarkContent = remark;
                    OrderRemarkByAdminDao.UpdateObject(orderremark);
                }
                else
                {
                    OrderRemarkByAdminDao.AddObject(new OrderRemarkByAdminPo()
                    {
                        AdminId = 1,
                        OrderId = orderStatusHistory.OrderId,
                        RemarkContent = remark,
                        RemarkType = (int)OrderRemarkType.SellerMemo,
                        DateCreated = DateTime.Now,
                    });
                }
            }
        }

        /// <summary>
        /// 根据语种Id获取所有的订单状态(后台) add by luohaiming 2015-04-17
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public IList<OrderStatus> GetAdminOrderStatus(int languageId)
        {
            var list = new List<OrderStatus>();
            var orderstatus = OrderStatusDescriptionDao.GetAdminOrderStatusDesc(languageId);
            foreach (var orderStatusDescriptionPo in orderstatus)
            {
                list.Add(GetOrderStatusVoFromPo(orderStatusDescriptionPo));
            }
            return list;
        }

        public PageData<SpecialOrder> GetSpecialOrder(int currentPage, int pageSize, IDictionary<SpecialOrderSearchCriteria, object> searchCriteria, IList<Sorter<SpecialOrderSorterCriteria>> sorterCriteria)
        {
            return SpecialOrderDao.GetSpecialOrder(currentPage, pageSize, searchCriteria, sorterCriteria);
        }

        public PageData<Service.Order.Order> GetOrdersList(int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public void DeleteSpecialOrder(int specialId)
        {
            var po = SpecialOrderDao.GetObject(specialId);
            if (!po.IsNullOrEmpty())
            {
                po.Status = -1;
                SpecialOrderDao.UpdateObject(po);
            }
        }

        public void AddSpecialOrder(SpecialOrder special)
        {
            if (!special.IsNullOrEmpty())
            {
                var c = ServiceFactory.CustomerService.GetCustomerByEmail(special.CustomerMail);
                if (!c.IsNullOrEmpty())
                {
                    var specialOrder = new SpecialOrderPo()
                    {
                        AdminId = special.Creator,
                        CustomerId = c.CustomerId,
                        IncreaseMoney = special.Increase,
                        IsNotifyCustomer = special.IsNotifyCustomer,
                        CurrencyCode = special.CurrencyCode,
                        Remark = special.Remark,
                        Status = 1,
                        DateCreated = DateTime.Now,
                    };
                    SpecialOrderDao.AddObject(specialOrder);
                }
                else
                {
                    throw new BussinessException(ERROR_CUSTOMER_EMAIL_NOT_EXIST);
                }  
            }
        }

        public SpecialOrder GetSpecialOrder(int specialId)
        {
            var po = SpecialOrderDao.GetObject(specialId);
            return GeSpecialOrderVoFromPo(po);
        }

        public string PlaceOrderByCustomer(CheckoutDraft checkoutDraft)
        {
            var parmsList = new List<SqlParameter>
                            {
                new SqlParameter("@shopping_cart_id", SqlDbType.Int) {Value = checkoutDraft.ShoppingCartId},
                new SqlParameter("@club_level", SqlDbType.Int) {Value = checkoutDraft.ClubLevel},
                new SqlParameter("@receiving_address_id", SqlDbType.Int) {Value = checkoutDraft.ReceivingAddressId},
                new SqlParameter("@bill_address_id", SqlDbType.Int) {Value = checkoutDraft.BillAddressId},
                new SqlParameter("@shipping_id", SqlDbType.Int) {Value = checkoutDraft.ShippingId},
                new SqlParameter("@order_source", SqlDbType.Int) {Value = checkoutDraft.OrderSource},
                new SqlParameter("@language_code", SqlDbType.Char) {Value = checkoutDraft.LanguageCode},
                new SqlParameter("@currency_code", SqlDbType.Char) {Value = checkoutDraft.CurrencyCode},
                new SqlParameter("@report_type", SqlDbType.Char) {Value = checkoutDraft.ReportType},
                new SqlParameter("@report_currency_code", SqlDbType.Char) {Value = checkoutDraft.ReportCurrencyCode},
                new SqlParameter("@report_product_money", SqlDbType.Decimal) {Value = checkoutDraft.ReportProductMoney},
                new SqlParameter("@report_shipping_money", SqlDbType.Decimal)
                {
                    Value = checkoutDraft.ReportShippingMoney
                },
                new SqlParameter("@customs_no_type", SqlDbType.Int) {Value = (int) checkoutDraft.CustomsNoType},
                new SqlParameter("@customs_no_number", SqlDbType.NVarChar) {Value = checkoutDraft.CustomsNoNumber},
                new SqlParameter("@order_remark", SqlDbType.NVarChar) {Value = checkoutDraft.OrderRemark},
                new SqlParameter("@out_of_stock_wait_type", SqlDbType.Int)
                {
                    Value = (int) checkoutDraft.OutOfStockWaitType
                },
                new SqlParameter("@coupon_customer_id", SqlDbType.Int) {Value = checkoutDraft.CouponCustomerId},
                
                new SqlParameter("@order_time", SqlDbType.DateTime) {Value = DateTime.Now},
                new SqlParameter("@order_ip_address", SqlDbType.VarChar) {Value = checkoutDraft.OrderIpAddress},
                new SqlParameter("@completed_time", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@is_completed", SqlDbType.Bit) {Value = false}
                            };
            string cmdText =
                "insert into t_checkout_draft (shopping_cart_id, club_level, receiving_address_id, bill_address_id, shipping_id, order_source, language_code, currency_code, report_type, report_currency_code, report_product_money, report_shipping_money, customs_no_type, customs_no_number, order_remark, out_of_stock_wait_type, coupon_customer_id, order_time, order_ip_address, completed_time, is_completed) values (@shopping_cart_id, @club_level, @receiving_address_id, @bill_address_id, @shipping_id, @order_source, @language_code, @currency_code, @report_type, @report_currency_code, @report_product_money, @report_shipping_money, @customs_no_type, @customs_no_number, @order_remark, @out_of_stock_wait_type, @coupon_customer_id, @order_time, @order_ip_address, @completed_time, @is_completed);select SCOPE_IDENTITY()";

            string orderNumber = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(SqlHelper.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        var result = SqlHelper.ExecuteScalar(transaction, CommandType.Text, cmdText, parmsList.ToArray());
                        int affected = Convert.ToInt32(result);

                        //触发生成订单
                        var parmsPlaceOrderList = new List<SqlParameter>  {
                            new SqlParameter("@DraftId", SqlDbType.Int){Value =  affected}
                        };
                        using (
                            var reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "up_place_order",
                                parmsPlaceOrderList.ToArray()))
                        {
                            try
                            {
                                reader.Read();
                                orderNumber = reader.GetString(0);
                                reader.Close();
                                transaction.Commit();
                            }
                            catch (SqlException ex)
                            {
                                transaction.Rollback();
                                var number = ex.Number;
                                if (number == 60000)
                                {
                                    throw new BussinessException(ex.Message);
                                }
                            }
                            finally
                            {
                                reader.Close();
                                conn.Close();
                                transaction.Dispose();
                                conn.Dispose();
                            }
                        }
                    }

                }

            }
            catch (SqlException ex)
            {
                var number = ex.Number;
                if (number == 60000)
                {
                    throw new BussinessException(ex.Message);
                }
            }

            return orderNumber;
        }

        public void CustomerCancelOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public void CustomerConfirmOrderPackageReceived(string orderId)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderIsReservationById(int orderId, bool isReviewAll)
        {
            var po = OrderDao.GetObject(orderId);
            po.IsReviewAll = isReviewAll;

            OrderDao.UpdateObject(po);
        }

        #region 支付相关

        public void CustomerChangePaymentType(string orderNo, PaymentType paymentType,
            Service.Payment.PayConfig.GlobalCollectType? gcType)
        {
            //1.数据验证 
            //1.1.GC支付必须选择信用卡类型
            if (paymentType == PaymentType.Gc && gcType == null)
            {
                throw new BussinessException(ERROR_PAYMENT_TYPE_GC_TYPE_EMPTY);
            }

            //1.2.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //2.保存数据
            var po = OrderDao.GetObject(order.OrderId);
            po.PaymentId = (int)paymentType;
            if (paymentType == PaymentType.Gc)
            {
                po.CollectType = (int?)gcType;
            }
            else
            {
                po.CollectType = null;
            }

            OrderDao.UpdateObject(po);
        }

        public void CustomerPayOrderByCash(string orderNo, bool isFullPay)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.Cash余额判断 
            //1.2.1.存在欠款不能Cash支付
            var debtCashUsd = CashService.GetCustomerArrear(order.CustomerId);
            if (debtCashUsd > 0)
            {
                throw new BussinessException(ERROR_PAYMENT_CASH_DEBT);
            }

            //1.2.2.Cash为0或者Cash不足全额支付
            var cashBalanceUsd = CashService.GetCustomerBalance(order.CustomerId);
            if (cashBalanceUsd <= 0 || (isFullPay && cashBalanceUsd < orderCost.NeedToPayAmt))
            {
                throw new BussinessException(ERROR_PAYMENT_NOT_ENOUGH_TO_PAY_BY_CASH);
            }

            //2.保存数据
            var cashUseAmounUsd = isFullPay ? orderCost.NeedToPayAmt : (cashBalanceUsd > orderCost.NeedToPayAmt ? orderCost.NeedToPayAmt : cashBalanceUsd);
            var cashUseAmount = cashUseAmounUsd;
            if (!string.Equals(ConfigureService.CURRENCY_CODE_USD, order.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                var currency = ConfigureService.GetCurrencyByCode(order.Currency);
                cashUseAmount = ImplToolHelper.GetRoundValue(cashUseAmount * currency.ExchangeRate, currency.DecimalPlaces);
            }

            //2.1.调用Cash接口完成支付
            var logId = CashService.CashPay(order.CustomerId, order.OrderId, order.Currency, cashUseAmount);

            //2.2.更新订单支付信息
            var orderCostPo = OrderPriceDao.GetOrderCostByOrderId‎(order.OrderId);
            orderCostPo.UseCash = (orderCostPo.UseCash.HasValue ? orderCostPo.UseCash.Value : 0.00M) + cashUseAmounUsd;
            OrderPriceDao.UpdateObject(orderCostPo);

            //2.3.全额支付
            if (isFullPay)
            {
                //2.3.1.更新订单状态和支付状态
                var orderPo = OrderDao.GetObject(order.OrderId);

                orderPo.PaymentId = (int)PaymentType.Cash;
                orderPo.PayStatus = (int)PaidStatusType.FullPay;

                if (IsHighRiskCustomerOrder(order))
                {
                    orderPo.OrderStatus = (int)OrderStatusType.UnderChecking;
                }
                else
                {
                    orderPo.OrderStatus = (int)OrderStatusType.Processing;
                }

                OrderDao.UpdateObject(orderPo);

                //2.3.2记录支付日志
                AddOrderPaymentLog(order.OrderId, PaymentType.Cash, logId);

                if (orderPo.OrderStatus == (int)OrderStatusType.Processing)
                {
                    AddOrderStatusHistLogByCustomer(orderPo.OrderId, orderPo.OrderStatus, string.Empty);
                }
            }
        }

        public void CustomerPayOrderByPaypal(string orderNo, Service.Payment.PayInfo.PaypalInfo paypalInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.支付信息判断 
            //1.2订单号不一致
            if (!string.Equals(orderNo, paypalInfo.ItemNumber))
            {
                throw new BussinessException(ERROR_PAYMENT_ORDER_NOT_SAME_AS_PAYPAL);
            }

            //1.3币种验证
            if (string.IsNullOrEmpty(paypalInfo.McCurrency))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //是否只能用下单时的币种支付？
            var payCurrencyCode = PaymentService.IsCurrencyUseUsdForPaypal(order.Currency) ? ConfigureService.CURRENCY_CODE_USD : order.Currency;

            if (!string.Equals(paypalInfo.McCurrency, payCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            var currency = ConfigureService.GetCurrencyByCode(paypalInfo.McCurrency);
            if (currency == null)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //1.4支付金额错误
            var amount = paypalInfo.McGross;
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            var amountUsd = amount;
            if (!string.Equals(ConfigureService.CURRENCY_CODE_USD, payCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                //其他币种转换为美元(下单时币种的汇率)
                amountUsd = ImplToolHelper.GetRoundValue(decimal.Divide(amount, order.ExchangeRate), currency.DecimalPlaces);
            }

            if (amountUsd <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //1.5paypal支付状态错误
            if (!paypalInfo.IsCompleted)
            {
                throw new BussinessException(ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR);
            }

            //1.6paypal重复支付
            var logPo = PaymentLogPaypalDao.GetPaymentLogByTransactionId(paypalInfo.TxnId);
            if (logPo != null)
            {
                throw new BussinessException(ERROR_PAYMENT_PAYPAL_DUPLICATE);
            }

            //2.保存数据
            //2.1.保存Paypal支付信息
            logPo = new PaymentLogPaypalPo();
            ObjectHelper.CopyProperties(paypalInfo, logPo, new[] { "TargetId", "PaypalTargetType", "IsCompleted", "PAYPAL_STATUS_COMPLETED" });
            logPo.TargetId = order.OrderId;
            logPo.PaypalTargetType = (int)PaypalTargetType.Order;
            logPo.CreateDate = DateTime.Now;

            PaymentLogPaypalDao.AddObject(logPo);

            //2.2.有欠款的先还欠款
            var payCashDebtAmountUsd = 0.00M;//用来归还Cash的美元金额
            var payCashDebtAmount = 0.00M;//用来归还Cash的金额

            ReturnCustomerCashArrear(order, amountUsd, out payCashDebtAmountUsd, out payCashDebtAmount);

            //2.3.更新订单支付信息
            var orderCostPo = OrderPriceDao.GetOrderCostByOrderId‎(order.OrderId);
            orderCostPo.PaymentAmount = (orderCostPo.PaymentAmount.HasValue ? orderCostPo.PaymentAmount.Value : 0.00M) + (amountUsd - payCashDebtAmountUsd);

            OrderPriceDao.UpdateObject(orderCostPo);

            //2.4.更新订单状态和支付状态
            var orderCostNew = GetOrderCostVoFromPo(orderCostPo);
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)PaymentType.Paypal;

            if (orderCostNew.NeedToPayAmt > 0)
            {
                orderPo.PayStatus = (int)PaidStatusType.PartPay;
            }
            else
            {
                orderPo.PayStatus = (int)PaidStatusType.FullPay;
            }

            if (IsHighRiskCustomerOrder(order))
            {
                orderPo.OrderStatus = (int)OrderStatusType.UnderChecking;
            }
            else
            {
                orderPo.OrderStatus = (int)OrderStatusType.Processing;
            }

            OrderDao.UpdateObject(orderPo);

            //2.5记录支付日志
            AddOrderPaymentLog(order.OrderId, PaymentType.Paypal, logPo.Id);

            //2.6.订单装填变更日志
            if (orderPo.OrderStatus == (int)OrderStatusType.Processing)
            {
                AddOrderStatusHistLogByCustomer(orderPo.OrderId, orderPo.OrderStatus, string.Empty);
            }

            //2.6.todo 需要发送支付邮件？
        }

        public void CustomerPayOrderByGc(string orderNo, Service.Payment.PayInfo.GlobalCollectInfo gcInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.支付信息判断 
            //1.2订单号不一致
            if (!string.Equals(orderNo, gcInfo.OrderNo))
            {
                throw new BussinessException(ERROR_PAYMENT_ORDER_NOT_SAME_AS_GC);
            }

            //1.2信用卡类型错误
            if ((int)gcInfo.GlobalCollectType == 0)
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_TYPE_ERROR);
            }

            //1.3信用卡支付状态错误
            if (GlobalCollectInfo.GC_PAY_STATUS_CHALLENGED != gcInfo.StatusId && GlobalCollectInfo.GC_PAY_STATUS_READY != gcInfo.StatusId)
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_STATUS_ERROR);
            }

            //1.4订单号不一致
            if (!string.Equals(orderNo, gcInfo.OrderNo))
            {
                throw new BussinessException(ERROR_PAYMENT_ORDER_NOT_SAME_AS_GC);
            }

            //1.5币种验证
            if (string.IsNullOrEmpty(gcInfo.Currency))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //是否只能用下单时的币种支付？
            if (!string.Equals(gcInfo.Currency, order.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            var currency = ConfigureService.GetCurrencyByCode(gcInfo.Currency);
            if (currency == null)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //1.6支付金额错误 
            var amount = gcInfo.Amount;
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            var amountUsd = amount;
            if (!string.Equals(ConfigureService.CURRENCY_CODE_USD, order.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                //其他币种转换为美元(下单时币种的汇率)
                amountUsd = ImplToolHelper.GetRoundValue(decimal.Divide(amount, order.ExchangeRate), currency.DecimalPlaces);
            }

            if (amountUsd <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //1.7paypal重复支付
            var logPo = PaymentLogGcDao.GetPaymentLogByTransactionId(gcInfo.GcOrderId);
            if (logPo != null)
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_DUPLICATE);
            }

            //2.保存数据
            //2.1.保存Paypal支付信息
            logPo = new PaymentLogGcPo();
            ObjectHelper.CopyProperties(gcInfo, logPo, new[] { "GC_PAY_STATUS_CHALLENGED", "GC_PAY_STATUS_PENDING", "GC_PAY_STATUS_READY", "GlobalCollectType" });
            logPo.TargetId = order.OrderId;
            logPo.GlobalCollectType = (int)gcInfo.GlobalCollectType;
            logPo.CreateDate = DateTime.Now;

            PaymentLogGcDao.AddObject(logPo);

            //2.2.有欠款的先还欠款
            var payCashDebtAmountUsd = 0.00M;//用来归还Cash的美元金额
            var payCashDebtAmount = 0.00M;//用来归还Cash的金额

            ReturnCustomerCashArrear(order, amountUsd, out payCashDebtAmountUsd, out payCashDebtAmount);

            //2.3.更新订单支付信息
            var orderCostPo = OrderPriceDao.GetOrderCostByOrderId‎(order.OrderId);
            orderCostPo.PaymentAmount = (orderCostPo.PaymentAmount.HasValue ? orderCostPo.PaymentAmount.Value : 0.00M) + (amountUsd - payCashDebtAmountUsd);

            OrderPriceDao.UpdateObject(orderCostPo);

            //2.4.更新订单状态和支付状态
            var orderCostNew = GetOrderCostVoFromPo(orderCostPo);
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)PaymentType.Gc;
            orderPo.CollectType = (int)gcInfo.GlobalCollectType;

            if (orderCostNew.NeedToPayAmt > 0)
            {
                orderPo.PayStatus = (int)PaidStatusType.PartPay;
            }
            else
            {
                orderPo.PayStatus = (int)PaidStatusType.FullPay;
            }

            if (IsHighRiskCustomerOrder(order))
            {
                orderPo.OrderStatus = (int)OrderStatusType.UnderChecking;
            }
            else
            {
                orderPo.OrderStatus = (int)OrderStatusType.Processing;
            }

            OrderDao.UpdateObject(orderPo);

            //2.5记录支付日志
            AddOrderPaymentLog(order.OrderId, PaymentType.Gc, logPo.Id);

            //2.6.订单装填变更日志
            if (orderPo.OrderStatus == (int)OrderStatusType.Processing)
            {
                AddOrderStatusHistLogByCustomer(orderPo.OrderId, orderPo.OrderStatus, string.Empty);
            }

            //2.6.todo 需要发送支付邮件？
        }

        public void CustomerPayOrderByOceanPayment(string orderNo,
            Service.Payment.PayInfo.OceanPaymentInfo oceanPaymentInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.支付信息判断 
            //1.2订单号不一致
            if (!string.Equals(orderNo, oceanPaymentInfo.OrderNumber))
            {
                throw new BussinessException(ERROR_PAYMENT_ORDER_NOT_SAME_AS_OCEANPAYMENT);
            }

            //1.2信用卡类型错误
            if (string.IsNullOrEmpty(oceanPaymentInfo.Method))
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_TYPE_ERROR);
            }

            PaymentType paymentType = PaymentType.Webmoney;
            if (string.Equals("Webmoney", oceanPaymentInfo.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.Webmoney; ;
            }
            else if (string.Equals("Yandex", oceanPaymentInfo.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.Yandex; ;
            }
            else if (string.Equals("Credit Card", oceanPaymentInfo.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.OceanCreditCard; ;
            }
            else if (string.Equals("QiWi", oceanPaymentInfo.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                paymentType = PaymentType.QiWi; ;
            }
            else
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_TYPE_ERROR);
            }

            //1.3信用卡支付状态错误
            if (oceanPaymentInfo.PaymentStatus != OceanPaymentInfo.OCEANPAYMENT_PAY_STATUS_SUCCESS)
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_STATUS_ERROR);
            }

            //1.5币种验证
            if (string.IsNullOrEmpty(oceanPaymentInfo.OrderCurrency))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //是否只能用下单时的币种支付？
            if (!string.Equals(oceanPaymentInfo.OrderCurrency, order.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            var currency = ConfigureService.GetCurrencyByCode(oceanPaymentInfo.OrderCurrency);
            if (currency == null)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //1.6支付金额错误 
            var amount = oceanPaymentInfo.OrderAmount;
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            var amountUsd = amount;
            if (!string.Equals(ConfigureService.CURRENCY_CODE_USD, order.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                //其他币种转换为美元(下单时币种的汇率)
                amountUsd = ImplToolHelper.GetRoundValue(decimal.Divide(amount, order.ExchangeRate), currency.DecimalPlaces);
            }

            if (amountUsd <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //1.7支付交易号重复
            var logPo = PaymentLogOceanPaymentDao.GetPaymentLogByTransactionId(oceanPaymentInfo.PaymentId);
            if (logPo != null)
            {
                throw new BussinessException(ERROR_PAYMENT_CREDITCARD_DUPLICATE);
            }

            //2.保存数据
            //2.1.保存Paypal支付信息
            logPo = new PaymentLogOceanPaymentPo();
            ObjectHelper.CopyProperties(oceanPaymentInfo, logPo, new[] { "OCEANPAYMENT_PAY_STATUS_SUCCESS" });
            logPo.TargetId = order.OrderId;
            logPo.CreateDate = DateTime.Now;

            PaymentLogOceanPaymentDao.AddObject(logPo);

            //2.2.有欠款的先还欠款
            var payCashDebtAmountUsd = 0.00M;//用来归还Cash的美元金额
            var payCashDebtAmount = 0.00M;//用来归还Cash的金额

            ReturnCustomerCashArrear(order, amountUsd, out payCashDebtAmountUsd, out payCashDebtAmount);

            //2.3.更新订单支付信息
            var orderCostPo = OrderPriceDao.GetOrderCostByOrderId‎(order.OrderId);
            orderCostPo.PaymentAmount = (orderCostPo.PaymentAmount.HasValue ? orderCostPo.PaymentAmount.Value : 0.00M) + (amountUsd - payCashDebtAmountUsd);

            OrderPriceDao.UpdateObject(orderCostPo);

            //2.4.更新订单状态和支付状态
            var orderCostNew = GetOrderCostVoFromPo(orderCostPo);
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)paymentType;

            if (orderCostNew.NeedToPayAmt > 0)
            {
                orderPo.PayStatus = (int)PaidStatusType.PartPay;
            }
            else
            {
                orderPo.PayStatus = (int)PaidStatusType.FullPay;
            }

            if (IsHighRiskCustomerOrder(order))
            {
                orderPo.OrderStatus = (int)OrderStatusType.UnderChecking;
            }
            else
            {
                orderPo.OrderStatus = (int)OrderStatusType.Processing;
            }

            OrderDao.UpdateObject(orderPo);

            //2.5记录支付日志
            AddOrderPaymentLog(order.OrderId, paymentType, logPo.Id);

            //2.6.订单装填变更日志
            if (orderPo.OrderStatus == (int)OrderStatusType.Processing)
            {
                AddOrderStatusHistLogByCustomer(orderPo.OrderId, orderPo.OrderStatus, string.Empty);
            }

            //2.6.todo 需要发送支付邮件？
        }

        public void CustomerPayOrderByBankOfChina(string orderNo,
            Service.Payment.PayInfo.BankOfChinaInfo bankOfChinaInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.转账信息判断
            //币种
            if (bankOfChinaInfo.CurrencyCode.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            if (bankOfChinaInfo.Amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //2.保存数据
            //2.1.记录转账信息日志
            var logPo = new PaymentLogBankOfChinaPo
            {
                TargetId = order.OrderId,
                OrderNo = order.OrderNo,
                IsStandardCurrency = bankOfChinaInfo.IsStandardCurrency,
                CurrencyCode = bankOfChinaInfo.CurrencyCode,
                Amount = bankOfChinaInfo.Amount,
                PaymentDate = bankOfChinaInfo.PaymentDate,
                PaymentReceipt = bankOfChinaInfo.PaymentReceipt,
                CreateDate = DateTime.Now
            };

            var logId = PaymentLogBankOfChinaDao.AddObject(logPo);

            //2.2.更新订单状态和支付状态
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)PaymentType.BankOfChina;
            orderPo.PayStatus = (int)PaidStatusType.Submit;

            OrderDao.UpdateObject(orderPo);

            //2.3.记录支付日志
            AddOrderPaymentLog(order.OrderId, PaymentType.BankOfChina, logId);

            //2.4.todo 发送邮件

        }

        public void CustomerPayOrderByHsbc(string orderNo, Service.Payment.PayInfo.HsbcInfo hsbcInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.转账信息判断
            //币种
            if (hsbcInfo.CurrencyCode.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            if (hsbcInfo.Amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //2.保存数据
            //2.1.记录转账信息日志
            var logPo = new PaymentLogHsbcPo
            {
                TargetId = order.OrderId,
                OrderNo = order.OrderNo,
                IsStandardCurrency = hsbcInfo.IsStandardCurrency,
                CurrencyCode = hsbcInfo.CurrencyCode,
                Amount = hsbcInfo.Amount,
                PaymentDate = hsbcInfo.PaymentDate,
                PaymentReceipt = hsbcInfo.PaymentReceipt,
                CreateDate = DateTime.Now
            };

            var logId = PaymentLogHsbcDao.AddObject(logPo);

            //2.2.更新订单状态和支付状态
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)PaymentType.Hsbc;
            orderPo.PayStatus = (int)PaidStatusType.Submit;

            OrderDao.UpdateObject(orderPo);

            //2.3.记录支付日志
            AddOrderPaymentLog(order.OrderId, PaymentType.Hsbc, logId);

            //2.4.todo 发送邮件

        }

        public void CustomerPayOrderByMoneyGram(string orderNo, Service.Payment.PayInfo.MoneyGramInfo moneyGramInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.转账信息判断
            //币种
            if (moneyGramInfo.CurrencyCode.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            if (moneyGramInfo.Amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //2.保存数据
            //2.1.记录转账信息日志
            var logPo = new PaymentLogMoneyGramPo
            {
                TargetId = order.OrderId,
                OrderNo = order.OrderNo,
                FullnameOfRemitter = moneyGramInfo.FullNameOfRemitter,
                CountryId = moneyGramInfo.CountryId,
                IsStandardCurrency = moneyGramInfo.IsStandardCurrency,
                CurrencyCode = moneyGramInfo.CurrencyCode,
                Amount = moneyGramInfo.Amount,
                ControlNo = moneyGramInfo.ControlNo,
                PaymentReceipt = moneyGramInfo.PaymentReceipt,
                CreateDate = DateTime.Now
            };

            var logId = PaymentLogMoneyGramDao.AddObject(logPo);

            //2.2.更新订单状态和支付状态
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)PaymentType.MoneyGram;
            orderPo.PayStatus = (int)PaidStatusType.Submit;

            OrderDao.UpdateObject(orderPo);

            //2.3.记录支付日志
            AddOrderPaymentLog(order.OrderId, PaymentType.MoneyGram, logId);

            //2.4.todo 发送邮件
        }

        public void CustomerPayOrderByWesternUnion(string orderNo,
            Service.Payment.PayInfo.WesternUnionInfo westernUnionInfo)
        {
            //1.数据验证  
            //1.1.验证订单和订单支付信息
            Service.Order.Order order;
            OrderCost orderCost;

            JudgeOrderCanPay(orderNo, out order, out orderCost);

            //1.2.转账信息判断
            //币种
            if (westernUnionInfo.CurrencyCode.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            if (westernUnionInfo.Amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //2.保存数据
            //2.1.记录转账信息日志
            var logPo = new PaymentLogWesternUnionPo
            {
                TargetId = order.OrderId,
                OrderNo = order.OrderNo,
                IsStandardCurrency = westernUnionInfo.IsStandardCurrency,
                CurrencyCode = westernUnionInfo.CurrencyCode,
                Amount = westernUnionInfo.Amount,
                ControlNo = westernUnionInfo.ControlNo,
                PaymentReceipt = westernUnionInfo.PaymentReceipt,
                CreateDate = DateTime.Now
            };

            var logId = PaymentLogWesternUnionDao.AddObject(logPo);

            //2.2.更新订单状态和支付状态
            var orderPo = OrderDao.GetObject(order.OrderId);

            orderPo.PaymentId = (int)PaymentType.WesternUnion;
            orderPo.PayStatus = (int)PaidStatusType.Submit;

            OrderDao.UpdateObject(orderPo);

            //2.3.记录支付日志
            AddOrderPaymentLog(order.OrderId, PaymentType.WesternUnion, logId);

            //2.4.todo 发送邮件;
        }

        /// <summary>
        /// 是否高危客户订单
        /// </summary>
        /// <param name="order"></param>
        public bool IsHighRiskCustomerOrder(Service.Order.Order order)
        {
            var isHighRisk = false;

            //先判断是否高危客户
            var customer = CustomerService.GetCustomerById(order.CustomerId);
            if (customer != null && customer.IsDanger.HasValue)
            {
                isHighRisk = customer.IsDanger.Value;
            }

            //再判断是否高危国家
            if (!isHighRisk)
            {
                var billAddress = GetOrderBillingAddressByOrderId‎(order.OrderId);
                if (billAddress != null)
                {
                    isHighRisk = ConfigureService.IsCountryHighRisk(billAddress.Country);
                }
            }

            return isHighRisk;
        }

        public Service.Order.Order GetOrder(int orderId)
        {
            return GetOrderVoFromPo(OrderDao.GetObject(orderId));
        }

        /// <summary>
        /// 判断订单是否能支付
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="order">订单基本信息</param>
        /// <param name="orderCost">订单金额信息</param>
        private void JudgeOrderCanPay(string orderNo, out Service.Order.Order order, out OrderCost orderCost)
        {
            order = null;
            orderCost = null;

            //1.0订单号为空
            if (string.IsNullOrEmpty(orderNo))
            {
                throw new BussinessException(ERROR_ORDER_NOT_EXIST);
            }

            var orderPo = OrderDao.GetOrderByOrderNo(orderNo);

            //1.1订单不存在或已取消 
            if (orderPo == null)
            {
                throw new BussinessException(ERROR_ORDER_NOT_EXIST);
            }

            order = GetOrderVoFromPo(orderPo);

            //1.2订单状态错误
            if (order.OrderStatus != OrderStatusType.Pending &&
                order.OrderStatus != OrderStatusType.UnderChecking)
            {
                throw new BussinessException(ERROR_ORDER_STATUS_WRONG);
            }

            //1.2.2订单支付状态错误
            if (order.PaidStatus != PaidStatusType.NotPay)
            {
                throw new BussinessException(ERROR_ORDER_PAYMENT_STATUS_WRONG);
            }

            var orderConstPo = OrderPriceDao.GetOrderCostByOrderId‎(orderPo.OrderId);

            if (orderConstPo == null)
            {
                throw new BussinessException(ERROR_ORDER_NOT_EXIST);
            }

            orderCost = GetOrderCostVoFromPo(orderConstPo);

            //1.3订单支付状态错误:需要支付的金额小于等于0，代表着不需要支付了
            if (orderCost.NeedToPayAmt <= 0)
            {
                throw new BussinessException(ERROR_ORDER_AMOUNT_WRONG);
            }

            //1.4订单支付状态错误:订单付款待确认状态限制
        }


        /// <summary>
        /// 归还Cash欠款
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="payAmountUsd">支付的美元金额</param> 
        /// <param name="payCashDebtAmountUsd">归还的美元金额</param>
        /// <param name="payCashDebtAmount">归还的金额</param>
        private void ReturnCustomerCashArrear(Service.Order.Order order, decimal payAmountUsd, out decimal payCashDebtAmountUsd, out decimal payCashDebtAmount)
        {
            payCashDebtAmountUsd = 0.00M;
            payCashDebtAmount = 0.00M;

            var cashArrearAmount = CashService.GetCustomerArrear(order.CustomerId);
            if (cashArrearAmount > 0)
            {
                if (cashArrearAmount >= payAmountUsd)
                {
                    //如果欠款大于等于支付金额那么支付金额全部用于还款
                    payCashDebtAmountUsd = payAmountUsd;
                }
                else
                {
                    //如果欠款小于支付金额那么只需要还掉欠的金额
                    payCashDebtAmountUsd = cashArrearAmount;
                }

                payCashDebtAmount = payCashDebtAmountUsd;

                if (!string.Equals(ConfigureService.CURRENCY_CODE_USD, order.Currency, StringComparison.InvariantCultureIgnoreCase))
                {
                    payCashDebtAmount = ImplToolHelper.GetRoundValue(payCashDebtAmountUsd * order.ExchangeRate);
                }

                CashService.ReturnArrear(order.CustomerId, order.Currency, payCashDebtAmount, string.Format("cash pepayment in order #{0}", order.OrderNo));
            }
        }

        private void AddOrderPaymentLog(int orderId, PaymentType paymentType, int objectId)
        {
            var po = new OrderPaymentLogPo()
            {
                OrderId = orderId,
                PaymentType = (int)paymentType,
                LogObjectId = objectId,
                IsCommunicated = false,
                PaymentDate = DateTime.Now,
                Remark = string.Empty
            };

            OrderPaymentLogDao.AddObject(po);
        }

        private void AddOrderStatusHistLogByCustomer(int orderId, int orderStatus, string comments)
        {
            var po = new OrderStatusHistoryPo
            {
                OrderId = orderId,
                NotifyCustomer = false,
                ChangeDate = DateTime.Now,
                Status = orderStatus,
                Comments = comments
            };

            OrderStatusHistoryDao.AddObject(po);
        }
        #endregion


        public IList<OrderStatus> GetAllOrderStatuses()
        {
            var list = new List<OrderStatus>();
            var orderstatus = OrderStatusDescriptionDao.GetAllOrderStatusDesc();
            foreach (var orderStatusDescriptionPo in orderstatus)
            {
                list.Add(GetOrderStatusVoFromPo(orderStatusDescriptionPo));
            }
            return list;
        }

        public IList<OrderStatus> GetAllCustomerOrderStatus(int languageId)
        {
            var list = new List<OrderStatus>();
            var orderstatus = OrderStatusDescriptionDao.GetAllCustomerOrderStatusDesc(languageId);
            foreach (var orderStatusDescriptionPo in orderstatus)
            {
                list.Add(GetOrderStatusVoFromPo(orderStatusDescriptionPo));
            }
            return list;
        }

        public IList<OrderPaidStatus> GetAllOrderPaidStatuses()
        {
            var list = new List<OrderPaidStatus>();
            var orderpaystatus = OrderPayStatusDescriptionDao.GetAllOrderPayStatusDesc();
            foreach (var orderPayStatusDescriptionPo in orderpaystatus)
            {
                list.Add(GetOrderPaidStatusVoFromPo(orderPayStatusDescriptionPo));
            }
            return list;
        }


        public IDictionary<int, int> GetEachOrderStatusCountByCustomerId(int customerId)
        {
            return OrderDao.GetOrderStatusCountByCustomerId(customerId);
        }

        public PageData<Service.Order.Order> GetOrdersByCustomerId(int customerId, int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria,
                                              IList<Sorter<OrderSorterCriteria>> sorterCriteria)
        {

            searchCriteria.Add(OrderSearchCriteria.CustomerId, customerId);
            return OrderDao.FindOrders(currentPage, pageSize, searchCriteria, sorterCriteria);
        }

        public Service.Order.Order GetOrderByOrderId(int orderId)
        {
            var po = OrderDao.GetObject(orderId);
            return GetOrderVoFromPo(po);
        }

        public Service.Order.Order GetOrderByOrderNo(string orderNo)
        {
            var po = OrderDao.GetOrderByOrderNo(orderNo);
            return GetOrderVoFromPo(po);
        }

        #region 私有方法

        internal Service.Order.Order GetOrderVoFromPo(OrderPo orderPo)
        {
            Service.Order.Order order = null;
            if (!orderPo.IsNullOrEmpty())
            {
                order = new Service.Order.Order
                {
                    OrderId = orderPo.OrderId,
                    OrderNo = orderPo.OrderNumber,
                    CustomerId = orderPo.CustomerId,
                    OrderTime = orderPo.OrderTime.HasValue ? orderPo.OrderTime.Value : DateTime.MinValue,
                    PaidTime = orderPo.LastPayTime.HasValue ? orderPo.LastPayTime.Value : DateTime.MinValue,
                    PaymentMethod = orderPo.PaymentId,
                    CollectType = orderPo.CollectType,
                    ExchangeRate = orderPo.ExchangeRate,
                    Currency = orderPo.CurrencyCode,
                    OrderStatus = OrderStatusDao.GetOrderStatusDisplay(orderPo.OrderStatus).ToEnum<OrderStatusType>(),
                    OrderType = orderPo.OrderType,
                    PaidStatus = orderPo.PayStatus.ToEnum<PaidStatusType>(),
                    OrderRemark = orderPo.OrderRemark,
                    PackageWeight = orderPo.PackageWeight,
                    Weight = orderPo.Weight,
                    OrderSource = orderPo.OrderSource,
                    ShippingId = orderPo.ShippingId,
                    IsUpgradeShipping = orderPo.IsUpgradeShippingMethod,
                    IsReviewAll = orderPo.IsReviewAll,
                    SoldWaitType = orderPo.OutOfStockWaitType,
                    CustomerTaxNumber = orderPo.TaxNumber,
                    ReportMoney = orderPo.ReportProductMoney,
                    ReportCurrencyCode = orderPo.ReportCurrencyCode,
                    ReportShippingMoney = orderPo.ReportShippingMoney,
                    OrderCost = GetOrderCostByOrderId‎(orderPo.OrderId),
                    OrderIpAddress = orderPo.OrderIpAddress

                };
            }
            return order;
        }

        internal static OrderCost GetOrderCostVoFromPo(OrderPricePo orderPricePo)
        {
            OrderCost orderCost = null;
            if (!orderPricePo.IsNullOrEmpty())
            {
                orderCost = new OrderCost
                 {
                     OrderId = orderPricePo.OrderId,
                     OriginalProductAmount = orderPricePo.ProductAmountOriginal,
                     NoDiscountProductAmount = orderPricePo.ProductAmountNormal,
                     DiscountProductAmount = orderPricePo.ProductAmountDiscount,
                     BaseShippingCost = orderPricePo.ShippingAmountBase,
                     FarawayCost = orderPricePo.ShippingAmountRemote.HasValue ? orderPricePo.ShippingAmountRemote.Value : 0.0M,
                     FreeShippingDiffAmt = orderPricePo.FreeShippingBalance.HasValue ? orderPricePo.FreeShippingBalance.Value : 0.0M,
                     ClubDiffAmt = orderPricePo.ClubShippingBalance.HasValue ? orderPricePo.ClubShippingBalance.Value : 0.0M,
                     ClubFee = orderPricePo.ClubHandlingFee,
                     CashAmt = orderPricePo.UseCash.HasValue ? orderPricePo.UseCash.Value : 0.0M,
                     CouponAmt = orderPricePo.CouponAmount,
                     OrderDiscount = orderPricePo.OrderDiscountPercent,//订单折扣
                     OrderDiscountLessAmount = orderPricePo.OrderDiscountMoney,//订单折扣优惠金额
                     ShippingCostDiscount = orderPricePo.ShippingDiscountPercent,//运费折扣
                     BusinessDerateAmount = orderPricePo.BusinessDerateAmount,//业务附加费
                     BusinessSurcharge = orderPricePo.BusinessAddedAmount,//业务附加费
                     VipDiscount = orderPricePo.VipDiscountPercent,//vip折扣
                     VipLessAmount = orderPricePo.VipDiscountMoney,//Vip价格
                     FreeShippingFee = orderPricePo.ShippingHandlingFee,
                     Refund = orderPricePo.RefundAmount.HasValue ? orderPricePo.RefundAmount.Value : 0.0M,
                     LastModifyTime = orderPricePo.DateModified.HasValue ? orderPricePo.DateModified.Value : DateTime.MinValue,
                     TotalShippingCost = orderPricePo.ShippingAmountTotal.HasValue ? orderPricePo.ShippingAmountTotal.Value : 0.0M,
                     TotalOrderAmt = orderPricePo.OrderAmountTotal.HasValue ? orderPricePo.OrderAmountTotal.Value : 0.0M,
                     TotalProductAmt = orderPricePo.ProductAmountTotal.HasValue ? orderPricePo.ProductAmountTotal.Value : 0.0M,
                     PromotionLessAmount = orderPricePo.ProductDiscount,
                     PaymentAmount = orderPricePo.PaymentAmount.HasValue ? orderPricePo.PaymentAmount.Value : 0.00M
                 };
            }
            return orderCost;
        }

        internal static OrderDetail GetOrderDetailVoFromPo(OrderDetailPo orderDetailPo)
        {
            OrderDetail orderDetail = null;
            if (!orderDetailPo.IsNullOrEmpty())
            {
                orderDetail = new OrderDetail
                    {
                        Id = orderDetailPo.OrderProductId,
                        OrderId = orderDetailPo.OrderId,
                        ProductId = orderDetailPo.ProductId,
                        ProductNo = orderDetailPo.ProductModel,
                        DeliveryQty = orderDetailPo.DeliveryNumber,
                        DiscountType = orderDetailPo.DiscountType,
                        DiscountValue = orderDetailPo.DiscountValue,
                        ForecastReachQty = orderDetailPo.ForecastReachNumber.HasValue ? orderDetailPo.ForecastReachNumber.Value : 0,
                        ForecastReachTime = orderDetailPo.ForecastReachDate,
                        Weight = orderDetailPo.ProductWeight,
                        VolumeWeight = orderDetailPo.ProductVolumeWeight,
                        MainImage = orderDetailPo.MainImage,
                        Remark = orderDetailPo.CustomerRemark,
                        OriginalPrice = orderDetailPo.ProductPrice,
                        Price = orderDetailPo.ProductPrice * orderDetailPo.DiscountValue,
                        IsReservation = orderDetailPo.IsReservation,
                        IsReviewed = orderDetailPo.IsReviewed,
                        Quantity = orderDetailPo.ProductQuantity,
                    };
            }
            return orderDetail;
        }

        internal static OrderStatus GetOrderStatusVoFromPo(OrderStatusDescriptionPo orderStatusDescriptionPo)
        {
            OrderStatus orderStatus = null;
            if (!orderStatusDescriptionPo.IsNullOrEmpty())
            {
                orderStatus = new OrderStatus
                 {
                     LanguageId = orderStatusDescriptionPo.Id.LanguageId,
                     Name = orderStatusDescriptionPo.OrdersStatusName,
                     Value = orderStatusDescriptionPo.Id.OrderStatus,
                 };

            }
            return orderStatus;
        }

        internal static OrderPaidStatus GetOrderPaidStatusVoFromPo(OrderPayStatusDescriptionPo orderPayStatusDescriptionPo)
        {
            OrderPaidStatus orderPaidStatus = null;
            if (!orderPayStatusDescriptionPo.IsNullOrEmpty())
            {
                orderPaidStatus = new OrderPaidStatus
                {
                    LanguageId = orderPayStatusDescriptionPo.Id.LanguageId,
                    Name = orderPayStatusDescriptionPo.OrdersStatusName,
                    Value = orderPayStatusDescriptionPo.Id.PayStatus,
                };

            }
            return orderPaidStatus;
        }

        internal OrderPaymentAmountLog GetOrderPaymentAmountLogVoFromPo(OrderPaymentAmountLogPo orderPaymentAmountLogPo)
        {
            OrderPaymentAmountLog orderPaymentAmountLog = null;
            if (!orderPaymentAmountLogPo.IsNullOrEmpty())
            {
                var admin = ServiceFactory.AdminUserService.GetAdminUser(orderPaymentAmountLogPo.AdminId);
                orderPaymentAmountLog = new OrderPaymentAmountLog
                {
                    OrderId = orderPaymentAmountLogPo.OrderId,
                    CreateTime = orderPaymentAmountLogPo.DateCreated,
                    OriginalAmount = orderPaymentAmountLogPo.OriginalAmount,
                    NewAmount = orderPaymentAmountLogPo.NewAmount,
                    Creator = orderPaymentAmountLogPo.AdminId,
                    LogId = orderPaymentAmountLogPo.LogId,
                    CreatorEmail = admin.IsNullOrEmpty() ? "" : admin.AccountEmail,
                };
            }
            return orderPaymentAmountLog;
        }

        internal static SpecialOrder GeSpecialOrderVoFromPo(SpecialOrderPo specialOrderPo)
        {
            SpecialOrder specialOrder = null;
            if (!specialOrderPo.IsNullOrEmpty())
            {
                specialOrder = new SpecialOrder
                {
                    SpecialOrderId = specialOrderPo.SpecialId,
                    CustomerId = specialOrderPo.CustomerId,
                    Increase = specialOrderPo.IncreaseMoney,
                    CurrencyCode = specialOrderPo.CurrencyCode,
                    Remark = specialOrderPo.Remark,
                    Status = specialOrderPo.Status,
                    Creator = specialOrderPo.AdminId,
                    CreateTime = specialOrderPo.DateCreated,
                };
            }
            return specialOrder;
        }

        internal static Package GetPackageVoFromPo(PackagePo orderPackagePo)
        {
            Package package = null;
            if (!orderPackagePo.IsNullOrEmpty())
            {
                package = new Package
                {
                    PackageId = orderPackagePo.PackageId,
                    OrderId = orderPackagePo.OrderId,
                    ErpPackageNumber = orderPackagePo.ErpPackageNumber,
                    OrderNumber = orderPackagePo.OrderNumber,
                    ShippedDate = orderPackagePo.DateShippinged,
                    ShippingId = orderPackagePo.ShippingId,
                    IsReceived = orderPackagePo.IsReceived,
                    TrackingNumber = orderPackagePo.TraceNumber,
                    LastModifyTime = orderPackagePo.DateModified
                };
            }
            return package;
        }

        /// <summary>
        /// 货运地址Po转Vo
        /// </summary>
        /// <param name="orderAddressPo">订单地址po</param>
        /// <returns></returns>
        internal static OrderShippingAddress GetOrderShippingAddressVoFromPo(OrderAddressPo orderAddressPo)
        {
            OrderShippingAddress orderShippingAddress = null;
            if (!orderAddressPo.IsNullOrEmpty())
            {
                orderShippingAddress = new OrderShippingAddress
                {
                    OrderId = orderAddressPo.OrderId,
                    CompanyName = orderAddressPo.Company,
                    Country = orderAddressPo.CountryId,
                    CountryName = orderAddressPo.Country,
                    Province = orderAddressPo.Province,
                    City = orderAddressPo.City,
                    FirstName = orderAddressPo.FirstName,
                    LastName = orderAddressPo.LastName,
                    FullName = orderAddressPo.FullName,
                    Street1 = orderAddressPo.StreetAddress,
                    Street2 = orderAddressPo.StreetAddress2,
                    ZipCode = orderAddressPo.ZipCode,
                    Telphone = orderAddressPo.PhoneNumber
                };
            }
            return orderShippingAddress;
        }

        /// <summary>
        /// 账单地址Po转Vo
        /// </summary>
        /// <param name="orderAddressPo">订单地址po</param>
        /// <returns></returns>
        internal static OrderBillingAddress GetOrderBillingAddressVoFromPo(OrderAddressPo orderAddressPo)
        {
            OrderBillingAddress orderBillingAddress = null;
            if (!orderAddressPo.IsNullOrEmpty())
            {
                orderBillingAddress = new OrderBillingAddress
                {
                    OrderId = orderAddressPo.OrderId,
                    CompanyName = orderAddressPo.Company,
                    Country = orderAddressPo.CountryId,
                    CountryName = orderAddressPo.Country,
                    Province = orderAddressPo.Province,
                    City = orderAddressPo.City,
                    FirstName = orderAddressPo.FirstName,
                    LastName = orderAddressPo.LastName,
                    FullName = orderAddressPo.FullName,
                    Street1 = orderAddressPo.StreetAddress,
                    Street2 = orderAddressPo.StreetAddress2,
                    ZipCode = orderAddressPo.ZipCode,
                    Telphone = orderAddressPo.PhoneNumber
                };
            }
            return orderBillingAddress;
        }

        /// <summary>
        /// 订单备注Po转Vo
        /// </summary>
        /// <param name="orderRemarkByAdminPo">订单备注po</param>
        /// <returns>订单备注Vo</returns>
        internal static OrderRemark GetOrderRemarkVoFromPo(OrderRemarkByAdminPo orderRemarkByAdminPo)
        {
            OrderRemark orderRemark = null;
            if (!orderRemarkByAdminPo.IsNullOrEmpty())
            {
                orderRemark = new OrderRemark
                {
                    RemarkId = orderRemarkByAdminPo.RemarkId,
                    OrderId = orderRemarkByAdminPo.OrderId,
                    RemarkContent = orderRemarkByAdminPo.RemarkContent,
                    DateCreated = orderRemarkByAdminPo.DateCreated,
                    RemarkType = orderRemarkByAdminPo.RemarkType.ToEnum<OrderRemarkType>(),
                    AdminId = orderRemarkByAdminPo.AdminId,
                };
            }
            return orderRemark;
        }
        #endregion
    }
}
