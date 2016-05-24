//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderRemarkByAdminDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 18:08:40 
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
    public class OrderRemarkByAdminDao:BaseDao<OrderRemarkByAdminPo,int>, IOrderRemarkByAdminDao
    {
        public IList<OrderRemarkByAdminPo> GetOrderRemarkList(int orderId)
        {
            return FindDataByHql("from OrderRemarkByAdminPo where OrderId=?", orderId);
        }

        public OrderRemarkByAdminPo GetOrderRemark(int orderId, int remarkType)
        {
            return GetOneObject("from OrderRemarkByAdminPo where OrderId=? And RemarkType=?", new object[] { orderId, remarkType });
        }
    } 
}
   