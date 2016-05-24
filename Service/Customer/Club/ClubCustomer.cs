using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Payment;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// Club会员
    /// </summary>
    [Serializable]
    public class ClubCustomer
    {
        /// <summary>
        /// ClubId
        /// </summary>
        public virtual int ClubId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 客服Id
        /// </summary>
        public virtual int CustomerManagerId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public virtual DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public virtual DateTime DateActived { get; set; }

        /// <summary>
        /// 会员费
        /// </summary>
        public virtual decimal Fee { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public virtual PaymentType PayType { get; set; }

        /// <summary>
        /// 支付日志Id
        /// </summary>
        public virtual int PayLogId { get; set; }

        /// <summary>
        /// Club类型
        /// </summary>
        public virtual ClubType ClubType { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public virtual PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// 会员状态
        /// </summary>
        public virtual ClubStatus ClubStatus { get; set; }

        /// <summary>
        /// 最后一次加入Club节省运费
        /// </summary>
        public virtual decimal SavingShippingFee { get; set; }
    }

    /// <summary>
    /// club会员类型（一期，二期）
    /// </summary>
    public enum ClubType
    {
        /// <summary>
        /// 一期
        /// </summary>
        One = 1,
        /// <summary>
        /// 二期
        /// </summary>
        Two = 2
    }

    /// <summary>
    /// 会员状态
    /// </summary>
    public enum ClubStatus
    {
        /// <summary>
        /// 激活
        /// </summary>
        Active = 1,
        /// <summary>
        /// 表示已享受过club政策
        /// </summary>
        InUse = 2,
        /// <summary>
        /// 到期
        /// </summary>
        DueTo = 3
    }

    public enum PaymentStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        NonPayment,
        /// <summary>
        /// 已支付
        /// </summary>
        Payment,
    }
}
