//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：SpecialOrder.cs
//创 建 人：罗海明
//创建时间：2015/04/07 17:10:50 
//功能说明：后台特殊报价订单Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Order
{
   public class SpecialOrder
    {
       /// <summary>
       /// Id
       /// </summary>
       public virtual int SpecialOrderId { get; set; }

       /// <summary>
       /// 客户Id
       /// </summary>
       public virtual int CustomerId { get; set; }

       /// <summary>
       /// 客户姓名
       /// </summary>
       public virtual string CustomerName { get; set; }

       /// <summary>
       /// 客户邮箱
       /// </summary>
       public virtual string CustomerMail { get; set; }

       /// <summary>
       /// 增加消费金额
       /// </summary>
       public virtual decimal Increase { get; set; }

       /// <summary>
       /// 币种编码
       /// </summary>
       public virtual string CurrencyCode{get;set;}

       /// <summary>
       /// 是否通知客户
       /// </summary>
       public virtual bool IsNotifyCustomer{get;set;}

       /// <summary>
       /// 状态 1活动 -1删除
       /// </summary>
       public virtual int Status { get; set; }

       /// <summary>
       /// 创建时间
       /// </summary>
       public virtual DateTime CreateTime { get; set; }

       /// <summary>
       /// 创建人
       /// </summary>
       public virtual int Creator { get; set; }

       /// <summary>
       /// 创建人账户
       /// </summary>
       public virtual string CreateAccount { get; set; }

       /// <summary>
       /// 备注
       /// </summary>
       public virtual string Remark { get; set; }
    }
}
