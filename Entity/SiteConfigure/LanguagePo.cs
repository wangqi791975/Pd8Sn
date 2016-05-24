
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：语种表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:12
    /// </summary>
    [Class(Table = "t_language", Lazy = false, NameType = typeof(LanguagePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class LanguagePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "LanguageId", Column = "languages_id")]
        [Generator(2, Class = "native")]

        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
        /// <summary>
        /// 如yyyy-MM-dd  hh:mm:ss
        /// </summary>
        [Property(Column = "date_format_long")]
        public virtual string DateFormatLong
        {
            get;
            set;
        }
        /// <summary>
        /// 如yyyy-MM-dd
        /// </summary>
        [Property(Column = "date_format_short")]
        public virtual string DateFormatShort
        {
            get;
            set;
        }
        /// <summary>
        /// 编码
        /// </summary>
        [Property(Column = "code")]
        public virtual string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 订单号前缀(如8seasons网站俄语站点为SR、德语站点为SD)
        /// </summary>
        [Property(Column = "order_prefix")]
        public virtual string OrderPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:废弃,1:显示)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// HOST
        /// </summary>
        [Property(Column = "host")]
        public virtual string Host
        {
            get;
            set;
        }
        /// <summary>
        /// 语种对应客户经理ID(t_customer_manager)
        /// </summary>
        [Property(Column = "customer_manager_id")]
        public virtual int? CustomerManagerId
        {
            get;
            set;
        }
    }
}

