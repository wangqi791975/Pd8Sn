//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMarketingCouponDao.cs
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

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public interface IMarketingCouponDao:IBaseDao<MarketingCouponPo,int>
    {
        /// <summary>
        /// 存储过程
        /// 获取前台注册页面提示送Coupon的信息
        /// </summary>
        List<Service.Coupon.Coupon> GetCouponCodeForRegister(CouponCriteria criteria);

        /// <summary>
        /// 存储过程
        /// 前台注册页面提示送Coupon的信息
        /// </summary>
        Service.Coupon.Coupon SendCouponCodeForRegister(CouponCriteria criteria);

        /// <summary>
        /// 通过couponCode获取营销coupon
        /// </summary>
        /// <param name="couponCode">couponCode</param>
        /// <returns>营销coupon</returns>
        MarketingCouponPo GetMarketingCoupon(string couponCode);

        /// <summary>
        /// 通过coupon类型获取获取营销coupon
        /// </summary>
        /// <param name="rewardType">coupon类型</param>
        /// <returns></returns>
        IList<MarketingCouponPo> GetMarketingCoupons(int rewardType);

        /// <summary>
        /// 通过活动类型获取coupon
        /// </summary>
        /// <param name="rewardType">活动类型</param>
        /// <returns></returns>
        MarketingCouponPo GetMarketingCoupon(int rewardType);
    } 
}
   