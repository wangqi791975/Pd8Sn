//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductMatchDao.cs
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

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class ProductMatchDao:BaseDao<ProductMatchPo,int>, IProductMatchDao
    {
        public IList<ProductMatchPo> GetMatchProduct(int productId)
        {
            return FindDataByHql("from ProductMatchPo where ProductId = ?", productId);
        }

        public IList<ProductMatchPo> GetMatchProduct(int productId, int topn)
        {
            return FindDataByHqlLimit("from ProductMatchPo  where  ProductId = ?", topn, productId);
        }


        public void SetBestMatch(KeyValuePair<string, string> item)
        {
            var parms = new[]
            {
                new SqlParameter("@productno", SqlDbType.VarChar) {Value = item.Key},
                new SqlParameter("@matchproduct", SqlDbType.VarChar) {Value = item.Value},
            };
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_product_match", parms);
        }

        public bool CanSetBestMatch(KeyValuePair<string, string> item)
        {
            var parms = new[]
            {
                new SqlParameter("@productno", SqlDbType.VarChar) {Value = item.Key},
                new SqlParameter("@matchproduct", SqlDbType.VarChar) {Value = item.Value},
                new SqlParameter("@can",SqlDbType.Int){Direction= ParameterDirection.Output},
            };
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_canimport_product_match", parms);
            var can = parms[2].Value.ParseTo(0);
            if (can==0)
            {
              return true;
            }
            else
            {
                return false;
            }
        }
    } 
}
   