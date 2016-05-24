using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment
{
    /// <summary>
    /// 支付方式 屏蔽 客户
    /// </summary>
    [Serializable]
    public class PaymentEnabledCustomer
    {

        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 支付类型:Paypal = 0,Hsbc = 1,BankOfChina=2,WesternUnion = 4,Gc=8,MoneyGram =16,Webmoney=32,Yandex=64,QiWi=128,OceanCreditCard=256
        /// </summary>
        public virtual PaymentType PaymentType
        {
            get;
            set;
        }
        /// <summary>
        /// 禁用对应的客户ID 不一定有值，可能添加的用户邮箱不是注册客户
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 禁用对应的客户邮箱
        /// </summary>
        public virtual string CustomerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员账户
        /// </summary>
        public virtual string AccountEmail
        {
            get;
            set;
        }
    }
}
