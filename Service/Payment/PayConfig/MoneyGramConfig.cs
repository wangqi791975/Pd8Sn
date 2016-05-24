using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment.PayConfig
{
    /// <summary>
    /// MoneyGram配置
    /// </summary>
    [Serializable]
    public class MoneyGramConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// FirstName
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string ZipCode { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual string City { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public virtual string Country { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; }
    }
}
