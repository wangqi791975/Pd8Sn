using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Panduo.Service.Coupon
{
    /// <summary>
    /// 客户优惠券表
    /// </summary>
    [Serializable]
    public class CouponCustomer
    {
        /// <summary>
        /// 自增Id，自动生成的Coupon编号
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// CouponId
        /// </summary>
        public virtual int CouponId { get; set; }

        /// <summary>
        /// Coupon编号
        /// </summary>
        public virtual string CouponCode { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 面额
        /// </summary>
        public virtual decimal? Amount { get; set; }

        /// <summary>
        /// 面额币种
        /// </summary>
        public virtual int? AmountCurrencyId { get; set; }

        #region 使用条件
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
        public virtual int? MinAmountCurrencyId { get; set; }

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
        public virtual DateTime LimitBeginTime { get; set; }

        /// <summary>
        /// 使用期限截止时间
        /// </summary>
        public virtual DateTime LimitEndTime { get; set; }
        #endregion

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime { get; set; }

        /// <summary>
        /// 使用说明
        /// </summary>
        public string UseDescribe { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 来源说明
        /// </summary>
        public string SourceDescribe { get; set; }

        /// <summary>
        /// 完全状态
        /// </summary>
        public CouponStatus Status { get; set; }

        /// <summary>
        /// 状态变更原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 停用时间
        /// </summary>
        public virtual DateTime? DateDisabled { get; set; }

        /// <summary>
        /// 管理员id
        /// </summary>
        public virtual int? AdminId { get; set; }

        /// <summary>
        /// coupon描述
        /// </summary>
        public virtual Dictionary<int, CouponDesc> CouponDescs
        {
            get; set;
        }
    }

    /// <summary>
    /// Coupon状态
    /// </summary>
    public enum CouponStatus
    {
        /// <summary>
        /// 未使用
        /// </summary>
        NotUsed = 1,
        /// <summary>
        /// 使用
        /// </summary>
        Used = 2,
        /// <summary>
        /// 销售使用
        /// </summary>
        MarketingUsed = 3,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 过期
        /// </summary>
        PassDue = 5,
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 6
    }
}