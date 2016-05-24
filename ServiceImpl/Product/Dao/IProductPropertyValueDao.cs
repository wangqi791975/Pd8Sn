//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IProductPropertyValueDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
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
using Com.Panduo.Service.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public interface IProductPropertyValueDao:IBaseDao<ProductPropertyValuePo,int>
    {
        /// <summary>
        /// 通过产品Id获取产品绑定的属性值列表
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>属性值列表</returns>
        IList<ProductPropertyValuePo> GetProductBindedAllPropertyValues(int productId);

        /// <summary>
        /// 通过产品ID和属性ID得到ProductPropertyValue Po
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="propertyId">属性ID</param>
        /// <returns></returns>
        ProductPropertyValuePo GetProductProductPropertyValue(int productId, int propertyId);
    } 
}
   