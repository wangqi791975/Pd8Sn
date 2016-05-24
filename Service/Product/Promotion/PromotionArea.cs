using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Promotion
{
    /// <summary>
    /// 促销区
    /// </summary>
    public class PromotionArea
    {
        /// <summary>
        /// 促销专区Id
        /// </summary>
        public virtual int PromotionAreaId { set; get; }

        /// <summary>
        /// 促销专区中文名称
        /// </summary>
        public virtual string PromotionName { set; get; }

        /// <summary>
        /// 多语种名称
        /// </summary>
        public virtual  List<PromotionDesc> PromotionDescs { set; get; }

        /// <summary>
        /// 促销开始时间
        /// </summary>
        public virtual DateTime SaleStartTime { get; set; }

        /// <summary>
        /// 促销结束时间
        /// </summary>
        public virtual DateTime SaleEndTime { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual bool IsValid { set; get; }

        /// <summary>
        /// 是否显示首页
        /// </summary>
        public virtual bool IsShowHome { set; get; }

        /// <summary>
        /// 是否存在产品
        /// </summary>
        public virtual bool HasProduct { set; get; }

        /// <summary>
        /// 折扣 二期需求确认 放在产品表
        /// </summary>
        //public virtual decimal Discount { set; get; }


    }
}
