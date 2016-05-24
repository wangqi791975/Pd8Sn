using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Com.Panduo.Service.Payment.PayInfo;

namespace Com.Panduo.Service.Customer.Club
{
    /// <summary>
    /// Club会员服务
    /// </summary>
    public interface IClubService
    {
        #region 常量
        /// <summary>
        /// 客户不存在
        /// </summary>
        string ERROR_CUSTOMER_NOT_EXIST { get; }

        /// <summary>
        /// 会员费不能为0
        /// </summary>
        string ERROR_MEMBERSHIPFEE_CANT_ZERO { get; }
        /// <summary>
        /// 要支付会员费的客户与paypal返回的客户不一致
        /// </summary>
        string ERROR_PAYMENT_CUSTOMER_NOT_SAME_AS_PAYPAL { get; }
        /// <summary>
        /// 支付币种错误
        /// </summary>
        string ERROR_PAYMENT_PAY_CURRENCY_ERROR { get; }
        /// <summary>
        /// 支付金额错误
        /// </summary>
        string ERROR_PAYMENT_PAY_AMOUNT_ERROR { get; }
        /// <summary>
        /// 支付状态错误
        /// </summary>
        string ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR { get; }
        /// <summary>
        /// 交易号重复
        /// </summary>
        string ERROR_PAYMENT_PAYPAL_DUPLICATE { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 获取会员费
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>会员费</returns>
        ClubShippingFee GetClubShippingFee(int customerId);

        /// <summary>
        /// 通过Paypal支付加入Club会员
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="paypalInfo"></param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>  
        /// <value>ERROR_PAYMENT_CUSTOMER_NOT_SAME_AS_PAYPAL:要支付会员费的客户与paypal返回的客户不一致</value>
        /// <value>ERROR_PAYMENT_PAY_CURRENCY_ERROR:支付币种错误</value>
        /// <value>ERROR_PAYMENT_PAY_AMOUNT_ERROR:支付金额错误</value>
        /// <value>ERROR_PAYMENT_PAYPAL_PAY_STATUS_ERROR:支付状态错误</value>
        /// <value>ERROR_PAYMENT_PAYPAL_DUPLICATE:paypal交易号重复</value>
        /// </exception>
        void JoinClubByPaypal(int customerId, PaypalInfo paypalInfo);

        /// <summary>
        /// 添加Club会员
        /// </summary>
        /// <param name="clubCustomer">club会员</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        /// <returns>返回新添加club会员Id</returns>
        int AddClub(ClubCustomer clubCustomer);

        /// <summary>
        /// 获取会员等级
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        /// <returns>会员等级</returns>
        ClubType GetClubLevel(int customerId);

        /// <summary>
        /// 确认支付状态
        /// </summary>
        /// <param name="clubCustomerId"></param>
        /// <param name="adminId"></param>
        void ConfirmPaymentStatus(int clubCustomerId, int adminId);

        /// <summary>
        /// 批量确认支付状态
        /// </summary>
        /// <param name="clubCustomerIds"></param>
        /// <param name="adminId"></param>
        void ConfirmPaymentStatus(List<int> clubCustomerIds, int adminId);

        /// <summary>
        /// 通过Id获取club会员
        /// </summary>
        /// <param name="clubCustomerId">会员Id</param>
        /// <returns>club会员</returns>
        ClubCustomer GetClubCustomer(int clubCustomerId);

        /// <summary>
        /// 通过Id获取club会员
        /// </summary>
        /// <param name="clubCustomerId">会员Id</param>
        /// <returns>club会员</returns>
        ClubCustomerView GetClubCustomerView(int clubCustomerId);

        /// <summary>
        /// 通过客户Id获取club会员
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>club会员</returns>
        ClubCustomer GetClubByCustomerId(int customerId);

        /// <summary>
        /// 通过客户Id获取有效club会员
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ClubCustomer GetValidClubByCustomerId(int customerId);

        /// <summary>
        /// 查询club会员
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<ClubCustomer> FindClubCustomer(int currentPage, int pageSize, IDictionary<ClubCustomerSearchCriteria, object> searchCriteria, IList<Sorter<ClubCustomerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 查询club会员
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<ClubCustomerView> FindClubCustomerView(int currentPage, int pageSize, IDictionary<ClubCustomerSearchCriteria, object> searchCriteria, IList<Sorter<ClubCustomerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 设置club会员客服
        /// </summary>
        /// <param name="clubCustomerId"></param>
        /// <param name="customerManagerId"></param>
        /// <param name="adminId"></param>
        void SetClubCustomerManager(int clubCustomerId, int customerManagerId, int adminId);

        /// <summary>
        /// 批量设置club会员客服
        /// </summary>
        /// <param name="clubCustomerIds"></param>
        /// <param name="customerManagerId"></param>
        /// <param name="adminId"></param>
        void SetClubCustomerManager(List<int> clubCustomerIds, int customerManagerId, int adminId);

        /// <summary>
        /// 获取所有club客户黑名单
        /// </summary>
        /// <returns></returns>
        IList<ClubBlackList> GetAllClubBlackList();

        /// <summary>
        /// 通过客户邮箱获取club黑名单
        /// </summary>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        ClubBlackList GetClubBlackList(string customerEmail);

        /// <summary>
        /// 设置club黑名单
        /// </summary>
        /// <param name="clubBlackLists">club黑名单</param>
        void SetClubBlackList(List<ClubBlackList> clubBlackLists);

        /// <summary>
        /// 删除黑名单
        /// </summary>
        /// <param name="customerEmail">客户邮箱</param>
        void DeleteClubBlackList(string customerEmail);

        #endregion
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public enum ClubCustomerSearchCriteria
    {
        /// <summary>
        /// 客户邮箱
        /// </summary>
        CustomerEmail,
        /// <summary>
        /// 客服经理
        /// </summary>
        ClubManager
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public enum ClubCustomerSorterCriteria
    {
    }
}