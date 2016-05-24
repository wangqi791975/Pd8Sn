
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SystemMail
{
    /// <summary>
    ///描述：t_email_logo ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:10:21
    /// </summary>
    [Class(Table = "t_email_logo", Lazy = false, NameType = typeof(EmailLogoPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class EmailLogoPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "LogoId", Column = "logo_id")]
        [Generator(2, Class = "native")]
        public virtual int LogoId
        {
            get;
            set;
        }
        /// <summary>
        /// 语种ID
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// logo图片
        /// </summary>
        [Property(Column = "logo_image")]
        public virtual string LogoImage
        {
            get;
            set;
        }
        /// <summary>
        /// logo链接地址
        /// </summary>
        [Property(Column = "logo_link")]
        public virtual string LogoLink
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:未使用，1:正在使用)
        /// </summary>
        [Property(Column = "status")]
        public virtual bool Status
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
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

