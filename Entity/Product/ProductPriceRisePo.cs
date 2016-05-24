
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：t_oem_sourcing ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:10:22
    /// </summary>
    [Class(Table = "t_product_price_rise", Lazy = false, NameType = typeof(ProductPriceRisePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductPriceRisePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 上升比例
        /// </summary>
        [Property(Column = "rise_value")]
        public virtual decimal RiseValue
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

