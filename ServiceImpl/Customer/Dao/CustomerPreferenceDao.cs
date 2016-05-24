//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomerPreferenceDao.cs
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

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public class CustomerPreferenceDao : BaseDao<CustomerPreferencePo, int>, ICustomerPreferenceDao
    {
        public CustomerPreferencePo GetPreference(int customerId, string key)
        {
            var obj = GetOneObject("FROM CustomerPreferencePo WHERE CustomerId = ? AND Key = ?", new object[] { customerId, key });
            return obj;
        }

        public IList<CustomerPreferencePo> GetPreferences(int customerId)
        {
            var obj = FindDataByHql("FROM CustomerPreferencePo WHERE CustomerId = ?", customerId);
            return obj;
        }

        public void UpdatePreference(int customerId, string key, string value)
        {
            UpdateObjectByHql("UPDATE CustomerPreferencePo SET Value = ? WHERE CustomerId = ? AND Key = ?", 
                new object[] { value, customerId, key });
        }
    }
}
