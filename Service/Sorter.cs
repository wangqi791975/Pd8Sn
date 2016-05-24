using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service
{
    [Serializable]
    public class Sorter<T>
    {
        /// <summary>
        /// 排序的字段
        /// </summary>
        public virtual T Key { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public virtual bool IsAsc { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public Sorter()
        {

        }

        /// <summary>
        /// 参构造函数
        /// </summary>
        /// <param name="key">排序的字段</param>
        /// <param name="isAsc">是否升序</param>
        public Sorter(T key, bool isAsc)
        {
            Key = key;
            IsAsc = isAsc;
        }
    }
}
