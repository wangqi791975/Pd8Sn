using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common.Mvc.Model
{
    /// <summary>
    /// Route配置对象
    /// </summary>
    public class RouteObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public virtual object Defaults { get; set; }
        /// <summary>
        /// 路由限制
        /// </summary>
        public virtual object Constraints { get; set; }
        /// <summary>
        /// 路由寻找命名空间
        /// </summary>
        public virtual string[] Namespaces { get; set; }

        /// <summary>
        /// 默认值字符串值
        /// </summary>
        public virtual string DefaultsValue { get; set; }
        /// <summary>
        /// 路由限制字符串值
        /// </summary>
        public virtual string ConstraintsValue { get; set; }
        /// <summary>
        /// 路由寻找命名空间字符串值
        /// </summary>
        public virtual string NamespacesValue { get; set; }

    }
}