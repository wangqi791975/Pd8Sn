using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public class CustomerProductViewDao : BaseDao<CustomerProductViewPo, int>, ICustomerProductViewDao
    {
        public IList<CustomerProductViewPo> GetCustomerProduct(int customerId)
        {
            return FindDataByHql("FROM CustomerProductViewPo WHERE CustomerId = ?", customerId);
        }
    }
}