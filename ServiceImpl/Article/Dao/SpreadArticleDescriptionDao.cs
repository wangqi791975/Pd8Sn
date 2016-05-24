//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：SpreadArticleDescriptionDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 18:08:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Article;

namespace Com.Panduo.ServiceImpl.Article.Dao
{ 
    public class SpreadArticleDescriptionDao:BaseDao<SpreadArticleDescriptionPo,int>, ISpreadArticleDescriptionDao
    {
        public SpreadArticleDescriptionPo GetSpreadArticleDescription(int articleId, int languageId)
        {
            return GetOneObject("from SpreadArticleDescriptionPo where ArticleId = ? And LanguageId = ?", new object[] { articleId, languageId });
        }

        public IList<SpreadArticleDescriptionPo> GetSpreadArticleDescriptions(int articleId) 
        {
            return FindDataByHql("FROM SpreadArticleDescriptionPo WHERE ArticleId = ?", articleId);
        }
    } 
}
   