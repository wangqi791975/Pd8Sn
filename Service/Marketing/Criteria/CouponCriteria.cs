using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.Criteria
{
    public class CouponCriteria : MarketingCriteria
    {
        public virtual int? CustomerId { get; set; }
        public virtual int RewardType { get; set; }
    }
}
