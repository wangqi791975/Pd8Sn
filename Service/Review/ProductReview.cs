using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Review
{
    /// <summary>
    /// 产品评论
    /// </summary>
    [Serializable]
    public class ProductReview
    {
        /// <summary>
        /// 产品评论Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 订单明细Id
        /// </summary>
        public virtual int OrderProductId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 评论时的语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

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
        /// 是否有效（用于控制评论显示隐藏）
        /// </summary>
        public virtual bool IsValid { get; set; }
    }
}
