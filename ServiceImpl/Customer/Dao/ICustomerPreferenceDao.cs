//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerPreferenceDao.cs
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
    public interface ICustomerPreferenceDao:IBaseDao<CustomerPreferencePo,int>
    {
        /// <summary>
        /// 通过客户Id和偏好key获取使用偏好
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="key">使用偏好key</param>
        /// <returns>使用偏好实体</returns>
        CustomerPreferencePo GetPreference(int customerId, string key);

        /// <summary>
        /// 通过客户Id获取使用偏好
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户所有的使用偏好</returns>
        IList<CustomerPreferencePo> GetPreferences(int customerId);

        /// <summary>
        /// 修改客户使用偏好
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        void UpdatePreference(int customerId, string key, string value);
    } 
}
   