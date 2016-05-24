using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Promotion
{
    /// <summary>
    /// 促销产品
    /// </summary>
    [Serializable]
    public class ProductPromotion
    {

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 促销专区Id
        /// </summary>
        public virtual int PromotionAreaId { set; get; }

        /// <summary>
        /// 折扣（例如促销20%存0.8）
        /// </summary>
        public virtual decimal Discount { get; set; }
        
        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual bool IsDisplay { get; set; }

        /// <summary>
        /// 促销开始时间
        /// </summary>
        public virtual DateTime SaleStartTime { get; set; }

        /// <summary>
        /// 促销结束时间
        /// </summary>
        public virtual DateTime SaleEndTime { get; set; }

        /// <summary>
        /// 是否绑定库存 二期需求确认，不需要该字段，用产品库存表字段判断
        /// </summary>
        //public virtual bool IsBind { get; set; }
    }
}
