using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：单位表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-08 15:45:33
    /// </summary>
    [Class(Table = "t_unit", Lazy = false, NameType = typeof(UnitPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class UnitPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "UnitId", Column = "unit_id")]
        [Generator(2, Class = "native")]
        public virtual int UnitId
        {
            get;
            set;
        }
        /// <summary>
        /// code
        /// </summary>
        [Property(Column = "code")]
        public virtual string Code
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

