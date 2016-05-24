//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICategoryDao.cs
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
    public interface ICategoryDao : IBaseDao<CategoryPo, int>
    {

        CategoryPo GetCategoryById(int categoryId);

        CategoryPo GetParentCategoryById(int categoryId);

        IList<CategoryPo> GetAllRootCategories();

        IList<CategoryPo> GetAllSubCategories(int categoryId);

        List<CategoryPo> GetAllCategories();

        IList<CategoryPo> GetAllLeafCategories();

        IList<CategoryPo> GetTopSubCategoriesById(int categoryId, int topN);

        /// <summary>
        /// 判断类别是否末级类别
        /// 查找类别表中是否有parent_id为传入的categoryId，没有则为末级类别
        /// </summary>
        bool IsLeafCategory(int categoryId);


    }
}
