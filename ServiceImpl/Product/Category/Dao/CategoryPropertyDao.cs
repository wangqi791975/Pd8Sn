//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CategoryPropertyDao.cs
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
using System.Data;
using System.Data.SqlClient;
using Com.Panduo.Entity.Product.Category;

namespace Com.Panduo.ServiceImpl.Product.Category.Dao
{
    public class CategoryPropertyDao : BaseDao<CategoryPropertyPo, int>, ICategoryPropertyDao
    {

        public IList<CategoryPropertyPo> GetCategoryBindedAllProperties(int categoryId)
        {
            return FindDataByHql("FROM CategoryPropertyPo where CategoryId=?", categoryId);
        }

        public CategoryPropertyPo GetCategoryProperty(int categoryId, int propertyId)
        {
            return GetOneObject("from CategoryPropertyPo where CategoryId=? and PropertyId=?", new object[] { categoryId, propertyId });
        }


        public IList<int> GetCategoryBindedAllPropertiesRecursive(int categoryId)
        {
            var list = new List<int>();

            var parameters = new[]
                                 {
                                     new SqlParameter("@CategoryId",SqlDbType.Int){Value = categoryId}
                                 };

            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_category_property_get_recursive", parameters))
            { 
                while (reader.Read())
                { 
                    list.Add(reader.GetInt32(0));
                }
            }

            return list;
        }
    }
}
