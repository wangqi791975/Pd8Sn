using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Suggestion
{
    /// <summary>
    /// 评分明细
    /// </summary>
    [Serializable]
    public class SuggestionDetail
    {
        /// <summary>
        /// 客户建议Id
        /// </summary>
        public virtual int SuggestionContentId { get; set; }

        /// <summary>
        /// 评分对象Id
        /// </summary>
        public virtual int ObjectId { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public virtual int Score { get; set; }
    }
}
