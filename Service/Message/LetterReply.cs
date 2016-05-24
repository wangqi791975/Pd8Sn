using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Message
{
    /// <summary>
    /// 站内信答复
    /// </summary>
    [Serializable]
    public class LetterReply
    {
        /// <summary>
        /// 站内信ID
        /// </summary>
        public virtual int LetterId
        {
            get;
            set;
        }
        /// <summary>
        /// 答复内容
        /// </summary>
        public virtual string ReplyContent
        {
            get;
            set;
        }

        /// <summary>
        /// 答复人
        /// </summary>
        public virtual int ReplyId
        {
            get;
            set;
        }

        /// <summary>
        /// 答复类型（客户或客服）
        /// </summary>
        public virtual int ReplyType
        {
            get;
            set;
        }
        /// <summary>
        /// 答复时间
        /// </summary>
        public virtual DateTime ReplyDatatime
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status
        {
            get;
            set;
        }

        ///答复附件
        public virtual List<ReplyAttachment> Attachment
        {
            get;
            set;
        }

        /// <summary>
        /// 答复Id
        /// </summary>
        public virtual int LetterReplyId
        {
            get;
            set;
        }


    }
}
