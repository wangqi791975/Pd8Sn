using System;

namespace Com.Panduo.Service.Coupon
{
    /// <summary>
    /// Coupon描述多语种
    /// </summary>
    [Serializable]
    public class CouponDesc
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 优惠券Id
        /// </summary>
        public virtual int CouponId { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name{get;set;}

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
    }
}