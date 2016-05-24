using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 邮件模板辅助类
    /// </summary>
    public static class MailTemplateHelper
    {

        public static readonly String REGEX_TAG_HTML_TITLE="<!--<tilte>(.*)</tilte>-->";

        /// <summary>
        /// 替换模板标记
        /// </summary>
        /// <param name="templete">要替换的模板</param>
        /// <param name="replaceTagMap">替换标记,Key为标记,Value为要替换的值</param>
        /// <returns></returns>
        public static string ReplaceTemplateTag(string templete, IDictionary<string, string> replaceTagMap)
        {
            if (!string.IsNullOrEmpty(templete) && replaceTagMap != null && replaceTagMap.Count > 0)
            {
                foreach (var item in replaceTagMap)
                {
                    templete = templete.Replace(item.Key, item.Value);
                }
            }

            return templete;
        }

        /// <summary>
        /// 获取模板中指定标签正则表达式指定标签的
        /// </summary>
        /// <param name="templete">模板</param>
        /// <param name="tag">标签正则表达式,注意保证获取的值所在组为1</param>
        /// <returns></returns>
        public static string GetTagValue(string templete, string tag)
        {
            var value = string.Empty;
            if (!string.IsNullOrEmpty(templete))
            {
                var match = Regex.Match(templete, tag, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (match.Groups.Count > 0)
                {
                    value = match.Groups[1].Value;
                }
            }
            return value;
        }

        /// <summary>
        /// 删除模板中指定标签正则表达式指定标签的内容
        /// </summary>
        /// <param name="templete">模板</param>
        /// <param name="tag">标签正则表达式,注意保证获取的值所在组为1</param>
        /// <returns></returns>
        public static string RemoveTag(string templete, string tag)
        {
            var value = string.Empty;
            if (!string.IsNullOrEmpty(templete))
            {
                value = ReplaceTagValue(templete, tag, string.Empty);
            }
            return value;
        }

        /// <summary>
        /// 删除模板中指定标签正则表达式指定标签的内容
        /// </summary>
        /// <param name="templete">模板</param>
        /// <param name="tag">标签正则表达式</param>
        /// <param name="replaceValue">替换值</param>
        /// <returns></returns>
        public static string ReplaceTagValue(string templete, string tag, string replaceValue)
        {
            var value = string.Empty;
            if (!string.IsNullOrEmpty(templete))
            {
                value = Regex.Replace(templete, tag, replaceValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }
            return value;
        }
    }
}
