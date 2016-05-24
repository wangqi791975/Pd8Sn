using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using NUnit.Framework;
namespace Com.Panduo.Tests
{
    public class CouponServiceTest : SpringTest
    {
        public Coupon NewCoupon
        {
            get
            {
                return new Coupon
                {
                    CouponCode = "00003",
                    Amount = 100M,
                    AmountCurrencyId = 1,
                    AmountType = AmountType.TotalAmount,
                    MinAmount = 1000M,
                    MinAmountCurrencyId = 1,
                    LanguageIds = "1,2,3",
                    CountryIds = "1,2,3",
                    CurrencyIds = "1,2,3",
                    LimitBeginTime = DateTime.Now,
                    LimitEndTime = DateTime.Now.AddDays(1000),
                    LimitType = LimitType.Day,
                    LimitDay = 500,
                    AllowManualPick = true,
                    PickBeginTime = DateTime.Now,
                    PickEndTime = DateTime.Now.AddDays(100),
                    Status = 2,
                    LimitCount = 2,
                    TotalCount = 1000,
                };
            }
        }

        public Coupon EditCoupon
        {
            get
            {
                return new Coupon
                {
                    CouponId = 2,
                    CouponCode = "00002",
                    Amount = 100M,
                    AmountCurrencyId = 1,
                    AmountType = AmountType.TotalAmount,
                    MinAmount = 1000M,
                    MinAmountCurrencyId = 1,
                    LanguageIds = "1,2,3",
                    CountryIds = "1,2,3",
                    CurrencyIds = "1,2,3",
                    LimitBeginTime = DateTime.Now,
                    LimitEndTime = DateTime.Now.AddDays(1000),
                    LimitType = LimitType.Day,
                    LimitDay = 500,
                    AllowManualPick = true,
                    PickBeginTime = DateTime.Now,
                    PickEndTime = DateTime.Now.AddDays(100),
                    Status = 2,
                    LimitCount = 2,
                    TotalCount = 1000,
                };
            }
        }

        [Test]
        public void CreateCouponTest()
        {
            int id = ServiceFactory.CouponService.CreateCoupon(NewCoupon, new List<CouponDesc>());
        }

        [Test]
        public void EditCouponTest()
        {
            ServiceFactory.CouponService.EditCoupon(EditCoupon, new List<CouponDesc>());
        }

        [Test]
        public void SendCouponTest()
        {
            ServiceFactory.CouponService.SendCoupon(2, 2, 2);
        }

        [Test]
        public void SendCouponsTest()
        {
            ServiceFactory.CouponService.SendCoupon(2, new int[] { 2, 3 }.ToList(), 2);
        }

        [Test]
        public void FindAllCouponTest()
        {
            ServiceFactory.CouponService.FindAllCoupon(1, 2, null, null);
        }

        [Test]
        public void GetCouponTest()
        {
            var coupon = ServiceFactory.CouponService.GetCoupon(2);
        }

        [Test]
        public void GetCouponByCodeTest()
        {
            var coupon = ServiceFactory.CouponService.GetCoupon("00002");
        }

        [Test]
        public void GetCouponDescTest()
        {
            var couponDesc = ServiceFactory.CouponService.GetCouponDesc(2);
        }

        [Test]
        public void GetCouponDescByLanguageIdTest()
        {
            var coupondesc = ServiceFactory.CouponService.GetCouponDesc(2, 1);
        }

        [Test]
        public void PickCustomerCouponTest()
        {
            ServiceFactory.CouponService.PickCustomerCoupon("r", 2, 1, 224, 1);
        }

        [Test]
        public void GetUsableCouponsTest()
        {
            var customerCoupon = ServiceFactory.CouponService.GetUsableCoupons(2,
                new Dictionary<AmountType, decimal>
                {
                    {AmountType.NormalAmount, 50M},
                    {AmountType.TotalAmount, 49M}
                }, 1, 4, 3);
        }

        [Test]
        public void IsCouponUsableTest()
        {
            bool a = ServiceFactory.CouponService.IsCouponUsable(13, 2, new Dictionary<AmountType, decimal>
            {
                {AmountType.NormalAmount, 101M},
                {AmountType.TotalAmount, 101m}
            }, 1, 2, 3);
        }

        [Test]
        public void UseCouponTest()
        {
            ServiceFactory.CouponService.UseCoupon(13, 2, 1, new Dictionary<AmountType, decimal>
            {
                {AmountType.NormalAmount, 101M},
                {AmountType.TotalAmount, 101m}
            }, 1, 2, 3);
        }

        [Test]
        public void GetCustomerCouponTest()
        {
            var couponCustomer = ServiceFactory.CouponService.GetCustomerCoupon(13);
            var couponCustomers = ServiceFactory.CouponService.GetCustomerCoupons(2);
        }

        [Test]
        public void FindAllCustomerCouponTest()
        {
            var a = ServiceFactory.CouponService.FindAllCustomerCoupon(1, 3, null, null);
        }

        [Test]
        public void FindCouponMarketings()
        {
            var a = ServiceFactory.MarketingService.FindCouponMarketings(1, 20, null, null);
        }

        [Test]
        public void FindAllCustomerCouponView()
        {
            int status = 1;
            string couponName = "额";
            string emailId = "";
            string orderCode = "";
            int leftDay = 1;
            int page = 1;
            int pageSize = 20;

            var searchCriteria = new Dictionary<CustomerCouponSearchCriteria, object>();
            if (status != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.Status, status);
            }
            if (couponName != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.CouponName, couponName);
            }
            if (emailId != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.EmailId, emailId);
            }
            if (orderCode != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.OrderCode, orderCode);
            }
            var sorterCriteria = new List<Sorter<CustomerCouponSorterCriteria>>();
            if (leftDay != null)
            {
                sorterCriteria.Add(new Sorter<CustomerCouponSorterCriteria>(CustomerCouponSorterCriteria.LeftDay, leftDay == 1));
            }
            var couponCustomerView = ServiceFactory.CouponService.FindAllCustomerCouponView(page, pageSize, searchCriteria, sorterCriteria);
        }
        [Test]
        public void FindMyCustomerCoupon()
        {
            var a = ServiceFactory.CouponService.FindMyCustomerCoupon(78, 1, 20,
                 new Dictionary<CustomerCouponSearchCriteria, object> { { CustomerCouponSearchCriteria.InActiveCoupon, "" } }, null);
        }
    }
}