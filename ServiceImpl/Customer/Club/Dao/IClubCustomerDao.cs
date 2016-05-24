using System.Security.Cryptography.X509Certificates;
using Com.Panduo.Entity.Customer.Club;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Club.Dao
{
    public interface IClubCustomerDao:IBaseDao<ClubCustomerPo,int>
    {
        /// <summary>
        /// 通过客户Id获取club客户
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>club客户</returns>
        ClubCustomerPo GetClubCustomer(int customerId);

        /// <summary>
        /// 通过客户Id获取有效club客户
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>club客户</returns>
        ClubCustomerPo GetValidClubCustomer(int customerId);

        /// <summary>
        /// 修改club支付状态
        /// </summary>
        /// <param name="clubCustomerId">自增Id</param>
        /// <param name="paymentStatus">支付状态</param>
        void UpdateClubCustomer(int clubCustomerId,PaymentStatus paymentStatus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clubCustomerId">自增Id</param>
        /// <param name="customerManagerId">客服Id</param>
        void UpdateClubCustomer(int clubCustomerId, int customerManagerId);
    }
}