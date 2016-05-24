using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Common
{
    public static class NoExtendHelper
    {
        /// <summary>
        /// 以【前缀 + 当前日期 + 随机数 】返回指定长度单号
        /// </summary>
        /// <param name="length">单号长度</param>
        /// <param name="prefix">单号前缀</param>
        /// <returns>单号</returns>
        public static string GetNoWithCurrentDate(this int length, string prefix)
        {
            prefix = prefix ?? string.Empty;
            var result = prefix + DateTime.Now.ToString("yyyyMMdd");

            return result.Length > length ? result.Substring(0, length) : (result + GetRandomNumberString(length - result.Length));
        }

        /// <summary>
        /// 将指定数字以【前缀 + 指定数字】格式化后返回指定长度的单号
        /// </summary>
        /// <param name="value">要格式化的数字</param>
        /// <param name="length">单号长度</param>
        /// <param name="prefix">单号前缀</param>
        /// <returns>单号</returns>
        public static string GetNoWithData(this int value, int length, string prefix)
        {
            prefix = prefix ?? string.Empty;
            var dataLength = length - prefix.Length;

            var result = dataLength > 0 ? value.ToString().PadLeft(dataLength, '0') : string.Empty;
            return prefix + result;
        }

        /// <summary>
        /// 将指定数字以【前缀 + 指定数字】格式化后返回指定长度的单号
        /// <para>指定数字格式化后只能取1-9、A-Z中除了E、I、0、R的值</para>
        /// </summary>
        /// <param name="value">要格式化的数字</param>
        /// <param name="length">单号长度</param>
        /// <param name="prefix">单号前缀</param>
        /// <returns>单号</returns>
        public static string GetNoWithChinaFormatData(this int value, int length, string prefix)
        {
            prefix = prefix ?? string.Empty;
            var dataLength = length - prefix.Length;

            var result = dataLength > 0 ? GetStandardChinaCode(value,dataLength) : string.Empty;
            return prefix + result;
        }

        /// <summary>
        /// 将指定数字以【前缀 + 指定数字】格式化后返回指定长度的单号
        /// <para>指定数字格式化后只能取1-9、A-Z中除了E、I、0、R的值</para>
        /// </summary>
        /// <param name="value">要格式化的数字</param>
        /// <param name="length">单号长度</param>
        /// <param name="prefix">单号前缀</param>
        /// <returns>单号</returns>
        public static string GetNoWithFormatData(this int value, int length, string prefix)
        {
            prefix = prefix ?? string.Empty;
            var dataLength = length - prefix.Length;

            var result = dataLength > 0 ? GetStandardCode(value, dataLength) : string.Empty;
            return prefix + result;
        }

        /// <summary>
        /// 格式化指定的整数，得到的编码只能从1-9、A-Z中除了E、I、0、R中取，原因是E、R与1,、2同音，I、O与1、0容易混淆
        /// </summary>
        /// <param name="value">要格式化的数字</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetStandardChinaCode(this int value, int length)
        {
            var result = string.Empty;

            var chars = "1,2,3,4,5,6,7,8,9,A,B,C,D,F,G,H,J,K,L,M,N,P,Q,S,T,U,V,W,X,Y,Z".Split(',');
            while (value != 0)
            {
                result = chars[value % 31] + result;
                value /= 31;
            }

            result = result.Length > length ? result.Substring(0, length) : result.PadLeft(length, '0');
            return result;
        } 

        /// <summary>
        /// 格式化指定的整数，得到的编码只能从1-9、A-Z中除了0中取，原因是O与0容易混淆
        /// </summary>
        /// <param name="value">要格式化的数字</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetStandardCode(this int value, int length)
        {
            var result = string.Empty;

            var chars = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
            while (value != 0)
            {
                result = chars[value % 34] + result;
                value /= 34;
            }

            result = result.Length > length ? result.Substring(0, length) : result.PadLeft(length, '0');
            return result;
        }

        /// <summary>
        /// 格式化指定的整数，得到的编码只能从1-9中中取
        /// </summary>
        /// <param name="value">要格式化的数字</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetStandardNumberCode(this int value, int length)
        {
            var result = string.Empty;

            var chars = "1,2,3,4,5,6,7,8,9".Split(',');
            while (value != 0)
            {
                result = chars[value % 9] + result;
                value /= 9;
            }

            result = result.Length > length ? result.Substring(0, length) : result.PadLeft(length, '0');
            return result;
        }

        /// <summary>
        /// 获取指定长度的随机数字字符串
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns>指定长度的随机数字</returns>
        public static string GetRandomNumberString(this int length)
        {
            var numberLetters = "0,1,2,3,4,5,6,7,8,9".Split(',');
            var numberBuilder = new StringBuilder();
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < length; i++)
            {
                numberBuilder.Append(numberLetters[random.Next(0, 9)]);
            }
            return numberBuilder.ToString();
        }

        /// <summary>
        /// 获取指定长度的随机大写字母字符串
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns>指定长度的随机数字</returns>
        public static string GetRandomLetterString(this int length)
        {
            var numberLetters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
            var numberBuilder = new StringBuilder();
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < length; i++)
            {
                numberBuilder.Append(numberLetters[random.Next(0, 24)]);
            }
            return numberBuilder.ToString();
        }

        /// <summary>
        /// 获取指定长度的随机字符串（数字和字母）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(this int length)
        {
            var numberLetters = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
            var numberBuilder = new StringBuilder();
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < length; i++)
            {
                numberBuilder.Append(numberLetters[random.Next(0, 34)]);
            }
            return numberBuilder.ToString();
        }
    }
}
