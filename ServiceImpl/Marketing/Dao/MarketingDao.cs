//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MarketingDao.cs
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
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Marketing;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Marketing.Criteria;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{
    public class MarketingDao : BaseDao<MarketingPo, int>, IMarketingDao
    {
        public PiecingOrderResult GetPiecingOrderInfo(PiecingOrderCriteria criteria)
        {
            var placeOrderResult = new PiecingOrderResult();
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_piecingorderinfo_get", new[] 
            {
                new SqlParameter("@ClubLevel", SqlDbType.Int){Value =criteria.ClubLevel},
                new SqlParameter("@VipLevel", SqlDbType.Int){Value = criteria.VipLevel},
                new SqlParameter("@GrandTotal", SqlDbType.Decimal){Value = criteria.GrandTotal},
                new SqlParameter("@NoDiscountProductAmount", SqlDbType.Decimal){Value = criteria.NoDiscountProductAmount},
                new SqlParameter("@CustomerId", SqlDbType.Int){Value = criteria.CustomerId??0},

                new SqlParameter("@IsChannel", SqlDbType.Bit){Value = criteria.IsChannel},


                //以下参数不用
                new SqlParameter("@CountryIsoCode2", SqlDbType.VarChar){Value = criteria.CountryIsoCode2??""},
                new SqlParameter("@LanguageId", SqlDbType.Int){Value = criteria.LanguageId??0},
                new SqlParameter("@CurrencyId", SqlDbType.Int){Value = criteria.CurrencyId??0},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }))
            {
                if (reader.Read())
                {
                    placeOrderResult = new PiecingOrderResult
                    {
                        HasPiecingOrderTip = reader["HasPiecingOrderTip"].ParseTo<bool>(),// 是否有凑单内容
                        IsClubFreeShipping = reader["IsClubFreeShipping"].ParseTo<bool>(),// 是否Club免运费
                        OrderBalance = reader["OrderBalance"].ParseTo<decimal>(),// 订单凑单差额
                        OrderDiscount = reader["OrderDiscount"].ParseTo<decimal>(),// 订单可享受折扣
                    };

                    reader.Close();
                }
            }
            return placeOrderResult;
        }

        public void UpdateMarketingStatus(int marketingId, bool status)
        {
            UpdateObjectByHql("UPDATE MarketingPo SET Status = ? WHERE Id = ?", new object[] { status, marketingId });
        }
    }
}
