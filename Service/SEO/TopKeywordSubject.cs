using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// 主题多语种
    /// </summary>
    [Serializable]
    public class TopKeywordSubject
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
        public virtual int LanguageName { get; set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        public virtual string TopKeywordSubjectName { get; set; }
    }
}
