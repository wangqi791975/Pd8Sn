//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：TopKeywordDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
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
    public class TopKeywordDao : BaseDao<TopKeywordPo, int>, ITopKeywordDao
    {
        public TopKeywordPo GeTopKeyword(string name)
        {
            return GetOneObject("FROM TopKeywordPo WHERE Name = ?", name);
        }

        public IList<TopKeywordPo> GetTopKeywords(int subjectIds)
        {
            return FindDataByHql("FROM TopKeywordPo WHERE TopKeywordSubjectId = ?", subjectIds);
        }

        public void DeleteKeywordBySubjectId(int subjectId)
        {
            DeleteObjectByHql("delete FROM TopKeywordPo WHERE TopKeywordSubjectId = ?", subjectId);
        }
    }
}
