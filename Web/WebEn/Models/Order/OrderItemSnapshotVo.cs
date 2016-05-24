using Com.Panduo.Service.Order;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.Order
{
    public class OrderItemSnapshotVo
    {
        /// <summary>
        /// 订单明细快照
        /// </summary>
        public OrderImpression OrderSnapshot { get; set; }

        /// <summary>
        /// 订单主表信息（主要获取订单状态，订单日期，订单No）
        /// </summary>
        public Service.Order.Order Order { get; set; }

        /// <summary>
        /// 订单明细信息
        /// </summary>
        public OrderDetail OrderDetail { get; set; }


        /// <summary>
        /// 产品扩展信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }
    }
}