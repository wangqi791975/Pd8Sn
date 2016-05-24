//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerDao.cs
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
using Com.Panduo.Service;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public interface ICustomerAvatarDao : IBaseDao<CustomerAvatarPo, int>
    {
       
        /// <summary>
        /// 查询数据库得到所有客户头像
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        PageData<CustomerAvatarPo> FindAllCustomerAvatars(int currentPage, int pageSize);
    } 
}
   