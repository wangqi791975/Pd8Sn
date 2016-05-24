//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：OrderRemark.cs
//创 建 人：罗海明
//创建时间：2015/04/14 16:59:50 
//功能说明：订单备注Vo
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
    /// 订单备注Vo
    /// </summary>
   public class OrderRemark
    {

        public virtual int RemarkId {get;set;}

        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int OrderId{get;set;}

        /// <summary>
        /// 如10:业务优惠金额，20:业务附加费，30:Seller Memo
        /// </summary>
        public virtual OrderRemarkType RemarkType{get;set;}

        /// <summary>
        /// 备注内容
        /// </summary>
        public virtual string RemarkContent {get;set;}

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated{get;set;}

        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int AdminId{get;set;}
    }

    /// <summary>
    /// 订单备注类型
    /// </summary>
   public enum OrderRemarkType
   {
       /// <summary>
       /// 业务优惠金额
       /// </summary>
       BusinessDiscount=10,

       /// <summary>
       /// 业务附加费
       /// </summary>
       BusinessSurcharge=20,

       /// <summary>
       /// Seller Memo
       /// </summary>
       SellerMemo=30,
   }
}
