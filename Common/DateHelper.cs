using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;

namespace Com.Panduo.Common
{
    public static class DateHelper
    {
        /// <summary>
        /// 否有效日期
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static bool IsValidDate(this string dateString)
        {
            bool isDate = false;
            if (!String.IsNullOrEmpty(dateString))
            {
                try
                {
                    Convert.ToDateTime(dateString);
                    isDate = true;
                }
                catch (Exception)
                {
                    isDate = false;
                }
            }

            return isDate;
        }

        /// <summary>
        /// 数据库查询开始日期格式：yyyy-MM-dd 00:00:00
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static string ToQueryFromDate(this string dateString)
        {
            if (dateString.IsValidDate())
            {
                return DateTime.Parse(dateString).ToString("yyyy-MM-dd 00:00:00", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            return dateString;
        }

        /// <summary>
        /// 数据库查询截止日期格式：yyyy-MM-dd 23:59:59
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static string ToQueryToDate(this string dateString)
        {
            if (dateString.IsValidDate())
            {
                return DateTime.Parse(dateString).ToString("yyyy-MM-dd 23:59:59", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            return dateString;
        }

        /// <summary>
        /// 日期比较,返回值 0：dtFrom= dtTo；1：dtFrom > dtTo；-1：dtFrom &lt; dtTo；
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        public static int Compare(this DateTime dtFrom, DateTime dtTo)
        {
            return DateTime.Compare(dtFrom, dtTo);
        }

        /// <summary>
        /// 日期字符串比较,返回值 0：dtFrom= dtTo；1：dtFrom > dtTo；-1：dtFrom &lt; dtTo；
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        public static int CompareDateTime(this string dtFrom, string dtTo)
        {
            return DateTime.Parse(dtFrom).Compare(DateTime.Parse(dtTo));
        }

        /// <summary>
        /// 当天
        /// </summary>
        /// <returns></returns>
        public static string Today()
        {
            return DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        //当天日期减去相应天数
        public static string GetTimeByDayReduce(double day)
        {
            return DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.AddDays(day).Day.ToString();
        }

        //指定月份年份的第一天
        public static DateTime GetFirstDayOfMonth(int Year, int Month)
        {
            return Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-1");
        }

        //指定月份年份的最后一天
        public static DateTime GetLastDayOfMonth(int Year, int Month)
        {
            int Days = DateTime.DaysInMonth(Year, Month);
            return Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-" + Days.ToString());
        }

        //当前一周时间
        public static void ThisWeek(out string bDate, out string eDate)
        {
            DateTime firstDay = Convert.ToDateTime(Today());
            double theday;
            if (firstDay.DayOfWeek == DayOfWeek.Sunday) { theday = 7; }
            else if (firstDay.DayOfWeek == DayOfWeek.Monday) { theday = 1; }
            else if (firstDay.DayOfWeek == DayOfWeek.Tuesday) { theday = 2; }
            else if (firstDay.DayOfWeek == DayOfWeek.Wednesday) { theday = 3; }
            else if (firstDay.DayOfWeek == DayOfWeek.Thursday) { theday = 4; }
            else if (firstDay.DayOfWeek == DayOfWeek.Friday) { theday = 5; }
            else { theday = 6; }
            double bday = -theday;
            double eday = 7 - theday;
            bDate = firstDay.AddDays(bday).ToString();
            eDate = firstDay.AddDays(eday).ToString();
        }

        //过去一周时间
        public static void BeforeWeek(out string bDate, out string eDate)
        {

            DateTime firstDay = Convert.ToDateTime(GetTimeByDayReduce(-ThisWeekLastDay() + 1));
            double theday;
            if (firstDay.DayOfWeek == DayOfWeek.Sunday) { theday = 7; }
            else if (firstDay.DayOfWeek == DayOfWeek.Monday) { theday = 1; }
            else if (firstDay.DayOfWeek == DayOfWeek.Tuesday) { theday = 2; }
            else if (firstDay.DayOfWeek == DayOfWeek.Wednesday) { theday = 3; }
            else if (firstDay.DayOfWeek == DayOfWeek.Thursday) { theday = 4; }
            else if (firstDay.DayOfWeek == DayOfWeek.Friday) { theday = 5; }
            else { theday = 6; }
            double bday = -theday;
            double eday = 7 - theday;
            bDate = firstDay.AddDays(bday).ToString();
            eDate = firstDay.AddDays(eday).ToString();
        }

        //本周最后一天
        public static double ThisWeekLastDay()
        {
            DateTime firstDay = Convert.ToDateTime(Today());
            double theday;
            if (firstDay.DayOfWeek == DayOfWeek.Sunday) { theday = 7; }
            else if (firstDay.DayOfWeek == DayOfWeek.Monday) { theday = 1; }
            else if (firstDay.DayOfWeek == DayOfWeek.Tuesday) { theday = 2; }
            else if (firstDay.DayOfWeek == DayOfWeek.Wednesday) { theday = 3; }
            else if (firstDay.DayOfWeek == DayOfWeek.Thursday) { theday = 4; }
            else if (firstDay.DayOfWeek == DayOfWeek.Friday) { theday = 5; }
            else { theday = 6; }
            return 7 - theday;
        }

        //本月第一天
        public static DateTime GetFirstDayOfMonthTime()
        {
            return GetFirstDayOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        }

        //本月最后一天
        public static DateTime GetLastDayOfMonthTime()
        {
            return GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.Month);
        }

        //上月第一天
        public static DateTime GetFirstDayOfBeforeMonthTime()
        {
            return GetFirstDayOfMonth(DateTime.Now.Year, DateTime.Now.Month - 1);
        }

        //上月最后一天
        public static DateTime GetLastDayOfBeforeMonthTime()
        {
            return GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.Month - 1);
        }
        //根据下拉框状态获取相应的时间
        public static void GetTime(out string lastDateTime, out string firstDateTime, string state)
        {
            switch (state)
            {
                case "": firstDateTime = ""; lastDateTime = "";
                    break;
                case "today": firstDateTime = Today(); lastDateTime = "";
                    break;
                case "yesterday": firstDateTime = Today(); lastDateTime = GetTimeByDayReduce(-1);
                    break;
                case "last7days": firstDateTime = Today(); lastDateTime = GetTimeByDayReduce(-7);
                    break;
                case "thisweek": ThisWeek(out firstDateTime, out lastDateTime);
                    break;
                case "lastweek": BeforeWeek(out firstDateTime, out lastDateTime);
                    break;
                case "thismonth": firstDateTime = GetFirstDayOfMonthTime().ToString(); lastDateTime = GetLastDayOfMonthTime().ToString();
                    break;
                case "lastmonth": firstDateTime = GetFirstDayOfBeforeMonthTime().ToString(); lastDateTime = GetLastDayOfBeforeMonthTime().ToString();
                    break;
                default: firstDateTime = ""; lastDateTime = "";
                    break;
            }
        }

        /// <summary>
        /// 返回近一个月的开始日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNearOneMonthOfToday()
        {
            return DateTime.Now.AddDays(-30);
        }
        /// <summary>
        /// 返回近一个月的开始日期字符串
        /// </summary>
        /// <returns></returns>
        public static string NearOneMonthOfTodayString
        {
            get
            {
                return DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
        }
        /// <summary>
        /// 返回现在的日期字符串
        /// </summary>
        /// <returns></returns>
        public static string DateOfTodayString
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
        }
    }
}
