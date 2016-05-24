//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductImageDao.cs
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
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Product;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class ProductImageDao:BaseDao<ProductImagePo,int>, IProductImageDao
    {
        public IList<ProductImagePo> GetAllProductImagesById(int productId)
        {
            return FindDataByHql("from ProductImagePo where ProductId = ? ", productId);  
        }

        public ProductImagePo GetProductMainImage(int productId)
        {
            return GetOneObject("from ProductImagePo where ProductId = ? And IsMain = ? ",new object[]{ productId,true});  
        }


        public IList<ProductImagePo> GetAllProductImagesByIds(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return new List<ProductImagePo>();
            }

            return FindDataByHql(string.Format("from ProductImagePo where ProductId in ({0})", productIds)); 
        }
    } 
}
   