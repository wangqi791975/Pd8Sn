using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Payment;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Payment.PayConfig;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.ServiceImpl.Order.Dao;
using Com.Panduo.ServiceImpl.Payment.Dao;

namespace Com.Panduo.ServiceImpl.Payment
{
    /// <summary>
    /// 支付配置接口实现
    /// </summary>
    public class PaymentService : IPaymentService
    {
        #region IOC
        public IOrderPaymentLogDao OrderPaymentLogDao { private get; set; }
        public IPaymentLogBankOfChinaDao PaymentLogBankOfChinaDao { private get; set; }
        public IPaymentLogGcDao PaymentLogGcDao { private get; set; }

        public IGlobalCollectDisabledCountryDao GlobalCollectDisabledCountryDao { private get; set; }
        public IPaymentLogHsbcDao PaymentLogHsbcDao { private get; set; }
        public IPaymentLogMoneyGramDao PaymentLogMoneyGramDao { private get; set; }
        public IPaymentLogOceanPaymentDao PaymentLogOceanPaymentDao { private get; set; }
        public IPaymentLogPaypalDao PaymentLogPaypalDao { private get; set; }
        public IPaymentLogWesternUnionDao PaymentLogWesternUnionDao { private get; set; }
        public IRandGcDao RandGcDao { private get; set; }

        public IOrderStatusHistoryDao OrderStatusHistoryDao { private get; set; }

        public IOrderService OrderService { private get; set; }

        public IPaymentEnabledCustomerDao PaymentEnabledCustomerDao { private get; set; }

        #endregion

        #region 获取支付配置
        public BankOfChinaConfig GetBankOfChinaConfig()
        {
            return PaymentConfigHelper.BankOfChinaConfig;
        }

        public HsbcConfig GetHsbcConfig()
        {
            return PaymentConfigHelper.HsbcConfig;
        }

        public WesternUnionConfig GetWesternUnionConfig()
        {
            return PaymentConfigHelper.WesternUnionConfig;
        }

        public MoneyGramConfig GetMoneyGramConfig()
        {
            return PaymentConfigHelper.MoneyGramConfig;
        }

        public PaypalConfig GetPaypalConfig()
        {
            return PaymentConfigHelper.PaypalConfig;
        }

        public PaypalExpressConfig GetPaypalExpressConfig()
        {
            return PaymentConfigHelper.PaypalExpressConfig;
        }

        public GlobalCollectConfig GetGlobalCollectConfig()
        {
            return PaymentConfigHelper.GlobalCollectConfig;
        }

        public OceanPaymentConfig GetOceanPaymentConfig()
        {
            return PaymentConfigHelper.OceanPaymentConfig;
        }
        #endregion

        #region 判断能否使用支付配置
        public bool CanUseBankOfChina(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.BankOfChinaConfig;
            if (config.IsEnable)
            {
                canUse = true;
            }

            return canUse;
        }

        public bool CanUseHsbc(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.HsbcConfig;
            if (config.IsEnable)
            {
                canUse = true;
            }

            return canUse;
        }

        public bool CanUseWesternUnion(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.WesternUnionConfig;
            if (config.IsEnable)
            {
                canUse = true;
            }

            return canUse;
        }

        public bool CanUseMoneyGram(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.MoneyGramConfig;
            if (config.IsEnable)
            {
                canUse = true;
            }

            return canUse;
        }

        public bool CanUsePaypal(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.PaypalConfig;
            if (config.IsEnable)
            {
                //todo 高危客户、国家、币种判断
                canUse = true;
            }

            return canUse;
        }

        public bool CanUsePaypalExpress(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.PaypalExpressConfig;
            if (config.IsEnable)
            {
                //todo 高危客户、国家、币种判断
                canUse = true;
            }

            return canUse;
        }

        public bool CanUseGlobalCollect(int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.GlobalCollectConfig;
            if (config.IsEnable)
            {
                if (!IsGlobalCollectDisabledCountry(countryId))
                {
                    //非GC屏蔽国家允许使用GC
                    canUse = true;
                }
            }

            return canUse;
        }

        public bool CanUseOceanPayment(string method, int customerId, int countryId, int currencyId)
        {
            var canUse = false;

            var config = PaymentConfigHelper.OceanPaymentConfig;
            if (config.MethodMap.ContainsKey(method))
            {
                var oceanPaymentMethod = config.MethodMap.Where(c => c.Key == method).Select(c => c.Value).FirstOrDefault();
                if (oceanPaymentMethod != null && oceanPaymentMethod.IsEnable)
                {
                    canUse = true;
                }
            }

            return canUse;
        }
        #endregion

