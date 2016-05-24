using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer.Club;
using Com.Panduo.Entity.Payment;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Customer.Club;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Payment;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl.Coupon.Dao;
using Com.Panduo.ServiceImpl.Customer.Club.Dao;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Com.Panduo.ServiceImpl.Marketing.Dao;
using Com.Panduo.ServiceImpl.Payment.Dao;
using Com.Panduo.Entity.SiteConfigure;

namespace Com.Panduo.ServiceImpl.Customer.Club
{
    public class ClubService : IClubService
    {
        public IClubCustomerDao ClubCustomerDao { private get; set; }
        public IClubCustomerViewDao ClubCustomerViewDao { private get; set; }
        public ICustomerDao CustomerDao { private get; set; }
        public IPaymentLogPaypalDao PaymentLogPaypalDao { private get; set; }
        public IMarketingCouponDao MarketingCouponDao { private get; set; }
        public ICouponDao CouponDao { private get; set; }
        public IClubFeeDao ClubFeeDao { private get; set; }
        public IClubBlackListDao ClubBlackListDao { private get; set; }

        public IConfigureService ConfigureService { private get; set; }

        public string ERROR_CUSTOMER_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_NOT_EXIST"; }
        }

        public string ERROR_MEMBERSHIPFEE_CANT_ZERO
        {
            get { return "ERROR_MEMBERSHIPFEE_CANT_ZERO"; }
        }

        public string ERROR_PAYMENT_CUSTOMER_NOT_SAME_AS_PAYPAL
        {
            get { return "ERROR_PAYMENT_CUSTOMER_NOT_SAME_AS_PAYPAL"; }
        }

        public string ERROR_PAYMENT_PAY_CURRENCY_ERROR
        {
            get { return "ERROR_PAYMENT_PAY_CURRENCY_ERROR"; }
        }

        public string ERROR_PAYMENT_PAY_AMOUNT_ERROR
        {
            get { return "ERROR_PAYMENT_PAY_AMOUNT_ERROR"; }
        }

        public string ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR
        {
            get { return "ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR"; }
        }

        public string ERROR_PAYMENT_PAYPAL_DUPLICATE
        {
            get { return "ERROR_PAYMENT_PAYPAL_DUPLICATE"; }
        }

        public ClubShippingFee GetClubShippingFee(int customerId)
        {
            var customerClubFee = ClubFeeDao.GetClubFee(customerId);
            var clubShippingFee = new ClubShippingFee
            {
                ShippingFeeBefore = ServiceFactory.ConfigureService.ShippingFeeBefore,
                HandlingFee = ServiceFactory.ConfigureService.HandlingFee,
                ShippingFeeAfter = customerClubFee.IsNullOrEmpty() ? ServiceFactory.ConfigureService.ShippingFeeAfter : customerClubFee.Fee
            };
            var marketingCoupon = MarketingCouponDao.GetMarketingCoupons((int)CouponMarketingRewardType.Club);
            if (!marketingCoupon.IsNullOrEmpty())
            {
                clubShippingFee.ExclusiveCoupon = CouponDao.GetCoupon(marketingCoupon.Last().CouponCode).Amount * 12;
            }
            else
            {
                clubShippingFee.ExclusiveCoupon = ServiceFactory.ConfigureService.ExclusiveCoupon;
            }

            return clubShippingFee;
        }

