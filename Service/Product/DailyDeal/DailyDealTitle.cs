using System;

namespace Com.Panduo.Service.Product.DailyDeal
{
    /// <summary>
    /// 一口价标语
    /// </summary>
    [Serializable]
    public class DailyDealTitle
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { get; set; }
        
        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }
        
        /// <summary>
        /// 标语
        /// </summary>
        public virtual string Name { get; set; }
    }
}
