//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MetaHomeDao.cs
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
    public class MetaHomeDao : BaseDao<MetaHomePo, int>, IMetaHomeDao
    {
        public IList<MetaHomePo> GetMetaHomesByLanguageId(int languageId)
        {
            return FindDataByHql("FROM MetaHomePo WHERE LanguageId = ?", languageId);
        }

        public MetaHomePo GetMetaHomeByType(int type, int languageId)
        {
            return GetOneObject("FROM MetaHomePo WHERE PageType = ? and LanguageId = ?", new object[] { type, languageId });
        }
    }
}
