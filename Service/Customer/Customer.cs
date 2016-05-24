using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 客户
    /// </summary>
    [Serializable]
    public class Customer
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string LastName { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string FullName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Password { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual string Birthday { get; set; }

        /// <summary>
        ///  客户类型
        /// </summary>
        public virtual CustomerType? CustomerType { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public virtual int? Country { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Gender? Gender { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Telphone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Cellphone { get; set; }

        /// <summary>
        /// 个人网站
        /// </summary>
        public virtual string PersonWebSite { get; set; }

        /// <summary>
        /// skype账号
        /// </summary>
        public virtual string Skype { get; set; }

        /// <summary>
        /// 总登陆次数
        /// </summary>
        public virtual int? TotalLoginCount { get; set; }

        /// <summary>
        /// 是否订阅
        /// </summary>
        public virtual bool IsSubscripttion { get; set; }

        /// <summary>
        /// 是否VIP
        /// </summary>
        public virtual bool IsVip { get; set; }

        /// <summary>
        /// 是否Club
        /// </summary>
        public virtual bool IsClub { get; set; }

        /// <summary>
        /// 是否RCD
        /// </summary>
        public virtual bool IsRcd { get; set; }

        /// <summary>
        /// 是否高危
        /// </summary>
        public virtual bool? IsDanger { get; set; }

        /// <summary>
        /// VIP折扣
        /// </summary>
        public virtual decimal VipDiscount { get; set; }

        /// <summary>
        /// RCD折扣
        /// </summary>
        public virtual decimal RcdDiscount { get; set; }

        /// <summary>
        /// 默认货运地址
        /// </summary>
        public virtual int? ShippingAddress { get; set; }

        /// <summary>
        /// 默认账单地址
        /// </summary>
        public virtual int? BillingAddress { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public virtual DateTime? LastLoginDateTime { get; set; }

        /// <summary>
        /// 历史购买金额
        /// </summary>
        public virtual decimal? HistoryAmount { get; set; }

        /// <summary>
        /// 客户等级
        /// </summary>
        public virtual int? CustomerGroupId { get; set; }

        /// <summary>
        /// CLUB等级
        /// </summary>
        public virtual int ClubLevel { get; set; }

        /// <summary>
        /// 关联facebook登录
        /// </summary>
        public virtual string FacebookId { get; set; }

        /// <summary>
        /// 近一年运费金额
        /// </summary>
        public virtual decimal? TotalShippingFee { get; set; }

        /// <summary>
        /// 最后下单时间
        /// </summary>
        public virtual DateTime? DateLastPlaceOrder { get; set; }

        /// <summary>
        /// 注册语言
        /// </summary>
        public int? RegisterLanguageId { get; set; }

        /// <summary>
        /// 状态（是否启用）
        /// </summary>
        public virtual bool Status { get; set; }

        /// <summary>
        /// 传真号码
        /// </summary>
        public virtual string FaxNumber { get; set; }

        /// <summary>
        /// 注册信息
        /// </summary>
        public virtual RegisterInfo RegisterInfo { get; set; }
    }

    /// <summary>
    /// 客户类型
    /// </summary>
    public enum CustomerType
    {
        //0无，10Jewelry DIY Fan，20Wholesaler，30Retailer，40Others
        /// <summary>
        /// 无
        /// </summary>
        Null = 0,
        /// <summary>
        /// 首饰DIY迷
        /// </summary>
        JewelryDiyFan = 10,
        /// <summary>
        /// 批发商
        /// </summary>
        Wholesaler = 20,
        /// <summary>
        /// 零售商
        /// </summary>
        Retailer = 30,
        /// <summary>
        /// 其他
        /// </summary>
        Others = 40
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 女
        /// </summary>
        Female = 'f',
        /// <summary>
        /// 男
        /// </summary>
        Male = 'm'
    }
}
