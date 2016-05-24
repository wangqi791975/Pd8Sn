using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T">返回的查询数据类型</typeparam>
    [Serializable]
    public class PageData<T>
    {
        /// <summary>
        /// 查询分页数据
        /// </summary>
        public virtual IList<T> Data { get; set; }

        /// <summary>
        /// 查询分页信息
        /// </summary>
        public virtual Pager Pager { get; set; }
    }
}
