
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：找回密码表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:00
    /// </summary>
    [Class(Table = "t_forgot_password", Lazy = false, NameType = typeof(ForgotPasswordPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ForgotPasswordPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]

        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 客户id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int? CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 加密串
        /// </summary>
        [Property(Column = "encrypted_string")]
        public virtual string EncryptedString
        {
            get;
            set;
        }
        /// <summary>
        /// 找回时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 有效时间
        /// </summary>
        [Property(Column = "date_expired")]
        public virtual DateTime? DateExpired
        {
            get;
            set;
        }
        /// <summary>
        /// 0未使用，1已使用
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 使用时间
        /// </summary>
        [Property(Column = "date_used")]
        public virtual DateTime? DateUsed
        {
            get;
            set;
        }
    }
}

