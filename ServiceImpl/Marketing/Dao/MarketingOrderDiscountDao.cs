//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MarketingOrderDiscountDao.cs
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
    public class MarketingOrderDiscountDao : BaseDao<MarketingOrderDiscountPo, int>, IMarketingOrderDiscountDao
    {

        public decimal MarketingForOrderDiscount(OrderDiscountCriteria criteria)
        {
            var discount = 1M;
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_for_orderdiscount", new[] 
            {
                new SqlParameter("@CountryIsoCode2", SqlDbType.Int){Value = criteria.CountryIsoCode2},
                new SqlParameter("@VipLevel", SqlDbType.Int){Value = criteria.VipLevel},
                new SqlParameter("@LanguageId", SqlDbType.Int){Value = criteria.LanguageId},
                new SqlParameter("@CustomerId", SqlDbType.Int){Value = criteria.CustomerId},
                new SqlParameter("@ClubLevel", SqlDbType.Int){Value = criteria.ClubLevel},
                new SqlParameter("@IsChannel", SqlDbType.Int){Value = criteria.IsChannel},
                new SqlParameter("@CurrencyId", SqlDbType.Int){Value = criteria.CurrencyId},
                new SqlParameter("@TotalAmount", SqlDbType.Int){Value = criteria.TotalAmount},
                new SqlParameter("@NormalAmount", SqlDbType.Int){Value = criteria.NormalAmount},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }))
            {
                if (reader.Read())
                {
                    discount = reader["Discount"].ParseTo<decimal>();
                    reader.Close();
                }
            }
            return discount;
        }

        public List<OrderAmountDiscount> GetMarketingForOrderDiscount(string countryIsoCode2, int languageId)
        {
            var lstOrderAmountDiscount = new List<OrderAmountDiscount>();
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_orderdiscount_enduring_get", new[] 
            {
                new SqlParameter("@countryIsoCode2", SqlDbType.Int){Value = countryIsoCode2},
                new SqlParameter("@languageId", SqlDbType.Int){Value = languageId},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }))
            {
                if (reader.Read())
                {
                    lstOrderAmountDiscount.Add(new OrderAmountDiscount
                    {
                        Id = reader["Id"].ParseTo<int>(),
                        Amount = reader["Amount"].ParseTo<decimal>(),
                        Discount = reader["Discount"].ParseTo<decimal>()
                    });
                    reader.Close();
                }
            }
            return lstOrderAmountDiscount;
        }
    }
}
