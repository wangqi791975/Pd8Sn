using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.Web.Models.Order
{
    public class OrderInvoiceVo
    {
        /// <summary>
        /// 订单主表
        /// </summary>
        public Service.Order.Order Order { get; set; }

        /// <summary>
        /// 订单货运地址
        /// </summary>
        public OrderShippingAddress OrderShippingAddress { get; set; }

        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 运送方式名称（对应语种，从IIS缓存读取）
        /// </summary>
        public string ShippingName { get; set; }


        /// <summary>
        /// 订单明细
        /// </summary>
        public IList<OrderDetailItemVo> OrderDetailList { get; set; }
    }
}