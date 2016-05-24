
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品图片表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:08
    /// </summary>
    [Class(Table = "t_product_image", Lazy = false, NameType = typeof(ProductImagePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductImagePo
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
        /// 顺序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        [Property(Column = "image")]
        public virtual string Image
        {
            get;
            set;
        }
        /// <summary>
        /// 1是，0否
        /// </summary>
        [Property(Column = "is_main")]
        public virtual bool IsMain
        {
            get;
            set;
        }
    }
}

