using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 客户头像
    /// </summary>
    [Serializable]
    public class CustomerAvatar
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int AvatarId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户id
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// full name
        /// </summary>
        public virtual string FullName
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string CustomerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 提交头像
        /// </summary>
        public virtual string SubmitAvatar
        {
            get;
            set;
        }
        /// <summary>
        /// 审核状态(10:未审核、20:拒绝、30:接受或已审核)
        /// </summary>
        public virtual AuditingStatus AuditingStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public virtual string RefuseReason
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员操作时间
        /// </summary>
        public virtual DateTime? AdminOperationTime
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int? AdminId
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditingStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        NotAuditing = 10,
        /// <summary>
        /// 已拒绝
        /// </summary>
        Refuse = 20,
        /// <summary>
        /// 已审核或接受
        /// </summary>
        Accepted = 30
    }
}
