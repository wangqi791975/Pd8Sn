
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Message
{
    /// <summary>
    ///描述：ticket回复表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:52
    /// </summary>
    [Class(Table = "t_ticket_reply", Lazy = false, NameType = typeof(TicketReplyPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class TicketReplyPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "ReplyId", Column = "reply_id")]
        [Generator(2, Class = "native")]

        public virtual int ReplyId
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
        /// 回复内容
        /// </summary>
        [Property(Column = "tiopic_content")]
        public virtual string TiopicContent
        {
            get;
            set;
        }
        /// <summary>
        /// 回复类型(0:客户,1:客服)
        /// </summary>
        [Property(Column = "user_type")]
        public virtual bool UserType
        {
            get;
            set;
        }
        /// <summary>
        /// 答复人
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户读取状态(0:未读,1:已读)
        /// </summary>
        [Property(Column = "customer_read_status")]
        public virtual bool CustomerReadStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员读取状态(0:未读,1:已读)2
        /// </summary>
        [Property(Column = "admin_read_status")]
        public virtual bool AdminReadStatus
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

