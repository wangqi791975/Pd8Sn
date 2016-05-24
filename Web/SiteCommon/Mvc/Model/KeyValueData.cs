using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Web.Common.Mvc.Model
{
    /// <summary>
    /// Key Value数据
    /// </summary>
    [Serializable]
    public class KeyValueData
    {
        /// <summary>
        /// 键
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public virtual string Value { get; set; }
    }
}
