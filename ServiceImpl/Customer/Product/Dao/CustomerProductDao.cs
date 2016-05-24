using System.Collections.Generic;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public class CustomerProductDao : BaseDao<CustomerProductPo, int>, ICustomerProductDao
    {
        public void DeleteCustomerProduct(int customerId, int productId)
        {
            DeleteObjectByHql("DELETE FROM CustomerProductPo WHERE CustomerId = ? AND ProductId = ?", new object[] { customerId, productId });
        }

        public CustomerProductPo GetCustomerProduct(int customerId, int productId)
        {
            return GetOneObject("FROM CustomerProductPo WHERE CustomerId = ? AND ProductId = ?", new object[] { customerId, productId });
        }

        public IList<CustomerProductPo> GetCustomerProductPos(int customerId)
        {
            return FindDataByHql("FROM CustomerProductPo WHERE CustomerId = ?", customerId);
        }
    }
}