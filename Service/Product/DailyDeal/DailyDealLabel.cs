namespace Com.Panduo.Service.Product.DailyDeal
{
    public class DailyDealLabel
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 一口价Id
        /// </summary>
        public virtual int DailyDealId { get; set; }

        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string LabelName { get; set; }
    }
}