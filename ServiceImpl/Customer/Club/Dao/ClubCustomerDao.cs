using System;
using Com.Panduo.Entity.Customer.Club;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Club.Dao
{
    public class ClubCustomerDao : BaseDao<ClubCustomerPo, int>, IClubCustomerDao
    {
        public ClubCustomerPo GetClubCustomer(int customerId)
        {
            return GetOneObject("FROM ClubCustomerPo WHERE CustomerId = ?", customerId);
        }

        public ClubCustomerPo GetValidClubCustomer(int customerId)
        {
            DateTime currentDateTime = DateTime.Now;
            return GetOneObject("FROM ClubCustomerPo WHERE CustomerId = ? AND EndedDate > ? AND AddedDate < ?", new object[] { customerId, currentDateTime, currentDateTime });
        }

        public void UpdateClubCustomer(int clubCustomerId, PaymentStatus paymentStatus)
        {
            UpdateObjectByHql("UPDATE ClubCustomerPo SET PaymentStatus = ? WHERE Id = ?", new object[] { paymentStatus, clubCustomerId });
        }

        public void UpdateClubCustomer(int clubCustomerId, int customerManagerId)
        {
            UpdateObjectByHql("UPDATE ClubCustomerPo SET CustomerManagerId = ? WHERE Id = ?", new object[] { customerManagerId, clubCustomerId });
        }
    }
}