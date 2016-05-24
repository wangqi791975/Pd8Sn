using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Web.Models.WishList;

namespace Com.Panduo.Web.Controllers
{
    public class WishListController : Controller
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Search(int? id)
        {
            int page = id ?? 1;
            ViewBag.Page = page;
            return View();
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(50);
            string email = Request["customeremail"] != null ? Request["customeremail"].Trim() : string.Empty;
            string productNo = Request["keyword"] != null ? Request["keyword"].Trim() : string.Empty;
            IDictionary<WishListSearchCriteria, object> searchDictionary = new Dictionary<WishListSearchCriteria, object>();
            if (!email.IsNullOrEmpty())
            {
                searchDictionary.Add(WishListSearchCriteria.CustomerEmail, email);
            }
            if (!productNo.IsNullOrEmpty())
            {
                searchDictionary.Add(WishListSearchCriteria.ProductNo, productNo);
            }
            var wishListPageData = ServiceFactory.WishListService.GetAdminWishLists(page, pageSize, searchDictionary, null);
            var wishLists = wishListPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(wishLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: false, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: false, isJudgeHotSeller: false, isJudgeHasSimilarProuct: false);
            var vos = wishLists.Select(c => new CustomerWishListVo { WishListInfo = c, ProductInfo = productInfos.FirstOrDefault(w => w.Product.ProductId == c.ProductId) }).ToList();
            var pageData = new PageData<CustomerWishListVo>
            {
                Data = vos,
                Pager = wishListPageData.Pager
            };
            ViewBag.Email = email;
            ViewBag.KeyWord = productNo;
            return View(pageData);
        }
    }
}
