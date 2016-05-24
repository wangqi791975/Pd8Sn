using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Com.Panduo.Service.AdminUser
{
    /// <summary>
    /// 管理员服务接口
    /// </summary>
    public interface IAdminUserService
    { 
        #region 常量
        /// <summary>
        /// 用户Email已经存在
        /// </summary>
        string ERROR_USER_EMAIL_EXIST { get; }

        /// <summary>
        /// 用户已经存在
        /// </summary>
        string ERROR_USER_EXIST { get; }

        /// <summary>
        /// 用户不存在
        /// </summary>
        string ERROR_USER_NOT_EXIST { get; }

        /// <summary>
        /// 用户是根用户
        /// </summary>
        string ERROR_USER_IS_ROOT { get; }

        /// <summary>
        /// 用户登陆失败
        /// </summary>
        string ERROR_LOGIN_FAILED { get; } 

        /// <summary>
        /// 用户密码错误
        /// </summary>
        string ERROR_WRONG_PASSWORD { get; }

        /// <summary>
        /// 不能删除超级管理员
        /// </summary>
        string ERROR_CANNOT_DELETE_ROOT_USER { get; }

        /// <summary>
        /// 超级管理员不能进行操作
        /// </summary>
        string ERROR_CANNOT_OPERATE_ROOT_USER { get; }

        /// <summary>
        /// 密码在半年内已经使用过
        /// </summary>
        string ERROR_PASSWORD_HAS_USED { get; }

        /// <summary>
        /// 密码必须为数字与字母结合
        /// </summary>
        string ERROR_PASSWORD_INCONFORMITY { get; }

        /// <summary>
        /// 两次输入的密码不一致，请修改
        /// </summary>
        string ERROR_PASSWORD_NOT_SAME { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 添加一个新后台管理员
        /// </summary>
        /// <param name="adminUser">要创建的管理员</param>
        /// <param name="password">密码</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_USER_EMAIL_EXIST:用户Email已注册过</value>
        /// </exception>
        /// <returns>新创建的后台管理员Id</returns>
        int AddAdminUser(AdminUser adminUser,string password);

        /// <summary>
        /// 用户登录系统
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">登录密码</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_LOGIN_FAILED:用户登录失败</value>
        /// <value>ERROR_USER_NOT_EXIST:用户不存在或则不为激活状态</value>
        /// <value>ERROR_WRONG_PASSWORD:用户密码错误</value>
        /// </exception>
        /// <returns>返回对应的用户信息</returns>
        AdminUser Login(string email, string password);

        /// <summary>
        /// 用户登出系统
        /// </summary>
        /// <param name="loginId">用户登录Id</param>
        /// <param name="ip">登出Ip地址</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_USER_NOT_EXIST:用户不存在</value>
        /// </exception>
        void Logout(string loginId,string ip);

        /// <summary>
        /// 修改用户基本信息
        /// 1.忽略对用户编号、是否root用户、用户密码、创建时间、登录时间的修改
        /// </summary>
        /// <param name="adminUser">要修改的用户</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_USER_NOT_EXIST:用户不存在</value>
        /// <value>ERROR_USER_EMAIL_EXIST:用户Email已注册过</value>
        /// </exception>
        void UpdateAdminUser(AdminUser adminUser);

        /// <summary>
        /// 用户更改密码
        /// </summary>
        /// <param name="adminUserId">用户Id</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="conPassword">确认密码</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_USER_NOT_EXIST:用户不存在</value>
        /// <value>ERROR_WRONG_PASSWORD:用户旧密码错误</value>
        /// </exception>
        void ChangePassword(int adminUserId, string oldPassword, string newPassword, string conPassword);

        /// <summary>
        /// 获取半年内密码对应的修改日志
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <returns>密码修改日志</returns>
        AdminPasswordUsed GetAdminPasswordUsed(string newPassword);

        /// <summary>
        /// 重置账号密码为123456
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_USER_NOT_EXIST:用户不存在</value>
        /// <value>ERROR_USER_IS_ROOT:根用户不允许重置密码</value>
        /// </exception>
        void ResetPassword(int adminUserId);

        /// <summary>
        /// 删除后台用户,用户不存在则忽略
        /// </summary>
        /// <param name="adminUserId">要删除的用户Id</param> 
        /// <exception cref="BussinessException">
        /// <value>ERROR_CANNOT_DELETE_ROOT_USER:Root用户不允许删除</value>
        /// </exception>
        void DeleteAdminUser(int adminUserId);

        /// <summary>
        /// 批量删除后台用户,用户不存在则忽略,返回实际删除的用户数量
        /// </summary>
        /// <param name="adminUserIds">要删除的用户Id列表</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_CANNOT_DELETE_ROOT_USER:Root用户不允许删除</value>
        /// </exception>
        /// <returns>实际删除的用户数量</returns>
        int DeleteAdminUsers(IEnumerable<int> adminUserIds);

        /// <summary>
        /// 锁定一个后台用户
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_USER_NOT_EXIST:用户不存在</value> 
        /// <value>ERROR_CANNOT_OPERATE_ROOT_USER:超级管理员不能锁定</value>  
        /// </exception>
        void LockAdminUser(int adminUserId);

        /// <summary>
        /// 解锁一个后台用户
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_USER_NOT_EXIST:用户不存在</value> 
        /// <value>ERROR_CANNOT_OPERATE_ROOT_USER:超级管理员不能解锁</value>  
        /// </exception>
        void UnLockAdminUser(int adminUserId);

        /// <summary>
        /// 根据Id获取后台用户
        /// </summary>
        /// <param name="adminUserId">用户Id</param>
        /// <returns>后台用户</returns>
        AdminUser GetAdminUser(int adminUserId);

        /// <summary>
        /// 根据Email地址获取后台用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        AdminUser GetAdminUserByEmail(string email);

        /// <summary>
        /// 获取所有后台用户,按照创建时间排序
        /// </summary>
        /// <returns>后台用户列表</returns>
        IList<AdminUser> GetAllAdminUsers();

        /// <summary>
        /// 获取一个角色下的所有后台用户,按照角色名称升序排序
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>后台用户列表</returns>
        IList<AdminUser> GetAdminUsersByRole(int roleId);

        /// <summary>
        /// 获取一个角色下的后台用户个数
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int GetCountOfAdminUsersByRole(int roleId);

        /// <summary>
        /// 搜索后台用户
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的后台用户列表</returns>
        PageData<AdminUser> FindAdminUsers(int currentPage, int pageSize, IDictionary<AdminUserSearchCriteria, object> searchCriteria, IList<Sorter<AdminUserSorterCriteria>> sorterCriteria);
        #endregion

        #region MenuModule

        /// <summary>
        /// 修改用户使用状态
        /// </summary>
        /// <param name="adminId">用户Id</param>
        /// <returns></returns>
        int UpdateAdminStatus(int adminId);

        /// <summary>
        /// 通过控制器和Action获取模块
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="action">Action</param>
        /// <returns>模块对象</returns>
        AdminModule GetAdminModule(string controller, string action);

        /// <summary>
        /// 获取所有的菜单及子菜单
        /// </summary>
        /// <returns>菜单列表</returns>
        IList<AdminMenu> GetAllAdminMenus();

        /// <summary>
        /// 获取用户对应模块
        /// </summary>
        /// <param name="adminId">用户ID</param>
        /// <returns>用户对应模块</returns>
        IList<AdminUserModule> GetAdminUserModules(int adminId);

        /// <summary>
        /// 用户模块设置
        /// </summary>
        /// <param name="adminUserModules">用户模块</param>
        /// <param name="adminId">用户Id</param>
        IList<int> SetAdminUserModules(List<AdminUserModule> adminUserModules, int adminId);

        /// <summary>
        /// 修改后台用户信息
        /// </summary>
        /// <param name="adminId">用户Id</param>
        /// <param name="name">用户名称</param>
        /// <param name="email">用户email</param>
        /// <param name="isValid">是否有效（启用/禁用）</param>
        /// <param name="isViewEmail">是否显示客户邮箱</param>
        void UpdateAdminUser(int adminId,string name, string email, bool isValid, bool isViewEmail);

        /// <summary>
        /// 修改模块
        /// </summary>
        /// <param name="adminId">后台用户Id</param>
        /// <param name="modulecodes">模块集合</param>
        void UpdateAdminModule(int adminId, List<string> modulecodes);
        #endregion

    }

    /// <summary>
    /// 后台用户查询条件
    /// </summary>
    public enum AdminUserSearchCriteria
    {
        /// <summary>
        /// 用户编码(登录名),精确查询
        /// </summary>
        UserCode,
        /// <summary>
        /// 用户名,模糊查询
        /// </summary>
        UserName,
        /// <summary>
        /// Email地址,模糊查询
        /// </summary>
        Email,
        /// <summary>
        /// 角色,精确查询,int类型
        /// </summary>
        RoleId,
        /// <summary>
        /// 用户状态，精确查询，UserStatus枚举类型
        /// </summary>
        Status,
        /// <summary>
        /// 创建日期(不包括时间部分)开始，比如要查询2011-12-13日以后创建的用户，条件为：CreateDate>='2011-12-13 00:00:00',DateTime类型
        /// </summary>
        CreateDateFrom,/// <summary>
        /// 创建日期(不包括时间部分)截止，比如要查询2011-12-13日以前创建的用户，条件为：CreateDate&lt;='2011-12-13 23:59:59',DateTime类型
        /// </summary>
        CreateDateTo,
        /// <summary>
        /// 是否root用户,bool类型精确匹配
        /// </summary>
        IsRoot
    }

    /// <summary>
    /// 后台用户排序条件
    /// </summary>
    public enum AdminUserSorterCriteria
    {
        /// <summary>
        /// Id排序
        /// </summary>
        Id,
        /// <summary>
        /// 用户编码排序
        /// </summary>
        UserCode,
        /// <summary>
        /// 用户名排序
        /// </summary>
        UserName,
        /// <summary>
        /// Email地址排序
        /// </summary>
        Email,
        /// <summary>
        /// 用户状态排序
        /// </summary>
        Status,
        /// <summary>
        /// 创建日期排序
        /// </summary>
        CreateDate,
        /// <summary>
        /// 是否Root用户排序
        /// </summary>
        IsRoot
    }
}
