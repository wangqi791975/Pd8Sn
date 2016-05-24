
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Message
{
    /// <summary>
    ///描述：网站ticket表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:54
    /// </summary>
    [Class(Table = "t_ticket_topic", Lazy = false, NameType = typeof(TicketTopicPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class TicketTopicPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "TicketId", Column = "ticket_id")]
        [Generator(2, Class = "native")]
        public virtual int TicketId
        {
            get;
            set;
        }
        /// <summary>
        /// 关联ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 主题内容
        /// </summary>
        [Property(Column = "tiopic_content")]
        public virtual string TiopicContent
        {
            get;
            set;
        }
        /// <summary>
        /// 发送/回复人
        /// </summary>
        [Property(Column = "send_user_id")]
        public virtual int SendUserId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型(0:发送,1:回复)
        /// </summary>
        [Property(Column = "send_type")]
        public virtual bool SendType
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
        /// 状态(0:关闭,1:开启)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }
}

