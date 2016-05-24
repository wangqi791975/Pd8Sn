using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单收货地址 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:30
    /// </summary>
    [Class(Table = "t_order_address", Lazy = false, NameType = typeof(OrderAddressPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderAddressPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "OrderAddressId", Column = "order_address_id")]
        [Generator(2, Class = "native")]
        public virtual int OrderAddressId
        {
            get;
            set;
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 地址簿ID
        /// </summary>
        [Property(Column = "address_book_id")]
        public virtual int AddressBookId
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
        /// 公司名称
        /// </summary>
        [Property(Column = "company")]
        public virtual string Company
        {
            get;
            set;
        }
        /// <summary>
        /// FirstName
        /// </summary>
        [Property(Column = "first_name")]
        public virtual string FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// LastName
        /// </summary>
        [Property(Column = "last_name")]
        public virtual string LastName
        {
            get;
            set;
        }
        /// <summary>
        /// FullName
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
        {
            get;
            set;
        }
        /// <summary>
        /// 街道1
        /// </summary>
        [Property(Column = "street_address")]
        public virtual string StreetAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 街道2
        /// </summary>
        [Property(Column = "street_address2")]
        public virtual string StreetAddress2
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
        /// 邮编
        /// </summary>
        [Property(Column = "zip_code")]
        public virtual string ZipCode
        {
            get;
            set;
        }
        /// <summary>
        /// 省份ID
        /// </summary>
        [Property(Column = "province_id")]
        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 省份
        /// </summary>
        [Property(Column = "province")]
        public virtual string Province
        {
            get;
            set;
        }
        /// <summary>
        /// 国家ID
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        [Property(Column = "country")]
        public virtual string Country
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
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 地址类型
        /// </summary>
        [Property(Column = "address_type")]
        public virtual int AddressType
        {
            get;
            set;
        }
    }
}

