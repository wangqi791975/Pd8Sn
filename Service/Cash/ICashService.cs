using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Cash
{
    /// <summary>
    /// 客户Cash服务
    /// </summary>
    public interface ICashService
    {
        #region
        /// <summary>
        /// CashAccount已存在
        /// </summary>
        string ERROR_CASHACCOUNT_IS_EXIST { get; }

        /// <summary>
        /// CashAccount不存在
        /// </summary>
        string ERROR_CASHACCOUNT_NOT_EXIST { get; }

        /// <summary>
        /// 币种不存在
        /// </summary>
        string ERROR_CURRENCY_NOT_EXIST { get; }

        /// <summary>
        /// 金额错误
        /// </summary>
        string ERROR_AMOUNT { get; }

        /// <summary>
        /// 余额不足
        /// </summary>
        string ERROR_AMOUNT_NOT_ENOUGH { get; }

        /// <summary>
        /// 不存在欠款(归还欠款时)
        /// </summary>
        string ERROR_ARREAR_NOT_EXIST { get; }

        #endregion
        #region 客户
        /// <summary>
        /// 客户是否有CashAccount
        /// </summary>
        /// <returns></returns>
        bool HasCashAccount(int customerId);

        /// <summary>
        /// 创建CashAccount
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <returns>成功后的主键ID</returns>
        int AddCashAccount(int customerId, string currencyCode);

        /// <summary>
        /// 得到客户CashAccount
        /// </summary>
        /// <param name="customerId">客户ID</param>
        CashAccount GetCashAccount(int customerId);

        /// <summary>
        /// 获取客户美元余额(小于0返回0)
        /// </summary>
        /// <param name="customerId">客户ID</param>
        decimal GetCustomerBalance(int customerId);

        /// <summary>
        /// 获取客户美元欠款(如果大于0返回0，小于0返回正值)
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns>正的金额</returns>
        decimal GetCustomerArrear(int customerId);

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="amount">金额(正值)</param>
        /// <param name="comment">备注</param>
        /// <returns>Cash明细ID</returns>
        int Recharge(int customerId, string currencyCode, decimal amount, string comment);

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="amount">金额(正值)</param>
        /// <param name="comment">备注</param>
        /// <returns>Cash明细ID</returns>
        int Withdrawal(int customerId, string currencyCode, decimal amount, string comment);

        /// <summary>
        /// 支出(如订单、Club会员费)，如是订单支付请先调用OrderSevice里的JudgeOrderCanPay方法
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="amount">金额(正值)</param>
        /// <returns>Cash明细ID</returns>
        int CashPay(int customerId, int orderId, string currencyCode, decimal amount);


        /// <summary>
        /// 归还欠款
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currencyCode">币种编码</param>
        /// <param name="amount">金额(正值)</param>
        /// <param name="comment">备注</param>
        /// <returns>Cash明细ID</returns>
        int ReturnArrear(int customerId, string currencyCode, decimal amount, string comment);

        /// <summary>
        /// 获取客户的Cash明细
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>CashItem分页数据</returns>
        PageData<CashItem> FindAllCashItems(int customerId, int currentPage, int pageSize); 
        #endregion

        #region 后台

        /// <summary>
        /// 得到所有CashSection
        /// </summary>
        /// <returns></returns>
        IList<CashSection> GetAllCashSections();

        /// <summary>
        /// 修改CashSection
        /// </summary>
        /// <param name="cashSection">CashSection实体</param>
        void UpdateCashSection(CashSection cashSection);

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
        int AdminRecharge(int customerId, string currencyCode, decimal amount, string comment, int adminId, bool nofiyCustomer);

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
        int AdminWithdrawal(int customerId, string currencyCode, decimal amount, string comment, int adminId, bool nofiyCustomer);

        /// <summary>
        /// 获取所有客户的Cash明细
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">搜索关键词(客户ID、客户姓名、客户邮箱)</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>CashItem分页数据</returns>
        PageData<CashItem> AdminFindAllCashItems(int currentPage, int pageSize,  IDictionary<CashItemSearchCriteria, object> searchCriteria, IList<Sorter<CashItemSorterCriteria>> sorterCriteria); 
        #endregion
    }

    public enum CashItemSearchCriteria
    {
        CustomerId,
        Keyword
    }

    public enum CashItemSorterCriteria
    {
        OpDate
    }
}
