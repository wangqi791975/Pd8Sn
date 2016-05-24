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
    public class ClubCustomerView
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
        /// 客户邮箱
        /// </summary>
        public virtual string CustomerEmail { get; set; }

        /// <summary>
        /// 客户等级
        /// </summary>
        public virtual string CustomerLevel { get; set; }

        /// <summary>
        /// 客服经理名称
        /// </summary>
        public virtual string ManagerName { get; set; }

        /// <summary>
        /// transaction_id
        /// </summary>
        public virtual string TransactionId
        {
            get;
            set;
        }

        /// <summary>
        /// website
        /// </summary>
        public virtual string Website
        {
            get;
            set;
        }
    }

}
