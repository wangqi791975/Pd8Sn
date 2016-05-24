using System;

namespace Com.Panduo.Service.Product.ClubProduct
{
    /// <summary>
    /// Club产品
    /// </summary>
    [Serializable]
    public class ClubProduct
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 折扣（20%存0.8）
        /// </summary>
        public virtual decimal Discount { get; set; }

        /// <summary>
        /// 类型（New,Hot）
        /// </summary>
        public virtual ClubProductType Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }

    public enum ClubProductType
    {
        /// <summary>
        /// 新品
        /// </summary>
        New = 10,
        /// <summary>
        /// 热卖
        /// </summary>
        Hot = 20
    }
}
