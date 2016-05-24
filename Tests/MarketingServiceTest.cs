
using System;
using System.Collections.Generic;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Marketing.Criteria;
using NUnit.Framework;
using Com.Panduo.Service;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.SEO;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class MarketingServiceTest : SpringTest
    {

        /// <summary>
        /// 后台查询免运费列表，测试通过
        /// </summary>
        [Test]
        public void FindShippingMarketings()
        {
            var a = ServiceFactory.MarketingService.FindShippingMarketings(1, 2, new Dictionary<MarketingSearchCriteria, object>(), new List<Sorter<MarketingSorterCriteria>>());
        }

        [Test]
        public void SendCouponCodeForRegister()
        {
            var a =
                ServiceFactory.MarketingService.SendCouponCodeForRegister(new CouponCriteria
                {
                    CustomerId = 2,
                    LanguageId = 1,
                    RewardType = (int)CouponMarketingRewardType.Register
                });
        }
    }
}
