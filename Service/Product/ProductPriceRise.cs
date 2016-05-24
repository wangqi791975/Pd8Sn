using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品调价上浮比例对象
    /// </summary>
    [Serializable]
    public class ProductPriceRise
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 上升比例
        /// </summary>
        public decimal RiseValue { get; set; }

        /// <summary>
        /// 上升比例
        /// </summary>
        public decimal DisplayRiseValue { get { return RiseValue*100; } }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AdminId { get; set; }
        
    }
}
