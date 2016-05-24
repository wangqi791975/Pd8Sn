//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductPropertyValueDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class ProductPropertyValueDao:BaseDao<ProductPropertyValuePo,int>, IProductPropertyValueDao
    {
        /// <summary>
        /// 通过产品Id获取产品绑定的属性值列表
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>属性值列表</returns>
        public IList<ProductPropertyValuePo> GetProductBindedAllPropertyValues(int productId)
        {
            return FindDataByHql("from ProductPropertyValuePo where ProductId=?", productId);
        }


        public ProductPropertyValuePo GetProductProductPropertyValue(int productId, int propertyId)
        {
            return GetOneObject("from ProductPropertyValuePo where ProductId = ? And PropertyId = ?", new object[] { productId, propertyId });
        }
    } 
}
   