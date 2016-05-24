using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Payment;
using NUnit.Framework;
namespace Com.Panduo.Tests
{
    public class ClubServiceTest : SpringTest
    {
        public ClubCustomer ClubCustomer
        {
            get
            {
                return new ClubCustomer
                    {
                        CustomerId = 2,
                        CustomerManagerId = 0,
                        BeginDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(365),
                        DateActived = DateTime.Now.AddDays(1),
                        Fee = 299,
                        PayType = PaymentType.Gc,
                        ClubType = ClubType.Two,
                        PaymentStatus = PaymentStatus.NonPayment,
                        ClubStatus = ClubStatus.Active,
                        SavingShippingFee = 0M
                    };
            }
        }

        [Test]
        public void AddClubTest()
        {
            var a = ServiceFactory.ClubService.AddClub(ClubCustomer);
        }

        [Test]
        public void GetClubLevelTest()
        {
            var a = ServiceFactory.ClubService.GetClubLevel(2);
        }

        [Test]
        public void ConfirmPaymentStatusTest()
        {
            ServiceFactory.ClubService.ConfirmPaymentStatus(1, 1);
            ServiceFactory.ClubService.ConfirmPaymentStatus(new[] { 1 }.ToList(), 1);
        }

        [Test]
        public void GetClubCustomerTest()
        {
            var a = ServiceFactory.ClubService.GetClubCustomer(1);
        }

        [Test]
        public void GetClubByCustomerIdTest()
        {
            var a = ServiceFactory.ClubService.GetClubByCustomerId(2);
        }

        [Test]
        public void FindClubCustomerTest()
        {
            var a = ServiceFactory.ClubService.FindClubCustomer(1, 2, null, null);
        }

        [Test]
        public void SetClubCustomerManagerTest()
        {
            ServiceFactory.ClubService.SetClubCustomerManager(1, 3, 2);
        }

        [Test]
        public void SetClubCustomerManagersTest()
        {
            ServiceFactory.ClubService.SetClubCustomerManager(new[] { 1 }.ToList(), 22, 2);
        }

        [Test]
        public void GetClubShippingFeeTest()
        {
            var a = ServiceFactory.ClubService.GetClubShippingFee(2);
        }
    }
}