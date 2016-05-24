using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.ServiceImpl.Product.Solr
{
    /// <summary>
    /// 产品Solr字段
    /// </summary> 
    public static class ProductSolrField
    {
        /// <summary>
        /// Solr的ID
        /// </summary>
        public const string Id = "id";
        /// <summary>
        /// 产品自增ID
        /// </summary>
        public const string ProductId = "product_id";
        /// <summary>
        /// 产品编号，比如B00001
        /// </summary>
        public const string ProductCode = "product_code";
        /// <summary>
        /// 产品Sku，比如B00001,但是具有like匹配功能
        /// </summary>
        public const string Sku = "sku";
        /// <summary>
        /// 产品重量(单位:g)
        /// </summary>
        public const string ProductWeight = "product_weight";
        /// <summary>
        /// 体积重量(单位:g)
        /// </summary>
        public const string VolumeWeight = "volume_weight";
        /// <summary>
        /// 产品上架时间
        /// </summary>
        public const string JoinDate = "join_date";
        /// <summary>
        /// 产品创建时间
        /// </summary>
        public const string CreateDate = "create_date";
        /// <summary>
        /// 产品最后修改时间
        /// </summary>
        public const string LastModifyDate = "last_modify_date";
        /// <summary>
        /// 产品状态
        /// </summary>
        public const string ProductStatus = "product_status";
        /// <summary>
        /// 最小起订量
        /// </summary>
        public const string MinOrderQty = "min_order_qty";
        /// <summary>
        /// 单位
        /// </summary>
        public const string ProductUnit = "product_unit";
        /// <summary>
        /// 每组数量
        /// </summary>
        public const string PackageQty = "package_qty";
        /// <summary>
        /// 产品名称
        /// </summary>
        public const string ProductName = "product_name";
        /// <summary>
        /// 营销标题
        /// </summary>
        public const string MarketingTitle = "marketing_title";
        /// <summary>
        /// 产品描述
        /// </summary>
        public const string ProductDesc = "product_desc";
        /// <summary>
        /// 是否混装
        /// </summary>
        public const string IsMixed = "is_mixed";
        /// <summary>
        /// 是否次品
        /// </summary>
        public const string IsDefective = "is_defective";

        /// <summary>
        /// 美元基本成本价 = 人民币成本价 *　固定汇率
        /// </summary>
        public const string ProductPrice = "product_price";
        /// <summary>
        /// 美元最终成本价　=　美元基本成本价 + 上下浮动价
        /// </summary>
        public const string FinalPrice = "final_price";
        /// <summary>
        /// 美元最低售价 = 美元最终成本价 * 最低利润系数
        /// </summary>
        public const string SalePriceMin = "sale_price_min";
        /// <summary>
        /// 美元最搞售价 = 美元最终成本价 * 最搞利润系数
        /// </summary>
        public const string SalePriceMax = "sale_price_max";

        /// <summary>
        /// 是否新品
        /// </summary>
        public const string IsNewArrival = "is_new_arrival";
        /// <summary>
        /// 是否推荐特色商品
        /// </summary>
        public const string IsRecommend = "is_recommend";
        /// <summary>
        /// 是否热销品
        /// </summary>
        public const string IsBestSeller = "is_best_seller";
        /// <summary>
        /// 是否DailyDeal商品
        /// </summary>
        public const string IsDailyDeal = "is_daily_deal";
        /// <summary>
        /// 是否Club商品
        /// </summary>
        public const string IsClub = "is_club";
        /// <summary>
        /// 是否促销商品
        /// </summary>
        public const string IsPromotion = "is_promotion";
        /// <summary>
        /// 促销类型
        /// </summary>
        public const string PromotionId = "promotion_id";
        /// <summary>
        /// 促销折扣
        /// </summary>
        public const string PromotionDiscount = "promotion_discount";
        /// <summary>
        /// 产品专区(多)
        /// </summary>
        public const string ProductAreaId = "product_area_id";
        /// <summary>
        /// 产品匹配产品(多)
        /// </summary>
        public const string ProductMatchId = "product_match_id";

        /// <summary>
        /// 产品库存量
        /// </summary>
        public const string ProductStock = "product_stock";
        /// <summary>
        /// 产品被查看次数
        /// </summary>
        public const string ProductViewed = "product_viewed";
        /// <summary>
        /// 产品总销量
        /// </summary>
        public const string ProductOrdered = "product_ordered";
        /// <summary>
        /// 产品年销量
        /// </summary>
        public const string ProductOrderedYear = "product_ordered_year";
        /// <summary>
        /// 产品季销量
        /// </summary>
        public const string ProductOrderedSeason = "product_ordered_season";
        /// <summary>
        /// 产品月销量
        /// </summary>
        public const string ProductOrderedMonth = "product_ordered_month";
        /// <summary>
        /// 产品周销量
        /// </summary>
        public const string ProductOrderedWeek = "product_ordered_week";
        /// <summary>
        /// 根类别Id
        /// </summary>
        public const string RootCategoryId = "root_category_id";
        /// <summary>
        /// 根类别名称
        /// </summary>
        public const string RootCategoryName = "root_category_name";
        /// <summary>
        /// 上级别Id
        /// </summary>
        public const string ParentCategoryId = "parent_category_id";
        /// <summary>
        /// 上级别名称
        /// </summary>
        public const string ParentCategoryName = "parent_category_name";
        /// <summary>
        /// 类别Id
        /// </summary>
        public const string CategoryId = "category_id";
        /// <summary>
        /// 类别名称
        /// </summary>
        public const string CategoryName = "category_name";
        /// <summary>
        /// 类别路径，包括多个类别
        /// </summary>
        public const string CategoryPath = "category_path";
        /// <summary>
        /// 属性值ID
        /// </summary>
        public const string PropertyValueId = "property_value_id";
        /// <summary>
        /// 属性值名称
        /// </summary>
        public const string PropertyValueName = "property_value_name";
        /// <summary>
        /// 属性值组ID
        /// </summary>
        public const string PropertyValueGroupId = "property_value_group_id";
        /// <summary>
        /// 属性值组名称
        /// </summary>
        public const string PropertyValueGroupName = "property_value_group_name";
        /// <summary>
        /// 推荐商品Best Match排序值
        /// </summary>
        public const string ProductSortRecommend = "product_sort_recommend";
        /// <summary>
        /// Dailys Deal商品Best Match排序值
        /// </summary>
        public const string ProductSortDailyDeal = "product_sort_daily_deal";
        /// <summary>
        /// 促销商品Best Match排序值
        /// </summary>
        public const string ProductSortPromotion = "product_sort_promotion";
        /// <summary>
        /// 热销商品Best Match排序值
        /// </summary>
        public const string ProductSortBestSeller = "product_sort_best_seller";
        /// <summary>
        /// 普通商品Best Match排序值
        /// </summary>
        public const string ProductSortNormal = "product_sort_normal";
        /// <summary>
        /// 产品状态排序值
        /// </summary>
        public const string ProductSortStatus = "product_sort_status";
        /// <summary>
        /// 产品额外分值
        /// </summary>
        public const string ProductScore = "product_score"; 
        /// <summary>
        /// 产品Solr搜索分值
        /// </summary>
        public const string Score = "score"; 
        /// <summary>
        /// 随机值
        /// </summary>
        public const string Random = "random_";
        
    }
}
