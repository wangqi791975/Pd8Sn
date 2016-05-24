using System;
using System.Collections.Generic;
using Com.Panduo.Service.Marketing.Coupon;

namespace Com.Panduo.Service.Coupon
{
    /// <summary>
    /// 优惠券服务
    /// </summary>
    public interface ICouponService
    {
        #region 常量
        /// <summary>
        /// 客户不存在
        /// </summary>
        string ERROR_CUSTOMER_NOT_EXIST { get; }

        /// <summary>
        /// 优惠券面额不能为0
        /// </summary>
        string ERROR_COUPON_CANT_ZERO { get; }

        /// <summary>
        /// 优惠券不存在
        /// </summary>
        string ERROR_COUPON_NOT_EXIST { get; }

        /// <summary>
        /// 优惠券已过期
        /// </summary>
        string ERROR_COUPON_PASS_DUE { get; }

        /// <summary>
        /// 优惠券领取超过次数
        /// </summary>
        string ERROR_COUPON_GT { get; }

        /// <summary>
        /// 币种不符合coupon领取条件不能领取
        /// </summary>
        string ERROR_CURRENCY_CANT_PICK { get; }

        /// <summary>
        /// 国家不符合coupon领取条件不能领取
        /// </summary>
        string ERROR_COUNTRY_CANT_PICK { get; }

        /// <summary>
        /// 语种不符合coupon领取条件不能领取
        /// </summary>
        string ERROR_LANGUAGE_CANT_PICK { get; }

        /// <summary>
        /// 客户优惠券不存在
        /// </summary>
        string ERROR_COUPONCUSTOMER_NOT_EXIST { get; }

        /// <summary>
        /// 客户优惠券开始时间大于结束时间
        /// </summary>
        string ERROR_BEGINTIME_GREATER_ENDTIME { get; }

        /// <summary>
        /// 当前客户不能使用该优惠券（当前客户的Id和该优惠券的Id不为同一个）
        /// </summary>
        string ERROR_CUSTOMER_CANTUSE_COUPON { get; }

        /// <summary>
        /// 金额不符合coupon使用条件
        /// </summary>
        string ERROR_AMOUNT_LOW { get; }

        /// <summary>
        /// coupon编号已存在
        /// </summary>
        string ERROR_COUPON_CODE_EXIST { get; }

        /// <summary>
        /// coupon已经领取
        /// </summary>
        string ERROR_COUPON_HAS_PICK { get; }

        /// <summary>
        /// coupon已过期
        /// </summary>
        string ERROR_COUPON_HAS_EXPIRED { get; }
        #endregion

        #region 方法
        #region Coupon

        /// <summary>
        /// 创建优惠券
        /// </summary>
        /// <param name="coupon">优惠券实体</param>
        /// <param name="couponDescs"></param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_COUPON_CANT_ZERO:优惠券面额不能为0</value>
        /// </exception>
        /// <returns>返回新建优惠券的Id</returns>
        int CreateCoupon(Coupon coupon, List<CouponDesc> couponDescs);

        /// <summary>
        /// 编辑优惠券
        /// </summary>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_COUPON_CANT_ZERO:优惠券面额不能为0</value>
        ///     <value>ERROR_COUPON_NOT_EXIST:优惠券不存在</value>
        /// </exception>
        /// <param name="coupon">优惠券实体</param>
        /// <param name="couponDescs"></param>
        void EditCoupon(Coupon coupon, List<CouponDesc> couponDescs);

        /// <summary>
        /// 发送优惠券
        /// </summary>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="adminId"></param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_COUPON_NOT_EXIST:优惠券不存在</value>
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void SendCoupon(int couponId, int customerId, int adminId);

        /// <summary>
        /// 批量发送优惠券
        /// </summary>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="customerIds">客户Id</param>
        /// <param name="adminId"></param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void SendCoupon(int couponId, List<int> customerIds, int adminId);

        /// <summary>
        /// 发送优惠券
        /// </summary>
        /// <param name="couponCode">优惠券编号</param>
        /// <param name="customerEmail">客户邮箱</param>
        /// <param name="adminId"></param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_COUPON_NOT_EXIST:优惠券不存在</value>
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void SendCoupon(string couponCode, string customerEmail, int adminId);