        #region 支付方式屏蔽国家

        public GlobalCollectDisabledCountry GetGlobalCollectDisabledCountryById(int countryId)
        {
            return GetGlobalCollectDisabledCountryFromPo(GlobalCollectDisabledCountryDao.GetObject(countryId));
        }

        public bool IsGlobalCollectDisabledCountry(int countryId)
        {
            var country = GlobalCollectDisabledCountryDao.GetObject(countryId);
            if (country != null)
            {
                return true;
            }
            return false;
        }

        public void AddGlobalCollectDisabledCountry(GlobalCollectDisabledCountry globalCollectDisabledCountry)
        {
            var po = new GlobalCollectDisabledCountryPo();
            ObjectHelper.CopyProperties(globalCollectDisabledCountry, po, new string[] { });
            GlobalCollectDisabledCountryDao.AddObject(po);
        }

        public void DeleteGlobalCollectDisabledCountryById(int countryId)
        {
            GlobalCollectDisabledCountryDao.DeleteObjectById(countryId);
        }

        public PageData<GlobalCollectDisabledCountry> FindGlobalCollectDisabledCountries(int currentPage, int pageSize,
            IDictionary<DisabledCountrySearchCriteria, object> searchCriteria,
            IList<Sorter<DisabledCountrySorterCriteria>> sorterCriteria)
        {
            var list = GlobalCollectDisabledCountryDao.FindGlobalCollectDisabledCountries(currentPage, pageSize, searchCriteria, sorterCriteria);
            PageData<GlobalCollectDisabledCountry> pageData = new PageData<GlobalCollectDisabledCountry>();
            pageData.Data = list.Data.Select(x => GetGlobalCollectDisabledCountryFromPo(x)).ToList();
            pageData.Pager = list.Pager;
            return pageData;
        }

        #endregion

        #region 支付信息查询
        public Service.PageData<Service.Payment.PayInfo.PaypalInfo> FindPaypalInfos(int currentPage, int pageSize, IDictionary<PaypalInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<PaypalInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.PageData<Service.Payment.PayInfo.GlobalCollectInfo> FindGlobalCollectInfos(int currentPage, int pageSize, IDictionary<GlobalCollectInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<GlobalCollectInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.PageData<Service.Payment.PayInfo.OceanPaymentInfo> FindOceanPaymentInfos(int currentPage, int pageSize, IDictionary<OceanPaymentInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<OceanPaymentInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.PageData<Service.Payment.PayInfo.BankOfChinaInfo> FindBankOfChinaInfos(int currentPage, int pageSize, IDictionary<BankOfChinaInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<BankOfChinaInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.PageData<Service.Payment.PayInfo.HsbcInfo> FindHsbcInfos(int currentPage, int pageSize, IDictionary<HsbcInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<HsbcInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.PageData<Service.Payment.PayInfo.MoneyGramInfo> FindMoneyGramInfos(int currentPage, int pageSize, IDictionary<MoneyGramInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<MoneyGramInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.PageData<Service.Payment.PayInfo.WesternUnionInfo> FindWesternUnionInfos(int currentPage, int pageSize, IDictionary<WesternUnionInfoSearchCriteria, object> searchCriteria, IList<Service.Sorter<WesternUnionInfoSorterCriteria>> sorterCriteria)
        {
            throw new NotImplementedException();
        }

        public Service.Payment.PayInfo.PaypalInfo GetPaypalInfo(int id)
        {
            var po = PaymentLogPaypalDao.GetObject(id);

            return ConvertPaypalInfoVoFromPo(po);
        }

        public Service.Payment.PayInfo.GlobalCollectInfo GetGlobalCollectInfo(int id)
        {
            var po = PaymentLogGcDao.GetObject(id);

            return ConvertGlobalCollectInfoVoFromPo(po);
        }

        public Service.Payment.PayInfo.OceanPaymentInfo GetOceanPaymentInfo(int id)
        {
            var po = PaymentLogOceanPaymentDao.GetObject(id);

            return ConvertOceanPaymentInfoVoFromPo(po);
        }

        public Service.Payment.PayInfo.BankOfChinaInfo GetBankOfChinaInfo(int id)
        {
            var po = PaymentLogBankOfChinaDao.GetObject(id);

            return ConvertBankOfChinaInfoVoFromPo(po);
        }

        public Service.Payment.PayInfo.HsbcInfo GetHsbcInfo(int id)
        {
            var po = PaymentLogHsbcDao.GetObject(id);

            return ConvertHsbcInfoVoFromPo(po);
        }

        public Service.Payment.PayInfo.MoneyGramInfo GetMoneyGramInfo(int id)
        {
            var po = PaymentLogMoneyGramDao.GetObject(id);

            return ConvertMoneyGramInfoVoFromPo(po);
        }

        public Service.Payment.PayInfo.WesternUnionInfo GetWesternUnionInfo(int id)
        {
            var po = PaymentLogWesternUnionDao.GetObject(id);

            return ConvertWesternUnionInfoVoFromPo(po);
        }

        public IList<Service.Payment.PayInfo.PaypalInfo> GetPaypalInfo(string orderNo)
        {
            var pos = PaymentLogPaypalDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertPaypalInfoVoFromPo(c)).ToList();

            return vos;
        }

        public IList<Service.Payment.PayInfo.GlobalCollectInfo> GetGlobalCollectInfo(string orderNo)
        {
            var pos = PaymentLogGcDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertGlobalCollectInfoVoFromPo(c)).ToList();

            return vos;
        }

        public IList<Service.Payment.PayInfo.OceanPaymentInfo> GetOceanPaymentInfo(string orderNo)
        {
            var pos = PaymentLogOceanPaymentDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertOceanPaymentInfoVoFromPo(c)).ToList();

            return vos;
        }

        public IList<Service.Payment.PayInfo.BankOfChinaInfo> GetBankOfChinaInfo(string orderNo)
        {
            var pos = PaymentLogBankOfChinaDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertBankOfChinaInfoVoFromPo(c)).ToList();

            return vos;
        }

        public IList<Service.Payment.PayInfo.HsbcInfo> GetHsbcInfo(string orderNo)
        {
            var pos = PaymentLogHsbcDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertHsbcInfoVoFromPo(c)).ToList();

            return vos;
        }

        public IList<Service.Payment.PayInfo.MoneyGramInfo> GetMoneyGramInfo(string orderNo)
        {
            var pos = PaymentLogMoneyGramDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertMoneyGramInfoVoFromPo(c)).ToList();

            return vos;
        }

