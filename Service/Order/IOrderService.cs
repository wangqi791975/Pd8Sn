using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Payment.PayConfig;
using Com.Panduo.Service.Payment.PayInfo;

namespace Com.Panduo.Service.Order
{
    public interface IOrderService
    {
        #region 常量

        /// <summary>
        /// COUPON不能使用
        /// </summary>
        string ERROR_COUPON_IS_DISABLED { get; }

        /// <summary>
        /// 新商品被下架
        /// </summary>
        string ERROR_NEW_PRODUCT_HAS_BEEN_NOT_ON_SALE    { get; }

        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }

        /// <summary>
        /// 提交失败
        /// </summary>
        string ERROR_TO_SUBMIT { get; }

        /// <summary>
        /// 订单不存在
        /// </summary>
        string ERROR_ORDER_NOT_EXIST { get; }
        /// <summary>
        /// 订单为发货状态或者完成状态，不允许修改
        /// </summary>
        string ERROR_ORDER_NOT_UPDATE { get; }

        /// <summary>
        /// 订单状态错误
        /// </summary>
        string ERROR_ORDER_STATUS_WRONG
        {
            get; 
            
        }
        /// <summary>
        /// 订单金额错误
        /// </summary>
        string ERROR_ORDER_AMOUNT_WRONG
        {
            get; 
            
        }
        /// <summary>
        /// 订单支状态错误
        /// </summary>
        string ERROR_ORDER_PAYMENT_STATUS_WRONG { get; }
        /// <summary>
        /// GC支付的信用卡类型不能为空
        /// </summary>
        string ERROR_PAYMENT_TYPE_GC_TYPE_EMPTY { get; }
        /// <summary>
        /// 没有足够的Cash全额支付订单
        /// </summary>
        string ERROR_PAYMENT_NOT_ENOUGH_TO_PAY_BY_CASH { get; }
        /// <summary>
        /// Cash存在欠款不能支付
        /// </summary>
        string ERROR_PAYMENT_CASH_DEBT { get; }
        
        /// <summary>
        /// 要支付的订单与paypal返回的订单号不一致
        /// </summary>
        string ERROR_PAYMENT_ORDER_NOT_SAME_AS_PAYPAL { get; }
        /// <summary>
        /// 订单支付币种错误
        /// </summary>
        string ERROR_PAYMENT_PAY_CURRENCY_ERROR { get; }
        /// <summary>
        /// 支付金额错误
        /// </summary>
        string ERROR_PAYMENT_PAY_AMOUNT_ERROR { get; }
        /// <summary>
        /// paypal支付状态错误
        /// </summary>
        string ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR { get; }
        /// <summary>
        /// paypal交易号重复
        /// </summary>
        string ERROR_PAYMENT_PAYPAL_DUPLICATE { get; }
        /// <summary>
        /// 要支付的订单与GC返回的订单号不一致
        /// </summary>
        string ERROR_PAYMENT_ORDER_NOT_SAME_AS_GC { get; }
        /// <summary>
        /// 信用卡类型错误
        /// </summary>
        string ERROR_PAYMENT_CREDITCARD_TYPE_ERROR { get; }
        /// <summary>
        /// 信用卡状态错误
        /// </summary>
        string ERROR_PAYMENT_CREDITCARD_STATUS_ERROR { get; }
        /// <summary>
        /// 信用卡重复支付
        /// </summary>
        string ERROR_PAYMENT_CREDITCARD_DUPLICATE { get; }
        /// <summary>
        /// 要支付的订单与OceanPayment返回的订单号不一致
        /// </summary>
        string ERROR_PAYMENT_ORDER_NOT_SAME_AS_OCEANPAYMENT { get; }
        
        /// <summary>
        /// 钱海支付交易号重复支付
        /// </summary>
        string ERROR_PAYMENT_OCEANPAYMENT_DUPLICATE { get; }


