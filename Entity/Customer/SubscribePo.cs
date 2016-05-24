
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：订阅表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:50
    /// </summary>
    [Class(Table = "t_subscribe", Lazy = false, NameType = typeof(SubscribePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class SubscribePo
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
        /// 游客为0
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int? CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Property(Column = "email")]
        public virtual string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 订阅时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 1有效，0无效（退订）
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }

        /// <summary>
        /// 1已上传至mailchiamp，0未上传
        /// </summary>
        [Property(Column = "is_uploaded")]
        public virtual bool IsUploaded
        {
            get;
            set;
        }

        /// <summary>
        /// 语言别
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名（默认“Customer”）
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
        {
            get;
            set;
        }
    }
}

