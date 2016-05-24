using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Cash;

namespace Com.Panduo.ServiceImpl.Cash.Dao
{
    class CashAccountDao : BaseDao<CashAccountPo, int>, ICashAccountDao
    {
        public CashAccountPo GetCashAccountByCustomerId(int customerId)
        {
            return GetOneObject("from CashAccountPo where CustomerId = ?", customerId);
        }
    }
}
