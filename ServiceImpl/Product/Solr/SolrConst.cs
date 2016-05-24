using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// Solr关键字
    /// </summary>
    public static class SolrConst
    {

        /// <summary>
        /// solr查询字符串最大长度.查询字符串长度过长将导致程序错误
        /// </summary>
        public const int MaxQueryStringLength = 3500;
        public const string True = "1";
        public const string False = "0";

        public const string Bf = "bf";
        public const string Qf = "qf";
        public const string DefType = "defType";
    }
}