        /// <summary>
        /// 通过CouponId删除Coupon
        /// </summary>
        /// <param name="couponId">couponId</param>
        void DeleteCoupon(int couponId);

        /// <summary>
        /// 获取所有优惠券
        /// </summary>
        /// <returns>所有优惠券</returns>
        PageData<Coupon> FindAllCoupon(int currentPage, int pageSize, IDictionary<CouponSearchCriteria, object> searchCriteria, IList<Sorter<CouponSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 通过优惠券Id获取优惠券
        /// </summary>
        /// <param name="couponId">couponId</param>
        /// <returns>优惠券</returns>
        Coupon GetCoupon(int couponId);

        /// <summary>
        /// 通过优惠券编号获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券编号</param>
        /// <returns>优惠券</returns>
        Coupon GetCoupon(string couponCode);

        /// <summary>
        /// 获取注册Coupon
        /// </summary>
        /// <param name="languageId">语言Id</param>
        /// <returns>coupon</returns>
        Coupon GetRegisterCoupon(int languageId);

        /// <summary>
        /// 获取coupon多语言信息
        /// </summary>
        /// <param name="couponId">couponId</param>
        /// <returns>coupon多语言信息key=语种Id</returns>
        Dictionary<int, CouponDesc> GetCouponDesc(int couponId);

        /// <summary>
        /// 获取对应语种coupon信息
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        CouponDesc GetCouponDesc(int couponId, int languageId);
        #endregion

        #region CustomerCoupon
        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券Code</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="currency">币种</param>
        /// <param name="country">国家</param>
        /// <param name="language">语种</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_COUPON_NOT_EXIST:优惠券不存在</value>
        /// </exception>
        void PickCustomerCoupon(string couponCode, int customerId, int currency, int country, int language);

        /// <summary>
        /// 领取优惠券（可重读）
        /// </summary>
        /// <param name="couponCode">优惠券Code</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="currency">币种</param>
        /// <param name="country">国家</param>
        /// <param name="language">语种</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_COUPON_NOT_EXIST:优惠券不存在</value>
        /// </exception>
        void PickCustomerCouponRep(string couponCode, int customerId, int currency, int country, int language);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="couponStatus"></param>
        /// <param name="couponMarketingRewardType"></param>
        /// <returns></returns>
        IList<CouponCustomer> GetCouponCustomer(int customerId, CouponStatus couponStatus, CouponMarketingRewardType couponMarketingRewardType);

        /// <summary>
        /// 获取可用的优惠券（传入金额必须为美元）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="amounts">key=物品金额类型，value=物品金额</param>
        /// <param name="country">国家</param>
        /// <param name="currency">币种</param>
        /// <param name="language">语种</param>
        /// <returns>可用的优惠券</returns>
        IList<CouponCustomer> GetUsableCoupons(int customerId, Dictionary<AmountType, decimal> amounts, int country, int currency, int language);

        //todo 验证客户是否唯一
        /// <summary>
        /// 优惠券是否可用
        /// </summary>
        /// <param name="customerCouponId">customerCouponId</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="amounts">key=物品金额类型，value=物品金额</param>
        /// <param name="country">国家</param>
        /// <param name="currency">币种</param>
        /// <param name="language">语言</param>
        /// <returns>是否可用</returns>
        bool IsCouponUsable(int customerCouponId, int customerId, Dictionary<AmountType, decimal> amounts, int country, int currency, int language);

        //todo 验证客户是否唯一
        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="customerCouponId">customerCouponId</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="amounts">key=物品金额类型，value=物品金额</param>
        /// <param name="country">国家</param>
        /// <param name="currency">币种</param>
        /// <param name="language">语言</param>
        void UseCoupon(int customerCouponId, int customerId, int orderId, Dictionary<AmountType, decimal> amounts, int country, int currency, int language);


        /// <summary>
        /// 关闭客户Coupon
        /// </summary>
        /// <param name="customerCouponId">客户couponId</param>
        /// <param name="adminId"></param>
        /// <param name="reason">关闭原因</param>
        void CloseCustomerCoupon(int customerCouponId, int adminId, string reason);

        /// <summary>
        /// 重新启用客户coupon
        /// </summary>
        /// <param name="customerCouponId">客户couponId</param>
        /// <param name="adminId"></param>
        /// <param name="enDateTime">结束时间</param>
        void StartCustomerCoupon(int customerCouponId,int adminId, DateTime enDateTime);

        /// <summary>
        /// 后台使用客户Coupon
        /// </summary>
        /// <param name="customerCouponId">客户couponId</param>
        /// <param name="adminId"></param>
        /// <param name="reason">使用原因</param>
        void UseCustomerCoupon(int customerCouponId, int adminId,string reason);

        ///// <summary>
        ///// 通过优惠券编号获取优惠券授权客户
        ///// </summary>
        ///// <param name="couponCode">优惠券编号</param>
        ///// <returns>优惠券授权客户</returns>
        //List<CouponAuthCustomer> GetCouponAuthCustomers(string couponCode);

        /// <summary>
        /// 通过customerCouponId获取客户优惠券
        /// </summary>
        /// <param name="customerCouponId">customerCouponId</param>
        /// <returns>客户优惠券</returns>
        CouponCustomer GetCustomerCoupon(int customerCouponId);

        /// <summary>
        /// 获取最新快到期的优惠券（提醒日期t_config取,一个coupon提醒一次）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户优惠券</returns>
        CouponCustomer GetNewestExpiryCustomerCoupon(int customerId) ;

        /// <summary>
        /// 通过customerCouponId获取客户优惠券
        /// </summary>
        /// <param name="customerCouponId">customerCouponId</param>
        /// <returns>客户优惠券</returns>
        CouponCustomerView GetCustomerCouponView(int customerCouponId);

        /// <summary>
        /// 通过客户Id获取客户优惠券
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户优惠券</returns>
        IList<CouponCustomer> GetCustomerCoupons(int customerId);

        /// <summary>
        /// 获取所有客户优惠券
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns>客户优惠券</returns>
        PageData<CouponCustomer> FindAllCustomerCoupon(int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria, IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取我的客户优惠券
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">当前页大小</param>
        /// <param name="searchCriteria">搜索条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<CouponCustomer> FindMyCustomerCoupon(int customerId, int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria, IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 后台视图获取所有客户优惠券
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns>客户优惠券</returns>
        PageData<CouponCustomerView> FindAllCustomerCouponView(int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria, IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取客户对应优惠券
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns>客户优惠券</returns>
        PageData<CouponCustomer> FindAllCustomerCoupon(int customer, int currentPage, int pageSize, IDictionary<CustomerCouponSearchCriteria, object> searchCriteria, IList<Sorter<CustomerCouponSorterCriteria>> sorterCriteria);

        #endregion
        #endregion
    }

    /// <summary>
    /// 购物券金额类型
    /// </summary>
    public enum AmountType
    {
        /// <summary>
        /// 物品最终金额
        /// </summary>
        TotalAmount,
        /// <summary>
        /// 正常价格
        /// </summary>
        NormalAmount
    }

    /// <summary>
    /// customercoupon查询条件
    /// </summary>
    public enum CustomerCouponSearchCriteria
    {
        /// <summary>
        /// coupon名称
        /// </summary>
        CouponName,
        /// <summary>
        /// 邮箱Id
        /// </summary>
        EmailId,
        /// <summary>
        /// 订单号
        /// </summary>
        OrderCode,
        /// <summary>
        /// 状态
        /// </summary>
        Status,
        /// <summary>
        /// 过期
        /// </summary>
        PassDue,
        /// <summary>
        /// 激活的coupon
        /// </summary>
        ActiveCoupon,
        /// <summary>
        /// 未激活的coupon
        /// </summary>
        InActiveCoupon,
    }

    /// <summary>
    /// customercoupon排序条件
    /// </summary>
    public enum CustomerCouponSorterCriteria
    {
        /// <summary>
        /// 剩余天数
        /// </summary>
        LeftDay
    }

    /// <summary>
    /// coupon查询条件
    /// </summary>
    public enum CouponSearchCriteria { }

    /// <summary>
    /// coupon排序条件
    /// </summary>
    public enum CouponSorterCriteria { }
}