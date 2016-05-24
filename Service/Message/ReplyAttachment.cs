using System;

namespace Com.Panduo.Service.Message
{
    /// <summary>
    /// 站内信附件
    /// </summary>
    [Serializable]
    public class ReplyAttachment
    {
        /// <summary>
        /// 附件Id
        /// </summary>
        public virtual int AttachmentId
        {
            get;
            set;
        }
        /// <summary>
        /// 附件名称
        /// </summary>
        public virtual string AttachmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 附件地址
        /// </summary>
        public virtual string AttachmentUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 附件大小
        /// </summary>
        public virtual double AttachmentSize
        {
            get;
            set;
        }

        /// <summary>
        /// 附件原文件名
        /// </summary>
        public virtual string OriginalName
        {
            get;
            set;
        }
    }
}
