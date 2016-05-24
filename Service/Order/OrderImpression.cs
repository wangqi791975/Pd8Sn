//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：OrderImpression.cs
//创 建 人：罗海明
//创建时间：2015/01/30 16:59:50 
//功能说明：订单快照Vo
//-----------------------------------------------------------------
//修改记录： 增加CategoryId，CategoryName，ProductName字段
//修改人：   罗海明
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 订单快照
    /// </summary>
    [Serializable]
    public class OrderImpression
    {
        /// <summary>
        /// 主键：自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 明细Id
        /// </summary>
        public virtual int OrderItemId { get; set; }

        /// <summary>
        /// 对于语种的产品名称
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 产品所属类别Id
        /// </summary>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 对应语种的类别名称
        /// </summary>
        public virtual string CategoryName { get; set; }

        /// <summary>
        /// 键值对(数据库中存储JSON格式)
        /// </summary>
        public virtual IDictionary<string, string> ProductPropertyStr { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 产品图片集合
        /// </summary>
        public virtual List<string> ProductImages { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastModifyTime { get; set; }
    }
}
