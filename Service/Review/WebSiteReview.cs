using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Review
{
    /// <summary>
    /// 网站订单评论
    /// </summary>
    [Serializable]
    public class WebSiteReview
    {
        /// <summary>
        /// 网站订单评论Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单Id（评论类型为网站时存0）
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 评论时的语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 评论类型（网站，订单）
        /// </summary>
        public virtual ReviewType ReviewType { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public virtual int Rating { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public virtual string ReviewContent { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public virtual DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 答复人Id
        /// </summary>
        public virtual int? AdminId { get; set; }

        /// <summary>
        /// 答复内容
        /// </summary>
        public virtual string ReplyContent { get; set; }

        /// <summary>
        /// 答复时间
        /// </summary>
        public virtual DateTime? ReplyDateTime { get; set; }

        /// <summary>
        /// 审核状态（默认审核通过）
        /// </summary>
        public virtual AuditStatus AuditStatus { get; set; }

        /// <summary>
        /// 是否有效（用于控制评论的显示隐藏）
        /// </summary>
        public virtual bool IsValid { get; set; }

        /// <summary>
        /// 是否首页推荐
        /// </summary>
        public virtual bool IsRecommend { get; set; }
    }

    /// <summary>
    /// 网站订单评论类型
    /// </summary>
    public enum ReviewType
    {
        /// <summary>
        /// 网站
        /// </summary>
        Web,
        /// <summary>
        /// 订单
        /// </summary>
        Order
    }

    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        NotAudit,
        /// <summary>
        /// 审核通过
        /// </summary>
        AuditPass,
        /// <summary>
        /// 审核不通过
        /// </summary>
        AuditNotPass
    }
}
