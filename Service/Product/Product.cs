//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：Product.cs
//创 建 人：罗海明
//创建时间：2014/12/18 14:40:40 
//功能说明：产品Vo以及产品状态
//-----------------------------------------------------------------
//修改记录： 2014/12/24
//修改人：   罗海明
//修改时间： 2014/12/24 15:40:40 
//修改内容： 增加最终成本价属性
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品其他包装类型
    /// </summary>
    public enum ProductOtherPackType
    {
        /// <summary>
        /// 正常
        /// </summary>
        None,

        /// <summary>
        /// 大包装
        /// </summary>
        Big,

        /// <summary>
        /// 小包装
        /// </summary>
        Small,
    }

    /// <summary>
    /// 产品状态
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>
        /// 0下货
        /// </summary>
        OffLine = 0,
        /// <summary>
        /// 1正常销售
        /// </summary>
        OnSale = 1,
        /// <summary>
        /// 2预定
        /// </summary>
        BackOrder = 2,
        /// <summary>
        /// 3删除
        /// </summary>
        Delete = 3,
    }

    /// <summary>
    /// 产品信息
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 重量(g)
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 体积重(g)
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }

        /// <summary>
        /// 产品主图
        /// </summary>
        public virtual string MainImage { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 创建时间（首次上架时间）
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual ProductStatus Status { get; set; }

        /// <summary>
        /// 成本价（rmb）
        /// </summary>
        public virtual decimal CostPriceRmb { get; set; }

        /// <summary>
        /// 成本价（美元）=成本价（rmb）*固定比例
        /// </summary>
        public virtual decimal? CostPrice { get; set; }

        /// <summary>
        /// 最终成本价=成本价（美元）* (1+上调比例)
        /// </summary>
        public virtual decimal? CostPriceFinal { get; set; }

        /// <summary>
        /// 上调20%存0.2
        /// </summary>
        public virtual decimal? IncreaseProportion { get; set; }

        /// <summary>
        /// 是否混装产品
        /// </summary>
        public virtual bool IsMixed { get; set; }

        /// <summary>
        /// 是否热销产品
        /// </summary>
        public virtual bool IsHot { get; set; }

        /// <summary>
        /// 产品单位Id
        /// </summary>
        public virtual int UnitId { get; set; }

        /// <summary>
        /// 每组数量
        /// </summary>
        public virtual int? GroupQuantity { get; set; }

        /// <summary>
        /// 最小起订量
        /// </summary>
        public virtual int MinOrderQuantity { get; set; }

        /// <summary>
        /// 是否包含其他包装方式
        /// </summary>
        public virtual bool IsOtherPack { get; set; }

        /// <summary>
        /// 是否存在评论
        /// </summary>
        public virtual bool HasReview { get; set; }

        /// <summary>
        /// 是否一期club目标商品，1是，0否
        /// </summary>
        public virtual bool IsClub { get; set; }

        /// <summary>
        /// 是否一期ru club目标商品，1是，0否
        /// </summary>
        public virtual bool IsRuClub { get; set; }

        #region 扩展

        /// <summary>
        /// 大小包装类型
        /// </summary>
        public virtual ProductOtherPackType OtherPackType
        {
            get { return ProductCode.EndsWith("H") ? ProductOtherPackType.Big : ProductCode.EndsWith("S") ? ProductOtherPackType.Small : ProductOtherPackType.None; }
        }
        #endregion
    }

}