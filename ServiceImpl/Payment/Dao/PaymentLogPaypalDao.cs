using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogPaypalDao : BaseDao<PaymentLogPaypalPo, int>, IPaymentLogPaypalDao
    {
        public PaymentLogPaypalPo GetPaymentLogByTransactionId(string transactionId)
        {
            return GetOneObject("from PaymentLogPaypalPo where TxnId = ?", transactionId);
        }

        public IList<PaymentLogPaypalPo> GetPaymentLogsByOrderNo(string orderNo)
        {
            return FindDataByHql("from PaymentLogPaypalPo where ItemNumber = ? order by CreateDate desc", orderNo);
        }
    }
}
