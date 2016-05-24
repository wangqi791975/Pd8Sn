using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.AdminUser;
using Com.Panduo.Service.Article;
using Com.Panduo.Service.Cash;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Customer.Club;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Help;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Product.ClubProduct;
using Com.Panduo.Service.Review;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SEO;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.ProductArea;
using Com.Panduo.Service.Suggestion;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Service.Product.DailyDeal;
using Com.Panduo.Service.Product.Promotion;
using Com.Panduo.Service.SystemMail;


namespace Com.Panduo.Service
{
    /// <summary>
    /// 服务工厂
    /// </summary>
    public class ServiceFactory
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            SystemService.LoadCacheAtferInit();
        }

        /// <summary>
        /// 后台用户服务
        /// </summary>  
        public static IAdminUserService AdminUserService { get; set; }
         
        /// <summary>
        /// 站点配置服务
        /// </summary>
        public static IConfigureService ConfigureService { get; set; }

        /// <summary>
        /// 产品服务
        /// </summary>
        public static IProductService ProductService { get; set; } 

        /// <summary>
        /// 产品图片服务
        /// </summary>
        public static IProductImageService ProductImageService { get; set; } 
        
        /// <summary>
        /// 属性服务
        /// </summary>
        public static IPropertyService PropertyService { get; set; }

        /// <summary>
        /// 客户服务
        /// </summary>
        public static ICustomerService CustomerService { get; set; }

        /// <summary>
        /// Club服务
        /// </summary>
        public static IClubService ClubService { get; set; }

        /// <summary>
        /// 订阅服务
        /// </summary>
        public static INewsletterService NewsletterService { get; set; }

        /// <summary>
        /// 评论服务
        /// </summary>
        public static IReviewService ReviewService { get; set; }

        /// <summary>
        /// 客户建议服务
        /// </summary>
        public static ISuggestionService SuggestionService { get; set; }

        /// <summary>
        /// SEO关键词服务
        /// </summary>
        public static ITopKeywordService TopKeywordService { get; set; }

        /// <summary>
        /// Meta信息服务
        /// </summary>
        public static IMetaService MetaService { get; set; }

        /// <summary>
        /// 渠道商服务
        /// </summary>
        public static IChannelService ChannelService { get; set; }

        /// <summary>
        /// 心愿单服务
        /// </summary>
        public static IWishListService WishListService { get; set; }

        /// <summary>
        /// 产品专区服务
        /// </summary>
        public static IProductAreaService ProductAreaService { get; set; }

        /// <summary>
        /// 产品类别服务
        /// </summary>
        public static ICategoryService CategoryService { get; set; }

        /// <summary>
        /// 一口价产品服务
        /// </summary>
        public static IProductDailyDealService ProductDailyDealService { get; set; }

        /// <summary>
        /// 促销服务
        /// </summary>
        public static IPromotionService PromotionService { get; set; }

        /// <summary>
        /// 系统站点服务
        /// </summary>
        public static ISystemService SystemService { get; set; }
        
        /// <summary>
        /// 支付配置服务
        /// </summary>
        public static IPaymentService PaymentService { get; set; }

        /// <summary>
        /// Cash服务
        /// </summary>
        public static ICashService CashService { get; set; }

        /// <summary>
        /// Coupon服务
        /// </summary>
        public static ICouponService CouponService { get; set; }

        /// <summary>
        /// 客户产品服务
        /// </summary>
        public static ICustomerProductService CustomerProductService { get; set; }

        /// <summary>
        /// club会员产品服务
        /// </summary>
        public static IClubProductService ClubProductService { get; set; }

        /// <summary>
        /// 帮助中心
        /// </summary>
        public static IHelpService HelpService { get; set; }
        
        #region 订单模块
        public static IOrderService OrderService { get; set; }

        #region 购物车
        /// <summary>
        /// 购物车服务
        /// </summary>
        public static IShoppingCartService ShoppingCartService { get; set; }
        #endregion

        #region 运费
        public static IShippingService ShippingService { get; set; }
        #endregion

        #endregion

        #region 营销模块
        /// <summary>
        /// 营销服务
        /// </summary>
        public static IMarketingService MarketingService { get; set; }

        public static IBannerService BannerService { get; set; }

        public static IMailService MailService { get; set; }

        /// <summary>
        /// 后台零散文章
        /// </summary>
        public static IArticleService ArticleService { get; set; }
        #endregion

    }
}
