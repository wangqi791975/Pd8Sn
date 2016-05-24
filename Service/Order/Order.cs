using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 订单实体
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// Order自增Id
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 订单编号(YYMMDD前台显示)
        /// </summary>
        public virtual string OrderNo { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public virtual DateTime OrderTime { get; set; }

        /// <summary>
        /// 支付日期
        /// </summary>
        public virtual DateTime PaidTime { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public virtual int PaymentMethod { get; set; }

        /// <summary>
        /// 信用卡类型
        /// </summary>
        public virtual int? CollectType { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public virtual decimal ExchangeRate { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public virtual string Currency { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual OrderStatusType OrderStatus { get; set; }

        /// <summary>
        /// 订单类型(顾客下单，Email下单，客服拆单)
        /// </summary>
        public virtual int OrderType { get; set; }

        /// <summary>
        /// 支付状态(已支付待验证、处理中)
        /// </summary>
        public virtual PaidStatusType PaidStatus { get; set; }


        /// <summary>
        /// 订单备注
        /// </summary>
        public virtual string OrderRemark { get; set; }

        /// <summary>
        /// 包裹重量
        /// </summary>
        public virtual decimal PackageWeight { get; set; }

        /// <summary>
        /// 物理重量[sum(产品物理重量*购买数量)]
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 最终重量(实际重和体积重取大)
        /// </summary>
        public virtual decimal ShippingWeight { get; set; }

        /// <summary>
        /// 订单来源(1手机、网站0)
        /// </summary>
        public virtual int OrderSource { get; set; }

        /// <summary>
        /// 是否关闭(逻辑删除)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public virtual bool IsClose { get; set; }

        /// <summary>
        /// 运送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }


        /// <summary>
        /// 是有否升级运送方式
        /// </summary>
        public virtual bool IsUpgradeShipping { get; set; }

        /// <summary>
        /// 巴西税号
        /// </summary>
        public virtual string CustomerTaxNumber { get; set; }

        /// <summary>
        /// 税号类型
        /// </summary>
        public virtual string CustomerTaxType { get; set; }

        /// <summary>
        /// 报关币种
        /// </summary>
        public virtual string ReportCurrencyCode { get; set; }

        /// <summary>
        /// 报关金额
        /// </summary>
        public virtual decimal ReportMoney { get; set; }

        /// <summary>
        /// 报关运费金额
        /// </summary>
        public virtual decimal ReportShippingMoney { get; set; }

        /// <summary>
        /// 缺货等待类型
        /// </summary>
        public virtual int SoldWaitType { get; set; }

        /// <summary>
        /// 送礼品等级
        /// </summary>
        public virtual string GiftLevel { get; set; }

        /// <summary>
        /// 是否已全部评论
        /// </summary>
        public virtual bool IsReviewAll { get; set; }

        /// <summary>
        /// 订单费用
        /// </summary>
        public OrderCost OrderCost { get; set; }

        /// <summary>
        /// 语种code
        /// </summary>
        public virtual string LanguageCode { get; set; }

        /// <summary>
        /// 下单时的IP
        /// </summary>
        public virtual string OrderIpAddress { get; set; }
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusType
    {
        Pending = 10,
        Processing = 20,
        Shipped = 30,
        Update = 40,
        Refund=41,
        UnderChecking = 42,
        Deleted=50,
        Canceled =100
    }

    /// <summary>
    /// 订单支付状态
    /// </summary>
    public enum PaidStatusType
    {
        /// <summary>
        /// 未支付
        /// </summary>
        NotPay = 0,
        /// <summary>
        /// 提交了付款信息
        /// </summary>
        Submit = 1,
        /// <summary>
        /// 部分支付
        /// </summary>
        PartPay = 2,
        /// <summary>
        /// 全部支付
        /// </summary>
        FullPay = 4,
        /// <summary>
        /// 付款待确认
        /// </summary>
        Confirming = 6
    }
}