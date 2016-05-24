//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderDetailVo.cs
//创 建 人：罗海明
//创建时间：2015/04/08 17:55:40 
//功能说明：订单明细Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.Web.Models.Order
{
    public class OrderDetailVo
    {
        /// <summary>
        /// 订单货运地址
        /// </summary>
        public OrderShippingAddress OrderShippingAddress { get; set; }

        /// <summary>
        /// 客户默认货运地址
        /// </summary>
        public Address DefaultShippingAddress { get; set; }

        /// <summary>
        /// 订单主表
        /// </summary>
        public Service.Order.Order Order { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public IList<OrderDetailItemVo> OrderDetailList { get; set; }

        /// <summary>
        /// 获取订单状态变更历史 
        /// </summary>
        public IList<OrderStatusHistory> OrderStatusHistoryList { get; set; }

        public IList<OrderRemark> OrderRemarkList { get; set; }


        public OrderSearchVo OrderSearchVo { get; set; }
    }
}