        /// <summary>
        /// 客户邮箱不存在
        /// </summary>
        string ERROR_CUSTOMER_EMAIL_NOT_EXIST { get; }


        #endregion

        #region 客户操作

        /// <summary>
        /// 客户下单
        /// </summary>
        /// <param name="checkoutDraft">下单对象</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品不存在</value>
        /// <value>ERROR_COUPON_IS_DISABLED:COUPON不能使用</value>
        /// <value>ERROR_NEW_PRODUCT_HAS_BEEN_NOT_ON_SALE:新商品被下架</value>
        /// </exception>
        /// <returns></returns>
        string PlaceOrderByCustomer(CheckoutDraft checkoutDraft);

        /// <summary>
        /// 客户取消订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_:</value>
        /// </exception>
        void CustomerCancelOrder(string orderId);

        /// <summary>
        /// 客户包裹确认收货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_:</value>
        /// </exception>
        void CustomerConfirmOrderPackageReceived(string orderId);

        /// <summary>
        /// 修改订单评论状态
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="isReviewAll">是否已评论(0：未评论，1：已评论)</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_:</value>
        /// </exception>
        void UpdateOrderIsReservationById(int orderId, bool isReviewAll);

        #endregion

        #region 订单支付
        #region 客户支付订单
        /// <summary>
        /// 客户更换支付方式
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="paymentType">新的支付方式</param>
        /// <param name="gcType">更换为GC支付的时候，GC信用卡类型需要填写,其他方式填写该值将被忽略</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding的订单不许更改支付方式</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_TYPE_GC_TYPE_EMPTY:GC支付的信用卡类型不能为空</value>
        /// </exception>
        void CustomerChangePaymentType(string orderNo, PaymentType paymentType, GlobalCollectType? gcType);

        /// <summary>
        /// 客户使用Cash支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="isFullPay">true-全额Cash支付,false-部分Cash支付</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_NOT_ENOUGH_TO_PAY_BY_CASH:没有足够的Cash全额支付订单</value>
        /// <value>ERROR_PAYMENT_CASH_DEBT:Cash存在欠款不能支付</value>
        /// </exception>
        void CustomerPayOrderByCash(string orderNo, bool isFullPay);

        /// <summary>
        /// 客户使用Paypal支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="paypalInfo"></param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_ORDER_NOT_SAME_AS_PAYPAL:要支付的订单与paypal返回的订单号不一致</value>
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:paypal支付金额错误</value>
        /// <value>ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR:paypal支付状态错误</value>
        /// <value>ERROR_PAYMENT_PAYPAL_DUPLICATE:paypal交易号重复</value>
        /// </exception>
        void CustomerPayOrderByPaypal(string orderNo, PaypalInfo paypalInfo);

        /// <summary>
        /// 客户使用GC信用卡支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="gcInfo">GC信用卡支付信息</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_ORDER_NOT_SAME_AS_GC:要支付的订单与GC返回的订单号不一致</value> 
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:要支付的订单与GC信用卡支付金额错误</value>  
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value>
        /// <value>ERROR_PAYMENT_CREDITCARD_TYPE_ERROR:信用卡类型错误</value>
        /// <value>ERROR_PAYMENT_CREDITCARD_STATUS_ERROR:信用卡状态错误</value>
        /// <value>ERROR_PAYMENT_CREDITCARD_DUPLICATE:信用卡重复支付</value> 
        /// </exception>
        void CustomerPayOrderByGc(string orderNo, GlobalCollectInfo gcInfo);

        /// <summary>
        /// 客户使用钱海支付订单(包括钱海平台上的Webmoney、Yandex、Credit Card、QiWi通道)
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="oceanPaymentInfo">钱海支付信息</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>  
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value> 
        /// <value>ERROR_PAYMENT_ORDER_NOT_SAME_AS_OCEANPAYMENT:要支付的订单与OceanPayment返回的订单号不一致</value> 
        /// <value>ERROR_PAYMENT_CREDITCARD_TYPE_ERROR:信用卡类型错误</value>
        /// <value>ERROR_PAYMENT_CREDITCARD_STATUS_ERROR:信用卡状态错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:支付金额错误</value>  
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value> 
        /// <value>ERROR_PAYMENT_OCEANPAYMENT_DUPLICATE:交易号重复支付</value> 
        /// </exception>
        void CustomerPayOrderByOceanPayment(string orderNo, OceanPaymentInfo oceanPaymentInfo);

