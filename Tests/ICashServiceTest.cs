using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.ServiceImpl.SiteConfigure;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 ICashServiceTest 的测试类
    /// </summary>
    [TestFixture()]
    public class ICashServiceTest : SpringTest
    {
        /// <summary>
        /// 获取单个配送方式
        /// </summary>
        [Test]
        public void HasCashAccount()
        {
            bool has = ServiceFactory.CashService.HasCashAccount(2);
            Console.WriteLine(has);
        }

        /// <summary>
        /// 创建CashAccount
        /// </summary>
        [Test]
        public void AddCashAccount()
        {
            int customerId = 6;
            ServiceFactory.CashService.AddCashAccount(customerId, "GBP");
            CashAccount cashAccount = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("CurrencyCode:{0}, Amount:{1}", cashAccount.CurrencyCode, cashAccount.Amount));
        }

        /// <summary>
        /// 得到客户CashAccount
        /// </summary>
        [Test]
        public void GetCashAccount()
        {
            CashAccount cashAccount = ServiceFactory.CashService.GetCashAccount(3);
            Console.WriteLine(string.Format("CurrencyCode:{0}, Amount:{1}", cashAccount.CurrencyCode, cashAccount.Amount));
        }

        /// <summary>
        /// 获取客户美元余额(小于0返回0)
        /// </summary>
        [Test]
        public void GetCustomerBalance()
        {
            decimal balance = ServiceFactory.CashService.GetCustomerBalance(5);
            Console.WriteLine(balance);
        }

        /// <summary>
        ///  获取客户美元欠款(如果大于0返回0，小于0返回正值)
        /// </summary>
        [Test]
        public void GetCustomerArrear()
        {
            decimal balance = ServiceFactory.CashService.GetCustomerArrear(5);
            Console.WriteLine(balance);
        }

        /// <summary>
        /// 充值
        /// </summary>
        [Test]
        public void Recharge()
        {
            int customerId = 2;
            decimal balance = 1m;
            string currency = "GBP";

            CashAccount cashAccountOld = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("充值【前】币种为:{0},金额为:{1}", cashAccountOld.CurrencyCode, cashAccountOld.Amount));
            ServiceFactory.CashService.Recharge(customerId, currency, balance, DateTime.Now.ToString());
            CashAccount cashAccountNew = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("充值【后】币种为:{0},金额为:{1}", cashAccountNew.CurrencyCode, cashAccountNew.Amount));

        }

        /// <summary>
        /// 提现
        /// </summary>
        [Test]
        public void Withdrawal()
        {
            int customerId = 6;
            decimal balance = 185m;
            string currency = "GBP";

            CashAccount cashAccountOld = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("提现【前】币种为:{0},金额为:{1}", cashAccountOld.CurrencyCode, cashAccountOld.Amount));
            ServiceFactory.CashService.Withdrawal(customerId, currency, balance, DateTime.Now.ToString());
            CashAccount cashAccountNew = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("提现【后】币种为:{0},金额为:{1}", cashAccountNew.CurrencyCode, cashAccountNew.Amount));
        }

        /// <summary>
        /// 归还欠款
        /// </summary>
        [Test]
        public void ReturnArrear()
        {
            int customerId = 2;
            decimal balance = 50m;
            string currency = "JPY";

            CashAccount cashAccountOld = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("归还欠款【前】币种为:{0},金额为:{1}", cashAccountOld.CurrencyCode, cashAccountOld.Amount));
            ServiceFactory.CashService.ReturnArrear(customerId, currency, balance, DateTime.Now.ToString());
            CashAccount cashAccountNew = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("归还欠款【后】币种为:{0},金额为:{1}", cashAccountNew.CurrencyCode, cashAccountNew.Amount));
        }

        /// <summary>
        /// Cash支付
        /// </summary>
        [Test]
        public void CashPay()
        {
            int result = ServiceFactory.CashService.CashPay(78, 3615, "EUR", 0.54M);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取客户的Cash明细
        /// </summary>
        [Test]
        public void FindAllCashItems()
        {
            int customerId = 2;
            int currentPage = 1;
            int pageSize = 20;

            Service.PageData<CashItem> pageData = ServiceFactory.CashService.FindAllCashItems(customerId, currentPage, pageSize);
            Console.WriteLine(pageData.Pager.TotalRowCount);
        }

        /// <summary>
        /// 充值
        /// </summary>
        [Test]
        public void AdminRecharge()
        {
            int customerId = 2;
            decimal balance = 103m;
            string currency = "GBP";

            CashAccount cashAccountOld = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("充值【前】币种为:{0},金额为:{1}", cashAccountOld.CurrencyCode, cashAccountOld.Amount));
            ServiceFactory.CashService.AdminRecharge(customerId, currency, balance, DateTime.Now.ToString(), 1, true);
            CashAccount cashAccountNew = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("充值【后】币种为:{0},金额为:{1}", cashAccountNew.CurrencyCode, cashAccountNew.Amount));

        }

        /// <summary>
        /// 提现
        /// </summary>
        [Test]
        public void AdminWithdrawal()
        {
            int customerId = 6;
            decimal balance = 185m;
            string currency = "JPY";

            CashAccount cashAccountOld = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("提现【前】币种为:{0},金额为:{1}", cashAccountOld.CurrencyCode, cashAccountOld.Amount));
            ServiceFactory.CashService.AdminWithdrawal(customerId, currency, balance, DateTime.Now.ToString(), 1, true);
            CashAccount cashAccountNew = ServiceFactory.CashService.GetCashAccount(customerId);
            Console.WriteLine(string.Format("提现【后】币种为:{0},金额为:{1}", cashAccountNew.CurrencyCode, cashAccountNew.Amount));
        }

        /// <summary>
        /// 管理员获取所有客户的Cash明细
        /// </summary>
        [Test]
        public void AdminFindAllCashItems()
        {
            int currentPage = 1;
            int pageSize = 20;

            IDictionary<CashItemSearchCriteria, object> searchCriteria = new Dictionary<CashItemSearchCriteria, object>
            {
                {CashItemSearchCriteria.Keyword, "nice"}
            };
            Service.PageData<CashItem> pageData = ServiceFactory.CashService.AdminFindAllCashItems(currentPage, pageSize, searchCriteria, null);
            Console.WriteLine(pageData.Pager.TotalRowCount);
            foreach (var item in pageData.Data)
            {
                Console.WriteLine(string.Format("客户邮箱:{0},余额:{1}，操作时间:{2}", item.CustomerEmail, item.AmountLeft, item.OpDate));
            }
        }
    }
}
