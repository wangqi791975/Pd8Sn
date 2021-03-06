﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;
using Com.Panduo.Service;
using Com.Panduo.Service.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public interface IPaymentEnabledCustomerDao : IBaseDao<PaymentEnabledCustomerPo, int>
    {
        /// <summary>
        /// 获取所有GC屏蔽国家
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">搜索关键词</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>GC屏蔽国家分页数据</returns>
        PageData<PaymentEnabledCustomerPo> FindPaymentDisabledCustomerPos(int currentPage, int pageSize,
            IDictionary<PaymentEnabledCustomerSearchCriteria, object> searchCriteria,
            IList<Sorter<PaymentEnabledCustomerSorterCriteria>> sorterCriteria);
    }
}
