using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShoppingCart
{
    public interface IShoppingCartService
    {
        #region 常量

        /// <summary>
        /// 购物车不存在
        /// </summary>
        string ERROR_SHOPPINGCART_NOT_EXIST { get; }

        /// <summary>
        /// 购物车中该产品不存在
        /// </summary>
        string ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 添加购物车失败
        /// </summary>
        string ERROR_SHOPPINGCART_ADD_PRODUCT { get; }
        /// <summary>
        /// 购物车转移到Wishlist失败，该客户不存在
        /// </summary>
        string ERROR_CUSTOMER_NOT_EXIST { get; }
        #endregion

        #region 方法

        /// <summary>
        ///  获取未登陆客户的临时ID(生成的负值)‎
        /// </summary>
        /// <returns>未登陆客户的临时CookID‎</returns>
        int GetCookIdforTempCustomerId();

        /// <summary>
        /// 获取客户购物车产品款数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>产品款数</returns>
        int GetShoppingCartProductCount(int customerId);

        /// <summary>
        /// 获取客户购物车下架产品款数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="languageId">当前语种</param>
        /// <returns>产品款数</returns>
        int GetShoppingCartUnAvailableProductCount(int customerId, int languageId);

        /// <summary>
        /// 合并购物车
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="tempCustomerId">购物车Id 未登录是临时ID(生成的负值）‎</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_:</value>
        /// </exception>
        void MergeShoppingCart‎(int customerId, int tempCustomerId);

        /// <summary>
        /// 点击CheckOut 修改下架产品的Remove_checked字段
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        void UpdateShoppingCartUnAvailableProductStatus(int shoppingCartId);

        /// <summary>
        /// 提交订单时验证是否又有新的商品不满足条件(可能又有新的商品被下架)
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <returns></returns>
        bool ValidateShoppingCartItem(int shoppingCartId);

        /// <summary>
        /// 根据产品Id获取该产品在购物车内中的数量，没有返回0. add by luohaiming 
        /// </summary>
        /// <remarks>wishlist 显示用</remarks>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productIds">产品Id集合</param>
        /// <returns>产品对应的购物车中的数量 Key：产品ID，value：产品数量</returns>
        Dictionary<int, int> GetShoppingCartProductsQuantity(int shoppingCartId, IList<int> productIds);

        /// <summary>
        /// 根据产品Id获取该产品在购物车内中的数量，没有返回0
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id集合</param>
        /// <returns>产品对应的购物车中的数量</returns>
        int GetShoppingCartProductsQuantity(int shoppingCartId, int productId);

        VShoppingCartItem GetShoppingCartProduct(int shoppingCartId, int productId);
        #region 查询
        /// <summary>
        /// 分页获取购物车产品明细
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        PageData<ShoppingCartItem> FindShoppingCartItems(int shoppingCartId, int currentPage, int pageSize,
            IDictionary<ShoppingCartSearchCriteria, object> searchCriteria, IList<Sorter<ShoppingCartSorterCriteria>> sorterCriteria);

        PageData<VShoppingCartItem> FindVShoppingCartItems(int shoppingCartId, int currentPage, int pageSize,
            IDictionary<ShoppingCartSearchCriteria, object> searchCriteria,
            IList<Sorter<ShoppingCartSorterCriteria>> sorterCriteria);
        /// <summary>
        /// 按类别分页获取购物车产品列表
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        //PageData<ShoppingCartItem> GetShoppingCartItemCategories(int customerId, int currentPage, int pageSize);

        /// <summary>
        /// 获取无效产品明细
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns>无效产品集合</returns>
        PageData<VShoppingCartItem> FindAllUnAvailableProducts(int shoppingCartId, int currentPage, int pageSize,
            IDictionary<ShoppingCartSearchCriteria, object> searchCriteria, IList<Sorter<ShoppingCartSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 得到用户购物车汇总信息
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <returns>ShoppingCart实体</returns>
        ShoppingCartSummary GetShoppingCartSummary(int shoppingCartId, int languageId, int currencyId, int countryId);

        #endregion

        #region  添加/修改
        /// <summary>
        /// 添加产品至购物车 
        /// </summary>
        /// <param name="shoppingCartItem‎">购物车产品对象</param>
        /// <returns>自增Id‎</returns>
        int AddProductToShoppingCart‎(ShoppingCartItem shoppingCartItem‎);

        /// <summary>
        /// 批量添加产品至购物车 
        /// </summary>
        /// <param name="shoppingCartItem‎s">购物车产品对象</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_ADD_PRODUCT:添加购物车失败</value>
        /// </exception>
        void BatchAddProductToShoppingCart(IList<ShoppingCartItem> shoppingCartItem‎s);

        /// <summary>
        /// 修改产品数量
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <param name="qty">数量</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// </exception>
        void UpdateShoppingCartProductQty‎(int shoppingCartId, int productId, int qty);

        /// <summary>
        /// 修改产品备注
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <param name="remark">产品备注</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// </exception>
        void SetShoppingCartProductRemark‎(int shoppingCartId, int productId, string remark);

        #endregion

        #region 删除
        /// <summary>
        /// 客户清空购物车
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_NOT_EXIST:购物车不存在</value>
        /// </exception>
        void CleanShoppingCart(int shoppingCartId);

        /// <summary>
        /// 根据产品Id删除购物车产品
        /// </summary>
        /// <param name="shoppingCartId">购物车Id 或未登录是临时ID(生成的负值）</param>
        /// <param name="productId">产品Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// </exception>
        void DeleteShoppingCartItemByProductId(int shoppingCartId, int productId);

        /// <summary>
        /// 将购物车所有产品移动到Wishlist
        /// </summary>
        /// <param name="shoppingCartId">购物车Id(客户Id）</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_CUSTOMER_NOT_EXIST:该客户不存在</value>
        /// </exception>
        void MoveAllToWishlist(int shoppingCartId);

        /// <summary>
        /// 单个产品Id移动到Wisthlist
        /// </summary>
        /// <param name="shoppingCartId">购物车Id(客户Id）</param>
        ///<param name="productId">产品Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST:购物车中该产品不存在</value>
        /// <value>ERROR_CUSTOMER_NOT_EXIST:该客户不存在</value>
        /// </exception>
        void MoveToWishlist(int shoppingCartId, int productId);

        #endregion

        #endregion

        /// <summary>
        /// 从购物车里移除已经Check的下架物品
        /// </summary>
        /// <param name="shoppingCartId">购物车Id(客户Id）</param>
        void RemoveShoppingCartUnAvailableProduct(int shoppingCartId);
    }

    public enum ShoppingCartSearchCriteria
    {
        /// <summary>
        /// 语种ID
        /// </summary>
        LanguageId,

    }

    public enum ShoppingCartSorterCriteria
    {
        /// <summary>
        /// 添加时间从新到旧
        /// </summary>
        AddedTimeNewToOld = 0,
        /// <summary>
        /// 添加时间从旧到新
        /// </summary>
        AddedTimeOldToNew,
        /// <summary>
        /// 产品价格从低到高
        /// </summary>
        PriceLowToHigh,
        /// <summary>
        /// 产品价格从高到低
        /// </summary>
        PriceHighToLow,
        /// <summary>
        /// 按照产品类别排序
        /// </summary>
        Catefory,
    }
}
