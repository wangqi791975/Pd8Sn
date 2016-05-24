//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMarketingOrderDiscountDao.cs
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
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Marketing.Criteria;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public interface IMarketingOrderDiscountDao:IBaseDao<MarketingOrderDiscountPo,int>
    {
        /// <summary>
        /// 存储过程：下单过程中 获取客户订单金额 匹配一个订单折扣
        /// 购物车 和 下单页面用到
        /// </summary>
        decimal MarketingForOrderDiscount(OrderDiscountCriteria criteria);

        /// <summary>
        /// 存储过程：把当前长期折扣取出来前台展示
        /// </summary>
        List<OrderAmountDiscount> GetMarketingForOrderDiscount(string countryIsoCode2, int languageId);
    } 
}
   