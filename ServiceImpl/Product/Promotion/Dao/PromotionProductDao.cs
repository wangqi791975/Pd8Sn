//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PromotionProductDao.cs
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
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Promotion.Dao
{
    public class PromotionProductDao : BaseDao<PromotionProductPo, int>, IPromotionProductDao
    {

        public PromotionProductPo GetPromotionProduct(int promotionAreaId, int productId)
        {
            return GetOneObject("from PromotionProductPo where PromotionId=? and ProductId=?", new object[] { promotionAreaId, productId });
        }

        public bool CheckProductForPromotionIsExists(int productId, DateTime dateStarted, DateTime dateEnded)
        {
            var result = SqlHelper.ExecuteScalar(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_checkproductforpromotion_is_exists", new[] 
            {
                new SqlParameter("@ProductId", SqlDbType.Int){Value =  productId},
                new SqlParameter("@BeginDate", SqlDbType.DateTime){Value =  dateStarted},
                new SqlParameter("@EndDate", SqlDbType.DateTime){Value =  dateEnded},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }); 
            return Convert.ToBoolean(result);
        }

        public bool IsPromotionProduct(int productId)
        {
            return !GetOneObject("from PromotionProductPo where ProductId=?", productId).IsNullOrEmpty();
        }


        public IList<PromotionProductPo> GetAllPromotionProductsByPromotionId(int promotionId)
        {
            return FindDataByHql("from PromotionProductPo where PromotionId=?", promotionId);
        }


        public PromotionProductPo GetProductPromotionByProductId(int productId)
        {
            return GetOneObject("from PromotionProductPo where ProductId=?", productId);
        }

        public IList<PromotionProductPo> GetProductPromotionByProductIds(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return new List<PromotionProductPo>();
            }

            return FindDataByHql(string.Format("from PromotionProductPo where ProductId in ({0})", productIds));
        }

    }
}
