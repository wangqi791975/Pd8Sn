using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Review
{
    /// <summary>
    /// 评论接口
    /// </summary>
    public interface IReviewService
    {
        #region 常量
        /// <summary>
        /// 不能发起产品评论
        /// </summary>
        string ERROR_CAN_NOT_REVIEW_WITHOUT_BUY_PRODUCT { get; }

        /// <summary>
        /// 该产品评论已有人答复
        /// </summary>
        string ERROR_ALREADY_REPLY { get; }

        /// <summary>
        /// 不能重复评论
        /// </summary>
        string ERROR_CAN_NOT_REVIEW_AGAIN { get; }

        /// <summary>
        /// 该网站订单评论已有人答复
        /// </summary>
        string ERROR_ALREADY_REPLAY_WEBSITE { get; }

        /// <summary>
        /// 订单明细不能重复评论
        /// </summary>
        string ERROR_ORDER_PRODUCT_DUPLICATE_REVIEW { get; }
        #endregion

        #region 方法

        #region 产品评论
        /// <summary>
        /// 发起产品评论,修改订单和明细的is_review_all状态
        /// </summary>
        /// <param name="productReview">要发起的评论</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CAN_NOT_REVIEW_WITHOUT_BUY_PRODUCT:不能发起评论，必须购买此产品之后才能评论</value>
        ///     <value>ERROR_ORDER_PRODUCT_DUPLICATE_REVIEW:订单明细不能重复评论</value>
        /// </exception>
        /// <returns>新建产品评论Id</returns>
        int AddProductReview(ProductReview productReview);

        /// <summary>
        /// 答复产品评论（重复答复覆盖原先内容）
        /// </summary>
        /// <param name="productReviewId">产品评论Id</param>
        /// <param name="adminId">答复人Id</param>
        /// <param name="replyDateTime">答复时间</param>
        /// <param name="replyContent">答复内容</param>
        void ReplyProductReview(int productReviewId, int adminId, DateTime replyDateTime, string replyContent);

        /// <summary>
        /// 修改产品评论状态
        /// </summary>
        /// <param name="productReviewId">产品评论Id</param>
        /// <param name="isValid">是否有效</param>
        void UpdateProductReviewStatus(int productReviewId, bool isValid);

        /// <summary>
        /// 修改产品评论审核状态
        /// </summary>
        /// <param name="productReviewId">产品评论Id</param>
        /// <param name="auditStatus">审核状态</param>
        void UpdateProductReviewAduitStatus(int productReviewId, AuditStatus auditStatus);

        /// <summary>
        /// 通过产品评论Id获取评论
        /// </summary>
        /// <param name="productReviewId">产品评论Id</param>
        /// <returns>产品评论</returns>
        ProductReview GetProductReviewById(int productReviewId);

        /// <summary>
        /// 获取客户评论数
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        int GetCustomerReivewsCount(int customerId);

        /// <summary>
        /// 通过订单Id获取所有产品评论
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>分页的商品评论</returns>
        PageData<ProductReview> FindProductReviewsByOrderId(int currentPage, int pageSize, int orderId, IDictionary<ProductReviewSearchCriteria, object> searchCriteria, IList<Sorter<ReviewSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 通过产品Id获取所有产品评论
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="productId">产品Id</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>分页的商品评论</returns>
        PageData<ProductReview> FindProductReviewsByProductId(int currentPage, int pageSize, int productId, IDictionary<ProductReviewSearchCriteria, object> searchCriteria, IList<Sorter<ReviewSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 通过产品Id获取所有产品评论
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <param name="productId">产品Id</param>
        /// <returns>分页的商品评论</returns>
        PageData<ReviewProductCustomerView> FindCustomerProductReviewsByProductId(int currentPage, int pageSize, IDictionary<CustomerReviewSearchCriteria, object> searchCriteria, IList<Sorter<CustomerReviewSorterCriteria>> sorterCriteria, int? productId);

        /// <summary>
        /// 通过评论Id获取产品评论
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <returns>评论</returns>
        ReviewProductCustomerView GetReviewProductCustomerViewById(int id);

        /// <summary>
        /// 通过订单明细Id获取订单评论
        /// </summary>
        /// <param name="orderProductId">订单明细Id</param>
        /// <returns>产品评论实体</returns>
        ProductReview GetProductReviewsByOrderProductId(int orderProductId);

        /// <summary>
        /// 获取产品评论数和平均分
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>返回相关项（评论数，平均分）</returns>
        IDictionary<Rating, decimal> GetRatingByProductId(int productId);

        /// <summary>
        /// 通过客户Id产品Id获取未评论
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        NotReviewProductView GetNotReviewProductView(int customerId, int productId);

        #endregion

        #region 网站订单评论

        /// <summary>
        /// 发起网站订单评论
        /// </summary>
        /// <param name="webSiteReview">网站订单评论</param>
        /// <returns>新建网站订单评论Id</returns>
        int AddWebSiteReview(WebSiteReview webSiteReview);

        /// <summary>
        /// 答复网站订单评论（答复覆盖原先内容），答复时间里面取
        /// </summary>
        /// <param name="webSiteReviewId">网站订单评论Id</param>
        /// <param name="adminId">答复人Id</param>
        /// <param name="replyDateTime">答复时间</param>
        /// <param name="replyContent">答复内容</param>
        void ReplyWebSiteReview(int webSiteReviewId, int adminId, DateTime replyDateTime, string replyContent);

        /// <summary>
        /// 修改网站订单评论
        /// </summary>
        /// <param name="webSiteReviewId">网站订单评论Id</param>
        /// <param name="isValid">是否有效</param>
        void UpdateWebSiteReviewStatus(int webSiteReviewId, bool isValid);

        /// <summary>
        /// 修改网站订单审核评论
        /// </summary>
        /// <param name="webSiteReviewId">网站订单评论Id</param>
        /// <param name="auditStatus">审核状态</param>
        void UpdateWebSiteReviewAuditStatus(int webSiteReviewId, AuditStatus auditStatus);

        /// <summary>
        /// 修改首页推荐状态
        /// </summary>
        /// <param name="webSiteReviewId">网站订单评论Id</param>
        /// <param name="isRecommend">是否推荐</param>
        void UpdateIsRecommend(int webSiteReviewId, bool isRecommend);

        /// <summary>
        /// 通过评论Id获取网站订单评论
        /// </summary>
        /// <param name="webSiteReviewId">网站订单评论Id</param>
        /// <returns>网站订单评论</returns>
        WebSiteReview GetWebSiteReviewById(int webSiteReviewId);

        /// <summary>
        /// 通过订单Id获取网站订单评论
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>网站订单评论集合</returns>
        IList<WebSiteReview> GetWebSiteReviewByOrderId(int orderId);

        /// <summary>
        /// 获取首页推荐评论
        /// </summary>
        /// <returns>首页推荐评论</returns>
        ReviewWebsiteCustomerView GetRecommendReview();

        /// <summary>
        /// 后台评论分页
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="reviewType">评论类型（网站，订单）</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<ReviewWebsiteCustomerView> FindCustomerWebSiteReviewsByType(int currentPage, int pageSize, ReviewType reviewType,
            IDictionary<CustomerReviewSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerReviewSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 通过评论Id获取站点评论
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <returns>评论</returns>
        ReviewWebsiteCustomerView GetReviewWebsiteCustomerViewById(int id);

        /// <summary>
        /// 通过评论类型获取网站订单评论
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="reviewType">评论类型（网站，订单）</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>分页返回评论</returns>
        PageData<WebSiteReview> FindWebSiteReviewsByType(int currentPage, int pageSize, ReviewType reviewType, IDictionary<WebSiteReviewSearchCriteria, object> searchCriteria, IList<Sorter<ReviewSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取首页推荐评论
        /// </summary>
        /// <param name="count">top数</param>
        /// <returns>网站订单评论</returns>
        IList<WebSiteReview> GetTopNRecommendWebSiteReviews(int count);

        #endregion

        #endregion
    }

    /// <summary>
    /// 评分相关项
    /// </summary>
    public enum Rating
    {
        /// <summary>
        /// 评论次数
        /// </summary>
        ReviewCount,
        /// <summary>
        /// 平均分值
        /// </summary>
        AvgScore
    }


    /// <summary>
    /// 产品评论查询条件
    /// </summary>
    public enum ProductReviewSearchCriteria
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        Email,
        /// <summary>
        /// 姓名
        /// </summary>
        Name,
        /// <summary>
        /// 创建时间
        /// </summary>
        DateCreated,
        /// <summary>
        /// 语种中文名
        /// </summary>
        LanguageChName,
        /// <summary>
        /// 是否显示
        /// </summary>
        IsValid
    }

    /// <summary>
    /// 网站订单评论查询条件
    /// </summary>
    public enum WebSiteReviewSearchCriteria
    {

    }

    /// <summary>
    /// 客户网站订单评论查询条件
    /// </summary>
    public enum CustomerReviewSearchCriteria
    {
        /// <summary>
        /// 关键字（名称或者Email）
        /// </summary>
        Keyword,
        /// <summary>
        /// 邮箱
        /// </summary>
        Email,
        /// <summary>
        /// 姓名
        /// </summary>
        Name,
        /// <summary>
        /// 语种Id
        /// </summary>
        LanguageId
    }

    /// <summary>
    /// 客户评论排序条件
    /// </summary>
    public enum CustomerReviewSorterCriteria
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        Email,
        /// <summary>
        /// 姓名
        /// </summary>
        Name,
        /// <summary>
        /// 创建时间
        /// </summary>
        DateCreated,
        /// <summary>
        /// 语种中文名
        /// </summary>
        LanguageChName,
        /// <summary>
        /// 是否显示
        /// </summary>
        IsValid
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public enum ReviewSorterCriteria
    {
        /// <summary>
        /// Id
        /// </summary>
        Id,
        /// <summary>
        /// 评分
        /// </summary>
        Rating,
        /// <summary>
        /// 评论时间
        /// </summary>
        CreateDateTime,
        /// <summary>
        /// 答复时间
        /// </summary>
        ReplyDateTime,
        /// <summary>
        /// 审核状态
        /// </summary>
        AuditStatus,
        /// <summary>
        /// 是否有效
        /// </summary>
        IsValid
    }

}
