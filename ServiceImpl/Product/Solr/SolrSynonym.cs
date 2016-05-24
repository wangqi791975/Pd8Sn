using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// 同义词类
    /// </summary>
    public static class SolrSynonym
    {
        private static IDictionary<string, string[]> _synonymsDictionary;

        static SolrSynonym()
        {
            Reload();
        }

        private static string GetSynonymsFile()
        {
            string file = SolrConfigurer.SolrSettings["Synonyms"];
            if (string.IsNullOrEmpty(file)) return string.Empty;
            if (file.StartsWith("~/"))
            {
                return AppDomain.CurrentDomain.BaseDirectory + file.Substring(2);
            }
            return file;
        }

        /// <summary>
        /// 重新加载同义词字典
        /// </summary>
        public static void Reload()
        {
            _synonymsDictionary = InitSynonymsDictionary(GetSynonymsFile());
        }

        /// <summary>
        /// 获取同义词(全匹配)
        /// </summary>
        public static string[] GetSynonymsByMatching(string s)
        {
            if (_synonymsDictionary == null || _synonymsDictionary.Count == 0)
            {
                return null;
            }
            s = s.ToLower();
            if (!_synonymsDictionary.ContainsKey(s)) return null;
            return _synonymsDictionary[s];
        }

        /// <summary>
        /// 获取同义词(逐字)
        /// </summary>
        public static string[] GetSynonymsByVerbatim(string s)
        {
            if (_synonymsDictionary == null || _synonymsDictionary.Count == 0)
            {
                return null;
            }
            s = s.ToLower();
            if (_synonymsDictionary.ContainsKey(s)) //如果有同义词
            {
                return _synonymsDictionary[s];
            }

            const string separator = " "; //单词分隔符
            if (!s.Contains(separator))//如果关键字只是一个单词
            {
                return null;
            }

            // 关键字为多个单词

            string sTemp = separator + s + separator;

            List<string> keywordList = new List<string>();
            string keyTemp;
            foreach (string key in _synonymsDictionary.Keys)
            {
                keyTemp = separator + key + separator; ;
                if (sTemp.Contains(keyTemp))
                {
                    foreach (string synonym in _synonymsDictionary[key])
                    {
                        keywordList.Add(sTemp.Replace(keyTemp, separator + synonym + separator).Trim());
                    }
                }
            }
            if (keywordList.Count == 0) return null;
            return keywordList.ToArray();
        }



        /// <summary>
        /// 初始化同义词字词
        /// </summary>
        internal static IDictionary<string, string[]> InitSynonymsDictionary(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            IDictionary<string, string[]> synonymsDic = new Dictionary<string, string[]>();
            string[] datas = File.ReadAllLines(filePath);
            if (datas == null || datas.Length == 0)
            {
                return synonymsDic;
            }
            string str;
            string[] tempArr;
            string key;
            try
            {
                foreach (string d in datas)
                {
                    str = d.Trim();
                    if (str.Length == 0 || str.StartsWith("#"))
                    {
                        continue;
                    }
                    tempArr = str.Trim(',').Split(',');
                    if (tempArr.Length < 2)
                    {
                        continue;
                    }
                    foreach (var s in tempArr)
                    {
                        key = s.ToLower();
                        if (synonymsDic.ContainsKey(key))
                        {
                            synonymsDic[key] = MergeArray(synonymsDic[key], tempArr, key);
                        }
                        else
                        {
                            synonymsDic.Add(key, IgnoreItem(tempArr, key));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to initialize the synonyms configuration of Solr.\n" + ex);
            }

            return synonymsDic;
        }

        /// <summary>
        /// 合并数组
        /// </summary>
        private static string[] MergeArray(string[] sourceArr, string[] appendArr, string ignore)
        {
            List<string> list = new List<string>(sourceArr);
            foreach (var aStr in appendArr)
            {
                if (aStr.Equals(ignore)) continue;
                if (list.Contains(aStr)) continue;
                list.Add(aStr);
            }
            return list.ToArray();
        }

        private static string[] IgnoreItem(string[] arr, string ignore)
        {
            List<string> temp = new List<string>();
            foreach (string s in arr)
            {
                if (!s.Equals(ignore, StringComparison.OrdinalIgnoreCase))
                {
                    temp.Add(s);
                }
            }
            return temp.ToArray();
        }
    }
}
