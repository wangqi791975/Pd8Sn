using System;
using System.ComponentModel;
using Panduo.Com.Email.SaveXml.Entity;


namespace Com.Panduo.Service.SystemMail
{
    public class EmailProduceLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 邮件类型 对应t_email_enum_type、枚举值
        /// </summary>
        public virtual MailType EmailType { get; set; }

        /// <summary>
        /// 关联Id 
        /// 订单相关邮件：这个字段存订单编号
        /// 客户相关邮件：这个字段存客户编号
        /// 订单和客户优先存订单编号
        /// </summary>
        public virtual string KeyNo { get; set; }

        /// <summary>
        /// 是否有附件
        /// </summary>
        public virtual bool HasAttachment { get; set; }

        /// <summary>
        /// 附件文件名 多个附件用|分割
        /// </summary>
        public virtual string Attachment { get; set; }

        /// <summary>
        /// 是否生成邮件XML
        /// </summary>
        [DefaultValue(false)]
        public virtual bool IsCreatedXml { get; set; }

        /// <summary>
        /// 生成XML时间
        /// </summary>
        public virtual DateTime DateCreatedXml { get; set; }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public virtual DateTime DateAdded { get; set; }


        /// <summary>
        /// 生成Xml错误次数
        /// </summary>
        public virtual int ErrCount { get; set; }

        /// <summary>
        /// 生成Xml错误消息
        /// </summary>
        public virtual int ErrMsg { get; set; }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId { get; set; }
    }
}
