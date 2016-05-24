using System;

namespace Com.Panduo.Service.Product.DailyDeal
{
    [Serializable]
    public class DailyDealDesc
    {
        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// dailydeal前台顶部图片
        /// </summary>
        public virtual string HeaderImg { get; set; }

        /// <summary>
        /// dailydeal前台中间
        /// </summary>
        public virtual string MiddleAreaHtml { get; set; }

        /// <summary>
        /// dailydeal前台底部推荐
        /// </summary>
        public virtual string RecommendAreaHtml { get; set; }

        /// <summary>
        /// 该语种dailydeal开关
        /// </summary>
        public virtual bool IsValid { get; set; }
    }
}
