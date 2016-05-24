//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomerGroupDescDao.cs
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
    public class CustomerGroupDescDao : BaseDao<CustomerGroupDescPo, int>, ICustomerGroupDescDao
    {
        public void UpdateCustomerGroupName(int id, string name)
        {
            UpdateObjectByHql("UPDATE CustomerGroupDescPo SET Name = ? WHERE Id = ?", new object[] { name, id });
        }

        public CustomerGroupDescPo GetCustomerGroupDesc(int customerGroupId, int languageId)
        {
            return GetOneObject("FROM CustomerGroupDescPo WHERE CustomerGroupId = ? AND LanguageId = ?", new object[] { customerGroupId, languageId });
        }

        public IList<CustomerGroupDescPo> GetCustomerGroupDescs(int languageId)
        {
            return FindDataByHql("FROM CustomerGroupDescPo WHERE LanguageId = ?", languageId);
        }

        public IList<CustomerGroupDescPo> GetCustomerGroupDescsByCustomerGroupId(int customerGroupId)
        {
            return FindDataByHql("FROM CustomerGroupDescPo WHERE CustomerGroupId = ?", customerGroupId);
        }
    }
}
