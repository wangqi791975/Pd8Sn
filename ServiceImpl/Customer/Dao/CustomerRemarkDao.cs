using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public class CustomerRemarkDao : BaseDao<CustomerRemarkPo, int>, ICustomerRemarkDao
    {
        public CustomerRemarkPo GetCustomerRemarkPo(int customerId)
        {
            return GetOneObject("FROM CustomerRemarkPo WHERE CustomerId = ?", customerId);
        }

        public void UpdateCustomerRemarkPo(int customerId, string remark, int adminid)
        {
            UpdateObjectByHql("UPDATE CustomerRemarkPo SET Remark = ? ,AdminId = ? WHERE CustomerId = ?", new object[] { remark, adminid, customerId });
        }
    }
}