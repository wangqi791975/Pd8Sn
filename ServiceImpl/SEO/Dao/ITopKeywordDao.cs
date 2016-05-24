//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ITopKeywordDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections;
using System.Collections.Generic;
using Com.Panduo.Entity.SEO;

namespace Com.Panduo.ServiceImpl.SEO.Dao
{ 
    public interface ITopKeywordDao:IBaseDao<TopKeywordPo,int>
    {
        /// <summary>
        /// 通过名称获取关键词
        /// </summary>
        /// <param name="name">关键词名称</param>
        /// <returns>关键词</returns>
        TopKeywordPo GeTopKeyword(string name);

        /// <summary>
        /// 通过主题Id获取关键词
        /// </summary>
        /// <param name="subjectIds">主题Id</param>
        /// <returns>关键词集合</returns>
        IList<TopKeywordPo> GetTopKeywords(int subjectIds);

        void DeleteKeywordBySubjectId(int subjectId);
    } 
}
   