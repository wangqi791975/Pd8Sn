using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Com.Panduo.Common;
using Com.Panduo.Entity.Coupon;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.ServiceImpl.Coupon.Dao;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Com.Panduo.ServiceImpl.Marketing.Dao;

namespace Com.Panduo.ServiceImpl.Coupon
{
    public class CouponService : ICouponService
    {
        public ICouponDao CouponDao { private get; set; }
        public ICouponDescDao CouponDescDao { private get; set; }
        public ICouponCustomerDao CouponCustomerDao { private get; set; }
        public ICouponCustomerViewDao CouponCustomerViewDao { private get; set; }
        public ICustomerDao CustomerDao { private get; set; }
        public IMarketingCouponDao MarketingCouponDao { private get; set; }

        public string ERROR_CUSTOMER_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_NOT_EXIST"; }
        }

        public string ERROR_COUPON_CANT_ZERO
        {
            get { return "ERROR_COUPON_CANT_ZERO"; }
        }

        public string ERROR_COUPON_NOT_EXIST
        {
            get { return "ERROR_COUPON_NOT_EXIST"; }
        }

        public string ERROR_COUPON_PASS_DUE
        {
            get { return "ERROR_COUPON_PASS_DUE"; }
        }

        public string ERROR_COUPON_GT
        {
            get { return "ERROR_COUPON_GT"; }
        }

        public string ERROR_CURRENCY_CANT_PICK
        {
            get { return "ERROR_CURRENCY_CANT_PICK"; }
        }

        public string ERROR_COUNTRY_CANT_PICK
        {
            get { return "ERROR_COUNTRY_CANT_PICK"; }
        }

        public string ERROR_LANGUAGE_CANT_PICK
        {
            get { return "ERROR_LANGUAGE_CANT_PICK"; }
        }

        public string ERROR_COUPONCUSTOMER_NOT_EXIST
        {
            get { return "ERROR_COUPONCUSTOMER_NOT_EXIST"; }
        }

        public string ERROR_BEGINTIME_GREATER_ENDTIME
        {
            get { return "ERROR_BEGINTIME_GREATER_ENDTIME"; }
        }

        public string ERROR_CUSTOMER_CANTUSE_COUPON
        {
            get { return "ERROR_CUSTOMER_CANTUSE_COUPON"; }
        }

        public string ERROR_AMOUNT_LOW
        {
            get { return "ERROR_AMOUNT_LOW"; }
        }

        public string ERROR_COUPON_CODE_EXIST
        {
            get { return "ERROR_COUPON_CODE_EXIST"; }
        }

        public string ERROR_COUPON_HAS_PICK
        {
            get { return "ERROR_COUPON_HAS_PICK"; }
        }

        public string ERROR_COUPON_HAS_EXPIRED
        {
            get { return "ERROR_COUPON_HAS_EXPIRED"; }
        }

