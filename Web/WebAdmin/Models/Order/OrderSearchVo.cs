using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.Web.Models.Order
{
    public class OrderSearchVo
    {
        public IList<Language> Languages { get; set; }

        public IList<Shipping> Shippings { get; set; }

        public IList<OrderStatus> OrderStatus { get; set; }

        public Dictionary<int, string> Payments { get; set; }
    }
}