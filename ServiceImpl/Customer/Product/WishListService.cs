using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Entity.ShoppingCart;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Com.Panduo.ServiceImpl.Customer.Product.Dao;
using Com.Panduo.ServiceImpl.Product.Dao;

namespace Com.Panduo.ServiceImpl.Customer.Product
{
    public class WishListService : IWishListService
    {
        public IWishListDao WishListDao { get; set; }
        public IWishListHistoryDao WishListHistoryDao { get; set; }
        public IWishListTypeDescDao WishListTypeDescDao { get; set; }
        public ICustomerDao CustomerDao { get; set; }
        public IProductDao ProductDao { get; set; }

        public string ERROR_CUSTOMER_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_NOT_EXIST"; }
        }

        public string ERROR_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_PRODUCT_NOT_EXIST"; }
        }

        public int AddWishListProduct(WishListProduct wishListProduct)
        {
            if (CustomerDao.GetObject(wishListProduct.CustomerId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            if (ProductDao.GetObject(wishListProduct.ProductId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PRODUCT_NOT_EXIST);
            }
            var wishlistvo = GetWishListPoFromVo(wishListProduct);
            var po=WishListDao.GetWishListByProductId(wishlistvo.CustomerId,wishlistvo.ProductId);
            if (po.IsNullOrEmpty())
            {
                return WishListDao.AddObject(GetWishListPoFromVo(wishListProduct));
            }
            po.ProductQuantity = wishlistvo.ProductQuantity;
            WishListDao.UpdateObject(po);
            return wishlistvo.WishListId;
        }

        public void AddWishListProducts(IList<WishListProduct> wishListProducts)
        {
            WishListDao.AddObjects(wishListProducts.Select(GetWishListPoFromVo));
        }

        /// <summary>
        /// 删除wishlist产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="isHistory">是否历史表</param>
        public void RemoveWishListProduct(int customerId, int productId,bool isHistory)
        {
            if (CustomerDao.GetObject(customerId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            if (ProductDao.GetObject(productId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PRODUCT_NOT_EXIST);
            }
            if (isHistory)
            {
                WishListHistoryDao.DeleteWishListHistory(customerId, productId);
            }
            WishListDao.DeleteWishListProduct(customerId, productId);
        }

        /// <summary>
        /// 批量删除wishlist产品
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productList">产品Id List</param>
        public void RemoveWishListProduct(int customerId, IList<KeyValuePair<int, bool>> productList)
        {
            var productIds = productList.Where(c => !c.Value).Select(c => c.Key).ToList();
            var historyProductIds = productList.Where(c => c.Value).Select(c => c.Key).ToList();
            WishListDao.DeleteWishListProudctByIds(customerId, productIds);
            WishListHistoryDao.DeleteWishListHistoryByIds(customerId, historyProductIds);
        }

        public void SetWishListType(int customerId, int wishListId, WishListType wishlistType, bool isHistory)
        {
            if (CustomerDao.GetObject(customerId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            if (isHistory)
            {
                WishListHistoryDao.UpdateWishListHistoryTpye(customerId,wishListId, (int)wishlistType);
            }
            WishListDao.UpdateWishListType(customerId,wishListId, (int)wishlistType);
        }

        public void SetWishListType(int customerId, IDictionary<int, WishListType> wishtlistTypes, bool isHistory)
        {
            if (CustomerDao.GetObject(customerId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
           
            if (isHistory)
            {
                var wishListHistoryPos = new List<WishListHistoryPo>();
                foreach (var item‎ in wishtlistTypes)
                {
                    var wishListHistoryPo = WishListHistoryDao.GetWishListHistoryByProductId(customerId, item‎.Key);
                    if (!wishListHistoryPo.IsNullOrEmpty())
                    {
                        wishListHistoryPo.Classification = (int) item‎.Value;
                        wishListHistoryPo.DateModified = DateTime.Now;
                        wishListHistoryPos.Add(wishListHistoryPo);
                    }
                }
                if (!wishListHistoryPos.IsNullOrEmpty())
                    WishListHistoryDao.UpdateObjects(wishListHistoryPos);
            }
            else
            {
                var wishListPos = new List<WishListPo>();
                foreach (var item‎ in wishtlistTypes)
                {
                    var wishListPo = WishListDao.GetWishListByProductId(customerId, item‎.Key);
                    if (!wishListPo.IsNullOrEmpty())
                    {
                        wishListPo.Classification = (int) item‎.Value;
                        wishListPo.DateModified = DateTime.Now;
                        wishListPos.Add(wishListPo);
                    }
                }
                if (!wishListPos.IsNullOrEmpty())
                    WishListDao.UpdateObjects(wishListPos);
            }
        }

        /// <summary>
        /// 根据wishListId获取WishList VO
        /// </summary>
        /// <param name="wishListId">wishListId</param>
        /// <param name="isHistory">是否历史表</param>
        /// <returns>WishListProduct</returns>
        public WishListProduct GetWishListProductById(int wishListId, bool isHistory)
        {
            if (isHistory)
            {
                return GetWishListHistoryVoFromPo(WishListHistoryDao.GetObject(wishListId));
            }
            return GetWishListVoFromPo(WishListDao.GetObject(wishListId));
        }

        public PageData<WishListProduct> GetWishListProducts(int currentPage, int pageSize, int customerId, IDictionary<WishListSearchCriteria, object> searchCriteria,
            IList<Sorter<WishListSorterCriteria>> sorterCriteria)
        {
            return GetSelectWishListProducts(currentPage, pageSize, customerId, "up_wishlist_search", searchCriteria, sorterCriteria);
        }

        private PageData<WishListProduct> GetSelectWishListProducts(int currentPage, int pageSize, int customerId, string proname, IDictionary<WishListSearchCriteria, object> searchCriteria,IList<Sorter<WishListSorterCriteria>> sorterCriteria)
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
                                    new SqlParameter("@categoryId", SqlDbType.Int){Value =  searchCriteria.TryGet(WishListSearchCriteria.CategoryId)},
                                    new SqlParameter("@classification", SqlDbType.Int){Value =  searchCriteria.TryGet(WishListSearchCriteria.ClassificationType)}, 
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
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, proname, parmsList.ToArray()))
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
                    wishListProduct.IsHistory = reader.IsDBNull(7) ? false : (reader.GetInt32(7)==1);
                    dataList.Add(wishListProduct);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);

            return pageData;
        }

        public PageData<WishListProduct> GetWishListHistoryProducts(int currentPage, int pageSize, int customerId, IDictionary<WishListSearchCriteria, object> searchCriteria,
                                                   IList<Sorter<WishListSorterCriteria>> sorterCriteria)
        {
            return GetSelectWishListProducts(currentPage, pageSize, customerId, "up_wishlist_history_search", searchCriteria, sorterCriteria);
        }

        public PageData<WishListProduct> GetWishListRemovedProducts(int currentPage, int pageSize, int customerId, IList<Sorter<WishListSorterCriteria>> sorterCriteria)
        {
            return WishListHistoryDao.GetWishListRemovedProducts(currentPage, pageSize, customerId, sorterCriteria);
        }

        /// <summary>
        /// wishList类别
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="isHistory">是否历史表</param>
        /// <returns>key=类别Id value=类别名称</returns>
        public IList<KeyValuePair<int, string>> GetWishListProductCategory(int customerId, bool isHistory)
        {
            if (isHistory)
            {
                return WishListHistoryDao.GetWishListHistoryCategory(ServiceFactory.ConfigureService.SiteLanguageId, customerId);
            }
            else
            {
                return WishListDao.GetWishListProductCategory(ServiceFactory.ConfigureService.SiteLanguageId, customerId);
            }
        }

        /// <summary>
        /// wishlist喜爱类型
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public IList<WishListTypeDesc> GetWishListType(int languageId)
        {
            IList<WishListTypeDesc> wishListTypeDescList=new List<WishListTypeDesc>();
            var list= WishListTypeDescDao.GetWishListTypeDesc(languageId);
            foreach (var wishListTypeDescPo in list)
            {
                wishListTypeDescList.Add(GetWishListTypeDescVoFromPo(wishListTypeDescPo));  
            }
            return wishListTypeDescList;
        }

        public int GetWishListCountByCustomerId(int customerId)
        {
           return WishListDao.GetWishListCountByCustomerId(customerId);
        }

        public PageData<CustomerWishListProduct> GetAdminWishLists(int currentPage, int pageSize, IDictionary<WishListSearchCriteria, object> searchCriteria, IList<Sorter<WishListSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<CustomerWishListProduct>();
            var dataList = new List<CustomerWishListProduct>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
                                {
                                    new SqlParameter("@pageIndex", SqlDbType.Int){Value =  currentPage}, 
                                    new SqlParameter("@pageSize", SqlDbType.Int){Value =  pageSize},
                                    new SqlParameter("@customer", SqlDbType.VarChar){Value =  searchCriteria.TryGet(WishListSearchCriteria.CustomerEmail).ToSqlString()},
                                    new SqlParameter("@partno", SqlDbType.VarChar){Value =  searchCriteria.TryGet(WishListSearchCriteria.ProductNo).ToSqlString()}, 
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

            CustomerWishListProduct wishListProduct;
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_admin_wishlist_search", parmsList.ToArray()))
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
                    wishListProduct = new CustomerWishListProduct();
                    wishListProduct.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    wishListProduct.CustomerId = reader["customer_id"].ParseTo<int>();
                    wishListProduct.ProductId = reader["product_id"].ParseTo<int>();
                    wishListProduct.Count = reader["product_quantity"].ParseTo<int>();
                    wishListProduct.WishListType = reader.IsDBNull(4) ? 0 : reader.GetInt32(4).ToEnum<WishListType>();
                    wishListProduct.AddDateTime = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5);
                    wishListProduct.DateModified = reader.IsDBNull(6) ? DateTime.MaxValue : reader.GetDateTime(6);
                    wishListProduct.IsHistory = reader["type"].ParseTo<bool>(false);
                    wishListProduct.CustomerEmail = reader["customer_email"].ParseTo<string>();
                    wishListProduct.CustomerFull = reader["full_name"].ParseTo<string>();
                    dataList.Add(wishListProduct);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);

            return pageData;
        }

        #region 辅助方法

        internal static WishListPo GetWishListPoFromVo(WishListProduct wishListProduct)
        {
            WishListPo wishListPo = null;
            if (!wishListProduct.IsNullOrEmpty())
            {
                wishListPo = new WishListPo
                {
                    WishListId = wishListProduct.Id,
                    CustomerId = wishListProduct.CustomerId,
                    ProductId = wishListProduct.ProductId,
                    ProductQuantity = wishListProduct.Count,
                    Classification = (int)wishListProduct.WishListType,
                    DateCreated = wishListProduct.AddDateTime,
                    DateModified = wishListProduct.DateModified
                };
            }
            return wishListPo;
        }

        internal static WishListProduct GetWishListVoFromPo(WishListPo wishListPo)
        {
            WishListProduct wishListProduct = null;
            if (!wishListPo.IsNullOrEmpty())
            {
                wishListProduct = new WishListProduct
                {
                    Id = wishListPo.WishListId,
                    CustomerId = wishListPo.CustomerId,
                    ProductId = wishListPo.ProductId,
                    Count = wishListPo.ProductQuantity,
                    WishListType = (WishListType)wishListPo.Classification,
                    AddDateTime = wishListPo.DateCreated,
                    IsHistory = false,
                    DateModified = wishListPo.DateModified
                };
            }
            return wishListProduct;
        }

        /// <summary>
        /// WishListHistory PO To VO
        /// </summary>
        /// <param name="wishListPo">WishListHistory po</param>
        /// <returns>Wishlist Vo</returns>
        internal static WishListProduct GetWishListHistoryVoFromPo(WishListHistoryPo wishListPo)
        {
            WishListProduct wishListProduct = null;
            if (!wishListPo.IsNullOrEmpty())
            {
                wishListProduct = new WishListProduct
                {
                    Id = wishListPo.WishListId,
                    CustomerId = wishListPo.CustomerId,
                    ProductId = wishListPo.ProductId,
                    Count = wishListPo.ProductQuantity,
                    WishListType = (WishListType)wishListPo.Classification,
                    AddDateTime = wishListPo.DateCreated,
                    IsHistory = true,
                    DateModified = wishListPo.DateModified
                };
            }
            return wishListProduct;
        }


        internal static WishListTypeDesc GetWishListTypeDescVoFromPo(WishListTypeDescPo wishListTypeDescPo)
        {
            WishListTypeDesc wishListTypeDesc = null;
            if (!wishListTypeDescPo.IsNullOrEmpty())
            {
                wishListTypeDesc = new WishListTypeDesc
                {
                    Id = wishListTypeDescPo.Id.TypeId,
                    LanguageId = wishListTypeDescPo.Id.LanguageId,
                    ItemName = wishListTypeDescPo.Name
                };
            }
            return wishListTypeDesc;
        }
        #endregion
    }
}