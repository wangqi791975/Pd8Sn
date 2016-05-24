//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：TopKeywordSubjectDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using Com.Panduo.Entity.SEO;

namespace Com.Panduo.ServiceImpl.SEO.Dao
{ 
    public class TopKeywordSubjectDao:BaseDao<TopKeywordSubjectPo,int>, ITopKeywordSubjectDao
    {
        public TopKeywordSubjectPo GeTopKeywordSubject(string subjectName)
        {
            return GetOneObject("FROM TopKeywordSubjectPo WHERE Name = ?", subjectName);
        }

        public int GetTopKeywordSubjectId(int id)
        {
            var obj =
                GetSingleObject("SELECT TopKeywordSubjectId FROM TopKeywordSubjectPo WHERE TopKeywordSubjectId = ?", id);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public int GetTopKeywordSubjectId(string name)
        {
            var obj =
                GetSingleObject("SELECT TopKeywordSubjectId FROM TopKeywordSubjectPo WHERE Name = ?", name);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }
    } 
}
   