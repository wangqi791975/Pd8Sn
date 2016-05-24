using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.Criteria
{
    public class PlaceOrderCriteria : MarketingCriteria
    {
        public virtual int CustomerId { get; set; }
        public virtual int ClubLevel { get; set; }
        /// <summary>
        /// 是否渠道商
        /// </summary>
        public virtual bool IsChannel { get; set; }
        /// <summary>
        /// 当前客户下单选择的站点币种
        /// </summary>
        public virtual int CurrencyId { get; set; }
        /// <summary>
        /// 必须是美元
        /// </summary>
        public virtual decimal TotalAmount { get; set; }
        /// <summary>
        /// 必须是美元
        /// </summary>
        public virtual decimal NormalAmount { get; set; }
    }
}
