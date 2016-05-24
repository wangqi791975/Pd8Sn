using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.SiteConfigure;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.SiteConfigure.Dao
{
    public class SearchKeywordDao : BaseDao<SearchKeywordPo, int>, ISearchKeywordDao
    {
        public SearchKeywordPo GetOneSearchKeyword(int languageId, string name, int type)
        {
            return GetOneObject("from SearchKeywordPo where LanguageId=? and Name=? And Type=?", new object[] { languageId, name, type });
        }

        public IList<SearchKeywordPo> GetSearchKeywordByType(KeywordType type)
        {
            return FindDataByHql("from SearchKeywordPo where Type= ?", type.ParseTo<int>());
        }
    }
}
