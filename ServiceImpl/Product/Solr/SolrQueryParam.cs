using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Service.Product;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// 商品Solr查询参数
    /// </summary>
    [Serializable]
    public class SolrQueryParam
    {
        /// <summary>
        /// 商品搜索区域
        /// </summary>
        public ProductSearchAreaType AreaType { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public virtual string Keyword { get; set; }
        /// <summary>
        /// 商品Id精确匹配(OR关系)
        /// </summary>
        public virtual ICollection<int> ProductIds { get; set; }
        /// <summary>
        /// 排除的商品ID
        /// </summary>
        public virtual ICollection<int> IgnoreProductIds { get; set; }
        /// <summary>
        /// 商品Sku(OR关系)
        /// </summary>
        public virtual ICollection<string> Skus { get; set; }
        /// <summary>
        /// 上架开始时间
        /// </summary>
        public DateTime? JoinDateFrom { get; set; }
        /// <summary>
        /// 上架截止时间
        /// </summary>
        public DateTime? JoinDateTo { get; set; }
        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? CreateDateFrom { get; set; }
        /// <summary>
        /// 创建截止时间
        /// </summary>
        public DateTime? CreateDateTo { get; set; }
        /// <summary>
        /// 最低售价开始金额(包含),美元金额
        /// </summary>
        public decimal? SalePriceMinFrom;
        /// <summary>
        /// 最低售价截止金额(包含),美元金额
        /// </summary>
        public decimal? SalePriceMinTo;

        /// <summary>
        /// 网站是否开启促销
        /// </summary>
        public bool IsPromotionOn { get; set; }
        /// <summary>
        /// 促销类型
        /// </summary>
        public virtual int? PromotionId { get; set; }
        /// <summary>
        /// 促销折扣开始
        /// </summary>
        public virtual decimal? PromotionDiscountFrom { get; set; }
        /// <summary>
        /// 促销折扣开始
        /// </summary>
        public virtual decimal? PromotionDiscountTo { get; set; }

        /// <summary>
        /// 产品专区
        /// </summary>
        public virtual int? ProductAreaId { get; set; }
        /// <summary>
        /// 产品匹配产品
        /// </summary>
        public virtual int? ProductMatchId { get; set; }
        
        /// <summary>
        /// 是否库存商品
        /// </summary>
        public virtual bool? IsInStock { get; set; }
        /// <summary>
        /// 是否热销商品
        /// </summary>
        public virtual bool? IsBestSeller { get; set; } 
        /// <summary>
        /// 是否正常销售商品
        /// </summary>
        public virtual bool? IsOnSale { get; set; }  
        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int? CategoryId { get; set; }
        /// <summary>
        /// 类别路径ID(对应类别、上级类别、根类别都会搜索)
        /// </summary>
        public virtual int? CategoryPath { get; set; }
        /// <summary>
        /// 属性值ID(And)关系
        /// </summary>
        public virtual ICollection<int> PropertyValueIds { get; set; }
        /// <summary>
        /// 属性值ID(Or)关系
        /// </summary>
        public virtual ICollection<int> OrPropertyValueIds { get; set; } 
        /// <summary>
        /// 忽略的属性值ID
        /// </summary>
        public virtual ICollection<int> IgnorePropertyValueIds { get; set; }
        /// <summary>
        /// 属性值组ID(And)关系
        /// </summary>
        public virtual ICollection<int> PropertyValueGroupIds { get; set; }
        /// <summary>
        /// 属性值组ID(Or)关系
        /// </summary>
        public virtual ICollection<int> OrPropertyValueGroupIds { get; set; } 
        /// <summary>
        /// 忽略的属性值组ID
        /// </summary>
        public virtual ICollection<int> IgnorePropertyValueGroupIds { get; set; }
        /// <summary>
        /// 属性值和属性值组过滤,(属性之间为And关系,属性内部的属性值和属性值组之间OR的关系)
        /// </summary>
        public virtual IDictionary<int, IList<KeyValuePair<SolrPropertyType,int>>> FiterPropertyValueAndGroupIds { get; set; }  

        /// <summary>
        /// 是否统计属性值
        /// </summary>
        public bool IsStatisticsPropertyValue;
        /// <summary>
        /// 是否统计类别
        /// </summary>
        public bool IsStatisticsCategory; 

        /// <summary>
        /// 排序
        /// </summary>
        public virtual ICollection<ProductSorterCriteria> Sorts { get; set; }

        /// <summary>
        /// 克隆参数
        /// </summary>
        /// <returns></returns>
        public virtual SolrQueryParam Clone()
        {
            //浅拷贝值类型
            var param = this.MemberwiseClone() as SolrQueryParam;

            if (param != null)
            {
                //应用类型数据需要自行复制
                param.ProductIds = new List<int>(this.ProductIds ?? new List<int>());
                param.IgnoreProductIds = new List<int>(this.IgnoreProductIds??new List<int>());
                param.Skus = new List<string>(this.Skus ?? new List<string>());
                param.PropertyValueIds = new List<int>(this.PropertyValueIds ?? new List<int>());
                param.OrPropertyValueIds = new List<int>(this.OrPropertyValueIds ?? new List<int>());
                param.IgnorePropertyValueIds = new List<int>(this.IgnorePropertyValueIds ?? new List<int>());
                param.PropertyValueGroupIds = new List<int>(this.PropertyValueGroupIds ?? new List<int>());
                param.OrPropertyValueGroupIds = new List<int>(this.OrPropertyValueGroupIds ?? new List<int>());
                param.IgnorePropertyValueGroupIds = new List<int>(this.IgnorePropertyValueGroupIds ?? new List<int>()); 
                if (!this.FiterPropertyValueAndGroupIds.IsNullOrEmpty())
                {
                    param.FiterPropertyValueAndGroupIds = new Dictionary<int, IList<KeyValuePair<SolrPropertyType, int>>>();

                    foreach (var item in this.FiterPropertyValueAndGroupIds)
                    {
                        param.FiterPropertyValueAndGroupIds.Add(item.Key, item.Value.Select(c => new KeyValuePair<SolrPropertyType, int>(c.Key, c.Value)).ToList());
                    }
                }
                
                param.Sorts = new List<ProductSorterCriteria>(this.Sorts ?? new List<ProductSorterCriteria>());

            }

            return param;
        }
    }

     public enum SolrPropertyType
    {
        /// <summary>
        /// 属性值
        /// </summary>
        PropertyValue,
        /// <summary>
        /// 属性值组
        /// </summary>
        PropertyValueGroup
    }

    /// <summary>
    /// Solr搜索引擎
    /// </summary>
    public enum SolrEngine
    {
        /// <summary>
        /// 标准
        /// </summary>
        Standard,
        /// <summary>
        /// dismax
        /// </summary>
        DisMax,
        /// <summary>
        /// edismax
        /// </summary>
        EDisMax
    }
}
