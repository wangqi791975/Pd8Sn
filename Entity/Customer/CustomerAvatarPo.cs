
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户头像表 ORM 映射类 
    ///创建人:万天文
    ///创建时间：03/30/2015 17:29:45
    /// </summary>
    [Class(Table = "t_customer_avatar", Lazy = false, NameType = typeof(CustomerAvatarPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerAvatarPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "AvatarId", Column = "avatar_id")]
        [Generator(2, Class = "native")]

        public virtual int AvatarId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// full name
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 提交头像
        /// </summary>
        [Property(Column = "submit_avatar")]
        public virtual string SubmitAvatar
        {
            get;
            set;
        }
        /// <summary>
        /// 审核状态(10:未审核、20:拒绝、30:接受或已审核)
        /// </summary>
        [Property(Column = "auditing_status")]
        public virtual int AuditingStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        [Property(Column = "refuse_reason")]
        public virtual string RefuseReason
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员操作时间
        /// </summary>
        [Property(Column = "admin_operation_time")]
        public virtual DateTime? AdminOperationTime
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int? AdminId
        {
            get;
            set;
        }

    }
}

