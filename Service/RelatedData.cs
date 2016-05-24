using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service
{
    /// <summary>
    /// 关联数据
    /// </summary>
    [Serializable]
    public class RelatedData<T>
    {
        /// <summary>
        /// 父数据
        /// </summary>
        public virtual T Data { get; set; }
        /// <summary>
        /// 子数据数量
        /// </summary>
        public virtual int Qty { get; set; }
        /// <summary>
        /// 子数据列表
        /// </summary>
        public virtual IList<RelatedData<T>> SubDataList { get; set; }
    }
}
