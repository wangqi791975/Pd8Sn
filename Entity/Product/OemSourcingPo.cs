
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：t_oem_sourcing ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:10:22
    /// </summary>
    [Class(Table = "t_oem_sourcing", Lazy = false, NameType = typeof(OemSourcingPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OemSourcingPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "OemId", Column = "oem_id")]
        [Generator(2, Class = "native")]
        public virtual int OemId
        {
            get;
            set;
        }
        /// <summary>
        /// 标题或链接
        /// </summary>
        [Property(Column = "title_link")]
        public virtual string TitleLink
        {
            get;
            set;
        }
        /// <summary>
        /// 详细内容
        /// </summary>
        [Property(Column = "detail_content")]
        public virtual string DetailContent
        {
            get;
            set;
        }
        /// <summary>
        /// 原始附件名称
        /// </summary>
        [Property(Column = "original_attachment_name")]
        public virtual string OriginalAttachmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 附件存在服务器上的名称
        /// </summary>
        [Property(Column = "attachment_name")]
        public virtual string AttachmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 附件链接
        /// </summary>
        [Property(Column = "attachment_link")]
        public virtual string AttachmentLink
        {
            get;
            set;
        }
        /// <summary>
        /// 客户名称
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 客户Email
        /// </summary>
        [Property(Column = "customer_name")]
        public virtual string CustomerName
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

