
using System;

namespace Com.Panduo.Service.ServiceConst
{
    /// <summary>
    /// 服务配置信息
    /// </summary>
    public class ServiceConfig
    {
        /// <summary>
        /// 初始化后是否加载数据到缓存
        /// </summary>
        public static bool IsLoadCacheInit { get; set; }
        /// <summary>
        /// 初始化后是否加载商品数据到缓存
        /// </summary>
        public static bool IsLoadProductCacheInit { get; set; }

        /// <summary>
        /// 当前语言ID
        /// </summary>
        [Obsolete("请不要直接使用该属性，请使用：ServiceFactory.ConfigureService.SiteLanguageId")]
        public static int LangId { get; set; }

        /// <summary>
        /// 英语语言ID
        /// </summary>
        [Obsolete("请不要直接使用该属性，请使用：ServiceFactory.ConfigureService.EnglishLangId")]
        public static int EnglishLangId { get; set; }

        /// <summary>
        /// 当前语言二位简码
        /// </summary>
        [Obsolete("请不要直接使用该属性，请使用：ServiceFactory.ConfigureService.SiteLanguageCode")]
        public static string Lang { get; set; }

        /// <summary>
        /// 默认国家二级简码(如：英文站为US、俄语站为RU)
        /// </summary>
        public static string DefaultCountrySimpleCode2 { get; set; }

        /// <summary>
        /// 支付配置文件完整路径
        /// </summary>
        public static string PaymentConfigFileFullPath { get; set; }

        /// <summary>
        /// 是否IP限制
        /// </summary>
        public static bool IsIpAddressLimit { get; set; }

        /// <summary>
        /// 匿名用户购物车数量
        /// </summary>
        public static int AnonymousUsersShoppingcartCount { get; set; }

        /// <summary>
        /// 原图下载地址
        /// </summary>
        public static string ImageUrl { get; set; }

        /// <summary>
        /// 原图批量压缩请求地址
        /// </summary>
        public static string ImageBatchUrl { get; set; }


        /// <summary>
        /// 错误邮箱
        /// </summary>
        public static string ErrorMail { get; set; }


        /// <summary>
        ///  用户建议邮箱
        /// </summary>
        public static string SuggestionMail { get; set; }

        /// <summary>
        /// 系统邮箱
        /// </summary>
        public static string SystemMail { get; set; }


        /// <summary>
        /// 客户地址上限个数
        /// </summary>
        public static int CustomersAddressMaxCount { get; set; }

        /// <summary>
        /// 客户历史浏览记录上限个数
        /// </summary>
        public static int RecentlyViewedMaxCount { get; set; }


        /// <summary>
        /// 网站联系方式 邮箱
        /// </summary>
        public static string SiteContactMailBox { get; set; }

        /// <summary>
        /// 网站联系方式Skype
        /// </summary>
        public static string SiteContactSkype { get; set; }

        /// <summary>
        /// 网站联系方式Telephone 
        /// </summary>
        public static string SiteContactTelephone { get; set; }

        /// <summary>
        /// 站点IP库，IP1，IP2
        /// </summary>
        public static string SiteIpLibrary { get; set; }


        /// <summary>
        /// 邮件模板路径
        /// </summary>
        public static string MailTemplate { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 非club客户的运费
        /// </summary>
        public static decimal ShippingFeeBefore { get; set; }

        /// <summary>
        /// club客户的运费
        /// </summary>
        public static decimal ShippingFeeAfter { get; set; }
        
        /// <summary>
        /// 手续费
        /// </summary>
        public static decimal HandlingFee { get; set; }
        /// <summary>
        /// 订单金额容差(美元)
        /// </summary>
        public static decimal AmountTolerance { get; set; }
        /// <summary>
        /// 需要用美元来支付Paypal的币种,用逗号分隔多个
        /// </summary>
        public static string PayByPaypalUseUsd { get; set; }
        /// <summary>
        /// 网络请求默认超时时长,单位:秒
        /// </summary>
        public static int RequestTimeout { get; set; }
        /// <summary>
        /// Admin修改密码时长
        /// </summary>
        public static int AdminModifyTime { get; set; }



        /*
        /// <summary>
        /// Solr地址
        /// </summary>
        public static string SolrUrl { get; set; }
        /// <summary>
        /// memcache地址
        /// </summary>
        public static string MemcacheUrl { get; set; }
        /// <summary>
        /// 站点当前语种
        /// </summary>
        public static string SiteLanguageCode { get; set; }
        /// <summary>
        /// 是否促销
        /// </summary>
        public static bool IsPromotion { get; set; }
        /// <summary>
        /// 产品图片Base Url
        /// </summary>
        public static string ProductImageBaseUrl { get; set; }

        /// <summary>
        /// 订单所有图片Base Url
        /// </summary>
        public static string OrderImagesBaseUrl { get; set; }*/

    }
}
