using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Entity.Coupon;
using Com.Panduo.Service.Coupon;

namespace Com.Panduo.ServiceImpl.Coupon.Dao
{
    public class CouponDescDao : BaseDao<CouponDescPo, int>, ICouponDescDao
    {
        public List<CouponDescPo> GetCouponDescs(int couponId)
        {
            return FindDataByHql("FROM CouponDescPo WHERE CouponId = ?", couponId).ToList();
        }

        public CouponDescPo GetCouponDesc(int couponId, int languageId)
        {
            return GetOneObject("FROM CouponDescPo WHERE CouponId = ? AND LanguageId = ?", new object[] { couponId, languageId });
        }

        public void UpdateCouponDesc(int couponId, int languageId, string desc)
        {
            UpdateObjectByHql("UPDATE CouponDescPo SET Description = ? WHERE CouponId = ? AND LanguageId = ?", new object[] { desc, couponId, languageId });
        }

        public void DelteCouponDesc(int couponId)
        {
            DeleteObjectByHql("DELETE FROM CouponDescPo WHERE CouponId = ?", couponId);
        }
    }
}