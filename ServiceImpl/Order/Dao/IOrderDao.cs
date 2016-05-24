//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderDao.cs
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
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public interface IOrderDao:IBaseDao<OrderPo,int>
    {
        /// <summary>
        /// 根据订单No获取订单
        /// </summary>
        /// <param name="orderNo">订单No</param>
        /// <returns>OrderPo</returns>
        OrderPo GetOrderByOrderNo(string orderNo);

        /// <summary>
        /// 判断是否客户订单
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        bool IsCustomerOrder(int customerId, int orderId);

        /// <summary>
        /// 根据客户Id获取每个状态的订单数量[用于绑定状态列表]‎
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>每个状态对应的订单数量,key=状态id，values=数量</returns>
        IDictionary<int, int> GetOrderStatusCountByCustomerId(int customerId);

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<Service.Order.Order> FindOrders(int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 后台订单查询
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<Service.Order.Order> GetAdminOrdersList(int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 读取客户最后一个订单信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="critria">判断条件</param>
        /// <returns></returns>
        OrderPo GetCustomerLatestOrder(int customerId, Sorter<LatestOrderSorterCriteria> critria);

    } 
}
   