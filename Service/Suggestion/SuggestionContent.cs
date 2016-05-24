using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Suggestion
{
    /// <summary>
    /// 客户建议
    /// </summary>
    [Serializable]
    public class SuggestionContent
    {
        /// <summary>
        /// 客户建议Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 客户邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 建议内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 评分对象明细
        /// </summary>
        public virtual List<SuggestionDetail> Details { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public virtual List<SuggestionAttachment> AttachmentList { get; set; }

        /// <summary>
        /// 建议时间
        /// </summary>
        public virtual DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 答复时间
        /// </summary>
        public virtual DateTime ReplyDate { get; set; }

        /// <summary>
        /// 答复内容
        /// </summary>
        public virtual string ReplyContent { get; set; }
    }
}
