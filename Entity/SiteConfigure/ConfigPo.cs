
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：站点配置表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:04
    /// </summary>
    [Class(Table = "t_config", Lazy = false, NameType = typeof(ConfigPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ConfigPo
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
        /// key
        /// </summary>
        [Property(Column = "`key`")]
        public virtual string Key
        {
            get;
            set;
        }
        /// <summary>
        /// value
        /// </summary>
        [Property(Column = "value")]
        public virtual string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "comment")]
        public virtual string Comment
        {
            get;
            set;
        }
    }
}

