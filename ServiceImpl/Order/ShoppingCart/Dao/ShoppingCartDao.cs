//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderPriceDao.cs
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
using Com.Panduo.Entity.ShoppingCart;
using Com.Panduo.Service.Order.ShoppingCart;

namespace Com.Panduo.ServiceImpl.Order.ShoppingCart.Dao
{
    public class ShoppingCartDao : BaseDao<ShoppingCartPo, int>, IShoppingCartDao
    {

        public ShoppingCartPo GetProductInShoppingCartByCustomerId(int customerId, int productId)
        {
            return GetOneObject("from ShoppingCartPo where CustomerId=? and ProductId=?", new object[] { customerId, productId });
        }

        public int GetShoppingCartProductCount(int customerId)
        {
            return GetObjectCount(string.Format("CustomerId={0}", customerId));
        }

        public void UpdateShoppingCartUnAvailableProductStatus(int shoppingCartId)
        {
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_shoppingcart_unavailable_updatestatus", new[] 
            {
                new SqlParameter("@customerId", SqlDbType.Int){Value =  shoppingCartId},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            });
        }

        public void MergeShoppingCart‎(int customerId, int tempCustomerId)
        {
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_shoppingcart_merge", new[] 
            {
                new SqlParameter("@customerId", SqlDbType.Int){Value =  customerId}, 
                new SqlParameter("@tempCustomerId", SqlDbType.Int){Value =  tempCustomerId} , 
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            });
        }

        public bool ValidateShoppingCartItem(int shoppingCartId)
        {
            var result = SqlHelper.ExecuteScalar(SqlHelper.CONN_STRING, CommandType.Text, "select dbo.uf_shoppingcart_is_unavailable(" + shoppingCartId + ")", null);
            return Convert.ToBoolean(result);
        }

        public ShoppingCartSummary GetShoppingCartSummary(int shoppingCartId, int languageId, int currencyId, int countryId)
        {
            var shoppingCart = new ShoppingCartSummary();
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_shoppingcart_summary_get", new[] 
            {
                new SqlParameter("@customerId", SqlDbType.Int){Value =  shoppingCartId},
                new SqlParameter("@currentLanguageId", SqlDbType.Int){Value =  languageId},
                new SqlParameter("@currentCurrencyId", SqlDbType.Int){Value =  currencyId},
                new SqlParameter("@currentCountryId", SqlDbType.Int){Value =  countryId},
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            }))
            {
                if (reader.Read())
                {
                    shoppingCart = new ShoppingCartSummary
                    {
                        ShoppingCartId = reader["CustomerId"].ParseTo<int>(),
                        ClubWeight = reader["ClubWeight"].ParseTo<decimal>(),
                        DiscountType = reader["DiscountType"].ParseTo<int>().ToEnum<DiscountType>(),
                        GrandTotal = reader["GrandTotal"].ParseTo<decimal>(),
                        GrossWeight = reader["GrossWeight"].ParseTo<decimal>(),
                        NoDiscountProductAmount = reader["NoDiscountProductAmount"].ParseTo<decimal>(),
                        OrderDiscount = reader["OrderDiscount"].ParseTo<decimal>(),
                        OrderDiscountAmount = reader["OrderDiscountAmount"].ParseTo<decimal>(),
                        OriginalProductAmount = reader["OriginalProductAmount"].ParseTo<decimal>(),
                        PackageWeight = reader["PackageWeight"].ParseTo<decimal>(),
                        PromotionAmount = reader["PromotionAmount"].ParseTo<decimal>(),
                        PromotionBeforeAmount = reader["PromotionBeforeAmount"].ParseTo<decimal>(),
                        PromotionDiscountAmount = reader["PromotionDiscountAmount"].ParseTo<decimal>(),
                        ShippingWeight = reader["ShippingWeight"].ParseTo<decimal>(),
                        TotalQuantity = reader["TotalQuantity"].ParseTo<int>(),
                        VipAndRcdDiscount = reader["VipAndRcdDiscount"].ParseTo<decimal>(),
                        VipAndRcdDiscountAmount = reader["VipAndRcdDiscountAmount"].ParseTo<decimal>(),
                        VipDiscount = reader["VipDiscount"].ParseTo<decimal>(),
                        VipDiscountAmount = reader["VipDiscountAmount"].ParseTo<decimal>(),
                        VolumeWeight = reader["VolumeWeight"].ParseTo<decimal>(),

                        HasCurrentDiscountTip = reader["HasCurrentDiscountTip"].ParseTo<bool>(),
                        CurrentDiscount = reader["CurrentDiscount"].ParseTo<decimal>(),
                        CurrentDiscountType = reader["CurrentDiscountType"].ParseTo<string>(),
                        ReplacingDiscount = reader["ReplacingDiscount"].ParseTo<decimal>(),
                        ReplacingDiscountAmount = reader["ReplacingDiscountAmount"].ParseTo<decimal>(),
                    };

                    reader.Close();
                }
            }
            return shoppingCart;
        }

        public void MoveAllToWishlist(int shoppingCartId)
        {
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_shoppingcart_move_all_to_wishlist", new[] 
            {
                new SqlParameter("@customerId", SqlDbType.Int){Value =  shoppingCartId}, 
                //new SqlParameter("@errorMsg", SqlDbType.NVarChar){Value =  tempCustomerId} //OutPut
            });
        }

        public IList<ShoppingCartPo> GetShoppingCartProductsQuantity(int shoppingCartId, IList<int> productIds)
        {
            if (!productIds.IsNullOrEmpty())
            {
                var obj = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("customerId", shoppingCartId),
                    new KeyValuePair<string, object>("id", productIds),
                };

                return FindDataByHql("from ShoppingCartPo where CustomerId=:customerId and ProductId in (:id)", obj);
            }
            return FindDataByHql("from ShoppingCartPo where CustomerId=?", new object[] { shoppingCartId });
        }
    }
}
