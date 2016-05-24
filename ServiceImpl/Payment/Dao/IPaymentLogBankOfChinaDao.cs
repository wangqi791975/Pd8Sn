using System.Collections.Generic;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public interface IPaymentLogBankOfChinaDao : IBaseDao<PaymentLogBankOfChinaPo,int>
    {
        IList<PaymentLogBankOfChinaPo> GetPaymentLogsByOrderNo(string orderNo);
    }
}
