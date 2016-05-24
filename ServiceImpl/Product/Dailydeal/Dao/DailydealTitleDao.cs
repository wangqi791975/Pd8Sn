using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;
using Com.Panduo.Entity.Product.Dailydeal;

namespace Com.Panduo.ServiceImpl.Product.Dailydeal.Dao
{
    public class DailydealTitleDao : BaseDao<DailydealTitlePo, int>, IDailydealTitleDao
    {
        public IList<DailydealTitlePo> GetAllTitles(int languageId)
        {
            return FindDataByHql("from DailydealTitlePo where Id.LanguageId=?", languageId);
        }
    }
}
