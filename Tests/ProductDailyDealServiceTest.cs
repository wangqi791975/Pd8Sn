using System;
using System.Collections.Generic;
using Com.Panduo.Service;
using NUnit.Framework;
using Com.Panduo.Service.Product.DailyDeal;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 IProductDailyDealServiceTest 的测试类，旨在
    ///包含所有 IProductDailyDealServiceTest 单元测试
    ///</summary>
    [TestFixture]
    public class ProductDailyDealServiceTest : SpringTest
    {
        /// <summary>
        ///GetProductArea 的测试
        ///</summary>
        [Test]
        public void FindProductDailyDealTest()
        {
            int currentPage = 1, pageSize = 10;
            var searchCriteria = new Dictionary<ProductDailyDealSearchCriteria, object>
                {
                    {ProductDailyDealSearchCriteria.ProductCode, "B000"},
                    //{ProductDailyDealSearchCriteria.ProductName,""}
                };
            IList<Sorter<ProductDailyDealSorterCriteria>> sorterCriteria = null;
            var pageData = ServiceFactory.ProductDailyDealService.FindProductDailyDeals(currentPage, pageSize, searchCriteria, sorterCriteria);

            //Console.WriteLine(pageData.Pager.TotalRowCount);
            //Assert.AreEqual(10, pageData.Data.Count);
        }

        /// <summary>
        ///AddProductArea 的测试
        ///</summary>
        [Test]
        public void AddProductDailyDealTest()
        {
            try
            {
                var lstProductAreaLanguage = new List<ProductDailyDeal> { 
                new ProductDailyDeal { 
                    ProductId = 1,
                    EndDateTime = DateTime.Now.AddDays(10),
                    IsValid = true,
                    Price =1.55M,
                    ProductImage = "aaa.jpg",
                    StartDateTime = DateTime.Now} ,
                new ProductDailyDeal { 
                    ProductId = 2,
                    EndDateTime = DateTime.Now.AddDays(10),
                    IsValid = true,
                    Price =2.66M,
                    ProductImage = "bbb.jpg",
                    StartDateTime = DateTime.Now} 
                
                };

                ServiceFactory.ProductDailyDealService.SetDailyDealList(lstProductAreaLanguage,new List<DailyDealLabel>());

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

    }
}