        public int CreateCoupon(Service.Coupon.Coupon coupon, List<CouponDesc> couponDescs)
        {
            var couponValid = CouponDao.GetCoupon(coupon.CouponCode);
            if (!couponValid.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPON_CODE_EXIST);
            }
            int couponId = CouponDao.AddObject(GetCouponPoFromVo(coupon));
            foreach (var couponDesc in couponDescs)
            {
                couponDesc.CouponId = couponId;
                CouponDescDao.AddObject(GetCouponDescPoFromVo(couponDesc));
            }
            return couponId;
        }

        public void EditCoupon(Service.Coupon.Coupon coupon, List<CouponDesc> couponDescs)
        {
            CouponDao.UpdateObject(GetCouponPoFromVo(coupon));
            foreach (var couponDesc in couponDescs)
            {
                CouponDescDao.UpdateCouponDesc(couponDesc.CouponId, couponDesc.LanguageId, couponDesc.Description);
            }
        }

        public void SendCoupon(int couponId, int customerId, int adminId)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var couponPo = CouponDao.GetObject(couponId);
            if (couponPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPON_NOT_EXIST);
            }
            var po = GetCouponCustomerPoFromCouponPo(couponPo, customerId);
            CouponCustomerDao.AddObject(po);
        }

        public void SendCoupon(int couponId, List<int> customerIds, int adminId)
        {
            var couponPo = CouponDao.GetObject(couponId);
            if (couponPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPON_NOT_EXIST);
            }
            var pos = customerIds.Select(c => GetCouponCustomerPoFromCouponPo(couponPo, c)).ToList();

            CouponCustomerDao.AddObjects(pos);
        }

        public void SendCoupon(string couponCode, string customerEmail, int adminId)
        {
            var customer = CustomerDao.GetCustomerByEmail(customerEmail);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var couponPo = CouponDao.GetCoupon(couponCode);
            if (couponPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPON_NOT_EXIST);
            }
            if (couponPo.ExpiredType == LimitType.BeginEnd && couponPo.UseDateEnded < DateTime.Now)
            {
                throw new BussinessException(ERROR_COUPON_PASS_DUE);
            }
            if (couponPo.ExpiredType == LimitType.Day && couponPo.GetDateEnded < DateTime.Now)
            {
                throw new BussinessException(ERROR_COUPON_PASS_DUE);
            }
            var customerCoupons = CouponCustomerDao.GetCouponCustomers(customer.CustomerId, couponCode);
            if (!customerCoupons.IsNullOrEmpty() && couponPo.GetPersonLimit != 0 &&
                customerCoupons.Count >= couponPo.GetPersonLimit)
            {
                throw new BussinessException(ERROR_COUPON_GT);
            }
            var po = GetCouponCustomerPoFromCouponPo(couponPo, customer.CustomerId);
            CouponCustomerDao.AddObject(po);
        }

        public void DeleteCoupon(int couponId)
        {
            CouponDao.DeleteObjectById(couponId);
            CouponDescDao.DelteCouponDesc(couponId);
        }

        public PageData<Service.Coupon.Coupon> FindAllCoupon(int currentPage, int pageSize, IDictionary<CouponSearchCriteria, object> searchCriteria, IList<Sorter<CouponSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CouponPo C");
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        //todo 查询条件
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        //todo 排序条件
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.CouponId", false);
            }

            var pageDataPo = CouponDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<Service.Coupon.Coupon>();
            var voList = pageDataPo.Data.Select(GetCouponVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public Service.Coupon.Coupon GetCoupon(int couponId)
        {
            return GetCouponVoFromPo(CouponDao.GetObject(couponId));
        }

        public Service.Coupon.Coupon GetCoupon(string couponCode)
        {
            return GetCouponVoFromPo(CouponDao.GetCoupon(couponCode));
        }

        public Service.Coupon.Coupon GetRegisterCoupon(int languageId)
        {
            MarketingCouponDao.GetCouponCodeForRegister(null);
            return null;
        }

        public Dictionary<int, CouponDesc> GetCouponDesc(int couponId)
        {
            var couponDescPos = CouponDescDao.GetCouponDescs(couponId);
            return couponDescPos.ToDictionary(couponDescPo => couponDescPo.LanguageId, GetCouponDescVoFromPo);
        }

        public CouponDesc GetCouponDesc(int couponId, int languageId)
        {
            return GetCouponDescVoFromPo(CouponDescDao.GetCouponDesc(couponId, languageId));
        }

        public void PickCustomerCoupon(string couponCode, int customerId, int currency, int country, int language)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var coupon = GetCouponVoFromPo(CouponDao.GetCoupon(couponCode));
            if (coupon.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPON_NOT_EXIST);
            }
            //if (!coupon.CurrencyIds.Split(',').Contains(currency.ToString(CultureInfo.InvariantCulture)))
            //{
            //    throw new BussinessException(ERROR_CURRENCY_CANT_PICK);
            //}
            if (!(coupon.CountryIds.Split(',').Contains(country.ToString(CultureInfo.InvariantCulture)) || coupon.CountryIds.Contains("All")))
            {
                throw new BussinessException(ERROR_COUNTRY_CANT_PICK);
            }
            if (!(coupon.LanguageIds.Split(',').Contains(language.ToString(CultureInfo.InvariantCulture)) || coupon.LanguageIds.Contains("All")))
            {
                throw new BussinessException(ERROR_LANGUAGE_CANT_PICK);
            }
            if (CouponCustomerDao.GetCouponCustomers(customerId, couponCode).Count > 0)
            {
                throw new BussinessException(ERROR_COUPON_HAS_EXPIRED);
            }
            DateTime pickDateTime = DateTime.Now;
            var couponCustomer = new CouponCustomer
            {
                CouponId = coupon.CouponId,
                CouponCode = coupon.CouponCode,
                CustomerId = customerId,
                Amount = coupon.Amount,
                AmountCurrencyId = coupon.AmountCurrencyId,
                AmountType = coupon.AmountType,
                MinAmount = coupon.MinAmount,
                MinAmountCurrencyId = coupon.MinAmountCurrencyId,
                LanguageIds = coupon.LanguageIds,
                CountryIds = coupon.CountryIds,
                Status = CouponStatus.NotUsed,
                CurrencyIds = coupon.CurrencyIds,
                LimitBeginTime = coupon.LimitType == LimitType.BeginEnd ? coupon.LimitBeginTime.Value : pickDateTime,
                LimitEndTime = coupon.LimitType == LimitType.BeginEnd ? coupon.LimitEndTime.Value : pickDateTime.AddDays(coupon.LimitDay.Value)
            };
            CouponCustomerDao.AddObject(GetCouponCustomerPoFromVo(couponCustomer));
        }

        public void PickCustomerCouponRep(string couponCode, int customerId, int currency, int country, int language)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var coupon = GetCouponVoFromPo(CouponDao.GetCoupon(couponCode));
            if (coupon.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPON_NOT_EXIST);
            }
            //if (!coupon.CurrencyIds.Split(',').Contains(currency.ToString(CultureInfo.InvariantCulture)))
            //{
            //    throw new BussinessException(ERROR_CURRENCY_CANT_PICK);
            //}
            if (!(coupon.CountryIds.Split(',').Contains(country.ToString(CultureInfo.InvariantCulture)) || coupon.CountryIds.Contains("All")))
            {
                throw new BussinessException(ERROR_COUNTRY_CANT_PICK);
            }
            if (!(coupon.LanguageIds.Split(',').Contains(language.ToString(CultureInfo.InvariantCulture)) || coupon.LanguageIds.Contains("All")))
            {
                throw new BussinessException(ERROR_LANGUAGE_CANT_PICK);
            }
            DateTime pickDateTime = DateTime.Now;
            var couponCustomer = new CouponCustomer
            {
                CouponId = coupon.CouponId,
                CouponCode = coupon.CouponCode,
                CustomerId = customerId,
                Amount = coupon.Amount,
                AmountCurrencyId = coupon.AmountCurrencyId,
                AmountType = coupon.AmountType,
                MinAmount = coupon.MinAmount,
                MinAmountCurrencyId = coupon.MinAmountCurrencyId,
                LanguageIds = coupon.LanguageIds,
                CountryIds = coupon.CountryIds,
                Status = CouponStatus.NotUsed,
                CurrencyIds = coupon.CurrencyIds,
                LimitBeginTime = coupon.LimitType == LimitType.BeginEnd ? coupon.LimitBeginTime.Value : pickDateTime,
                LimitEndTime = coupon.LimitType == LimitType.BeginEnd ? coupon.LimitEndTime.Value : pickDateTime.AddDays(coupon.LimitDay.Value)
            };
            CouponCustomerDao.AddObject(GetCouponCustomerPoFromVo(couponCustomer));
        }

        public IList<CouponCustomer> GetCouponCustomer(int customerId, CouponStatus couponStatus, CouponMarketingRewardType couponMarketingRewardType)
        {
            IList<CouponCustomer> couponCustomer = null;
            var marketingCouponPo = MarketingCouponDao.GetMarketingCoupon((int)couponMarketingRewardType);
            if (!marketingCouponPo.IsNullOrEmpty())
            {
                string couponCode = marketingCouponPo.CouponCode;
                couponCustomer = CouponCustomerDao.GetCouponCustomers(customerId, couponCode, (int)couponStatus).Select(GetCouponCustomerVoFromPo).ToList();
            }
            return couponCustomer;
        }

        public IList<CouponCustomer> GetUsableCoupons(int customerId, Dictionary<AmountType, decimal> amounts, int country, int currency, int language)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var couponCustomers = CouponCustomerDao.GetCouponCustomers(customerId);
            //foreach (var amount in amounts)
            //{
            //    couponCustomers = couponCustomers.Where(m => m.AmountType == (int)amount.Key && m.Amount >= amount.Value).ToList();
            //}

            var currencies = ServiceFactory.ConfigureService.GetAllCurrencies();
            var amountCurrencyMap =
                currencies.Select(c => new KeyValuePair<int, IDictionary<AmountType, decimal>>(c.CurrencyId,
                            amounts.Select(k => new KeyValuePair<AmountType, decimal>(k.Key, ImplToolHelper.GetRoundValue(k.Value * c.ExchangeRate, c.DecimalPlaces))).ToDictionary())
                            ).ToDictionary();

            var pos = new List<CouponCustomerPo>();
            foreach (var po in couponCustomers)
            {
                var amountCurrency = amountCurrencyMap[po.MinAmountCurrency.Value];
                var useCountryIds = (po.UseCountryId.IsNullOrEmpty() ? "" : po.UseCountryId).Split(new[] { ',', ';', '|' });
                var useCurrencyCode = (po.UseCurrencyCode.IsNullOrEmpty() ? "" : po.UseCurrencyCode).Split(new[] { ',', ';', '|' });
                var useLanguageId = (po.UseLanguageId.IsNullOrEmpty() ? "" : po.UseLanguageId).Split(new[] { ',', ';', '|' });

                if (amountCurrency != null
                    //国家判断
                    && (useCountryIds.IsNullOrEmpty() || useCountryIds.Any(c => c.Equals("*")) || useCountryIds.Any(c => string.Equals(c, "ALl", StringComparison.InvariantCultureIgnoreCase)) || useCountryIds.Any(c => string.Equals(c, country.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    //币种判断
                    && (useCurrencyCode.IsNullOrEmpty() || useCurrencyCode.Any(c => c.Equals("*")) || useCurrencyCode.Any(c => string.Equals(c, "ALl", StringComparison.InvariantCultureIgnoreCase)) || useCurrencyCode.Any(c => string.Equals(c, currency.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    //语种判断
                    && (useLanguageId.IsNullOrEmpty() || useLanguageId.Any(c => c.Equals("*")) || useLanguageId.Any(c => string.Equals(c, "ALl", StringComparison.InvariantCultureIgnoreCase)) || useLanguageId.Any(c => string.Equals(c, language.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    //最小使用金额判断
                    && (amountCurrency.Any(c => (int)c.Key == po.AmountType.Value && c.Value >= po.MinAmount.Value)) && po.Status == (int)CouponStatus.NotUsed
                    )
                {
                    pos.Add(po);
                }
            }

            //couponCustomers = amounts.Aggregate(couponCustomers, (current, amount) => current.Where(m => m.AmountType == (int)amountCurrencyMap[m.MinAmountCurrency].Key && m.Amount >= amount.Value).ToList());
            //couponCustomers = couponCustomers.Where(m => m.UseCountryId.Split(',').Contains(country.ToString(CultureInfo.InvariantCulture))).ToList();
            //couponCustomers = couponCustomers.Where(m => m.UseCurrencyCode.Split(',').Contains(currency.ToString(CultureInfo.InvariantCulture))).ToList();
            //couponCustomers = couponCustomers.Where(m => m.UseLanguageId.Split(',').Contains(language.ToString(CultureInfo.InvariantCulture))).ToList();

            return pos.Select(GetCouponCustomerVoFromPo).ToList();
        }

        public bool IsCouponUsable(int customerCouponId, int customerId, Dictionary<AmountType, decimal> amounts, int country, int currency, int language)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var couponCustomer = GetCouponCustomerVoFromPo(CouponCustomerDao.GetObject(customerCouponId));
            if (couponCustomer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_COUPONCUSTOMER_NOT_EXIST);
            }
            if (customerId != couponCustomer.CustomerId)
            {
                throw new BussinessException(ERROR_CUSTOMER_CANTUSE_COUPON);
            }
            decimal amount = amounts.First(m => m.Key == couponCustomer.AmountType).Value;
            if (couponCustomer.Amount > amount)
            {
                throw new BussinessException(ERROR_AMOUNT_LOW);
            }
            if (!couponCustomer.CurrencyIds.Split(',').Contains(currency.ToString(CultureInfo.InvariantCulture)))
            {
                throw new BussinessException(ERROR_CURRENCY_CANT_PICK);
            }
            if (!couponCustomer.CountryIds.Split(',').Contains(country.ToString(CultureInfo.InvariantCulture)))
            {
                throw new BussinessException(ERROR_COUNTRY_CANT_PICK);
            }
            if (!couponCustomer.LanguageIds.Split(',').Contains(language.ToString(CultureInfo.InvariantCulture)))
            {
                throw new BussinessException(ERROR_LANGUAGE_CANT_PICK);
            }
            return true;
        }

        public void UseCoupon(int customerCouponId, int customerId, int orderId, Dictionary<AmountType, decimal> amounts, int country, int currency,
            int language)
        {
            if (IsCouponUsable(customerCouponId, customerId, amounts, country, currency, language))
            {
                CouponCustomerDao.UpdateCouponCustomer(customerCouponId, DateTime.Now, orderId, (int)CouponStatus.Used);
            }
        }


        public void CloseCustomerCoupon(int customerCouponId, int adminId, string reason)
        {
            CouponCustomerDao.UpdateCouponCustomer(customerCouponId, adminId, (int)CouponStatus.Close, reason, DateTime.Now);
        }

        public void StartCustomerCoupon(int customerCouponId, int adminId, DateTime enDateTime)
        {
            var couponCustomerPo = CouponCustomerDao.GetObject(customerCouponId);
            if (couponCustomerPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_COUPONCUSTOMER_NOT_EXIST);
            if (couponCustomerPo.UseDateStarted >= enDateTime)
                throw new BussinessException(ERROR_BEGINTIME_GREATER_ENDTIME);
            CouponCustomerDao.UpdateCouponCustomer(customerCouponId, adminId, (int)CouponStatus.NotUsed, enDateTime);
        }

        public void UseCustomerCoupon(int customerCouponId, int adminId, string reason)
        {
            CouponCustomerDao.UpdateCouponCustomer(customerCouponId, adminId, (int)CouponStatus.MarketingUsed, reason, DateTime.Now);
        }

        public CouponCustomer GetCustomerCoupon(int customerCouponId)
        {
            return GetCouponCustomerVoFromPo(CouponCustomerDao.GetObject(customerCouponId));
        }

        public CouponCustomer GetNewestExpiryCustomerCoupon(int customerId)
        {
            return GetCouponCustomerVoFromPo(CouponCustomerDao.GetNewsExpireCouponCustomer(customerId, (int)CouponStatus.NotUsed));
        }

        public CouponCustomerView GetCustomerCouponView(int customerCouponId)
        {
            return GetCouponCustomerViewVoFromPo(CouponCustomerViewDao.GetObject(customerCouponId));
        }

        public IList<CouponCustomer> GetCustomerCoupons(int customerId)
        {
            return CouponCustomerDao.GetCouponCustomers(customerId).Select(GetCouponCustomerVoFromPo).ToList();
        }

        public PageData<CouponCustomer> FindAllCustomerCoupon(int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CouponCustomerPo C");
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        //todo 查询条件
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        //todo 排序条件
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.Id", false);
            }

            var pageDataPo = CouponCustomerDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<CouponCustomer>();
            var voList = pageDataPo.Data.Select(GetCouponCustomerVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public PageData<CouponCustomer> FindMyCustomerCoupon(int customerId, int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CouponCustomerPo C");
            hqlHelper.AddWhere("C.CustomerId", HqlOperator.Eq, "CustomerId", customerId);
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CustomerCouponSearchCriteria.Status:
                            hqlHelper.AddWhere("C.Status", HqlOperator.Eq, "Status", item.Value);
                            break;
                        case CustomerCouponSearchCriteria.ActiveCoupon:
                            hqlHelper.AddWhere("C.Status", HqlOperator.Eq, "StatusN", item.Value);
                            hqlHelper.AddWhere("C.UseDateEnded", HqlOperator.Gt, "UseDateEnded", DateTime.Now);
                            hqlHelper.AddWhere("C.UseDateStarted", HqlOperator.Lt, "UseDateStarted", DateTime.Now);
                            break;
                        case CustomerCouponSearchCriteria.InActiveCoupon:
                            hqlHelper.AddWhere(string.Format("(C.Status = {0} Or C.Status = {1} Or (C.Status = {2} And ({3} > C.UseDateEnded)))", (int)CouponStatus.Used, (int)CouponStatus.MarketingUsed, (int)CouponStatus.NotUsed, ":NowDateTime"), HqlOperator.Exp, "NowDateTime", DateTime.Now);
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
                        //todo 排序条件
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.Id", false);
            }

            var pageDataPo = CouponCustomerDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<CouponCustomer>();
            var voList = pageDataPo.Data.Select(GetCouponCustomerVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public PageData<CouponCustomerView> FindAllCustomerCouponView(int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria, IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CouponCustomerViewPo C");
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CustomerCouponSearchCriteria.CouponName:
                            hqlHelper.AddWhere("C.CouponName", HqlOperator.Like, "CouponName", item.Value);
                            break;
                        case CustomerCouponSearchCriteria.EmailId:
                            hqlHelper.AddWhere(string.Format("(C.CustomerEmail Like {0} Or C.CustomerIdStr Like {0})", ":Keyword"), HqlOperator.Exp, "Keyword", string.Format("%{0}%", item.Value));
                            break;
                        case CustomerCouponSearchCriteria.OrderCode:
                            hqlHelper.AddWhere("C.UseOrderIdStr", HqlOperator.Like, "UseOrderIdStr", item.Value);
                            break;
                        case CustomerCouponSearchCriteria.Status:
                            hqlHelper.AddWhere("C.Status", HqlOperator.Eq, "Status", item.Value);
                            break;
                        case CustomerCouponSearchCriteria.PassDue:
                            hqlHelper.AddWhere("C.Status", HqlOperator.Eq, "StatusN", (int)CouponStatus.NotUsed);
                            hqlHelper.AddWhere("C.UseDateEnded", HqlOperator.Lt, "UseDateEnded", item.Value);
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
                        case CustomerCouponSorterCriteria.LeftDay:
                            hqlHelper.AddSorter("C.LeftDay", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.Id", false);
            }

            var pageDataPo = CouponCustomerViewDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<CouponCustomerView>();
            var voList = pageDataPo.Data.Select(GetCouponCustomerViewVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public PageData<CouponCustomer> FindAllCustomerCoupon(int customer, int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CouponCustomerPo C");
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        //todo 查询条件
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        //todo 排序条件
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.Id", false);
            }

            var pageDataPo = CouponCustomerDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<CouponCustomer>();
            var voList = pageDataPo.Data.Select(GetCouponCustomerVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        #region 辅助方法

        internal static Service.Coupon.Coupon GetCouponVoFromPo(CouponPo couponPo)
        {
            Service.Coupon.Coupon coupon = null;
            if (!couponPo.IsNullOrEmpty())
            {
                coupon = new Service.Coupon.Coupon
                {
                    CouponId = couponPo.CouponId,
                    CouponName = couponPo.ChineseName,
                    CouponCode = couponPo.CouponCode,
                    Amount = couponPo.Amount,
                    AmountCurrencyId = couponPo.AmountCurrency,
                    AmountType = couponPo.AmountType,
                    CurrencyIds = couponPo.GetCurrencyCode.IsNullOrEmpty() ? "" : couponPo.GetCurrencyCode,
                    CountryIds = couponPo.GetCountryId.IsNullOrEmpty() ? "" : couponPo.GetCountryId,
                    Status = couponPo.Status,
                    LimitType = couponPo.ExpiredType,
                    LimitDay = couponPo.ExpiredDay,
                    LimitBeginTime = couponPo.UseDateStarted,
                    LimitEndTime = couponPo.UseDateEnded,
                    PickBeginTime = couponPo.GetDateStarted,
                    PickEndTime = couponPo.GetDateEnded,
                    LanguageIds = couponPo.GetLanguageId.IsNullOrEmpty() ? "" : couponPo.GetLanguageId,
                    MinAmount = couponPo.MinAmount,
                    MinAmountCurrencyId = couponPo.MinAmountCurrency,
                    AllowManualPick = couponPo.GetAble,
                    LimitCount = couponPo.GetPersonLimit
                };
            }
            return coupon;
        }

        internal static CouponPo GetCouponPoFromVo(Service.Coupon.Coupon coupon)
        {
            CouponPo couponPo = null;
            if (!coupon.IsNullOrEmpty())
            {
                couponPo = new CouponPo
                {
                    CouponId = coupon.CouponId,
                    ChineseName = coupon.CouponName,
                    CouponCode = coupon.CouponCode,
                    Amount = coupon.Amount.HasValue ? coupon.Amount.Value : 0,
                    AmountCurrency = coupon.AmountCurrencyId,
                    AmountType = coupon.AmountType,
                    GetCurrencyCode = coupon.CurrencyIds,
                    GetCountryId = coupon.CountryIds,
                    Status = coupon.Status,
                    ExpiredType = coupon.LimitType,
                    ExpiredDay = coupon.LimitDay,
                    UseDateStarted = coupon.LimitBeginTime,
                    UseDateEnded = coupon.LimitEndTime,
                    GetDateStarted = coupon.PickBeginTime,
                    GetDateEnded = coupon.PickEndTime,
                    GetLanguageId = coupon.LanguageIds,
                    MinAmount = coupon.MinAmount.HasValue ? coupon.MinAmount.Value : 0,
                    MinAmountCurrency = coupon.MinAmountCurrencyId,
                    GetAble = coupon.AllowManualPick,
                    GetPersonLimit = coupon.LimitCount.HasValue ? coupon.LimitCount.Value : 0
                };
            }
            return couponPo;
        }

        internal static CouponCustomer GetCouponCustomerVoFromPo(CouponCustomerPo couponCustomerPo)
        {
            CouponCustomer couponCustomer = null;
            if (!couponCustomerPo.IsNullOrEmpty())
            {
                couponCustomer = new CouponCustomer
                {
                    Id = couponCustomerPo.Id,
                    CouponId = couponCustomerPo.CouponId,
                    CouponCode = couponCustomerPo.CouponCode,
                    CustomerId = couponCustomerPo.CustomerId,
                    Amount = couponCustomerPo.Amount,
                    AmountCurrencyId = couponCustomerPo.AmountCurrency,
                    AmountType = couponCustomerPo.AmountType == null ? AmountType.TotalAmount : (AmountType)couponCustomerPo.AmountType,
                    MinAmount = couponCustomerPo.MinAmount,
                    MinAmountCurrencyId = couponCustomerPo.MinAmountCurrency,
                    LanguageIds = couponCustomerPo.UseLanguageId,
                    CountryIds = couponCustomerPo.UseCountryId,
                    CurrencyIds = couponCustomerPo.UseCurrencyCode,
                    LimitBeginTime = couponCustomerPo.UseDateStarted,
                    LimitEndTime = couponCustomerPo.UseDateEnded,
                    OrderId = couponCustomerPo.UseOrderId,
                    UseTime = couponCustomerPo.DateUsed,
                    UseDescribe = couponCustomerPo.UseDesc,
                    SourceType = couponCustomerPo.Source,
                    SourceDescribe = couponCustomerPo.SourceDesc,
                    Status = (CouponStatus)couponCustomerPo.Status,
                    Reason = couponCustomerPo.UpdateReason,
                    DateDisabled = couponCustomerPo.DateDisabled,
                    AdminId = couponCustomerPo.AdminId,
                    CouponDescs = ServiceFactory.CouponService.GetCouponDesc(couponCustomerPo.CouponId),
                };
            }
            return couponCustomer;
        }

        internal static CouponCustomerView GetCouponCustomerViewVoFromPo(CouponCustomerViewPo couponCustomerPo)
        {
            CouponCustomerView couponCustomer = null;
            if (!couponCustomerPo.IsNullOrEmpty())
            {
                couponCustomer = new CouponCustomerView
                {
                    Id = couponCustomerPo.Id,
                    CouponId = couponCustomerPo.CouponId,
                    CouponCode = couponCustomerPo.CouponCode,
                    CustomerId = couponCustomerPo.CustomerId,
                    Amount = couponCustomerPo.Amount,
                    AmountCurrencyId = couponCustomerPo.AmountCurrency,
                    AmountType = couponCustomerPo.AmountType == null ? AmountType.TotalAmount : (AmountType)couponCustomerPo.AmountType,
                    MinAmount = couponCustomerPo.MinAmount,
                    MinAmountCurrencyId = couponCustomerPo.MinAmountCurrency,
                    LanguageIds = couponCustomerPo.UseLanguageId,
                    CountryIds = couponCustomerPo.UseCountryId,
                    CurrencyIds = couponCustomerPo.UseCurrencyCode,
                    LimitBeginTime = couponCustomerPo.UseDateStarted,
                    LimitEndTime = couponCustomerPo.UseDateEnded,
                    OrderCode = couponCustomerPo.UseOrderId,
                    UseTime = couponCustomerPo.DateUsed,
                    UseDescribe = couponCustomerPo.UseDesc,
                    SourceType = couponCustomerPo.Source,
                    SourceDescribe = couponCustomerPo.SourceDesc,
                    Status = (CouponStatus)couponCustomerPo.Status,
                    Reason = couponCustomerPo.UpdateReason,
                    DateDisabled = couponCustomerPo.DateDisabled,
                    AdminId = couponCustomerPo.AdminId,
                    CustomerIdStr = couponCustomerPo.CustomerIdStr,
                    UseOrderIdStr = couponCustomerPo.UseOrderIdStr,
                    CustomerName = couponCustomerPo.CustomerName,
                    CustomerEmail = couponCustomerPo.CustomerEmail,
                    LeftDay = couponCustomerPo.LeftDay,
                    CouponName = couponCustomerPo.CouponName,
                    AdminName = couponCustomerPo.AdminName
                };
            }
            return couponCustomer;
        }

        internal static CouponCustomerPo GetCouponCustomerPoFromVo(CouponCustomer couponCustomer)
        {
            CouponCustomerPo couponCustomerPo = null;
            if (!couponCustomer.IsNullOrEmpty())
            {
                couponCustomerPo = new CouponCustomerPo
                {
                    CouponId = couponCustomer.CouponId,
                    CouponCode = couponCustomer.CouponCode,
                    CustomerId = couponCustomer.CustomerId,
                    Amount = couponCustomer.Amount,
                    AmountCurrency = couponCustomer.AmountCurrencyId,
                    AmountType = couponCustomer.AmountType == null ? (int)AmountType.TotalAmount : (int)couponCustomer.AmountType,
                    MinAmount = couponCustomer.MinAmount,
                    MinAmountCurrency = couponCustomer.MinAmountCurrencyId,
                    UseLanguageId = couponCustomer.LanguageIds,
                    UseCountryId = couponCustomer.CountryIds,
                    UseCurrencyCode = couponCustomer.CurrencyIds,
                    UseDateStarted = couponCustomer.LimitBeginTime,
                    UseDateEnded = couponCustomer.LimitEndTime,
                    UseOrderId = couponCustomer.OrderId,
                    DateUsed = couponCustomer.UseTime,
                    UseDesc = couponCustomer.UseDescribe,
                    Source = couponCustomer.SourceType,
                    SourceDesc = couponCustomer.SourceDescribe,
                    Status = (int)couponCustomer.Status,
                    UpdateReason = couponCustomer.Reason,
                    DateDisabled = couponCustomer.DateDisabled,
                    AdminId = couponCustomer.AdminId

                };
            }
            return couponCustomerPo;
        }

        internal static CouponCustomerPo GetCouponCustomerPoFromCouponPo(CouponPo couponPo, int customerId)
        {
            CouponCustomerPo couponCustomerPo = null;
            if (!couponPo.IsNullOrEmpty())
            {
                DateTime pickDateTime = DateTime.Now;
                couponCustomerPo = new CouponCustomerPo
                {
                    CouponId = couponPo.CouponId,
                    CouponCode = couponPo.CouponCode,
                    CustomerId = customerId,
                    Amount = couponPo.Amount,
                    AmountCurrency = couponPo.AmountCurrency,
                    AmountType = (int?)couponPo.AmountType,
                    MinAmount = couponPo.MinAmount,
                    MinAmountCurrency = couponPo.MinAmountCurrency,
                    UseLanguageId = couponPo.GetLanguageId,
                    UseCountryId = couponPo.GetCountryId,
                    UseCurrencyCode = couponPo.GetCurrencyCode,
                    UseDateStarted = couponPo.ExpiredType == LimitType.BeginEnd ? couponPo.UseDateStarted.Value : pickDateTime,
                    UseDateEnded = couponPo.ExpiredType == LimitType.BeginEnd ? couponPo.UseDateEnded.Value : pickDateTime.AddDays(couponPo.ExpiredDay.Value),
                    UseOrderId = 0,
                    DateUsed = null,
                    UseDesc = string.Empty,
                    Source = 10,
                    SourceDesc = string.Empty,
                    Status = (int)CouponStatus.NotUsed,
                    UpdateReason = string.Empty
                };

            }
            return couponCustomerPo;
        }

        internal static CouponDesc GetCouponDescVoFromPo(CouponDescPo couponDescPo)
        {
            CouponDesc couponDesc = null;
            if (!couponDescPo.IsNullOrEmpty())
            {
                couponDesc = new CouponDesc
                {
                    Id = couponDescPo.Id,
                    CouponId = couponDescPo.CouponId,
                    LanguageId = couponDescPo.LanguageId,
                    Name = couponDescPo.Name,
                    Description = couponDescPo.Description
                };
            }
            return couponDesc;
        }

        internal static CouponDescPo GetCouponDescPoFromVo(CouponDesc couponDesc)
        {
            CouponDescPo couponDescPo = null;
            if (!couponDesc.IsNullOrEmpty())
            {
                couponDescPo = new CouponDescPo
                {
                    Id = couponDesc.Id,
                    CouponId = couponDesc.CouponId,
                    LanguageId = couponDesc.LanguageId,
                    Name = couponDesc.Name,
                    Description = couponDesc.Description
                };
            }
            return couponDescPo;
        }
        #endregion
    }
}