using System;

namespace Com.Panduo.Service.Product
{
    public class OemSourcing
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int OemId
        {
            get;
            set;
        }

        /// <summary>
        /// 标题或链接
        /// </summary>
        public virtual string TitleLink
        {
            get;
            set;
        }

        /// <summary>
        /// 详细内容
        /// </summary>
        public virtual string DetailContent
        {
            get;
            set;
        }

        /// <summary>
        /// 原始附件名称
        /// </summary>
        public virtual string OriginalAttachmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 附件存在服务器上的名称
        /// </summary>
        public virtual string AttachmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 附件链接
        /// </summary>
        public virtual string AttachmentLink
        {
            get;
            set;
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string CustomerEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 客户Email
        /// </summary>
        public virtual string CustomerName
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }
}