        /// <summary>
        /// 客户使用中国银行支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="bankOfChinaInfo">中国银行转账支付信息</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:支付金额错误</value>  
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value> 
        /// </exception>
        void CustomerPayOrderByBankOfChina(string orderNo, BankOfChinaInfo bankOfChinaInfo);

        /// <summary>
        /// 客户使用中国银行支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="hsbcInfo">中国工行转账支付信息</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:支付金额错误</value>  
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value> 
        /// </exception>
        void CustomerPayOrderByHsbc(string orderNo, HsbcInfo hsbcInfo);

        /// <summary>
        /// 客户使用中国银行支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="moneyGramInfo">MoneyGram汇款支付信息</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:支付金额错误</value>  
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value> 
        /// </exception>
        void CustomerPayOrderByMoneyGram(string orderNo, MoneyGramInfo moneyGramInfo);

        /// <summary>
        /// 客户使用中国银行支付订单
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="westernUnionInfo">西联汇款支付信息</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_STATUS_WRONG:非Peding和部分支付状态的订单不允许支付</value>
        /// <value>ERROR_ORDER_PAYMENT_STATUS_WRONG:订单支付状态错误</value>
        /// <value>ERROR_ORDER_AMOUNT_WRONG:订单金额错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:支付金额错误</value>  
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:订单支付币种错误</value> 
        /// </exception>
        void CustomerPayOrderByWesternUnion(string orderNo, WesternUnionInfo westernUnionInfo);

        /// <summary>
        /// 是否高危客户订单
        /// </summary>
        /// <param name="order"></param>
        bool IsHighRiskCustomerOrder(Service.Order.Order order);

        #endregion

        #region 客服操作订单支付信息

        #endregion


        #endregion

        #region 订单基本信息

        /// <summary>
        /// 通过订单Id获取订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单</returns>
        Order GetOrder(int orderId);

        /// <summary>
        /// 获取所有订单状态
        /// </summary>
        /// <returns>订单状态集合</returns>
        IList<OrderStatus> GetAllOrderStatuses();

        /// <summary>
        /// 获取所有订单状态
        /// </summary>
        /// <returns>订单状态集合</returns>
        IList<OrderStatus> GetAllCustomerOrderStatus(int languageId);

        /// <summary>
        /// 获取所有订单支付状态
        /// </summary>
        /// <returns>订单支付状态集合</returns>
        IList<OrderPaidStatus> GetAllOrderPaidStatuses();

        /// <summary>
        /// 根据客户Id获取每个订单状态的数量[用于绑定状态列表]‎
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>每个状态对应的订单数量,key=状态id，values=数量</returns>
        IDictionary<int, int> GetEachOrderStatusCountByCustomerId(int customerId);

       

