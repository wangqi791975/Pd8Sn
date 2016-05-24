using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:42
    /// </summary>
    [Class(Table = "t_order_detail", Lazy = false, NameType = typeof(OrderDetailPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderDetailPo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Id(1, Name = "OrderProductId", Column = "order_product_id")]
        [Generator(2, Class = "native")]
        public virtual int OrderProductId
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
        /// 产品ID
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
        /// 主图地址
        /// </summary>
        [Property(Column = "main_image")]
        public virtual string MainImage
        {
            get;
            set;
        }
        /// <summary>
        /// 购买数量
        /// </summary>
        [Property(Column = "product_quantity")]
        public virtual int ProductQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// 产品原价
        /// </summary>
        [Property(Column = "product_price")]
        public virtual decimal ProductPrice
        {
            get;
            set;
        }
        
        [Property(Column = "discount_value")]
        public virtual decimal DiscountValue
        {
            get;
            set;
        }

        /// <summary>
        /// 折扣类型(0:一口价,10:VIP,20:促销)
        /// </summary>
        [Property(Column = "discount_type")]
        public virtual int DiscountType
        {
            get;
            set;
        }
        ///// <summary>
        ///// 折扣价
        ///// </summary>
        //[Property(Column = "discount_price")]
        //public virtual decimal DiscountPrice
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 重量(重量:g)
        /// </summary>
        [Property(Column = "product_weight")]
        public virtual decimal ProductWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 体积重量(重量:g)
        /// </summary>
        [Property(Column = "product_volume_weight")]
        public virtual decimal ProductVolumeWeight
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
        /// 状态
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 发货量
        /// </summary>
        [Property(Column = "delivery_number")]
        public virtual int DeliveryNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 客户备注
        /// </summary>
        [Property(Column = "customer_remark")]
        public virtual string CustomerRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 预计到货数量
        /// </summary>
        [Property(Column = "forecast_reach_number")]
        public virtual int? ForecastReachNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 预计到货日期
        /// </summary>
        [Property(Column = "forecast_reach_date")]
        public virtual DateTime? ForecastReachDate
        {
            get;
            set;
        }
        /// <summary>
        /// 是否预订(0:否,1:是)
        /// </summary>
        [Property(Column = "is_reservation")]
        public virtual bool IsReservation
        {
            get;
            set;
        }
        /// <summary>
        /// 库存类型(0:不充足,10:充足,20:缺货)
        /// </summary>
        [Property(Column = "stock_type")]
        public virtual int? StockType
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已评论
        /// </summary>
        [Property(Column = "is_reviewed")]
        public virtual bool IsReviewed
        {
            get;
            set;
        }
    }
}

