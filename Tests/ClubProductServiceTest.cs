using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    public class ClubProductServiceTest : SpringTest
    {
        [Test]
        public void AddClubTest()
        {
           // var s = ServiceFactory.ProductService.ClubProductService;
        }

        [Test]
        public void FindAllClubProducts()
        {
            var a =ServiceFactory.ClubProductService.FindAllClubProducts(1, 20, null, null);
        }
    }
}
