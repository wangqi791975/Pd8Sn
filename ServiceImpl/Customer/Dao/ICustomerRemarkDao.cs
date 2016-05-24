using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public interface ICustomerRemarkDao : IBaseDao<CustomerRemarkPo, int>
    {
        /// <summary>
        /// 获取销售备注信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>销售备注信息</returns>
        CustomerRemarkPo GetCustomerRemarkPo(int customerId);

        /// <summary>
        /// 修改客户备注
        /// </summary>
        /// <param name="customerId">Id</param>
        /// <param name="remark">备注</param>
        /// <param name="adminId">操作人员Id</param>
        void UpdateCustomerRemarkPo(int customerId, string remark, int adminId);
    }
}