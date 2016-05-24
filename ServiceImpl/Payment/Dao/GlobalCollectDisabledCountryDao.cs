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
    public class GlobalCollectDisabledCountryDao : BaseDao<GlobalCollectDisabledCountryPo, int>, IGlobalCollectDisabledCountryDao
    {
        public PageData<GlobalCollectDisabledCountryPo> FindGlobalCollectDisabledCountries(int currentPage, int pageSize,
            IDictionary<DisabledCountrySearchCriteria, object> searchCriteria,
            IList<Sorter<DisabledCountrySorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("Select p from GlobalCollectDisabledCountryPo p");

            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case DisabledCountrySearchCriteria.KeyWord:
                            hqlHelper.AddWhere(string.Format("(p.EnglisName Like {0}  or p.Chinese Like {0})", ":keyWord"), HqlOperator.Exp, "keyWord", string.Format("%{0}%", item.Value));
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
                        case DisabledCountrySorterCriteria.DateCreated:
                            hqlHelper.AddSorter("p.DateCreated", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("p.DateCreated", false);
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }
    }
}
