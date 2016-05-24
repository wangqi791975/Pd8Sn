using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public class WishListHistoryDao : BaseDao<WishListHistoryPo, int>, IWishListHistoryDao
    {
        public WishListHistoryPo GetWishList(int customerId, int wishlistId)
        {
            return GetOneObject("from WishListHistoryPo WHERE WishListId = ? And CustomerId = ?", new object[] {wishlistId, customerId });
        }

        public WishListHistoryPo GetWishListHistoryByProductId(int customerId, int productId)
        {
            return GetOneObject("from WishListHistoryPo WHERE ProductId = ?  AND  CustomerId = ?", new object[] { productId, customerId });
        }

        public void DeleteWishListHistory(int customerId, int productId)
        {
            DeleteObjectByHql("DELETE FROM WishListHistoryPo WHERE CustomerId = ? AND ProductId = ?", new object[] { customerId, productId });
        }

        public void DeleteWishListHistoryByIds(int customerId, IList<int> productIds)
        {
            if (!productIds.IsNullOrEmpty())
            {
                var obj = new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("customerId", customerId),
                        new KeyValuePair<string, object>("id", productIds),
                    };
                DeleteObjectByHql(
                    "delete from WishListHistoryPo  WHERE CustomerId=:customerId and ProductId in (:id)", obj);
            }
        }

        public void UpdateWishListHistoryTpye(int customerId, int wishlistId, int wishListType)
        {
            UpdateObjectByHql("UPDATE WishListHistoryPo SET Classification = ? WHERE WishListId = ? And CustomerId = ?",
                new object[] { wishListType, wishlistId, customerId });
        }

        /// <summary>
        /// 根据客户Id和语种Id获取客户wishListHistory产品有哪些类别，[add By luohaiming]
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        public IList<KeyValuePair<int, string>> GetWishListHistoryCategory(int languageId, int customerId)
        {
            IList<KeyValuePair<int, string>> dic = new  List<KeyValuePair<int, string>>();
            var parms = new[] {
                                new SqlParameter("@languageId", SqlDbType.Int){Value =  languageId}, 
                                new SqlParameter("@customerId", SqlDbType.Int){Value =  customerId}, 
                              };
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_wishlist_history_product_category_get", parms))
            {
                while (reader.Read())
                {
                    dic.Add(new KeyValuePair<int, string>(reader.IsDBNull(0) ? 0 : reader.GetInt32(0), reader.IsDBNull(1) ? string.Empty : reader.GetString(1)));;
                }
            }
            return dic;
        }

        public PageData<WishListProduct> GetWishListRemovedProducts(int currentPage, int pageSize, int customerId, IList<Sorter<WishListSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<WishListProduct>();
            var dataList = new List<WishListProduct>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
                                {
                                    new SqlParameter("@pageIndex", SqlDbType.Int){Value =  currentPage}, 
                                    new SqlParameter("@pageSize", SqlDbType.Int){Value =  pageSize},
                                    new SqlParameter("@customerId", SqlDbType.Int){Value =customerId},
                                    new SqlParameter("@sortField", SqlDbType.VarChar,100){Value =  string.Empty},
                                    new SqlParameter("@sortDirecton", SqlDbType.VarChar,10){Value =  "ASC"}
                                };

            //设置排序条件
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case WishListSorterCriteria.AddDate:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "AddDate";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc ? "ASC" : "DESC";
                            break;
                    }
                }
            }

            WishListProduct wishListProduct;
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_wishlist_remove_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                //分页数据 
                while (reader.Read())
                {
                    wishListProduct = new WishListProduct();
                    wishListProduct.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    wishListProduct.CustomerId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    wishListProduct.ProductId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    wishListProduct.Count = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    wishListProduct.WishListType = reader.IsDBNull(4) ? 0 : reader.GetInt32(4).ToEnum<WishListType>();
                    wishListProduct.AddDateTime = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5);
                    wishListProduct.DateModified = reader.IsDBNull(6) ? DateTime.MaxValue : reader.GetDateTime(6);
                    wishListProduct.IsHistory = reader.IsDBNull(7) ? false : (reader.GetInt32(7) == 1);
                    dataList.Add(wishListProduct);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);

            return pageData;
        }
    }
}