using System.Collections.Generic;
using Com.Panduo.Entity.Customer;
using NHibernate.Mapping;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public interface ICustomerProductViewDao:IBaseDao<CustomerProductViewPo,int>
    {
        /// <summary>
        /// 通过客户Id获取客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户产品</returns>
        IList<CustomerProductViewPo> GetCustomerProduct(int customerId);
    }
}