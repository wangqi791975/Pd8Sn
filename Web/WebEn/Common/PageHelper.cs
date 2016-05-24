using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Resources;
using Spring.Globalization;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 页面辅助
    /// </summary>
    public static class PageHelper
    {
        #region 客户偏好信息

        /// <summary>
        /// 得到当前客户默认货币
        /// <para> 1.已登陆客户:从Session获取Preference.Currency</para>
        /// <para> 2.未登录客户或者Preference中未设置：根据语言站点与默认货币对应关系获取</para>
        /// </summary>
        /// <returns></returns>
        public static Currency GetCurrentCurrency()
        {
            Currency currency = null;
            try
            {
                if (!SessionHelper.CurrentCurrency.IsNullOrEmpty())
                {
                    currency = SessionHelper.CurrentCurrency;
                }
                else
                {
                    //1.已登陆客户:从Session获取Preference.Currency
                    Preference preference = null;
                    if (SessionHelper.CurrentCustomerPreference.IsNullOrEmpty())
                    {
                        //未登录客户或者Preference中未设置:从Cookie获取
                        preference = CookieHelper.CurrentCustomerPreference;
                    }
                    if (!preference.IsNullOrEmpty())
                    {
                        currency = CacheHelper.Currencies.Find(x => x.CurrencyId == preference.CurrencyId);
                    }
                    var defaultCurrencyCode = "USD";
                    //2.未登录客户或者Preference中未设置：根据语言站点与默认货币对应关系获取
                    if (currency.IsNullOrEmpty())
                    {
                        // 语言站点与默认货币对应关系如下：
                        //1.英语、站点，默认选中：USD
                        //2.德语、法语、西语、意语站点，默认选中：EUR
                        //3.俄语站点，默认选中：RUB
                        //4.日语站点，默认选中：JPY
                        var currencyWithLanguages = CacheHelper.DefaultCurrencyWithLanguages;
                        if (!currencyWithLanguages.IsNullOrEmpty())
                        {
                            var defaultLanguage = GetCurrentLanguage();
                            if (currencyWithLanguages.Values.ToList().Exists(x => x.Exists(y => y.ToLower() == defaultLanguage.LanguageCode.ToLower())))
                                defaultCurrencyCode = currencyWithLanguages.First(x => x.Value.Exists(y => y.ToLower() == defaultLanguage.LanguageCode.ToLower())).Key;
                            currency = CacheHelper.Currencies.Find(x => x.CurrencyCode.ToLower() == defaultCurrencyCode.ToLower());
                        }
                    }
                    if (currency.IsNullOrEmpty())
                        currency = CacheHelper.Currencies.Find(x => x.CurrencyCode.ToLower() == defaultCurrencyCode.ToLower());

                    if (!currency.IsNullOrEmpty())
                        SessionHelper.CurrentCurrency = currency;
                }
            }
            catch (Exception ex)
            {
                currency = new Currency { CurrencyCode = "USD" };
            }

            return currency;
        }

        /// <summary>
        /// 得到当前客户默认语种
        /// <para> 1.已登陆客户:从Session获取Preference.Language</para>
        /// <para> 2.未登录客户或者Preference中未设置:从Cookie获取</para>
        /// <para> 3.Cookie中没有：获取客户浏览器语言（有则取之，无则默认为英语）</para>
        /// </summary>
        /// <returns>语言名称</returns>
        public static Language GetCurrentLanguage()
        {
            Language language = null;
            //1.已登陆客户:从Session获取Preference.Language
            var preference = SessionHelper.CurrentCustomerPreference;
            if (preference.IsNullOrEmpty())
            {
                //2.未登录客户或者Preference中未设置:从Cookie获取
                preference = CookieHelper.CurrentCustomerPreference;
            }
            if (!preference.IsNullOrEmpty())
            {
                language = CacheHelper.Languages.Find(x => x.LanguageId == preference.LanguageId);
            }
            //3.Cookie中没有：获取客户浏览器语言（有则取之，无则默认为英语）
            if (language.IsNullOrEmpty())
            {
                var browserLanguages = HttpContext.Current.Request.UserLanguages;
                if (!browserLanguages.IsNullOrEmpty() && !browserLanguages[0].IsNullOrEmpty())
                {
                    var browserLanguage = browserLanguages[0].Substring(0, 2).ToLower();
                    browserLanguage = browserLanguage.Replace("ja", "jp");
                    //if (CacheHelper.Languages.Exists(x => x.LanguageCode == browserLanguage))
                    language = CacheHelper.Languages.Find(x => x.LanguageCode.ToLower() == browserLanguage);
                }
            }
            if (language.IsNullOrEmpty())
            {
                var defaultLanguage = ServiceFactory.ConfigureService.SiteLanguageCode.IsNullOrEmpty()
                    ? "en"
                    : ServiceFactory.ConfigureService.SiteLanguageCode;
                language = CacheHelper.Languages.Find(x => x.LanguageCode.ToLower() == defaultLanguage.ToLower());
            }

            return language;
        }

        #endregion

        #region 站点语种code
        /// <summary>
        /// 英语站点code
        /// </summary>
        public static readonly string LANGUAGE_CODE_EN = ServiceFactory.ConfigureService.LANGUAGE_CODE_EN;
        #endregion

        #region 货币转换
        /// <summary>
        /// 美元金额
        /// </summary>
        public static readonly string CURRENCY_CODE_USD = ServiceFactory.ConfigureService.CURRENCY_CODE_USD;

        /// <summary>
        /// 兑换货币
        /// </summary>
        /// <param name="moneyOfUsd">美元金额</param>
        /// <returns></returns>
        public static decimal ExchangeMoneyByUsd(decimal moneyOfUsd)
        {
            return GetRoundValue(moneyOfUsd * GetCurrentCurrency().ExchangeRate, GetCurrentCurrency().DecimalPlaces);
        }

        /// <summary>
        /// 兑换货币
        /// </summary>
        /// <param name="moneyOfUsd">美元金额</param>
        /// <param name="targetCurrency">目标币种</param>
        /// <returns></returns>
        public static decimal ExchangeMoneyByUsd(decimal moneyOfUsd, Currency targetCurrency)
        {
            return ExchangeMoneyByUsd(moneyOfUsd, targetCurrency.ExchangeRate, targetCurrency.DecimalPlaces);
        }


        /// <summary>
        /// 兑换货币
        /// </summary>
        /// <param name="moneyOfUsd">美元金额</param>
        /// <param name="exchangeRate">目标汇率</param>
        /// <param name="decimalPlaces">保留小数位数</param>
        /// <returns></returns>
        public static decimal ExchangeMoneyByUsd(decimal moneyOfUsd, decimal exchangeRate, int decimalPlaces)
        {
            return GetRoundValue(moneyOfUsd * exchangeRate, decimalPlaces);
        }


        /// <summary>
        /// 转换成美元
        /// </summary>
        /// <param name="money"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static decimal ExchangeMoneyToUsd(decimal money, Currency currency)
        {
            return GetRoundValue(money / currency.ExchangeRate, currency.DecimalPlaces);
        }


        /// <summary>
        /// 金额格式化显示
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="usedCurrency">使用币种</param>
        /// <returns></returns>
        public static decimal MoneyFormat(decimal money, Currency usedCurrency)
        {
            return GetRoundValue(money, usedCurrency.DecimalPlaces);
        }

        /// <summary>
        /// 金额格式化显示
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="currencyCode">币种code</param>
        /// <returns></returns>
        public static decimal MoneyFormat(decimal money, string currencyCode)
        {
            var c = CacheHelper.GetCurrencyByCode(currencyCode);
            if (c.IsNullOrEmpty())
            {
                return MoneyFormat(money);
            }
            else
            {
                return GetRoundValue(money, c.DecimalPlaces);
            }
        }

        /// <summary>
        /// 金额格式化2位小数
        /// </summary>
        /// <param name="money">金额</param>
        /// <returns></returns>
        public static decimal MoneyFormat(decimal money)
        {
            return GetRoundValue(money, 2);
        }

        public static decimal GetRoundValue(decimal d, int decimals)
        {
            return Math.Round(d * 1.00M, decimals);
        }

        public static string ToIntText(this decimal d)
        {
            return d.ToMoney("#");
        }

        public static int ToIntValue(decimal d)
        {
            return (int)d;
        }

        public static double ToDoubleValue(decimal d)
        {
            return (double)d;
        }

        /// <summary>
        /// 显示对应币种的金额信息
        /// </summary>
        /// <param name="amount">币种对应的金额</param>
        /// <param name="currency">币种</param>
        /// <param name="isShort">是否简写模式</param>
        /// <returns></returns>
        public static string ToCurrencyMoneyString(this decimal amount, Currency currency, bool isShort = false)
        {
            if (isShort)
            {
                return currency.FormatShort(amount);
            }

            return currency.Format(amount);
        }

        /// <summary>
        /// 转换美元金额为对应币种的金额并返回格式化的金额信息,如果提供了额外汇率则用额外汇率，否则用提供币种的汇率计算
        /// </summary>
        /// <param name="amountUsd">美元金额</param>
        /// <param name="currency">币种</param>
        /// <param name="exchangeRate">额外汇率</param>
        /// <param name="isShort">是否简写模式</param>
        /// <returns></returns>
        public static string ToExchangeCurrencyMoneyString(this decimal amountUsd, Currency currency, decimal? exchangeRate = null, bool isShort = false)
        {
            return (currency.CurrencyCode == CURRENCY_CODE_USD ? Math.Round(amountUsd * 1.00M, currency.DecimalPlaces) : (exchangeRate.HasValue ? ExchangeMoneyByUsd(amountUsd, exchangeRate.Value, currency.DecimalPlaces) : ExchangeMoneyByUsd(amountUsd, currency))).ToCurrencyMoneyString(currency, isShort);
        }

        /// <summary>
        /// 转换美元金额为对应币种的金额并返回格式化的金额信息,如果提供了额外汇率则用额外汇率，否则用提供币种的汇率计算
        /// </summary>
        /// <param name="amountUsd">美元金额</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="exchangeRate">额外汇率</param>
        /// <param name="isShort">是否简写模式</param>
        /// <returns></returns>
        public static string ToExchangeCurrencyMoneyString(this decimal amountUsd, string currencyCode, decimal? exchangeRate = null, bool isShort = false)
        {
            var currency = CacheHelper.GetCurrencyByCode(currencyCode);
            return amountUsd.ToExchangeCurrencyMoneyString(currency, exchangeRate, isShort);
        }

        #endregion

        #region Url 相关

        public static string GetUrl(int pageIndex, string pageUrl = null)
        {
            return PageManager.GetUrl(pageIndex, pageUrl);
        }

        /// <summary>
        /// 生成各语种站点 URL
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetWebSizeUrlByLanguage(Language language, string pageUrl = null)
        {
            pageUrl = string.IsNullOrEmpty(pageUrl) ? HttpContext.Current.Request.Url.AbsoluteUri : pageUrl;//PageManager.GetRootUrl()
            if (language.LanguageCode.ToLower() != "en")
            {
                //pageUrl = string.Format(Regex.Replace(pageUrl, "[a-z]{2,3}.8seasons", "{0}.8seasons"), language.LanguageCode.ToLower());
                pageUrl = string.Format(pageUrl.Replace("de", "{0}"), language.LanguageCode.ToLower());
                pageUrl = string.Format(pageUrl.Replace("www", "{0}"), language.LanguageCode.ToLower());
                pageUrl = string.Format(pageUrl.Replace("it", "{0}"), language.LanguageCode.ToLower());
                pageUrl = string.Format(pageUrl.Replace("es", "{0}"), language.LanguageCode.ToLower());
                pageUrl = string.Format(pageUrl.Replace("jp", "{0}"), language.LanguageCode.ToLower());
                pageUrl = string.Format(pageUrl.Replace("fr", "{0}"), language.LanguageCode.ToLower());
            }
            return pageUrl;
        }
        #endregion

        #region 日期转换

        /// <summary>
        /// 当前月份
        /// </summary>
        /// <param name="dt">待转换时间</param>
        /// <returns></returns>
        public static string ToCurrentMonth(DateTime dt)
        {
            return dt.ToDateString("mmm");
        }


        /// <summary>
        /// 当前语种短日期
        /// </summary>
        /// <param name="dt">待转换时间</param>
        /// <returns></returns>
        public static string ToCurrentShortDate(DateTime dt)
        {
            return dt.ToDateString(GetCurrentLanguage().DateFormatShort);
        }

        /// <summary>
        /// 当前语种长日期
        /// </summary>
        /// <param name="dt">待转换时间</param>
        /// <returns></returns>
        public static string ToCurrentLongDate(DateTime dt)
        {
            return dt.ToDateString(GetCurrentLanguage().DateFormatLong);
        }
        #endregion

        #region 页面展示内容显示
        /// <summary>
        /// 获取显示折扣
        /// </summary>
        /// <param name="discount">折扣</param>
        /// <returns></returns>
        public static string GetShowDiscount(decimal discount)
        {
            discount = 1 - discount;
            return string.Format("{0}{1:0.##}{2}", Lang.TipDiscountPrefix, Math.Abs(discount * 100), Lang.TipDiscountSubfix);
        }

        /// <summary>
        /// 获取显示折扣
        /// </summary>
        /// <param name="price">ProductPrice</param>
        /// <returns></returns>
        public static string GetShowDiscount(ProductPrice price)
        {
            var p = GetDiscount(price);
            return GetShowDiscount(p);
        }

        /// <summary>
        /// 获取折扣
        /// </summary>
        /// <param name="price">ProductPrice</param>
        /// <returns></returns>
        public static decimal GetDiscount(ProductPrice price)
        {
            var customer = SessionHelper.CurrentCustomer;
            var discount = 0.0M;
            if (!price.IsNullOrEmpty())
            {
                if (price.IsNoHaggle && price.StepPrice.Count > 0)
                {
                    discount = price.NoHaggle / price.StepPrice[0].OriginalPrice;
                }
                else if (price.PromotionalDiscount < 1 && price.PromotionalDiscount > 0)
                {
                    discount = price.PromotionalDiscount;
                }
                else if (price.ClubDiscount < 1 && price.ClubDiscount > 0 && !customer.IsNullOrEmpty() && customer.IsClub)
                {
                    discount = price.ClubDiscount;
                }

                else if (!customer.IsNullOrEmpty() && customer.IsVip)
                {
                    discount = customer.VipDiscount;
                }
                else
                {
                    discount = 1;
                }
            }
            return discount;
        }



        /// <summary>
        /// 获取VIP,CLUB图标class
        /// </summary>
        /// <param name="price">ProductPrice</param>
        /// <returns></returns>
        public static string GetProductTipClass(ProductPrice price)
        {
            var customer = SessionHelper.CurrentCustomer;
            var icon = "";
            if (!price.IsNullOrEmpty())
            {
                if (price.PromotionalDiscount < 1 && price.PromotionalDiscount > 0)
                {
                    icon = "pro_sale";
                }

                else if (price.IsNoHaggle)
                {
                    icon = "pro_sale";
                }

                else if (price.ClubDiscount < 1 && price.ClubDiscount > 0 && !customer.IsNullOrEmpty() && customer.IsClub)
                {
                    icon = "pro_club";
                }

                else if (!customer.IsNullOrEmpty() && customer.IsVip)
                {
                    icon = "pro_vip";
                }
                else
                {
                    icon = "pro_sale";
                }
            }
            return icon;
        }

        /// <summary>
        /// 获取产品最低价格
        /// </summary>
        /// <param name="price">ProductPrice</param>
        /// <returns></returns>
        public static string GetProductAsLowAsPrice(ProductPrice price)
        {
            var step = price.StepPrice;
            var diplayprice = 0.0M;
            if (!step.IsNullOrEmpty())
            {
                var discount = GetDiscount(price);
                if (price.IsNoHaggle)
                {
                    diplayprice = step[0].GetDiscountPrice(discount);
                }
                else
                {
                    diplayprice = step[step.Count - 1].GetDiscountPrice(discount);
                }

            }
            return GetCurrentCurrency().Format(ExchangeMoneyByUsd(diplayprice, GetCurrentCurrency()));
        }


        #endregion

        /// <summary>
        /// 判断是否限制库存
        /// </summary>
        /// <param name="stock">产品库存</param>
        /// <returns></returns>
        public static bool JudgeStocklimit(this ProductStock stock)
        {
            return (!stock.IsNullOrEmpty() && (stock.BindStockType == StockStatus.Bind));
        }

        /// <summary>
        /// 获取库存时间(前台判断0显示 in 15 )
        /// </summary>
        /// <param name="stock">产品库存</param>
        /// <returns></returns>
        public static int GetProductStockDay(this ProductStock stock)
        {
            int days = 0;
            if (!stock.IsNullOrEmpty())
            {
                if (stock.DateReturn.HasValue)
                {
                    var day = (stock.DateReturn.Value - DateTime.Today).Days;
                    if (day <= 5)
                    {
                        days = 5;
                    }
                    else if (day <= 10)
                    {
                        days = 10;
                    }
                    else
                    {
                        days = 15;
                    }
                }
                else if (stock.DayReturn.HasValue)
                {
                    days = stock.DayReturn.Value;
                }
                else
                {
                    days = 0;
                }
            }
            return days;
        }

        /// <summary>
        /// 获取客户设置的显示类型
        /// </summary>
        /// <returns></returns>
        public static bool GetCustomerShowType()
        {
            bool type = false;
            var p = SessionHelper.CurrentCustomerPreference;
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                if (!p.IsNullOrEmpty() && p.ListShowType.HasValue)
                {
                    switch (p.ListShowType.Value)
                    {
                        case ListShowType.Gallery:
                            type = true;
                            break;
                        case ListShowType.List:
                            type = false;
                            break;
                    }
                }
            }
            return type;
        }

        /// <summary>
        /// 获取客户设置的页码大小
        /// </summary>
        /// <returns></returns>
        public static int GetCustomerPageSize()
        {
            var pageSize = 60;//默认60
            var p = SessionHelper.CurrentCustomerPreference;
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                if (!p.IsNullOrEmpty() && p.ListShowCount.HasValue)
                {
                    switch (p.ListShowCount.Value)
                    {
                        case ListShowCount.T:
                            pageSize = 30;
                            break;
                        case ListShowCount.S:
                            pageSize = 60;
                            break;
                        case ListShowCount.N:
                            pageSize = 90;
                            break;
                    }
                }
            }
            return pageSize;
        }

        #region 获取客户默认地址
        /// <summary>
        /// 获取客户默认地址
        /// </summary>
        /// <param name="country"></param>
        /// <param name="countryId"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public static Address GetDefaultShippingAddress(out Country country, int countryId = 0, string zipCode = "")
        {
            country = null;
            if (countryId > 0)
            {
                country = ServiceFactory.ConfigureService.GetCountryById(countryId);
            }

            var shippingAddress = new Address();
            if (SessionHelper.CurrentCustomer != null)
            {
                shippingAddress = ServiceFactory.CustomerService.GetDefaultShippingAddress(SessionHelper.CurrentCustomer.CustomerId);
            }
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty() && (shippingAddress == null || shippingAddress.AddressId == 0))
            {
                shippingAddress =
                    ServiceFactory.CustomerService.GetAddressById(SessionHelper.CurrentCustomer.ShippingAddress.ParseTo(0));
            }

            if (shippingAddress == null || shippingAddress.AddressId == 0)
                shippingAddress = new Address { AddressId = 0, Province = string.Empty, ZipCode = zipCode };

            if (country == null && shippingAddress.Country > 0)
            {
                country = ServiceFactory.ConfigureService.GetCountryById(shippingAddress.Country);
            }

            if (country != null) return shippingAddress;

            country = ServiceFactory.ConfigureService.GetCountryByIp(PageManager.GetClientIp());

            return shippingAddress;
        }
        #endregion
    }
}
