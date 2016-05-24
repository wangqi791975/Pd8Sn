

using System;
using System.Collections.Generic;
using Com.Panduo.Service;
using NUnit.Framework;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.ProductArea;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 IProductAreaServiceTest 的测试类，旨在
    ///包含所有 IProductAreaServiceTest 单元测试
    ///</summary>
    [TestFixture]
    public class ProductAreaServiceTest : SpringTest
    {
        /// <summary>
        ///GetProductArea 的测试
        ///</summary>
        [Test]
        public void GetProductAreaTest()
        {
            int currentPage = 1, pageSize = 10;
            var searchCriteria = new Dictionary<ProductAreaSearchCriteria, object>
                {
                    {ProductAreaSearchCriteria.AreaName, "圣诞"}
                };
            var sorterCriteria = new List<Sorter<ProductAreaSorterCriteria>>
                {
                   new Sorter<ProductAreaSorterCriteria>{ Key = ProductAreaSorterCriteria.ProductCode, IsAsc = true}
                }; ;
            var pageData = ServiceFactory.ProductAreaService.FindProductAreas(currentPage, pageSize, searchCriteria, sorterCriteria);

            //Console.WriteLine(pageData.Pager.TotalRowCount);
            //Assert.AreEqual(10, pageData.Data.Count);
        }


        /// <summary>
        ///AddProductArea 的测试
        ///</summary>
        [Test]
        public void SetProductAreaTest()
        {
            try
            {
                var productArea = new ProductArea
                {
                    AreaName = "圣诞",
                    IsShowHome = true,
                    IsValid = true
                };

                var lstProductAreaLanguage = new List<ProductAreaLanguage> { 
                new ProductAreaLanguage { AreaName = "Christmas", LanguageId = 1 }, 
                new ProductAreaLanguage { AreaName = "Weihnachten", LanguageId = 2 }, 
                new ProductAreaLanguage { AreaName = "рождество", LanguageId = 3 } };
                productArea.ProductAreaLanguages = lstProductAreaLanguage;
                ServiceFactory.ProductAreaService.SetProductArea(productArea);

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        /// <summary>
        ///AddProductArea 的测试
        ///</summary>
        [Test]
        public void SetProductAreaRelativeList()
        {
            try
            {
                var productArea = new ProductArea
                {
                    AreaId = 10,
                    AreaName = "圣诞区",
                    IsShowHome = true,
                    IsValid = true
                };

                var lstProductAreaRelative = new List<ProductAreaRelative> { 
                new ProductAreaRelative { ProductId  = 59862, AreaId = 10 }, 
                //new ProductAreaRelative { ProductId  = 59863, AreaId = 10 }, 
                new ProductAreaRelative { ProductId  = 59864, AreaId = 10 }, 
                new ProductAreaRelative { ProductId  = 59865, AreaId = 10 }, 
                new ProductAreaRelative { ProductId  = 59866, AreaId = 10 } };

                //ServiceFactory.ProductAreaService.SetProductAreaRelativeList(10);

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        [Test]
        public void GetProductAreaURL()
        {
            var a = ServiceFactory.ProductAreaService.GetProductAreaURL(2, 10, 2, "hhhhhh");
        }
    }
}
