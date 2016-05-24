using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using NUnit.Framework;


namespace Com.Panduo.Tests
{
    [TestFixture]
    public class WishListServiceTest : SpringTest
    {
        private readonly WishListProduct _wishListProduct = new WishListProduct
        {
            CustomerId = 2,
            ProductId = 1,
            Count = 5,
            AddDateTime = DateTime.Now,
            WishListType = WishListType.Frequently
        };

        private readonly List<WishListProduct> _wishListProducts = new List<WishListProduct>
        {
            new WishListProduct
            {
                CustomerId = 2,
                ProductId = 4,
                Count = 5,
                AddDateTime = DateTime.Now,
                WishListType = WishListType.EveryTime
            },
            new WishListProduct
            {
                CustomerId = 2,
                ProductId = 5,
                Count = 5,
                AddDateTime = DateTime.Now,
                WishListType = WishListType.Like
            },
            new WishListProduct
            {
                CustomerId = 2,
                ProductId = 6,
                Count = 5,
                AddDateTime = DateTime.Now,
                WishListType = WishListType.Like
            }
        };

        /// <summary>
        /// 添加心愿单产品（功能测试通过）
        /// </summary>
        [Test]
        public void AddWishListProductTest()
        {
            int a = ServiceFactory.WishListService.AddWishListProduct(_wishListProduct);
        }

        /// <summary>
        /// 批量添加心愿单产品（功能测试通过）
        /// </summary>
        [Test]
        public void AddWishListProductsTest()
        {
            ServiceFactory.WishListService.AddWishListProducts(_wishListProducts);
        }

        /// <summary>
        /// 删除心愿单产品（功能测试通过）
        /// </summary>
        [Test]
        public void RemoveWishListProductTest()
        {
           ServiceFactory.WishListService.RemoveWishListProduct(2, 1,false);
        }

            [Test]
        public void GetWishListTypeTest()
        {
           var ss= ServiceFactory.WishListService.GetWishListType(1);
           Assert.AreEqual(ss.Count > 0, true);

        }

        /// <summary>
        /// 批量删除心愿单产品（功能测试通过）
        /// </summary>
        [Test]
        public void RemoveWishListProductsTest()
        {

            var list = new List<KeyValuePair<int, bool>>()
                {
                    new KeyValuePair<int, bool>(359, false),
                    new KeyValuePair<int, bool>(361, false),
                    new KeyValuePair<int, bool>(107, false)
                };
          ServiceFactory.WishListService.RemoveWishListProduct(2,list);
        }

        /// <summary>
        /// 设置喜爱类型（功能测试通过）
        /// </summary>
        [Test]
        public void SetWishListTypeTest()
        {
           ServiceFactory.WishListService.SetWishListType(2, 7, WishListType.EveryTime,false);
        }

        /// <summary>
        /// 批量设置喜爱类型（功能测试通过）
        /// </summary>
        [Test]
        public void SetWishListTypesTest()
        {
            //ServiceFactory.WishListService.SetWishListType(2, new Dictionary<int, WishListType>
            // {
            //     {2,WishListType.Like},
            //     {3,WishListType.Like},
            //     {7,WishListType.Like}
            // });
        }

        /// <summary>
        /// 通过id获取心愿单（功能测试通过）
        /// </summary>
        [Test]
        public void GetWishListProductById()
        {
            //var a = ServiceFactory.WishListService.GetWishListProductById(3);
        }

        /// <summary>
        /// 分页（功能测试通过）
        /// </summary>
        [Test]
        public void GetWishListProducts()
        {
            //var a = ServiceFactory.WishListService.GetWishListProducts(1, 1, 2, null, null);
            var b = ServiceFactory.WishListService.GetWishListProducts(1, 2, 2, null, null);
            //var c = ServiceFactory.WishListService.GetWishListProducts(1, 1, 1, null, null);
        }
    }
}