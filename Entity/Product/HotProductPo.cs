
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：热销产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 17:39:08
    /// </summary>
    [Class(Table = "t_hot_product", Lazy = false, NameType = typeof(HotProductPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class HotProductPo
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
        /// 一级类别id
        /// </summary>
        [Property(Column = "category_1st")]
        public virtual int? Category1St
        {
            get;
            set;
        }
        /// <summary>
        /// 二级类别id
        /// </summary>
        [Property(Column = "category_2nd")]
        public virtual int? Category2Nd
        {
            get;
            set;
        }
        /// <summary>
        /// 三级类别id
        /// </summary>
        [Property(Column = "category_3rd")]
        public virtual int? Category3Rd
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 销售量
        /// </summary>
        [Property(Column = "sold_quantity")]
        public virtual int? SoldQuantity
        {
            get;
            set;
        }
    }
}

