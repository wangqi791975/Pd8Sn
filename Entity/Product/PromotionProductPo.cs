
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：促销产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:21
    /// </summary>
    [Class(Table = "t_promotion_product", Lazy = false, NameType = typeof(PromotionProductPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PromotionProductPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]

        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 促销id
        /// </summary>
        [Property(Column = "promotion_id")]
        public virtual int PromotionId
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
        /// 有效1，无效0
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        [Property(Column = "discount")]
        public virtual decimal Discount
        {
            get;
            set;
        }
        /// <summary>
        /// 1绑定，0不绑定 二期需求确认，不需要该字段，用产品库存表字段判断
        /// </summary>
        //[Property(Column = "limit_stock")]
        //public virtual bool LimitStock
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 开始时间
        /// </summary>
        [Property(Column = "date_started")]
        public virtual DateTime DateStarted
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Property(Column = "date_ended")]
        public virtual DateTime DateEnded
        {
            get;
            set;
        }
    }
}

