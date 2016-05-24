using System;
using Com.Panduo.Service;
using Com.Panduo.Service.Payment;
using NHibernate.Linq;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 ICategoryServiceTest 的测试类，旨在
    ///包含所有 ICategoryServiceTest 单元测试
    ///</summary>
    [TestFixture]
    public class PaymentServiceTest : SpringTest
    {  

        /// <summary>
        ///CanUseBankOfChina 的测试
        ///</summary>
        [Test()]
        public void CanUseBankOfChinaTest()
        {
            
        }

        /// <summary>
        ///CanUseGlobalCollect 的测试
        ///</summary>
        [Test()]
        public void CanUseGlobalCollectTest()
        { 

        }

        /// <summary>
        ///CanUseHsbc 的测试
        ///</summary>
        [Test()]
        public void CanUseHsbcTest()
        {
             
        }

        /// <summary>
        ///CanUseMoneyGram 的测试
        ///</summary>
        [Test()]
        public void CanUseMoneyGramTest()
        {
             
        }

        /// <summary>
        ///CanUseOceanPayment 的测试
        ///</summary>
        [Test()]
        public void CanUseOceanPaymentTest()
        {
             
        }

        /// <summary>
        ///CanUsePaypal 的测试
        ///</summary>
        [Test()]
        public void CanUsePaypalTest()
        {
            
        }

        /// <summary>
        ///CanUsePaypalExpress 的测试
        ///</summary>
        [Test()]
        public void CanUsePaypalExpressTest()
        {
            
        }

        /// <summary>
        ///CanUseWesternUnion 的测试
        ///</summary>
        [Test()]
        public void CanUseWesternUnionTest()
        {
           
        }

        /// <summary>
        ///GetBankOfChinaConfig 的测试
        ///</summary>
        [Test()]
        public void GetBankOfChinaConfigTest()
        { 
            var list = ServiceFactory.PaymentService.GetBankOfChinaInfo("1503058954");

            Console.WriteLine(list.Count);
        }

        /// <summary>
        ///GetGlobalCollectConfig 的测试
        ///</summary>
        [Test()]
        public void GetGlobalCollectConfigTest()
        {
            
        }

        /// <summary>
        ///GetGlobalCollectOrderNo 的测试
        ///</summary>
        [Test()]
        public void GetGlobalCollectOrderNoTest()
        {
        }

        /// <summary>
        ///GetHsbcConfig 的测试
        ///</summary>
        [Test()]
        public void GetHsbcConfigTest()
        {
             
        }

        /// <summary>
        ///GetMoneyGramConfig 的测试
        ///</summary>
        [Test()]
        public void GetMoneyGramConfigTest()
        {
            var aa = ServiceFactory.PaymentService.GetPaypalConfig();
        }

        /// <summary>
        ///GetOceanPaymentConfig 的测试
        ///</summary>
        [Test()]
        public void GetOceanPaymentConfigTest()
        {
             
        }

        /// <summary>
        ///GetPaypalConfig 的测试
        ///</summary>
        [Test()]
        public void GetPaypalConfigTest()
        {
            
        }

        /// <summary>
        ///GetPaypalExpressConfig 的测试
        ///</summary>
        [Test()]
        public void GetPaypalExpressConfigTest()
        {
            
        }

        /// <summary>
        ///GetWesternUnionConfig 的测试
        ///</summary>
        [Test()]
        public void GetWesternUnionConfigTest()
        {
            
        }

        /// <summary>
        /// 得到GC屏蔽国家
        /// </summary>
        [Test()]
        public void GetGlobalCollectDisabledCountryById()
        {
            var vo = ServiceFactory.PaymentService.GetGlobalCollectDisabledCountryById(30);
            Console.WriteLine(vo.DateCreated);
        }

        /// <summary>
        /// 是否是GC屏蔽国家
        /// </summary>
        [Test()]
        public void IsGlobalCollectDisabledCountry()
        {
            var disabled = ServiceFactory.PaymentService.IsGlobalCollectDisabledCountry(30);
            Console.WriteLine(disabled);
        }

        /// <summary>
        /// 添加GC屏蔽国家
        /// </summary>
        [Test()]
        public void AddGlobalCollectDisabledCountry()
        {
            var vo = new GlobalCollectDisabledCountry
            {
                CountryId = 1,
                DateCreated = DateTime.Now,
                AdminId = 1
            };
            ServiceFactory.PaymentService.AddGlobalCollectDisabledCountry(vo);
        }

        /// <summary>
        /// 删除GC屏蔽国家
        /// </summary>
        [Test()]
        public void DeleteGlobalCollectDisabledCountryById()
        {
            ServiceFactory.PaymentService.DeleteGlobalCollectDisabledCountryById(1);
        }

        /// <summary>
        /// 查找GC屏蔽国家
        /// </summary>
        [Test()]
        public void FindGlobalCollectDisabledCountries()
        {
            var list = ServiceFactory.PaymentService.FindGlobalCollectDisabledCountries(1, 2, null, null);
            foreach (var value in list.Data)
            {
                Console.WriteLine(string.Format("{0}->{1}", value.CountryId, value.DateCreated));
            }
        }
    }
}
