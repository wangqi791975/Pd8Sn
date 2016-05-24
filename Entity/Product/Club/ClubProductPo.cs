using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Club
{
    /// <summary>
    ///描述：club产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-28 14:29:30
    /// </summary>
    [Class(Table = "t_club_product", Lazy = false, NameType = typeof(ClubProductPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ClubProductPo
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
        /// Club产品类型(10:new,20:hot)
        /// </summary>
        [Property(Column = "club_product_type")]
        public virtual int ClubProductType
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
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 到期时间
        /// </summary>
        [Property(Column = "date_ended")]
        public virtual DateTime? DateEnded
        {
            get;
            set;
        }
    }
}

