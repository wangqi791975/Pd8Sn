//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ITopKeywordSubjectDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
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
    public interface ITopKeywordSubjectDao:IBaseDao<TopKeywordSubjectPo,int>
    {
        /// <summary>
        /// 通过主题名称获取主题
        /// </summary>
        /// <param name="subjectName">主题名称</param>
        /// <returns>主题</returns>
        TopKeywordSubjectPo GeTopKeywordSubject(string subjectName);

        /// <summary>
        /// 通过主题Id获取主题Id
        /// </summary>
        /// <param name="id">主题Id</param>
        /// <returns>主题Id</returns>
        int GetTopKeywordSubjectId(int id);

        /// <summary>
        /// 通过主题名称获取主题Id
        /// </summary>
        /// <param name="name">主题名称</param>
        /// <returns>主题Id</returns>
        int GetTopKeywordSubjectId(string name);
    } 
}
   