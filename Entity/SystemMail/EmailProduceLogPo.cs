
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SystemMail
{
    /// <summary>
    ///描述：t_email_produce_log ORM 映射类 
    ///创建人:lxf
    ///创建时间：2015-04-17 11:10:21
    /// </summary>
    [Class(Table = "t_email_produce_log", Lazy = false, NameType = typeof(EmailProduceLogPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class EmailProduceLogPo
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
        /// 邮件类型 对应t_email_enum_type、枚举值
        /// </summary>
        [Property(Column = "email_type")]
        public virtual int EmailType
        {
            get;
            set;
        }
        /// <summary>
        /// 关联Id
        /// </summary>
        [Property(Column = "key_no")]
        public virtual string KeyNo
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有附件
        /// </summary>
        [Property(Column = "has_attachment")]
        public virtual bool HasAttachment
        {
            get;
            set;
        }
        /// <summary>
        /// 附件文件名 多个附件用|分割
        /// </summary>
        [Property(Column = "attachment")]
        public virtual string Attachment
        {
            get;
            set;
        }
        /// <summary>
        /// 是否生成邮件XML
        /// </summary>
        [Property(Column = "is_created_xml")]
        public virtual bool IsCreatedXml
        {
            get;
            set;
        }
        /// <summary>
        /// 生成XML时间
        /// </summary>
        [Property(Column = "date_created_xml")]
        public virtual DateTime DateCreatedXml
        {
            get;
            set;
        }
        /// <summary>
        /// 日志记录时间
        /// </summary>
        [Property(Column = "date_added")]
        public virtual DateTime DateAdded
        {
            get;
            set;
        }


        /// <summary>
        /// 生成Xml错误次数
        /// </summary>
        [Property(Column = "err_count")]
        public virtual int ErrCount
        {
            get;
            set;
        }
        /// <summary>
        /// 生成Xml错误消息
        /// </summary>
        [Property(Column = "err_msg")]
        public virtual int ErrMsg
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
    }
}

