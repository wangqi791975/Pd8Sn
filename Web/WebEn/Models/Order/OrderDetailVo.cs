//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderDetailVo.cs
//创 建 人：罗海明
//创建时间：2015/02/15 15:40:40 
//功能说明：订单明细Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShippingOption;

namespace Com.Panduo.Web.Models.Order
{
    public class OrderDetailVo
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
        /// 支付类型
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 运送方式名称（对应语种，从IIS缓存读取）
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        ///  配送方式天数
        /// </summary>
        public ShippingDay ShippingDay { get; set; }

        /// <summary>
        /// 包裹信息
        /// </summary>
        public IList<Package> PackageList { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public PageData<OrderDetailItemVo> OrderDetailList { get; set; }

        /// <summary>
        /// 获取订单状态变更历史 
        /// </summary>
        public IList<OrderStatusHistory> OrderStatusHistoryList { get; set; }

    }
}