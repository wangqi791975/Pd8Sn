//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderDetailItemVo.cs
//创 建 人：罗海明
//创建时间：2015/02/15 15:40:40 
//功能说明：订单明细项扩展Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.Order
{
    public class OrderDetailItemVo
    {
        /// <summary>
        /// 产品扩展信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 订单明细信息
        /// </summary>
        public OrderDetail OrderDetail { get; set; }
    }
}