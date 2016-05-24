using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式
    /// </summary>
    [Serializable]
    public class Shipping
    {
        /// <summary>
        /// ShippingId
        /// </summary>
        public virtual int ShippingId { get; set; }
        /// <summary>
        /// ShippingCode
        /// </summary>
        public virtual string ShippingCode { get; set; }
        /// <summary>
        /// 运送方式名称
        /// </summary>
        public virtual string ShippingName { get; set; }
        /// <summary>
        /// 语种
        /// </summary>
        public virtual string Language { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int SortOrder { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual bool ShippingStatus { get; set; }
        /// <summary>
        /// 燃油附加费
        /// </summary>
        public virtual decimal ExtraOil { get; set; }
        /// <summary>
        /// 附加费
        /// </summary>
        public virtual decimal ExtraAmt { get; set; }
        /// <summary>
        /// 额外倍数
        /// </summary>
        public virtual decimal ExtraTimes { get; set; }
        /// <summary>
        /// 运费折扣物流商给的折扣
        /// </summary>
        public virtual decimal ShippingDiscount { get; set; }
        /// <summary>
        /// 是否计算体积
        /// </summary>
        public virtual bool CalVolume { get; set; }
        /// <summary>
        /// 是否计算偏远费
        /// </summary>
        public virtual bool CalRemote { get; set; }
        /// <summary>
        /// 是否进行分包
        /// </summary>
        public virtual bool SplitType { get; set; }
        /// <summary>
        /// 最小产品重量(0不进行判断)
        /// </summary>
        public virtual decimal MinWeightKg { get; set; }
        /// <summary>
        /// 最大产品重量(0不进行判断)
        /// </summary>
        public virtual decimal MaxWeightKg { get; set; }
        /// <summary>
        /// 查询订单状态网站URL
        /// </summary>
        public virtual string TrackUrl { get; set; }
        /// <summary>
        /// 分包类型(false:老的分包规则,true:新的分包规则)
        /// </summary>
        public virtual string PackageType { get; set; }
        /// <summary>
        /// 新的分包规则每包重量
        /// </summary>
        public virtual decimal PackageWeightKg { get; set; }
        /// <summary>
        /// 快递/平邮(1快递,0默认平邮)
        /// </summary>
        public virtual bool ExpressDelivery { get; set; }
    }
}
