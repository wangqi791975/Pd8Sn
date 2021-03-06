//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderAddressDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using Com.Panduo.Entity.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public class OrderAddressDao:BaseDao<OrderAddressPo,int>, IOrderAddressDao
    {
        /// <summary>
        /// 根据订单Id和地址类型获取订单地址信息
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="addressType">地址类型</param>
        /// <returns></returns>
        public OrderAddressPo GetOrderAddressByOrderId(int orderId,int addressType)
        {
            return GetOneObject("from OrderAddressPo where OrderId = ? and AddressType=?",new object[]{ orderId,addressType});
        }
    } 
}