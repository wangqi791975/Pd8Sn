using System;
using System.Text;


namespace Com.Panduo.Web.Common.Mvc.Routing
{
    /// <summary>
    ///   URL重写辅助类
    /// </summary>
    public static class RewriteUrlHelper
    {
        private const char Minus = '-';

        /// <summary>
        ///   把输入的单词拆分成用 - 连接的小写形式
        /// </summary>
        /// <param name="input"> Pascal形式的字符串 </param>
        /// <returns> 用 - 连接的小写形式 </returns>
        public static string Spliter(string input)
        {
            var builder = new StringBuilder();
            var index = 0;
            foreach (var str in input)
            {
                if (str >= 'A' && str <= 'Z')
                {
                    if (index > 0)
                    {
                        builder.Append(Minus);
                    }
                    builder.Append(Char.ToLower(str));
                }
                else if (str == Minus)
                {
                    builder.Append(Minus);
                    builder.Append(Minus);
                }
                else
                {
                    builder.Append(str);
                }
                index++;
            }
            return builder.ToString();
        }

        /// <summary>
        ///   把用 - 连接的小写单词还原为Pascal形式
        /// </summary>
        /// <param name="input"> 以 - 连接的小写单词 字符串 </param>
        /// <returns> 单词的 Pascal 形式 </returns>
        public static string Restore(string input)
        {
            var builder = new StringBuilder();
            var iterator = input.GetEnumerator();
            var index = 0;
            while (iterator.MoveNext())
            {
                if (iterator.Current != Minus)
                {
                    var c = iterator.Current;
                    if (index == 0)
                    {
                        c = char.ToUpper(c);
                    }
                    builder.Append(c);
                    index++;
                    continue;
                }
                if (!iterator.MoveNext())
                {
                    builder.Append(Minus);
                    break;
                }
                if (iterator.Current == Minus)
                {
                    builder.Append(Minus);
                }
                else
                {
                    var c = iterator.Current;
                    c = char.ToUpper(c);
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
    }
}