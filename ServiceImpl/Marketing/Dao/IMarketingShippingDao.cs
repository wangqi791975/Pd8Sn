//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMarketingShippingDao.cs
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

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public interface IMarketingShippingDao:IBaseDao<MarketingShippingPo,int>
    {
        /// <summary>
        /// 根据marketingId获取运费活动信息
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        MarketingShippingPo GetMarketingShippingByMarketingId(int marketingId);


        List<Service.Order.ShippingOption.ShippingAmount> GetMarketingForShippingFee(List<Service.Order.ShippingOption.ShippingAmount> shippingAmounts, Service.Order.ShippingOption.ShipppingCriteria criteria);
    } 
}
   