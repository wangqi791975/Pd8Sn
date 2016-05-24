
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：促销多语种表 ORM 映射类 
    ///创建人:lxf
    ///创建时间：03/15/2015 16:40:19
    /// </summary>
    [Class(Table = "t_promotion_desc", Lazy = false, NameType = typeof(PromotionDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PromotionDescPo
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

        [Property(Column = "promotion_id")]
        public virtual int PromotionId
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
        /// 多语种名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 多语种首页
        /// </summary>
        [Property(Column = "home")]
        public virtual string Home
        {
            get;
            set;
        }
    }
}

