using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Review;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Review;
using Com.Panduo.ServiceImpl.Order.Dao;
using Com.Panduo.ServiceImpl.Review.Dao;

namespace Com.Panduo.ServiceImpl.Review
{
    public class ReviewService : IReviewService
    {
        public IOrderDetailDao OrderDetailDao { private get; set; }
        public IReviewProductDao ReviewProductDao { private get; set; }
        public IReviewWebsiteDao ReviewWebsiteDao { private get; set; }
        public IReviewWebsiteCustomerViewDao ReviewWebsiteCustomerViewDao { private get; set; }
        public IReviewProductCustomerViewDao ReviewProductCustomerViewDao { private get; set; }
        public INotReviewProductViewDao NotReviewProductViewDao { private get; set; }

        public string ERROR_CAN_NOT_REVIEW_WITHOUT_BUY_PRODUCT
        {
            get { return "ERROR_CAN_NOT_REVIEW_WITHOUT_BUY_PRODUCT"; }
        }

        public string ERROR_ALREADY_REPLY
        {
            get { return "ERROR_ALREADY_REPLY"; }
        }

        public string ERROR_CAN_NOT_REVIEW_AGAIN
        {
            get { return "ERROR_CAN_NOT_REVIEW_AGAIN"; }
        }

        public string ERROR_ALREADY_REPLAY_WEBSITE
        {
            get { return "ERROR_ALREADY_REPLAY_WEBSITE"; }
        }

        public string ERROR_ORDER_PRODUCT_DUPLICATE_REVIEW
        {
            get { return "ERROR_ORDER_PRODUCT_DUPLICATE_REVIEW"; }
        }

        public int AddProductReview(ProductReview productReview)
        {
            //todo 判断是否购买此商品
            //if (!OrderDetailDao.IsBuyProduct(productReview.CustomerId, productReview.ProductId))
            //{
            //    throw new BussinessException(ERROR_CAN_NOT_REVIEW_WITHOUT_BUY_PRODUCT);
            //}
            if (!ReviewProductDao.GetReviewProduct(productReview.OrderProductId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_ORDER_PRODUCT_DUPLICATE_REVIEW);
            }
            return ReviewProductDao.AddObject(GetReviewProductPoFromVo(productReview));
        }

        public void ReplyProductReview(int productReviewId, int adminId, DateTime replyDateTime, string replyContent)
        {
            ReviewProductDao.UpdateReviewProduct(productReviewId, adminId, replyDateTime, replyContent);
        }

        public void UpdateProductReviewStatus(int productReviewId, bool isValid)
        {
            ReviewProductDao.UpdateReviewProduct(productReviewId, isValid);
        }

        public void UpdateProductReviewAduitStatus(int productReviewId, AuditStatus auditStatus)
        {
            ReviewProductDao.UpdateReviewProduct(productReviewId, (int)auditStatus);
        }

        public ProductReview GetProductReviewById(int productReviewId)
        {
            return GetReviewProductVoFromPo(ReviewProductDao.GetObject(productReviewId));
        }

        public int GetCustomerReivewsCount(int customerId)
        {
            return ReviewProductDao.GetReivewCusotmerCount(customerId); ;
        }

