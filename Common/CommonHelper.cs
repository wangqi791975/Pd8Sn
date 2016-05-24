using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 公共方法类-用于存放普通公用的方法
    /// </summary>
    public static partial class CommonHelper
    {
        #region 常用方法
        /// <summary>
        /// 判断对象是否为null,集合对象（包括数组）是否元素个数为0
        /// </summary>
        /// <typeparam name="T">任意对象数据类型</typeparam>
        /// <param name="data">要判断的数据</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T data) where T : class
        {
            if (data == null)
            {
                return true;
            }

            if (data is string)
            {
                return string.IsNullOrEmpty(data as string);
            }

            if (data is ICollection)
            {
                return (data as ICollection).Count == 0;
            }

            return false;
        }
        /// <summary>
        /// 如果对象为null则直接实例化一个新对象否则返回该对象
        /// <para>如果T为泛型接口则只处理IList,IEnumerable,ICollection,IDictionary,ISet</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToNullSafe<T>(this T obj) where T : class
        {
            if (obj != null)
            {
                return obj;
            }

            var type = typeof(T);

            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {

                    var paramType = type.GetGenericArguments()[0];

                    if (type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        return (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(paramType));
                    }
                    else if (type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        var paramValueType = type.GetGenericArguments()[1];

                        return (T)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(paramType, paramValueType));
                    }
                    else if (type.GetGenericTypeDefinition() == typeof(ISet<>))
                    {
                        return (T)Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(paramType));
                    }
                }
            }
            else
            {
                if (type.IsArray)
                {
                    return (T)Activator.CreateInstance(type, new object[] { 0 });
                }

                return Activator.CreateInstance<T>();
            }

            return default(T);
        }

        /// <summary>
        /// 用指定分隔符号将一个集合连接成字符串
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="data">要连接的集合</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> data, string split = ",")
        {
            return data.IsNullOrEmpty() || data.Count() == 0 ? string.Empty : data.Select(c => c.ToString()).Where(c => !string.IsNullOrEmpty(c)).Aggregate((a, b) => a + split + b);
        }

        /// <summary>
        /// 用指定分隔符将一个字符串分隔成一个集合(split为正则表达式格式)
        /// </summary>
        /// <typeparam name="T">最终数据类型</typeparam>
        /// <param name="data">要分隔的数据</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static IList<T> SplitRegex<T>(this string data, string split) where T : struct
        {
            return Regex.Split(data, @split, RegexOptions.IgnorePatternWhitespace).Where(c => !c.IsNullOrEmpty()).Select(c => c.ParseTo(default(T))).ToList();
        }

        /// <summary>
        /// 用指定分隔符将一个字符串分隔成一个字符串集合(split为正则表达式格式)
        /// </summary> 
        /// <param name="data">要分隔的数据</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static IList<string> SplitRegex(this string data, string split)
        {
            return Regex.Split(data, @split, RegexOptions.IgnorePatternWhitespace).Where(c => !c.IsNullOrEmpty()).ToList();
        }

        /// <summary>
        /// 用指定分隔符将一个字符串分隔成一个集合(split为普通字符串)
        /// </summary>
        /// <typeparam name="T">最终数据类型</typeparam>
        /// <param name="data">要分隔的数据</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static IList<T> Split<T>(this string data, string split) where T : struct
        {
            return Split<T>(data, split.ToCharArray());
        }

        /// <summary>
        /// 用指定分隔符将一个字符串分隔成一个字符串集合(split为普通字符串)
        /// </summary> 
        /// <param name="data">要分隔的数据</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static IList<string> Split(this string data, string split)
        {
            return Split(data,split.ToCharArray());
        }

        /// <summary>
        /// 用指定分隔符将一个字符串分隔成一个集合
        /// </summary>
        /// <typeparam name="T">最终数据类型</typeparam>
        /// <param name="data">要分隔的数据</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static IList<T> Split<T>(this string data, char[] split) where T : struct
        {
            return data.Split(split, StringSplitOptions.RemoveEmptyEntries).Where(c => !c.IsNullOrEmpty()).Select(c => c.ParseTo(default(T))).ToList();
        }

        /// <summary>
        /// 用指定分隔符将一个字符串分隔成一个集合
        /// </summary>
        /// <param name="data">要分隔的数据</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static IList<string> Split(this string data, char[] split)
        {
            return data.Split(split,StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 将字符串进行HTML编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns>编码后的字符串</returns>
        public static string ToHtml(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : HttpUtility.HtmlEncode(s);
        }
        /// <summary>
        /// 将字符串进行HTML解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns>解码后的字符串</returns>
        public static string FromHtml(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : HttpUtility.HtmlDecode(s);
        }
        /// <summary>
        /// 将字符串进行Url编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns>编码后的字符串</returns>
        public static string ToUrl(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// 将字符串进行Url解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns>解码后的字符串</returns>
        public static string FromUrl(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : HttpUtility.UrlDecode(s);
        }

        /// <summary>
        /// 金额格式转换
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToMoney(this decimal dec, string format)
        {
            return dec.ToString(format, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 两位小数的金额格式
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="useLocalSetting"></param>
        /// <returns></returns>
        public static string ToMoney(this decimal dec, bool? useLocalSetting)
        {
            return useLocalSetting.GetValueOrDefault() ? dec.ToString("0.00") : dec.ToMoney("0.00");
        }
        /// <summary>
        /// 获取枚举值的键值对
        /// </summary>
        /// <param name="o">枚举类型</param>
        /// <returns></returns>
        public static IList<KeyValuePair<int, string>> ToEnumList(this Type o)
        {
            var list = new List<KeyValuePair<int, string>>();
            var values = Enum.GetValues(o);
            foreach (var item in values)
            {
                list.Add(new KeyValuePair<int, string>((int)item, Enum.GetName(o, item)));
            }
            return list;
        }

        /// <summary>
        /// 获取枚举值的键值对
        /// </summary>
        /// <param name="o">枚举类型</param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> ToEnumStringList(this Type o)
        {
            return o.ToEnumList().Select(c => new KeyValuePair<string, string>(c.Key.ToString(), c.Value)).ToList();
        }

        /// <summary>
        /// 根据值返回枚举文本
        /// </summary>
        /// <param name="o">枚举类型</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumText(this Type o, object value)
        {
            return value.IsNullOrEmpty() ? string.Empty : Enum.GetName(o, value);
        }

        /// <summary>
        /// 替换 用户输入的内容中,包含html字符引起的错误
        /// </summary>
        /// <returns></returns>
        public static string FilterHtml(this string str)
        {
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);
            str = str.Replace("&", string.Empty);

            return str;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">要查找的值</param>
        /// <param name="c">包含数据的容器</param>
        /// <returns></returns>
        public static bool In<T>(this T t, params T[] c)
        {
            return c.Any(i => i.Equals(t));
        }
        /// <summary>
        /// 重复字符
        /// </summary>
        /// <param name="times">个数</param>
        /// <param name="letter">字符</param>
        /// <returns></returns>
        public static string Repeat(int times, char letter)
        {
            return string.Empty.PadLeft(times, letter);
        }

        /// <summary>
        /// 重复空格
        /// </summary>
        /// <param name="times">个数</param>
        /// <returns></returns>
        public static string Repeat(int times)
        {
            return Repeat(times, ' ');
        }

        /// <summary>
        /// 获取指定范围内连续的序列
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="from">开始序列</param>
        /// <param name="to">结束序列</param>
        /// <returns></returns>
        public static IList<T> Range<T>(int from, int to) where T : struct
        {
            return Enumerable.Range(from, to).Select(c => c.ToString().ParseTo(default(T))).ToList();
        }

        /// <summary>
        /// 是否满足正则表达式
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            return IsMatch(s,pattern,RegexOptions.IgnoreCase);
        }

        public static bool IsMatch(this string s, string pattern,RegexOptions regexOptions)
        {
            return s.IsNullOrEmpty() ? false : Regex.IsMatch(s, pattern, regexOptions);
        }

        /// <summary>
        /// 找到符合条件的第一个值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string s, string pattern)
        {
            return s.IsNullOrEmpty() ? string.Empty : Regex.Match(s, pattern).Value;
        }
        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }
        /// <summary>
        /// 转换为整数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return IsInt(s) ? s.ParseTo(int.MinValue) : 0;
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamel(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : s[0].ToString().ToLower() + s.Substring(1);
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascal(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : s[0].ToString().ToUpper() + s.Substring(1);
        }

        /// <summary>
        /// 转换字典到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ToObject<T>(this Dictionary<string, object> data) where T : class, new()
        {
            var instance = new T();

            if (data == null)
            {
                return default(T);
            }

            var properties = instance.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (data.ContainsKey(properties[i].Name))
                {
                    try
                    {
                        if (!properties[i].PropertyType.IsGenericType)
                        {
                            properties[i].SetValue(instance, Convert.ChangeType(data[properties[i].Name], properties[i].PropertyType), null);
                        }
                        else
                        {
                            //泛型赋值

                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return instance;
        }
        /// <summary>
        /// 转换字典到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ToObject<T>(this NameValueCollection data) where T : class, new()
        {
            var instance = new T();

            if (data == null)
            {
                return default(T);
            }

            var properties = instance.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (data.AllKeys.Contains(properties[i].Name))
                {
                    try
                    {
                        properties[i].SetValue(instance, Convert.ChangeType(data[properties[i].Name], properties[i].PropertyType), null);//Convert.ChangeType(jsonData[properties[i].Name], properties[i].PropertyType)
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 金额大小写转换
        /// </summary>
        /// <typeparam name="T">金额类型</typeparam>
        /// <param name="money">金额</param>
        /// <returns></returns>
        public static string MoneyToChinese<T>(this T money) where T : struct
        {
            var stringMoney = money.ToString();
            try
            {
                return MoneyToChinese(stringMoney);
            }
            catch (Exception)
            {

            }
            return stringMoney;
        }

        /// <summary>金额转大写
        /// 
        /// </summary>
        /// <param name="lowerMoney">金额</param>
        /// <returns></returns>
        public static string MoneyToChinese(this string lowerMoney)
        {
            string functionReturnValue = null;
            bool isNegative = false; // 是否是负数
            if (lowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                lowerMoney = lowerMoney.Trim().Remove(0, 1);
                isNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            lowerMoney = Math.Round(double.Parse(lowerMoney), 2).ToString();
            if (lowerMoney.IndexOf(".") > 0)
            {
                if (lowerMoney.IndexOf(".") == lowerMoney.Length - 2)
                {
                    lowerMoney = lowerMoney + "0";
                }
            }
            else
            {
                lowerMoney = lowerMoney + ".00";
            }
            strLower = lowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;


            return isNegative ? "负" + functionReturnValue : functionReturnValue;
        }

        /// <summary>
        /// 字符串中获取子字符串
        /// </summary>
        /// <param name="stringValue">要从中获取子串的字符串</param>
        /// <param name="start">开始字符串（不包括）</param>
        /// <param name="end">截止字符串（不包括）</param>
        /// <returns>子字符串</returns>
        public static string Substring(this string stringValue, string start, string end)
        {
            if (string.IsNullOrEmpty(stringValue) || string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            {
                return stringValue;
            }

            try
            {
                int startIndex = stringValue.IndexOf(start);
                int endIndex = stringValue.IndexOf(end);

                startIndex = startIndex < 0 ? 0 : startIndex;
                endIndex = endIndex < 0 ? stringValue.Length : endIndex;

                return stringValue.Substring(startIndex + 1, endIndex - startIndex - 1);
            }
            catch (Exception)
            {
            }
            return stringValue;
        }

        /// <summary>
        /// 字符长度（中文占两个）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int UnicodeLength(this string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : Encoding.GetEncoding("GB18030").GetBytes(value).Length;
        }

        /// <summary>
        /// 获取枚举数据值列表
        /// </summary>
        /// <typeparam name="TEnum"></typeparam> 
        /// <returns></returns>
        public static IList<TEnum> GetEnumList<TEnum>() where TEnum : struct
        {
            var list = new List<TEnum>();
            var type = typeof(TEnum);
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                list.Add((TEnum)Enum.Parse(type, name, false));
            }
            return list;
        }

        /// <summary>
        /// 获取一个整形的随机数
        /// </summary>
        /// <param name="minData">最小值</param>
        /// <param name="maxData">最大值</param>
        /// <returns></returns>
        public static int RandomInt(int minData = int.MinValue, int maxData = int.MaxValue)
        {
            return RandomSeed().Next(minData, maxData);
        }

        /// <summary>
        /// 获取一个整形的随机数
        /// </summary> 
        /// <returns></returns>
        public static double RandomDouble(double maxData = double.MaxValue)
        {
            return RandomSeed().NextDouble() * maxData;
        }

        /// <summary>
        /// 随机数种子
        /// </summary>
        /// <returns></returns>
        public static Random RandomSeed()
        {
            return new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32));
        }

        /// <summary>
        /// 获取带位置需要列表，Key为序号，比如1，2,3,4,Value为对应的数据
        /// </summary>
        /// <typeparam name="T">值类型数据</typeparam>
        /// <param name="urlParams"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<int, T>> ToDataIndexList<T>(this IList<T> urlParams)
        {
            var list = new List<KeyValuePair<int, T>>();
            if (!urlParams.IsNullOrEmpty())
            {
                var count = urlParams.Count;
                for (var i = 0; i < count; i++)
                {
                    list.Add(new KeyValuePair<int, T>(i + 1, urlParams[i]));
                }
            }

            return list;
        }

        /// <summary>
        /// 获取带位置需要列表，Key为序号，比如1，2,3,4,Value为对应的数据
        /// </summary>
        /// <typeparam name="T">值类型数据</typeparam>
        /// <param name="urlParams"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<int, T>> ToDataIndexList<T>(this T[] urlParams)
        {
            return urlParams.ToList().ToDataIndexList();
        }
        #endregion

        #region 字符串扩展
        /// <summary>
        /// 取左边N个字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(this string value, int len)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > len)
            {
                value = value.Substring(0, len);
            }

            return value;
        }

        /// <summary>
        /// 取右边N个字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(this string value, int len)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > len)
            {
                value = value.Substring(value.Length - len, len);
            }

            return value;
        }

        /// <summary>
        /// 取指定位置开始的所有字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string Mid(this string value, int startIndex)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Substring(startIndex);
            }

            return value;
        }

        /// <summary>
        /// 取指定位置开始的N个字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mid(this string value, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Substring(startIndex, length);
            }

            return value;
        }

        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReverseValue(this string value)
        {
            return value.IsNullOrEmpty() ? string.Empty : new string(value.Reverse().ToArray());
        }
        #endregion

        #region 字典扩展
        /// <summary>
        /// KeyValue键值对列表转换为字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> list)
        {
            return list.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        ///  获取对象属性及属性值列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="onlyPublic">只获取公开的属性及属性值</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object obj, bool onlyPublic = true)
        {
            if (obj == null)
            {
                return new Dictionary<string, object>();
            }

            return obj.GetType().GetProperties((onlyPublic ? (BindingFlags.Public | BindingFlags.NonPublic) : BindingFlags.Public) | BindingFlags.Instance | BindingFlags.Static).Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(obj, null))).ToDictionary();
        }

        /// <summary>
        /// KeyValue键值对列表转换为字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToList<TKey, TValue>(this IDictionary<TKey, TValue> map)
        {
            return map == null ? new List<KeyValuePair<TKey, TValue>>() : map.Select(c => new KeyValuePair<TKey, TValue>(c.Key, c.Value));
        }

        /// <summary>
        /// 用指定符号将一个字典连接成字符串
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <param name="mapSplit"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string Join<TKey, TValue>(this IDictionary<TKey, TValue> map, string mapSplit = "=>", string split = ",")
        {
            return (map == null || map.Count == 0) ? string.Empty : map.Select(c => c.Key.ToString() + mapSplit + (c.Value == null ? string.Empty : c.Value.ToString())).Join(split);
        }

        /// <summary>
        /// 安全的获取字典中的值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> map, TKey key)
        {
            if (map.ContainsKey(key))
            {
                return map[key];
            }

            return default(TValue);
        }


        /// <summary>
        /// 转换HashTable为IDictionary
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IDictionary<TK, TV> ToDictionary<TK, TV>(this Hashtable table)
        {
            return table.Cast<DictionaryEntry>().ToDictionary(kvp => (TK)kvp.Key, kvp => (TV)kvp.Value);
        }

        /// <summary>
        /// 安全的获取字典值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        #endregion

        #region Json和对象转换
        /// <summary>
        /// 将对象转换为json格式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(this object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// json字符串转为对象
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="o">json字符串</param>
        /// <returns></returns>
        public static T FromJson<T>(this string o)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(o);
        }


        /// <summary>
        /// json字符串转为Object对象
        /// </summary> 
        /// <param name="o">json字符串</param>
        /// <returns></returns>
        public static object FromJson(this string o)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(o);
        }

        /// <summary>
        /// 将一个JSON字符串转换为匿名对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object FromJsonAnonymous(this string o)
        {
            var jObj = JObject.Parse(o);
            var propMap = jObj.Properties().ToDictionary(c => c.Name, c => (object)c.Value.ToString());

            var dynamicObject = new ExpandoObject();
            var dynamicPropMap = (ICollection<KeyValuePair<string, object>>)dynamicObject;

            foreach (var item in propMap)
            {
                dynamicPropMap.Add(item);
            }

            return dynamicObject;
        }

        /// <summary>
        /// 对象转换为json字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">要转换的对象</param>
        /// <returns></returns>
        public static string ToJson<T>(this T o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        #endregion

        #region 字符串截取
        /// <summary>
        ///  截取字符串，默认超过制定的字符个数用制定字符串代替
        /// </summary>
        /// <param name="inputString">原字符串</param>
        /// <param name="cutLength">截取个数</param>
        /// <param name="appendString">用于替换截取后多余的字符串</param>
        /// <returns></returns>
        public static string ToCutString(this string inputString, int cutLength, string appendString)
        {
            if (string.IsNullOrEmpty(inputString) || cutLength <= 0) return inputString;

            int iCount = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(inputString);
            if (iCount > cutLength)
            {
                int iLength = 0;
                for (int i = 0; i < inputString.Length; i++)
                {
                    int iCharLength = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(new char[] { inputString[i] });
                    iLength += iCharLength;
                    if (iLength == cutLength)
                    {
                        inputString = inputString.Substring(0, i + 1);
                        break;
                    }
                    else if (iLength > cutLength)
                    {
                        inputString = inputString.Substring(0, i);
                        break;
                    }
                }
                inputString = inputString + appendString;
            }
            return inputString;
        }

        #endregion

        #region 关键字高亮扩展
        private static readonly string PATTERN_HIGHTLIGHT_KEYWORD = "<{0}{1}{2}>$1</{0}>";
        /// <summary>
        /// 获取关键字匹配正则表达式(空格分隔多个关键字,转义特殊字符)
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string ToKeywordPattern(this string keyword)
        {
            return string.IsNullOrEmpty(keyword) ? keyword : string.Format("({0})", Regex.Replace(Regex.Replace(keyword, "([\\.$^{[(|)*+?\\\\])", "\\$1"), "[\\s|-]+", "|"));
        }

        /// <summary>
        /// 获取高亮显示的字符串文本
        /// </summary>
        /// <param name="text">原字符串文本</param>
        /// <param name="keyword">关键字(空格分隔多个关键字,转义特殊字符)</param>
        /// <param name="htmlTag">用于包裹的Html标签,例如span,b,div等</param>
        /// <param name="htmlClass">包裹的Html标签的Class</param>
        /// <param name="htmlStyle">包裹的Html标签的Style</param>
        /// <param name="isHtmlEncode">是否进行Html编码</param>
        /// <returns></returns>
        public static string ToHightlightText(this string text, string keyword, string htmlTag = "span", string htmlClass = "Hightlight", string htmlStyle = "color:red", bool isHtmlEncode = false)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
            {
                return text;
            }

            if (isHtmlEncode)
            {
                text = HttpUtility.HtmlEncode(text);
                keyword = HttpUtility.HtmlEncode(keyword);
            }

            return Regex.Replace(text, keyword.ToKeywordPattern(), string.Format(PATTERN_HIGHTLIGHT_KEYWORD, htmlTag, string.IsNullOrEmpty(htmlClass) ? string.Empty : string.Format(" class='{0}'", htmlClass), string.IsNullOrEmpty(htmlStyle) ? string.Empty : string.Format(" style='{0}'", htmlStyle)), RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// 获取高亮显示的字符串文本
        /// </summary>
        /// <param name="text">原字符串文本</param>
        /// <param name="keyword">关键字(空格分隔多个关键字,转义特殊字符)</param>
        /// <param name="matchEvaluator">替换回调表达式</param>
        /// <param name="isHtmlEncode">是否精细Html编码</param>
        /// <returns></returns>
        public static string ToHightlightText(this string text, string keyword, MatchEvaluator matchEvaluator, bool isHtmlEncode = false)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
            {
                return text;
            }

            if (isHtmlEncode)
            {
                text = HttpUtility.HtmlEncode(text);
                keyword = HttpUtility.HtmlEncode(keyword);
            }

            return Regex.Replace(text, keyword.ToKeywordPattern(), matchEvaluator, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
        #endregion

        #region 日期
        /// <summary>
        /// 按给定日期格式转换日期字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateValue, string format)
        {
            return dateValue.ToDateString(format, false);
        }

        /// <summary>
        /// 按给定日期格式以及是否要用本地语言设置来转换日期字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <param name="format">日期格式</param>
        /// <param name="useLocalSetting">是否用本地语言设置</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateValue, string format, bool? useLocalSetting)
        {
            return useLocalSetting.GetValueOrDefault() ? dateValue.ToString(format) : dateValue.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 按给定日期格式以及是否要用本地语言设置来转换日期字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <param name="format">日期格式</param>
        /// <param name="dateTimeFormatInfo">地方语言</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateValue, string format, System.Globalization.DateTimeFormatInfo dateTimeFormatInfo)
        {
            return dateValue.ToString(format, dateTimeFormatInfo);
        }
        #endregion

        #region 数据库相关
        public static string ToSqlString(this string oldString)
        {
            return oldString.Replace("'", "''");
        }
        #endregion

        #region IP转换
        /// <summary>
        /// 转换Ip为数字类型
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static long ToIpNumber(this string ipAddress)
        {
            var ipNumber = 0L;

            try
            {
                var ipArray = ipAddress.Split('.').Select(c => int.Parse(c)).ToArray();
                if (ipArray.Count() == 4)
                {
                    ipNumber = (long)ipArray[0] * 256 * 256 * 256 + ipArray[1] * 256 * 256 + ipArray[2] * 256 + ipArray[3];
                }

            }
            catch (Exception)
            {

            }
            return ipNumber;
        }
        #endregion

        #region 字符串转换为值类型
        /// <summary>
        /// 将From或则QueryString传过来的值转换为指定类型的值
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="dataBag">值来源</param>
        /// <param name="key">值的键</param>
        /// <returns></returns>
        public static T TryGet<T>(this NameValueCollection dataBag, string key)
        {
            return dataBag.TryGet(key, default(T));
        }

        /// <summary>
        /// 将From或则QueryString传过来的值转换为指定类型的值
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="dataBag">值来源</param>
        /// <param name="key">值的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T TryGet<T>(this NameValueCollection dataBag, string key, T defaultValue)
        {
            if (dataBag.IsNullOrEmpty() || dataBag[key].IsNullOrEmpty())
            {
                return defaultValue;
            }

            return dataBag[key].ParseTo(defaultValue);
        }

        /// <summary>
        /// 将字符串值转换为指定值
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">要转换的值</param>
        /// <returns></returns>
        public static T ParseTo<T>(this string value)
        {
            return value.ParseTo(default(T));
        }

        /// <summary>
        /// 将字符串值转换为指定值
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ParseTo<T>(this string value, T defaultValue)
        {
            if (Parser<T>.ParseMethod != null)
                try
                {
                    return Parser<T>.ParseMethod(value);
                }
                catch
                {
                    //do nothing 
                }

            return defaultValue;
        }

        /// <summary>
        /// 将一个类型的数据转换为其他类型的数据
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">要转换的数据</param>
        /// <returns></returns>
        public static T ParseTo<T>(this object value)
        {
            return value.ParseTo(default(T));
        }

        /// <summary>
        /// 将一个类型的数据转换为其他类型的数据
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">要转换的数据</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ParseTo<T>(this object value, T defaultValue)
        {
            try
            {
                var convertValue = Convert.ChangeType(value, typeof(T));
                return convertValue.IsNullOrEmpty() ? defaultValue : (T)convertValue;
            }
            catch
            {
                // do nothing
            }
            return defaultValue;
        }

        static CommonHelper()
        {
            Parser<short>.ParseMethod = short.Parse;
            Parser<int>.ParseMethod = int.Parse;
            Parser<long>.ParseMethod = long.Parse;
            Parser<byte>.ParseMethod = byte.Parse;
            Parser<ushort>.ParseMethod = ushort.Parse;
            Parser<uint>.ParseMethod = uint.Parse;
            Parser<ulong>.ParseMethod = ulong.Parse;
            Parser<sbyte>.ParseMethod = sbyte.Parse;
            Parser<float>.ParseMethod = float.Parse;
            Parser<double>.ParseMethod = double.Parse;
            Parser<decimal>.ParseMethod = decimal.Parse;
            Parser<bool>.ParseMethod = bool.Parse;
            Parser<DateTime>.ParseMethod = DateTime.Parse;
            Parser<TimeSpan>.ParseMethod = TimeSpan.Parse;
            Parser<Guid>.ParseMethod = Guid.Parse;
        }

        private class Parser<T>
        {
            public delegate T ParseMethodDelegate(string value);


            private static bool noParseMethod = false;
            private static ParseMethodDelegate _parseMethod;

            public static ParseMethodDelegate ParseMethod
            {
                get
                {

                    if (_parseMethod != null)
                        return _parseMethod;

                    if (noParseMethod)
                        return null;

                    var method = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    if (method != null && (method.Attributes & MethodAttributes.Static) != 0)
                    {
                        var dynamicMethod = new DynamicMethod(typeof(T).FullName + "_Parse", typeof(T), new Type[] { typeof(string) });

                        var il = dynamicMethod.GetILGenerator();
                        il.Emit(OpCodes.Ldarg_0);
                        il.EmitCall(OpCodes.Call, method, null);
                        il.Emit(OpCodes.Ret);

                        return _parseMethod = (Parser<T>.ParseMethodDelegate)dynamicMethod.CreateDelegate(typeof(Parser<T>.ParseMethodDelegate));

                    }

                    noParseMethod = true;
                    return null;
                }
                set
                {
                    _parseMethod = value;
                }
            }
        }
        #endregion

        #region From和QueryString转换
        /// <summary>
        /// Post的Form数据转换为QueryString数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection form)
        {
            return form.ToQueryString(Encoding.UTF8);
        }

        /// <summary>
        /// Post的Form数据转换为QueryString数据
        /// </summary>
        /// <param name="form"></param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection form, Encoding encoding)
        {
            encoding = encoding ?? Encoding.UTF8;
            if (form != null && form.Count > 0)
            {
                var qs = Array.ConvertAll(form.AllKeys, key => string.Format("{0}={1}", key, HttpUtility.UrlEncode(form[key], encoding)));

                return string.Join("&", qs);
            }

            return string.Empty;
        }

        #endregion
    }
}
