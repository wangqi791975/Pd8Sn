using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Coupon
{
    /// <summary>
    /// 优惠券
    /// </summary>
    [Serializable]
    public class Coupon
    {
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public virtual int CouponId { get; set; }

        /// <summary>
        /// 优惠券名称
        /// </summary>
        public virtual string CouponName { get; set; }

        /// <summary>
        /// 优惠券编号
        /// </summary>
        public virtual string CouponCode { get; set; }

        /// <summary>
        /// 面额
        /// </summary>
        public virtual decimal? Amount { get; set; }

        /// <summary>
        /// 面额币种
        /// </summary>
        public virtual int AmountCurrencyId { get; set; }

        /// <summary> 
        /// 金额对象
        /// </summary>
        public virtual AmountType? AmountType { get; set; }

        /// <summary>
        /// 启用金额
        /// </summary>
        public virtual decimal? MinAmount { get; set; }

        /// <summary>
        /// 启用金额币种
        /// </summary>
        public virtual int MinAmountCurrencyId { get; set; }

        /// <summary>
        /// 使用语种（多个）
        /// </summary>
        public virtual string LanguageIds { get; set; }

        /// <summary>
        /// 使用国家（多个）
        /// </summary>
        public virtual string CountryIds { get; set; }

        /// <summary>
        /// 使用币种（多个）
        /// </summary>
        public virtual string CurrencyIds { get; set; }

        /// <summary>
        /// 使用期限起始时间
        /// </summary>
        public virtual DateTime? LimitBeginTime { get; set; }

        /// <summary>
        /// 使用期限截止时间
        /// </summary>
        public virtual DateTime? LimitEndTime { get; set; }

        /// <summary>
        /// 使用期限类型
        /// </summary>
        public virtual LimitType LimitType { get; set; }

        /// <summary>
        /// 使用期限（天）
        /// </summary>
        public virtual int? LimitDay { get; set; }

        /// <summary>
        /// 是否允许手动领取
        /// </summary>
        public virtual bool AllowManualPick { get; set; }

        /// <summary>
        /// 领取期限起始时间
        /// </summary>
        public virtual DateTime? PickBeginTime { get; set; }

        /// <summary>
        /// 领取期限截止时间
        /// </summary>
        public virtual DateTime? PickEndTime { get; set; }

        /// <summary>
        /// 是否有效（标识启用 未启用）
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 每个客户领取次数限制
        /// </summary>
        public int? LimitCount { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 使用限制类型
    /// </summary>
    public enum LimitType
    {
        /// <summary>
        /// 按天计算
        /// </summary>
        Day,
        /// <summary>
        /// 按起始截止日期计算
        /// </summary>
        BeginEnd
    }
}
