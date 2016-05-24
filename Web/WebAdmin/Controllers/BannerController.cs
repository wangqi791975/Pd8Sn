using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Marketing.Banner;

namespace Com.Panduo.Web.Controllers
{
    public class BannerController : Controller
    {
        /// <summary>
        /// 横幅广告（无倒计时）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            var list= ServiceFactory.BannerService.GetBanner(false);
            return View(list);
        }

        /// <summary>
        /// 倒计时Banner
        /// </summary>
        /// <returns></returns>
        public ActionResult CountdownIndex()
        {
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            var list = ServiceFactory.BannerService.GetBanner(true);
            return View(list);
        }

        [ValidateInput(false)]
        public ActionResult SetBanner()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                string[] languageIds = Request["HID_languageId"].Split(',');
                bool isCountdown = Request["HID_IsCountdown"].ParseTo<bool>(false);
                IList<BannerInfo> bannerInfos = new List<BannerInfo>();
                foreach (var languageId in languageIds)
                {
                    var limitBeginDate = Request["limitBeginDate_" + languageId] ?? string.Empty;
                    var limitEndDate = Request["limitEndDate_" + languageId] ?? string.Empty;
                    if (!limitBeginDate.IsNullOrEmpty())
                    {
                        if (limitEndDate.IsNullOrEmpty())
                        {
                            limitEndDate = DateTime.MaxValue.ToShortDateString();
                        }
                        BannerInfo bannerInfo = new BannerInfo()
                        {
                            LanguageId = languageId.ParseTo(1),
                            IsCountdown = isCountdown,
                            IsShowHome = Request["IsShowHome_" + languageId].ParseTo<bool>(true),
                            IsValid =  Request["IsValid_" + languageId].ParseTo<bool>(true),
                            Content = Request["banner_language_content_" + languageId] ?? string.Empty,
                            BannerStartTime =
                                Convert.ToDateTime(limitBeginDate + " " + Request["limitBeginTime_" + languageId] ??
                                                   string.Empty),
                            BannerEndTime =
                                Convert.ToDateTime(limitEndDate + " " + Request["limitEndTime_" + languageId] ??
                                                   string.Empty),
                        };
                        bannerInfos.Add(bannerInfo);
                    }
                }
                ServiceFactory.BannerService.SetBanner(bannerInfos);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }

            return Json(hashtable);
        }

    }
}
