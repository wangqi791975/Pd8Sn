//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CategoryKeywordDao.cs
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
using Com.Panduo.Entity.Product.Category;

namespace Com.Panduo.ServiceImpl.Product.Category.Dao
{
    public class CategoryKeywordDao : BaseDao<CategoryKeywordPo, int>, ICategoryKeywordDao
    {

        public IList<CategoryKeywordPo> GetCategoryKeywords(int categoryId, int languageId)
        {
            return FindDataByHql("from CategoryKeywordPo where CategoryId=? and LanguageId=? ORDER BY SortIndex ASC", new object[] { categoryId, languageId });
        }


        public CategoryKeywordPo GetCategoryKeyword(int categoryId, int languageId, string keyword)
        {
            return GetOneObject("from CategoryKeywordPo where CategoryId=? and LanguageId=? and Keyword=?", new object[] { categoryId, languageId, keyword });
        }

        public void DeleteCategoryKeyword(int categoryId)
        {
            DeleteObjectByHql("DELETE FROM CategoryKeywordPo WHERE CategoryId = ?", categoryId);
        }

        public void DeleteCategoryKeyword(int categoryId, int langId)
        {
            DeleteObjectByHql("DELETE FROM CategoryKeywordPo WHERE CategoryId = ? AND LanguageId = ?", new object[] { categoryId, langId });
        }
    }
}
