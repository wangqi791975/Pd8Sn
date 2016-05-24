using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public interface IPaymentLogPaypalDao : IBaseDao<PaymentLogPaypalPo,int>
    {
        /// <summary>
        /// 根据交易号获取交易日志
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        PaymentLogPaypalPo GetPaymentLogByTransactionId(string transactionId);

        IList<PaymentLogPaypalPo> GetPaymentLogsByOrderNo(string orderNo);
    }
}
