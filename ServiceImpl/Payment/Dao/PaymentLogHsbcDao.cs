using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogHsbcDao : BaseDao<PaymentLogHsbcPo, int>, IPaymentLogHsbcDao
    {
        public IList<PaymentLogHsbcPo> GetPaymentLogsByOrderNo(string orderNo)
        {
            return FindDataByHql("from PaymentLogHsbcPo where OrderNo = ? order by CreateDate desc", orderNo);
        }
    }
}
