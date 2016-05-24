using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal
{
    /// <summary>
    /// Paypal地址
    /// </summary>
    [Serializable]
    public class PayPalAddress
    { 
        public string AddressStatus { get; set; }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string Phone { get; set; }

        public string Zip { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Email { get; set; }

        private string _addressName;

        /// <summary>
        /// 用户全名
        /// </summary>
        public string AddressName
        {
            get
            {
                if (string.IsNullOrEmpty(_addressName))
                {
                    return string.Concat(FirstName ?? string.Empty, " ", LastName ?? string.Empty);
                }
                return _addressName;
            }
            set { _addressName = value; }
        }
    }
}