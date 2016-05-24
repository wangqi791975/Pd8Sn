using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单快照表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:44
    /// </summary>
    [Class(Table = "t_order_snapshot", Lazy = false, NameType = typeof(OrderSnapshotPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderSnapshotPo
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        [Id(1, Name = "OrderDetailId", Column = "order_detail_id")]
        [Generator(2, Class = "native")]
        public virtual int OrderDetailId
        {
            get;
            set;
        }

        /// <summary>
        /// 订单明细ID
        /// </summary>
        [Property(Column = "order_product_id")]
        public virtual int OrderProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Property(Column = "product_name")]
        public virtual string ProductName
        {
            get;
            set;
        }           

        /// <summary>
        /// 类别ID
        /// </summary>
        [Property(Column = "category_id")]
        public virtual int CategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 类别名称
        /// </summary>
        [Property(Column = "category_name")]
        public virtual string CategoryName
        {
            get;
            set;
        }
        /// <summary>
        /// 图片(为,隔开)
        /// </summary>
        [Property(Column = "images")]
        public virtual string Images
        {
            get;
            set;
        }
        /// <summary>
        /// 键值对(JSON格式存储数据)
        /// </summary>
        [Property(Column = "key_value")]
        public virtual string KeyValue
        {
            get;
            set;
        }
        /// <summary>
        /// 简述
        /// </summary>
        [Property(Column = "description")]
        public virtual string Description
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
    }
}

