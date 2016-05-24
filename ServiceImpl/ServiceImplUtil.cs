using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Com.Panduo.ServiceImpl
{
    internal static class ServiceImplUtil
    {
        public static string ToSqlString(this string orgValue)
        {
            orgValue = orgValue ?? string.Empty;
            return orgValue.Trim().Replace("%", @"\%").Replace("_", @"\%");
        }

        public static string ToSqlString(this object orgValue)
        {
            if (orgValue == null)
            {
                return string.Empty;
            }
            else
            {
                return orgValue.ToString().ToSqlString();
            }
        }

        /// <summary>
        /// 格式化开始日期成符合查询的日期格式(yyyy-MM-dd 00:00:00)
        /// </summary>
        public static DateTime ToDateQueryFrom(this object dateFrom)
        {
            var date = DateTime.Parse("1800-1-1");

            if(DateTime.TryParse(dateFrom.ToString(),out date))
            {
                date = DateTime.Parse(date.ToString("yyyy-MM-dd 00:00:00", System.Globalization.DateTimeFormatInfo.InvariantInfo)); ;
            }

            return date;
        }

        /// <summary>
        /// 格式化结束日期成符合查询的日期格式(yyyy-MM-dd 23:59:59)
        /// </summary>
        public static DateTime ToDateQueryTo(this object dateTo)
        {
             var date = DateTime.MaxValue;

            if(DateTime.TryParse(dateTo.ToString(),out date))
            {
               date = DateTime.Parse(date.ToString("yyyy-MM-dd 23:59:59", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            }

            return date;
        } 
    }
}
