//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MarketingCouponDao.cs
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
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Marketing.Criteria;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{
    public class MarketingCouponDao : BaseDao<MarketingCouponPo, int>, IMarketingCouponDao
    {
        /// <summary>
        /// 存储过程
        /// 获取前台注册页面提示送Coupon的信息
        /// </summary>
        public List<Service.Coupon.Coupon> GetCouponCodeForRegister(CouponCriteria criteria)
        {
            var lstCoupon = new List<Service.Coupon.Coupon>();
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_coupon_for_register_get", new[] 
            {
                new SqlParameter("@CustomerId", SqlDbType.Int){Value = criteria.CustomerId},
                new SqlParameter("@CountryIsoCode2", SqlDbType.VarChar){Value = criteria.CountryIsoCode2},
                new SqlParameter("@VipLevel", SqlDbType.Int){Value = criteria.VipLevel},
                new SqlParameter("@LanguageId", SqlDbType.Int){Value = criteria.LanguageId},

                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }))
            {
                if (reader.Read())
                {
                    lstCoupon.Add(new Service.Coupon.Coupon
                    {
                        CouponId = reader["CouponId"].ParseTo<int>(),// 优惠券Id
                        CouponCode = reader["CouponCode"].ParseTo<string>(),// 优惠券编号
                        Amount = reader["Amount"].ParseTo<decimal>(),// 面额
                        AmountCurrencyId = reader["AmountCurrencyId"].ParseTo<int>(),// 面额币种
                        AmountType = reader["AmountType"].ParseTo<int>().ToEnum<AmountType>(),//AmountType 金额对象
                        MinAmount = reader["MinAmount"].ParseTo<decimal>(),// 启用金额
                        MinAmountCurrencyId = reader["MinAmountCurrencyId"].ParseTo<int>(),// 启用金额币种
                        LanguageIds = reader["LanguageIds"].ParseTo<string>(),// 使用语种（多个）
                        CountryIds = reader["CountryIds"].ParseTo<string>(),// 使用国家（多个）
                        CurrencyIds = reader["CurrencyIds"].ParseTo<string>(),// 使用币种（多个）
                        LimitBeginTime = reader["LimitBeginTime"].ParseTo<DateTime>(),// 使用期限起始时间
                        LimitEndTime = reader["LimitEndTime"].ParseTo<DateTime>(),// 使用期限截止时间
                        LimitType = reader["LimitType"].ParseTo<int>().ToEnum<LimitType>(),//LimitType 使用期限类型
                        LimitDay = reader["LimitDay"].ParseTo<int>(),// 使用期限（天）
                        AllowManualPick = reader["AllowManualPick"].ParseTo<bool>(),// 是否允许手动领取
                        PickBeginTime = reader["PickBeginTime"].ParseTo<DateTime>(),// 领取期限起始时间
                        PickEndTime = reader["PickEndTime"].ParseTo<DateTime>(),// 领取期限截止时间
                        Status = reader["Status"].ParseTo<int>(),// 是否有效（标识启用 未启用）
                        LimitCount = reader["LimitCount"].ParseTo<int>(),// 每个客户领取次数限制
                        TotalCount = reader["TotalCount"].ParseTo<int>(),// 总数
                    });
                    reader.Close();
                }
            }
            return lstCoupon;
        }

        /// <summary>
        /// 存储过程
        /// 前台注册送Coupon：方法里要实现送给该客户
        /// 提示送Coupon信息要提示给客户
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Service.Coupon.Coupon SendCouponCodeForRegister(CouponCriteria criteria)
        {
            Service.Coupon.Coupon lstCoupon = null;
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_marketing_send_coupon_for_register", new[] 
            {
                new SqlParameter("@CustomerId", SqlDbType.Int){Value = criteria.CustomerId},
                new SqlParameter("@LanguageId", SqlDbType.Int){Value = criteria.LanguageId},
                new SqlParameter("@RewardType", SqlDbType.Int){Value = (int)CouponMarketingRewardType.Register}, 
            }))
            {
                if (reader.Read())
                {
                    lstCoupon = new Service.Coupon.Coupon
                    {
                        CouponId = reader["coupon_id"].ParseTo<int>(),// 优惠券Id
                        CouponCode = reader["coupon_code"].ParseTo<string>(),// 优惠券编号
                        Amount = reader["amount"].ParseTo<decimal>(),// 面额
                        AmountCurrencyId = reader["amount_currency"].ParseTo<int>(),// 面额币种
                        AmountType = reader["amount_type"].ParseTo<int>().ToEnum<AmountType>(),//AmountType 金额对象
                        MinAmount = reader["min_amount"].ParseTo<decimal>(),// 启用金额
                        MinAmountCurrencyId = reader["min_amount_currency"].ParseTo<int>(),// 启用金额币种
                        LanguageIds = reader["get_language_id"].ParseTo<string>(),// 使用语种（多个）
                        CountryIds = reader["get_country_id"].ParseTo<string>(),// 使用国家（多个）
                        LimitBeginTime = reader["use_date_started"].ParseTo<DateTime>(),// 使用期限起始时间
                        LimitEndTime = reader["use_date_ended"].ParseTo<DateTime>(),// 使用期限截止时间
                        LimitType = reader["expired_type"].ParseTo<int>().ToEnum<LimitType>(),//LimitType 使用期限类型
                        LimitDay = reader["expired_day"].ParseTo<int>(),// 使用期限（天）
                        AllowManualPick = reader["get_able"].ParseTo<bool>(),// 是否允许手动领取
                        PickBeginTime = reader["get_date_started"].ParseTo<DateTime>(),// 领取期限起始时间
                        PickEndTime = reader["get_date_ended"].ParseTo<DateTime>(),// 领取期限截止时间
                        Status = reader["status"].ParseTo<int>(),// 是否有效（标识启用 未启用）
                    };
                    reader.Close();
                }
            }
            return lstCoupon;
        }

        public MarketingCouponPo GetMarketingCoupon(string couponCode)
        {
            return GetOneObject("FROM MarketingCouponPo WHERE CouponCode = ?", couponCode);
        }

        public IList<MarketingCouponPo> GetMarketingCoupons(int rewardType)
        {
            return FindDataByHql("FROM MarketingCouponPo WHERE RewardType = ?", rewardType);
        }

        public MarketingCouponPo GetMarketingCoupon(int rewardType)
        {
            return GetOneObject("FROM MarketingCouponPo WHERE RewardType = ? ORDER BY Id DESC", rewardType);
        }
    }
}
