
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：IP黑白名单 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:11
    /// </summary>
    [Class(Table = "t_ip_white_black_list", Lazy = false, NameType = typeof(IpWhiteBlackListPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class IpWhiteBlackListPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "IpId", Column = "ip_id")]
        [Generator(2, Class = "native")]
        public virtual int IpId
        {
            get;
            set;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        [Property(Column = "ip_address")]
        public virtual string IpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 名单类型(0:白名单,1:黑名单)
        /// </summary>
        [Property(Column = "`type`")]
        public virtual bool Type
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
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime? DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 创建管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

