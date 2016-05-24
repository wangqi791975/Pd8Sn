using System;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 找回密码日志
    /// </summary>
     [Serializable]
    public class ForgotPassword
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 客户id
        /// </summary>
        public virtual int? CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 加密串
        /// </summary>
        public virtual string EncryptedString
        {
            get;
            set;
        }

        /// <summary>
        /// 找回时间
        /// </summary>
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 有效时间
        /// </summary>
        public virtual DateTime? DateExpired
        {
            get;
            set;
        }

        /// <summary>
        /// 0未使用，1已使用
        /// </summary>
        public virtual bool Status
        {
            get;
            set;
        }

        /// <summary>
        /// 使用时间
        /// </summary>
        public virtual DateTime? DateUsed
        {
            get;
            set;
        }
    }
}