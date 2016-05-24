using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Category
{
    /// <summary>
    /// 类别广告
    /// </summary>
    [Serializable]
    public class CategoryAdvertisement
    {
        /// <summary>
        /// 类别广告图
        /// </summary>
        public virtual string AdvertisingImage
        {
            get;
            set;
        }

        /// <summary>
        /// 类别广告词
        /// </summary>
        public virtual string AdvertisingWords
        {
            get;
            set;
        }

        /// <summary>
        /// Url
        /// </summary>
        public virtual string Url
        {
            get;
            set;
        }

        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 营销标题
        /// </summary>
        public virtual string MarketingTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 产品详情页营销区域
        /// </summary>
        public virtual string ProductMarketingArea 
        { 
            get; 
            set;
        }
    }
}
