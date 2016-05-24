using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    public class KeywordRedirect
    {
        private static readonly string KeywordRedirectPath = GetFileAbsolutePath(ConfigurationManager.AppSettings["Keyword.Redirect"]);

        private static string GetFileAbsolutePath(string relativePath)
        {
            if (relativePath.IsNullOrEmpty()) return string.Empty;
            return HttpContext.Current.Server.MapPath(relativePath);
        }

        private static KeywordRedirect _instance;

        private KeywordRedirect()
        {
            ReloadConfig();
        }

        public static KeywordRedirect Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(KeywordRedirect))
                    {
                        if (_instance == null)
                        {
                            _instance = new KeywordRedirect();
                        }
                    }
                }
                return _instance;
            }
        }

        private IDictionary<string, string> _setting;
        private bool _hasSettings;

        public void ReloadConfig()
        {
            _setting = new Dictionary<string, string>();
            _hasSettings = false;
            if (!File.Exists(KeywordRedirectPath)) return;
            try
            {
                string[] contents = File.ReadAllLines(KeywordRedirectPath, Encoding.UTF8);
                if (contents.IsNullOrEmpty()) return;
                string s;
                int index;
                foreach (var line in contents)
                {
                    s = line.Trim();
                    if (s.StartsWith("#") || s.Length == 0) continue;
                    index = s.IndexOf("=>");
                    if (index < 1 || index == (s.Length - 2)) continue;
                    _setting.Add(s.Substring(0, index).Trim().ToLower(), s.Substring(index + 2).Trim());
                }
                _hasSettings = _setting.Count > 0;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Load KeywordRedirect config error.", ex);
            }
        }

        /// <summary>
        /// 跳转
        /// </summary>
        public bool Redirect(string keyword)
        {
            if (_hasSettings)
            {
                string url;
                if (_setting.TryGetValue(keyword.ToLower(), out url))
                {
                    HttpContext.Current.Response.Redirect(url, true);
                    return true;
                }
            }
            return false;
        }
    }
}
