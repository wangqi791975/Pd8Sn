using System;

namespace Com.Panduo.Service.Product.ClubProduct
{
    public class ClubProductView
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// club会员产品类型
        /// </summary>
        public virtual int ClubProductType { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public virtual decimal Discount { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public virtual DateTime? DateEnded { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public virtual string Image { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public virtual decimal ProfitRate { get; set; }
    }
}