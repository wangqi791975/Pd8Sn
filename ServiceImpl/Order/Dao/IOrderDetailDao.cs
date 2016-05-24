//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderDetailDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 12:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Order;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public interface IOrderDetailDao:IBaseDao<OrderDetailPo,int>
    {
        /// <summary>
        /// 是否购买过产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Od</param>
        /// <returns>是否购买产品</returns>
        bool IsBuyProduct(int customerId, int productId);

        /// <summary>
        /// 获取客户订单的明细
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCreCriteria">查询条件</param>
        /// <returns></returns>
        PageData<OrderDetailPo> GetOrderDetsById(int orderId, int currentPage, int pageSize, IDictionary<OrderDetailSearchCriteria, object> searchCriteria);

        /// <summary>
        /// 获取一条订单明细
        /// </summary>
        /// <param name="orderDetsId">明细Id</param>
        /// <returns></returns>
        OrderDetailPo GetOrderDetsById(int orderDetsId);

        /// <summary>
        /// 根据订单Id获取订单明细数量
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单明细数量 key=状态 value=数量</returns>
        IDictionary<int, int> GetOrderDetsCountByOrderId(int orderId);

    } 
}
   