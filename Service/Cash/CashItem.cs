using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Cash
{
    /// <summary>
    /// 客户Cash明细
    /// </summary>
    public class CashItem
    {
        /// <summary>
        /// 唯一标识ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 客户full name
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 客户Email
        /// </summary>
        public virtual string CustomerEmail {  get;set; }

        /// <summary>
        /// 收入金额
        /// </summary>
        public virtual decimal AmountIn { get; set; }

        /// <summary>
        /// 支出金额
        /// </summary>
        public virtual decimal AmountOut { get; set; }

        /// <summary>
        /// 当时汇率
        /// </summary>
        public virtual decimal ExchangeRate { get; set; }

        /// <summary>
        /// 当时币种
        /// </summary>
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// 操作之后的余额
        /// </summary>
        public virtual decimal AmountLeft { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual OperationType OpType { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime OpDate { get; set; }

        /// <summary>
        /// 操作管理员ID
        /// </summary>
        public virtual int OpAdmin { get; set; }

        /// <summary>
        /// 操作人Email
        /// </summary>
        public virtual string OpAccountEmail { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Comment { get; set; }

    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 收入
        /// </summary>
        Income = 10,

        /// <summary>
        /// 支出
        /// </summary>
        Payout = 20
    }
}
