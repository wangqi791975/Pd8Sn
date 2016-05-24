
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Message
{
    /// <summary>
    ///描述：ticket附件表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:56
    /// </summary>
    [Class(Table = "t_ticket_topic_attachment", Lazy = false, NameType = typeof(TicketTopicAttachmentPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class TicketTopicAttachmentPo
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
        /// ticket ID
        /// </summary>
        [Property(Column = "ticket_id")]
        public virtual int TicketId
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

