using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;
using SolrNet.Utils;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// 产品Solr实体映射
    /// </summary>
    [Serializable]
    public class ProductSolrInfo
    {
        /// <summary>
        /// Solr的ID
        /// </summary>
        [SolrUniqueKey(ProductSolrField.Id)]
        public string Id { get; set; }
        /// <summary>
        /// 产品自增ID
        /// </summary>
        [SolrField(ProductSolrField.ProductId)]
        public int ProductId { get; set; }
        /// <summary>
        /// 产品编号，比如B00001
        /// </summary>
        [SolrField(ProductSolrField.ProductCode)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品Sku，比如B00001,但是具有like匹配功能
        /// </summary> 
        [SolrField(ProductSolrField.Sku)]
        public string Sku { get; set; }

        /// <summary>
        /// 产品重量(单位:g)
        /// </summary>
        [SolrField(ProductSolrField.ProductWeight)]
        public decimal ProductWeight { get; set; }
        /// <summary>
        /// 体积重量(单位:g)
        /// </summary> 
        [SolrField(ProductSolrField.VolumeWeight)]
        public decimal VolumeWeight { get; set; }
        /// <summary>
        /// 产品上架时间
        /// </summary> 
        [SolrField(ProductSolrField.JoinDate)]
        public DateTime JoinDate { get; set; } 
        /// <summary>
        /// 产品创建时间
        /// </summary> 
        [SolrField(ProductSolrField.CreateDate)]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 产品最后修改时间
        /// </summary> 
        [SolrField(ProductSolrField.LastModifyDate)]
        public DateTime LastModifyDate { get; set; }
        /// <summary>
        /// 产品状态
        /// </summary> 
        [SolrField(ProductSolrField.ProductStatus)]
        public int ProductStatus { get; set; }
        /// <summary>
        /// 最小起订量
        /// </summary> 
        [SolrField(ProductSolrField.MinOrderQty)]
        public int MinOrderQty { get; set; }
        /// <summary>
        /// 单位
        /// </summary> 
        [SolrField(ProductSolrField.ProductUnit)]
        public string ProductUnit { get; set; }
        /// <summary>
        /// 每组数量
        /// </summary> 
        [SolrField(ProductSolrField.PackageQty)]
        public int PackageQty { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary> 
        [SolrField(ProductSolrField.ProductName)]
        public string ProductName { get; set; }
        /// <summary>
        /// 营销标题
        /// </summary> 
        [SolrField(ProductSolrField.MarketingTitle)]
        public string MarketingTitle { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary> 
        [SolrField(ProductSolrField.ProductDesc)]
        public string ProductDesc { get; set; }
        /// <summary>
        /// 是否混装
        /// </summary> 
        [SolrField(ProductSolrField.IsMixed)]
        public bool IsMixed { get; set; }
        /// <summary>
        /// 是否次品
        /// </summary> 
        [SolrField(ProductSolrField.IsDefective)]
        public bool IsDefective { get; set; }

        /// <summary>
        /// 美元基本成本价 = 人民币成本价 *　固定汇率
        /// </summary> 
        [SolrField(ProductSolrField.ProductPrice)]
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 美元最终成本价　=　美元基本成本价 + 上下浮动价
        /// </summary> 
        [SolrField(ProductSolrField.FinalPrice)]
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 美元最低售价 = 美元最终成本价 * 最低利润系数
        /// </summary> 
        [SolrField(ProductSolrField.SalePriceMin)]
        public decimal SalePriceMin { get; set; }
        /// <summary>
        /// 美元最搞售价 = 美元最终成本价 * 最搞利润系数
        /// </summary> 
        [SolrField(ProductSolrField.SalePriceMax)]
        public decimal SalePriceMax { get; set; }

        /// <summary>
        /// 是否新品
        /// </summary> 
        [SolrField(ProductSolrField.IsNewArrival)]
        public bool IsNewArrival { get; set; }
        /// <summary>
        /// 是否推荐特色商品
        /// </summary> 
        [SolrField(ProductSolrField.IsRecommend)]
        public bool IsRecommend { get; set; }
        /// <summary>
        /// 是否热销品
        /// </summary> 
        [SolrField(ProductSolrField.IsBestSeller)]
        public bool IsBestSeller { get; set; }
        /// <summary>
        /// 是否DailyDeal商品
        /// </summary> 
        [SolrField(ProductSolrField.IsDailyDeal)]
        public bool IsDailyDeal { get; set; }
        /// <summary>
        /// 是否Club商品
        /// </summary> 
        [SolrField(ProductSolrField.IsClub)]
        public bool IsClub { get; set; }
        /// <summary>
        /// 是否促销商品
        /// </summary> 
        [SolrField(ProductSolrField.IsPromotion)]
        public bool IsPromotion { get; set; }
        /// <summary>
        /// 促销类型
        /// </summary> 
        [SolrField(ProductSolrField.PromotionId)]
        public int? PromotionId { get; set; }
        /// <summary>
        /// 促销折扣
        /// </summary> 
        [SolrField(ProductSolrField.PromotionDiscount)]
        public decimal? PromotionDiscount { get; set; }
        /// <summary>
        /// 产品专区(多)
        /// </summary> 
        [SolrField(ProductSolrField.ProductAreaId)]
        public int[] ProductAreaId { get; set; }

        /// <summary>
        /// 产品匹配产品(多)
        /// </summary>
        [SolrField(ProductSolrField.ProductMatchId)]
        public int[] ProductMatchId { get; set; }

        /// <summary>
        /// 产品库存量
        /// </summary> 
        [SolrField(ProductSolrField.ProductStock)]
        public int ProductStock { get; set; }
        /// <summary>
        /// 产品被查看次数
        /// </summary> 
        [SolrField(ProductSolrField.ProductViewed)]
        public int ProductViewed { get; set; }
        /// <summary>
        /// 产品总销量
        /// </summary> 
        [SolrField(ProductSolrField.ProductOrdered)]
        public int ProductOrdered { get; set; }
        /// <summary>
        /// 产品年销量
        /// </summary> 
        [SolrField(ProductSolrField.ProductOrderedYear)]
        public int ProductOrderedYear { get; set; }
        /// <summary>
        /// 产品季销量
        /// </summary> 
        [SolrField(ProductSolrField.ProductOrderedSeason)]
        public int ProductOrderedSeason { get; set; }
        /// <summary>
        /// 产品月销量
        /// </summary> 
        [SolrField(ProductSolrField.ProductOrderedMonth)]
        public int ProductOrderedMonth { get; set; }
        /// <summary>
        /// 产品周销量
        /// </summary> 
        [SolrField(ProductSolrField.ProductOrderedWeek)]
        public int ProductOrderedWeek { get; set; }
        /// <summary>
        /// 根类别Id
        /// </summary> 
        [SolrField(ProductSolrField.RootCategoryId)]
        public int? RootCategoryId { get; set; }
        /// <summary>
        /// 根类别名称
        /// </summary> 
        [SolrField(ProductSolrField.RootCategoryName)]
        public string RootCategoryName { get; set; }
        /// <summary>
        /// 上级别Id
        /// </summary> 
        [SolrField(ProductSolrField.ParentCategoryId)]
        public int? ParentCategoryId { get; set; }
        /// <summary>
        /// 上级别名称
        /// </summary> 
        [SolrField(ProductSolrField.ParentCategoryName)]
        public string ParentCategoryName { get; set; }
        /// <summary>
        /// 类别Id
        /// </summary> 
        [SolrField(ProductSolrField.CategoryId)]
        public int CategoryId { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary> 
        [SolrField(ProductSolrField.CategoryName)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 类别路径，包括多个类别
        /// </summary>
        [SolrField(ProductSolrField.CategoryPath)]
        public int[] CategoryPath { get; set; }
        /// <summary>
        /// 属性值ID
        /// </summary> 
        [SolrField(ProductSolrField.PropertyValueId)]
        public int[] PropertyValueId { get; set; }
        /// <summary>
        /// 属性值名称
        /// </summary> 
        [SolrField(ProductSolrField.PropertyValueName)]
        public string[] PropertyValueName { get; set; }
        /// <summary>
        /// 属性值组ID
        /// </summary> 
        [SolrField(ProductSolrField.PropertyValueGroupId)]
        public int[] PropertyValueGroupId { get; set; }
        /// <summary>
        /// 属性值组名称
        /// </summary> 
        [SolrField(ProductSolrField.PropertyValueGroupName)]
        public string[] PropertyValueGroupName { get; set; }
        /// <summary>
        ///  推荐商品商品Best Match排序值
        /// </summary> 
        [SolrField(ProductSolrField.ProductSortRecommend)]
        public int ProductSortRecommend { get; set; }
        /// <summary>
        ///  Dailys Deal商品Best Match排序值
        /// </summary> 
        [SolrField(ProductSolrField.ProductSortDailyDeal)]
        public int ProductSortDailyDeal { get; set; }
        /// <summary>
        /// 促销商品Best Match排序值
        /// </summary> 
        [SolrField(ProductSolrField.ProductSortPromotion)]
        public string ProductSortPromotion { get; set; }
        /// <summary>
        /// 热销商品Best Match排序值
        /// </summary> 
        [SolrField(ProductSolrField.ProductSortBestSeller)]
        public string ProductSortBestSeller { get; set; }
        /// <summary>
        /// 普通商品Best Match排序值
        /// </summary> 
        [SolrField(ProductSolrField.ProductSortNormal)]
        public string ProductSortNormal { get; set; }
        /// <summary>
        /// 产品排序值
        /// </summary> 
        [SolrField(ProductSolrField.ProductSortStatus)]
        public int ProductSortStatus { get; set; }
        /// <summary>
        /// 产品额外分值
        /// </summary> 
        [SolrField(ProductSolrField.ProductScore)]
        public decimal ProductScore { get; set; }
        /// <summary>
        /// 产品Solr搜索分值
        /// </summary> 
        [SolrField(ProductSolrField.Score)]
        public decimal Score { get; set; }
    }
}
