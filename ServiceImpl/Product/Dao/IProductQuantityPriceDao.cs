//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IProductQuantityPriceDao.cs
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
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public interface IProductQuantityPriceDao:IBaseDao<ProductQuantityPricePo,int>
    {
        /// <summary>
        /// 获取产品梯度价格
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品梯度价格列表</returns>
        IList<ProductQuantityPricePo> GetProductQuantityPrices(int productId);
        /// <summary>
        /// 批量获取产品梯度价格
        /// </summary>
        /// <param name="productIds">产品Id列表，逗号分隔</param>
        /// <returns>产品梯度价格列表</returns>
        IList<ProductQuantityPricePo> GetProductQuantityPricesBatch(string productIds);

        /// <summary>
        /// 根据产品Ids获取产品梯度价格
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <returns>产品梯度价格列表</returns>
        IList<ProductQuantityPricePo> GetProductQuantityPrices(string productIds);

        /// <summary>
        /// 根据产品Id和购买数量获取产品梯度价格
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="purchaseQuantity">购买数量</param>
        /// <returns></returns>
        ProductQuantityPricePo GetProductQuantityPriceByPurchaseQty(int productId, int purchaseQuantity);

        /// <summary>
        /// 根据产品Id和购买数量获取产品梯度价格
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="purchaseQuantity">购买数量</param>
        /// <returns></returns>
        ProductQuantityPricePo GetProductQuantityPrice(int productId, int purchaseQuantity);
    } 
}
   