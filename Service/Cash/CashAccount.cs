using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Cash
{
    /// <summary>
    /// 客户Cash(一对一客户信息)
    /// </summary>
    public class CashAccount
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
        /// 客户Email
        /// </summary>
        public virtual string CustomerEmail { get; set;  }

        /// <summary>
        /// 余额
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Comment { get; set; }
    }
}
