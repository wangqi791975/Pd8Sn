
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：币种汇率修改日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:34
    /// </summary>
    [Class(Table = "t_currency_log", Lazy = false, NameType = typeof(CurrencyLogPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CurrencyLogPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "LogId", Column = "log_id")]
        [Generator(2, Class = "native")]
        public virtual int LogId
        {
            get;
            set;
        }
        /// <summary>
        /// 币种编码
        /// </summary>
        [Property(Column = "currency_code")]
        public virtual string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 修改前汇率
        /// </summary>
        [Property(Column = "previous_value",Precision = 18, Scale = 8)]
        public virtual decimal PreviousValue
        {
            get;
            set;
        }
        /// <summary>
        /// 修改后汇率
        /// </summary>
        [Property(Column = "`value`", Precision = 18, Scale = 8)]
        public virtual decimal Value
        {
            get;
            set;
        }
        /// <summary>
        /// 修改管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime DateModified
        {
            get;
            set;
        }
    }
}

