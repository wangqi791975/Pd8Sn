using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogGcDao : BaseDao<PaymentLogGcPo, int>, IPaymentLogGcDao
    {
        public PaymentLogGcPo GetPaymentLogByTransactionId(string transactionId)
        {
            return GetOneObject("from PaymentLogGcPo where GcOrderId = ?", transactionId);
        }

        public IList<PaymentLogGcPo> GetPaymentLogsByOrderNo(string orderNo)
        {
            return FindDataByHql("from PaymentLogGcPo where OrderNo = ? order by CreateDate desc", orderNo);
        }
    }
}
