using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 税号
    /// </summary>
    [Serializable]
    public class CustomsNo
    {
        /// <summary>
        /// 关税类型
        /// </summary>
        public virtual CustomsNoType CustomsNoType { get; set; }
        /// <summary>
        /// 是否必填项
        /// </summary>
        public virtual bool IsRequired { get; set; }
        /// <summary>
        /// 描述(和语言包拼接)
        /// </summary>
        public virtual int Note { set; get; }
    }

    public enum CustomsNoType
    {
        CustomsNo = 10,
        CnpjNo = 20,
        EoriNo = 30
    }
}
