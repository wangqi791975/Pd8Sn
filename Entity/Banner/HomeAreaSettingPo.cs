
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Banner
{
    /// <summary>
    ///描述：t_home_area_setting ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:10:22
    /// </summary>
    [Class(Table = "t_home_area_setting", Lazy = false, NameType = typeof(HomeAreaSettingPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class HomeAreaSettingPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "AreaId", Column = "area_id")]
        [Generator(2, Class = "native")]
        public virtual int AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 区域类型(1:首页横导航，2:类别展示右侧，3:类别展示下方)
        /// </summary>
        [Property(Column = "area_type")]
        public virtual int AreaType
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
        /// 内容
        /// </summary>
        [Property(Column = "description",Length = 500000)]
        public virtual string Description
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
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

