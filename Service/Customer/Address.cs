using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 用户地址簿
    /// </summary>
    [Serializable]
    public class Address
    {
        /// <summary>
        /// 地址Id
        /// </summary>
        public virtual int AddressId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Telphone { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public virtual int Country { get; set; }

        /// <summary>
        /// 省/州ID
        /// </summary>
        public virtual int? ProvinceId { get; set; }

        /// <summary>
        /// 省/州
        /// </summary>
        public virtual string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string ZipCode { get; set; }

        /// <summary>
        /// 街道1
        /// </summary>
        public virtual string Street1 { get; set; }

        /// <summary>
        /// 街道2
        /// </summary>
        public virtual string Street2 { get; set; }
    }
}
