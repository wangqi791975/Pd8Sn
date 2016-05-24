using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 订单明细实体
    /// </summary>
    [Serializable]
    public class OrderDetail
    {
        /// <summary>
        /// 主键：自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductNo { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public virtual string MainImage { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// 购买价格:折扣价
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 折扣类型：(0:一口价,10:VIP,20:促销)
        /// </summary>
        public virtual int DiscountType { get; set; }

        /// <summary>
        /// 折扣值(0.9)
        /// </summary>
        public virtual decimal DiscountValue { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public virtual decimal OriginalPrice { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 体积重
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }

        /// <summary>
        /// 状态Value
        /// </summary>
        public virtual int StatusValue { get; set; }

        /// <summary>
        /// 已完成数量（发货量） 
        /// </summary>
        public virtual int DeliveryQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否预定
        /// </summary>
        public virtual bool IsReservation { get; set; }

        /// <summary>
        /// 预计到货数量
        /// </summary>
        public virtual int ForecastReachQty { get; set; }

        /// <summary>
        /// 预计到货日期
        /// </summary>
        public virtual DateTime? ForecastReachTime { get; set; }

        /// <summary>
        /// 是否已评论
        /// </summary>
        public virtual bool IsReviewed { get; set; }
    }
}
