﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment.PayInfo
{
    /// <summary>
    /// 银行转账支付信息
    /// </summary>
    [Serializable]
    public class BankOfChinaInfo : BasePayInfo
    {
        /// <summary>
        /// 表自增长ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 支付目标对象ID(订单ID)
        /// </summary>
        public virtual int TargetId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNo { get; set; }
        /// <summary>
        /// 是否标准币种
        /// </summary>
        public virtual bool IsStandardCurrency { get; set; }
        /// <summary>
        /// 币种编码，比如USD,JPY
        /// </summary>
        public virtual string CurrencyCode { get; set; }
        /// <summary>
        /// 转账金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 转账日期
        /// </summary>
        public virtual DateTime PaymentDate { get; set; }
        /// <summary>
        /// 转账凭证,存储凭证附件的相对路径
        /// </summary>
        public virtual string PaymentReceipt { get; set; }
    }
}
