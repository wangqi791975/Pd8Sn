//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IChannelDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 2015/03/03
//修改人：   罗海明
//修改时间： 
//修改内容： 添加IsExists方法
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public interface ICustomerHighRiskDao : IBaseDao<CustomerHighRiskPo, int>
    {

        /// <summary>
        /// 根据Email得到客户高危信息
        /// </summary>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        CustomerHighRiskPo GetCustomerHighRiskByCustomerEmail(string customerEmail);

        /// <summary>
        /// 分页查询高危客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<CustomerHighRiskPo> FindCustomerHighRisks(int currentPage, int pageSize,
            IDictionary<CustomerHighRiskSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerHighRiskSorterCriteria>> sorterCriteria);

    } 
}
   