//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PrductStockDao.cs
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
using System.Data;
using System.Data.SqlClient;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Product;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class ProductStockDao:BaseDao<ProductStockPo,int>, IProductStockDao
    {
        public ProductStockPo GetProductStock(int productId)
        {
            return GetOneObject("from ProductStockPo where ProductId = ?", productId);
        }

        public IList<ProductStockPo> GetProductStockByIds(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return new List<ProductStockPo>();
            }
            return FindDataByHql(string.Format("from ProductStockPo where ProductId in ({0})", productIds)); 
        }

        public void UpdateProductStockByOrderId(int orderId)
        {
            var parms = new[]
            {
                new SqlParameter("@OrderId", SqlDbType.Int) {Value = orderId},
            };
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure,"up_product_stock_update", parms);
        }
    } 
}
   