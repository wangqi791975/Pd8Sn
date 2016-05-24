using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：单位名称表 ORM 映射类 ,遗漏
    ///创建人:罗海明
    ///创建时间：2015-01-08 15:41:49
    /// </summary>
    [Class(Table = "t_unit_desc", Lazy = false, NameType = typeof(UnitDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class UnitDescPo
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
        /// 单位id
        /// </summary>
        [Property(Column = "unit_id")]
        public virtual int UnitId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言别
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name
        {
            get;
            set;
        }
    }
}
