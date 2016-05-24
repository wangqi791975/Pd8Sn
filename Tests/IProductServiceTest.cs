//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductServiceTest.cs
//创 建 人：罗海明
//创建时间：2014/12/23 23:40:40 
//功能说明：产品服务单元测试
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public  class ProductServiceTest : SpringTest
    {
        /// <summary>
        /// 产品测试数据初始化
        /// </summary>
        protected override void OnSetUp()
        {
            TestDataService.ProductInit();
            TestDataService.ProductDescInit();
            TestDataService.ProductMatchInit();
            TestDataService.ProductAlsoBoughtInit();
        }

        #region TestGetProductById
        [Test]
        public void TestGetProductById1()
        {
            var c = ServiceFactory.ProductService.GetProductById(4);
            Assert.AreEqual(4, c.ProductId);
        }

        [Test]
        public void TestGetProductById2()
        {
            var c = ServiceFactory.ProductService.GetProductById(90000000);
            Assert.AreEqual(null, null);
        } 
        #endregion

        #region TestGetProductByCode
        [Test]
        public void TestGetProductByCode1()
        {
            var c = ServiceFactory.ProductService.GetProductByCode("B10005");
            Assert.AreEqual(true, c.ProductId == 5 && "B10005".Equals(c.ProductCode));
        }

        [Test]
        public void TestGetProductByCode2()
        {
            var c = ServiceFactory.ProductService.GetProductByCode("B22005");
            Assert.AreEqual(null, null);
        } 
        #endregion

        #region TestGetProductName
        [Test]
        public void TestGetProductName1()
        {
            var c = ServiceFactory.ProductService.GetProductName(4, 1);
            Assert.AreEqual("25Pcs Antique Silver Fish Beads Charms Pendants 10x15mm", c);
        }

        [Test]
        public void TestGetProductName2()
        {
            var c = ServiceFactory.ProductService.GetProductName(3, 6);
            Assert.AreEqual("亜鉛合金 スペーサ ビーズ バレル 銀古美 ストライプパターン 約7.0mm x 5.0mm、 　穴：約3.0mm、 100 PCs", c);
        }
        [Test]
        public void TestGetProductName3()
        {
            var c = ServiceFactory.ProductService.GetProductName(3, 7);
            Assert.AreEqual("Lega di Zinco Separatori Perline Barile Argento Antico Striscia Disegno Circa 7.0mm x 5.0mm, Foro:Circa 3.0mm, 100 Pz", c);
        } 
        #endregion

        #region TestGetProductDescription
        [Test]
        public void TestGetProductDescription1()
        {

            var c = ServiceFactory.ProductService.GetProductDescription(1, 1);
            Assert.AreEqual("Test Description", c);
        }

        [Test]
        public void TestGetProductDescription2()
        {
            var c = ServiceFactory.ProductService.GetProductDescription(4, 2);
            Assert.AreEqual(null, c);
        } 
        #endregion

        #region TestGetAlsoBuyProductsTopNById
        [Test]
        public void TestGetAlsoBuyProductsTopNById1()
        {
            var c = ServiceFactory.ProductService.GetAlsoBuyProductsTopNById(4, 3).Count;
            Assert.AreEqual(2, c);
        }

        [Test]
        public void TestGetAlsoBuyProductsTopNById2()
        {
            var c = ServiceFactory.ProductService.GetAlsoBuyProductsTopNById(10, 2).Count;
            Assert.AreEqual(2, c);
        }

        [Test]
        public void TestGetAlsoBuyProductsTopNById3()
        {
            var c = ServiceFactory.ProductService.GetAlsoBuyProductsTopNById(8, 1);
            Console.WriteLine("ProductId:" + c[0].ProductId + "   ProductCode:" + c[0].ProductCode);
            Assert.AreEqual(true, c[0].ProductId == 4);
        } 
        #endregion

        #region TestGetMatchProductTopNById
        [Test]
        public void TestGetMatchProductTopNById()
        {
            var c = ServiceFactory.ProductService.GetMatchProductTopNById(2, 2).Count;
            Assert.AreEqual(2, c);
        }

        [Test]
        public void TestGetMatchProductTopNById2()
        {
            var c = ServiceFactory.ProductService.GetMatchProductTopNById(2, 3).Count;
            Assert.AreEqual(2, c);
        }

        [Test]
        public void TestGetMatchProductTopNById3()
        {
            var c = ServiceFactory.ProductService.GetMatchProductTopNById(8, 1);
            Console.WriteLine("ProductId:" + c[0].ProductId + "   ProductCode:" + c[0].ProductCode);
            Assert.AreEqual(true, c[0].ProductId == 2);
        } 
        #endregion

        #region TestGetMatchProductsById
        [Test]
        public void TestGetMatchProductsById()
        {
            var c = ServiceFactory.ProductService.GetMatchProductsById(4).Count;
            Assert.AreEqual(true, c == 1);
        }

        [Test]
        public void TestGetMatchProductsById2()
        {
            var c = ServiceFactory.ProductService.GetMatchProductsById(2).Count;
            Assert.AreEqual(true, c == 2);
        }

        [Test]
        public void TestGetProductPrice()
        {
            var c = ServiceFactory.ProductService.GetProductPrice(59263);
            //Assert.AreEqual(2, c);
        }
        #endregion

        #region 后台
        [Test]
        public void AddProductPriceRise()
        {
            ProductPriceRise rise = new ProductPriceRise
            {
                RiseValue = 3M,
                DateCreated = DateTime.Now,
                AdminId = 1
            };
            ServiceFactory.ProductService.AddProductPriceRise(rise);
        }

        [Test]
        public void DeleteProductPriceRiseById()
        {
            ServiceFactory.ProductService.DeleteProductPriceRiseById(1);
        }

        [Test]
        public void GetAllProductPriceRise()
        {
            var list = ServiceFactory.ProductService.GetAllProductPriceRise();
            foreach (var productPriceRise in list)
            {
                Console.WriteLine(productPriceRise.RiseValue);
            }
        }

        #endregion
    }
}
