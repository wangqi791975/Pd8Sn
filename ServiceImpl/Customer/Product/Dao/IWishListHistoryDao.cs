//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IWishListHistoryDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录：2015/02/05 09:59:50
//修改人：    罗海明 
//修改时间： 添加方法
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{
    public interface IWishListHistoryDao : IBaseDao<WishListHistoryPo, int>
    {
        /// <summary>
        /// 获取一条历史wishlist信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="wishlistId">心愿单Id</param>
        /// <returns></returns>
        WishListHistoryPo GetWishList(int customerId, int wishlistId);

        /// <summary>
        /// 获取一条历史wishlist信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        WishListHistoryPo GetWishListHistoryByProductId(int customerId, int productId);

        /// <summary>
        /// 通过客户Id产品Id删除wishlist历史
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="productId">产品Id</param>
       void DeleteWishListHistory(int customerId, int productId);


       /// <summary>
       /// 通过客户Id,wishlistIds批量删除历史wishlist
       /// </summary>
       /// <param name="customerId">客户Id</param>
       /// <param name="productIds">产品Id List</param>
       void DeleteWishListHistoryByIds(int customerId, IList<int> productIds);

       /// <summary>
       /// 修改WishList历史的喜爱类型
       /// </summary>
       /// <param name="customerId">客户Id</param>
       /// <param name="wishlistId">心愿单Id</param>
       /// <param name="wishListType">喜爱类型</param>
       void UpdateWishListHistoryTpye(int customerId, int wishlistId, int wishListType);

        /// <summary>
        /// 根据客户Id和语种Id获取客户WishListHistory产品有哪些类别
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        IList<KeyValuePair<int, string>> GetWishListHistoryCategory(int languageId, int customerId);

        /// <summary>
        /// 查询移除的wishlist产品
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="customerId">客户Id</param>
        /// <param name="sorterCriteria">排序</param>
        /// <returns></returns>
        PageData<WishListProduct> GetWishListRemovedProducts(int currentPage, int pageSize, int customerId,
                                                             IList<Sorter<WishListSorterCriteria>> sorterCriteria);

    } 
}
   