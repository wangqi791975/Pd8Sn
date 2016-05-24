using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Customer;

namespace Com.Panduo.Web.Controllers
{
    public class WishListController : BaseController
    {
        private int CustomerId
        {
            get { return SessionHelper.CurrentCustomer.CustomerId; }
        }
     
        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult AddToWishList(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var productId = form["productId"].ParseTo(-1);
                var productQty = form["productQty"].ParseTo(0);
                if (productId>0)
                {
                    hashtable["result"] = ActionJsonResult.Error;
                }
                ServiceFactory.WishListService.AddWishListProduct(new WishListProduct()
                    {
                        CustomerId = CustomerId,
                        WishListType = WishListType.Unclassified,
                        AddDateTime =DateTime.Now,
                        ProductId = productId,
                        Count = productQty
                    });
                hashtable["result"] = ActionJsonResult.Success;
            }
            catch (Exception ex)
            {
                //记录日志
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = ex.Message;
            }
            return Json(hashtable);
        }

        /// <summary>
        /// 获取我的心愿单
        /// </summary>
        /// <returns></returns>
        public ActionResult MyWishList()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(50);//页大小
            int categoryid = Request["wishlist_category"].ParseTo(-1);//分类
            int wishlistType = Request["wishlist_type"].ParseTo(-1);//类型
            int wishlistSort = Request["wishlist_sort"].ParseTo(0);//排序
            IDictionary<WishListSearchCriteria, object> search =new Dictionary<WishListSearchCriteria, object>();
            if (categoryid != -1)
            {
                search.Add(WishListSearchCriteria.CategoryId, categoryid);
            }
            if (wishlistType != -1)
            {
                search.Add(WishListSearchCriteria.ClassificationType, wishlistType);
            }
            var sort = new List<Sorter<WishListSorterCriteria>>();
            sort.Add(new Sorter<WishListSorterCriteria> { Key = WishListSorterCriteria.AddDate, IsAsc = (wishlistSort==1?true:false) });

            var wishListPageData = ServiceFactory.WishListService.GetWishListProducts(page, pageSize, CustomerId, search, sort);
            var wishLists = wishListPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(wishLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);
            ViewBag.WishListType=ServiceFactory.WishListService.GetWishListType(ServiceFactory.ConfigureService.SiteLanguageId);
            ViewBag.ProductCategory = ServiceFactory.WishListService.GetWishListProductCategory(CustomerId, false);
            var vos = productInfos.Select(c => new WishListProductInfoVo { ProductInfo = c, WishListInfo = wishLists.FirstOrDefault(w=>w.ProductId == c.Product.ProductId) }).ToList();

            var pageData = new PageData<WishListProductInfoVo>
                {
                    Data = vos,
                    Pager = wishListPageData.Pager
                };

