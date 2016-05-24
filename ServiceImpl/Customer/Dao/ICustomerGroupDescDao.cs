//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerGroupDescDao.cs
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
    public interface ICustomerGroupDescDao:IBaseDao<CustomerGroupDescPo,int>
    {
        /// <summary>
        /// 通过等级id修改名称
        /// </summary>
        /// <param name="id">等级Id</param>
        /// <param name="name">等级名称</param>
        void UpdateCustomerGroupName(int id, string name);

        /// <summary>
        /// 获取等级描述
        /// </summary>
        /// <param name="customerGroupId">客户等级Id</param>
        /// <param name="languageId">语言Id</param>
        /// <returns>客户等级描述</returns>
        CustomerGroupDescPo GetCustomerGroupDesc(int customerGroupId, int languageId);

        /// <summary>
        /// 获取等级描述
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        IList<CustomerGroupDescPo> GetCustomerGroupDescs(int languageId);

        /// <summary>
        /// 通过客户组Id获取描述
        /// </summary>
        /// <param name="customerGroupId">客户组Id</param>
        /// <returns>客户等级描述</returns>
        IList<CustomerGroupDescPo> GetCustomerGroupDescsByCustomerGroupId(int customerGroupId);
    } 
}
   