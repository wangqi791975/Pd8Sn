using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.SEO.TopKeyword
{
    public class TopKeywordSubjectVo : BaseViewModel
    {
        /// <summary>
        /// 主题Id
        /// </summary>
        public virtual int TopKeywordSubjectId { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }
        /// <summary>
        /// 语种名称
        /// </summary>
        public virtual string LanguageName { get; set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        public virtual string TopKeywordSubjectName { get; set; }
    }
}