            ViewBag.ShoppingCartProductQuantity = ServiceFactory.ShoppingCartService.GetShoppingCartProductsQuantity(CustomerId, wishLists.Select(p => p.ProductId).ToList());
            ViewBag.Sitemaps = Sitemap.GetMyAccountWishListSitemap();
            if (Request.IsAjaxRequest())
            {
                return View("WishListPageList", pageData);
            }
            return View(pageData);
        }

        /// <summary>
        /// 获取我的历史心愿单
        /// </summary>
        /// <returns></returns>
        public ActionResult MyWishHistoryList()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(50);//页大小
            int categoryid = Request["his_wishlist_category"].ParseTo(-1);//分类
            int wishlistType = Request["his_wishlist_type"].ParseTo(-1);//类型
            int wishlistSort = Request["his_wishlist_sort"].ParseTo(0);//排序

            IDictionary<WishListSearchCriteria, object> search = new Dictionary<WishListSearchCriteria, object>();
            if (categoryid != -1)
            {
                search.Add(WishListSearchCriteria.CategoryId, categoryid);
            }
            if (wishlistType != -1)
            {
                search.Add(WishListSearchCriteria.ClassificationType, wishlistType);
            }

            var sort = new List<Sorter<WishListSorterCriteria>>();
            sort.Add(new Sorter<WishListSorterCriteria> { Key = WishListSorterCriteria.AddDate, IsAsc = (wishlistSort == 1 ? true : false) });

            var wishListHistoryPageData = ServiceFactory.WishListService.GetWishListHistoryProducts(page, pageSize, CustomerId, search, sort);
            var wishLists = wishListHistoryPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(wishLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);
            ViewBag.WishListType = ServiceFactory.WishListService.GetWishListType(ServiceFactory.ConfigureService.SiteLanguageId);
            ViewBag.ProductCategory = ServiceFactory.WishListService.GetWishListProductCategory(CustomerId, true);
            var vos = productInfos.Select(c => new WishListProductInfoVo { ProductInfo = c, WishListInfo = wishLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();
            ViewBag.ShoppingCartProductQuantity = ServiceFactory.ShoppingCartService.GetShoppingCartProductsQuantity(CustomerId, wishLists.Select(p => p.ProductId).ToList());
            var pageData = new PageData<WishListProductInfoVo>
            {
                Data = vos,
                Pager = wishListHistoryPageData.Pager
            };
            ViewBag.Sitemaps = Sitemap.GetMyAccountWishListSitemap();
            return View("WishListHistoryPageList", pageData);
        }

        /// <summary>
        /// 获取我的心愿单将要被移除
        /// </summary>
        /// <returns></returns>
        public ActionResult MyWishListRemoved()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(50);//页大小
            var sort = new List<Sorter<WishListSorterCriteria>>();
            sort.Add(new Sorter<WishListSorterCriteria> { Key = WishListSorterCriteria.AddDate, IsAsc = true });

            var wishListPageData = ServiceFactory.WishListService.GetWishListRemovedProducts(page, pageSize, CustomerId, sort);
            var wishLists = wishListPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(wishLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: true);

            var vos = productInfos.Select(c => new WishListProductInfoVo { ProductInfo = c, WishListInfo = wishLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();

            var pageData = new PageData<WishListProductInfoVo>
            {
                Data = vos,
                Pager = wishListPageData.Pager
            };
            return View("WishListRemovedPageList", pageData);
        }

        /// <summary>
        /// 删除一个Wishlist
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveMyWishListOne()
        {
            var t = Request["pid"] ?? string.Empty;
            var ishistory = Request["h"].ParseTo<int>(0) ==1;
            var removed = Request["r"].ParseTo<int>(-1);


            if (removed != -1)
            {
                ServiceFactory.WishListService.RemoveWishListProduct(CustomerId, t.ParseTo(-1), removed==1);
                return MyWishListRemoved();
            }

            ServiceFactory.WishListService.RemoveWishListProduct(CustomerId, t.ParseTo(-1), ishistory);
            if (ishistory)
            {
                return MyWishHistoryList();
            }
            else
            {
                return MyWishList();
            }
        }

        /// <summary>
        /// 批量移除Wishlist
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveMyWishList()
        {
            IList<KeyValuePair<int,bool>> list=new List<KeyValuePair<int, bool>>();
            var t = Request["ckb"] ?? string.Empty;
            var ishistory = Request["h"].IsNullOrEmpty()?false:true;
            if (!t.IsNullOrEmpty())
            {
                var productlist = t.Split<int>(",");
                foreach (var i in productlist)
                {
                    list.Add(new KeyValuePair<int, bool>(i, ishistory));
                }
                ServiceFactory.WishListService.RemoveWishListProduct(CustomerId, list);
            }
            if (ishistory)
            {
                return MyWishHistoryList();
            }
            else
            {
                return MyWishList();
            }
        }

        /// <summary>
        /// 批量设置Wishlist
        /// </summary>
        /// <returns></returns>
        public ActionResult SetMyWishList()
        {
            IDictionary<int,WishListType> dic=new Dictionary<int, WishListType>();
            var t = Request["ckb"]??string.Empty;
            var ishistory = Request["h"].IsNullOrEmpty() ? false : true;
            string pre = "a_";//前缀
            if (!t.IsNullOrEmpty())
            {
                var productlist = t.Split<int>(",");
                foreach (var i in productlist)
                {
                    var j = Request[pre + i.ToString()] ?? ((int)WishListType.Unclassified).ToString();
                    if (dic.ContainsKey(i))
                    {
                        dic.Remove(i);
                    }
                    dic.Add(i, j.ToInt().ToEnum<WishListType>());
                }
                ServiceFactory.WishListService.SetWishListType(CustomerId, dic, ishistory);
            }
            if (ishistory)
            {
                return MyWishHistoryList();
            }
            else
            {
                return MyWishList();
            }
        }
    }
}
