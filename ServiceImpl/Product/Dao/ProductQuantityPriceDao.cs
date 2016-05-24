//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductQuantityPriceDao.cs
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
using Com.Panduo.Entity.Product;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class ProductQuantityPriceDao:BaseDao<ProductQuantityPricePo,int>, IProductQuantityPriceDao
    {
        public IList<ProductQuantityPricePo> GetProductQuantityPrices(int productId)
        {
            return FindDataByHql("from ProductQuantityPricePo where ProductId=?", productId);
        }

        public IList<ProductQuantityPricePo> GetProductQuantityPricesBatch(string productIds)
        {
            return FindDataByHql(string.Format("from ProductQuantityPricePo where ProductId in({0})", productIds));
        }

        public IList<ProductQuantityPricePo> GetProductQuantityPrices(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return new List<ProductQuantityPricePo>();
            }
            return FindDataByHql(string.Format("from ProductQuantityPricePo where ProductId in ({0})", productIds)); 
        }

        public ProductQuantityPricePo GetProductQuantityPriceByPurchaseQty(int productId, int purchaseQuantity)
        {
            return GetOneObject("from ProductQuantityPricePo where ProductId = ? And Quantity >= ?", new object[] { productId, purchaseQuantity });
        }

        public ProductQuantityPricePo GetProductQuantityPrice(int productId, int purchaseQuantity)
        {
            return GetOneObject("from ProductQuantityPricePo where ProductId = ? And Quantity = ?", new object[] { productId, purchaseQuantity });
        }
    } 
}
   