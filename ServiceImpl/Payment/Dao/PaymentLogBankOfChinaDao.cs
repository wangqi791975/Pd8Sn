using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogBankOfChinaDao : BaseDao<PaymentLogBankOfChinaPo, int>, IPaymentLogBankOfChinaDao
    {
        public IList<PaymentLogBankOfChinaPo> GetPaymentLogsByOrderNo(string orderNo)
        {
            return FindDataByHql("from PaymentLogBankOfChinaPo where OrderNo = ? order by CreateDate desc", orderNo);
        }
    }
}
