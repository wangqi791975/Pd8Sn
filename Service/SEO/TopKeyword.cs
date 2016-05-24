using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// 关键词
    /// </summary>
    [Serializable]
    public class TopKeyword
    {
        /// <summary>
        /// 关键词Id
        /// </summary>
        public virtual int TopKeyworId { get; set; }

        /// <summary>
        /// 主题Id
        /// </summary>
        public virtual int TopKeywordSubjectId { get; set; }

        /// <summary>
        /// 关键词名称
        /// </summary>
        public virtual string TopKeywordName { get; set; }
    }
}
