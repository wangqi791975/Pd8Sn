using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common.Config
{
    /// <summary>
    /// 登录顾虑配置
    /// </summary>
    [Serializable]
    public class LoginFilterConfig
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public virtual string ControllerName { get; set; }
        /// <summary>
        /// 需要登录验证的Action集合,如果包含*或者All则表示所有Action都需要验证
        /// </summary>
        public virtual IList<string> IncludeActionList { get; set; }
        /// <summary>
        /// 不需要登录验证的Action集合
        /// </summary>
        public virtual IList<string> ExcludeActionList { get; set; }
    }
}