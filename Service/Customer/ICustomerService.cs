using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 客户服务接口
    /// </summary>
    public interface ICustomerService
    {
        #region 常量

        /// <summary>
        /// 邮箱地址已存在
        /// </summary>
        string ERROR_EMAIL_EXIST { get; }

        /// <summary>
        /// 邮箱地址不存在
        /// </summary>
        string ERROR_EMAIL_NOT_EXIST { get; }

        /// <summary>
        /// 客户不存在
        /// </summary>
        string ERROR_CUSTOMER_NOT_EXIST { get; }

        /// <summary>
        /// 客户密码错误
        /// </summary>
        string ERROR_WRONG_PASSWORD { get; }

        /// <summary>
        /// facebook账号密码不正确
        /// </summary>
        string ERROR_FACEBOOK_ACCOUNT_PASSWORD { get; }

        /// <summary>
        /// 密码串超时
        /// </summary>
        string ERROR_PASSWORD_CODE_TIMEOUT { get; }

        /// <summary>
        /// 密码串错误
        /// </summary>
        string ERROR_WRONG_PASSWORD_CODE { get; }

        /// <summary>
        /// 没有默认的货运地址
        /// </summary>
        string ERROR_NO_SHIPPING_ADDRESS { get; }

        /// <summary>
        /// 没有默认的账单地址
        /// </summary>
        string ERROR_NO_BILLING_ADDRESS { get; }

        /// <summary>
        /// 地址不存在
        /// </summary>
        string ERROR_ADDRESS_NOT_EXIST { get; }

        /// <summary>
        /// 超过最大允许注册数
        /// </summary>
        string ERROR_OVER_MAX_REGISTER_COUNT { get; }

        /// <summary>
        /// 这个客户已经绑定了其他facebook账户
        /// </summary>
        string ERROR_CUSTOMER_EXIST_BIND_FACEBOOK { get; }

        /// <summary>
        /// 这个facebook账号已经绑定了其他客户
        /// </summary>
        string ERROR_FACEBOOK_EXIST_BIND_CUSTOMER { get; }

        /// <summary>
        /// 密码串已经使用过
        /// </summary>
        string EEOR_PASSWORD_USED { get; }

        /// <summary>
        /// 客户Id不对应
        /// </summary>
        string ERROR_WRONG_CUSTOMER { get; }

        /// <summary>
        /// 未订阅
        /// </summary>
        string ERROR_NOT_SUBSCRIBE { get; }

        /// <summary>
        /// 默认货运地址
        /// </summary>
        string ERROR_DEFAULT_SHIPPING_ADDRESS { get; }

        /// <summary>
        /// 默认账单地址
        /// </summary>
        string ERROR_DEFAULT_BILLING_ADDRESS { get; }

        /// <summary>
        /// 加密串不存在
        /// </summary>
        string ERROR_ENCRYPT_CODE_NOT_EXIST { get; }

        /// <summary>
        /// 超时
        /// </summary>
        string ERROR_OVERTIME { get; }
        #endregion

        #region 方法

        #region 客户

        /// <summary>
        /// 注册（此处实现需要创建Cash账户）
        /// 1.只能填写（邮箱，密码，姓名[不填时默认存“Customer”]，客户类型，性别，是否订阅），其他都忽略
        /// </summary>
        /// <param name="customer">客户</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_EMAIL_EXIST:该Email已经存在</value>
        ///     <value>ERROR_OVER_MAX_REGISTER_COUNT:超过最大允许注册数</value>
        /// </exception>
        /// <returns>新增客户Id</returns>
        int Register(Customer customer);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="email">登录邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="ip">登录IP地址</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_EMAIL_NOT_EXIST:邮箱不存在</value>
        ///     <value>ERROR_WRONG_PASSWORD:密码错误</value>
        /// </exception>
        /// <returns>返回是否登录成功</returns>
        bool Login(string email, string password, string ip);

        /// <summary>
        /// 登出（记录登出时间）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void Logout(int customerId);

        /// <summary>
        /// 判断facebook账号是否存在（是否已绑定）
        /// </summary>
        /// <param name="facebookId">facebookId</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_FACEBOOK_ACCOUNT_PASSWORD:账号密码不正确</value>
        /// </exception>
        /// <returns>返回facebook绑定的customer</returns>
        Customer CheckFacebookAccount(string facebookId);

        /// <summary>
        /// （facebook登录成功之后，未绑定时）绑定facebook账号
        /// </summary>
        /// <param name="facebookInfo">facebook信息</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_CUSTOMER_EXIST_BIND_FACEBOOK:这个客户已经绑定了其他facebook账户</value>
        ///     <value>ERROR_FACEBOOK_EXIST_BIND_CUSTOMER:这个facebook账号已经绑定了其他客户</value>
        /// </exception>
        void BindFacebookInfo(FacebookInfo facebookInfo);

        /// <summary>
        /// 修改客户信息
        /// 1.客户Id，是否渠道商，最后登录时间，历史购买金额，IP地址，客户邮箱忽略修改
        /// </summary>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        /// <param name="customer">客户实体</param>
        void UpdateCustomerInfo(Customer customer);

        /// <summary>
        /// 修改客户状态
        /// </summary>
        /// <param name="customerId"></param>
        void UpdateCustomerStatus(int customerId);

        /// <summary>
        /// 编辑头像
        /// </summary>
        /// <param name="avatarPath">头像路径</param>
        void EditCustomerAvatar(int customerId, string avatarPath);

        /// <summary>
        /// 修改客户邮箱
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="email">邮箱</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_EMAIL_EXIST:该Email已经存在</value>
        /// </exception>
        void ChangeEmail(int customerId, string email);

        /// <summary>
        /// 验证客户密码
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="password"></param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        /// <returns>密码是否正确</returns>
        bool CheckPassword(int customerId, string password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="oldPassword">老密码</param>
        /// <param name="newPassword">新密码</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_WRONG_PASSWORD:密码错误</value>
        /// </exception>
        void UpdatePassword(int customerId, string oldPassword, string newPassword);

        /// <summary>
        /// 找回密码（验证邮件是否注册，邮件发送密码修改串[加密]，记录找回密码日志实体）
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_EMAIL_NOT_EXIST:Email不存在</value>
        /// </exception>
        string FindPassword(string email);

        /// <summary>
        /// 通过加密串获取忘记密码日志信息
        /// </summary>
        /// <param name="encryptCode">加密串</param>
        /// <returns>忘记密码日志信息</returns>
        ForgotPassword GetForgotPassword(string encryptCode);

        /// <summary>
        /// 验证密码串有效性
        /// </summary>
        /// <param name="encryptCode">密码串</param>
        /// <returns>解密串</returns>
        string VerifyCode(string encryptCode);

        /// <summary>
        /// 修改忘记密码状态
        /// </summary>
        /// <param name="encryptCode"></param>
        void UpdateForgotStatus(string encryptCode);

        /// <summary>
        /// 客户重置密码
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="forgotPasswordId">忘记密码Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void ResetPassword(int customerId, string newPassword, int forgotPasswordId);

        /// <summary>
        /// 客服重置密码
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="adminId">操作人Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void ResetPassword(int customerId, int adminId);

        /// <summary>
        /// 通过客户Id获取客户信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户实体</returns>
        Customer GetCustomerById(int customerId);

        /// <summary>
        /// 通过客户email获取客户信息
        /// </summary>
        /// <param name="email">客户email</param>
        /// <returns>客户实体</returns>
        Customer GetCustomerByEmail(string email);

        /// <summary>
        /// 查询数据库得到所有客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<Customer> FindAllCustomers(int currentPage, int pageSize, IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 查询数据库得到所有客户头像
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        PageData<CustomerAvatar> FindAllCustomerAvatars(int currentPage, int pageSize);

        /// <summary>
        /// 查询购物车有商品且14天未登录的客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<Customer> FindLongTimeNotUpdatedCustomers(int currentPage, int pageSize, IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取当天当前IP地址注册的客户数
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>当天注册的客户数量</returns>
        int GetRegisterCountByIP(string ip);

        /// <summary>
        /// 通过客户Id获取订阅信息
        /// </summary>
        /// <param name="email">客户Id</param>
        /// <returns></returns>
        Newsletter GetNewsletter(string email);

        /// <summary>
        /// 订阅（MyAccount入口退订）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void Subscribe(int customerId);

        /// <summary>
        /// 客户退订（MyAccount入口退订）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        ///     <value>ERROR_NOT_SUBSCRIBE:未订阅</value>
        /// </exception>
        void UnSubscribe(int customerId);

        #region 高危客户

        /// <summary>
        /// 添加高危客户
        /// </summary>
        /// <param name="customerHighRisk"></param>
        void AddCustomerHighRisk(CustomerHighRisk customerHighRisk);

        /// <summary>
        /// 删除高危客户
        /// </summary>
        /// <param name="customerId"></param>
        void DelCustomerHighRisk(int customerId);

        /// <summary>
        /// 分页查询高危客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<CustomerHighRisk> FindCustomerHighRisks(int currentPage, int pageSize, IDictionary<CustomerHighRiskSearchCriteria, object> searchCriteria, IList<Sorter<CustomerHighRiskSorterCriteria>> sorterCriteria);

        #endregion

        #endregion

        #region 销售备注
        /// <summary>
        /// 获取客户销售备注
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>销售备注信息</returns>
        CustomerRemark GetCustomerRemark(int customerId);

        /// <summary>
        /// 修改销售备注
        /// </summary>
        /// <param name="customerId">Id</param>
        /// <param name="remark">备注信息</param>
        /// <param name="adminId"></param>
        void SetCustomerRemark(int customerId, string remark, int adminId);
        #endregion

        #region 客服经理

        /// <summary>
        /// 查询数据库得到所有客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<CustomerManager> FindAllManager(int currentPage, int pageSize, IDictionary<ManagerSearchCriteria, object> searchCriteria, IList<Sorter<ManagerSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 通过客户经理Id获取客户经理对象
        /// </summary>
        /// <param name="customerManagerId">客户经理Id</param>
        /// <returns>客户经理对象</returns>
        CustomerManager GetCustomerManager(int customerManagerId);

        /// <summary>
        /// 编辑客户经理信息
        /// </summary>
        /// <param name="customerManager"></param>
        int EditCustomerManager(CustomerManager customerManager);

        /// <summary>
        /// 删除客户经理
        /// </summary>
        /// <param name="customerManagerId">客户经理Id</param>
        void DeleteCustomerManager(int customerManagerId);
        #endregion

        #region 使用偏好

        /// <summary>
        /// 设置使用偏好
        /// </summary>
        /// <param name="preference">使用偏好实体</param>
        void SetPreference(Preference preference);

        /// <summary>
        /// 通过客户Id获取客户使用偏好
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>使用偏好实体</returns>
        Preference GetPreferenceByCustomerId(int customerId);

        #endregion

        #region 地址

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="address">地址实体</param>
        /// <param name="isDefaultShippingAddress">是否默认货运地址</param>
        /// <param name="isDefaultBillingAddress">是否默认账单地址</param>
        /// <returns>新建地址Id</returns>
        int AddAddress(Address address, bool isDefaultShippingAddress, bool isDefaultBillingAddress);

        /// <summary>
        /// 修改地址（判断修改客户是否为同一个）
        /// 1.忽略地址Id，客户Id的修改
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="address">地址实体</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_WRONG_CUSTOMER:错误客户</value>
        ///     <value>ERROR_ADDRESS_NOT_EXIST:地址不存在</value>
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:客户不存在</value>
        /// </exception>
        void UpdateAddress(int customerId, Address address);

        /// <summary>
        /// 删除地址（判断修改客户是否为同一个）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="addressId">地址Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_WRONG_CUSTOMER:错误客户</value>
        ///     <value>ERROR_ADDRESS_NOT_EXIST:地址不存在</value>
        ///     <value>ERROR_DEFAULT_SHIPPING_ADDRESS:该地址为默认货运地址</value>
        ///     <value>ERROR_DEFAULT_BILLING_ADDRESS:该地址为默认账单地址</value>
        /// </exception>
        void DeleteAddress(int customerId, int addressId);

        /// <summary>
        /// 设置货运默认地址（判断修改客户是否为同一个）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="addressId">地址Id</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_WRONG_CUSTOMER:错误客户</value>
        ///     <value>ERROR_ADDRESS_NOT_EXIST:地址不存在</value>
        /// </exception>
        void SetShippingAddress(int customerId, int addressId);

        /// <summary>
        /// 设置账单默认地址（判断修改客户是否为同一个）
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="addressId"></param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_WRONG_CUSTOMER:错误客户</value>
        ///     <value>ERROR_ADDRESS_NOT_EXIST:地址不存在</value>
        /// </exception>
        void SetBillingAddress(int customerId, int addressId);

        /// <summary>
        /// 通过地址Id获取地址
        /// </summary>
        /// <param name="addressId">地址Id</param>
        /// <returns>地址实体</returns>
        Address GetAddressById(int addressId);

        /// <summary>
        /// 通过客户Id获取客户地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户对应的所有地址</returns>
        IList<Address> GetAddressesByCustomerId(int customerId);

        /// <summary>
        /// 获取客户默认货运地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>默认货运地址</returns>
        Address GetDefaultShippingAddress(int customerId);

        /// <summary>
        /// 获取客户默认账单地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>默认账单地址</returns>
        Address GetDefaultBillingAddress(int customerId);

        #endregion

        #region 客户等级
        /// <summary>
        /// 设置等级名称（key：等级名称Id，value：等级名称）
        /// </summary>
        /// <param name="nameMap">等级名称map</param>
        void SetCustomerGroupDesc(Dictionary<int, string> nameMap);

        /// <summary>
        /// 获取所有客户等级信息
        /// </summary>
        /// <returns>客户等级信息集合</returns>
        IList<CustomerGroup> GetAllCustomerGroups();

        /// <summary>
        /// 通过Id获取客户登记信息
        /// </summary>
        /// <param name="customerGroupId">客户等级Id</param>
        /// <returns>客户等级实体</returns>
        CustomerGroup GetCustomerGroupById(int customerGroupId);

        /// <summary>
        /// 获取等级名称
        /// </summary>
        /// <param name="customerGroupId">客户等级Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>等级名称实体</returns>
        CustomerGroupDesc GetCustomerGroupDesc(int customerGroupId, int languageId);

        /// <summary>
        /// 通过客户等级Id获取所有语种名称
        /// </summary>
        /// <param name="customerGroupId">客户等级Id</param>
        /// <returns>客户等级名称集合</returns>
        IList<CustomerGroupDesc> GetCustomerGroupDesc(int customerGroupId);

        /// <summary>
        /// 通过语种Id获取所有该语种等级名称
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns>客户等级名称集合</returns>
        IList<CustomerGroupDesc> GetCustomerGroupDescsByLanguage(int languageId);

        /// <summary>
        /// 获取客户下一个等级
        /// </summary>
        /// <param name="customerGroupId">客户等级Id</param>
        /// <returns>等级名称实体</returns>
        CustomerGroup GetNextCustomerGroup(int customerGroupId);

        #endregion


        #endregion

    }

    /// <summary>
    /// 客户搜索条件
    /// </summary>
    public enum CustomerSearchCriteria
    {
        /// <summary>
        /// 关键词
        /// </summary>
        KeyWord,
        /// <summary>
        /// 客户类型
        /// </summary>
        CustomerType,
        /// <summary>
        /// 语言Id
        /// </summary>
        LanguageId,
        /// <summary>
        /// 来源类型
        /// </summary>
        SourceType
    }

    /// <summary>
    /// 客户排序条件
    /// </summary>
    public enum CustomerSorterCriteria
    {
        CustomerId,
    }

    /// <summary>
    /// 高危客户搜索条件
    /// </summary>
    public enum CustomerHighRiskSearchCriteria
    {
        /// <summary>
        /// 关键词
        /// </summary>
        KeyWord,
        /// <summary>
        /// 客户类型
        /// </summary>
        CustomerType,
        /// <summary>
        /// 语言Id
        /// </summary>
        LanguageId,
        /// <summary>
        /// 来源类型
        /// </summary>
        SourceType
    }

    /// <summary>
    /// 高危客户排序条件
    /// </summary>
    public enum CustomerHighRiskSorterCriteria
    {

    }
}