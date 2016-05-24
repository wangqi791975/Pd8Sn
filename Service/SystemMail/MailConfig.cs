using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.Service.SystemMail
{
    public class MailConfig
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public virtual MailType Type { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 发件人名字
        /// </summary>
        public virtual string FromName { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public virtual string From { get; set; }

        /// <summary>
        /// 收件人名称
        /// </summary>
        public virtual string ToName { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public virtual string To { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        public virtual string Cc { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual int Creator { get; set; }

        /// <summary>
        /// 1客户 0销售
        /// </summary>
        public virtual int Receiver { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
