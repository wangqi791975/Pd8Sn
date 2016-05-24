using System.Linq;
using System.Text;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Review;
using Com.Panduo.Service.SEO;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Product;
using Com.Panduo.Web.Models.SEO;
using System;
using System.Web.Mvc;
using Com.Panduo.Web.Models.Home;

namespace Com.Panduo.Web.Controllers
{
    public class HomeController : BaseController
    {

        [OutputCache(CacheProfile = "HomeCacheProfile")]
        public ActionResult Index()
        {
            var user = ServiceFactory.AdminUserService.GetAdminUser(1);

            //左侧类别树
            var categoryTree = new CategoryTreeVo();
            categoryTree.TreeType = CategoryTreeVo.CategoryTreeType.Home;
            categoryTree.CategoryRelatedDatas = CacheHelper.CategoryLanguages;
            categoryTree.CurrentCategoryId = 0;
            categoryTree.CurrentParentCategoryId = 0;

            ViewBag.CurrentUser = user.IsNullOrEmpty() ? "Customer" : user.Name;
            ViewBag.CategoryTree = categoryTree;

            var metaHome = ServiceFactory.MetaService.GetMetaHomeByType(MetaHomePageType.Home, ServiceFactory.ConfigureService.SiteLanguageId);
            if (!metaHome.IsNullOrEmpty())
            {
                ViewBag.MetaInfo = new MetaInfoVo
                {
                    Breadcrumb = metaHome.Breadcrumb,
                    Title = metaHome.Title,
                    Keywords = metaHome.Keywords,
                    Description = metaHome.Description
                };
            }
            //缓存读取右侧，底部静态HTML
          var model= new HomeVo
            {
                BottomHtml = CacheHelper.GetHomeAreaBelowCategory(ServiceFactory.ConfigureService.SiteLanguageId),
                RightHtml = CacheHelper.GetHomeAreaRightCategory(ServiceFactory.ConfigureService.SiteLanguageId),
            };
          return View(model);
        }

        /// <summary>
        /// 提交综合评价
        /// </summary>
        /// <param name="testimonials">评价</param>
        /// <returns>是否提交成功</returns>
        public bool SubTestimonial(string testimonials)
        {
            try
            {
                if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
                {
                    var wenSiteReview = new WebSiteReview
                    {
                        LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                        CustomerId = SessionHelper.CurrentCustomer.CustomerId,
                        OrderId = 0,
                        ReviewType = ReviewType.Web,
                        ReviewContent = testimonials,
                        CreateDateTime = DateTime.Now,
                        AuditStatus = AuditStatus.AuditPass,
                        IsValid = true,
                        IsRecommend = false,
                    };
                    ServiceFactory.ReviewService.AddWebSiteReview(wenSiteReview);
                    return true;
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "":
                        break;
                }
            }
            return false;
        }

        //IP验证失败显示页面
        public ActionResult AccessDenied()
        {
            return View("Error/IpCheckError");
        }

        /// <summary>
        /// 脚本错误收集
        /// </summary>
        [ValidateInput(false)]
        public ActionResult ScriptError()
        { 
            if (Request.UrlReferrer == null || Request.QueryString.Count == 0)
            {
                return null;
            }

            var customer = SessionHelper.CurrentCustomer;
            var err = new StringBuilder();
            err.AppendLine(Environment.NewLine + "Referrer: " + Request.UrlReferrer.ToString());
            err.AppendLine("User Agent: " + Request.UserAgent ?? string.Empty);
            err.AppendLine("Cookie: " + Request.Browser.Cookies);
            err.AppendLine("IP: " + PageManager.GetClientIp());
            err.AppendLine("Customer Id: " + (customer == null ? string.Empty : customer.CustomerId.ToString()));
            err.AppendLine("Error: ");
            err.AppendLine(Request.QueryString.AllKeys.Select(c => string.Format("{0}: {1}", c, HttpUtility.UrlDecode(Request.QueryString[c]))).Join(Environment.NewLine));
            
            LoggerHelper.GetLogger(LoggerType.ScriptError).Info(err.ToString());

            return null;
        }
        
    }
}
