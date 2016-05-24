using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{

    /// <summary>
    /// 库存状态
    /// </summary>
    public enum StockStatus
    {
        /// <summary>
        /// 0不绑定
        /// </summary>
        NotBind = 0,

        /// <summary>
        /// 1绑定&下货
        /// </summary>
        Bind = 1,
        /// <summary>
        /// 2绑定&预定&不限制购买
        /// </summary>
        BindNotLimit = 2,

        /// <summary>
        /// 3绑定&预定&限制购买
        /// </summary>
        BindLimit = 3
    }

    /// <summary>
    /// 库存
    /// </summary>
    [Serializable]
    public class ProductStock
    {
        /// <summary>
        /// 库存ID
        /// </summary>
        public virtual int StockId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int ProductId
        {
            get;
            set;
        }


        /// <summary>
        /// 库存数
        /// </summary>
        public virtual int StockNumber
        {
            get;
            set;
        }

        /// <summary>
        ///绑定库存类型
        /// </summary>
        public virtual StockStatus BindStockType
        {
            get;
            set;
        }

        /// <summary>
        /// 回货周期
        /// </summary>
        public virtual int? DayReturn
        {
            get;
            set;
        }
        /// <summary>
        /// 回货日期
        /// </summary>
        public virtual DateTime? DateReturn
        {
            get;
            set;
        }

    }

}
