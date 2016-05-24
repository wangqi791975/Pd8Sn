using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Marketing.Banner;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Home;

namespace Com.Panduo.Web.Controllers
{
    public class HomeAreaController : Controller
    {
        public ActionResult Index()
        {
            HomeAreaVo homeArea=new HomeAreaVo();
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            homeArea.HomeAreaSetting= ServiceFactory.BannerService.GetHomeAreaSetting(HomeAreaType.Navigation);
            return View(homeArea);
        }

        [ValidateInput(false)]
        public ActionResult SetHomeArea()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                #region 修改HomeAreaSetting
                string[] languageIds = Request["HID_languageId"].Split(',');
                IList<HomeAreaSetting> homeAreaSettings = new List<HomeAreaSetting>();
                foreach (var languageId in languageIds)
                {
                    HomeAreaSetting setting = new HomeAreaSetting()
                    {
                        AreaType = HomeAreaType.Navigation,
                        LanguageId = languageId.ParseTo(1),
                        Content = Request["homearea_language_content_" + languageId] ?? string.Empty,
                    };
                    homeAreaSettings.Add(setting);
                }
                ServiceFactory.BannerService.SetHomeAreaSetting(homeAreaSettings);
                #endregion
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
            //CacheHelper.ClearAllNavigationHomeAreaSetting();
            return Json(hashtable);
        }

        public ActionResult MiddleIndex()
        {
            var type = Request["type"].ParseTo(0);
            if (Request.IsAjaxRequest())
            {
                switch (type.ToEnum<HomeAreaType>())
                {
                    case HomeAreaType.RightCategory:
                        return HomeMiddle(HomeAreaType.RightCategory);
                    case HomeAreaType.BelowCategory:
                        return HomeMiddle(HomeAreaType.BelowCategory);
                    default:
                        return Content("请选择一项。。。");
                }
            }
            ViewBag.HidType = type;
            ViewBag.HidAreaId = Request["areaId"].ParseTo(0);
            return View();
        }

        public ActionResult HomeMiddle(HomeAreaType type)
        {
            HomeAreaVo homeArea = new HomeAreaVo();
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            homeArea.HomeAreaSetting = ServiceFactory.BannerService.GetHomeAreaSetting(type);
            ViewBag.HomeAreaType = (int) type;
            return View("~/Views/HomeArea/HomeMiddle.cshtml",homeArea);
        }

        [ValidateInput(false)]
        public ActionResult SetHomeAreaType()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                #region 修改HomeAreaSetting
                string[] languageIds = Request["HID_languageId"].Split(',');
                var type=  Request["HID_type"].ParseTo<int>();
                IList<HomeAreaSetting> homeAreaSettings = new List<HomeAreaSetting>();
                foreach (var languageId in languageIds)
                {
                    HomeAreaSetting setting = new HomeAreaSetting()
                    {
                        AreaType = type.ToEnum<HomeAreaType>(),
                        LanguageId = languageId.ParseTo(1),
                        Content = Request["homearea_language_content_" + languageId] ?? string.Empty,
                    };
                    homeAreaSettings.Add(setting);
                }
                ServiceFactory.BannerService.SetHomeAreaSetting(homeAreaSettings);
                #endregion
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
            //CacheHelper.ClearAllRightCategoryHomeAreaSetting();
            //CacheHelper.ClearAllBelowCategoryHomeAreaSetting();
            return Json(hashtable);
        }
    }
}
