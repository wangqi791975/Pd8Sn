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
using System.Linq;
using System.Text;
using Com.Panduo.Entity.ShoppingCart;

namespace Com.Panduo.ServiceImpl.Order.ShoppingCart.Dao
{
    public interface IShoppingCartDao : IBaseDao<ShoppingCartPo, int>
    {

        ShoppingCartPo GetProductInShoppingCartByCustomerId(int customerId, int productId);

        int GetShoppingCartProductCount(int customerId);

        void UpdateShoppingCartUnAvailableProductStatus(int shoppingCartId);

        void MergeShoppingCart‎(int customerId, int tempCustomerId);

        bool ValidateShoppingCartItem(int shoppingCartId);

        Service.Order.ShoppingCart.ShoppingCartSummary GetShoppingCartSummary(int shoppingCartId, int languageId, int currencyId, int countryId);

        void MoveAllToWishlist(int shoppingCartId);

        IList<ShoppingCartPo> GetShoppingCartProductsQuantity(int shoppingCartId, IList<int> productIds);
    }
}
