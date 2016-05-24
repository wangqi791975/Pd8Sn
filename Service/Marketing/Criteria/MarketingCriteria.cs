using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.Criteria
{
    public class MarketingCriteria
    {
        public virtual string CountryIsoCode2 { get; set; }
        /// <summary>
        /// 客户VIP等级 ID
        /// </summary>
        public virtual int VipLevel { get; set; }
        public virtual int? LanguageId { get; set; }
        
    }
}
