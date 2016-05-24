using System;
using Com.Panduo.Entity.Order;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Dailydeal
{
    /// <summary>
    ///描述：Dailydeal标语从表 ORM 映射类 
    /// </summary>
    [Class(Table = "t_dailydeal_title", Lazy = false, NameType = typeof(DailydealTitlePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class DailydealTitlePo
    {
        [CompositeId(1, Name = "Id", ClassType = typeof(DailydealTitlePk))]
        [KeyProperty(2, Name = "TitleId", Column = "title_id")]
        [KeyProperty(3, Name = "LanguageId", Column = "language_id")]
        public virtual DailydealTitlePk Id
        {
            get; 
            set;
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
    }
}

