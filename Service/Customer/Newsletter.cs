using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 订阅
    /// </summary>
    [Serializable]
    public class Newsletter
    {
        /// <summary>
        /// 订阅Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual  int? CustomerId { get; set; }

        /// <summary>
        /// 姓名（默认“Customer”）
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 订阅时间
        /// </summary>
        public virtual DateTime NewsletterDateTime { get; set; }

        /// <summary>
        /// 是否退订
        /// </summary>
        public virtual bool IsUnNewsletter { get; set; }
    }
}
