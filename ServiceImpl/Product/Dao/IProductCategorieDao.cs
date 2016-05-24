//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IProductCategorieDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 2015/01/08 
//修改人：  罗海明 
//修改时间： 增加产品类别获取方法
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public interface IProductCategorieDao:IBaseDao<ProductCategoriePo,int>
    {
        /// <summary>
        /// 根据产品ID获取产品类别
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ProductCategoriePo GetProductCategorieByProductId(int productId);

        /// <summary>
        /// 根据类别ID获取产品类别
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <returns></returns>
        IList<ProductCategoriePo> GetProductCategorieByCategoryId(int categoryId);
    } 
}
   