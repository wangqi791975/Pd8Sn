using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Service.SiteConfigure.Payment;
using Com.Panduo.ServiceImpl.SiteConfigure.Dao;
using Com.Panduo.Entity.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure
{
    public class ConfigureService : IConfigureService
    {
        #region IOC
        public IIpWhiteBlackListDao IpWhiteBlackListDao { private get; set; }
        public ICountryContinentDao CountryContinentDao { private get; set; }
        public ICountryDao CountryDao { private get; set; }
        public ICountryDescriptionDao CountryDescriptionDao { private get; set; }
        public IAddressFormatDao AddressFormatDao { private get; set; }
        public ICountryProvinceDao CountryProvinceDao { private get; set; }
        public ICountryProvinceDescriptionDao CountryProvinceDescriptionDao { private get; set; }
        public ICountryCityDao CountryCityDao { private get; set; }
        public ICountryCityDescriptionDao CountryCityDescriptionDao { private get; set; }
        public ICountryHighRiskDao CountryHighRiskDao { private get; set; }
        public ISearchKeywordDao SearchKeywordDao { private get; set; }
        public ILanguageDao LanguageDao { private get; set; }
        public ICurrencyDao CurrencyDao { private get; set; }
        public ICurrencyLogDao CurrencyLogDao { private get; set; }
        public ICountryIp1Dao CountryIp1Dao { private get; set; }
        public ICountryIp2Dao CountryIp2Dao { private get; set; }
        public IConfigDao ConfigDao { private get; set; }

        #endregion

        #region 常量
        /// <summary>
        /// 促销开关是否开启
        /// </summary>
        public static readonly string SYSTEM_CONFIG_IS_PROMOTION_SWITCH_ON = "IsPromotionSwitchOn";
        /// <summary>
        /// 促销开始时间
        /// </summary>
        public static readonly string SYSTEM_CONFIG_PROMOTION_DATE_BEGIN = "PromotionDateBegin";
        /// <summary>
        /// 促销截止时间
        /// </summary>
        public static readonly string SYSTEM_CONFIG_PROMOTION_DATE_END = "PromotionDateEnd";
        /// <summary>
        /// Dailydeals开始时间
        /// </summary>
        public static readonly string SYSTEM_CONFIG_DAILYDEALS_DATE_BEGIN = "DailydealsDateBegin";
        /// <summary>
        /// Dailydeals截止时间
        /// </summary>
        public static readonly string SYSTEM_CONFIG_DAILYDEALS_DATE_END = "DailydealsDateEnd";
        /// <summary>
        /// 专属优惠券
        /// </summary>
        public static readonly string SYSTEM_CONFIG_SHIPPING_FEE_BEFORE = "ShippingFeeBefore";
        /// <summary>
        /// 专属优惠券
        /// </summary>
        public static readonly string SYSTEM_CONFIG_EXCLUSIVE_COUPON = "ExclusiveCoupon";
        /// <summary>
        /// 专属优惠券
        /// </summary>
        public static readonly string SYSTEM_CONFIG_SHIPPING_FEE_AFTER = "ShippingFeeAfter";
        /// <summary>
        /// 专属优惠券
        /// </summary>
        public static readonly string SYSTEM_CONFIG_HANDLING_FEE = "HandlingFee";

        public static readonly string Common_AD = "Common_AD_{0}";

        public string ERROR_KEY_NOT_EXIST { get { return "ERROR_KEY_NOT_EXIST"; } }
        public string ERROR_WHITELIST_EXIST { get { return "ERROR_WHITELIST_EXIST"; } }
        public string ERROR_WHITELIST_NOT_EXIST { get { return "ERROR_WHITELIST_NOT_EXIST"; } }
        public string ERROR_BLACKLIST_EXIST { get { return "ERROR_BLACKLIST_EXIST"; } }
        public string ERROR_BLACKLIST_NOT_EXIST { get { return "ERROR_BLACKLIST_NOT_EXIST"; } }
        public string ERROR_CURRENCY_NOT_EXIST { get { return "ERROR_CURRENCY_NOT_EXIST"; } }
        public string ERROR_CURRENCY_RATE_NOT_EXIST { get { return "ERROR_CURRENCY_RATE_NOT_EXIST"; } }
        public string ERROR_COUNTRY_NOT_EXIST { get { return "ERROR_COUNTRY_NOT_EXIST"; } }
        public string ERROR_SEACH_KEYWORD_NOT_EXIST { get { return "ERROR_SEACH_KEYWORD_NOT_EXIST"; } }
        public string ERROR_RATE_LESS_THAN_ZERO { get { return "ERROR_RATE_LESS_THAN_ZERO"; } }
        public string CURRENCY_CODE_USD { get { return "USD"; } }
        public string LANGUAGE_CODE_EN { get { return "en"; } }
        #endregion

        #region 属性
        public bool IsIpAddressLimit
        {
            get { return ServiceConfig.IsIpAddressLimit; }
            set { ServiceConfig.IsIpAddressLimit = value; }
        }
        /// <summary>
        /// 促销总开关
        /// </summary>
        public bool IsPromotion
        {
            get
            {
                return GetConfigFromCache<bool>(SYSTEM_CONFIG_IS_PROMOTION_SWITCH_ON);
            }
            set { SetConfigAndCache(SYSTEM_CONFIG_IS_PROMOTION_SWITCH_ON, value); }
        }


        public DateTime? DailydealsDateBegin
        {
            get
            {
                return GetConfigFromCache<DateTime>(SYSTEM_CONFIG_DAILYDEALS_DATE_BEGIN);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_DAILYDEALS_DATE_BEGIN, value);
            }
        }

        public DateTime? DailydealsDateEnd
        {
            get
            {
                return GetConfigFromCache<DateTime>(SYSTEM_CONFIG_DAILYDEALS_DATE_END);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_DAILYDEALS_DATE_END, value);
            }
        }

        public DateTime? PromotionDateBegin
        {
            get
            {
                return GetConfigFromCache<DateTime>(SYSTEM_CONFIG_PROMOTION_DATE_BEGIN);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_PROMOTION_DATE_BEGIN, value);
            }
        }

        public DateTime? PromotionDateEnd
        {
            get
            {
                return GetConfigFromCache<DateTime>(SYSTEM_CONFIG_PROMOTION_DATE_END);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_PROMOTION_DATE_END, value);
            }
        }

        public int AnonymousUsersShoppingcartCount
        {
            get { return ServiceConfig.AnonymousUsersShoppingcartCount; }
            set { ServiceConfig.AnonymousUsersShoppingcartCount = value; }
        }

        public string ImageUrl
        {
            get { return ServiceConfig.ImageUrl; }
            set { ServiceConfig.ImageUrl = value; }
        }

        public string ImageBatchUrl
        {
            get { return ServiceConfig.ImageBatchUrl; }
            set { ServiceConfig.ImageBatchUrl = value; }
        }

        public string ErrorMail
        {
            get { return ServiceConfig.ErrorMail; }
            set { ServiceConfig.ErrorMail = value; }
        }

        public string SuggestionMail
        {
            get { return ServiceConfig.SuggestionMail; }
            set { ServiceConfig.SuggestionMail = value; }
        }

        public string SystemMail
        {
            get { return ServiceConfig.SystemMail; }
            set { ServiceConfig.SystemMail = value; }
        }

        public int CustomersAddressMaxCount
        {
            get { return ServiceConfig.CustomersAddressMaxCount; }
            set { ServiceConfig.CustomersAddressMaxCount = value; }
        }

        public int RecentlyViewedMaxCount
        {
            get { return ServiceConfig.RecentlyViewedMaxCount; }
            set { ServiceConfig.RecentlyViewedMaxCount = value; }
        }

        public string SiteLanguageCode
        {
            // ReSharper disable once CSharpWarnings::CS0618
            get { return ServiceConfig.Lang; }
            // ReSharper disable once CSharpWarnings::CS0618
            set { ServiceConfig.Lang = value; }
        }

        public int SiteLanguageId
        {
            // ReSharper disable once CSharpWarnings::CS0618
            get { return ServiceConfig.LangId; }
            // ReSharper disable once CSharpWarnings::CS0618
            set { ServiceConfig.LangId = value; }
        }

        public int EnglishLangId
        {
            // ReSharper disable once CSharpWarnings::CS0618
            get { return ServiceConfig.EnglishLangId; }
            // ReSharper disable once CSharpWarnings::CS0618
            set { ServiceConfig.EnglishLangId = value; }
        }

        public string SiteIpLibrary
        {
            get { return ServiceConfig.SiteIpLibrary; }
            set { ServiceConfig.SiteIpLibrary = value; }
        }

        #endregion

        #region Coupon
        public decimal ShippingFeeBefore
        {
            get
            {
                return GetConfigFromCache<decimal>(SYSTEM_CONFIG_SHIPPING_FEE_BEFORE);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_SHIPPING_FEE_BEFORE, value);
            }
        }

        public decimal ExclusiveCoupon
        {
            get
            {
                return GetConfigFromCache<decimal>(SYSTEM_CONFIG_EXCLUSIVE_COUPON);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_EXCLUSIVE_COUPON, value);
            }
        }

        public decimal ShippingFeeAfter
        {
            get
            {
                return GetConfigFromCache<decimal>(SYSTEM_CONFIG_SHIPPING_FEE_AFTER);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_SHIPPING_FEE_AFTER, value);
            }
        }

        public decimal HandlingFee
        {
            get
            {
                return GetConfigFromCache<decimal>(SYSTEM_CONFIG_HANDLING_FEE);
            }
            set
            {
                SetConfigAndCache(SYSTEM_CONFIG_HANDLING_FEE, value);
            }
        }
        #endregion

        #region 黑白名单管理
        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="whitelist">白名单Vo</param>
        public void AddWhiteList(WhiteList whitelist)
        {
            var ipWhiteBlack = new IpWhiteBlackListPo()
            {
                IpAddress = whitelist.IpAddress,
                Type = false,
                DateCreated = DateTime.Now,
                AdminId = whitelist.CreateId
            };
            if (!IpWhiteBlackListDao.IsExistIpWhiteBlackList(whitelist.IpAddress, false))
            {
                IpWhiteBlackListDao.AddObject(ipWhiteBlack);
            }
            else
            {
                throw new BussinessException(ERROR_WHITELIST_EXIST);
            }
        }

        /// <summary>
        /// 删除白名单
        /// </summary>
        /// <param name="whiteListId">白名单Id</param>
        public void DeleteWhiteListById(int whiteListId)
        {
            IpWhiteBlackListDao.DeleteWhiteListById(whiteListId);
        }

        public IList<WhiteList> GetWhiteList()
        {
            var list = new List<WhiteList>();
            var ipWhiteList = IpWhiteBlackListDao.GetAllIpWhiteList();
            if (!ipWhiteList.IsNullOrEmpty())
            {
                foreach (var whitePo in ipWhiteList)
                {
                    if (whitePo.Type == false)
                    {
                        var white = new WhiteList();
                        white.CreateTime = whitePo.DateCreated;
                        white.IpAddress = whitePo.IpAddress;
                        white.ModifyTime = whitePo.DateModified;
                        white.CreateId = whitePo.AdminId;
                        list.Add(white);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blackListId"></param>
        public void DeleteBlackListById(int blackListId)
        {
            IpWhiteBlackListDao.DeleteBlackListById(blackListId);
        }

        public void AddBlackList(BlackList blackList)
        {
            var ipWhiteBlack = new IpWhiteBlackListPo()
            {
                IpAddress = blackList.IpAddress,
                Type = true,
                DateCreated = DateTime.Now,
                AdminId = blackList.CreateId
            };

            if (!IpWhiteBlackListDao.IsExistIpWhiteBlackList(blackList.IpAddress, true))
            {
                IpWhiteBlackListDao.AddObject(ipWhiteBlack);
            }
            else
            {
                throw new BussinessException(ERROR_BLACKLIST_EXIST);
            }

        }

        public IList<BlackList> GetBlackList()
        {
            var list = new List<BlackList>();
            var ipBlackList = IpWhiteBlackListDao.GetAllIpBlackList();
            if (!ipBlackList.IsNullOrEmpty())
            {
                foreach (var blackListPo in ipBlackList)
                {
                    list.Add(new BlackList()
                    {
                        CreateTime = blackListPo.DateCreated,
                        IpAddress = blackListPo.IpAddress,
                        ModifyTime = blackListPo.DateModified,
                        CreateId = blackListPo.AdminId,

                    });
                }
            }
            return list;
        }

        public bool IsIpAllowVisit(string ipAddress)
        {
            var whitePo = IpWhiteBlackListDao.GetIpWhiteBlackListByIpAddress(ipAddress, false);

            if (!whitePo.IsNullOrEmpty())
            {
                return true;
            }

            else
            {
                var blackPo = IpWhiteBlackListDao.GetIpWhiteBlackListByIpAddress(ipAddress, true);

                if (!blackPo.IsNullOrEmpty())
                {
                    return false;
                }
                else
                {
                    string code = GetCountrySimpleCode2ByIp(ipAddress);
                    if ("CN".Equals(code.ToUpper()))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        #endregion

        #region Config

        public void SetConfig(Config config)
        {
            var configPo = GetConfigPoByVo(config);
            if (ConfigDao.GetConfigByKey(config.Key).IsNullOrEmpty())
                ConfigDao.AddObject(configPo);
            else
                ConfigDao.UpdateConfig(config.Key, config.Value);
        }


        public void SetConfig(List<Config> configs)
        {
            foreach (var config in configs)
            {
                SetConfig(config);
            }
        }

        public void UpdateConfig(string key, string value)
        {
            ConfigDao.UpdateConfig(key, value);
        }

        public Config GetConfig(string key)
        {
            return GetConfigVoByPo(ConfigDao.GetConfigByKey(key));
        }

        internal static ConfigPo GetConfigPoByVo(Config config)
        {
            ConfigPo configPo = null;
            if (!config.IsNullOrEmpty())
            {
                configPo = new ConfigPo
                {
                    Id = config.Id,
                    Key = config.Key,
                    Value = config.Value,
                    Comment = config.Cmmt
                };
            }
            return configPo;
        }

        internal static Config GetConfigVoByPo(ConfigPo configPo)
        {
            Config config = null;
            if (!configPo.IsNullOrEmpty())
            {
                config = new Config
                {
                    Id = configPo.Id,
                    Key = configPo.Key,
                    Value = configPo.Value,
                    Cmmt = configPo.Comment
                };
            }
            return config;
        }
        #endregion

        public void SetCurrencyStatusById(int currencyId, bool status)
        {
            var currencyPo = CurrencyDao.GetObject(currencyId);
            if (currencyPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUNTRY_NOT_EXIST);
            }
            currencyPo.Status = status;
            CurrencyDao.UpdateObject(currencyPo);
        }

        public Currency GetCurrency(int currencyId)
        {
            var lstCurrenciesVo = ImplCacheHelper.GetAllCurrencies();
            if (!lstCurrenciesVo.IsNullOrEmpty())
                return lstCurrenciesVo.Find(m => m.CurrencyId == currencyId);
            return GetCurrencyVoFromPo(CurrencyDao.GetObject(currencyId));
        }

        public IList<Currency> GetAllCurrencies()
        {
            var lstCurrenciesVo = ImplCacheHelper.GetAllCurrencies();
            if (!lstCurrenciesVo.IsNullOrEmpty()) return lstCurrenciesVo;

            var currencyPoList = CurrencyDao.GetAll();
            if (!currencyPoList.IsNullOrEmpty())
            {
                lstCurrenciesVo = currencyPoList.Select(GetCurrencyVoFromPo).ToList();
            }

            ImplCacheHelper.SetAllCurrencies(lstCurrenciesVo);
            return lstCurrenciesVo;
        }

        public IList<Currency> GetAllValidCurrencies()
        {
            var lstValidCurrenciesVo = ImplCacheHelper.GetAllValidCurrencies();
            if (!lstValidCurrenciesVo.IsNullOrEmpty()) return lstValidCurrenciesVo;

            var currencyPoList = CurrencyDao.GetAllValidCurrency();
            if (!currencyPoList.IsNullOrEmpty())
            {
                lstValidCurrenciesVo = currencyPoList.Select(GetCurrencyVoFromPo).ToList();
            }

            ImplCacheHelper.SetAllValidCurrencies(lstValidCurrenciesVo);
            return lstValidCurrenciesVo;
        }

        #region 获取远程汇率
        private const string CURRENCYURL = "http://www.boc.cn/sourcedb/whpj/";
        public const string CURRENCYCODE = "USD|THB|SGD|RUB|NZD|JPY|GBP|EUR|CHF|CAD|BRL|AUD|UAH";//货币类型
        public IList<Currency> GetAllRemoteCurrencies()
        {
            IList<Currency> currencyList = new List<Currency>();
            string url = CURRENCYURL; //+ HttpUtility.UrlEncode(tmSet.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo));
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string sHtml = wc.DownloadString(url);
            string sXml = string.Empty;
            int iValueCnt = CURRENCYCODE.Split(new char[] { '|' }, 20, StringSplitOptions.RemoveEmptyEntries).Length;

            string[] sTBody = sHtml.Split(new string[] { "<table width=\"880\"" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ss in sTBody)
            {
                if (ss.Contains("货币名称") || ss.Contains("Currency Name"))
                {
                    string[] sbrs = ss.Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
                    //剔除第一行
                    for (var i = 1; i < sbrs.Length; i++)
                    {
                        string scur = string.Empty;
                        string row = sbrs[i];
                        decimal dRate = GetCurrencyRate(row, out scur);
                        if (scur != string.Empty)
                        {
                            var currency = GetCurrencyByCode(scur);
                            if (currency != null)
                            {
                                if (dRate != 0.0M && !string.IsNullOrEmpty(scur))
                                {
                                    currency.ExchangeRateRemote = Convert.ToDecimal(dRate);
                                }
                                currencyList.Add(currency);
                            }
                        }


                    }
                    break;
                }
            }
            var usDollers = currencyList.Where(x => x != null && x.CurrencyCode == "USD").FirstOrDefault().ExchangeRateRemote;
            if (usDollers != null)
            {
                foreach (var item in currencyList)
                {
                    item.ExchangeRateRemote = Convert.ToDecimal(string.Format("{0:N8}", usDollers / item.ExchangeRateRemote));
                }
            }
            return currencyList.OrderBy(x => x.CurrencyId).ToList();
        }
        internal static decimal GetCurrencyRate(string source, out string sCurrency)
        {
            sCurrency = string.Empty;
            var moneyType = string.Empty;
            var moneyCode = string.Empty;
            var rate = 0.0M;
            //string sPattern = @"<td.+?>(.+?)</td>";
            string sPattern = @"<td.?>(.+?)</td>";
            var mc = Regex.Matches(source, sPattern);
            if (mc.Count > 5)
            {
                moneyType = mc[0].Groups[1].Value;
                moneyCode = GetCode(CURRENCYCODE, moneyType);
                if (!string.IsNullOrEmpty(moneyCode))
                {
                    sCurrency = moneyCode;
                }
                if (mc.Count > 6)
                {
                    if (IsNumeric(mc[6].Groups[1].Value))
                    {
                        rate = Convert.ToDecimal(mc[6].Groups[1].Value);
                    }
                    else
                    {
                        rate = Convert.ToDecimal(mc[5].Groups[1].Value);
                    }
                }
                else
                {
                    rate = Convert.ToDecimal(mc[3].Groups[1].Value);
                }
            }
            return rate;
        }
        internal static string GetCode(string codes, string codeName)
        {
            var code = string.Empty;
            var moneyArray = codes.Split('|');
            CurrencyEnum tType = CurrencyEnum.USD;
            foreach (var money in moneyArray)
            {
                tType = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), money);
                if (tType.GetDescription() == codeName)
                {
                    code = tType.ToString();
                    break;
                }
            }

            return code;
        }
        internal static bool IsNumeric(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.|,]?\d*$");
            return reg.IsMatch(str);
        }

        #endregion

        public void ModfiyRate(Currency currency)
        {
            var currencyPo = CurrencyDao.GetObject(currency.CurrencyId);
            if (currencyPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CURRENCY_NOT_EXIST);
            }
            if (currency.ExchangeRate < 0)
            {
                throw new BussinessException(ERROR_RATE_LESS_THAN_ZERO);
            }
            RecordExchangeRateLog(currencyPo.CurrencyCode, currencyPo.Value, currency.ExchangeRate, 0);
            currencyPo.Value = currency.ExchangeRate;
            CurrencyDao.UpdateObject(currencyPo);
        }

        public void ModfiyRates(IList<KeyValuePair<int, decimal>> currencies, int adminId)
        {
            IList<CurrencyPo> updates = new List<CurrencyPo>();
            foreach (var currency in currencies)
            {
                var po = CurrencyDao.GetObject(currency.Key);
                decimal before = po.Value;
                po.Value = currency.Value;
                po.DateModified = DateTime.Now;
                if (before != currency.Value && currency.Value > 0)
                {
                    RecordExchangeRateLog(po.CurrencyCode, before, currency.Value, adminId);
                    updates.Add(po);
                }
            }
            CurrencyDao.UpdateObjects(updates);
        }

        /// <summary>
        /// 记录汇率修改日志
        /// </summary>
        /// <param name="code">币种编码</param>
        /// <param name="before">修改前汇率</param>
        /// <param name="after">修改后汇率</param>
        /// <param name="adminId">修改人Id</param>
        private void RecordExchangeRateLog(string code, decimal before, decimal after, int adminId)
        {
            var po = new CurrencyLogPo()
            {
                AdminId = adminId,
                CurrencyCode = code,
                DateModified = DateTime.Now,
                PreviousValue = before,
                Value = after
            };
            CurrencyLogDao.AddObject(po);
        }

        public decimal GetSingleCurrencyRate(int currencyId)
        {
            return CurrencyDao.GetSingleCurrencyRate(currencyId);
        }

        public decimal GetSingleCurrencyRate(string currencyCode)
        {
            return CurrencyDao.GetSingleCurrencyRate(currencyCode);
        }

        public Currency GetCurrencyByCode(string currencyCode)
        {
            return GetAllValidCurrencies().FirstOrDefault(c => string.Equals(c.CurrencyCode, currencyCode, StringComparison.InvariantCultureIgnoreCase));
        }

        public PageData<ExchangeRateLog> FindRateLog(int currentPage, int pageSize, IDictionary<ExchangeRateLogCriteria, object> searchCriteria, IList<Sorter<ExchangeRateLogSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("Select a From CurrencyLogPo a");

            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case ExchangeRateLogCriteria.CurrencyCode:
                            hqlHelper.AddWhere("a.CurrencyCode", HqlOperator.Like, "CurrencyCode", item.Value);
                            break;
                        case ExchangeRateLogCriteria.ModifiyId:
                            hqlHelper.AddWhere("a.AdminId", HqlOperator.Eq, "AdminId", item.Value);
                            break;
                        case ExchangeRateLogCriteria.CreateDateFrom:
                            hqlHelper.AddWhere("a.DateModified", HqlOperator.Egt, "CreateDateFrom", item.Value.ToDateQueryFrom());
                            break;
                        case ExchangeRateLogCriteria.CreateDateTo:
                            hqlHelper.AddWhere("a.DateModified", HqlOperator.Lt, "CreateDateTo", item.Value.ToDateQueryTo());
                            break;
                    }
                }
            }

            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case ExchangeRateLogSorterCriteria.CreateDate:
                            hqlHelper.AddSorter("a.DateModified", sorter.IsAsc);
                            break;
                        case ExchangeRateLogSorterCriteria.ModifiyId:
                            hqlHelper.AddSorter("a.AdminId", sorter.IsAsc);
                            break;
                        case ExchangeRateLogSorterCriteria.LogId:
                            hqlHelper.AddSorter("a.LogId", sorter.IsAsc);
                            break;
                        case ExchangeRateLogSorterCriteria.CurrencyCode:
                            hqlHelper.AddSorter("a.CurrencyCode", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("a.CreateDate", false);
            }

            //3.执行查询并返回数据
            var pageDataPo = CurrencyLogDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);

            var pageDataVo = new PageData<ExchangeRateLog>();
            var voList = new List<ExchangeRateLog>();
            foreach (var po in pageDataPo.Data)
            {
                voList.Add(GetExchangeRateLogVoFromPo(po));
            }

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;

            return pageDataVo;
        }

        public IList<Continent> GetAllContinent()
        {
            var continentList = new List<Continent>();
            var list = CountryContinentDao.GetAll();
            if (!list.IsNullOrEmpty())
            {
                continentList.AddRange(list.Select(po => new Continent
                {
                    ContinentId = po.ContinentId,
                    ContinentName = po.ContinentName
                }));
            }
            return continentList;
        }

        /// <summary>
        /// 设置国家地址格式
        /// </summary>
        /// <param name="countryId">国家Id</param>
        /// <param name="format">地址格式</param>
        public void SetCountryAddressFormat(int countryId, string format)
        {
            var countryPo = CountryDao.GetObject(countryId);
            if (countryPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUNTRY_NOT_EXIST);
            }
            if (countryPo.AddressFormat.AddressFormatId > 0 & AddressFormatDao.ExistObject("AddressFormatId", countryPo.AddressFormat.AddressFormatId))
            {
                AddressFormatDao.UpdateObjectByHql("UPDATE AddressFormatPo SET AddressFormat=? WHERE AddressFormatId = ?", new object[] { format, countryPo.AddressFormat.AddressFormatId });
            }
            else
            {
                var po = new AddressFormatPo()
                {
                    AddressFormat = format,
                };

                int i = AddressFormatDao.AddObject(po);
                countryPo.AddressFormat = AddressFormatDao.GetObject(i);
                CountryDao.UpdateObject(countryPo);
            }

        }

        public void SetCountryHidden(int countryId, bool isHidden)
        {
            var countryPo = CountryDao.GetObject(countryId);
            if (countryPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUNTRY_NOT_EXIST);
            }
            countryPo.Status = isHidden;
            CountryDao.UpdateObject(countryPo);
        }

        public IList<Country> GetAllCountry()
        {
            var countryList = new List<Country>();
            var list = CountryDao.GetAll();
            if (!list.IsNullOrEmpty())
            {
                foreach (var countryPo in list)
                {
                    countryList.Add(GetCountryVoFromPo(countryPo));
                }
            }
            return countryList;
        }

        public IList<Country> GetAllCountryByContinent(int continentId)
        {
            var countryList = new List<Country>();
            var list = CountryDao.FindDataByHql("from CountryPo where ContinentId=?", continentId);
            if (!list.IsNullOrEmpty())
            {
                foreach (var countryPo in list)
                {
                    countryList.Add(GetCountryVoFromPo(countryPo));
                }
            }
            return countryList;
        }

        public IList<Country> GetCommonCountry()
        {
            var countryList = new List<Country>();
            var list = CountryDao.GetAllCommonCountry();
            if (!list.IsNullOrEmpty())
            {
                foreach (var countryPo in list)
                {
                    countryList.Add(GetCountryVoFromPo(countryPo));
                }
            }
            return countryList;
        }

        public IList<Country> GetUnCommonCountry()
        {
            var unCountryList = new List<Country>();
            var list = CountryDao.GetAllUnCommonCountry();
            if (!list.IsNullOrEmpty())
            {
                unCountryList.AddRange(list.Select(GetCountryVoFromPo));
            }
            return unCountryList;
        }

        public IList<Country> GetAllValidCountry()
        {
            var countryList = new List<Country>();
            var list = CountryDao.GetAllValidCountry();
            if (!list.IsNullOrEmpty())
            {
                foreach (var countryPo in list)
                {
                    countryList.Add(GetCountryVoFromPo(countryPo));
                }
            }
            return countryList;
        }

        public IList<CountryLanguage> GetAllCountryLanguages()
        {
            var countryLanguageList = new List<CountryLanguage>();
            var countryDescriptionPoList = CountryDescriptionDao.GetAll();
            if (!countryDescriptionPoList.IsNullOrEmpty())
            {
                foreach (var countryPo in countryDescriptionPoList)
                {
                    countryLanguageList.Add(GetCountryLanguageVoFromPo(countryPo));
                }
            }
            return countryLanguageList;
        }

        public IList<CountryLanguage> GetCountryLanguages(int countryId)
        {
            var list = new List<CountryLanguage>();
            var countryDescriptionList = CountryDescriptionDao.GetCountryLanguages(countryId);

            if (!countryDescriptionList.IsNullOrEmpty())
            {
                foreach (var countryPo in countryDescriptionList)
                {
                    list.Add(GetCountryLanguageVoFromPo(countryPo));
                }
            }
            return list;
        }

        public CountryLanguage GetCountryLanguage(int countryId, int languageId)
        {
            var po = CountryDescriptionDao.GetCountryLanguage(countryId, languageId);
            return GetCountryLanguageVoFromPo(po);
        }

        public void SetCommonCountry(int countryId, bool flag, int displayOrder)
        {
            var countryPo = CountryDao.GetObject(countryId);
            if (countryPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUNTRY_NOT_EXIST);
            }
            countryPo.IsCommon = flag;
            countryPo.SortOrder = displayOrder;
            CountryDao.UpdateObject(countryPo);
        }

        public string GetCountryAddressFormat(int countryId)
        {
            return CountryDao.GetCountryAddressFormat(countryId);
        }

        public string GetCountrySimpleCode2ByIp(string ipAddress)
        {
            long ipNum = ipAddress.ToIpNumber();
            if ("ip1".Equals(SiteIpLibrary.ToLower()))//判断当前是否IP库1
            {
                var countryIp1Po = CountryIp1Dao.GetCountryIp1(ipNum);
                if (!countryIp1Po.IsNullOrEmpty())
                {
                    return countryIp1Po.CountryIsoCode2;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                var countryIp2Po = CountryIp2Dao.GetCountryIp2(ipNum);
                if (!countryIp2Po.IsNullOrEmpty())
                {
                    return countryIp2Po.CountryIsoCode2;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public Country GetCountryBySimpleCode2(string simpleCode2)
        {
            var po = CountryDao.GetCountryBySimpleCode2(simpleCode2);
            return GetCountryVoFromPo(po);
        }

        public Country GetCountryByIp(string ipAddress)
        {
            string simpleCode2 = GetCountrySimpleCode2ByIp(ipAddress);
            Country country = null;
            if (simpleCode2 != string.Empty)
            {
                country = GetCountryBySimpleCode2(simpleCode2);
            }
            if (country == null || simpleCode2 == string.Empty)
            {
                country = GetCountryBySimpleCode2(ServiceConfig.DefaultCountrySimpleCode2);
            }
            return country;
        }

        public Country GetCountryById(int countryId)
        {
            var countryPo = CountryDao.GetObject(countryId);
            return GetCountryVoFromPo(countryPo);
        }

        public string GetCoutryNameByIds(string countryIds, string decollator)
        {
            if (countryIds == "All")
                return "所有";
            var countryIdArr = countryIds.Split(decollator);
            string countryName = countryIdArr.Select(id => Convert.ToInt32(id)).Aggregate("", (current, countryId) => current + (CountryDao.GetObject(countryId).CountryName + decollator));
            if (countryName != "")
                countryName = countryName.Substring(0, countryName.Length - decollator.Length);
            return countryName;
        }

        public string GetLanguageNameByIds(string languageIds, string decollator)
        {
            if (languageIds == "All")
                return "所有";
            var languageIdArr = languageIds.Split(decollator);
            string languageName = "";
            foreach (string id in languageIdArr)
            {
                int languageId = Convert.ToInt32(id);
                languageName += LanguageDao.GetObject(languageId).ChineseName + decollator;
            }
            if (languageName != "")
                languageName = languageName.Substring(0, languageName.Length - decollator.Length);
            return languageName;
        }

        public IList<Province> GetAllProvinceByCountryId(int countryId)
        {
            var provinceList = new List<Province>();
            var provincePoList = CountryProvinceDao.GetCountryProvinceByCountryId(countryId);

            if (!provincePoList.IsNullOrEmpty())
            {
                foreach (var provincePo in provincePoList)
                {
                    provinceList.Add(GetProvinceVoFromPo(provincePo));
                }
            }

            return provinceList;
        }

        public IList<ProvinceLanguage> GetAllProvinceLanguages()
        {
            var provinceLanguageList = new List<ProvinceLanguage>();
            var poList = CountryProvinceDescriptionDao.GetAll();
            if (!poList.IsNullOrEmpty())
            {
                foreach (var provincePo in poList)
                {
                    provinceLanguageList.Add(GetProvinceDescriptionVoFromPo(provincePo));
                }
            }
            return provinceLanguageList;
        }

        public IList<ProvinceLanguage> GetProvinceLanguages(int provinceId)
        {
            var provinceLanguageList = new List<ProvinceLanguage>();
            var poList = CountryProvinceDescriptionDao.GetProvinceLanguages(provinceId);
            if (!poList.IsNullOrEmpty())
            {
                foreach (var provincePo in poList)
                {
                    provinceLanguageList.Add(GetProvinceDescriptionVoFromPo(provincePo));
                }
            }
            return provinceLanguageList;
        }

        public ProvinceLanguage GetProvinceLanguage(int provinceId, int languageid)
        {
            var po = CountryProvinceDescriptionDao.GetProvinceLanguage(provinceId, languageid);
            return GetProvinceDescriptionVoFromPo(po);
        }

        public IList<City> GetAllCityByProvinceId(int provinceId)
        {
            var cityList = new List<City>();
            var cityPoList = CountryCityDao.GetAllCityByProvinceId(provinceId);

            if (!cityPoList.IsNullOrEmpty())
            {
                foreach (var cityPo in cityPoList)
                {
                    cityList.Add(GetCityVoFromPo(cityPo));
                }
            }
            return cityList;
        }


        public IList<CityLanguage> GetAllCityLanguages()
        {
            var cityLanguageList = new List<CityLanguage>();
            var cityLanguagePoList = CountryCityDescriptionDao.GetAll();
            if (!cityLanguagePoList.IsNullOrEmpty())
            {
                foreach (var cityPo in cityLanguagePoList)
                {
                    cityLanguageList.Add(GetCityLanguageVoFromPo(cityPo));
                }
            }
            return cityLanguageList;
        }

        public IList<CityLanguage> GetCityLanguages(int cityId)
        {
            var cityLanguageList = new List<CityLanguage>();
            var cityLanguagePoList = CountryCityDescriptionDao.GetCityLanguages(cityId);
            if (!cityLanguagePoList.IsNullOrEmpty())
            {
                foreach (var cityPo in cityLanguagePoList)
                {
                    cityLanguageList.Add(GetCityLanguageVoFromPo(cityPo));
                }
            }
            return cityLanguageList;
        }

        public CityLanguage GetCityLanguage(int cityId, int languageId)
        {
            var cityLanguagePo = CountryCityDescriptionDao.GetCityLanguage(cityId, languageId);
            return GetCityLanguageVoFromPo(cityLanguagePo);
        }

        public bool IsCountryHighRisk(int countryId)
        {
            var risk = CountryHighRiskDao.GetObject(countryId);
            if (risk != null)
            {
                return true;
            }
            return false;
        }

        public void AddCountryHighRisks(IList<CountryHighRisk> countryHighRisks)
        {
            //CountryHighRiskDao.AddObjects(countryHighRisks.Select(x => GetCountryHighRiskPoFromVo((CountryHighRisk)x)).ToList());
            foreach (var countryHighRisk in countryHighRisks)
            {
                var risk = GetCountryHighRiskPoFromVo(countryHighRisk);
                CountryHighRiskDao.AddObject(risk);
            }
        }

        public IList<CountryHighRisk> GetAllCountryHighRisks()
        {

            var list = CountryHighRiskDao.GetAll();
            return list.Select(x => GetCountryHighRiskVoFromPo((CountryHighRiskPo)x)).ToList();
        }

        /// <summary>
        /// 站内联系方式
        /// </summary>
        /// <returns>SiteContact</returns>
        public SiteContact GetContact()
        {
            return new SiteContact()
            {
                MailBox = ServiceConfig.SiteContactMailBox,
                Skype = ServiceConfig.SiteContactSkype,
                Telephone = ServiceConfig.SiteContactTelephone,
            };
        }

        #region 支付方式
        public GlobalCollectConfig GetGlobalCollectPaymentConfig()
        {
            throw new NotImplementedException();
        }

        public AlipayConfig GetAlipayPaymentConfig()
        {
            throw new NotImplementedException();
        }

        public PayPalConfig GetPaypalPaymentConfig()
        {
            throw new NotImplementedException();
        }

        public QiwiConfig GetQiwiPaymentConfig()
        {
            throw new NotImplementedException();
        }
        #endregion


        public string GetCommonAdvertisement()
        {
            string keyformat = string.Format(Common_AD, ServiceFactory.ConfigureService.SiteLanguageId);
            var config = ConfigDao.GetConfigByKey(keyformat);
            if (!config.IsNullOrEmpty())
            {
                return config.Value;
            }
            return string.Empty;
        }

        #region 搜索关键词
        public void SetSearchKeyword(SearchKeyword word)
        {
            var po = GetSearchKeywordPoFromVo(word);
            if (po.Id != 0)
            {
                SearchKeywordDao.UpdateObject(po);
            }
            else
            {
                SearchKeywordDao.AddObject(po);
            }
        }

        public void DeleteSeachKeywordById(int keywordId)
        {
            SearchKeywordDao.DeleteObjectById(keywordId);
        }

        public SearchKeyword GetSearchKeyword(int keywordId)
        {
            return GetSearchKeywordVoFromPo(SearchKeywordDao.GetObject(keywordId));
        }

        public IList<SearchKeyword> GetSearchKeywordByType(KeywordType type)
        {
            var searchKeywordList = new List<SearchKeyword>();
            var list = SearchKeywordDao.GetSearchKeywordByType(type);
            if (!list.IsNullOrEmpty())
            {
                foreach (var searchKeywordPo in list)
                {
                    searchKeywordList.Add(GetSearchKeywordVoFromPo(searchKeywordPo));
                }
            }
            return searchKeywordList;
        }

        public PageData<SearchKeyword> FindSearchKeyword(int currentPage, int pageSize, IDictionary<SearchKeywordCriteria, object> searchCriteria, IList<Sorter<SearchKeywordSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM SearchKeywordPo");

            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case SearchKeywordCriteria.KeywordType:
                            hqlHelper.AddWhere("Type", HqlOperator.Eq, "Type", item.Value);
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("SortOrder", true);
            }

            var pageDataPo = SearchKeywordDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<SearchKeyword>();
            var voList = pageDataPo.Data.Select(GetSearchKeywordVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #endregion
        public IList<Language> GetAllValidLanguage()
        {
            var languageList = new List<Language>();
            var list = LanguageDao.GetAllValidLanguageList();
            if (!list.IsNullOrEmpty())
            {
                foreach (var languagePo in list)
                {
                    var language = new Language();
                    language.LanguageId = languagePo.LanguageId;
                    language.DateFormatLong = languagePo.DateFormatLong;
                    language.DateFormatShort = languagePo.DateFormatShort;
                    language.DisplayOrder = languagePo.SortOrder;
                    language.Host = languagePo.Host;
                    language.ChineseName = languagePo.ChineseName;
                    language.LanguageName = languagePo.Name;
                    language.LanguageCode = languagePo.Code;
                    language.OrderPrefix = languagePo.OrderPrefix;
                    language.CustomerManagerId = languagePo.CustomerManagerId;
                    languageList.Add(language);
                }
            }
            return languageList;
        }

        #region Po VO转换
        /// <summary>
        ///国家Po转Vo
        /// </summary>
        /// <param name="countryPo">国家Po</param>
        /// <returns>国家Vo</returns>
        internal static Country GetCountryVoFromPo(CountryPo countryPo)
        {
            Country country = null;
            if (!countryPo.IsNullOrEmpty())
            {
                country = new Country
                {
                    CountryId = countryPo.CountryId,
                    CountryName = countryPo.CountryName,
                    IsCommonCountry = countryPo.IsCommon,
                    SimpleCode2 = countryPo.CountryIsoCode2,
                    SimpleCode3 = countryPo.CountryIsoCode3,
                    ContinentId = countryPo.ContinentId.ContinentId,
                    DisplayFormat = countryPo.AddressFormat.AddressFormat,
                    DisplayFormat1 = countryPo.AddressFormat.AddressFormat1,
                    DisplayOrder = countryPo.SortOrder,
                    IsDisplay = countryPo.Status,
                };

            }
            return country;
        }


        /// <summary>
        /// 国家多语言Po转Vo
        /// </summary>
        /// <param name="countryDescriptionPo">国家多语言Po</param>
        /// <returns>国家多语言Vo</returns>
        internal static CountryLanguage GetCountryLanguageVoFromPo(CountryDescriptionPo countryDescriptionPo)
        {
            CountryLanguage countryLanguage = null;
            if (!countryDescriptionPo.IsNullOrEmpty())
            {
                countryLanguage = new CountryLanguage
                {
                    CountryId = countryDescriptionPo.CountryId,
                    CountryName = countryDescriptionPo.CountryName,
                    LanguageId = countryDescriptionPo.LanguageId,
                };
            }
            return countryLanguage;
        }

        /// <summary>
        /// 国家省Po转Vo
        /// </summary>
        /// <param name="countryProvincePo">国家省Po</param>
        /// <returns>国家省Vo</returns>
        internal static Province GetProvinceVoFromPo(CountryProvincePo countryProvincePo)
        {
            Province province = null;
            if (!countryProvincePo.IsNullOrEmpty())
            {
                province = new Province
                {
                    CountryId = countryProvincePo.CountryId,
                    ProvinceId = countryProvincePo.ProvinceId,
                    ProvinceName = countryProvincePo.ProvinceName,
                    ProvinceCode = countryProvincePo.ProvinceCode,
                };
            }
            return province;
        }

        /// <summary>
        /// 国家省多语种Po转Vo
        /// </summary>
        /// <param name="countryProvinceDescriptionPo">国家省多语种Po</param>
        /// <returns>国家省多语种Vo</returns>
        internal static ProvinceLanguage GetProvinceDescriptionVoFromPo(CountryProvinceDescriptionPo countryProvinceDescriptionPo)
        {
            ProvinceLanguage provinceLanguage = null;
            if (!countryProvinceDescriptionPo.IsNullOrEmpty())
            {
                provinceLanguage = new ProvinceLanguage
                {
                    Province = countryProvinceDescriptionPo.ProvinceId,
                    LanguageId = countryProvinceDescriptionPo.LanguageId,
                    ProvinceName = countryProvinceDescriptionPo.ProvinceName,
                };
            }
            return provinceLanguage;
        }

        /// <summary>
        ///  国家城市Po转Vo
        /// </summary>
        /// <param name="cityPo">国家城市Po</param>
        /// <returns>国家城市Vo</returns>
        internal static City GetCityVoFromPo(CountryCityPo cityPo)
        {
            City city = null;
            if (!cityPo.IsNullOrEmpty())
            {
                city = new City
                {
                    CityId = cityPo.CityId,
                    CityName = cityPo.CityName,
                };
            }
            return city;
        }

        /// <summary>
        ///  国家城市多语种Po转Vo
        /// </summary>
        /// <param name="cityDescriptionPo">国家城市多语种Po</param>
        /// <returns>国家城市多语种Vo</returns>
        internal static CityLanguage GetCityLanguageVoFromPo(CountryCityDescriptionPo cityDescriptionPo)
        {
            CityLanguage cityLanguage = null;
            if (!cityDescriptionPo.IsNullOrEmpty())
            {
                cityLanguage = new CityLanguage
                {
                    CityId = cityDescriptionPo.CityId,
                    LanguageId = cityDescriptionPo.LanguageId,
                    CityName = cityDescriptionPo.CityName,
                };
            }
            return cityLanguage;
        }

        internal static Currency GetCurrencyVoFromPo(CurrencyPo currencyPo)
        {
            Currency currency = null;
            if (!currencyPo.IsNullOrEmpty())
            {
                currency = new Currency
                {
                    CurrencyId = currencyPo.CurrencyId,
                    ChineseName = currencyPo.ChineseName,
                    CurrencyCode = currencyPo.CurrencyCode,
                    DecimalPlaces = currencyPo.DecimalPlaces,
                    SymbolLeft = currencyPo.SymbolLeft,
                    SymbolRight = currencyPo.SymbolRight,
                    Status = currencyPo.Status,
                    ExchangeRate = currencyPo.Value,
                    DisplayOrder = currencyPo.SortOrder,
                    SymbolShort = currencyPo.SymbolShort,
                    DateModified = currencyPo.DateModified
                };
            }
            return currency;
        }

        internal static ExchangeRateLog GetExchangeRateLogVoFromPo(CurrencyLogPo currencyLogPo)
        {
            ExchangeRateLog exchangeRateLog = null;
            if (!currencyLogPo.IsNullOrEmpty())
            {
                exchangeRateLog = new ExchangeRateLog
                {
                    LogId = currencyLogPo.LogId,
                    ModifiedRate = currencyLogPo.Value,
                    OriginalRate = currencyLogPo.PreviousValue,
                    ModifiyId = currencyLogPo.AdminId,
                    ModifiyTime = currencyLogPo.DateModified,
                    CurrencyCode = currencyLogPo.CurrencyCode,
                };
            }
            return exchangeRateLog;
        }


        internal static SearchKeyword GetSearchKeywordVoFromPo(SearchKeywordPo searchKeywordPo)
        {
            SearchKeyword searchKeyword = null;
            if (!searchKeywordPo.IsNullOrEmpty())
            {
                searchKeyword = new SearchKeyword
                {
                    Id = searchKeywordPo.Id,
                    KeywordType = searchKeywordPo.Type.ToEnum<KeywordType>(),
                    LanguageId = searchKeywordPo.LanguageId,
                    KeywordName = searchKeywordPo.Name,
                    DisplayOrder = searchKeywordPo.SortOrder,
                    KeywordUrl = searchKeywordPo.Link,
                };

            }
            return searchKeyword;
        }


        internal static SearchKeywordPo GetSearchKeywordPoFromVo(SearchKeyword searchKeyword)
        {
            SearchKeywordPo searchKeywordPo = null;
            if (!searchKeyword.IsNullOrEmpty())
            {
                searchKeywordPo = new SearchKeywordPo
                {
                    Id = searchKeyword.Id,
                    Type = (int)searchKeyword.KeywordType,
                    LanguageId = searchKeyword.LanguageId,
                    Name = searchKeyword.KeywordName,
                    SortOrder = searchKeyword.DisplayOrder,
                    Link = searchKeyword.KeywordUrl,
                };
            }
            return searchKeywordPo;
        }

        internal static CountryHighRisk GetCountryHighRiskVoFromPo(CountryHighRiskPo countryHighRiskPo)
        {
            CountryHighRisk countryHighRisk = null;
            if (!countryHighRiskPo.IsNullOrEmpty())
            {
                countryHighRisk = new CountryHighRisk();
                ObjectHelper.CopyProperties(countryHighRiskPo, countryHighRisk, new string[] { });
            }
            return countryHighRisk;
        }

        internal static CountryHighRiskPo GetCountryHighRiskPoFromVo(CountryHighRisk countryHighRisk)
        {
            CountryHighRiskPo countryHighRiskPo = null;
            if (!countryHighRisk.IsNullOrEmpty())
            {
                countryHighRiskPo = new CountryHighRiskPo();
                ObjectHelper.CopyProperties(countryHighRisk, countryHighRiskPo, new string[] { });
            }
            return countryHighRiskPo;
        }

        #endregion

        #region 配置设置与缓存
        /// <summary>
        /// 设置并缓存配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        internal void SetConfigAndCache(string key, object value, string comment = "")
        {
            //1.存储到数据库
            var po = ConfigDao.GetConfigByKey(key);
            if (po == null)
            {
                po = new ConfigPo
                {
                    Key = key,
                    Value = value.ToString(),
                    Comment = comment
                };

                ConfigDao.AddObject(po);
            }
            else
            {
                po.Value = value.ToString();
                po.Comment = comment.IsNullOrEmpty() ? po.Comment : comment;

                ConfigDao.UpdateObject(po);
            }

            //2.刷新缓存
            ImplCacheHelper.SetSystemConfig(key, po.Value);
        }

        /// <summary>
        /// 从缓存读取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal T GetConfigFromCache<T>(string key)
        {
            //1.从缓存读取配置值
            var isExist = ImplCacheHelper.ExistKey(key);
            var value = default(T);
            if (!isExist)
            {
                //2.从数据库读取配置
                var configPo = ConfigDao.GetConfigByKey(key);
                value = configPo.Value.ParseTo<T>();

                //3.存储到缓存
                ImplCacheHelper.SetSystemConfig(key, value);
            }
            return value;
        }

        #endregion

        public enum CurrencyEnum
        {
            [Description("美元")]
            USD = 0,
            [Description("泰国铢")]
            THB = 1,
            [Description("新加坡元")]
            SGD = 2,
            [Description("卢布")]
            RUB = 3,
            [Description("新西兰元")]
            NZD = 5,
            [Description("欧元")]
            EUR = 6,
            [Description("日元")]
            JPY = 7,
            [Description("英镑")]
            GBP = 8,
            [Description("瑞士法郎")]
            CHF = 9,
            [Description("加拿大元")]
            CAD = 10,
            [Description("瑞典克朗")]
            BRL = 11,
            [Description("澳大利亚元")]
            AUD = 12,
            [Description("乌克兰赫夫米")]
            UAH = 13
        }
    }
}
