using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Club
{
    [Class(Table = "v_club_product", Lazy = false, NameType = typeof(ClubProductViewPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ClubProductViewPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 产品Id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言Id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// club会员产品类型
        /// </summary>
        [Property(Column = "club_product_type")]
        public virtual int ClubProductType
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        [Property(Column = "discount")]
        public virtual decimal Discount
        {
            get;
            set;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 截止日期
        /// </summary>
        [Property(Column = "date_ended")]
        public virtual DateTime? DateEnded
        {
            get;
            set;
        }

        /// <summary>
        /// 图片
        /// </summary>
        [Property(Column = "`image`")]
        public virtual string Image
        {
            get;
            set;
        }

        /// <summary>
        /// 原价
        /// </summary>
        [Property(Column = "profit_rate")]
        public virtual decimal ProfitRate { get; set; }
    }
}
