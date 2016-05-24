//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：Package.cs
//创 建 人：罗海明
//创建时间：2015/02/20 14:49:50 
//功能说明：包裹Vo
//-----------------------------------------------------------------
//修改记录： 增加ErpPackageNumber，OrderNumber字段
//修改人：   罗海明
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 包裹表
    /// </summary>
    [Serializable]
    public class Package
    {
        /// <summary>
        /// 包裹Id
        /// </summary>
        public virtual int PackageId { get; set; }
        
        /// <summary>
        /// ERP包裹编号 add by luohaiming
        /// </summary>
        public virtual string ErpPackageNumber { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 订单编号 add by luohaiming
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 配送方式Id
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 跟踪号
        /// </summary>
        public virtual string TrackingNumber { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public virtual DateTime ShippedDate { get; set; }

        /// <summary>
        /// 客户是否收货
        /// </summary>
        public virtual bool IsReceived { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastModifyTime { get; set; }
    }
}
