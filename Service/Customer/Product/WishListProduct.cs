using System;

namespace Com.Panduo.Service.Customer.Product
{
    /// <summary>
    /// 客户心愿单产品
    /// </summary>
    [Serializable]
    public class WishListProduct
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Count { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime AddDateTime { get; set; }

        /// <summary>
        /// 喜爱类型Id
        /// </summary>
        public virtual WishListType WishListType { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? DateModified { get; set; }

        /// <summary>
        /// 是否历史心愿单
        /// </summary>
        public virtual bool IsHistory 
        {
             get; set; 
        }
    }

    /// <summary>
    /// 喜爱类型
    /// </summary>
    public enum WishListType
    {
        /// <summary>
        /// 我喜欢的(items I like)
        /// </summary>
        Like=1,
        /// <summary>
        /// 每单都买（items I buy in each order）
        /// </summary>
        EveryTime=2,
        /// <summary>
        /// 频繁购买（items I buy frequently）
        /// </summary>
        Frequently=3,
        /// <summary>
        /// 未经分类的
        /// </summary>
        Unclassified=4
    }
}
