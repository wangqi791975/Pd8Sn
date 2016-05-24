using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment.PayConfig
{
    /// <summary>
    /// 中国银行配置
    /// </summary>
    [Serializable]
    public class BankOfChinaConfig
    { 
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public virtual string AccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string AccountNumber { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public virtual string BankName { get; set; }
        /// <summary>
        /// Swift Code
        /// </summary>
        public virtual string SwiftCode { get; set; }
        /// <summary>
        /// 银行地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; } 
    }
}
