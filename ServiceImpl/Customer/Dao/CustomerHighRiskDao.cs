//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ChannelDao.cs
//创 建 人：万天文
//创建时间：2015/05/03 15:08:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public class CustomerHighRiskDao : BaseDao<CustomerHighRiskPo, int>, ICustomerHighRiskDao
    {
        public CustomerHighRiskPo GetCustomerHighRiskByCustomerEmail(string customerEmail)
        {
            return GetOneObject("from CustomerHighRiskPo where CustomerEmail = ?", new object[] { customerEmail });
        }

        public PageData<CustomerHighRiskPo> FindCustomerHighRisks(int currentPage, int pageSize,
            IDictionary<CustomerHighRiskSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerHighRiskSorterCriteria>> sorterCriteria)
        {
            var hqlHelpder = new HqlHelper("Select c from CustomerHighRiskPo c");
            if (sorterCriteria == null)
            {
                hqlHelpder.AddSorter("DateCreated", false);
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelpder.Hql, hqlHelpder.ParamMap);
        }
    } 
}
   