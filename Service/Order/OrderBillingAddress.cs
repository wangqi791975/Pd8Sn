using System;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 客户账单地址
    /// </summary>
    [Serializable]
    public class OrderBillingAddress
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OrderId { get; set; }
        
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
        /// 国家名称
        /// </summary>
        public virtual string CountryName { get; set; }

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
