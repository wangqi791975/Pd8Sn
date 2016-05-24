using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Com.Panduo.Web.Common.Mvc.Result
{
    /// <summary>
    /// 输出图形
    /// </summary>
    public class ChartResult : ActionResult
    {
        /// <summary>
        /// 要输出的图形
        /// </summary>
        public Chart Chart { get; set; }

        /// <summary>
        /// 图形格式
        /// </summary>
        public string ChartFormat { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            Chart.Write(ChartFormat);
        }
    }
}