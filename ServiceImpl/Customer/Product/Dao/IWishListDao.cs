using System.Collections.Generic;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public interface IWishListDao : IBaseDao<WishListPo, int>
    {

        /// <summary>
        /// 获取一条wishlist信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="wishlistId">心愿单Id</param>
        /// <returns></returns>
        WishListPo GetWishList(int customerId, int wishlistId);

        /// <summary>
        /// 获取一条wishlist信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        WishListPo GetWishListByProductId(int customerId, int productId);

        /// <summary>
        /// 通过客户Id产品Id删除wishlist
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        void DeleteWishListProduct(int customerId, int productId);

        /// <summary>
        /// 通过客户Id,wishlistIds批量删除wishlist
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productIds">产品Id List</param>
        void DeleteWishListProudctByIds(int customerId, IList<int> productIds);
        

        /// <summary>
        /// 修改喜爱类型
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="wishlistId">心愿单Id</param>
        /// <param name="wishListType">喜爱类型</param>
        void UpdateWishListType(int customerId, int wishlistId, int wishListType);

        /// <summary>
        /// 根据客户Id和语种Id获取客户wishList产品有哪些类别，[add By luohaiming]
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="customerId">客户Id</param>
        IList<KeyValuePair<int, string>> GetWishListProductCategory(int languageId, int customerId);

        /// <summary>
        /// 通过客户ID获取wishlist个数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        int GetWishListCountByCustomerId(int customerId);
    }
}