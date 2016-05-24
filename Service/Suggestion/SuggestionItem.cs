using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Suggestion
{
    /// <summary>
    /// 评分类型
    /// </summary>
    [Serializable]
    public class SuggestionItem
    {
        /// <summary>
        /// 评分类型Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 建议对象
        /// </summary>
        public virtual List<SuggestionObject> SuggestionObjects { get; set; }
    }
}
