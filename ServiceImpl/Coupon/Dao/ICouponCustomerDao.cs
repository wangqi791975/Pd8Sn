using System;
using System.Collections;
using System.Collections.Generic;
using Com.Panduo.Entity.Coupon;
using NHibernate.Mapping;

namespace Com.Panduo.ServiceImpl.Coupon.Dao
{
    public interface ICouponCustomerDao : IBaseDao<CouponCustomerPo, int>
    {
        /// <summary>
        /// 通过客户Id获取客户coupon
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        IList<CouponCustomerPo> GetCouponCustomers(int customerId);

        /// <summary>
        /// 通过客户Id，couponCode获取coupon
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="couponCode">coupon编号</param>
        /// <returns></returns>
        IList<CouponCustomerPo> GetCouponCustomers(int customerId, string couponCode);

        /// <summary>
        /// 通过客户Id，couponCode获取coupon
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="couponCode">coupon编号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        IList<CouponCustomerPo> GetCouponCustomers(int customerId, string couponCode, int status);

        /// <summary>
        /// 获取最新到期coupon
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="status">状态</param>
        /// <returns>最近到期coupon</returns>
        CouponCustomerPo GetNewsExpireCouponCustomer(int customerId, int status);

        /// <summary>
        /// 修改客户优惠券
        /// </summary>
        /// <param name="couponCustomerId">客户优惠券Id</param>
        /// <param name="useTime">使用时间</param>
        /// <param name="orderId">订单号</param>
        /// <param name="status">状态</param>
        void UpdateCouponCustomer(int couponCustomerId, DateTime useTime, int orderId, int status);

        /// <summary>
        /// 修改客户优惠券
        /// </summary>
        /// <param name="couponCustomerId">客户优惠券Id</param>
        /// <param name="adminId">操作人Id</param>
        /// <param name="status">状态</param>
        /// <param name="reason">修改原因</param>
        /// <param name="useTime"></param>
        void UpdateCouponCustomer(int couponCustomerId, int adminId, int status, string reason, DateTime useTime);

        /// <summary>
        /// 修改客户优惠券
        /// </summary>
        /// <param name="couponCustomerId">客户优惠券Id</param>
        /// <param name="adminId"></param>
        /// <param name="status">状态</param>
        /// <param name="endDateTime">coupon结束时间</param>
        void UpdateCouponCustomer(int couponCustomerId, int adminId, int status, DateTime endDateTime);
    }
}