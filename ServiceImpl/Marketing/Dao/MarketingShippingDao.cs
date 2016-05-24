//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MarketingShippingDao.cs
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
using System.Data;
using System.Linq; 
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Marketing;
using Com.Panduo.Service.Marketing.PlaceOrder;
using Com.Panduo.Service.Order.ShippingOption;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public class MarketingShippingDao:BaseDao<MarketingShippingPo,int>, IMarketingShippingDao
    {
        public MarketingShippingPo GetMarketingShippingByMarketingId(int marketingId)
        {
            return GetOneObject("FROM MarketingShippingPo WHERE MarketingId = ?", marketingId);
        }

        public List<ShippingAmount> GetMarketingForShippingFee(List<ShippingAmount> shippingAmounts, ShipppingCriteria criteria)
        {
            var shippingAmount = new ShippingAmount();
            //using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_for_placeorder", new[] 
            //{
            //}))
            //{
            //    if (reader.Read())
            //    {
            //        placeOrderResult = new ShippingAmount
            //        {
            //        };

            //        reader.Close();
            //    }
            //}
            //return placeOrderResult;
            //  ToDo
            return new List<ShippingAmount>();
        }
    } 
}
   