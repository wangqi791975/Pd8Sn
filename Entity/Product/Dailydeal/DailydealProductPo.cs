using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Dailydeal
{
    /// <summary>
    ///描述：dailydeal产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:58
    /// </summary>
    [Class(Table = "t_dailydeal_product", Lazy = false, NameType = typeof(DailydealProductPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class DailydealProductPo
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
        /// 产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品图片
        /// </summary>
        [Property(Column = "image")]
        public virtual string Image
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
        /// 状态
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 价格
        /// </summary>
        [Property(Column = "price")]
        public virtual decimal Price
        {
            get;
            set;
        }
        /// <summary>
        /// 已售数量
        /// </summary>
        [Property(Column = "sold_quantity")]
        public virtual int? SoldQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// 刷新时间
        /// </summary>
        [Property(Column = "date_update")]
        public virtual DateTime? DateUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// 标语Id
        /// </summary>
        [Property(Column = "title_id")]
        public virtual int TitleId
        {
            get;
            set;
        }
    }
}