        public PageData<ProductReview> FindProductReviewsByOrderId(int currentPage, int pageSize, int orderId, IDictionary<ProductReviewSearchCriteria, object> searchCriteria,
            IList<Sorter<ReviewSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT R FROM ReviewProductPo R WHERE OrderId = " + orderId);

            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case ReviewSorterCriteria.AuditStatus:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.CreateDateTime:
                            hqlHelper.AddSorter("R.DateCreated", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.Id:
                            hqlHelper.AddSorter("R.ReviewId", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.Rating:
                            hqlHelper.AddSorter("R.ReviewRating", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.ReplyDateTime:
                            hqlHelper.AddSorter("R.ReplyDate", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.IsValid:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("R.DateCreated", false);
            }

            var pageDataPo = ReviewProductDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ProductReview>();
            var voList = pageDataPo.Data.Select(GetReviewProductVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public PageData<ProductReview> FindProductReviewsByProductId(int currentPage, int pageSize, int productId, IDictionary<ProductReviewSearchCriteria, object> searchCriteria,
            IList<Sorter<ReviewSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT R FROM ReviewProductPo R WHERE ProductId = " + productId);

            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case ReviewSorterCriteria.AuditStatus:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.CreateDateTime:
                            hqlHelper.AddSorter("R.DateCreated", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.Id:
                            hqlHelper.AddSorter("R.ReviewId", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.Rating:
                            hqlHelper.AddSorter("R.ReviewRating", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.ReplyDateTime:
                            hqlHelper.AddSorter("R.ReplyDate", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.IsValid:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("R.DateCreated", false);
            }

            var pageDataPo = ReviewProductDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ProductReview>();
            var voList = pageDataPo.Data.Select(GetReviewProductVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public PageData<ReviewProductCustomerView> FindCustomerProductReviewsByProductId(int currentPage, int pageSize, IDictionary<CustomerReviewSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerReviewSorterCriteria>> sorterCriteria, int? productId)
        {
            var hqlHelper = new HqlHelper("SELECT R FROM ReviewProductCustomerViewPo R");
            if (productId.HasValue)
            {
                hqlHelper.AddWhere("ProductId", HqlOperator.Eq, "ProductId", productId);
            }
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CustomerReviewSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("R.LanguageId", HqlOperator.Eq, "LanguageId", item.Value);
                            break;
                        case CustomerReviewSearchCriteria.Name:
                            hqlHelper.AddWhere("R.Name", HqlOperator.Like, "Name", item.Value);
                            break;
                        case CustomerReviewSearchCriteria.Email:
                            hqlHelper.AddWhere("R.Email", HqlOperator.Like, "Email", item.Value);
                            break;
                        case CustomerReviewSearchCriteria.Keyword:
                            hqlHelper.AddWhere(string.Format("(R.Email Like {0} Or R.ProductName Like {0})", ":Keyword"), HqlOperator.Exp, "Keyword", string.Format("%{0}%", item.Value));
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case CustomerReviewSorterCriteria.Email:
                            hqlHelper.AddSorter("R.Email", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.Name:
                            hqlHelper.AddSorter("R.Name", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.DateCreated:
                            hqlHelper.AddSorter("R.DateCreated", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.LanguageChName:
                            hqlHelper.AddSorter("R.LanguageChName", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.IsValid:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("R.DateCreated", false);
            }

            var pageDataPo = ReviewProductCustomerViewDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ReviewProductCustomerView>();
            var voList = pageDataPo.Data.Select(GetReviewProductCustomerViewVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public ReviewProductCustomerView GetReviewProductCustomerViewById(int id)
        {
            return GetReviewProductCustomerViewVoFromPo(ReviewProductCustomerViewDao.GetObject(id));
        }

        public ProductReview GetProductReviewsByOrderProductId(int orderProductId)
        {
            return GetReviewProductVoFromPo(ReviewProductDao.GetReviewProduct(orderProductId));
        }

        public IDictionary<Rating, decimal> GetRatingByProductId(int productId)
        {
            IDictionary<Rating, decimal> dictionary = new Dictionary<Rating, decimal>();
            var count = ReviewProductDao.GetReviewProductCount(productId);
            dictionary.Add(new KeyValuePair<Rating, decimal>(Rating.ReviewCount, count));
            var avg = ReviewProductDao.GetReviewProductAvg(productId);
            dictionary.Add(new KeyValuePair<Rating, decimal>(Rating.AvgScore, avg));
            return dictionary;
        }

        public NotReviewProductView GetNotReviewProductView(int customerId, int productId)
        {
            var notReviewProductViewPo = NotReviewProductViewDao.GetNotReviewProductViewPo(customerId, productId, (int)OrderStatusType.Shipped);
            NotReviewProductView notReviewProductView = null;
            if (!notReviewProductViewPo.IsNullOrEmpty())
            {
                notReviewProductView = new NotReviewProductView
                {
                    OrderProductId = notReviewProductViewPo.OrderProductId,
                    CustomerId = notReviewProductViewPo.CustomerId,
                    FullName = notReviewProductViewPo.FullName,
                    ProductId = notReviewProductViewPo.ProductId,
                    ProductCode = notReviewProductViewPo.ProductCode,
                    ProductName = notReviewProductViewPo.ProductName,
                    Status = notReviewProductViewPo.Status,
                };
            }
            return notReviewProductView;
        }

        public int AddWebSiteReview(WebSiteReview webSiteReview)
        {
            return ReviewWebsiteDao.AddObject(GetReviewWebsitePoFromVo(webSiteReview));
        }

        public void ReplyWebSiteReview(int webSiteReviewId, int adminId, DateTime replyDateTime, string replyContent)
        {
            ReviewWebsiteDao.UpdateReviewWebSite(webSiteReviewId, adminId, replyDateTime, replyContent);
        }

        public void UpdateWebSiteReviewStatus(int webSiteReviewId, bool isValid)
        {
            ReviewWebsiteDao.UpdateReviewWebSite(webSiteReviewId, isValid);
        }

        public void UpdateWebSiteReviewAuditStatus(int webSiteReviewId, AuditStatus auditStatus)
        {
            ReviewWebsiteDao.UpdateReviewWebSite(webSiteReviewId, (int)auditStatus);
        }

        public void UpdateIsRecommend(int webSiteReviewId, bool isRecommend)
        {
            ReviewWebsiteDao.UpdateReviewWebSiteRecommend(webSiteReviewId, isRecommend);
        }

        public WebSiteReview GetWebSiteReviewById(int webSiteReviewId)
        {
            return GetReviewWebsiteVoFromPo(ReviewWebsiteDao.GetObject(webSiteReviewId));
        }

        public IList<WebSiteReview> GetWebSiteReviewByOrderId(int orderId)
        {
            var reviewWebsitePos = ReviewWebsiteDao.GetReviewProductPos(orderId);
            return reviewWebsitePos.Select(GetReviewWebsiteVoFromPo).ToList();
        }

        public ReviewWebsiteCustomerView GetRecommendReview()
        {
            return GetReviewWebsiteCustomerViewVoFromPo(ReviewWebsiteCustomerViewDao.GetRecommendReview());
        }

        public PageData<ReviewWebsiteCustomerView> FindCustomerWebSiteReviewsByType(int currentPage, int pageSize, ReviewType reviewType, IDictionary<CustomerReviewSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerReviewSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT R FROM ReviewWebsiteCustomerViewPo R");
            hqlHelper.AddWhere("R.Type", HqlOperator.Eq, "Type", ((int)reviewType).ToString(CultureInfo.InvariantCulture));
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CustomerReviewSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("R.LanguageId", HqlOperator.Eq, "LanguageId", item.Value);
                            break;
                        case CustomerReviewSearchCriteria.Name:
                            hqlHelper.AddWhere("R.Name", HqlOperator.Like, "Name", item.Value);
                            break;
                        case CustomerReviewSearchCriteria.Email:
                            hqlHelper.AddWhere("R.Email", HqlOperator.Like, "Email", item.Value);
                            break;
                        case CustomerReviewSearchCriteria.Keyword:
                            hqlHelper.AddWhere(string.Format("(R.Email Like {0}  Or R.Name Like {0})", ":Keyword"), HqlOperator.Exp, "Keyword", string.Format("%{0}%", item.Value));
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case CustomerReviewSorterCriteria.Email:
                            hqlHelper.AddSorter("R.Email", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.Name:
                            hqlHelper.AddSorter("R.Name", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.DateCreated:
                            hqlHelper.AddSorter("R.DateCreated", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.LanguageChName:
                            hqlHelper.AddSorter("R.LanguageChName", sorter.IsAsc);
                            break;
                        case CustomerReviewSorterCriteria.IsValid:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("R.DateCreated", false);
            }

            var pageDataPo = ReviewWebsiteCustomerViewDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ReviewWebsiteCustomerView>();
            var voList = pageDataPo.Data.Select(GetReviewWebsiteCustomerViewVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public ReviewWebsiteCustomerView GetReviewWebsiteCustomerViewById(int id)
        {
            return GetReviewWebsiteCustomerViewVoFromPo(ReviewWebsiteCustomerViewDao.GetObject(id));
        }


        public PageData<WebSiteReview> FindWebSiteReviewsByType(int currentPage, int pageSize, ReviewType reviewType, IDictionary<WebSiteReviewSearchCriteria, object> searchCriteria,
            IList<Sorter<ReviewSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT R FROM ReviewWebsitePo R WHERE Type = " + (int)reviewType);

            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case ReviewSorterCriteria.AuditStatus:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.CreateDateTime:
                            hqlHelper.AddSorter("R.DateCreated", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.Id:
                            hqlHelper.AddSorter("R.ReviewId", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.Rating:
                            hqlHelper.AddSorter("R.ReviewRating", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.ReplyDateTime:
                            hqlHelper.AddSorter("R.ReplyDate", sorter.IsAsc);
                            break;
                        case ReviewSorterCriteria.IsValid:
                            hqlHelper.AddSorter("R.Status", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("R.DateCreated", false);
            }

            var pageDataPo = ReviewWebsiteDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<WebSiteReview>();
            var voList = pageDataPo.Data.Select(GetReviewWebsiteVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public IList<WebSiteReview> GetTopNRecommendWebSiteReviews(int count)
        {
            var reviewWebsitePos = ReviewWebsiteDao.GetTopNReviewProductPos(count);
            return reviewWebsitePos.Select(GetReviewWebsiteVoFromPo).ToList();
        }

        #region 辅助方法

        internal static ReviewProductPo GetReviewProductPoFromVo(ProductReview productReview)
        {
            ReviewProductPo reivewProductPo = null;
            if (!productReview.IsNullOrEmpty())
            {
                reivewProductPo = new ReviewProductPo
                {
                    ReviewId = productReview.Id,
                    CustomerId = productReview.CustomerId,
                    OrderId = productReview.OrderId,
                    OrderProductId = productReview.OrderProductId,
                    ProductId = productReview.ProductId,
                    LanguageId = productReview.LanguageId,
                    ReviewRating = productReview.Rating,
                    ReviewContent = productReview.ReviewContent,
                    DateCreated = productReview.CreateDateTime,
                    Status = (int)productReview.AuditStatus,
                    ReplyContent = productReview.ReplyContent,
                    AdminId = productReview.AdminId,
                    ReplyDate = productReview.ReplyDateTime,
                    IsValid = productReview.IsValid
                };
            }
            return reivewProductPo;
        }

        internal static ProductReview GetReviewProductVoFromPo(ReviewProductPo reviewProductPo)
        {
            ProductReview productReview = null;
            if (!reviewProductPo.IsNullOrEmpty())
            {
                productReview = new ProductReview
                {
                    Id = reviewProductPo.ReviewId,
                    CustomerId = reviewProductPo.CustomerId,
                    OrderId = reviewProductPo.OrderId,
                    OrderProductId = reviewProductPo.OrderProductId,
                    ProductId = reviewProductPo.ProductId,
                    LanguageId = reviewProductPo.LanguageId,
                    Rating = reviewProductPo.ReviewRating,
                    ReviewContent = reviewProductPo.ReviewContent,
                    CreateDateTime = reviewProductPo.DateCreated,
                    AuditStatus = (AuditStatus)reviewProductPo.Status,
                    ReplyContent = reviewProductPo.ReplyContent,
                    AdminId = reviewProductPo.AdminId,
                    ReplyDateTime = reviewProductPo.ReplyDate,
                    IsValid = reviewProductPo.IsValid
                };
            }
            return productReview;
        }

        internal static ReviewWebsitePo GetReviewWebsitePoFromVo(WebSiteReview webSiteReview)
        {
            ReviewWebsitePo reviewWebsitePo = null;
            if (!webSiteReview.IsNullOrEmpty())
            {
                reviewWebsitePo = new ReviewWebsitePo
                {
                    ReviewId = webSiteReview.Id,
                    CustomerId = webSiteReview.CustomerId,
                    Type = Convert.ToBoolean((int)webSiteReview.ReviewType),
                    OrderId = webSiteReview.OrderId,
                    LanguageId = webSiteReview.LanguageId,
                    ReviewRating = webSiteReview.Rating,
                    ReviewContent = webSiteReview.ReviewContent,
                    DateCreated = webSiteReview.CreateDateTime,
                    Status = (int)webSiteReview.AuditStatus,
                    IsValid = webSiteReview.IsValid,
                    Recommend = webSiteReview.IsRecommend,
                    ReplyContent = webSiteReview.ReplyContent,
                    AdminId = webSiteReview.AdminId,
                    ReplyDate = webSiteReview.ReplyDateTime
                };
            }
            return reviewWebsitePo;
        }

        internal static ReviewProductCustomerView GetReviewProductCustomerViewVoFromPo(
            ReviewProductCustomerViewPo reviewProductCustomerViewPo)
        {
            ReviewProductCustomerView reviewProductCustomer = null;
            if (!reviewProductCustomerViewPo.IsNullOrEmpty())
            {
                reviewProductCustomer = new ReviewProductCustomerView
                {
                    Id = reviewProductCustomerViewPo.Id,
                    ProductId = reviewProductCustomerViewPo.ProductId,
                    ProductName = reviewProductCustomerViewPo.ProductName,
                    ProductModel = reviewProductCustomerViewPo.ProductModel,
                    Email = reviewProductCustomerViewPo.Email,
                    Name = reviewProductCustomerViewPo.Name,
                    CountryId = reviewProductCustomerViewPo.CountryId,
                    CountryName = reviewProductCustomerViewPo.CountryName,
                    Rating = reviewProductCustomerViewPo.Rating,
                    DateCreated = reviewProductCustomerViewPo.DateCreated,
                    LanguageId = reviewProductCustomerViewPo.LanguageId,
                    LanguageChName = reviewProductCustomerViewPo.LanguageChName,
                    IsValid = reviewProductCustomerViewPo.IsValid,
                    Content = reviewProductCustomerViewPo.Content,
                    RepContent = reviewProductCustomerViewPo.RepContent
                };
            }
            return reviewProductCustomer;
        }

        internal static ReviewWebsiteCustomerView GetReviewWebsiteCustomerViewVoFromPo(
            ReviewWebsiteCustomerViewPo reviewWebsiteCustomerViewPo)
        {
            ReviewWebsiteCustomerView reviewWebsiteCustomerView = null;
            if (!reviewWebsiteCustomerViewPo.IsNullOrEmpty())
            {
                reviewWebsiteCustomerView = new ReviewWebsiteCustomerView
                {
                    Id = reviewWebsiteCustomerViewPo.Id,
                    Type = reviewWebsiteCustomerViewPo.Type,
                    Email = reviewWebsiteCustomerViewPo.Email,
                    Avatar = reviewWebsiteCustomerViewPo.Avatar,
                    Name = reviewWebsiteCustomerViewPo.Name,
                    CountryName = reviewWebsiteCustomerViewPo.CountryName,
                    DateCreated = reviewWebsiteCustomerViewPo.DateCreated,
                    LanguageChName = reviewWebsiteCustomerViewPo.LanguageChName,
                    Recommend = reviewWebsiteCustomerViewPo.Recommend,
                    IsValid = reviewWebsiteCustomerViewPo.IsValid,
                    Content = reviewWebsiteCustomerViewPo.Content,
                    RepContent = reviewWebsiteCustomerViewPo.RepContent
                };
            }
            return reviewWebsiteCustomerView;
        }

        internal static WebSiteReview GetReviewWebsiteVoFromPo(ReviewWebsitePo webSiteReviewPo)
        {
            WebSiteReview reviewWebsite = null;
            if (!webSiteReviewPo.IsNullOrEmpty())
            {
                reviewWebsite = new WebSiteReview
                {
                    Id = webSiteReviewPo.ReviewId,
                    CustomerId = webSiteReviewPo.CustomerId,
                    ReviewType = (ReviewType)Convert.ToInt32(webSiteReviewPo.Type),
                    OrderId = webSiteReviewPo.OrderId,
                    LanguageId = webSiteReviewPo.LanguageId,
                    Rating = webSiteReviewPo.ReviewRating,
                    ReviewContent = webSiteReviewPo.ReviewContent,
                    CreateDateTime = webSiteReviewPo.DateCreated,
                    AuditStatus = (AuditStatus)webSiteReviewPo.Status,
                    IsValid = webSiteReviewPo.IsValid,
                    IsRecommend = webSiteReviewPo.Recommend,
                    ReplyContent = webSiteReviewPo.ReplyContent,
                    AdminId = webSiteReviewPo.AdminId,
                    ReplyDateTime = webSiteReviewPo.ReplyDate
                };
            }
            return reviewWebsite;
        }
        #endregion
    }
}