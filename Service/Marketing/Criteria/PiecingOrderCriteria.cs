using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.Criteria
{
    public class PiecingOrderCriteria
    {

        /// <summary>
        /// 币种ID
        /// </summary>
        public virtual int? CurrencyId { get; set; }

        /// <summary>
        /// Club等级，非Club时为0
        /// </summary>
        public virtual int ClubLevel { get; set; }

        /// <summary>
        /// 购物车物品总金额(包含正常产品、促销产品、CLUB产品、一口价产品等)、（美元）
        /// </summary>
        public virtual decimal GrandTotal { get; set; }

        /// <summary>
        /// 正价商品的总金额（即没有折扣的商品的总金额）
        /// </summary>
        public virtual decimal NoDiscountProductAmount { get; set; }




        /// <summary>
        /// 国家二级编码
        /// </summary>
        public virtual string CountryIsoCode2 { get; set; }

        /// <summary>
        /// 城市全称
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string PostCode { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        public virtual decimal GrossWeight { get; set; }

        /// <summary>
        /// 体积重
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }

        /// <summary>
        /// 发货重量
        /// </summary>
        public virtual decimal ShippingWeight { get; set; }

        /// <summary>
        /// Club会员发货重量
        /// </summary>
        public virtual decimal ClubWeight { get; set; }

        /// <summary>
        /// 包装箱重量
        /// </summary>
        public virtual decimal PackageWeight { get; set; }
        /// <summary>
        /// 是否渠道商
        /// </summary>
        public virtual bool IsChannel { get; set; }


        /// <summary>
        /// 客户VIP等级 ID
        /// </summary>
        public virtual int VipLevel { get; set; }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int? LanguageId { get; set; }


        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int? CustomerId { get; set; }

        
    }
}
