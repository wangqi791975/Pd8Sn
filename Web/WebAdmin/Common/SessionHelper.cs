using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.AdminUser;
using Com.Panduo.Web.Common;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// Session辅助类
    /// </summary>
    public class SessionHelper
    {
        #region 公共信息
        /// <summary>
        /// 当前用户是否已检查过IP
        /// </summary>
        public static readonly string SESSION_KEY_IS_IP_CHECKED = "SESSION_KEY_IS_IP_CHECKED";
        /// <summary>
        /// 当前登陆客户
        /// </summary>
        public static bool IsIpCheckd
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_IS_IP_CHECKED).ToString().ParseTo(false);
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_IS_IP_CHECKED, value);
            }
        }

        #region 后台用户相关
        /// <summary>
        /// 当前后台用户
        /// </summary>
        public static readonly string SESSION_KEY_CURRENT_ADMINUSER = "SESSION_KEY_CURRENT_ADMINUSER";
        /// <summary>
        /// 当前后台用户
        /// </summary>
        public static AdminUser CurrentAdminUser
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_CURRENT_ADMINUSER) as AdminUser;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_ADMINUSER, value);
            }
        }

        public static readonly string SESSION_KEY_CURRENT_ADMINMODULE = "SESSION_KEY_CURRENT_ADMINMODULE";

        public static List<AdminUserModule> CurrentAdminUserModules
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_CURRENT_ADMINMODULE) as List<AdminUserModule>;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_ADMINMODULE, value);
            }
        }
        #endregion

        #endregion
        #region 客户信息相关
        /// <summary>
        /// 当前登陆客户
        /// </summary>
        public static readonly string SESSION_KEY_CURRENT_USER = "SESSION_KEY_CURRENT_USER";
        /// <summary>
        /// 当前登陆客户
        /// </summary>
        public static AdminUser CurrentCustomer
        {
            get
            {
                return SessionManager.GetSession(SESSION_KEY_CURRENT_USER) as AdminUser;
            }
            set
            {
                SessionManager.SetSession(SESSION_KEY_CURRENT_USER, value);
            }
        }
        #endregion
    }
}
