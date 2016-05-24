//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICategoryKeywordDao.cs
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
    public interface ICategoryKeywordDao : IBaseDao<CategoryKeywordPo, int>
    {

        IList<CategoryKeywordPo> GetCategoryKeywords(int categoryId, int languageId);

        CategoryKeywordPo GetCategoryKeyword(int categoryId, int languageId, string keyword);

        /// <summary>
        /// 通过类别Id删除关键字
        /// </summary>
        /// <param name="categoryId"></param>
        void DeleteCategoryKeyword(int categoryId);

        /// <summary>
        /// 通过类别Id和语言Id删除关键字
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="langId"></param>
        void DeleteCategoryKeyword(int categoryId,int langId);
    }
}
