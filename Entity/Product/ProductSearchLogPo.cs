
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品搜索日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 09:32:12
    /// </summary>
    [Class(Table = "t_product_search_log", Lazy = false, NameType = typeof(ProductSearchLogPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductSearchLogPo
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
        /// 关键字
        /// </summary>
        [Property(Column = "keyword")]
        public virtual string Keyword
        {
            get;
            set;
        }
        /// <summary>
        /// ip
        /// </summary>
        [Property(Column = "ip")]
        public virtual string Ip
        {
            get;
            set;
        }
        /// <summary>
        /// 客户id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int? CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 搜索时间
        /// </summary>
        [Property(Column = "date_searched")]
        public virtual DateTime? DateSearched
        {
            get;
            set;
        }
        /// <summary>
        /// 返回记录数
        /// </summary>
        [Property(Column = "return_count")]
        public virtual int? ReturnCount
        {
            get;
            set;
        }
    }
}

