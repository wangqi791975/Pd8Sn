
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：club客户经理表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:44
    /// </summary>
    [Class(Table = "t_customer_manager", Lazy = false, NameType = typeof(CustomerManagerPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerManagerPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "CustomerManagerId", Column = "customer_manager_id")]
        [Generator(2, Class = "native")]

        public virtual int CustomerManagerId
        {
            get;
            set;
        }
        /// <summary>
        /// 名字
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名字
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        [Property(Column = "telphone")]
        public virtual string Telphone
        {
            get;
            set;
        }
        /// <summary>
        /// skype
        /// </summary>
        [Property(Column = "skype")]
        public virtual string Skype
        {
            get;
            set;
        }
        /// <summary>
        /// 头像
        /// </summary>
        [Property(Column = "avatar")]
        public virtual string Avatar
        {
            get;
            set;
        }
        /// <summary>
        /// 服务邮箱
        /// </summary>
        [Property(Column = "service_email")]
        public virtual string ServiceEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [Property(Column = "company_email")]
        public virtual string CompanyEmail
        {
            get;
            set;
        }
    }
}

