using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public interface IPaymentLogOceanPaymentDao : IBaseDao<PaymentLogOceanPaymentPo,int>
    {
        PaymentLogOceanPaymentPo GetPaymentLogByTransactionId(string transactionId);

        IList<PaymentLogOceanPaymentPo> GetPaymentLogsByOrderNo(string orderNo);
    }
}
