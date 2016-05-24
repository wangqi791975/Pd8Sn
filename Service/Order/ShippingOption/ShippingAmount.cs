using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 基本运费信息
    /// </summary>
    [Serializable]
    public class ShippingAmount
    {
        #region 基本信息
        /// <summary>
        /// 配送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 配送方式编码
        /// </summary>
        public virtual string ShippingCode { get; set; }

        /// <summary>
        /// 配送方式名称(当前语种)
        /// </summary>
        public virtual string ShippingName { get; set; }

        /// <summary>
        /// 最快到达开数
        /// </summary>
        public virtual int DayLow { get; set; }

        /// <summary>
        /// 最慢到达天数
        /// </summary>
        public virtual int DayHigh { get; set; }

        /// <summary>
        /// 包裹号跟踪查询地址
        /// </summary>
        public virtual string TrackUrl { get; set; }
        #endregion

        #region 运费计算
        /// <summary>
        /// 是否计算偏远费
        /// </summary>
        public virtual bool IsCalculateRemote { get; set; }

        /// <summary>
        /// 是否计算体积重
        /// </summary>
        public virtual bool IsCalculateVolume { get; set; }

        /// <summary>
        /// 包裹数量
        /// </summary>
        public virtual int ShippingBoxNumber { get; set; }

        /// <summary>
        /// 基础运费
        /// </summary>
        public virtual decimal ShippingCost { get; set; }

        /// <summary>
        /// 偏远费
        /// </summary>
        public virtual decimal RemoteAmount { get; set; }

        /// <summary>
        /// 总运费=基础运费 + 偏远费(可能被划掉的运费)
        /// </summary>
        public virtual decimal TotalShippingCost {
            get { return ShippingCost + RemoteAmount; }
        }

        /// <summary>
        /// CLUB手续费(Handling Fee)
        /// </summary>
        public virtual decimal HandlingFeeForClub { get; set; }

        /// <summary>
        /// CLUB运费差额
        /// </summary>
        public virtual decimal ClubShippingBalance { get; set; }

        #endregion

        #region 运费活动
        /// <summary>
        /// 活动类型
        /// </summary>
        public virtual ActivityType ActivityType { get; set; }

        #region 免运费
        /// <summary>
        /// 基准运输方式ID
        /// </summary>
        public virtual int FreeBaseShippingId { get; set; }

        /// <summary>
        /// 免运费活动时收取的手续费
        /// </summary>
        public virtual decimal HandlingFeeForFreeShipping { get; set; }
        
        /// <summary>
        /// 免运费活动时差额
        /// </summary>
        public virtual decimal FreeShippingBalance { get; set; }
        #endregion

        #region 折扣
        /// <summary>
        /// 运费折扣(如20%存0.8)
        /// </summary>
        public virtual decimal ShippingCostDiscount { get; set; }

        /// <summary>
        /// 运费折扣金额
        /// </summary>
        public virtual decimal ShippingCostDiscountAmount { get; set; }
        #endregion

        #region 升级
        /// <summary>
        /// 从哪个配送方式升级而来
        /// </summary>
        public virtual int? UpgradeFromShippingId { get; set; }

        /// <summary>
        /// 升级前的运费
        /// </summary>
        public virtual decimal? UpgradeFromShippingAmount { get; set; }
        #endregion

        #endregion

        /// <summary>
        /// 最终显示运费
        /// </summary>
        public virtual decimal FinalShippingAmount { get; set; }

        /// <summary>
        /// 是否默认选中
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// 冗余 运费计算页面使用
        /// </summary>
        public virtual string Service { get; set;}

    }

    public enum ActivityType
    {
        Default,
        FreeShipping,
        Discount,
        Upgrade
    }

    public enum ShoppingAmountSorterCriteria
    {
        /// <summary>
        /// 时间从新到旧
        /// </summary>
        ShippingTimeNewToOld = 0,
        /// <summary>
        /// 添加时间从旧到新
        /// </summary>
        ShippingTimeOldToNew,
        /// <summary>
        /// 运费从低到高
        /// </summary>
        ShippingCostLowToHigh,
        /// <summary>
        /// 运费从高到低
        /// </summary>
        ShippingCostHighToLow
    }
}