        public void JoinClubByPaypal(int customerId, PaypalInfo paypalInfo)
        {
            //1.数据验证  
            //1.1.客户验证
            if (customerId <= 0)
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }

            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo == null)
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }

            //1.2.支付信息验证
            //1.2订单号不一致
            if (customerId != paypalInfo.TargetId || !string.Equals(customerPo.CustomerEmail,paypalInfo.ItemNumber,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BussinessException(ERROR_PAYMENT_CUSTOMER_NOT_SAME_AS_PAYPAL);
            }

            //1.3币种验证
            if (string.IsNullOrEmpty(paypalInfo.McCurrency))
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            var currency = ConfigureService.GetCurrencyByCode(paypalInfo.McCurrency);
            if (currency == null)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_CURRENCY_ERROR);
            }

            //1.4支付金额错误
            var amount = paypalInfo.McGross;
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            var amountUsd = amount;
            if (!string.Equals(ConfigureService.CURRENCY_CODE_USD, paypalInfo.McCurrency, StringComparison.InvariantCultureIgnoreCase))
            {
                //其他币种转换为美元(下单时币种的汇率)
                amountUsd = ImplToolHelper.GetRoundValue(decimal.Divide(amount, currency.ExchangeRate), currency.DecimalPlaces);
            }

            if (amountUsd <= 0)
            {
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            } 

            //支付的金额小于年费金额
            var clubShippingFee = GetClubShippingFee(customerId);
            if (clubShippingFee == null || amountUsd < clubShippingFee.ShippingFeeAfter)
            { 
                throw new BussinessException(ERROR_PAYMENT_PAY_AMOUNT_ERROR);
            }

            //1.5paypal支付状态错误
            if (!paypalInfo.IsCompleted)
            {
                throw new BussinessException(ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR);
            }

            //1.6paypal重复支付
            var logPo = PaymentLogPaypalDao.GetPaymentLogByTransactionId(paypalInfo.TxnId);
            if (logPo != null)
            {
                throw new BussinessException(ERROR_PAYMENT_PAYPAL_DUPLICATE);
            }

            //2.保存数据
            //2.1.保存Paypal支付信息
            logPo = new PaymentLogPaypalPo();
            ObjectHelper.CopyProperties(paypalInfo, logPo, new[] { "TargetId", "PaypalTargetType", "IsCompleted", "PAYPAL_STATUS_COMPLETED" });
            logPo.TargetId = customerId;
            logPo.PaypalTargetType = (int)PaypalTargetType.ClubFee;
            logPo.CreateDate = DateTime.Now;

            PaymentLogPaypalDao.AddObject(logPo);

            //todo 逻辑删除这个客户之前的Club Customer信息

            var langs = ConfigureService.GetAllValidLanguage();
            var currentLanguage = langs.FirstOrDefault(c => c.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId);
            //2.2.保存Club会员信息
            var customerClubPo = new ClubCustomerPo
            {
                CustomerId = customerId,
                CustomerManagerId = currentLanguage != null && currentLanguage.CustomerManagerId.HasValue ? currentLanguage.CustomerManagerId.Value :0 ,
                Status = ClubStatus.Active,
                DateActived = DateTime.Now,
                Fee = amountUsd,
                Type = ClubType.Two,
                AddedDate = DateTime.Now,
                AddedType = 0,
                PaymentStatus = PaymentStatus.Payment,
                PayType = PaymentType.Paypal,
                PayLogId = logPo.Id,
                EndedDate = DateTime.Now.AddYears(1)
            };

            ClubCustomerDao.AddObject(customerClubPo);

            //2.3.保存customer表的等级信息
            customerPo.ClubLevel = (int)ClubType.Two;
            CustomerDao.UpdateObject(customerPo);

            //todo 是否需要发邮件？
        }

        public int AddClub(ClubCustomer clubCustomer)
        {
            return ClubCustomerDao.AddObject(GetClubCustomerPoFromVo(clubCustomer));
        }

        public ClubType GetClubLevel(int customerId)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            return ClubCustomerDao.GetClubCustomer(customerId).Type;
        }

        public void ConfirmPaymentStatus(int clubCustomerId, int adminId)
        {
            ClubCustomerDao.UpdateClubCustomer(clubCustomerId, PaymentStatus.Payment);
        }

        public void ConfirmPaymentStatus(List<int> clubCustomerIds, int adminId)
        {
            foreach (var clubCustomerId in clubCustomerIds)
            {
                ConfirmPaymentStatus(clubCustomerId, adminId);
            }
        }

        public ClubCustomer GetClubCustomer(int clubCustomerId)
        {
            return GetClubCustomerVoFromPo(ClubCustomerDao.GetObject(clubCustomerId));
        }

        public ClubCustomerView GetClubCustomerView(int clubCustomerId)
        {
            return GetClubCustomerViewVoFromPo(ClubCustomerViewDao.GetObject(clubCustomerId));
        }

        public ClubCustomer GetClubByCustomerId(int customerId)
        {
            return GetClubCustomerVoFromPo(ClubCustomerDao.GetClubCustomer(customerId));
        }

        public ClubCustomer GetValidClubByCustomerId(int customerId)
        {
            return GetClubCustomerVoFromPo(ClubCustomerDao.GetValidClubCustomer(customerId));
        }

        public PageData<ClubCustomer> FindClubCustomer(int currentPage, int pageSize, IDictionary<ClubCustomerSearchCriteria, object> searchCriteria, IList<Sorter<ClubCustomerSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM ClubCustomerPo C");
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

            var pageDataPo = ClubCustomerDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<ClubCustomer>();
            var voList = pageDataPo.Data.Select(GetClubCustomerVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public PageData<ClubCustomerView> FindClubCustomerView(int currentPage, int pageSize, IDictionary<ClubCustomerSearchCriteria, object> searchCriteria, IList<Sorter<ClubCustomerSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM ClubCustomerViewPo C");
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case ClubCustomerSearchCriteria.ClubManager:
                            hqlHelper.AddWhere("C.CustomerManagerId", HqlOperator.Eq, "CustomerManagerId", item.Value);
                            break;
                        case ClubCustomerSearchCriteria.CustomerEmail:
                            hqlHelper.AddWhere("C.CustomerEmail", HqlOperator.Like, "CustomerEmail", item.Value);
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
                hqlHelper.AddSorter("C.AddedDate", false);
                hqlHelper.AddSorter("C.Id", false);
            }

            var pageDataPo = ClubCustomerViewDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<ClubCustomerView>();
            var voList = pageDataPo.Data.Select(GetClubCustomerViewVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public void SetClubCustomerManager(int clubCustomerId, int customerManagerId, int adminId)
        {
            ClubCustomerDao.UpdateClubCustomer(clubCustomerId, customerManagerId);
        }

        public void SetClubCustomerManager(List<int> clubCustomerIds, int customerManagerId, int adminId)
        {
            foreach (var clubCustomerId in clubCustomerIds)
            {
                SetClubCustomerManager(clubCustomerId, customerManagerId, adminId);
            }
        }

        public IList<ClubBlackList> GetAllClubBlackList()
        {
            return ClubBlackListDao.GetAll().Select(GetClubBlackListVoFromPo).ToList();
        }

        public ClubBlackList GetClubBlackList(string customerEmail)
        {
            return GetClubBlackListVoFromPo(ClubBlackListDao.GetClubBlackList(customerEmail));
        }

        public void SetClubBlackList(List<ClubBlackList> clubBlackLists)
        {
            ClubBlackListDao.AddObjects(clubBlackLists.Select(GetClubBlackPoFromVo));
        }

        public void DeleteClubBlackList(string customerEmail)
        {
            ClubBlackListDao.DeleteClubBlackList(customerEmail);
        }

        #region 辅助方法

        internal static ClubCustomer GetClubCustomerVoFromPo(ClubCustomerPo clubCustomerPo)
        {
            ClubCustomer clubCustomer = null;
            if (!clubCustomerPo.IsNullOrEmpty())
            {
                clubCustomer = new ClubCustomer
                {
                    ClubId = clubCustomerPo.Id,
                    CustomerId = clubCustomerPo.CustomerId,
                    CustomerManagerId = clubCustomerPo.CustomerManagerId,
                    BeginDate = clubCustomerPo.AddedDate,
                    EndDate = clubCustomerPo.EndedDate,
                    DateActived = clubCustomerPo.DateActived,
                    Fee = clubCustomerPo.Fee,
                    PayType = clubCustomerPo.PayType,
                    PayLogId = clubCustomerPo.PayLogId,
                    ClubType = clubCustomerPo.Type,
                    PaymentStatus = clubCustomerPo.PaymentStatus,
                    ClubStatus = clubCustomerPo.Status,
                    SavingShippingFee = clubCustomerPo.SavingShippingFee
                };
            }
            return clubCustomer;
        }

        internal static ClubCustomerView GetClubCustomerViewVoFromPo(ClubCustomerViewPo clubCustomerPo)
        {
            ClubCustomerView clubCustomer = null;
            if (!clubCustomerPo.IsNullOrEmpty())
            {
                clubCustomer = new ClubCustomerView
                {
                    ClubId = clubCustomerPo.Id,
                    CustomerId = clubCustomerPo.CustomerId,
                    CustomerManagerId = clubCustomerPo.CustomerManagerId,
                    BeginDate = clubCustomerPo.AddedDate,
                    EndDate = clubCustomerPo.EndedDate,
                    DateActived = clubCustomerPo.DateActived,
                    Fee = clubCustomerPo.Fee,
                    PayType = clubCustomerPo.PayType,
                    PayLogId = clubCustomerPo.PayLogId,
                    ClubType = clubCustomerPo.Type,
                    PaymentStatus = clubCustomerPo.PaymentStatus,
                    ClubStatus = clubCustomerPo.Status,
                    CustomerEmail = clubCustomerPo.CustomerEmail,
                    CustomerLevel = clubCustomerPo.CustomerLevel,
                    ManagerName = clubCustomerPo.ManagerName,
                    TransactionId = clubCustomerPo.TransactionId,
                    Website = clubCustomerPo.Website
                };
            }
            return clubCustomer;
        }

        internal static ClubCustomerPo GetClubCustomerPoFromVo(ClubCustomer clubCustomer)
        {
            ClubCustomerPo clubCustomerPo = null;
            if (!clubCustomer.IsNullOrEmpty())
            {
                clubCustomerPo = new ClubCustomerPo
                {
                    Id = clubCustomer.ClubId,
                    CustomerId = clubCustomer.CustomerId,
                    CustomerManagerId = clubCustomer.CustomerManagerId,
                    AddedDate = clubCustomer.BeginDate,
                    EndedDate = clubCustomer.EndDate,
                    DateActived = clubCustomer.DateActived,
                    Fee = clubCustomer.Fee,
                    PayType = clubCustomer.PayType,
                    PayLogId = clubCustomer.PayLogId,
                    Type = clubCustomer.ClubType,
                    PaymentStatus = clubCustomer.PaymentStatus,
                    Status = clubCustomer.ClubStatus,
                    SavingShippingFee = clubCustomer.SavingShippingFee
                };
            }
            return clubCustomerPo;
        }

        internal static ClubBlackList GetClubBlackListVoFromPo(ClubBlackListPo clubBlackListPo)
        {
            ClubBlackList clubBlackList = null;
            if (!clubBlackListPo.IsNullOrEmpty())
            {
                clubBlackList = new ClubBlackList
                {
                    Id = clubBlackListPo.Id,
                    CustomerEmail = clubBlackListPo.CustomerEmail
                };
            }
            return clubBlackList;
        }

        internal static ClubBlackListPo GetClubBlackPoFromVo(ClubBlackList clubBlackList)
        {
            ClubBlackListPo clubBlackListPo = null;
            if (!clubBlackList.IsNullOrEmpty())
            {
                clubBlackListPo = new ClubBlackListPo
                {
                    Id = clubBlackList.Id,
                    CustomerEmail = clubBlackList.CustomerEmail
                };
            }
            return clubBlackListPo;
        }
        #endregion
    }
}