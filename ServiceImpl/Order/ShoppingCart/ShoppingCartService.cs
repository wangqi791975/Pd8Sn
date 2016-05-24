using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Entity.ShoppingCart;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Com.Panduo.ServiceImpl.Customer.Product.Dao;
using Com.Panduo.ServiceImpl.Order.ShoppingCart.Dao;
using Com.Panduo.Common;
using NHibernate.Linq;
using SolrNet.Utils;

namespace Com.Panduo.ServiceImpl.Order.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        public IShoppingCartDao ShoppingCartDao { private get; set; }
        public IVShoppingCartDao VShoppingCartDao { private get; set; }
        public IAutoCookieIdDao AutoCookieIdDao { private get; set; }
        public IVshoppingCartUnAvailablDao VshoppingCartUnAvailablDao { private get; set; }
        public IWishListDao WishListDao { get; set; }
        public ICustomerDao CustomerDao { get; set; }

        #region 常量
        /// <summary>
        /// 客户不存在
        /// </summary>
        public string ERROR_CUSTOMER_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_NOT_EXIST"; }
        }
        public string ERROR_SHOPPINGCART_NOT_EXIST
        {
            get { return "ERROR_SHOPPINGCART_NOT_EXIST"; }
        }
        /// <summary>
        /// 购物车中该产品不存在
        /// </summary>
        public string ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST"; }
        }
        /// <summary>
        /// 添加购物车失败
        /// </summary>
        public string ERROR_SHOPPINGCART_ADD_PRODUCT
        {
            get { return "ERROR_SHOPPINGCART_ADD_PRODUCT"; }
        }
        #endregion

        /// <summary>
        ///  获取未登陆客户的临时ID(生成的负值)‎
        /// </summary>
        /// <returns>未登陆客户的临时CookID‎</returns>
        public int GetCookIdforTempCustomerId()
        {
            var autoCookieId = new AutoCookieIdPo { AddTime = DateTime.Now };
            return AutoCookieIdDao.AddObject(autoCookieId);
        }

        /// <summary>
        /// 获取客户购物车产品款数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>产品款数</returns>
        public int GetShoppingCartProductCount(int customerId)
        {
            return ShoppingCartDao.GetShoppingCartProductCount(customerId);
        }

        /// <summary>
        /// 获取客户购物车下架产品款数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="languageId">当前语种</param>
        /// <returns>产品款数</returns>
        public int GetShoppingCartUnAvailableProductCount(int customerId, int languageId)
        {
            return VshoppingCartUnAvailablDao.GetObjectCount(string.Format("CustomerId={0} and language_id={1}", customerId, languageId));
        }

        /// <summary>
        /// 合并购物车
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="tempCustomerId">购物车Id 未登录是临时ID(生成的负值）‎</param>
        public void MergeShoppingCart‎(int customerId, int tempCustomerId)
        {
            //if(ShoppingCartDao.ExistObject("",customerId))
            //    throw new BussinessException(ERROR_SHOPPINGCART_NOT_EXIST);
            ShoppingCartDao.MergeShoppingCart‎(customerId, tempCustomerId);
        }

        /// <summary>
        /// 点击CheckOut 修改下架产品的Remove_checked字段
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        public void UpdateShoppingCartUnAvailableProductStatus(int shoppingCartId)
        {
            ShoppingCartDao.UpdateShoppingCartUnAvailableProductStatus(shoppingCartId);
        }

        /// <summary>
        /// 提交订单时验证是否又有新的商品不满足条件(可能又有新的商品被下架)
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <returns></returns>
        public bool ValidateShoppingCartItem(int shoppingCartId)
        {
            return ShoppingCartDao.ValidateShoppingCartItem(shoppingCartId);
        }

        /// <summary>
        /// 根据产品Id获取该产品在购物车内中的数量，没有返回0. add by luohaiming 
        /// </summary>
        /// <remarks>wishlist 显示用</remarks>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productIds">产品Id集合</param>
        /// <returns>产品对应的购物车中的数量 Key：产品ID，value：产品数量</returns>
        public Dictionary<int, int> GetShoppingCartProductsQuantity(int shoppingCartId, IList<int> productIds)
        {
            var dicShoppingCartProductsQuantity = new Dictionary<int, int>();
            var lstShoppingCartPo = ShoppingCartDao.GetShoppingCartProductsQuantity(shoppingCartId, productIds);
            foreach (var x in lstShoppingCartPo)
            {
                if (dicShoppingCartProductsQuantity.ContainsKey(x.ProductId))
                    dicShoppingCartProductsQuantity[x.ProductId] = x.ProductQuantity;
                else
                    dicShoppingCartProductsQuantity.Add(x.ProductId, x.ProductQuantity);
            }
            return dicShoppingCartProductsQuantity;
        }

        /// <summary>
        /// 根据产品Id获取该产品在购物车内中的数量，没有返回0
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <returns>产品对应的购物车中的数量 Key：产品ID，value：产品数量</returns>
        public int GetShoppingCartProductsQuantity(int shoppingCartId, int productId)
        {
            var shoppingCartPo = ShoppingCartDao.GetOneObject("from ShoppingCartPo where CustomerId=? and ProductId=?", new object[] { shoppingCartId, productId });
            return shoppingCartPo.IsNullOrEmpty() ? 0 : shoppingCartPo.ProductQuantity;
        }

        /// <summary>
        /// 根据产品Id获取该产品在购物车内中的数量，没有返回0. add by luohaiming 
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <returns>产品对应的购物车中的数量 Key：产品ID，value：产品数量</returns>
        public VShoppingCartItem GetShoppingCartProduct(int shoppingCartId, int productId)
        {
            var shoppingCartPo = VShoppingCartDao.GetOneObject("from VShoppingCartPo where CustomerId=? and ProductId=?", new object[] { shoppingCartId, productId });
            return ShoppingCartPoToItemVo(shoppingCartPo);
        }


        #region 查询
        /// <summary>
        /// 分页按类别分组获取购物车产品明细
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        public PageData<ShoppingCartItem> FindShoppingCartItems(int shoppingCartId, int currentPage, int pageSize, IDictionary<ShoppingCartSearchCriteria, object> searchCriteria,
            IList<Sorter<ShoppingCartSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM ShoppingCartPo");
            hqlHelper.AddWhere("CustomerId", HqlOperator.Eq, "ShoppingCartId", shoppingCartId);
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ShoppingCartSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("language_id", HqlOperator.Like, "LanguageId", item.Value);
                            break;
                        //case ProductDailyDealSearchCriteria.ProductName:
                        //    hqlHelper.AddWhere("ProductName", HqlOperator.Like, "ProductName", item.Value);
                        //    break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //TODO 排序条件
                sorterCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ShoppingCartSorterCriteria.AddedTimeNewToOld:
                            hqlHelper.AddSorter("DateCreated", true);
                            break;
                        case ShoppingCartSorterCriteria.AddedTimeOldToNew:
                            hqlHelper.AddSorter("DateCreated", false);
                            break;
                        case ShoppingCartSorterCriteria.PriceHighToLow:
                            hqlHelper.AddSorter("DateCreated", false);
                            break;
                        case ShoppingCartSorterCriteria.PriceLowToHigh:
                            hqlHelper.AddSorter("DateCreated", true);
                            break;
                        case ShoppingCartSorterCriteria.Catefory:
                            hqlHelper.AddSorter("DateCreated", true);
                            break;
                    }
                });
            }
            //3.执行查询并返回数据
            var pageDataPo = ShoppingCartDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ShoppingCartItem>();
            var voList = pageDataPo.Data.Select(ShoppingCartPoToItemVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        /// <summary>
        /// 分页按类别分组获取购物车产品明细
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        public PageData<VShoppingCartItem> FindVShoppingCartItems(int shoppingCartId, int currentPage, int pageSize, IDictionary<ShoppingCartSearchCriteria, object> searchCriteria,
            IList<Sorter<ShoppingCartSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VShoppingCartPo");
            hqlHelper.AddWhere("CustomerId", HqlOperator.Eq, "ShoppingCartId", shoppingCartId);
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ShoppingCartSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("language_id", HqlOperator.Like, "LanguageId", item.Value);
                            break;
                        //case ProductDailyDealSearchCriteria.ProductName:
                        //    hqlHelper.AddWhere("ProductName", HqlOperator.Like, "ProductName", item.Value);
                        //    break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //TODO 排序条件
                sorterCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ShoppingCartSorterCriteria.AddedTimeNewToOld:
                            hqlHelper.AddSorter("DateCreated", true);
                            break;
                        case ShoppingCartSorterCriteria.AddedTimeOldToNew:
                            hqlHelper.AddSorter("DateCreated", false);
                            break;
                        case ShoppingCartSorterCriteria.PriceHighToLow:
                            hqlHelper.AddSorter("Price", false);
                            break;
                        case ShoppingCartSorterCriteria.PriceLowToHigh:
                            hqlHelper.AddSorter("Price", true);
                            break;
                        case ShoppingCartSorterCriteria.Catefory:
                            hqlHelper.AddSorter("CategoryId", true);
                            break;
                    }
                });
            }
            //3.执行查询并返回数据
            var pageDataPo = VShoppingCartDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<VShoppingCartItem>();
            var voList = pageDataPo.Data.Select(ShoppingCartPoToItemVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        private ShoppingCartItem ShoppingCartPoToItemVo(ShoppingCartPo shoppingCartPo)
        {
            if (shoppingCartPo.IsNullOrEmpty())
                return null;
            var shoppingCartItem = new ShoppingCartItem
            {
                Id = shoppingCartPo.CustomerBasketId,
                ShoppingCartId = shoppingCartPo.CustomerId,
                ProductId = shoppingCartPo.ProductId,
                Quantity = shoppingCartPo.ProductQuantity,
                Remark = shoppingCartPo.Remark,
                DateCreated = shoppingCartPo.DateCreated,
                DateModified = shoppingCartPo.DateModified,
            };
            return shoppingCartItem;
        }
        private VShoppingCartItem ShoppingCartPoToItemVo(VShoppingCartPo shoppingCartPo)
        {
            if (shoppingCartPo.IsNullOrEmpty())
                return null;
            var shoppingCartItem = new VShoppingCartItem
            {
                Id = shoppingCartPo.CustomerBasketId,
                ShoppingCartId = shoppingCartPo.CustomerId,
                ProductId = shoppingCartPo.ProductId,
                Quantity = shoppingCartPo.ProductQuantity,
                Remark = shoppingCartPo.Remark,
                DateCreated = shoppingCartPo.DateCreated,
                DateModified = shoppingCartPo.DateModified,
                ProductCode = shoppingCartPo.ProductCode,
                ProductName = shoppingCartPo.ProductName,
                ProductEnName = shoppingCartPo.ProductEnName,
                MainImage = shoppingCartPo.MainImage,
                OriginalPrice = shoppingCartPo.OriginalPrice,
                Discount = shoppingCartPo.Discount,
                Price = shoppingCartPo.Price,
                ProductSubTotal = shoppingCartPo.ProductSubTotal,
                ProdDiscountType = shoppingCartPo.ProdDiscountType.ToEnum<ProdDiscountType>(),
                Weight = shoppingCartPo.Weight,
                VolumeWeight = shoppingCartPo.VolumeWeight,
                BackorderDays = shoppingCartPo.BackorderDays,
                IsLimitStock = shoppingCartPo.IsLimitStock,
                Tip = shoppingCartPo.Tip,
                IsBackorder = shoppingCartPo.IsBackorder,
                StockQty = shoppingCartPo.StockQty,
                LabelName = shoppingCartPo.LabelName
            };
            return shoppingCartItem;
        }

        /// <summary>
        /// 获取无效产品明细
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns>无效产品集合</returns>
        public PageData<VShoppingCartItem> FindAllUnAvailableProducts(int shoppingCartId, int currentPage, int pageSize, IDictionary<ShoppingCartSearchCriteria, object> searchCriteria,
            IList<Sorter<ShoppingCartSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VshoppingCartUnAvailablPo");
            hqlHelper.AddWhere("CustomerId", HqlOperator.Eq, "ShoppingCartId", shoppingCartId);
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ShoppingCartSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("language_id", HqlOperator.Like, "LanguageId", item.Value);
                            break;
                        //case ProductDailyDealSearchCriteria.ProductName:
                        //    hqlHelper.AddWhere("ProductName", HqlOperator.Like, "ProductName", item.Value);
                        //    break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //TODO 排序条件
            }
            //3.执行查询并返回数据
            var pageDataPo = VshoppingCartUnAvailablDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<VShoppingCartItem>();
            var voList = pageDataPo.Data.Select(shoppingCartPo => new VShoppingCartItem
            {
                Id = shoppingCartPo.CustomerBasketId,
                ShoppingCartId = shoppingCartPo.CustomerId,
                ProductId = shoppingCartPo.ProductId,
                Quantity = shoppingCartPo.ProductQuantity,
                Remark = shoppingCartPo.Remark,
                DateCreated = shoppingCartPo.DateCreated,
                DateModified = shoppingCartPo.DateModified,

                ProductCode = shoppingCartPo.ProductCode,
                ProductName = shoppingCartPo.ProductName,
                ProductEnName = shoppingCartPo.ProductEnName,
                MainImage = shoppingCartPo.MainImage,
                OriginalPrice = shoppingCartPo.OriginalPrice,
                Price = shoppingCartPo.Price,
                ProductSubTotal = shoppingCartPo.ProductSubTotal,
                ProdDiscountType = shoppingCartPo.ProdDiscountType.ToEnum<ProdDiscountType>(),
                Weight = shoppingCartPo.Weight,
                VolumeWeight = shoppingCartPo.VolumeWeight,
                Tip = shoppingCartPo.Tip
            }).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        /// <summary>
        /// 得到用户购物车汇总信息
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <returns>ShoppingCartSummary实体</returns>
        public ShoppingCartSummary GetShoppingCartSummary(int shoppingCartId, int languageId, int currencyId, int countryId)
        {
            return ShoppingCartDao.GetShoppingCartSummary(shoppingCartId, languageId, currencyId, countryId);
        }
        #endregion

        #region  添加/修改
        /// <summary>
        /// 添加产品至购物车 
        /// </summary>
        /// <param name="shoppingCartItem‎">购物车产品对象</param>
        /// <returns>自增Id‎</returns>
        public int AddProductToShoppingCart‎(ShoppingCartItem shoppingCartItem‎)
        {
            int shoppingCartId = 0;
            var shoppingCartPo = ShoppingCartDao.GetProductInShoppingCartByCustomerId(shoppingCartItem‎.ShoppingCartId, shoppingCartItem‎.ProductId);
            if (shoppingCartPo.IsNullOrEmpty())
            {
                shoppingCartPo = new ShoppingCartPo
                {
                    CustomerId = shoppingCartItem‎.ShoppingCartId,
                    ProductId = shoppingCartItem‎.ProductId,
                    ProductQuantity = shoppingCartItem‎.Quantity,
                    Remark = shoppingCartItem‎.Remark,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };
                shoppingCartId = ShoppingCartDao.AddObject(shoppingCartPo);
            }
            else
            {
                shoppingCartId = shoppingCartPo.CustomerBasketId;
                shoppingCartPo.ProductQuantity = shoppingCartItem‎.Quantity;
                shoppingCartPo.Remark = shoppingCartItem‎.Remark;
                shoppingCartPo.DateModified = DateTime.Now;
                ShoppingCartDao.UpdateObject(shoppingCartPo);
            }
            return shoppingCartId;
        }

        /// <summary>
        /// 批量添加产品至购物车 
        /// </summary>
        /// <param name="shoppingCartItems‎">购物车产品对象</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_ADD_PRODUCT:添加购物车失败</value>
        /// </exception>
        public void BatchAddProductToShoppingCart(IList<ShoppingCartItem> shoppingCartItems‎)
        {
            try
            {
                var lstAddToCart = new List<ShoppingCartPo>();
                var lstEditToCart = new List<ShoppingCartPo>();
                foreach (var shoppingCartItem‎ in shoppingCartItems‎)
                {
                    var shoppingCartPo = ShoppingCartDao.GetProductInShoppingCartByCustomerId(shoppingCartItem‎.ShoppingCartId, shoppingCartItem‎.ProductId);
                    if (shoppingCartPo.IsNullOrEmpty())
                    {
                        lstAddToCart.Add(new ShoppingCartPo
                        {
                            CustomerId = shoppingCartItem‎.ShoppingCartId,
                            ProductId = shoppingCartItem‎.ProductId,
                            ProductQuantity = shoppingCartItem‎.Quantity,
                            Remark = shoppingCartItem‎.Remark,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now
                        });
                    }
                    else
                    {
                        shoppingCartPo.ProductQuantity = shoppingCartItem‎.Quantity;
                        shoppingCartPo.Remark = shoppingCartItem‎.Remark;
                        shoppingCartPo.DateModified = DateTime.Now;
                        lstEditToCart.Add(shoppingCartPo);
                    }
                }
                if (!lstAddToCart.IsNullOrEmpty())
                    ShoppingCartDao.AddObjects(lstAddToCart);

                if (!lstEditToCart.IsNullOrEmpty())
                    ShoppingCartDao.UpdateObjects(lstEditToCart);
            }
            catch (Exception exp)
            {
                throw new BussinessException(ERROR_SHOPPINGCART_ADD_PRODUCT);
            }
        }

        /// <summary>
        /// 修改产品数量
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <param name="qty">数量</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// </exception>
        public void UpdateShoppingCartProductQty‎(int shoppingCartId, int productId, int qty)
        {
            var shoppingCartPo = ShoppingCartDao.GetProductInShoppingCartByCustomerId(shoppingCartId, productId);
            if (shoppingCartPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST);

            shoppingCartPo.ProductQuantity = qty;
            shoppingCartPo.DateModified = DateTime.Now;
            ShoppingCartDao.UpdateObject(shoppingCartPo);
        }

        /// <summary>
        /// 修改产品备注
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <param name="remark">产品备注</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// </exception>
        public void SetShoppingCartProductRemark‎(int shoppingCartId, int productId, string remark)
        {
            var shoppingCartPo = ShoppingCartDao.GetProductInShoppingCartByCustomerId(shoppingCartId, productId);
            if (shoppingCartPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST);

            shoppingCartPo.Remark = remark;
            shoppingCartPo.DateModified = DateTime.Now;
            ShoppingCartDao.UpdateObject(shoppingCartPo);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 客户清空购物车
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_NOT_EXIST:购物车不存在</value>
        /// </exception>
        public void CleanShoppingCart(int shoppingCartId)
        {
            //if (ShoppingCartDao.ExistObject("CustomerId", shoppingCartId))
            //    throw new BussinessException(ERROR_SHOPPINGCART_NOT_EXIST);
            ShoppingCartDao.DeleteObjectByHql("delete from ShoppingCartPo where CustomerId=?", new object[] { shoppingCartId });
        }

        /// <summary>
        /// 根据产品Id删除购物车产品
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// </exception>
        public void DeleteShoppingCartItemByProductId(int shoppingCartId, int productId)
        {
            var shoppingCartPo = ShoppingCartDao.GetProductInShoppingCartByCustomerId(shoppingCartId, productId);
            if (shoppingCartPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST);

            ShoppingCartDao.DeleteObject(shoppingCartPo);
        }

        /// <summary>
        /// 将购物车所有产品移动到Wishlist
        /// </summary>
        /// <param name="shoppingCartId">购物车Id(客户Id）</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_CUSTOMER_NOT_EXIST:该客户不存在</value>
        /// </exception>
        public void MoveAllToWishlist(int shoppingCartId)
        {
            if (CustomerDao.GetObject(shoppingCartId).IsNullOrEmpty())
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);

            ShoppingCartDao.MoveAllToWishlist(shoppingCartId);
        }

        /// <summary>
        /// 单个产品Id移动到Wisthlist
        /// </summary>
        /// <param name="shoppingCartId">购物车Id(客户Id）</param>
        ///<param name="productId">产品Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// <value>ERROR_CUSTOMER_NOT_EXIST:该客户不存在</value>
        /// </exception>
        public void MoveToWishlist(int shoppingCartId, int productId)
        {
            if (CustomerDao.GetObject(shoppingCartId).IsNullOrEmpty())
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);

            var shoppingCartPo = ShoppingCartDao.GetProductInShoppingCartByCustomerId(shoppingCartId, productId);
            if (shoppingCartPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST);


            var wishListPo = WishListDao.GetOneObject("from WishListPo where CustomerId=? and ProductId=?", new object[] { shoppingCartId, productId });
            if (wishListPo.IsNullOrEmpty())
            {
                wishListPo = new WishListPo
                {
                    CustomerId = shoppingCartPo.CustomerId,
                    ProductId = shoppingCartPo.ProductId,
                    ProductQuantity = shoppingCartPo.ProductQuantity,
                    Classification = (int)WishListType.Unclassified,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };
                WishListDao.AddObject(wishListPo);
            }
            else
            {
                wishListPo.ProductQuantity = shoppingCartPo.ProductQuantity;
                wishListPo.DateModified = DateTime.Now;
                WishListDao.UpdateObject(wishListPo);
            }
            ShoppingCartDao.DeleteObject(shoppingCartPo);
        }
        #endregion


        public void RemoveShoppingCartUnAvailableProduct(int shoppingCartId)
        {
            ShoppingCartDao.DeleteObjectByHql("delete from ShoppingCartPo where CustomerId=? and RemoveChecked=? ", new object[] { shoppingCartId, true });
        }
    }
}
