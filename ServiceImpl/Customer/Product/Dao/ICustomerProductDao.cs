using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public interface ICustomerProductDao : IBaseDao<CustomerProductPo,int>
    {
        /// <summary>
        /// 删除客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        void DeleteCustomerProduct(int customerId, int productId);

        /// <summary>
        /// 获取客户产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns>客户产品</returns>
        CustomerProductPo GetCustomerProduct(int customerId, int productId);

        /// <summary>
        /// 获取客户绑定产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户绑定产品</returns>
        IList<CustomerProductPo> GetCustomerProductPos(int customerId);
    }
}