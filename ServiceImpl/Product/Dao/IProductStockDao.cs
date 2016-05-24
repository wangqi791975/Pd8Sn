//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IPrductStockDao.cs
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
    public interface IProductStockDao:IBaseDao<ProductStockPo,int>
    {
        /// <summary>
        /// 根据产品Id获取库存信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>库存信息</returns>
        ProductStockPo GetProductStock(int productId);

        /// <summary>
        /// 根据产品Ids获取产品库存信息
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <returns></returns>
        IList<ProductStockPo> GetProductStockByIds(string productIds);

        /// <summary>
        /// 根据订单Id，更新库存数
        /// </summary>
        /// <param name="orderId"></param>
        void UpdateProductStockByOrderId(int orderId);
    } 
}
   