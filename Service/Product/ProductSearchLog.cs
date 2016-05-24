using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    [Serializable]
    public class ProductSearchLog
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords
        {
            get;
            set;
        }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip
        {
            set;
            get;
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int? CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 返回记录数
        /// </summary>
        public int? ReturnCount
        {
            get;
            set;
        }

    }
}
