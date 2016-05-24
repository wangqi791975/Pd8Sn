using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Dailydeal
{
    /// <summary>
    ///描述：dailydeal标签表 ORM 映射类 
    /// </summary>
    [Class(Table = "t_dailydeal_label", Lazy = false, NameType = typeof(DailyDealLabelPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class DailyDealLabelPo
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Id(Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 一口价Id
        /// </summary>
        [Property(Column = "dailydeal_id")]
        public virtual int DailyDealId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言Id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 标签名称
        /// </summary>
        [Property(Column = "label_name")]
        public virtual string LabelName
        {
            get;
            set;
        }
    }
}