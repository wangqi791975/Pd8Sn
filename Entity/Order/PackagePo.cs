using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单包裹表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-30 11:07:38
    /// </summary>
    [Class(Table = "t_package", Lazy = false, NameType = typeof(PackagePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PackagePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "PackageId", Column = "package_id")]
        [Generator(2, Class = "native")]
        public virtual int PackageId
        {
            get;
            set;
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 配送方式ID
        /// </summary>
        [Property(Column = "shipping_id")]
        public virtual int ShippingId
        {
            get;
            set;
        }
        /// <summary>
        /// 跟踪号
        /// </summary>
        [Property(Column = "trace_number")]
        public virtual string TraceNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 发货时间
        /// </summary>
        [Property(Column = "date_shippinged")]
        public virtual DateTime DateShippinged
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已收货(0:未收货,10:已收货)
        /// </summary>
        [Property(Column = "is_received")]
        public virtual bool IsReceived
        {
            get;
            set;
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Property(Column = "order_number")]
        public virtual string OrderNumber
        {
            get;
            set;
        }
        /// <summary>
        /// erp包裹号
        /// </summary>
        [Property(Column = "erp_package_number")]
        public virtual string ErpPackageNumber
        {
            get;
            set;
        }
    }
}

