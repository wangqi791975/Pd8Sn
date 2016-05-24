//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMarketingPlaceOrderDao.cs
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
using Com.Panduo.Entity.Marketing;
using Com.Panduo.Service.Marketing.Criteria;
using Com.Panduo.Service.Marketing.PlaceOrder;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public interface IMarketingPlaceOrderDao:IBaseDao<MarketingPlaceOrderPo,int>
    {
        /// <summary>
        /// 存储过程：根据订单数据匹配下单送礼活动
        /// 完成送礼操作
        /// </summary>
        PlaceOrderResult MarketingForPlaceOrder(int orderId, PlaceOrderCriteria criteria);
    } 
}
   