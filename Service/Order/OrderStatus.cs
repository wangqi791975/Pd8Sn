//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：OrderStatus.cs
//创 建 人：罗海明
//创建时间：2015/01/30 16:59:50 
//功能说明：订单状态Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;


namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 订单状态
    /// </summary>
    [Serializable]
    public class OrderStatus
    {
        /// <summary>
        /// 状态Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 状态Value
        /// </summary>
        public virtual int Value { get; set; }

    }
}
