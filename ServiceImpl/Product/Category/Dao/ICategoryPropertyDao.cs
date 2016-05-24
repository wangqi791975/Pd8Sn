//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICategoryPropertyDao.cs
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
    public interface ICategoryPropertyDao : IBaseDao<CategoryPropertyPo, int>
    {

        IList<CategoryPropertyPo> GetCategoryBindedAllProperties(int categoryId);

        /// <summary>
        /// 一个类别关联的所有属性ID列表（不重复）
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IList<int> GetCategoryBindedAllPropertiesRecursive(int categoryId);

        CategoryPropertyPo GetCategoryProperty(int categoryId, int propertyId);
    }
}
