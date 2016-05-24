using System;
using System.IO;
using Com.Panduo.Service;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class IProductImageServiceTest:SpringTest
    { 
        /// <summary>
        ///GetNoPhotoImageUrl 的测试
        ///</summary>
        [Test()]
        public void GetNoPhotoImageUrlTest()
        { 

        }

        /// <summary>
        ///GetNoPhotoImageUrl 的测试
        ///</summary>
        [Test()]
        public void GetNoPhotoImageUrlTest1()
        {
            var assemblyPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string configFile = Path.Combine(assemblyPath, @"Config\PhotosRule.xml");
            
        }

        /// <summary>
        ///GetProductImageUrl 的测试
        ///</summary>
        [Test()]
        public void GetProductImageUrlTest()
        {
            var imageName = "B00004A.JPG";//B27638A_310_310.JPG
            //var imageName = Guid.NewGuid().ToString() + ".jpeg";

            var ulr = ServiceFactory.ProductImageService.GetProductImageUrl(imageName, 80);

            var ulr2 = ServiceFactory.ProductImageService.GetProductImageUrl(imageName, 310, 310);

        }

        /// <summary>
        ///GetProductImageUrl 的测试
        ///</summary>
        [Test()]
        public void GetProductImageUrlTest1()
        {

            var imageName = Guid.NewGuid().ToString() + ".jpeg";

            Console.WriteLine(imageName);
        }

        /// <summary>
        ///GetProductImageUrl 的测试
        ///</summary>
        [Test()]
        public void GetProductImageUrlTest2()
        {
             
        }
    }
}
