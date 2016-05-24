//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderPaymentAmountLogDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
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
    public interface IOrderPaymentAmountLogDao:IBaseDao<OrderPaymentAmountLogPo,int>
    {
        /// <summary>
        /// 根据订单号获取金额修改日志记录
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        IList<OrderPaymentAmountLogPo> GetOrderPaymentAmountLogList(int orderId);

    } 
}
   