        /// <summary>
        /// 根据客户Id 分页获取该客户订单列表
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的订单列表</returns>
        PageData<Order> GetOrdersByCustomerId(int customerId, int currentPage, int pageSize,
            IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 根据订单Id获取订单对象
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>返回对应的订单对象</returns>
        Order GetOrderByOrderId(int orderId);

        /// <summary>
        /// 根据订单No获取订单对象
        /// </summary>
        /// <param name="orderNo">订单No</param>
        /// <returns>返回对应的订单对象</returns>
        Order GetOrderByOrderNo(string orderNo);

        /// <summary>
        /// 根据客户Id取最近一条订单信息
        /// </summary>
        /// <returns></returns>
        Order GetCustomerLatestOrder(int customerId, Sorter<LatestOrderSorterCriteria> critria);

        /// <summary>
        /// 搜索订单列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的客户订单列表</returns>
        PageData<Order> FindOrders(int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria);

        #endregion

        #region 订单金额
        /// <summary>
        /// 获取订单金额对象
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单金额</returns>
        OrderCost GetOrderCostByOrderId‎(int orderId);

        #endregion

        #region 订单商品
        /// <summary>
        /// 根据订单Id 分页获取订单明细列表
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <returns>包含分页的订单明细列表</returns>
        PageData<OrderDetail> GetOrderDetsById(int customerId,int orderId, int currentPage, int pageSize, IDictionary<OrderDetailSearchCriteria, object> searchCriteria);

        /// <summary>
        /// 根据订单Id获取订单明细列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IList<OrderDetail> GetOrderDetailListByOrderId(int orderId);


        OrderDetail GetOrderDetsById(int customerId, int orderDetsId);

        /// <summary>
        /// 根据订单Id获取订单明细数量
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单明细数量 key=状态 value=数量</returns>
        IDictionary<int,int> GetOrderItemsCountByOrderId(int orderId);


        /// <summary>
        /// 请求下载单张图片
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns>成功返回下载URL地址，失败返回空</returns>
        string RequestDownloadImage(string imageName);

        /// <summary>
        /// 请求批量下载图片
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>成功返回压缩包下载URL地址，失败返回空</returns>
        string RequestDownloadImageBatch(int orderId);

        /// <summary>
        /// 修改订单明细状态
        /// </summary>
        /// <param name="orderItemId">订单明细Id</param>
        /// <param name="orderItemStatus">订单明细状态</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_:</value>
        /// </exception>
        void UpdateOrderItemStatusById(string orderItemId, int orderItemStatus);

        /// <summary>
        /// 修改订单明细评论状态
        /// </summary>
        /// <param name="orderItemId">订单明细Id</param>
        /// <param name="isReviewed">是否已评论(0：未评论，1：已评论)</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_:</value>
        /// </exception>
        void UpdateOrderItemIsReviewedById(int orderItemId, bool isReviewed);
        #endregion

        #region 订单状态变更历史

        /// <summary>
        /// 获取订单状态变更历史 add by luohaiming
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        IList<OrderStatusHistory> GetOrderStatusHistoryByOrderId‎(int orderId); 

        #endregion

        #region 订单地址
        /// <summary>
        /// 根据账单地址Id获取订单账单地址
        /// </summary>
        /// <param name="orderBillingAddressId">订单账单地址Id</param>
        /// <returns>订单账单地址</returns>
        OrderBillingAddress GetOrderBillingAddressById‎(int orderBillingAddressId);

        /// <summary>
        /// 根据订单Id获取订单账单地址
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单账单地址</returns>
        OrderBillingAddress GetOrderBillingAddressByOrderId‎(int orderId);

        /// <summary>
        /// 根据收货地址Id获取订单收货地址
        /// </summary>
        /// <param name="orderShippingAddressId">订单收货地址Id</param>
        /// <returns>订单收货地址</returns>
        OrderShippingAddress GetOrderShippingAddressById‎(int orderShippingAddressId);

        /// <summary>
        /// 根据订单Id获取订单收货地址
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单收货地址</returns>
        OrderShippingAddress GetOrderShippingAddressByOrderId‎(int orderId);
        #endregion

        #region 订单包裹
        /// <summary>
        /// 根据订单Id获取包裹列表
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        IList<Package> GetOrderPackageByOrderId(int orderId);

        /// <summary>
        /// 搜索订单包裹列表 add by luohaiming
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的订单包裹列表</returns>
        PageData<PackageDetail> FindOrderPackages(int currentPage, int pageSize, IDictionary<PackageSearchCriteria, object> searchCriteria, IList<Sorter<PackageSorterCriteria>> sorterCriteria);


        #endregion

        #region 订单快照

        /// <summary>
        /// 获取订单快照‎
        /// </summary>
        /// <param name="orderDetId">订单明细Id</param>
        /// <returns>订单快照‎</returns>
        OrderImpression GetOrderImpressionByOrderId‎(int orderDetId);
        #endregion

        #region 订单评论

        //IReviewService
        #endregion

        #region 后台操作
        /*/// <summary>
        /// 后台客服添加订单：线下Email下单
        /// </summary>
        /// <param name="order">订单对象</param>
        /// <param name="shippingAddress">订单对象</param>
        /// <param name="billingAddress">订单对象</param>
        /// <param name="shopCartId">订单对象</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_:</value>
        /// </exception>
        /// <returns>新添加的订单Id</returns>
        //void AddOrderForSale(CheckoutDraft checkoutDraft);//(Order order, OrderShippingAddress shippingAddress, OrderBillingAddress billingAddress, string shopCartId);
        */

        /// <summary>
        /// 后台客服修改订单
        /// </summary>
        /// <param name="order">要修改的订单</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_NOT_UPDATE:订单允许修改</value>
        /// </exception>
        void UpdateOrderForSale(Order order);

        /// <summary>
        /// 后台客服修改订单
        /// </summary>
        /// <param name="orderId">要修改的订单Id</param>
        /// <param name="status">要修改的订单状态</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// <value>ERROR_ORDER_NOT_UPDATE:订单不允许修改</value>
        /// </exception>
        void UpdateOrderStatusForSale(string orderId, string status);

        /// <summary>
        /// 后台客服回填订单发货信息
        /// </summary>
        /// <param name="orderId">要回填的订单Id</param>
        /// <param name="shippingId">配送方式Id</param>
        /// <param name="trackingNumber">跟踪号</param>
        /// <param name="shippedDate">发货时间</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// </exception>
        void SignOrderStatusForSale(string orderId, string shippingId, string trackingNumber, DateTime shippedDate);

        /// <summary>
        /// 后台客服确认订单
        /// </summary>
        /// <param name="orderId">要确认的订单Id</param>
        /// <param name="isPass">是否通过</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_ORDER_NOT_EXIST:订单不存在</value>
        /// </exception>
        void CheckOrderStatusForSale(string orderId, bool isPass);

        /// <summary>
        /// 根据客户Id 分页获取该客户订单列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的订单列表</returns>
        PageData<Order> GetAdminOrdersList(int currentPage, int pageSize,
            IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria);


        void DeleteOrder(int orderId,int adminId);

        IList<OrderPaymentAmountLog> GetOrderPaymentAmountLogList(int orderId);

        void AddOrderPaymentAmountLog(int orderId, decimal paymentAmount, int adminId);

        void UpdateOrderBusinessDiscountAmount(int orderId, decimal amount, string remark, int adminId);

        void UpdateOrderBusinessSurcharge(int orderId, decimal amount, string remark, int adminId);

        IList<OrderRemark> GetOrderRemarks(int orderId);

        /// <summary>
        /// 后台获取订单状态变更历史
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IList<OrderStatusHistory> GetOrderAdminStatusHistoryByOrderId‎(int orderId);

        /// <summary>
        /// 获取订单状态变更历史 add by luohaiming
        /// </summary>
        /// <param name="orderStatusHistory">OrderStatusHistory</param>
        /// <returns></returns>
        void ChangeOrderStatus‎(OrderStatusHistory orderStatusHistory,string remark);

        /// <summary>
        /// 根据语种Id获取所有的订单状态(后台) add by luohaiming 2015-04-17
        /// </summary>
        /// <param name="languageId">语种Id</param>
        IList<OrderStatus> GetAdminOrderStatus(int languageId);

       #region 后台特殊报价
        /// <summary>
        /// 获取特殊报价订单列表
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        PageData<SpecialOrder> GetSpecialOrder(int currentPage, int pageSize, IDictionary<SpecialOrderSearchCriteria, object> searchCriteria, IList<Sorter<SpecialOrderSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 删除特殊报价订单
        /// </summary>
        /// <param name="specialId">特殊报价Id</param>
        void DeleteSpecialOrder(int specialId);

        /// <summary>
        /// 添加特殊报价
        /// </summary>
        /// <param name="special">特殊报价实体</param>
        void AddSpecialOrder(SpecialOrder special);

        /// <summary>
        /// 获取特殊报价
        /// </summary>
        /// <param name="specialId">特殊报价Id</param>
        /// <returns></returns>
        SpecialOrder GetSpecialOrder(int specialId);
        #endregion
        #endregion

    }

    public enum SpecialOrderSorterCriteria
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        CreateDate
    }

