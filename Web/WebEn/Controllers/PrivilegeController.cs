using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Customer;

namespace Com.Panduo.Web.Controllers
{
    public class PrivilegeController : BaseController
    {
        private int CustomerId
        {
            get { return SessionHelper.CurrentCustomer.CustomerId; }
        }


        public ActionResult Club()
        {
            var clubCustomer = ServiceFactory.ClubService.GetClubByCustomerId(CustomerId);
            var customerManager = new CustomerManager();
            if (clubCustomer != null)
            {
                customerManager = ServiceFactory.CustomerService.GetCustomerManager(clubCustomer.CustomerManagerId);

                if (customerManager == null)
                {
                    var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                    var language = languages.FirstOrDefault(x => x.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId);
                    if (language != null && language.CustomerManagerId != null)
                    {
                        customerManager = ServiceFactory.CustomerService.GetCustomerManager(language.CustomerManagerId ?? 0);
                    }
                }
            }
            

            ViewBag.ClubCustomer = clubCustomer;
            ViewBag.CustomerManager = customerManager;
            return View();
        }

        public ActionResult Invoice()
        {
            var clubCustomer = ServiceFactory.ClubService.GetClubByCustomerId(CustomerId);
            if (clubCustomer == null)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            if (clubCustomer.DateActived < DateTime.Now.AddYears(-1))
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var paymentInfo = ServiceFactory.PaymentService.GetPaypalInfo(clubCustomer.PayLogId);

            ViewBag.PaymentInfo = paymentInfo;
            ViewBag.ClubCustomer = clubCustomer;
            return View();
        }

        /// <summary>
        /// 获取MyProduct
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProducts()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(20);//页大小
            string keyWord = Request["keyword"] ?? string.Empty;
            
            
            IDictionary<CustomerProductSearchCriteria, object> search  = new Dictionary<CustomerProductSearchCriteria, object>();
            if (!keyWord.IsNullOrEmpty())
            {
                search.Add(CustomerProductSearchCriteria.KeyWrod, keyWord);
            }
            
            var sorter = new List<Sorter<CustomerProductSorterCriteria>>();
            var myProductsPageData = ServiceFactory.CustomerProductService.FindCustomerProducts(CustomerId, page,
                pageSize, search, sorter);
            var myProducts = myProductsPageData.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(myProducts.Select(p => p.ProductId).ToList(), isIncludeProductStock: false, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: false, isJudgeHasSimilarProuct: true);
            ViewBag.WishListType = ServiceFactory.WishListService.GetWishListType(ServiceFactory.ConfigureService.SiteLanguageId);
            ViewBag.ProductCategory = ServiceFactory.WishListService.GetWishListProductCategory(CustomerId, false);
            var vos = productInfos.Select(c => new MyProductsProductInfoVo { ProductInfo = c, CustomerProductInfo = myProducts.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();

            var pageData = new PageData<MyProductsProductInfoVo>
            {
                Data = vos,
                Pager = myProductsPageData.Pager
            };
            ViewBag.KeyWord = keyWord;
            if (Request.IsAjaxRequest())
            {
                return View("Partial/_ProductList", pageData);
            }
            
            return View(pageData);
        }

        

    }
}
