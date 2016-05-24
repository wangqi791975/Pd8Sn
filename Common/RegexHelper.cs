using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Com.Panduo.Common;

namespace Com.Panduo.Common
{
    public class RegexHelper
    {
        /// <summary>
        /// 判断是否是整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            string pattern = @"^[+-]?\d+$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否是小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(string value)
        {
            string pattern = @"^(\d*\.)?\d+$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 是否是实数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsRealNum(string value)
        {
            string pattern = @"^[-+]?[0-9]+\.?[0-9]*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否是合法的Email
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否是字母、数字和特殊字符（-_.'&）的组合
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWord(string value)
        {
            string pattern = @"^([a-zA-z0-9_\-\.\'\&])*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否是字母和数字的组合
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWordChar(string value)
        {
            string pattern = @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否是合法的密码格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPassword(string value)
        {
            string pattern = @"^(\w|\s|[`~!@#\$%\^&\*\(\)_\+\-=\{\}\[\]\:\'\<\>,\.\?\|/\\;""])*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否为有效的用户名，包含英文字母、数字、和特殊字符（'_-.&）,特殊字符必须在字母或数字之后，并且只能在字串中间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUserName(string value)
        {
            string pattern = @"^(([a-zA-z0-9][\'_\-\.\&])*)?[a-zA-z0-9]+$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 判断是否是中文、字母、数字和特殊字符（-_.'&）的组合
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsName(string value)
        {
            string pattern = @"^(\w|\s|[\'_\-\.\&])*$";
            return Regex.IsMatch(value, pattern);
        }

        // 获取批量字符
        public static string GetStrUnit(string unit)
        {
            return Regex.Replace(unit, @"[0-9]", "", RegexOptions.IgnoreCase).ToString().Trim();
        }

        /// <summary>
        /// 得到批量单位中的数字
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static decimal GetUnitNum(string unit)
        {
            unit = unit.Trim().Replace(" ", "");
            //定义并初始化返回值
            decimal unitNum = 0;

            //定义并初始化非数字部分
            string left = Regex.Replace(unit, @"[A-Za-z]", "", RegexOptions.IgnoreCase);
            try
            {
                unitNum = decimal.Parse(left);
            }
            catch //如果批量中不包括数字
            {
                if (unit.Trim().ToLower().Replace(" ", "") == "twounit")
                {
                    unitNum = 2;
                }
                else
                {
                    unitNum = 1;
                }
            }
            return unitNum;
        }

        /// <summary>
        /// 根据数量（批量）和批量单位得到个体数
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static int GetQuantity(decimal quantity, string unit)
        {
            decimal dunit = GetUnitNum(unit);
            return (int)(quantity * dunit + 0.5m);
        }

        public static string GetActualQty(decimal quantity, string unit)
        {
            int actualQty = 0;
            decimal dunit = GetUnitNum(unit);
            actualQty = (int)(quantity * dunit + 0.5m);
            return actualQty.ToString() + " " + GetStrUnit(unit);
        }

        /// <summary>
        /// 根据个体数量和批量单位得到批量数量
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static decimal GetUnitQty(decimal quantity, string unit)
        {
            decimal dunit = GetUnitNum(unit);
            return quantity / dunit;
        }

        /// <summary>
        /// 得到最小单位价格，如果是以重量计算的单位，要除以批量
        /// </summary>
        /// <returns></returns>
        public static decimal GetLeastPrice(string unit, decimal price)
        {
            //去掉批量中的数字部分
            string sUnit = Regex.Replace(unit, @"[0-9]", "", RegexOptions.IgnoreCase);

            //批量是以重量计算(g或pound)
            if (sUnit.Trim().ToLower() == "g" || sUnit.ToLower().IndexOf("pound") > 0)
            {
                //得到批量单位中的数字部分
                decimal dUnit = GetUnitNum(unit);
                return decimal.Divide(price, dUnit);
            }
            else
            {
                //批量已经是最小单位
                return price;
            }
        }

        /// <summary>
        /// 分类显示商品价格，注意数据库里面保存的是最小单位价格
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static decimal GetUnitPrice(string unit, decimal price)
        {
            // 去掉批量中的数字部分
            string sUnit = Regex.Replace(unit, @"[0-9]", "", RegexOptions.IgnoreCase);

            // 批量是以重量计算(g或pound)，要显示批量价格
            if (sUnit.Trim().ToLower() == "g" || sUnit.ToLower().IndexOf("pound") > 0)
            {
                // 得到批量单位中的数字部分
                decimal dUnit = GetUnitNum(unit);
                return decimal.Multiply(price, dUnit);
            }
            else
            {
                // 个体单位批量的商品显示最小单位价格
                return price;
            }
        }
    }
}
