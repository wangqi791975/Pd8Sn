
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Coupon
{
    /// <summary>
    ///描述：coupon名称表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-28 14:29:27
    /// </summary>
    [Class(Table = "t_coupon_desc", Lazy = false, NameType = typeof(CouponDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CouponDescPo
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
        /// coupon id
        /// </summary>
        [Property(Column = "coupon_id")]
        public virtual int CouponId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言别id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        [Property(Column = "description")]
        public virtual string Description
        {
            get;
            set;
        }
    }
}

