//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IProductImageDao.cs
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
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public interface IProductImageDao:IBaseDao<ProductImagePo,int>
    {
        /// <summary>
        /// 根据产品Id获取产品图片信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品主图信息</returns>
        IList<ProductImagePo> GetAllProductImagesById(int productId);
        
        /// <summary>
        /// 根据产品Ids获取产品所有图片
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <returns></returns>
        IList<ProductImagePo> GetAllProductImagesByIds(string productIds);

        /// <summary>
        /// 获取产品主图信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品主图信息</returns>
        ProductImagePo GetProductMainImage(int productId);
    } 
}
   