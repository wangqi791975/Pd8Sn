
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.ShoppingCart
{
    /// <summary>
    ///描述：客户未登陆 自动生成cookieId 购物车专用 ORM 映射类 
    /// </summary>
    [Class(Table = "t_auto_cookie_id", Lazy = false, NameType = typeof(AutoCookieIdPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AutoCookieIdPo
    {
        /// <summary>
        /// 自增ID 为负数
        /// </summary>

        [Id(1, Name = "AutoCookieId", Column = "cookie_id")]
        [Generator(2, Class = "native")]
        public virtual int AutoCookieId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Property(Column = "add_time")]
        public virtual DateTime AddTime
        {
            get;
            set;
        }
    }
}

