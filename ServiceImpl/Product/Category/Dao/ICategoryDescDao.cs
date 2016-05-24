//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICategoryDescDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
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
    public interface ICategoryDescDao:IBaseDao<CategoryDescPo,int>
    {

        CategoryDescPo GetCategoryLanguage(int categoryId, int languageId);

        IList<CategoryDescPo> GetCategoryLanguagesByCategoryId(int categoryId);

        IList<CategoryDescPo> GetAllCategoryLanguages();

        IList<CategoryDescPo> GetAllCategoryLanguagesByLanguageId(int languageId);
    } 
}
   