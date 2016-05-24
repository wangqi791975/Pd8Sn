using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment
{
    /// <summary>
    /// 网站支付类型
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// Paypal支付
        /// </summary>
        Paypal = 0,
        /// <summary>
        /// HSBC银行转账
        /// </summary>
        Hsbc = 1,
        /// <summary>
        /// 中国银行转账
        /// </summary>
        BankOfChina=2,
        /// <summary>
        /// 西联汇款
        /// </summary>
        WesternUnion = 4,
        /// <summary>
        /// GC信用卡
        /// </summary>
        Gc=8,
        /// <summary>
        /// MoneyGram汇款
        /// </summary>
        MoneyGram =16,
        /// <summary>
        /// Webmoney支付
        /// </summary>
        Webmoney=32,
        /// <summary>
        /// Yandex支付
        /// </summary>
        Yandex=64,
        /// <summary>
        /// QiWi支付
        /// </summary>
        QiWi=128,
        /// <summary>
        /// 钱海信用卡支付
        /// </summary>
        OceanCreditCard=256,
        /// <summary>
        /// Cash全额支付
        /// </summary>
        Cash = 10000
    }
}
