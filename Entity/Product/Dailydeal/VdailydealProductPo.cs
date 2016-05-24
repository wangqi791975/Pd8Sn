using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Dailydeal
{
    /// <summary>
    ///描述：dailydeal 查询视图 ORM 映射实体
    ///创建时间：12/25/2014
    /// </summary>
    [Class(Table = "V_DailydealProduct", Lazy = false, NameType = typeof(VdailydealProductPo), DynamicUpdate = false)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class VdailydealProductPo
    {
        #region dailydeal表
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        [Property(Column = "image")]
        public virtual string Image { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Property(Column = "date_started")]
        public virtual DateTime DateStarted { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Property(Column = "date_ended")]
        public virtual DateTime DateEnded { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Property(Column = "price")]
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 已售数量
        /// </summary>
        [Property(Column = "sold_quantity")]
        public virtual int? SoldQuantity { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        [Property(Column = "date_update")]
        public virtual DateTime? DateUpdate { get; set; }

        /// <summary>
        /// 标语Id
        /// </summary>
        [Property(Column = "title_id")]
        public virtual int TitleId { get; set; }

        /// <summary>
        /// 标语
        /// </summary>
        [Property(Column = "title")]
        public virtual string Title { get; set; }
        #endregion

        #region 产品表
        /// <summary>
        /// 产品编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductModel { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Property(Column = "product_name")]
        public virtual string ProductName { get; set; }
        /// <summary>
        /// 产品英文名称
        /// </summary>
        [Property(Column = "product_enname")]
        public virtual string ProductEnName { get; set; }
        #endregion

        /// <summary>
        /// 语言Id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual string LanguageId { get; set; }

        /// <summary>
        /// 一口价梯度价格（一口价原价）
        /// </summary>
        [Property(Column = "daily_product_price")]
        public virtual decimal DailyProductPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        [Property(Column = "discount")]
        public virtual decimal Discount { get; set; }

    }
}

