//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MarketingPlaceOrderDao.cs
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
using System.Data.SqlClient;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Marketing;
using Com.Panduo.Service.Marketing.Criteria;
using Com.Panduo.Service.Marketing.PlaceOrder;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{
    public class MarketingPlaceOrderDao : BaseDao<MarketingPlaceOrderPo, int>, IMarketingPlaceOrderDao
    {
        public PlaceOrderResult MarketingForPlaceOrder(int orderId, PlaceOrderCriteria criteria)
        {
            var placeOrderResult = new PlaceOrderResult();
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_for_placeorder", new[] 
            {
                new SqlParameter("@OrderId", SqlDbType.Int){Value = orderId},
                new SqlParameter("@CountryIsoCode2", SqlDbType.VarChar){Value = criteria.CountryIsoCode2},
                new SqlParameter("@VipLevel", SqlDbType.Int){Value = criteria.VipLevel},
                new SqlParameter("@LanguageId", SqlDbType.Int){Value = criteria.LanguageId},

                //以下参数不用
                new SqlParameter("@CustomerId", SqlDbType.Int){Value = criteria.CustomerId},
                new SqlParameter("@ClubLevel", SqlDbType.Int){Value = criteria.ClubLevel},
                new SqlParameter("@IsChannel", SqlDbType.Bit){Value = criteria.IsChannel},
                new SqlParameter("@CurrencyId", SqlDbType.Int){Value = criteria.CurrencyId},
                new SqlParameter("@TotalAmount", SqlDbType.Decimal){Value = criteria.TotalAmount},
                new SqlParameter("@NormalAmount", SqlDbType.Decimal){Value = criteria.NormalAmount},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }))
            {
                if (reader.Read())
                {
                    placeOrderResult = new PlaceOrderResult
                    {
                        OrderId = reader["OrderId"].ParseTo<int>(),
                        CustomerId = reader["CustomerId"].ParseTo<int>(),
                        ResultType = reader["ResultType"].ParseTo<int>().ToEnum<MarketingPlaceOrderResultType>(),
                        GiftLevel = reader["GiftLevel"].ParseTo<string>(),
                        CouponCode = reader["CouponCode"].ParseTo<string>(),
                        CurrencyId = reader["CurrencyId"].ParseTo<int>()
                    };

                    reader.Close();
                }
            }
            return placeOrderResult;
        }
    }
}
