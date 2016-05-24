//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：OrderPaymentAmountLog.cs
//创 建 人：罗海明
//创建时间：2015/04/14 16:59:50 
//功能说明：订单金额修改日志Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：  
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Order
{
    public class OrderPaymentAmountLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int LogId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 原始金额
        /// </summary>
        public virtual decimal OriginalAmount { get; set; }

        /// <summary>
        /// 更改后最终金额
        /// </summary>
        public virtual decimal NewAmount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public virtual int Creator { get; set; }

        /// <summary>
        /// 创建人Email
        /// </summary>
        public virtual string CreatorEmail { get; set; }
    }
}
