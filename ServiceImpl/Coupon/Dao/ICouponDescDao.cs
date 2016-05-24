using System.Collections.Generic;
using Com.Panduo.Entity.Coupon;

namespace Com.Panduo.ServiceImpl.Coupon.Dao
{
    public interface ICouponDescDao : IBaseDao<CouponDescPo, int>
    {
        /// <summary>
        /// 通过couponId获取coupon多语言描述
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        List<CouponDescPo> GetCouponDescs(int couponId);

        /// <summary>
        /// 通过couponId和语种Id获取描述信息
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        CouponDescPo GetCouponDesc(int couponId, int languageId);

        /// <summary>
        /// 通过couponId和语种Id修改描述
        /// </summary>
        /// <param name="couponId">coupondId</param>
        /// <param name="languageId">语种Id</param>
        /// <param name="desc">描述</param>
        void UpdateCouponDesc(int couponId, int languageId, string desc);

        /// <summary>
        /// 通过couponId删除coupon对应描述
        /// </summary>
        /// <param name="couponId">couponId</param>
        void DelteCouponDesc(int couponId);
    }
}