using Com.Panduo.Entity.Coupon;

namespace Com.Panduo.ServiceImpl.Coupon.Dao
{
    public interface ICouponDao : IBaseDao<CouponPo, int>
    {
        /// <summary>
        /// 通过coupon编号获取coupon
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        CouponPo GetCoupon(string couponCode);
    }
}