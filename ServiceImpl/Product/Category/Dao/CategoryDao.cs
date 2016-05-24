//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CategoryDao.cs
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
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Category;

namespace Com.Panduo.ServiceImpl.Product.Category.Dao
{
    public class CategoryDao : BaseDao<CategoryPo, int>, ICategoryDao
    {

        public CategoryPo GetCategoryById(int categoryId)
        {
            return GetOneObject("from CategoryPo where CategoryId=?", categoryId);
        }

        public CategoryPo GetParentCategoryById(int categoryId)
        {
            return GetOneObject("from CategoryPo where CategoryId in ( Select ParentId from CategoryPo where CategoryId =?)", categoryId);
        }

        /// <summary>
        /// 判断类别是否末级类别
        /// 查找类别表中是否有parent_id为传入的categoryId，没有则为末级类别
        /// </summary>
        public bool IsLeafCategory(int categoryId)
        {
            return GetOneObject("from CategoryPo where ParentId=?", categoryId).IsNullOrEmpty();
        }

        /// <summary>
        /// 获取所有一级类别
        /// </summary>
        public IList<CategoryPo> GetAllRootCategories()
        {
            return FindDataByHql("from CategoryPo where ParentId=?", 0);
        }

        /// <summary>
        /// 根据类别Id获取子类别
        /// </summary>
        public IList<CategoryPo> GetAllSubCategories(int categoryId)
        {
            return FindDataByHql("from CategoryPo where ParentId=?", categoryId);
        }

        public List<CategoryPo> GetAllCategories()
        {
            return FindDataByHql("from CategoryPo ").ToList();
        }

        /// <summary>
        /// 获取所有末级类别
        /// </summary>
        public IList<CategoryPo> GetAllLeafCategories()
        {
            return FindDataByHql("FROM CategoryPo a WHERE NOT EXISTS(SELECT 1 FROM CategoryPo b WHERE b.ParentId=a.CategoryId)");
        }

        public IList<CategoryPo> GetTopSubCategoriesById(int categoryId, int topN)
        {
            return FindDataByHqlLimit("FROM CategoryPo where ParentId=?", topN, categoryId);
        }


    }
}
