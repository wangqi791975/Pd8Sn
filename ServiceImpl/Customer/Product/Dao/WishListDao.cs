using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public class WishListDao : BaseDao<WishListPo, int>, IWishListDao
    {
        public WishListPo GetWishList(int customerId, int wishlistId)
        {
            return GetOneObject("from WishListPo WHERE WishListId = ?  AND  CustomerId = ?", new object[] { wishlistId, customerId });
        }

        public WishListPo GetWishListByProductId(int customerId, int productId)
        {
            return GetOneObject("from WishListPo WHERE ProductId = ?  AND  CustomerId = ?", new object[] { productId, customerId });
        }

        public void DeleteWishListProduct(int customerId, int productId)
        {
            DeleteObjectByHql("DELETE FROM WishListPo WHERE CustomerId = ? AND ProductId = ?", new object[] { customerId, productId });
        }

        public void DeleteWishListProudctByIds(int customerId, IList<int> productIds)
        {
            if (!productIds.IsNullOrEmpty())
            { 
            var obj = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("customerId", customerId), 
                    new KeyValuePair<string, object>("id", productIds), 
                };
            DeleteObjectByHql("delete from WishListPo  WHERE CustomerId=:customerId and ProductId in (:id)", obj);
           }
        }

        public void UpdateWishListType(int customerId, int wishlistId, int wishListType)
        {
            UpdateObjectByHql("UPDATE WishListPo SET Classification = ? WHERE WishListId = ?  AND  CustomerId = ? ",
                new object[] { wishListType, wishlistId, customerId });
        }

        /// <summary>
        /// 根据客户Id和语种Id获取客户wishList产品有哪些类别，[add By luohaiming]
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        public IList<KeyValuePair<int, string>> GetWishListProductCategory(int languageId, int customerId)
        {
            IList<KeyValuePair<int, string>> dic = new  List<KeyValuePair<int, string>>();
            var parms = new[] {
                                new SqlParameter("@languageId", SqlDbType.Int){Value =  languageId}, 
                                new SqlParameter("@customerId", SqlDbType.Int){Value =  customerId}, 
                              };
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_wishlist_product_category_get", parms))
            {
                while(reader.Read())
                {
                    dic.Add(new KeyValuePair<int, string>(reader.IsDBNull(0) ? 0 : reader.GetInt32(0), reader.IsDBNull(1) ? string.Empty : reader.GetString(1)));;
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取wishlist个数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        public int GetWishListCountByCustomerId(int customerId)
        {
            var parms = new List<SqlParameter> {
                                    new SqlParameter("@num", SqlDbType.Int){Direction = ParameterDirection.ReturnValue},
                                    new SqlParameter("@customer_id", SqlDbType.Int){Value = customerId}  
                       };
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_wishlist_count_get", parms.ToArray());

            return (int)parms[0].Value;
        }
    }
}