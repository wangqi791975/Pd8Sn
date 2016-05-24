using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Com.Panduo.Common;
using SolrNet.Utils;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// Solr扩展
    /// </summary>
    public static class SolrExtendHelper
    { 
        private const string HighligthTag = "<span style='color: #ff0000'>$1</span>";
        
        /// <summary>
        /// 关键字转义
        /// </summary>
        public static string KeywordEncode(string keyword)
        {
            return Regex.Replace(keyword, "([\"*~])", "\\$1");
        }

        /// <summary>
        /// 把多个空格替换成一个空格
        /// </summary>
        public static string ReplaceBlanks(string str)
        {
            return Regex.Replace(str, "\\s{2,}", " ").Trim();
        }

        /// <summary>
        ///过滤搜索关键字 
        /// </summary>
        public static string FilterKeyword(string keyword)
        {
            //keyword = keyword.Replace("'", " ");  
            //keyword = keyword.Replace("-", " ");
            keyword = Regex.Replace(keyword, "(^-)|(-$)", ""); //把开头或结尾的"-"去掉
            keyword = Regex.Replace(keyword, "\\s+-", " ");  //
            keyword = Regex.Replace(keyword, "(})|(])|([,:+!|\\s\\u005E\\u0028\\u0029\\u005B\\u003F\\u005C\\u007B]+)", " ");
            keyword = Regex.Replace(keyword, "(<\\S+\\s*/?>)|(</\\s*\\S+\\s*>)", " ", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            keyword = Regex.Replace(keyword, "\\s{2,}", " "); //把多个空格替换成一个空格 
            return keyword.Trim();
        }

        /// <summary>
        /// 高亮显示搜索关键字
        /// </summary>
        public static string HighlightKeyword(string displayText, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return displayText;
            }
            return Regex.Replace(displayText, GetKeywordRegexPattern(keyword), HighligthTag, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// 高亮显示搜索关键字
        /// </summary>
        public static string HighlightKeyword(string displayText, string keyword, bool isHtmlEncode)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return displayText;
            }
            if (isHtmlEncode)
            {
                displayText = HttpUtility.HtmlEncode(displayText);
                keyword = HttpUtility.HtmlEncode(keyword);
            }
            return Regex.Replace(displayText, GetKeywordRegexPattern(keyword), HighligthTag, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// 高亮显示搜索关键字
        /// </summary>
        /// <param name="displayText"></param>
        /// <param name="keywordRegexPattern">关键字正则表达式</param>
        public static string HighlightKeywordByRegex(string displayText, string keywordRegexPattern)
        {
            if (string.IsNullOrEmpty(keywordRegexPattern))
            {
                return displayText;
            }
            return Regex.Replace(displayText, keywordRegexPattern, HighligthTag, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// 高亮显示搜索关键字
        /// </summary>
        /// <param name="displayText"></param>
        /// <param name="keywordRegexPattern">关键字正则表达式</param>
        /// <param name="isHtmlEncode"></param>
        public static string HighlightKeywordByRegex(string displayText, string keywordRegexPattern, bool isHtmlEncode)
        {
            if (string.IsNullOrEmpty(keywordRegexPattern))
            {
                return displayText;
            }
            if (isHtmlEncode)
            {
                displayText = HttpUtility.HtmlEncode(displayText);
            }
            return Regex.Replace(displayText, keywordRegexPattern, HighligthTag, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// 获取关键字正则表达式
        /// </summary>
        public static string GetKeywordRegexPattern(string keyword)
        {
            return string.Concat("(", Regex.Replace(RegexEscape(keyword), "\\s+", "|"), ")");
        }

        /// <summary>
        /// 获取关键字正则表达式
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="isRegexEscape">正则表达式特别符号是否转义</param>
        public static string GetKeywordRegexPattern(string keyword, bool isRegexEscape)
        {
            return string.Concat("(", Regex.Replace(isRegexEscape ? RegexEscape(keyword) : keyword, "\\s+", "|"), ")");
        }

        private static string RegexEscape(string s)
        {
            return Regex.Replace(s, "([\\.$^{[(|)*+?\\\\])", "\\$1");
        }


        #region 关键字转换,eg 1x2x3 => 1x3x2 2x1x3 2x3x1 3x1x2 3x2x1 
        /// <summary>
        /// 递归算法求排列(私有成员)
        /// </summary>
        /// <param name="list">返回的列表</param>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        private static void GetPermutation<T>(ref List<T[]> list, T[] t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<T[]>();
                }
                T[] temp = new T[t.Length];
                t.CopyTo(temp, 0);
                list.Add(temp);
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(ref t[startIndex], ref t[i]);
                    GetPermutation(ref list, t, startIndex + 1, endIndex);
                    Swap(ref t[startIndex], ref t[i]);
                }
            }
        }

        /// <summary>
        /// 求从起始标号到结束标号的排列，其余元素不变
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        /// <returns>从起始标号到结束标号排列的范型</returns>
        public static List<T[]> GetPermutation<T>(this T[] t, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex > t.Length - 1)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            GetPermutation(ref list, t, startIndex, endIndex);
            return list;
        }

        /// <summary>
        /// 返回数组所有元素的全排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <returns>全排列的范型</returns>
        public static List<T[]> GetPermutation<T>(this T[] t)
        {
            return GetPermutation(t, 0, t.Length - 1);
        }

        /// <summary>
        /// 交换两个变量
        /// </summary>
        /// <param name="a">变量1</param>
        /// <param name="b">变量2</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// 获取数字组合关键字衍化出来的关键字列表,最多只衍化三组数字
        /// <para>比如1x2x3衍化出:1x2x3,1x3x2,2x1x3,2x3x1,3x1x2,3x2x1</para> 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static IList<string> GetSolrNumberKeywords(this string keyword)
        {
            IList<string> result = null;

            var keywordMatches = Regex.Matches(keyword, @"(?<first>\d+(?:\.\d+)?)[x|*](?<second>\d+(?:\.\d+)?)(?:[x|*](?<third>\d+(?:\.\d+)?))?", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            if (keywordMatches.Count > 0)
            {
                var oldValue = keywordMatches[0].Groups[0].Value;
                decimal[] datas;
                if (keywordMatches[0].Groups["third"].Success)
                {
                    datas = new[] { 
                        decimal.Parse(keywordMatches[0].Groups["first"].Value),
                        decimal.Parse(keywordMatches[0].Groups["second"].Value),
                        decimal.Parse(keywordMatches[0].Groups["third"].Value)
                    };
                }
                else
                {
                    datas = new[] { 
                        decimal.Parse(keywordMatches[0].Groups["first"].Value),
                        decimal.Parse(keywordMatches[0].Groups["second"].Value)
                    };
                }

                result = datas.GetPermutation().Select(c => c.Join("x")).Distinct().Select(c => keyword.Replace(oldValue, c)).ToList();
            }

            result = result.IsNullOrEmpty() ? new List<string> { keyword } : result;

            return result;
        } 
        #endregion 
    }
}
