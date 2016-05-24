
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户地址表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:37
    /// </summary>
    [Class(Table = "t_customer_address", Lazy = false, NameType = typeof(CustomerAddressPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerAddressPo
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
        /// 客户id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
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
        /// 公司名称
        /// </summary>
        [Property(Column = "company")]
        public virtual string Company
        {
            get;
            set;
        }
        /// <summary>
        /// 街道地址
        /// </summary>
        [Property(Column = "street_address")]
        public virtual string StreetAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 街道地址2
        /// </summary>
        [Property(Column = "street_address2")]
        public virtual string StreetAddress2
        {
            get;
            set;
        }
        /// <summary>
        /// 邮编
        /// </summary>
        [Property(Column = "zip_code")]
        public virtual string ZipCode
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        [Property(Column = "city")]
        public virtual string City
        {
            get;
            set;
        }
        /// <summary>
        /// 州id
        /// </summary>
        [Property(Column = "province_id")]
        public virtual int? ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 州名称
        /// </summary>
        [Property(Column = "province_name")]
        public virtual string ProvinceName
        {
            get;
            set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 电话号码
        /// </summary>
        [Property(Column = "phone_number")]
        public virtual string PhoneNumber
        {
            get;
            set;
        }
    }
}

