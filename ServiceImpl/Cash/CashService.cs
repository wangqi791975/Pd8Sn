using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Cash;
using Com.Panduo.Entity.Order;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Payment;
using Com.Panduo.ServiceImpl.Cash.Dao;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl.Customer;
using Com.Panduo.ServiceImpl.Order.Dao;

namespace Com.Panduo.ServiceImpl.Cash
{
    public class CashService : ICashService
    {
        public string ERROR_CASHACCOUNT_IS_EXIST
        {
            get { return "ERROR_CASHACCOUNT_IS_EXIST"; }
        }

        public string ERROR_CASHACCOUNT_NOT_EXIST
        {
            get { return "ERROR_CASHACCOUNT_NOT_EXIST"; }
        }

        public string ERROR_CURRENCY_NOT_EXIST
        {
            get { return "ERROR_CURRENCY_NOT_EXIST"; }
        }

        public string ERROR_AMOUNT
        {
            get { return "ERROR_AMOUNT"; }
        }

        public string ERROR_AMOUNT_NOT_ENOUGH
        {
            get { return "ERROR_AMOUNT_NOT_ENOUGH"; }
        }

        public string ERROR_ARREAR_NOT_EXIST
        {
            get { return "ERROR_ARREAR_NOT_EXIST"; }
        }

        #region IOC注入
        public ICashSectionDao CashSectionDao { private get; set; }
        public ICashAccountDao CashAccountDao { private get; set; }
        public ICashItemDao CashItemDao { private get; set; }

        public IOrderDao OrderDao { private get; set; }
        public IOrderPriceDao OrderPriceDao { private get; set; }
        #endregion

        public bool HasCashAccount(int customerId)
        {
            var cashAccountPo = CashAccountDao.GetCashAccountByCustomerId(customerId);
            if (cashAccountPo != null)
            {
                return true;
            }
            return false;
        }

        public int AddCashAccount(int customerId, string currencyCode)
        {
            var hasCashAccount = HasCashAccount(customerId);
            if (hasCashAccount)
            {
                throw new BussinessException(ERROR_CASHACCOUNT_IS_EXIST);
            }
            var cashAccount = new CashAccountPo();
            var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);
            cashAccount.CustomerId = customerId;
            cashAccount.CustomerEmail =customer.Email;
            cashAccount.Amount = 0m;
            cashAccount.CurrencyCode = currencyCode;
            cashAccount.Comment = DateTime.Now.ToString();
            return CashAccountDao.AddObject(cashAccount);
        }

        public CashAccount GetCashAccount(int customerId)
        {
            CashAccount cashAccount = new CashAccount();
            var po = CashAccountDao.GetCashAccountByCustomerId(customerId);
            ObjectHelper.CopyProperties(po, cashAccount, new string[] { });
            if (cashAccount.CustomerId <= 0) return null;
            return cashAccount;
        }

        public decimal GetCustomerBalance(int customerId)
        {
            return GetCustomerBalanceOrArrear(customerId, true);
        }

        public decimal GetCustomerArrear(int customerId)
        {
            return GetCustomerBalanceOrArrear(customerId, false);
        }


        public int Recharge(int customerId, string currencyCode, decimal amount, string comment)
        {
            return RechargeCommon(customerId, currencyCode, amount, comment, 0, false);
        }

        public int Withdrawal(int customerId, string currencyCode, decimal amount, string comment)
        {
            return WithdrawalCommon(customerId, currencyCode, amount, comment, 0, false);
        }

