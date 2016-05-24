//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MetaListDao.cs
//创 建 人 ：xiaoyong.lv
//创建时间：2015-02-09
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.SEO;

namespace Com.Panduo.ServiceImpl.SEO.Dao
{
    public class MetaListDao : BaseDao<MetaListPo, int>, IMetaListDao
    {
        public IList<MetaListPo> GetMetaListByType(int categoryId, int languageId)
        {
            return FindDataByHql("FROM MetaListPo WHERE CategoryId = ? and LanguageId = ?", new object[] { categoryId, languageId });
        }

        public MetaListPo GetMetaListByType(int categoryId, int languageId, int type)
        {
            return GetOneObject("FROM MetaListPo WHERE CategoryId = ? and LanguageId = ? and PageType = ?", new object[] { categoryId, languageId, type });
        }
    }
}
