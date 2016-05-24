using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.OceanPayment
{
    /// <summary>
    /// 钱海支付请求参数
    /// </summary>
    [Serializable]
    public class OceanPaymentParm
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public virtual string Terminal { get; set; }  
        /// <summary>
        /// 签名
        /// </summary>
        public virtual string SignValue { get; set; }
        /// <summary>
        /// 支付返回Url地址
        /// </summary>
        public virtual string BackUrl { get; set; }
        /// <summary>
        /// 支付方式Method(比如Webmoney、Yandex、Credit Card、QiWi)
        /// </summary>
        public virtual string Methods { get; set; }
        /// <summary>
        /// 显示界面类型：0: PC端页面,1: 手机端页面
        /// </summary>
        public virtual string Pages { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }
        /// <summary>
        /// 交易币种
        /// </summary>
        public virtual string OrderCurrency { get; set; }
        /// <summary>
        /// 订单交易金额,只取小数点后2位
        /// </summary>
        public virtual decimal OrderAmount { get; set; }
        /// <summary>
        /// 交易备注
        /// </summary>
        public virtual string OrderNotes { get; set; }
        /// <summary>
        /// 购物车程序 ，如 zencart、magento
        /// </summary>
        public virtual string CartInfo { get; set; }
        /// <summary>
        /// Oceanpayment  API接口版本
        /// </summary>
        public virtual string CartApi { get; set; }

        /// <summary>
        /// 账单地址
        /// </summary>
        public virtual OceanPaymentBillingAddress BillingAddress { get; set; }
        /// <summary>
        /// 货运地址
        /// </summary>
        public virtual OceanPaymentShippingAddress ShippingAddress { get; set; } 
    }


}