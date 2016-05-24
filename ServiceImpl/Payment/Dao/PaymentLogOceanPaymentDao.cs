using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogOceanPaymentDao : BaseDao<PaymentLogOceanPaymentPo, int>, IPaymentLogOceanPaymentDao
    {
        public PaymentLogOceanPaymentPo GetPaymentLogByTransactionId(string transactionId)
        {
            return GetOneObject("from PaymentLogOceanPaymentPo where PaymentId = ?", transactionId);
        }

        public IList<PaymentLogOceanPaymentPo> GetPaymentLogsByOrderNo(string orderNo)
        {
            return FindDataByHql("from PaymentLogOceanPaymentPo where OrderNumber = ? order by CreateDate desc", orderNo);
        }
    }
}
