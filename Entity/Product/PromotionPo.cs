
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：促销表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:40:19
    /// </summary>
    [Class(Table = "t_promotion", Lazy = false, NameType = typeof(PromotionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PromotionPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "PromotionId", Column = "promotion_id")]
        [Generator(2, Class = "native")]

        public virtual int PromotionId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        //[Property(Column = "discount")]
        //public virtual decimal Discount
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 1有效，0无效
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
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
        /// <summary>
        /// 是否显示首页
        /// </summary>
        [Property(Column = "is_show_home")]
        public virtual bool IsShowHome
        {
            get;
            set;
        }
        /// <summary>
        /// 是否存在产品
        /// </summary>
        [Property(Column = "has_product")]
        public virtual bool HasProduct
        {
            get;
            set;
        }
    }
}

