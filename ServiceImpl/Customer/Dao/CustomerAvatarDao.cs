//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomerDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
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
    public class CustomerAvatarDao : BaseDao<CustomerAvatarPo, int>, ICustomerAvatarDao
    {

        public PageData<CustomerAvatarPo> FindAllCustomerAvatars(int currentPage, int pageSize)
        {
            var hqlHelper = new HqlHelper("Select c from CustomerAvatarPo c");
            hqlHelper.AddSorter("c.AuditingStatus", true);
            hqlHelper.AddSorter("c.AvatarId", false);
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }
    }
}
