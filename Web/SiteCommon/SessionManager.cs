using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Web.Common; 

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// Session辅助类
    /// </summary>
    public partial class SessionManager
    {
        #region 公共方法
        
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="key">Session键</param>
        /// <returns></returns>
        public static object GetSession(string key)
        {
            return HttpContext.Current.Session[key];
        }
        /// <summary>
        /// 保存Session
        /// </summary>
        /// <param name="key">Session键</param>
        /// <param name="value">Sessionz值</param>
        public static void SetSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
        /// <summary>
        /// 清除当前Session
        /// </summary>
        public static void ClearAllSession()
        {
            HttpContext.Current.Session.Clear();
        }
        /// <summary>
        /// 删除Session值
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
        /// <summary>
        /// Session失效
        /// </summary>
        public static void KillSession()
        {
            HttpContext.Current.Session.Abandon();
        } 
        #endregion 
    }
}
