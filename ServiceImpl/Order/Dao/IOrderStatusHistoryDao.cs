//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderStatusHistoryDao.cs
//创 建 人：罗海明
//创建时间：2015/02/10 15:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Order;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{
    public interface IOrderStatusHistoryDao : IBaseDao<OrderStatusHistoryPo, int>
    {

        /// <summary>
        /// 获取订单状态变更历史 
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        IList<OrderStatusHistory> GetOrderStatusHistoryByOrderId‎(int orderId, int languageId);

        /// <summary>
        /// 后台获取订单状态变更历史 
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        IList<OrderStatusHistory> GetOrderAdminStatusHistoryByOrderId‎(int orderId);
    } 
}
   