        public IList<Service.Payment.PayInfo.WesternUnionInfo> GetWesternUnionInfo(string orderNo)
        {
            var pos = PaymentLogWesternUnionDao.GetPaymentLogsByOrderNo(orderNo);

            var vos = pos.Select(c => ConvertWesternUnionInfoVoFromPo(c)).ToList();

            return vos;
        }

        internal PaypalInfo ConvertPaypalInfoVoFromPo(PaymentLogPaypalPo po)
        {
            PaypalInfo vo = null;
            if (po != null)
            {
                vo = new PaypalInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "PaypalTargetType" });
                vo.PaypalTargetType = EnumHelper.ToEnum<PaypalTargetType>(po.PaypalTargetType.Value);
            }

            return vo;
        }

        internal GlobalCollectInfo ConvertGlobalCollectInfoVoFromPo(PaymentLogGcPo po)
        {
            GlobalCollectInfo vo = null;
            if (po != null)
            {
                vo = new GlobalCollectInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "GlobalCollectType", "TargetId" });
                vo.OrderId = po.TargetId.HasValue ? po.TargetId.Value : 0;
                vo.GlobalCollectType = EnumHelper.ToEnum<GlobalCollectType>(po.GlobalCollectType.Value);
            }

            return vo;
        }

        internal OceanPaymentInfo ConvertOceanPaymentInfoVoFromPo(PaymentLogOceanPaymentPo po)
        {
            OceanPaymentInfo vo = null;
            if (po != null)
            {
                vo = new OceanPaymentInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "" });
            }

            return vo;
        }

        internal BankOfChinaInfo ConvertBankOfChinaInfoVoFromPo(PaymentLogBankOfChinaPo po)
        {
            BankOfChinaInfo vo = null;
            if (po != null)
            {
                vo = new BankOfChinaInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "" });
            }

            return vo;
        }

        internal HsbcInfo ConvertHsbcInfoVoFromPo(PaymentLogHsbcPo po)
        {
            HsbcInfo vo = null;
            if (po != null)
            {
                vo = new HsbcInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "" });
            }

            return vo;
        }

        internal MoneyGramInfo ConvertMoneyGramInfoVoFromPo(PaymentLogMoneyGramPo po)
        {
            MoneyGramInfo vo = null;
            if (po != null)
            {
                vo = new MoneyGramInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "" });
            }

            return vo;
        }

        internal WesternUnionInfo ConvertWesternUnionInfoVoFromPo(PaymentLogWesternUnionPo po)
        {
            WesternUnionInfo vo = null;
            if (po != null)
            {
                vo = new WesternUnionInfo();
                ObjectHelper.CopyProperties(po, vo, new[] { "" });
            }

            return vo;
        }


        #endregion

        #region 支付辅助
        public string GenerateGcOrderNo()
        {
            var po = RandGcDao.GetOneUnUsedGcNo();

            po.Status = true;
            po.DateUsed = DateTime.Now;

            RandGcDao.UpdateObject(po);

            var gcOrderNo = string.Format("{0}{1}", DateTime.Now.ToString("yyMMdd"), po.RandValue);

            return gcOrderNo;
        }

        public bool IsCurrencyUseUsdForPaypal(string currencyCode)
        {
            return _useUsdForPaypalCurrencies.Contains(currencyCode);
        }


        private static string[] _useUsdForPaypalCurrencies = ServiceConfig.PayByPaypalUseUsd.ToUpper().Split(',', '|', ';', ':') ?? new string[] { };

        #endregion

        #region 支付方式限制客户
        public PageData<PaymentEnabledCustomer> FindPaymentEnabledCustomers(int page, int pageSize, Dictionary<PaymentEnabledCustomerSearchCriteria, object> searchCriteria,
            List<Sorter<PaymentEnabledCustomerSorterCriteria>> sorterCriteria)
        {
            var pageDataPo = PaymentEnabledCustomerDao.FindPaymentDisabledCustomerPos(page, pageSize, searchCriteria,
                sorterCriteria);
            var pageDataVo = new PageData<PaymentEnabledCustomer>();
            var voList = pageDataPo.Data.Select(PaymentEnabledCustomerPoConvertToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        private PaymentEnabledCustomer PaymentEnabledCustomerPoConvertToVo(PaymentEnabledCustomerPo po)
        {
            if (po.IsNullOrEmpty())
                return null;

            return new PaymentEnabledCustomer
            {
                Id = po.Id,
                PaymentType = po.PaymentType.ToEnum<PaymentType>(),
                CustomerId = po.CustomerId,
                CustomerEmail = po.CustomerEmail,
                DateCreated = po.DateCreated,
                AdminId = po.AdminId,
                AccountEmail = po.CustomerEmail,
            };
        }

        public void SetPaymentEnabledCustomer(PaymentEnabledCustomer paymentEnabledCustomer)
        {
            var enabledCustomerPo = PaymentEnabledCustomerDao.GetOneObject("from PaymentEnabledCustomerPo where CustomerEmail=? and PaymentType=?",
                new object[] { paymentEnabledCustomer.CustomerEmail, (int)paymentEnabledCustomer.PaymentType });

            if (enabledCustomerPo.IsNullOrEmpty())
            {
                enabledCustomerPo = new PaymentEnabledCustomerPo
                {
                    CustomerEmail = paymentEnabledCustomer.CustomerEmail,
                    PaymentType = (int)paymentEnabledCustomer.PaymentType,
                    DateCreated = paymentEnabledCustomer.DateCreated,
                    AdminId = paymentEnabledCustomer.AdminId,
                    AccountEmail = paymentEnabledCustomer.AccountEmail
                };
                enabledCustomerPo.Id = PaymentEnabledCustomerDao.AddObject(enabledCustomerPo);
            }
            else
            {
                enabledCustomerPo.CustomerEmail = paymentEnabledCustomer.CustomerEmail;
                enabledCustomerPo.PaymentType = (int)paymentEnabledCustomer.PaymentType;
                enabledCustomerPo.DateCreated = paymentEnabledCustomer.DateCreated;
                enabledCustomerPo.AdminId = paymentEnabledCustomer.AdminId;
                enabledCustomerPo.AccountEmail = paymentEnabledCustomer.AccountEmail;
                PaymentEnabledCustomerDao.UpdateObject(enabledCustomerPo);
            }
        }

        public void DeletePaymentEnabledCustomerById(int id)
        {
            var productArea = PaymentEnabledCustomerDao.GetObject(id);
            if (productArea.IsNullOrEmpty())
                throw new BussinessException("ERROR_OBJECT_NOT_EXIST");
            PaymentEnabledCustomerDao.DeleteObjectById(id);
        }

        #endregion

        #region 私有方法

        internal GlobalCollectDisabledCountry GetGlobalCollectDisabledCountryFromPo(GlobalCollectDisabledCountryPo po)
        {
            GlobalCollectDisabledCountry globalCollectDisabledCountry = null;
            if (!po.IsNullOrEmpty())
            {
                globalCollectDisabledCountry = new GlobalCollectDisabledCountry();
                ObjectHelper.CopyProperties(po, globalCollectDisabledCountry, new string[] { });
            }
            return globalCollectDisabledCountry;
        }

        #endregion
    }
}
