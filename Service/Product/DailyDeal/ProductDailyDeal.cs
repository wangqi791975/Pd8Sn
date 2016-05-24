using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Product.DailyDeal
{
    /// <summary>
    /// 一口价
    /// </summary>
    [Serializable]
    public class ProductDailyDeal
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 产品图片 做成站点虚拟目录，通过FTP上传
        /// </summary>
        public virtual string ProductImage { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime StartDateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndDateTime { get; set; }

        /// <summary>
        /// 已出售数量
        /// </summary>
        public virtual int SaledQuantity { get; set; }

        /// <summary>
        /// 是否生效
        /// </summary>
        public virtual bool IsValid { get; set; }

        /// <summary>
        /// 标语
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 
        /// 标语Id
        /// </summary>
        public virtual int TitleId { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public virtual DateTime? DateUpdate { get; set; }

        #region  扩展属性 用于前台显示
        /// <summary>
        /// 产品Code
        /// </summary>
        public virtual string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        /// 产品英文名称
        /// </summary>
        public virtual string ProductEnName { get; set; }
        #endregion

        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual string LanguageId { get; set; }

        /// <summary>
        /// 一口价原价
        /// </summary>
        public virtual decimal DailyProductPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public virtual decimal Discount { get; set; }

        /// <summary>
        /// 节省
        /// </summary>
        public virtual decimal SaveMoney { get; set; }
    }
}
