using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentLogWesternUnionDao : BaseDao<PaymentLogWesternUnionPo, int>, IPaymentLogWesternUnionDao
    {
        public IList<PaymentLogWesternUnionPo> GetPaymentLogsByOrderNo(string orderNo)
        { 
            return FindDataByHql("from PaymentLogWesternUnionPo where OrderNo = ? order by CreateDate desc", orderNo);
        }
    }
}
