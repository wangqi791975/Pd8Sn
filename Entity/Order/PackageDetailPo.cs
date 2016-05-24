
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单包裹明细表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-30 11:07:39
    /// </summary>
    [Class(Table = "t_package_detail", Lazy = false, NameType = typeof(PackageDetailPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PackageDetailPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "PackageDetailId", Column = "package_detail_id")]
        [Generator(2, Class = "native")]
        public virtual int PackageDetailId
        {
            get;
            set;
        }
        /// <summary>
        /// 包裹id
        /// </summary>
        [Property(Column = "package_id")]
        public virtual int PackageId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 购买数量
        /// </summary>
        [Property(Column = "product_qty")]
        public virtual int ProductQty
        {
            get;
            set;
        }
        /// <summary>
        /// 已发货数量
        /// </summary>
        [Property(Column = "total_shipped")]
        public virtual int TotalShipped
        {
            get;
            set;
        }
        /// <summary>
        /// 发货数量
        /// </summary>
        [Property(Column = "shipped_qty")]
        public virtual int ShippedQty
        {
            get;
            set;
        }
    }
}

