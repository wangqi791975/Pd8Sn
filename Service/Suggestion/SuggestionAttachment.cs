using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Suggestion
{
    /// <summary>
    /// 客户建议附件
    /// </summary>
    [Serializable]
    public class SuggestionAttachment
    {
        /// <summary>
        /// 客户建议Id
        /// </summary>
        public virtual int SuggestionContentId { get; set; }

        /// <summary>
        /// 附件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public virtual string Path { get; set; }
    }
}
