using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 客户等级名称表
    /// </summary>
    [Serializable]
    public class CustomerGroupDesc
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户等级Id
        /// </summary>
        public virtual int CustomerGroupId { get; set; }

        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public virtual string GroupName { get; set; }
    }
}
