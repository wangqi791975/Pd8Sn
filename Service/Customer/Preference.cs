using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 使用偏好
    /// </summary>
    [Serializable]
    public class Preference
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 尺寸单位
        /// </summary>
        public virtual Unit? SizeUnit { get; set; }

        /// <summary>
        /// 重量单位
        /// </summary>
        public virtual Unit? WeightUnit { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public virtual int? CurrencyId { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        public virtual int? LanguageId { get; set; }

        /// <summary>
        /// 展示类型
        /// </summary>
        public virtual ListShowType? ListShowType { get; set; }

        /// <summary>
        /// 展示数量
        /// </summary>
        public virtual ListShowCount? ListShowCount { get; set; }
    }

    /// <summary>
    /// 单位（公制，英制）
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// 公制
        /// </summary>
        Metric,
        /// <summary>
        /// 英制
        /// </summary>
        Imperial
    }

    /// <summary>
    /// 展示类型
    /// </summary>
    public enum ListShowType
    {
        /// <summary>
        /// List
        /// </summary>
        List,
        /// <summary>
        /// Gallery
        /// </summary>
        Gallery
    }

    public enum ListShowCount
    {
        /// <summary>
        /// 30
        /// </summary>
        T = 30,
        /// <summary>
        /// 60
        /// </summary>
        S = 60,
        /// <summary>
        /// 90
        /// </summary>
        N = 90
    }
}
