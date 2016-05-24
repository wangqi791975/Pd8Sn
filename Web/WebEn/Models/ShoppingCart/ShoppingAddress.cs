using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.Web.Models.ShoppingCart
{
    public class ShoppingAddress
    {
        /// <summary>
        /// 当前语种下所有地址列表
        /// </summary>
        public IList<CountryLanguage> CountryLanguages { get; set; }

        /// <summary>
        /// 当前语种下所有地址列表(常用)
        /// </summary>
        public IList<CountryLanguage> CommonCountryLanguages { get; set; }

        /// <summary>
        /// 用户当前地址
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// 当前国家
        /// </summary>
        public CountryLanguage CountryLanguage { get; set; }

        /// <summary>
        /// 是否默认选中
        /// </summary>
        public string IsCheckedAddress { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public string IsDisabledAddress { get; set; }
    }
}