
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Shipping
{
    /// <summary>
    ///描述：配送方式表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 14:06:53
    /// </summary>
    [Class(Table = "t_shipping", Lazy = false, NameType = typeof(ShippingPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ShippingPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "ShippingId", Column = "shipping_id")]
        [Generator(2, Class = "native")]
        public virtual int ShippingId
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:废弃,1:启用)
        /// </summary>
        [Property(Column = "shipping_status")]
        public virtual bool ShippingStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 配送方式中文名称
        /// </summary>
        [Property(Column = "shipping_name")]
        public virtual string ShippingName
        {
            get;
            set;
        }
        /// <summary>
        /// 配送方式编码
        /// </summary>
        [Property(Column = "shipping_code")]
        public virtual string ShippingCode
        {
            get;
            set;
        }
        /// <summary>
        /// 燃油附加费
        /// </summary>
        [Property(Column = "extra_oil")]
        public virtual decimal ExtraOil
        {
            get;
            set;
        }
        /// <summary>
        /// 附加费
        /// </summary>
        [Property(Column = "extra_amt")]
        public virtual decimal ExtraAmt
        {
            get;
            set;
        }
        /// <summary>
        /// 额外倍数
        /// </summary>
        [Property(Column = "extra_times")]
        public virtual decimal ExtraTimes
        {
            get;
            set;
        }
        /// <summary>
        /// 运费折扣
        /// </summary>
        [Property(Column = "shipping_discount")]
        public virtual decimal ShippingDiscount
        {
            get;
            set;
        }
        /// <summary>
        /// 是否计算体积(0:不计算,1:计算)
        /// </summary>
        [Property(Column = "cal_volume")]
        public virtual bool CalVolume
        {
            get;
            set;
        }
        /// <summary>
        /// 是否计算偏远费(0:不计算,1:计算)
        /// </summary>
        [Property(Column = "cal_remote")]
        public virtual bool CalRemote
        {
            get;
            set;
        }
        /// <summary>
        /// 最小物品重量(0不进行判断)
        /// </summary>
        [Property(Column = "min_weight_kg")]
        public virtual decimal MinWeightKg
        {
            get;
            set;
        }
        /// <summary>
        /// 最大物品重量(0不进行判断)
        /// </summary>
        [Property(Column = "max_weight_kg")]
        public virtual decimal MaxWeightKg
        {
            get;
            set;
        }
        /// <summary>
        /// 是否进行分包(0:不进行,1:进行)
        /// </summary>
        [Property(Column = "split_type")]
        public virtual bool SplitType
        {
            get;
            set;
        }
        /// <summary>
        /// 查询订单状态网站URL
        /// </summary>
        [Property(Column = "track_url")]
        public virtual string TrackUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 分包类型(0:老的分包规则,1:新的分包规则)
        /// </summary>
        [Property(Column = "package_type")]
        public virtual bool PackageType
        {
            get;
            set;
        }
        /// <summary>
        /// 新的分包规则每包重量
        /// </summary>
        [Property(Column = "package_weight_kg")]
        public virtual decimal PackageWeightKg
        {
            get;
            set;
        }
        /// <summary>
        /// 快递/平邮(1快递,0默认平邮)
        /// </summary>
        [Property(Column = "express_delivery")]
        public virtual bool ExpressDelivery
        {
            get;
            set;
        }
    }
}

