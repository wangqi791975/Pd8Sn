
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品其他包装关联表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:12
    /// </summary>
    [Class(Table = "t_product_otherpack", Lazy = false, NameType = typeof(ProductOtherpackPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductOtherpackPo
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
        /// 主产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 其他包装产品1
        /// </summary>
        [Property(Column = "other_id1")]
        public virtual int? OtherId1
        {
            get;
            set;
        }
        /// <summary>
        /// 其他包装产品2
        /// </summary>
        [Property(Column = "other_id2")]
        public virtual int? OtherId2
        {
            get;
            set;
        }
    }
}

