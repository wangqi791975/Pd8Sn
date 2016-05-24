
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /*
	
CREATE TABLE [dbo].[t_payment_enabled_customer](
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [payment_type] int NOT NULL,--存PaymentType 枚举值
    [customer_id] [INT] NULL,
    [customer_email] [VARCHAR](256) NOT NULL,
    [date_created] [DATETIME] NOT NULL,
    [admin_id] [INT] NOT NULL,
    [account_email] [VARCHAR](256) NOT NULL,
) 

     */
    /// <summary>
	///描述：支付方式禁用客户表 ORM 映射类 
	///创建人:lxf
	///创建时间：2015-04-27 13:54:19
	/// </summary>
    [Class(Table = "t_payment_enabled_customer", Lazy = false, NameType = typeof(PaymentEnabledCustomerPo), DynamicUpdate = true)]
	[Cache(Usage = CacheUsage.NonStrictReadWrite)]
	public class PaymentEnabledCustomerPo
	{
		/// <summary>
		/// 主键,自增长
		/// </summary>
		[Id(1, Name = "Id", Column = "id")]
		[Generator(2, Class = "native")]
		public virtual int Id
		{
			get;
			set;
		}
		/// <summary>
		/// 支付类型:Paypal = 0,Hsbc = 1,BankOfChina=2,WesternUnion = 4,Gc=8,MoneyGram =16,Webmoney=32,Yandex=64,QiWi=128,OceanCreditCard=256
		/// </summary>
		[Property(Column = "payment_type")]
		public virtual int PaymentType
		{
			get;
			set;
		}
		/// <summary>
		/// 禁用对应的客户ID 不一定有值，可能添加的用户邮箱不是注册客户
		/// </summary>
		[Property(Column = "customer_id")]
		public virtual int CustomerId
		{
			get;
			set;
		}
		/// <summary>
		/// 禁用对应的客户邮箱
		/// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
		{
			get;
			set;
		}
		/// <summary>
		/// 添加时间
		/// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
		{
			get;
			set;
		}
		/// <summary>
		/// 管理员ID
		/// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
		{
			get;
			set;
		}
		/// <summary>
		/// 管理员账户
		/// </summary>
        [Property(Column = "account_email")]
        public virtual string AccountEmail
		{
			get;
			set;
		}
	}
}

