
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:03
    /// </summary>
    [Class(Table = "t_product", Lazy = false, NameType = typeof(ProductPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "ProductId", Column = "product_id")]
        [Generator(2, Class = "native")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 成本价（rmb）
        /// </summary>
        [Property(Column = "product_price_rmb")]
        public virtual decimal ProductPriceRmb
        {
            get;
            set;
        }
        /// <summary>
        /// 成本价计算列
        /// </summary>
        [Property(Column = "product_price", Update = false, Insert = false)]
        public virtual decimal ProductPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 上调20%存0.2
        /// </summary>
        [Property(Column = "increase_proportion")]
        public virtual decimal? IncreaseProportion
        {
            get;
            set;
        }
        /// <summary>
        /// 最终成本价计算列
        /// </summary>
        [Property(Column = "product_price_final", Update = false, Insert = false)]
        public virtual decimal? ProductPriceFinal
        {
            get;
            set;
        }
        /// <summary>
        /// 重量
        /// </summary>
        [Property(Column = "product_weight")]
        public virtual decimal ProductWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 体积重量
        /// </summary>
        [Property(Column = "product_volume_weight")]
        public virtual decimal ProductVolumeWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime? DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 0下货，1正常销售，2预定，3删除
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 1是，0否
        /// </summary>
        [Property(Column = "is_mixed")]
        public virtual bool IsMixed
        {
            get;
            set;
        }
        /// <summary>
        /// 最小起订量
        /// </summary>
        [Property(Column = "min_quantity")]
        public virtual int? MinQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// 单位id
        /// </summary>
        [Property(Column = "unit_id")]
        public virtual int? UnitId
        {
            get;
            set;
        }
        /// <summary>
        /// 每组数量
        /// </summary>
        [Property(Column = "pack_quantity")]
        public virtual int? PackQuantity
        {
            get;
            set;
        }
        /// <summary>
        ///是否次品 1是，0否
        /// </summary>
        [Property(Column = "is_defective")]
        public virtual bool IsDefective
        {
            get;
            set;
        }
        /// <summary>
        /// 是否存在大小包装 1是，0否
        /// </summary>
        [Property(Column = "is_other_pack")]
        public virtual bool IsOtherPack
        {
            get;
            set;
        }
        /// <summary>
        /// 是否存在评论，1是，0否
        /// </summary>
        [Property(Column = "has_review")]
        public virtual bool HasReview
        {
            get;
            set;
        }
        /// <summary>
        /// 是否一期club目标商品，1是，0否
        /// </summary>
        [Property(Column = "is_club")]
        public virtual bool IsClub
        {
            get;
            set;
        }
        /// <summary>
        /// 是否一期ru club目标商品，1是，0否
        /// </summary>
        [Property(Column = "is_ruclub")]
        public virtual bool IsRuClub
        {
            get;
            set;
        }
    }
}

