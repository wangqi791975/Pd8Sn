using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.Panduo.Web.Controllers
{
    /// <summary>
    /// Controller基类
    /// </summary>
    public class BaseController : Controller
    {


        internal class ActionJsonResult
        {

            #region 成员字段

            /// <summary>
            /// 已存在
            /// </summary>
            public static readonly string Exists = "exists";

            /// <summary>
            /// 成功
            /// </summary>
            public static readonly string Success = "success";
            /// <summary>
            /// 失败
            /// </summary>
            public static readonly string Failing = "failing";
            /// <summary>
            ///错误
            /// </summary>
            public static readonly string Error = "error";
            /// <summary>
            /// 页面需要刷新
            /// </summary>
            public static readonly string PageRefresh = "refresh";
            #endregion
        }
    }
}
