
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：随机数forGC表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-03-04 16:04:56
    /// </summary>
    [Class(Table = "t_rand_gc", Lazy = false, NameType = typeof(RandGcPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class RandGcPo
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
        /// 随机数
        /// </summary>
        [Property(Column = "rand_value")]
        public virtual string RandValue
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0未使用,1已使用)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 使用时间
        /// </summary>
        [Property(Column = "date_used")]
        public virtual DateTime? DateUsed
        {
            get;
            set;
        }
    }
}

