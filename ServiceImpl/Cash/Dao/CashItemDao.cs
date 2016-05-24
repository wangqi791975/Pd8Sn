using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Cash;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;

namespace Com.Panduo.ServiceImpl.Cash.Dao
{
    class CashItemDao : BaseDao<CashItemPo, int>, ICashItemDao
    {

        public PageData<CashItemPo> FindAllCashItems(int currentPage, int pageSize,  IDictionary<CashItemSearchCriteria, object> searchCriteria, IList<Sorter<CashItemSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("Select p from CashItemPo p");

            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CashItemSearchCriteria.CustomerId:
                            hqlHelper.AddWhere("p.CustomerId", HqlOperator.Eq, "CustomerId", item.Value);
                            break;
                        case CashItemSearchCriteria.Keyword:
                            hqlHelper.AddWhere(string.Format("(p.CustomerEmail Like {0}  or p.Comment Like {0})", ":keyWord"), HqlOperator.Exp, "keyWord", string.Format("%{0}%", item.Value));
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
                        case CashItemSorterCriteria.OpDate:
                            hqlHelper.AddSorter("p.OpDate", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("p.Id", false);
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }
    }
}
