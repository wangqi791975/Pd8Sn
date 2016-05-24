using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Payment;
using Com.Panduo.Service;
using Com.Panduo.Service.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class PaymentEnabledCustomerDao : BaseDao<PaymentEnabledCustomerPo, int>, IPaymentEnabledCustomerDao
    {
        public PageData<PaymentEnabledCustomerPo> FindPaymentDisabledCustomerPos(int currentPage, int pageSize,
            IDictionary<PaymentEnabledCustomerSearchCriteria, object> searchCriteria,
            IList<Sorter<PaymentEnabledCustomerSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("from PaymentEnabledCustomerPo");

            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case PaymentEnabledCustomerSearchCriteria.KeyWord:
                            hqlHelper.AddWhere(string.Format("CustomerEmail Like {0}", ":keyWord"), HqlOperator.Exp, "keyWord", string.Format("%{0}%", item.Value));
                            break;
                        case PaymentEnabledCustomerSearchCriteria.PaymentType:
                            hqlHelper.AddWhere("PaymentType", HqlOperator.Eq, "PaymentType", item.Value);
                            break;
                    }
                }
            }

            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case PaymentEnabledCustomerSorterCriteria.DateCreated:
                            hqlHelper.AddSorter("DateCreated", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("DateCreated", false);
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }
    }
}
