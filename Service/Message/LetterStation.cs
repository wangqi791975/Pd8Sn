using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Message
{
    /// <summary>
    /// 站内信主题
    /// </summary>
    [Serializable]
    public class LetterStation
    {
        /// <summary>
        /// 站内信主题ID 
        /// </summary>
        public virtual int LetterId
        {
            get;
            set;
        }

        /// <summary>
        /// 主题
        /// </summary>
        public virtual string Subject { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 关联类型(产品或订单)
        /// </summary>
        public virtual int RelationType { get; set; }

        /// <summary>
        /// 关联ID 产品或订单
        /// </summary>
        public virtual int RelationId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 客服ID
        /// </summary>
        public virtual int ServiceId
        {
            get;
            set;
        }

        /// <summary>
        /// 客服读取状态 
        /// </summary>
        public virtual int ServiceReadStatus
        {
            get;
            set;
        }


        /// <summary>
        /// 状态(删除或关闭 )
        /// </summary>
        public virtual int Status
        {
            get;
            set;
        }



        /// <summary>
        /// 客户读取状态
        /// </summary>
        public virtual int CustomerReadStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 发起类型：客户或客服 
        /// </summary>
        public virtual int StartType
        {
            get;
            set;
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime LastModifyTime
        {
            get;
            set;
        }

        /// <summary>
        /// 站内信主题答复列表
        /// </summary>
        public virtual List<LetterReply> Reply { get; set; }

    }

}