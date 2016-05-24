using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl
{
    /// <summary>
    /// 实现层专用辅助类
    /// </summary>
    public static class ImplToolHelper
    {
        private const string FORMAT_MONEY = "0,000.00";
        private const string FORMAT_DATE_SHORT = "MMM dd,yyyy";
        private const string FORMAT_DATE_SHORT_CHINESE = "yyyy-MM-dd";
        private const string FORMAT_DATE_LONG = "MMM dd,yyyy H:mm:ss a";
        private const string FORMAT_DATE_LONG_CHINESE = "yyyy-MM-dd HH:mm:ss";
        #region 数据显示
        /// <summary>
        /// 转换为短日期格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isChinese"></param>
        /// <returns></returns>
        public static string ToDateShortString(this DateTime dateTime, bool isChinese = false)
        {
            return dateTime.ToDateString(isChinese ? FORMAT_DATE_SHORT_CHINESE : FORMAT_DATE_SHORT);
        }

        /// <summary>
        /// 转换为长日期格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isChinese"></param>
        /// <returns></returns>
        public static string ToDateLongString(this DateTime dateTime, bool isChinese = false)
        {
            return dateTime.ToDateString(isChinese ? FORMAT_DATE_LONG_CHINESE : FORMAT_DATE_LONG);
        }

        #endregion

        /// <summary>
        /// 获取指定小数点的decimal数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal GetRoundValue(decimal value, int decimals=2)
        {
            return decimal.Round(value * 1.00M, decimals, MidpointRounding.AwayFromZero);
        }
    }
}

