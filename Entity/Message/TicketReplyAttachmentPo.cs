
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Message
{
    /// <summary>
    ///描述：ticket回复附件表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:53
    /// </summary>
    [Class(Table = "t_ticket_reply_attachment", Lazy = false, NameType = typeof(TicketReplyAttachmentPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class TicketReplyAttachmentPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "AttachmentId", Column = "attachment_id")]
        [Generator(2, Class = "native")]

        public virtual int AttachmentId
        {
            get;
            set;
        }
        /// <summary>
        /// 回复ID
        /// </summary>
        [Property(Column = "reply_id")]
        public virtual int ReplyId
        {
            get;
            set;
        }
        /// <summary>
        /// 附件
        /// </summary>
        [Property(Column = "attachment")]
        public virtual string Attachment
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }
}

