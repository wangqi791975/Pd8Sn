using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogMoneyGramDao : BaseDao<PaymentLogMoneyGramPo, int>, IPaymentLogMoneyGramDao
    {
        public IList<PaymentLogMoneyGramPo> GetPaymentLogsByOrderNo(string orderNo)
        {
            return FindDataByHql("from PaymentLogMoneyGramPo where OrderNo = ? order by CreateDate desc", orderNo);
        }
    }
}
