using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式多语种
    /// </summary>
    [Serializable]
    public class ShippingLanguage
    {
        /// <summary>
        /// 唯一标识ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 配送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 配送方式简述
        /// </summary>
        public virtual string ShippingDescription { get; set; }
    }
}