    public enum SpecialOrderSearchCriteria
    {
    }


    public enum LatestOrderSorterCriteria
    {
        OrderTime
    }

    public enum OrderSearchCriteria
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        OrderId,

        /// <summary>
        /// 订单编号(YYMMDD前台显示)
        /// </summary>
        OrderNo,

        /// <summary>
        /// 客户ID
        /// </summary>
        CustomerId,

        /// <summary>
        /// 语种code
        /// </summary>
        LanguageCode,

        /// <summary>
        /// 客户姓名或Email
        /// </summary>
        Customer,

        /// <summary>
        /// 产品编号
        /// </summary>
        PartNo,

        /// <summary>
        /// 下单开始日期
        /// </summary>
        OrderDateFrom,

        /// <summary>
        ///  下单结束日期
        /// </summary>
        OrderDateTo,

        /// <summary>
        /// 支付日期
        /// </summary>
        PaidTime,

        /// <summary>
        /// 支付方式
        /// </summary>
        PaymentMethod,

        /// <summary>
        /// 运输方式
        /// </summary>
        ShippingMethod,

        /// <summary>
        /// 订单状态
        /// </summary>
        OrderStatus,

        /// <summary>
        /// 订单类型(顾客下单，Email下单，客服拆单)
        /// </summary>
        OrderType,

