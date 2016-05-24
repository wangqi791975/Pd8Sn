//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CategoryDescDao.cs
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
    public class CategoryDescDao : BaseDao<CategoryDescPo, int>, ICategoryDescDao
    {

        public CategoryDescPo GetCategoryLanguage(int categoryId, int languageId)
        {
            return GetOneObject("from CategoryDescPo where CategoryId=? and LanguageId=?", new object[] { categoryId, languageId });
        }

        public IList<CategoryDescPo> GetCategoryLanguagesByCategoryId(int categoryId)
        {
            return FindDataByHql("FROM CategoryDescPo where CategoryId=?", categoryId);
        }

        public IList<CategoryDescPo> GetAllCategoryLanguages()
        {
            return FindDataByHql("FROM CategoryDescPo ");
        }

        public IList<CategoryDescPo> GetAllCategoryLanguagesByLanguageId(int languageId)
        {
            return FindDataByHql("FROM CategoryDescPo where LanguageId=?", languageId);
        }
    }
}
