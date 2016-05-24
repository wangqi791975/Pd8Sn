using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Suggestion
{
    /// <summary>
    /// 评分对象
    /// </summary>
    [Serializable]
   public class SuggestionObject
    {
       /// <summary>
       /// 评分对象Id
       /// </summary>
       public virtual int Id { get; set; }

       /// <summary>
       /// 评分类型Id
       /// </summary>
       public virtual int ItemId { get; set; }

       /// <summary>
       /// 评分对象名称
       /// </summary>
       public virtual string Name { get; set; }

       /// <summary>
       /// 满分分值
       /// </summary>
       public virtual int TotalScore { get; set; }

       /// <summary>
       /// 排序
       /// </summary>
       public virtual int Sort { get; set; }
    }
}