        public int CashPay(int customerId, int orderId, string currencyCode, decimal amount)
        {

            IList<Currency> currencies = ServiceFactory.ConfigureService.GetAllCurrencies().Where(x => x.CurrencyCode == currencyCode).ToList();
            if (currencies.Count > 0)
            {
                Currency currency = currencies[0];
                decimal balance = GetCustomerBalance(customerId);
                var amountUsd = ExchangeMoneyToUsd(amount, currency);

                //如果客户想付款的金额(已转换成美元)大于客户的账户余额(美元)，想付款的金额转换成账户余额
                if (amountUsd > balance)
                {
                    amountUsd = balance;
                }
                ////var payCashDebtAmountUsd = 0.00M;//用来归还Cash的美元金额
                ////var payCashDebtAmount = 0.00M;//用来归还Cash的金额

                ////ServiceFactory.OrderService.ReturnCustomerCashArrear(order, amountUsd, out payCashDebtAmountUsd, out payCashDebtAmount);

                ////2.3.更新订单支付信息
                //var orderCostPo = OrderPriceDao.GetOrderCostByOrderId‎(order.OrderId);
                //orderCostPo.UseCash = amountUsd;
                //orderCostPo.PaymentAmount = (orderCostPo.PaymentAmount.HasValue ? orderCostPo.PaymentAmount.Value : 0.00M) + (amountUsd);

                //OrderPriceDao.UpdateObject(orderCostPo);

                ////2.4.更新订单状态和支付状态
                //var orderCostNew = GetOrderCostVoFromPo(orderCostPo);
                //var orderPo = OrderDao.GetObject(order.OrderId);

                //orderPo.PaymentId = (int)PaymentType.Paypal;

                //if (orderCostNew.NeedToPayAmt > orderCostNew.TotalOrderAmt)
                //{
                //    orderPo.PayStatus = (int)PaidStatusType.PartPay;
                //}
                //else
                //{
                //    orderPo.PayStatus = (int)PaidStatusType.FullPay;
                //}

                //if (ServiceFactory.OrderService.IsHighRiskCustomerOrder(order))
                //{
                //    orderPo.OrderStatus = (int)OrderStatusType.UnderChecking;
                //}
                //else
                //{
                //    orderPo.OrderStatus = (int)OrderStatusType.Processing;
                //}

                //OrderDao.UpdateObject(orderPo);

                var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);
                CashAccount cashAccount = GetCashAccount(customerId);
                CashAccountPo cashAccountPo = CashAccountDao.GetObject(cashAccount.Id);
                cashAccountPo.CurrencyCode = currency.CurrencyCode;
                cashAccountPo.Amount = ExchangeMoneyByUsd(balance - amountUsd, currency);
                CashAccountDao.UpdateObject(cashAccountPo);

                CashItemPo cashItemPo = new CashItemPo();
                cashItemPo.CustomerId = customerId;
                cashItemPo.FullName = customer.FullName;
                cashItemPo.CustomerEmail = customer.Email;
                cashItemPo.AmountIn = 0m;
                cashItemPo.AmountOut = ExchangeMoneyByUsd(amountUsd, currency);
                cashItemPo.ExchangeRate = currency.ExchangeRate;
                cashItemPo.CurrencyCode = currencyCode;
                cashItemPo.AmountLeft = cashAccountPo.Amount;
                cashItemPo.OpType = (int)OperationType.Payout;
                cashItemPo.OpDate = DateTime.Now;
                cashItemPo.OpAdmin = 0;
                cashItemPo.OpAccountEmail = customer.Email;
                cashItemPo.Comment = "Cash Pay for Order:#" + orderId;

                return CashItemDao.AddObject(cashItemPo);
                
            }
            else
            {
                throw new BussinessException(ERROR_CURRENCY_NOT_EXIST);
            }


            
        }

        public int ReturnArrear(int customerId, string currencyCode, decimal amount, string comment)
        {
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_AMOUNT);
            }
            decimal arrear = GetCustomerArrear(customerId);
            if (arrear <= 0)
            {
                throw new BussinessException(ERROR_ARREAR_NOT_EXIST);
            }

            //调用充值
            return Recharge(customerId, currencyCode, amount, comment);
        }

        public Service.PageData<CashItem> FindAllCashItems(int customerId, int currentPage, int pageSize)
        {
            IDictionary<CashItemSearchCriteria, object> searchCriteria = new Dictionary<CashItemSearchCriteria, object>
            {
                {CashItemSearchCriteria.CustomerId, customerId}
            };
            var list = CashItemDao.FindAllCashItems(currentPage, pageSize, searchCriteria, null);
            PageData<CashItem> pageData = new PageData<CashItem>();
            pageData.Data = list.Data.Select(x => GetCashItemFromPo((CashItemPo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        public IList<CashSection> GetAllCashSections()
        {
            var list = CashSectionDao.GetAll();
            return list.Select(x => GetCashSectionFromPo(x)).ToList();
        }

        public void UpdateCashSection(CashSection cashSection)
        {
            CashSectionDao.UpdateObject(GetCashSectionFromVo(cashSection));
        }

        public int AdminRecharge(int customerId, string currencyCode, decimal amount, string comment, int adminId, bool nofiyCustomer)
        {
            return RechargeCommon(customerId, currencyCode, amount, comment, adminId, nofiyCustomer);
        }

        public int AdminWithdrawal(int customerId, string currencyCode, decimal amount, string comment, int adminId, bool nofiyCustomer)
        {
            return WithdrawalCommon(customerId, currencyCode, amount, comment, adminId, nofiyCustomer);
        }

        public Service.PageData<CashItem> AdminFindAllCashItems(int currentPage, int pageSize, IDictionary<CashItemSearchCriteria, object> searchCriteria, IList<Service.Sorter<CashItemSorterCriteria>> sorterCriteria)
        {
            var list = CashItemDao.FindAllCashItems(currentPage, pageSize, searchCriteria, null);
            PageData<CashItem> pageData = new PageData<CashItem>();
            pageData.Data = list.Data.Select(x => GetCashItemFromPo((CashItemPo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        #region 公用方法
        /// <summary>
        /// 得到余额或欠款
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="positive">余额为true，欠款为false</param>
        /// <returns></returns>
        private decimal GetCustomerBalanceOrArrear(int customerId, bool positive)
        {
            decimal balance = 0m;
            CashAccount cashAccount = GetCashAccount(customerId);
            if (cashAccount != null)
            {
                if (cashAccount.CurrencyCode.Equals(ServiceFactory.ConfigureService.CURRENCY_CODE_USD) && cashAccount.Amount > 0 == positive)
                {
                    balance = cashAccount.Amount;
                }
                IList<Currency> currencies = ServiceFactory.ConfigureService.GetAllCurrencies().Where(x => x.CurrencyCode == cashAccount.CurrencyCode).ToList();
                if (currencies.Count > 0)
                {
                    Currency currency = currencies[0];
                    if (cashAccount.Amount > 0 == positive)
                    {
                        balance = ExchangeMoneyToUsd(cashAccount.Amount, currency);
                    }
                }
            }
            return Math.Abs(balance);
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="amount">金额(正值)</param>
        /// <param name="comment">备注</param>
        /// <param name="adminId">管理员ID</param>
        /// <param name="nofiyCustomer">是否邮件通知</param>
        /// <returns>Cash明细ID</returns>
        public int RechargeCommon(int customerId, string currencyCode, decimal amount, string comment, int adminId, bool nofiyCustomer)
        {
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_AMOUNT);
            }
            Currency changeCurrency = null;
            IList<Currency> changeCurrencies = ServiceFactory.ConfigureService.GetAllCurrencies().Where(x => x.CurrencyCode == currencyCode).ToList();
            if (changeCurrencies.Count > 0)
            {
                changeCurrency = changeCurrencies[0];
            }
            else
            {
                throw new BussinessException(ERROR_CURRENCY_NOT_EXIST);
            }

            CashAccount cashAccount = GetCashAccount(customerId);
            CashAccountPo cashAccountPo = CashAccountDao.GetObject(cashAccount.Id);
            if (cashAccount.Id <= 0)
            {
                //int cashAccountId = AddCashAccount(customerId, currencyCode);
                //cashAccountPo = CashAccountDao.GetObject(cashAccountPo.Id);
                //cashAccountPo = CashAccountDao.GetObject(cashAccountId);
                throw new BussinessException(ERROR_CASHACCOUNT_NOT_EXIST);
            }
            else
            {
                //ObjectHelper.CopyProperties(cashAccount, cashAccountPo, new string[] {});
                //cashAccountPo.Id = cashAccount.Id;
                if (cashAccount.CurrencyCode.Equals(currencyCode))
                {
                    cashAccountPo.Amount = cashAccountPo.Amount + amount;
                }
                else
                {
                    decimal cashAmountUsd = 0m;
                    decimal targetCurrencyCashAmount = 0m;

                    IList<Currency> currencies = ServiceFactory.ConfigureService.GetAllCurrencies().Where(x => x.CurrencyCode == cashAccount.CurrencyCode).ToList();
                    if (currencies.Count > 0)
                    {
                        Currency currency = currencies[0];
                        //CashAccount里的金额转成美元
                        cashAmountUsd = ExchangeMoneyToUsd(cashAccountPo.Amount, currency);
                    }
                    else
                    {
                        throw new BussinessException(ERROR_CURRENCY_NOT_EXIST);
                    }

                    if (!changeCurrency.IsNullOrEmpty())
                    {
                        //充值的金额转成美元
                        decimal changeCashAmountUsd = ExchangeMoneyToUsd(amount, changeCurrency);

                        decimal cashAmountFinalUsd = cashAmountUsd + changeCashAmountUsd;
                        targetCurrencyCashAmount = ExchangeMoneyByUsd(cashAmountFinalUsd, changeCurrency);
                    }
                    cashAccountPo.Amount = targetCurrencyCashAmount;
                }
                cashAccountPo.CurrencyCode = currencyCode;

                CashAccountDao.UpdateObject(cashAccountPo);
            }

            CashItemPo cashItemPo = new CashItemPo();
            var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);
            var admin = ServiceFactory.AdminUserService.GetAdminUser(adminId);
            cashItemPo.CustomerId = customerId;
            cashItemPo.FullName = customer.FullName;
            cashItemPo.CustomerEmail = customer.Email;
            cashItemPo.AmountIn = amount;
            cashItemPo.AmountOut = 0m;
            cashItemPo.ExchangeRate = changeCurrency.ExchangeRate;
            cashItemPo.CurrencyCode = currencyCode;
            cashItemPo.AmountLeft = cashAccountPo.Amount;
            cashItemPo.OpType = (int)OperationType.Income;
            cashItemPo.OpDate = DateTime.Now;
            cashItemPo.OpAdmin = adminId;
            cashItemPo.OpAccountEmail = admin.Name;
            cashItemPo.Comment = comment;
            if (nofiyCustomer)
            {
                //todo 发送邮件
            }

            return CashItemDao.AddObject(cashItemPo);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="amount">金额(正值)</param>
        /// <param name="comment">备注</param>
        /// <param name="adminId">管理员ID</param>
        /// <param name="nofiyCustomer">是否邮件通知</param>
        /// <returns>Cash明细ID</returns>
        public int WithdrawalCommon(int customerId, string currencyCode, decimal amount, string comment, int adminId, bool nofiyCustomer)
        {
            if (amount <= 0)
            {
                throw new BussinessException(ERROR_AMOUNT);
            }
            Currency changeCurrency = null;
            IList<Currency> changeCurrencies = ServiceFactory.ConfigureService.GetAllCurrencies().Where(x => x.CurrencyCode == currencyCode).ToList();
            if (changeCurrencies.Count > 0)
            {
                changeCurrency = changeCurrencies[0];
            }
            else
            {
                throw new BussinessException(ERROR_CURRENCY_NOT_EXIST);
            }

            CashAccount cashAccount = GetCashAccount(customerId);
            CashAccountPo cashAccountPo = CashAccountDao.GetObject(cashAccount.Id);
            if (cashAccount.Id <= 0)
            {
                throw new BussinessException(ERROR_CASHACCOUNT_NOT_EXIST);
            }
            else
            {
                if (cashAccount.CurrencyCode.Equals(currencyCode))
                {
                    if (amount > cashAccountPo.Amount)
                    {
                        throw new BussinessException(ERROR_AMOUNT_NOT_ENOUGH);
                    }
                    cashAccountPo.Amount = cashAccountPo.Amount - amount;
                }
                else
                {
                    decimal cashAmountUsd = 0m;
                    decimal targetCurrencyCashAmount = 0m;

                    IList<Currency> currencies = ServiceFactory.ConfigureService.GetAllCurrencies().Where(x => x.CurrencyCode == cashAccount.CurrencyCode).ToList();
                    if (currencies.Count > 0)
                    {
                        Currency currency = currencies[0];
                        //CashAccount里的金额转成美元
                        cashAmountUsd = ExchangeMoneyToUsd(cashAccountPo.Amount, currency);
                    }
                    else
                    {
                        throw new BussinessException(ERROR_CURRENCY_NOT_EXIST);
                    }

                    if (!changeCurrency.IsNullOrEmpty())
                    {
                        //提现的金额转成美元
                        decimal changeCashAmountUsd = ExchangeMoneyToUsd(amount, changeCurrency);

                        decimal cashAmountFinalUsd = cashAmountUsd - changeCashAmountUsd;
                        if (cashAmountFinalUsd < 0)
                        {
                            throw new BussinessException(ERROR_AMOUNT_NOT_ENOUGH);
                        }
                        targetCurrencyCashAmount = ExchangeMoneyByUsd(cashAmountFinalUsd, changeCurrency);
                    }
                    cashAccountPo.Amount = targetCurrencyCashAmount;
                }
                cashAccountPo.CurrencyCode = currencyCode;

                CashAccountDao.UpdateObject(cashAccountPo);
            }

            CashItemPo cashItemPo = new CashItemPo();
            var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);
            var admin = ServiceFactory.AdminUserService.GetAdminUser(adminId);
            cashItemPo.CustomerId = customerId;
            cashItemPo.FullName = customer.FullName;
            cashItemPo.CustomerEmail = customer.Email;
            cashItemPo.AmountIn = 0m;
            cashItemPo.AmountOut = amount;
            cashItemPo.ExchangeRate = changeCurrency.ExchangeRate;
            cashItemPo.CurrencyCode = currencyCode;
            cashItemPo.AmountLeft = cashAccountPo.Amount;
            cashItemPo.OpType = (int)OperationType.Income;
            cashItemPo.OpDate = DateTime.Now;
            cashItemPo.OpAdmin = adminId;
            cashItemPo.OpAccountEmail = admin.Name;
            cashItemPo.Comment = comment;
            if (nofiyCustomer)
            {
                //todo 发送邮件
            }

            return CashItemDao.AddObject(cashItemPo);
        }

        #endregion

        #region 非接口方法
        /// <summary>
        /// 兑换货币
        /// </summary>
        /// <param name="moneyOfUsd">美元金额</param>
        /// <param name="targetCurrency">目标币种</param>
        /// <returns></returns>
        public decimal ExchangeMoneyByUsd(decimal moneyOfUsd, Currency targetCurrency)
        {
            return GetRoundValue(moneyOfUsd * targetCurrency.ExchangeRate, targetCurrency.DecimalPlaces);
        }

        /// <summary>
        /// 转换成美元
        /// </summary>
        /// <param name="money"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public decimal ExchangeMoneyToUsd(decimal money, Currency currency)
        {
            return GetRoundValue(money / currency.ExchangeRate, currency.DecimalPlaces);
        }

        private decimal GetRoundValue(decimal d, int decimals)
        {
            return Math.Round(d, decimals);
        }

        internal static CashSection GetCashSectionFromPo(CashSectionPo cashSectionPo)
        {
            CashSection cashSection = new CashSection();
            if (!cashSectionPo.IsNullOrEmpty())
            {
                ObjectHelper.CopyProperties(cashSectionPo, cashSection, new string[] { });
            }
            return cashSection;
        }

        internal static CashSectionPo GetCashSectionFromVo(CashSection cashSection)
        {
            CashSectionPo cashSectionPo = new CashSectionPo();
            if (!cashSection.IsNullOrEmpty())
            {
                ObjectHelper.CopyProperties(cashSection, cashSectionPo, new string[] { });
            }
            return cashSectionPo;
        }


        internal static CashItem GetCashItemFromPo(CashItemPo cashItemPo)
        {
            CashItem cashItem = new CashItem();
            if (!cashItemPo.IsNullOrEmpty())
            {
                ObjectHelper.CopyProperties(cashItemPo, cashItem, new string[] {});
            }
            return cashItem;
        }
        
        #endregion

    }
}
