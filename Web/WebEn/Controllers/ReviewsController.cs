using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Review;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Order;
using Com.Panduo.Web.Models.Review;

namespace Com.Panduo.Web.Controllers
{
    public class ReviewsController : BaseController
    {
        private int CustomerId
        {
            get { return SessionHelper.CurrentCustomer.CustomerId; }
        }

        /// <summary>
        /// 获取MyProduct
        /// </summary>
        /// <returns></returns>
        public ActionResult Write()
        {
            ViewBag.Sitemaps = Sitemap.GetDetailReivewSitemap();
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(20);//页大小
            int orderId = Request["order_id"].ParseTo(0);

            var orderInfo = ServiceFactory.OrderService.GetOrderByOrderId(orderId);
            if (orderInfo == null || orderInfo.CustomerId != CustomerId)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            if (orderInfo.IsReviewAll == true)
            {
                return Redirect(UrlRewriteHelper.GetReadReviews(orderId));
            }

            IDictionary<OrderDetailSearchCriteria, object> searchCriteria = new Dictionary<OrderDetailSearchCriteria, object>();
            searchCriteria.Add(OrderDetailSearchCriteria.IsReviewed, false);

            var myProductsPageData = ServiceFactory.OrderService.GetOrderDetsById(CustomerId, orderId, page,
                pageSize, searchCriteria);
            var myProducts = myProductsPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(myProducts.Select(p => p.ProductId).ToList(), isIncludeProductStock: false, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: false, isJudgeHotSeller: false, isJudgeHasSimilarProuct: false);
            var vos = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = myProducts.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();

            var pageData = new PageData<OrderDetailItemVo>
            {
                Data = vos,
                Pager = myProductsPageData.Pager
            };
            if (Request.IsAjaxRequest())
            {
                return View("Partial/_WriteList", pageData);
            }
            ViewBag.OrderId = orderId;
            return View(pageData);
        }

        [HttpPost]
        public JsonResult Submit(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                int orderId = form["order_id"].ParseTo(0);
                string orderDetailIds = form["order_detail_ids"] ?? string.Empty;
                string orderProductIds = form["order_product_ids"] ?? string.Empty;
                int rating = 0;
                string reviewContent = form["review_content_0"] ?? string.Empty;
                var orderDetailIdList = orderDetailIds.Split<int>(",").ToList();
                var orderProductIdList = orderProductIds.Split<int>(",").ToList();
                var createDate = DateTime.Now;

                if (reviewContent != string.Empty && reviewContent.Length <= 800)//批量评论
                {
                    rating = form["rating_0"].ParseTo(0);

                    if (rating > 0)
                    {
                        for (int i = 0; i < orderDetailIdList.Count; i++)
                        {
                            var productReview = new ProductReview();
                            productReview.CustomerId = CustomerId;
                            productReview.OrderId = orderId;
                            productReview.OrderProductId = orderDetailIdList[i];
                            productReview.ProductId = orderProductIdList[i];
                            productReview.LanguageId = ServiceFactory.ConfigureService.SiteLanguageId;
                            productReview.Rating = rating;
                            productReview.ReviewContent = reviewContent;
                            productReview.CreateDateTime = createDate;
                            productReview.AdminId = 0;
                            productReview.ReplyContent = string.Empty;
                            productReview.AuditStatus = (int)AuditStatus.NotAudit;
                            productReview.IsValid = true;

                            ServiceFactory.ReviewService.AddProductReview(productReview);
                            ServiceFactory.OrderService.UpdateOrderItemIsReviewedById(orderDetailIdList[i], true);
                            //todo 评论成功后发送邮件给销售,接口未实现
                            var customer = ServiceFactory.CustomerService.GetCustomerById(CustomerId);
                            ServiceFactory.MailService.ReviewEmail(customer.FullName,customer.Email,DateTime.Now,"");
                        }
                    }
                }
                else//当前页面所有商品评论
                {
                    for (int i = 0; i < orderDetailIdList.Count; i++)
                    {
                        int productId = orderProductIdList[i];
                        reviewContent = form["review_content_" + productId] ?? string.Empty;
                        rating = form["rating_" + productId].ParseTo(0);

                        if (rating > 0 && reviewContent != string.Empty && reviewContent.Length <= 800)
                        {
                            var productReview = new ProductReview();
                            productReview.CustomerId = CustomerId;
                            productReview.OrderId = orderId;
                            productReview.OrderProductId = orderDetailIdList[i];
                            productReview.ProductId = orderProductIdList[i];
                            productReview.LanguageId = ServiceFactory.ConfigureService.SiteLanguageId;
                            productReview.Rating = rating;
                            productReview.ReviewContent = reviewContent;
                            productReview.CreateDateTime = createDate;
                            productReview.AdminId = 0;
                            productReview.ReplyContent = string.Empty;
                            productReview.AuditStatus = (int)AuditStatus.NotAudit;
                            productReview.IsValid = true;

                            ServiceFactory.ReviewService.AddProductReview(productReview);
                            ServiceFactory.OrderService.UpdateOrderItemIsReviewedById(orderDetailIdList[i], true);
                        }
                    }
                }

                IDictionary<OrderDetailSearchCriteria, object> searchCriteria = new Dictionary<OrderDetailSearchCriteria, object>();
                searchCriteria.Add(OrderDetailSearchCriteria.IsReviewed, false);

                var myProductsPageData = ServiceFactory.OrderService.GetOrderDetsById(CustomerId, orderId, 1,
                    1, searchCriteria);
                //如果没有发现未评论的，修改订单主表里的评论状态
                if (myProductsPageData.Pager.TotalRowCount <= 0)
                {
                    ServiceFactory.OrderService.UpdateOrderIsReservationById(orderId, true);
                }

                hashtable["result"] = ActionJsonResult.Success;
            }
            catch (Exception ex)
            {
                //记录日志
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = ex.Message;
            }
            return Json(hashtable);//, JsonRequestBehavior.AllowGet
        }

