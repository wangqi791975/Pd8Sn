using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.SEO.TopKeyword
{
    public class TopKeywordVo : BaseViewModel
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
        /// 主题名称
        /// </summary>
        public virtual string TopKeywordSubjectName { get; set; }

        /// <summary>
        /// 关键词名称
        /// </summary>
        public virtual string TopKeywordName { get; set; }
    }
}