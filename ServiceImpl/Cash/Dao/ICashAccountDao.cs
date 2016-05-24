using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Cash;
using Com.Panduo.Service.Cash;

namespace Com.Panduo.ServiceImpl.Cash.Dao
{
    public interface ICashAccountDao : IBaseDao<CashAccountPo, int>
    {
        /// <summary>
        /// 通过客户ID获取CashAccount实体
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns>CashAccount</returns>
        CashAccountPo GetCashAccountByCustomerId(int customerId);
    }
}
