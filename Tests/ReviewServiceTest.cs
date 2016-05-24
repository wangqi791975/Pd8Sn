using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Review;
using Com.Panduo.Service.ServiceConst;
using NUnit.Framework;


namespace Com.Panduo.Tests
{
    [TestFixture]
    public class ReviewServiceTest : SpringTest
    {
        private readonly ProductReview _productReview = new ProductReview
        {
            CustomerId = 2,
            ProductId = 1,
            LanguageId = 1,
            Rating = 5,
            ReviewContent = "",
            CreateDateTime = DateTime.Now,
            OrderProductId = 5
        };

        private readonly WebSiteReview _webSiteReview = new WebSiteReview
        {
            OrderId = 0,
            LanguageId = 1,
            CustomerId = 2,
            ReviewType = ReviewType.Order,
            Rating = 3,
            ReviewContent = "",
            CreateDateTime = DateTime.Now
        };


        /// <summary>
        /// 添加商品评论(第一个异常未能测试（订单接口未实现），第二个异常测试通过，功能测试通过)
        /// </summary>
        [Test]
        public void AddProductReviewTest()
        {
            var b = new ProductReview
            {
                CustomerId = 2,
                OrderProductId = 5,
                ProductId = 241,
                LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                Rating = 5,
                ReviewContent = "qqweqweqwe",
                CreateDateTime = DateTime.Now,
                AuditStatus = AuditStatus.AuditPass,
                IsValid = true
            };
            var a = ServiceFactory.ReviewService.AddProductReview(b);
        }

        /// <summary>
        /// 回复评论(功能测试通过)
        /// </summary>
        [Test]
        public void ReplyProductReviewTest()
        {
            ServiceFactory.ReviewService.ReplyProductReview(3, 1, DateTime.Now, "");
        }

        /// <summary>
        /// 修改状态(功能测试通过)
        /// </summary>
        [Test]
        public void UpdateProductReviewStatusTest()
        {
            ServiceFactory.ReviewService.UpdateProductReviewStatus(3, true);
        }

        /// <summary>
        /// 修改审核状态(功能测试通过)
        /// </summary>
        [Test]
        public void UpdateProductReviewAduitStatusTest()
        {
            ServiceFactory.ReviewService.UpdateProductReviewAduitStatus(3, AuditStatus.AuditPass);
        }

        /// <summary>
        /// 通过Id获取评论(功能测试通过)
        /// </summary>
        [Test]
        public void GetProductReviewByIdTest()
        {
            var a = ServiceFactory.ReviewService.GetProductReviewById(3);
        }

        /// <summary>
        /// 通过Id获取评论(数据太少没测全，测试有数据)
        /// </summary>
        [Test]
        public void FindProductReviewsByProductIdTest()
        {
            var a = ServiceFactory.ReviewService.FindProductReviewsByProductId(1, 1, 1, null, null);
        }

        /// <summary>
        /// 通过订单明细Id获取评论(功能测试通过)
        /// </summary>
        [Test]
        public void GetProductReviewsByOrderProductIdTest()
        {
            var a = ServiceFactory.ReviewService.GetProductReviewsByOrderProductId(5);
        }

        /// <summary>
        /// 获取产品评论数和平均分(功能测试通过)
        /// </summary>
        [Test]
        public void GetRatingByProductIdTest()
        {
            var a = ServiceFactory.ReviewService.GetRatingByProductId(1);
        }

        /// <summary>
        /// 添加网站评论(功能测试通过)
        /// </summary>
        [Test]
        public void AddWebSiteReviewTest()
        {
            int a = ServiceFactory.ReviewService.AddWebSiteReview(_webSiteReview);
        }

        /// <summary>
        /// 答复评论(功能测试通过)
        /// </summary>
        [Test]
        public void ReplyWebSiteReviewTest()
        {
            ServiceFactory.ReviewService.ReplyWebSiteReview(2, 3, DateTime.Now, "");
        }

        /// <summary>
        /// 修改评论状态(功能测试通过)
        /// </summary>
        [Test]
        public void UpdateWebSiteReviewStatusTest()
        {
            ServiceFactory.ReviewService.UpdateWebSiteReviewStatus(2, true);
        }

        /// <summary>
        /// 修改评论审核状态(功能测试通过)
        /// </summary>
        [Test]
        public void UpdateWebSiteReviewAuditStatusTest()
        {
            ServiceFactory.ReviewService.UpdateWebSiteReviewAuditStatus(2, AuditStatus.AuditNotPass);
        }

        /// <summary>
        /// 通过Id获取网站评论(功能测试通过)
        /// </summary>
        [Test]
        public void GetWebSiteReviewByIdTest()
        {
            var a = ServiceFactory.ReviewService.GetWebSiteReviewById(2);
        }

        /// <summary>
        /// 通过订单Id获取评论(功能测试通过)
        /// </summary>
        [Test]
        public void GetWebSiteReviewByOrderIdTest()
        {
            var a = ServiceFactory.ReviewService.GetWebSiteReviewByOrderId(0);
        }

        /// <summary>
        /// 分页查找(功能测试通过)
        /// </summary>
        [Test]
        public void FindWebSiteReviewsByTypeTest()
        {
            var a = ServiceFactory.ReviewService.FindWebSiteReviewsByType(2, 2, ReviewType.Order, null, null);
        }

        /// <summary>
        /// 分页查找(功能测试通过)
        /// </summary>
        [Test]
        public void FindCustomerWebSiteReviewsByType()
        {
            const string keyword = "w";
            Dictionary<CustomerReviewSearchCriteria, object> searchCriteria = null;
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria = new Dictionary<CustomerReviewSearchCriteria, object> { { CustomerReviewSearchCriteria.Name, keyword }, { CustomerReviewSearchCriteria.Email, keyword } };
            }
            var a = ServiceFactory.ReviewService.FindCustomerWebSiteReviewsByType(1, 1, ReviewType.Web, null, null);
        }

        /// <summary>
        /// 分页查找(功能测试通过)
        /// </summary>
        [Test]
        public void FindProductReviewsByType()
        {
            var a = ServiceFactory.ReviewService.FindCustomerWebSiteReviewsByType(2, 2, ReviewType.Order, null, null);
            var productReviews = ServiceFactory.ReviewService.FindCustomerProductReviewsByProductId(1, 10, null, null, 241);
        }

        /// <summary>
        /// 获取topn数据(功能测试通过)
        /// </summary>
        [Test]
        public void GetTopNRecommendWebSiteReviewsTest()
        {
            var a = ServiceFactory.ReviewService.GetTopNRecommendWebSiteReviews(10);
        }

        [Test]
        public void TestKeyword()
        {
            int page = 1;
            int pageSize = 20;
            var keyword = "188";

            var searchCriteria = new Dictionary<CustomerReviewSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<CustomerReviewSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(CustomerReviewSearchCriteria.Email, keyword);
            }
            /*
            searchCriteria.Add(CustomerReviewSearchCriteria.LanguageId, 1);
            */

            var sort = new Sorter<CustomerReviewSorterCriteria>(CustomerReviewSorterCriteria.DateCreated, true);
            sorterCriteria.Add(sort);

            var webSiteReviews = ServiceFactory.ReviewService.FindCustomerWebSiteReviewsByType(page, pageSize, ReviewType.Web, searchCriteria, sorterCriteria);

        }

        [Test]
        public void TestGetRecomendReview()
        {
            var a = ServiceFactory.ReviewService.GetRecommendReview();
        }
    }
}