        /// <summary>
        /// 支付状态(已支付待验证、处理中)
        /// </summary>
        PaidStatus,

        /// <summary>
        /// 缺货等待类型
        /// </summary>
        SoldWaitType,

        /// <summary>
        /// 订单来源
        /// </summary>
        OrderSource,
    }
    public enum OrderSorterCriteria
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        OrderId,

        /// <summary>
        /// 订单编号(YYMMDD前台显示)
        /// </summary>
        OrderNo,

        /// <summary>
        /// 下单日期
        /// </summary>
        OrderTime,

        /// <summary>
        /// 支付日期
        /// </summary>
        PaidTime,

        /// <summary>
        /// 订单状态
        /// </summary>
        OrderStatus,

        /// <summary>
        /// 订单类型(顾客下单，Email下单，客服拆单)
        /// </summary>
        OrderType,

        /// <summary>
        /// 支付状态(已支付待验证、处理中)
        /// </summary>
        PaidStatus,
    }

    /// <summary>
    /// 包裹搜索条件 add by luohaiming
    /// </summary>
    public enum PackageSearchCriteria
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        PartNo,
        /// <summary>
        /// 跟踪号
        /// </summary>
        TrackingNumber,
        /// <summary>
        /// Erp包裹号
        /// </summary>
        PackingNo,
        /// <summary>
        /// 订单编号
        /// </summary>
        OrderNo,
        /// <summary>
        /// 客户Id
        /// </summary>
        CustomerId,
    }

    /// <summary>
    /// 包裹排序条件 add by luohaiming
    /// </summary>
    public enum PackageSorterCriteria
    {
        /// <summary>
        /// 根据包裹号排序
        /// </summary>
        PackageId

    }

    public enum OrderDetailSearchCriteria
    {
        /// <summary>
        /// 是否已评论
        /// </summary>
        IsReviewed
    }
}
