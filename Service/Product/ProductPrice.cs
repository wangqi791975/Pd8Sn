//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductPrice.cs
//创 建 人：罗海明
//创建时间：2014/12/20 14:49:50 
//功能说明：产品显示价Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品显示价
    /// </summary>
    [Serializable]
    public class ProductPrice
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public virtual decimal CostPrice { get; set; }

        /// <summary>
        /// 促销折扣（例如促销20%存0.8）
        /// </summary>
        public virtual decimal PromotionalDiscount { get; set; }

        /// <summary>
        /// Club折扣（例如20%存0.8）
        /// </summary>
        public virtual decimal ClubDiscount { get; set; }
        
        /// <summary>
        /// 是否一口价
        /// </summary>
        public virtual bool IsNoHaggle { get; set; }

        /// <summary>
        /// 一口价
        /// </summary>
        public virtual decimal NoHaggle { get; set; }

        /// <summary>
        /// 产品梯度价
        /// </summary>
        public virtual IList<ProductStepPrice> StepPrice { get; set; }
    }
}
