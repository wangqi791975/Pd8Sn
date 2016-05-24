using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Web.PaymentCommon.PayInfo
{
    /// <summary>
    /// 钱海支付信息
    /// </summary>
    [Serializable]
    public class OceanPaymentInfo : BasePayInfo
    {
        /// <summary>
        /// 支付状态:未处理
        /// </summary>
        public static readonly string OCEANPAYMENT_PAY_STATUS_UNHANDLE = "-1"; 
        /// <summary>
        /// 支付状态:失败
        /// </summary>
        public static readonly string OCEANPAYMENT_PAY_STATUS_FAILURE = "0";
        /// <summary>
        /// 支付状态:成功
        /// </summary>
        public static readonly string OCEANPAYMENT_PAY_STATUS_SUCCESS = "1";

        /// <summary>
        /// 表自增长ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 支付目标对象ID(订单ID)
        /// </summary>
        public virtual int TargetId { get; set; }
        /// <summary>
        /// 响应类型:0-浏览器响应 1-服务器响应
        /// </summary>
        public virtual string ResponseType { get; set; }
        /// <summary>
        /// 支付通道,比如Webmoney、Yandex、Credit Card、QiWi
        /// </summary>
        public virtual string Method { get; set; }
        /// <summary>
        /// 钱海支付账户
        /// </summary>
        public virtual string Account{ get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public virtual string Terminal { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public virtual string SignValue { get; set; }
        /// <summary>
        /// 网站订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }
        /// <summary>
        /// 订单号 的交易币种,比如USD,JPY
        /// </summary>
        public virtual string OrderCurrency { get; set; }
        /// <summary>
        /// 订单号的交易金额(2位小数)
        /// </summary>
        public virtual decimal OrderAmount{ get; set; }
        /// <summary>
        /// 订单备注信息
        /// </summary>
        public virtual string OrderNotes { get; set; }
        /// <summary>
        /// 持卡人的信用卡卡号 ，截取前6位和后4位
        /// </summary>
        public virtual string CardNumber{ get; set; }
        /// <summary>
        /// Oceanpayment唯一的支付交易号
        /// </summary>
        public virtual string PaymentId { get; set; }
        /// <summary>
        /// 交易类型：0-消费交易,1-预授权交易
        /// </summary>
        public virtual string PaymentAuthType { get; set; }
        /// <summary>
        /// 交易状态: -1: 待处理,0: 失败,1: 成功
        /// </summary>
        public virtual string PaymentStatus { get; set; }
        /// <summary>
        /// 支付详情
        /// </summary>
        public virtual string PaymentDetails { get; set; }
        /// <summary>
        /// 未通过的风控规则
        /// </summary>
        public virtual string PaymentRisk { get; set; }
        /// <summary>
        /// 响应错误代码
        /// </summary>
        public virtual string ErrorCode { get; set; } 
        /// <summary>
        /// 所有返回信息组成的key-value用空格分隔起来的信息
        /// </summary>
        public virtual string TotalInfo { get; set; }
    }
}
