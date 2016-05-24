//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderPriceDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 12:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public interface IOrderPriceDao:IBaseDao<OrderPricePo,int>
    {
        /// <summary>
        /// 获取订单金额对象
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单金额</returns>
        OrderPricePo GetOrderCostByOrderId‎(int orderId);
    } 
}
   