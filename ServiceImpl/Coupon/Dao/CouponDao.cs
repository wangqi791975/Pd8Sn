using Com.Panduo.Entity.Coupon;

namespace Com.Panduo.ServiceImpl.Coupon.Dao
{
    public class CouponDao : BaseDao<CouponPo, int>, ICouponDao
    {
        public CouponPo GetCoupon(string couponCode)
        {
            return GetOneObject("FROM CouponPo WHERE CouponCode = ?", couponCode);
        }
    }
}