using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Spring.Globalization;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 页面辅助
    /// </summary>
    public static class PageHelper
    {
        public static string GetUrl(int pageIndex, string pageUrl = null)
        {
            return PageManager.GetUrl(pageIndex, pageUrl);
        }


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
            var c = ServiceFactory.ConfigureService.GetCurrencyByCode(currencyCode);
            if (c.IsNullOrEmpty())
            {
                return MoneyFormat(money);
            }
            else
            {
                return GetRoundValue(money, c.DecimalPlaces);
            }
            return 0.0M;
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
        ////public static string ToExchangeCurrencyMoneyString(this decimal amountUsd, string currencyCode, decimal? exchangeRate = null, bool isShort = false)
        ////{
        ////    var currency = CacheHelper.GetCurrencyByCode(currencyCode);
        ////    return amountUsd.ToExchangeCurrencyMoneyString(currency, exchangeRate, isShort);
        ////}

        #endregion



        public static string ToCurrentShortDate(DateTime dt)
        {
            return dt.ToDateString("MMM. dd, yyyy");
        }

        #region Url 相关





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






    









    }
}
