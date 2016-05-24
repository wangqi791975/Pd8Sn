using System;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class NewsletterService : SpringTest
    {
        [Test]
        public void TestMethod1()
        {

            var v = ServiceFactory.NewsletterService;
            var newsletter = new Newsletter
           {
               CustomerId = 0,
               FullName = "customer",
               Email = "123456789@qq.com",
               LanguageId = 2,
               NewsletterDateTime = DateTime.Now,
           };
            //订阅  (功能测试通过)
            v.Subscribe(newsletter);
        }
    }
}