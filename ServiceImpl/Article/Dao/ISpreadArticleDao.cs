//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ISpreadArticleDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Entity.Article;
using Com.Panduo.Service;
using Com.Panduo.Service.Article;

namespace Com.Panduo.ServiceImpl.Article.Dao
{ 
    public interface ISpreadArticleDao:IBaseDao<SpreadArticlePo,int>
    {
        PageData<SomeArticle> FindSomeArticle(int currentPage, int pageSize,
            IDictionary<SomeArticleCriteria, object> searchCriteria,
            IList<Sorter<SomeArticleSorterCriteria>> sorterCriteria);
    } 
}
   