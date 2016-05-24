using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户心愿清单类型语种表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 17:43:42
    /// </summary>
    [Class(Table = "t_wishlist_type_desc", Lazy = false, NameType = typeof(WishListTypeDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class WishListTypeDescPo
    {
        [CompositeId(1, Name = "Id", ClassType = typeof(WishListTypeDescPk))]
        [KeyProperty(2, Name = "TypeId", Column = "type_id")]
        [KeyProperty(3, Name = "LanguageId", Column = "language_id")]
        public virtual WishListTypeDescPk Id
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name { get; set; }
    }
}