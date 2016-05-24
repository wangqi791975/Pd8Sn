
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SystemMail
{
    /// <summary>
    ///描述：t_email_config ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:10:21
    /// </summary>
    [Class(Table = "t_email_config", Lazy = false, NameType = typeof(EmailConfigPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class EmailConfigPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 配置类型(如10:注册、20:订单提交、30:网站建议等)
        /// </summary>
        [Property(Column = "`type`")]
        public virtual int Type
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
        /// 发件人名称
        /// </summary>
        [Property(Column = "from_name")]
        public virtual string FromName
        {
            get;
            set;
        }
        /// <summary>
        /// 收件人名称
        /// </summary>
        [Property(Column = "to_name")]
        public virtual string ToName
        {
            get;
            set;
        }
        /// <summary>
        /// Email From
        /// </summary>
        [Property(Column = "email_form")]
        public virtual string EmailForm
        {
            get;
            set;
        }
        /// <summary>
        /// Email To
        /// </summary>
        [Property(Column = "email_to")]
        public virtual string EmailTo
        {
            get;
            set;
        }
        /// <summary>
        /// Email CC
        /// </summary>
        [Property(Column = "email_cc")]
        public virtual string EmailCc
        {
            get;
            set;
        }
        /// <summary>
        /// 接收者(0:销售，1:客户)
        /// </summary>
        [Property(Column = "receiver")]
        public virtual int Receiver
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

