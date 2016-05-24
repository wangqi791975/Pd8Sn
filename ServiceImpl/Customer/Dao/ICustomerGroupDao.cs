//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerGroupDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{ 
    public interface ICustomerGroupDao:IBaseDao<CustomerGroupPo,int>
    {
        /// <summary>
        /// 获取折扣
        /// </summary>
        /// <param name="orderAmount"></param>
        /// <returns></returns>
        decimal GetCustomerDiscount(decimal orderAmount);

        /// <summary>
        /// 获取下一等级的对象
        /// </summary>
        /// <param name="localPercentage">当前等级折扣</param>
        /// <returns>客户等级对象</returns>
        CustomerGroupPo GetNextCustomerGroup(decimal localPercentage);
    } 
}
   