//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：OrderStatusHistory.cs
//创 建 人：罗海明
//创建时间：2015/01/30 16:59:50 
//功能说明：订单状态变更Vo
//-----------------------------------------------------------------
//修改记录： 增加Id，NotifyCustomer字段
//修改人：   罗海明
//修改时间： 
//修改内容： 
//修改记录： 增加NotifyEmailWithComments字段
//修改人：   罗海明
//修改时间： 2015/04/13 16:59:50 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Order
{
    public class OrderStatusHistory
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 状态变更时间
        /// </summary>
        public virtual DateTime ChangeDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 状态对应语种名称（前台显示）
        /// </summary>
        public virtual string StatusName { get; set; }

        /// <summary>
        /// 变更备注
        /// </summary>
        public virtual string Comments { get; set; }

        /// <summary>
        /// 是否邮件通知客户
        /// </summary>
        public virtual bool NotifyCustomer { get; set; }

        /// <summary>
        /// 发送邮件时是否附加comments
        /// </summary>
        public virtual bool NotifyEmailWithComments { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public virtual int AdminId { get; set; }

    }
}