        public ActionResult Read()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(20);//页大小
            int orderId = Request["order_id"].ParseTo(0);

            var orderInfo = ServiceFactory.OrderService.GetOrderByOrderId(orderId);
            if (orderInfo == null || orderInfo.CustomerId != CustomerId)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var myProductsPageData = ServiceFactory.ReviewService.FindProductReviewsByOrderId(page,
                pageSize, orderId, null, null);
            var myProducts = myProductsPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(myProducts.Select(p => p.ProductId).ToList(), isIncludeProductStock: false, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: false, isJudgeHotSeller: false, isJudgeHasSimilarProuct: false);
            var vos = productInfos.Select(c => new ReviewProductsProductInfoVo { ProductInfo = c, ProductReviewInfo = myProducts.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();

            var pageData = new PageData<ReviewProductsProductInfoVo>
            {
                Data = vos,
                Pager = myProductsPageData.Pager
            };
            if (Request.IsAjaxRequest())
            {
                return View("Partial/_ReadList", pageData);
            }
            ViewBag.OrderId = orderId;
            ViewBag.OrderInfo = orderInfo;
            return View(pageData);
        }



        /// <summary>
        /// 批量设置Wishlist
        /// </summary>
        /// <returns></returns>
        public ActionResult SetMyWishList()
        {
            IDictionary<int, WishListType> dic = new Dictionary<int, WishListType>();
            var t = Request["ckb"] ?? string.Empty;
            var ishistory = Request["h"].IsNullOrEmpty() ? false : true;
            string pre = "a_";//前缀
            if (!t.IsNullOrEmpty())
            {
                var productlist = t.Split<int>(",");
                foreach (var i in productlist)
                {
                    var j = Request[pre + i.ToString()] ?? ((int)WishListType.Unclassified).ToString();
                    dic.Add(i, j.ToInt().ToEnum<WishListType>());
                }
                ServiceFactory.WishListService.SetWishListType(CustomerId, dic, ishistory);
            }
            return Content("sss");
        }


        #region Testimonial
        [HttpGet]
        public ActionResult Testimonial(int page = 1, int pageSize = 20, int filter = 0)
        {
            ViewBag.Sitemaps = Sitemap.GetTestimonialSitemap();
            IDictionary<CustomerReviewSearchCriteria, object> searchCriteria = new Dictionary<CustomerReviewSearchCriteria, object>();
            if (filter != 0)
                searchCriteria.Add(CustomerReviewSearchCriteria.LanguageId, filter);

            var reviewWebsiteCustomerViews = ServiceFactory.ReviewService.FindCustomerWebSiteReviewsByType(page, pageSize, ReviewType.Web, searchCriteria, null);
            if (Request.IsAjaxRequest())
            {
                return View("TestimonialList", reviewWebsiteCustomerViews);
            }
            return View(reviewWebsiteCustomerViews);
        }

        #endregion
    }
}
