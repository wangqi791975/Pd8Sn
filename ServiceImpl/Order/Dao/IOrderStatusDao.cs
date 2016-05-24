//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderStatusDao.cs
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
    public interface IOrderStatusDao:IBaseDao<OrderStatusPo,int>
    {
        /// <summary>
        /// 根据订单状态获取显示状态值
        /// </summary>
        /// <param name="orderStatus">订单状态</param>
        /// <returns>显示状态值</returns>
        int GetOrderStatusDisplay(int orderStatus);
    } 
}
   