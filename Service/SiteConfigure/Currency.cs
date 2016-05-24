using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 币种，汇率
    /// </summary>
    [Serializable]
    public class Currency
    {

        /// <summary>
        /// 中文名称
        /// </summary>
        public virtual string ChineseName
        {
            get;
            set;
        }

        /// <summary>
        /// 币种ID
        /// </summary>
        public virtual int CurrencyId
        {
            get;
            set;
        }


        /// <summary>
        /// 简码
        /// </summary>
        public virtual string CurrencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public virtual string SymbolLeft
        {
            get;
            set;
        }

        /// <summary>
        ///后缀
        /// </summary>
        public virtual string SymbolRight { get; set; }

        /// <summary>
        /// 小数点位数
        /// </summary>
        public virtual int DecimalPlaces { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual bool Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public virtual decimal ExchangeRate { get; set; }

        /// <summary>
        /// 汇率(远程)
        /// </summary>
        public virtual decimal? ExchangeRateRemote { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        public virtual DateTime? DateModified { get; set; }

        /// <summary>
        /// 符号简写,比如美元$,人民币￥
        /// </summary>
        public virtual string SymbolShort
        {
            get;
            set;
        }

        /// <summary>
        /// 格式化金额，输出格式化后的字符串
        /// </summary>
        /// <param name="amount">要格式化的金额</param>
        /// <returns></returns>
        public string Format(decimal amount)
        {
            return string.Format("{0}{1" + (DecimalPlaces <= 0 ? string.Empty : string.Format(":0.{0}", new string('0', DecimalPlaces))) + "}{2}", SymbolLeft, amount, SymbolRight);
        }

        /// <summary>
        /// 格式化简短符号金额，输出格式化后的字符串
        /// </summary>
        /// <param name="amount">要格式化的金额</param>
        /// <returns></returns>
        public string FormatShort(decimal amount)
        {
            return string.Format("{0}{1" + (DecimalPlaces <= 0 ? string.Empty : string.Format(":0.{0}", new string('0', DecimalPlaces))) + "}", SymbolShort, amount);
        }

        public string Format()
        {
            return string.Format("{0}{1}", SymbolLeft, SymbolRight);
        }
    }
}
