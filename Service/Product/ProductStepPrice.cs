//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductStepPrice.cs
//创 建 人：罗海明
//创建时间：2014/12/20 13:49:50 
//功能说明：产品梯度价格Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品梯度价
    /// </summary>
    [Serializable]
    public class ProductStepPrice
    {

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        ///  利润系数
        /// </summary>
        public virtual decimal ProfitCoefficient { get; set; }

        /// <summary>
        /// 原价(折扣前销售价格)
        /// </summary>
        public virtual decimal OriginalPrice {
            get { return CostPrice*ProfitCoefficient; }
        }

        /// <summary>
        /// 成本价(网站默认美元)
        /// </summary>
        public virtual decimal CostPrice { get; set; }

        /// <summary>
        /// 获取折扣价
        /// </summary>
        /// <param name="discount">折扣</param>
        public virtual decimal GetDiscountPrice(decimal discount)
        {
            return  OriginalPrice * discount;
        }
    }
}
