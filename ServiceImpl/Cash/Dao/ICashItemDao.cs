using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Cash;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;

namespace Com.Panduo.ServiceImpl.Cash.Dao
{
    public interface ICashItemDao : IBaseDao<CashItemPo, int>
    {
        /// <summary>
        /// 获取所有客户的Cash明细
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">搜索关键词(客户ID、客户姓名、客户邮箱)</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>CashItem分页数据</returns>
        PageData<CashItemPo> FindAllCashItems(int currentPage, int pageSize,  IDictionary<CashItemSearchCriteria, object> searchCriteria, IList<Sorter<CashItemSorterCriteria>> sorterCriteria);
    }
}
