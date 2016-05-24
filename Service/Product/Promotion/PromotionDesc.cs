using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Promotion
{
    /// <summary>
    /// 促销区多语种
    /// </summary>
    public class PromotionDesc
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { set; get; }

        /// <summary>
        /// 促销专区Id
        /// </summary>
        public virtual int PromotionAreaId { set; get; }
        
        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { set; get; }

        /// <summary>
        /// 促销专区名称
        /// </summary>
        public virtual string PromotionName { set; get; }

        /// <summary>
        /// 促销专区首页HTML
        /// </summary>
        public virtual string PromotionHome { set; get; }

    }
}
