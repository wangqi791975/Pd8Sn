
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Review
{
    /// <summary>
    ///描述：网站订单评论表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:35
    /// </summary>
    [Class(Table = "t_review_website", Lazy = false, NameType = typeof(ReviewWebsitePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ReviewWebsitePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "ReviewId", Column = "review_id")]
        [Generator(2, Class = "native")]

        public virtual int ReviewId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 评论类型(0:网站评论,1:订单评论)
        /// </summary>
        [Property(Column = "`type`")]
        public virtual bool Type
        {
            get;
            set;
        }
        /// <summary>
        /// 订单号(网站评论插入0)
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言ID
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 评分
        /// </summary>
        [Property(Column = "review_rating")]
        public virtual int ReviewRating
        {
            get;
            set;
        }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Property(Column = "review_content")]
        public virtual string ReviewContent
        {
            get;
            set;
        }
        /// <summary>
        /// 评论时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:未审核,1:审核通过,2:审核不通过)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有效(0:无效,1:有效)
        /// </summary>
        [Property(Column = "is_valid")]
        public virtual bool IsValid
        {
            get;
            set;
        }
        /// <summary>
        /// 是否首页推荐
        /// </summary>
        [Property(Column = "recommend")]
        public virtual bool Recommend
        {
            get;
            set;
        }
        /// <summary>
        /// 客服答复内容
        /// </summary>
        [Property(Column = "reply_content")]
        public virtual string ReplyContent
        {
            get;
            set;
        }
        /// <summary>
        /// 客服ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int? AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 客服答复时间
        /// </summary>
        [Property(Column = "reply_date")]
        public virtual DateTime? ReplyDate
        {
            get;
            set;
        }
    }
}

