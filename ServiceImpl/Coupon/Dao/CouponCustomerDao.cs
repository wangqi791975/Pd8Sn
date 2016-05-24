using System;
using System.Collections.Generic;
using Com.Panduo.Entity.Coupon;

namespace Com.Panduo.ServiceImpl.Coupon.Dao
{
    public class CouponCustomerDao : BaseDao<CouponCustomerPo, int>, ICouponCustomerDao
    {
        public IList<CouponCustomerPo> GetCouponCustomers(int customerId)
        {
            return FindDataByHql("FROM CouponCustomerPo WHERE CustomerId = ?", customerId);
        }

        public IList<CouponCustomerPo> GetCouponCustomers(int customerId, string couponCode)
        {
            return FindDataByHql("FROM CouponCustomerPo WHERE CustomerId = ? AND CouponCode = ?", new object[] { customerId, couponCode });
        }

        public IList<CouponCustomerPo> GetCouponCustomers(int customerId, string couponCode, int status)
        {
            return FindDataByHql("FROM CouponCustomerPo WHERE CustomerId = ? AND CouponCode = ? AND Status = ?", new object[] { customerId, couponCode, status });
        }

        public CouponCustomerPo GetNewsExpireCouponCustomer(int customerId, int status)
        {
            return GetOneObject("FROM CouponCustomerPo WHERE CustomerId = ? AND Status = ? ORDER BY UseDateEnded ASC", new object[] { customerId, status });
        }

        public void UpdateCouponCustomer(int couponCustomerId, DateTime useTime, int orderId, int status)
        {
            UpdateObjectByHql("UPDATE CouponCustomerPo SET DateUsed = ?,UseOrderId = ?,Status = ? WHERE Id = ?", new object[] { useTime, orderId, status, couponCustomerId });
        }

        public void UpdateCouponCustomer(int couponCustomerId, int adminId, int status, string reason, DateTime useTime)
        {
            UpdateObjectByHql("UPDATE CouponCustomerPo SET AdminId = ?, UpdateReason = ?,Status = ? ,DateUsed = ? WHERE Id = ?", new object[] { adminId, reason, status, useTime, couponCustomerId });
        }

        public void UpdateCouponCustomer(int couponCustomerId, int adminId, int status, DateTime endDateTime)
        {
            UpdateObjectByHql("UPDATE CouponCustomerPo SET AdminId = ?,UseDateEnded = ?,Status = ? WHERE Id = ?", new object[] { adminId, endDateTime, status, couponCustomerId });
        }
    }
}