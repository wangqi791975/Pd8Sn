using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.Order.ShoppingCart;
using NUnit.Framework;

using Com.Panduo.Common;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class OrderServiceTest : SpringTest
    {
        /// <summary>
        /// 客户下单
        /// </summary>
        [Test]
        public void TestPlaceOrderByCustomer()
        {
            var checkoutDraft = new CheckoutDraft
            {
                ShoppingCartId = 48,
                ClubLevel = 0, //1、2、3、4、5
                ReceivingAddressId = 4,
                BillAddressId = 5,
                ShippingId = 30,
                OrderSource = 0,
                LanguageCode = "en", //jp
                CurrencyCode = "EUR", //JPY
                ReportCurrencyCode = "EUR", //JPY
                ReportProductMoney = 30m,
                ReportShippingMoney = 5m,
                CustomsNoType = CustomsNoType.CnpjNo,
                CustomsNoNumber = "N525968585363",
                OrderRemark = "Please quickly",
                OutOfStockWaitType = OutOfStockWaitType.SendPart,
                CouponCustomerId = 0
            };
            /*
            var shoppingCartSummary = new ShoppingCartSummary
            {
                ShoppingCartId = 48,
                OriginalProductAmount = 536,
                NoDiscountProductAmount = 300,
                PromotionDiscountAmount = 100,
                VipDiscount = 0m,
                VipDiscountAmount = 0m,
                VipAndRcdDiscount = 0,
                VipAndRcdDiscountAmount = 0,
                OrderDiscount = 0m,
                OrderDiscountAmount = 0m,
                DiscountType = DiscountType.NoDiscount,
                TotalQuantity = 10,
                GrossWeight = 3596m,
                VolumeWeight = 4596m,
                ShippingWeight = 4596m,
                PackageWeight = 0m,
                ClubWeight = 0m,
                GrandTotal = 936

            };
            */
            string result = ServiceFactory.OrderService.PlaceOrderByCustomer(checkoutDraft);
            Console.WriteLine(result);
        }
        [Test]
        [Category("OrderStatus")]
        public void TestGetAllOrderStatuses()
        {
            int c = ServiceFactory.OrderService.GetAllOrderStatuses().Count;
            Assert.AreEqual(true, c == 28);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [Category("OrderStatus")]
        public void TestGetAllCustomerOrderStatus(int languageId)
        {
            int c = ServiceFactory.OrderService.GetAllCustomerOrderStatus(languageId).Count;
            Assert.AreEqual(true, c == 4);
        }

        [Test]
        public void TestGetOrderDetsById()
        {
            var test = ServiceFactory.OrderService.GetOrderDetsById(2, 9, 1, 10, null);
            Assert.AreEqual(true, test.Pager.TotalRowCount > 0);
        }

        [Test]
        public void TestGetOrderDetsById1()
        {
            var c = ServiceFactory.OrderService.GetOrderDetsById(2, 10);
            Assert.AreNotEqual(null,c);
        }


        [Test]
        public void TestGetOrderDetsById2()
        {
            var c = ServiceFactory.OrderService.GetOrderDetsById(12, 10);
            Assert.AreEqual(null, c);
        }

        [Test]
        public void TestGetOrdersByCustomerId()
        {  
            IDictionary<OrderSearchCriteria,object> dic=new Dictionary<OrderSearchCriteria, object>();
            var c = ServiceFactory.OrderService.GetOrdersByCustomerId(2, 1, 10, dic, null);
            Assert.AreEqual(true, c.Pager.StartRowNumber>0);

        }


        public void TestGet()
        {

        }
    }
}
