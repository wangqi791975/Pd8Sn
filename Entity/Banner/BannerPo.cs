
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Banner
{
    /// <summary>
    ///描述：t_banner ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:27:44
    /// </summary>
    [Class(Table = "t_banner", Lazy = false, NameType = typeof(BannerPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class BannerPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 语种ID
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 横幅代码
        /// </summary>
        [Property(Column = "ad_word")]
        public virtual string AdWord
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Property(Column = "start_time")]
        public virtual DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Property(Column = "end_time")]
        public virtual DateTime EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否倒计时
        /// </summary>
        [Property(Column = "is_countdown")]
        public virtual bool IsCountdown
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        [Property(Column = "show_index")]
        public virtual bool ShowIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        [Property(Column = "status")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }
}

