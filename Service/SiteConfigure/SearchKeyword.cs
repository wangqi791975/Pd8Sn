using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 搜索关键词
    /// </summary>
    public class SearchKeyword
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string KeywordName { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string KeywordUrl { get; set; }


        /// <summary>
        /// 类型（搜索框内或框外或底部）
        /// </summary>
        public KeywordType KeywordType { get; set; }


        /// <summary>
        /// 语种Id
        /// </summary>
        public int LanguageId { get; set; }


    }

    public enum KeywordType {
        /// <summary>
        /// 10搜索框内
        /// </summary>
        InBox = 10,
        /// <summary>
        /// 20搜索框下
        /// </summary>
        UnderBox = 20,
        /// <summary>
        /// 30底部
        /// </summary>
        Bottom = 30,
    }
}
