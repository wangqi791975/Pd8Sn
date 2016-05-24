using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.GlobalCollect
{
    [Serializable]
    public class GlobalCollectBillingAddress
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 州
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 国家缩写(2位)
        /// </summary>
        public string CountryCode { set; get; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { set; get; }
        /// <summary>
        /// 税号
        /// </summary>
        public string VatCode { set; get; } 
    }
}
