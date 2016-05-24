using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.OceanPayment
{
    [Serializable]
    public class OceanPaymentShippingAddress
    {
        /// <summary>
        /// 姓
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// 名
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// 国家简码
        /// </summary>
        public virtual string CountryCode { get; set; }
        /// <summary>
        /// 州
        /// </summary>
        public virtual string State { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual string City { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string Zip { get; set; } 
    }
}