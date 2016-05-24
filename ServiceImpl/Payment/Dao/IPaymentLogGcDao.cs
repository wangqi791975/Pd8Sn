using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public interface IPaymentLogGcDao : IBaseDao<PaymentLogGcPo,int>
    {
        PaymentLogGcPo GetPaymentLogByTransactionId(string transactionId);

        IList<PaymentLogGcPo> GetPaymentLogsByOrderNo(string orderNo);
    }
}
