
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:36
    /// </summary>
    [Class(Table = "t_customer", Lazy = false, NameType = typeof(CustomerPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerPo : IDisposable
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "CustomerId", Column = "customer_id")]
        [Generator(2, Class = "native")]

        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [Property(Column = "`password`")]
        public virtual string Password
        {
            get;
            set;
        }
        /// <summary>
        /// first name
        /// </summary>
        [Property(Column = "first_name")]
        public virtual string FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// last name
        /// </summary>
        [Property(Column = "last_name")]
        public virtual string LastName
        {
            get;
            set;
        }
        /// <summary>
        /// full name
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        [Property(Column = "gender")]
        public virtual string Gender
        {
            get;
            set;
        }
        /// <summary>
        /// 生日
        /// </summary>
        [Property(Column = "birthday")]
        public virtual DateTime? Birthday
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        [Property(Column = "telephone")]
        public virtual string PhoneNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 手机
        /// </summary>
        [Property(Column = "cellphone")]
        public virtual string CellPhone
        {
            get;
            set;
        }
        /// <summary>
        /// 个人网站
        /// </summary>
        [Property(Column = "person_website")]
        public virtual string PersonWebSite
        {
            get;
            set;
        }
        /// <summary>
        /// skype账号
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
        /// 国家
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int? CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 注册浏览器语言
        /// </summary>
        [Property(Column = "register_useragent_language")]
        public virtual string RegisterUserAgentLanguage
        {
            get;
            set;
        }
        /// <summary>
        /// 注册语言别
        /// </summary>
        [Property(Column = "register_language_id")]
        public virtual int? RegisterLanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 注册ip
        /// </summary>
        [Property(Column = "register_ip")]
        public virtual string RegisterIp
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
        /// 总登陆次数
        /// </summary>
        [Property(Column = "total_login_count")]
        public virtual int? TotalLoginCount
        {
            get;
            set;
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Property(Column = "date_login")]
        public virtual DateTime? DateLogin
        {
            get;
            set;
        }
        /// <summary>
        /// 客户等级
        /// </summary>
        [Property(Column = "customer_group_id")]
        public virtual int? CustomerGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// CLUB等级
        /// </summary>
        [Property(Column = "club_level")]
        public virtual int ClubLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 默认运送地址
        /// </summary>
        [Property(Column = "default_shipping_address")]
        public virtual int? DefaultShippingAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 默认账单地址
        /// </summary>
        [Property(Column = "default_billing_address")]
        public virtual int? DefaultBillingAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 历史购买金额
        /// </summary>
        [Property(Column = "order_amount")]
        public virtual decimal? OrderAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 1是，0否
        /// </summary>
        [Property(Column = "high_risk")]
        public virtual bool? HighRisk
        {
            get;
            set;
        }
        /// <summary>
        /// 关联facebook登录
        /// </summary>
        [Property(Column = "facebook_id")]
        public virtual string FacebookId
        {
            get;
            set;
        }
        /// <summary>
        /// 关联FB时间
        /// </summary>
        [Property(Column = "date_facebook")]
        public virtual DateTime? DateFacebook
        {
            get;
            set;
        }
        /// <summary>
        /// 0无，10Jewelry DIY Fan，20Wholesaler，30Retailer，40Others
        /// </summary>
        [Property(Column = "describes_type")]
        public virtual int? DescribesType
        {
            get;
            set;
        }
        /// <summary>
        /// 近一年运费金额
        /// </summary>
        [Property(Column = "total_shipping_fee")]
        public virtual decimal? TotalShippingFee
        {
            get;
            set;
        }

        /// <summary>
        /// 是否享受RCD，1是，0否
        /// </summary>
        [Property(Column = "is_rcd")]
        public virtual bool? IsRcd
        {
            get;
            set;
        }

        /// <summary>
        /// 最后下单时间
        /// </summary>
        [Property(Column = "date_last_placeorder")]
        public virtual DateTime? DateLastPlaceOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 来源类型
        /// </summary>
        [Property(Column = "source_type")]
        public virtual int SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 来源URL
        /// </summary>
        [Property(Column = "source_url")]
        public virtual string SourceUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 状态（是否启用）
        /// </summary>
        [Property(Column = "status")]
        public virtual bool Status
        {
            get;
            set;
        }

        /// <summary>
        /// 传真号码
        /// </summary>
        [Property(Column = "fax_number")]
        public virtual string FaxNumber { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

