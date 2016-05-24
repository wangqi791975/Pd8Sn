using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Marketing.Gift;
using Com.Panduo.Service.Marketing.PlaceOrder;
using Com.Panduo.Service.Marketing.ShippingMarketing;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Web.Common;
using NPOI.SS.Formula.Functions;

namespace Com.Panduo.Web.Controllers
{
    public class MarketingController : BaseController
    {

        #region 系统页面营销推荐
        [HttpGet]
        public ActionResult PageNotFoundAd()
        {
            ViewBag.HomeUrl = ServiceFactory.ConfigureService.GetConfig("HomeUrl").Value;
            ViewBag.NewArrUrl = ServiceFactory.ConfigureService.GetConfig("NewArrUrl").Value;
            ViewBag.HomePath = ServiceFactory.ConfigureService.GetConfig("HomePath").Value;
            ViewBag.NewArrPath = ServiceFactory.ConfigureService.GetConfig("NewArrPath").Value;
            ViewBag.BestSellUrl = ServiceFactory.ConfigureService.GetConfig("BestSellUrl").Value;
            ViewBag.SaleUrl = ServiceFactory.ConfigureService.GetConfig("SaleUrl").Value;
            ViewBag.BestSellPath = ServiceFactory.ConfigureService.GetConfig("BestSellPath").Value;
            ViewBag.SalePath = ServiceFactory.ConfigureService.GetConfig("SalePath").Value;
            return View();
        }

        [HttpPost]
        public ActionResult PageNotFoundAd(string homeUrl, string newArrUrl, string bestSellUrl, string saleUrl)
        {
            var configHomeUrl = new Config
            {
                Key = "HomeUrl",
                Value = homeUrl
            };
            ServiceFactory.ConfigureService.SetConfig(configHomeUrl);
            var configNewArrUrl = new Config
            {
                Key = "NewArrUrl",
                Value = newArrUrl
            };
            ServiceFactory.ConfigureService.SetConfig(configNewArrUrl);
            var configBestSellUrl = new Config
            {
                Key = "BestSellUrl",
                Value = bestSellUrl
            };
            ServiceFactory.ConfigureService.SetConfig(configBestSellUrl);
            var configSaleUrl = new Config
            {
                Key = "SaleUrl",
                Value = saleUrl
            };
            ServiceFactory.ConfigureService.SetConfig(configSaleUrl);
            foreach (string upload in Request.Files)
            {
                var file = Request.Files[upload];
                if (file != null && file.ContentLength == 0) continue;
                string filePath = Path.Combine(HttpContext.Server.MapPath("../Upload/MarketingAd"),
                        upload + Path.GetExtension(file.FileName));
                if (Request.Url != null)
                {
                    string path = "http://" + Request.Url.Host.ToString(CultureInfo.InvariantCulture) + "/Upload/MarketingAd/" + upload + Path.GetExtension(file.FileName);
                    var config = new Config
                    {
                        Key = upload + "Path",
                        Value = path
                    };
                    ServiceFactory.ConfigureService.SetConfig(config);
                }
                file.SaveAs(filePath);
            }
            ViewBag.HomeUrl = ServiceFactory.ConfigureService.GetConfig("HomeUrl").Value;
            ViewBag.NewArrUrl = ServiceFactory.ConfigureService.GetConfig("NewArrUrl").Value;
            ViewBag.HomePath = ServiceFactory.ConfigureService.GetConfig("HomePath").Value;
            ViewBag.NewArrPath = ServiceFactory.ConfigureService.GetConfig("NewArrPath").Value;
            ViewBag.BestSellUrl = ServiceFactory.ConfigureService.GetConfig("BestSellUrl").Value;
            ViewBag.SaleUrl = ServiceFactory.ConfigureService.GetConfig("SaleUrl").Value;
            ViewBag.BestSellPath = ServiceFactory.ConfigureService.GetConfig("BestSellPath").Value;
            ViewBag.SalePath = ServiceFactory.ConfigureService.GetConfig("SalePath").Value;
            return View();
        }
        #endregion

        #region 运费活动
        [HttpGet]
        public ActionResult Shipping()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShippingList(string keyword, MarketingSorterCriteria? sorter)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<MarketingSearchCriteria, object>
            {
                {MarketingSearchCriteria.MarketingType, MarketingType.Shipping}
            };
            var sorterCriteria = new List<Sorter<MarketingSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(MarketingSearchCriteria.Name, keyword);
            }
            if (sorter.HasValue)
            {
                var sort = new Sorter<MarketingSorterCriteria>(sorter.Value, true);
                sorterCriteria.Add(sort);
            }

            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();
            var shippingMarketing = ServiceFactory.MarketingService.FindShippingMarketings(page, pageSize, searchCriteria, sorterCriteria);
            return View(shippingMarketing);
        }

        [HttpGet]
        public ActionResult ShippingEdit(int id)
        {
            #region 公共信息设置
            //  初始化语种、国家等资料
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();
            ViewBag.CustomerGroupDescs = ServiceFactory.CustomerService.GetCustomerGroupDescsByLanguage(ServiceFactory.ConfigureService.EnglishLangId).ToList();
            ViewBag.Continents = ServiceFactory.ConfigureService.GetAllContinent().ToList();
            ViewBag.Shippings = ServiceFactory.ShippingService.GetAllShippings();
            ViewBag.Countries = new Dictionary<int, List<Country>>();
            foreach (var continent in ViewBag.Continents)
            {
                ViewBag.Countries[continent.ContinentId] = ServiceFactory.ConfigureService.GetAllCountryByContinent(continent.ContinentId);
            }
            #endregion

            //订单折扣活动信息
            var shippingMarketing = new ShippingMarketing
            {
                MarketingType = MarketingType.Shipping,
                IsExcludeChannels = true,
                IsExcludeClub = true,
                Status = true
            };
            if (id > 0)
            {
                try
                {
                    shippingMarketing = ServiceFactory.MarketingService.GetShippingMarketingById(id);
                    if (!shippingMarketing.IsNullOrEmpty() && !shippingMarketing.ShippingDiscounts.IsNullOrEmpty())
                        shippingMarketing.ShippingDiscounts.ForEach(x => x.Discount = (1M - x.Discount) * 100.0M);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_MARKETING_NOT_EXIST":
                            ViewBag.ErrorMsg = "活动不存在！";
                            break;
                    }
                }
            }
            ViewBag.ShippingMarketing = shippingMarketing;

            //ViewBag.Shippings = ServiceFactory.ShippingService.FindAllShippings(1,2,new Dictionary<ShippingSearchCriteria, object>(), new List<Sorter<ShippingSorterCriteria>>() );
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("运费活动", Url.Content("/Marketing/Shipping")));

            return View();
        }

        [HttpPost]
        public JsonResult ShippingSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var marketingId = form["marketingId"].ParseTo(0);
                var shippingMarketingId = form["shippingMarketingId"].ParseTo(0);
                var freeShippingId = form["freeShippingId"].ParseTo(0);
                var customerType = form["FD_CustomerType"].ParseTo<int>().ToEnum<MarketingCustomerType>();
                var isExcludeClub = form["FD_IsExcludeClub"].ParseTo(0);
                var isExcludeChannels = form["FD_IsExcludeChannels"].ParseTo(0);
                var vipIds = form["FD_CustomerVipIds"];
                var languages = form["FD_LanguageIds"];
                var countryIds = form["FD_CountryIds"];
                var amountType = form["FD_AmountType"].ParseTo(0);
                var shippingRewardType = form["FD_RewardType"].ParseTo(0).ToEnum<ShippingRewardType>();
                var marketingName = form["FD_Name"];
                var begindate = form["FD_EffectiveBegin_Date"];
                var begintime = form["FD_EffectiveBegin_Time"];
                var enddate = form["FD_EffectiveEnd_Date"];
                var endtime = form["FD_EffectiveEnd_Time"];
                var status = form["FD_Status"].ParseTo(0);

                #region 免运费
                var amount = form["FD_Amount"].ParseTo(0M);
                var baseshippingid = form["FD_Baseshippingid"].ParseTo(0);
                var freeShippingFee = form["FD_FreeShippingFee"].ParseTo(0M);
                #endregion
                var weightLimit = form["FD_WeightLimit"].ParseTo(0M);

                var discounts = form["discounts"];
                var upgrades = form["upgrades"];
                var shippingIds = form["shippingIds"];
                if (marketingName.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "活动名称不能为空！";
                    return Json(hashtable);
                }
                if (begindate.IsNullOrEmpty() || enddate.IsEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "有效期开始日期不能为空！";
                    return Json(hashtable);
                }
                var shippingMarketing = new ShippingMarketing
                {
                    Id = marketingId,
                    ShippingMarketingId = shippingMarketingId,
                    MarketingType = MarketingType.Shipping,
                    CustomerType = customerType,
                    WeightLimit = weightLimit,
                    IsExcludeClub = isExcludeClub > 0,
                    IsExcludeChannels = isExcludeChannels > 0,
                    Name = marketingName,
                    AmountType = amountType.ParseTo<MarketingAmountType>(),
                    CountryIds = new List<int>(),
                    LanguageIds = new List<int>(),
                    CustomerVipIds = new List<int>(),//vip等级 
                    ClubLevels = new List<int>(),//去掉
                    CurrencyIds = new List<int>(),//去掉
                    ShippingIds = new List<int>(),
                    CustomerInfo = new Dictionary<int, string>(),//导入客户
                    Status = status > 0,
                    EffectiveBegin = Convert.ToDateTime(begindate + " " + begintime),
                    EffectiveEnd = Convert.ToDateTime(enddate + " " + endtime)
                };

                #region idList

                switch (customerType)
                {
                    case MarketingCustomerType.ImportCustomer://导入客户
                        {
                            var file = Request.Files["file_importcustomers"];
                            if (file != null)
                            {
                                var filePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportCustomer"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName));
                                file.SaveAs(filePath);
                                shippingMarketing.CustomerInfo = ExcelReadToDictionary(filePath);
                                if (System.IO.File.Exists(filePath))
                                    System.IO.File.Delete(filePath);
                            }
                        } break;
                    case MarketingCustomerType.VipCustomer://vip等级
                        if (!vipIds.IsNullOrEmpty())
                        {
                            var viplist = vipIds.Split<int>(",");
                            foreach (var v in viplist.Where(v => v > 0))
                            {
                                shippingMarketing.CustomerVipIds.Add(v);
                            }
                        }
                        break;
                }

                //shippingId ids
                if (!shippingIds.IsNullOrEmpty())
                {
                    foreach (var item in shippingIds.Split<int>(",").Where(l => l > 0))
                        shippingMarketing.ShippingIds.Add(item);
                }

                //language ids
                if (!languages.IsNullOrEmpty())
                {
                    var languagelist = languages.Split<int>(",");
                    foreach (var l in languagelist.Where(l => l > 0))
                        shippingMarketing.LanguageIds.Add(l);
                }

                //国家Ids
                if (!countryIds.IsNullOrEmpty())
                {
                    var countrylist = countryIds.Split<int>(",");
                    foreach (var countryId in countrylist.Where(countryId => countryId > 0))
                    {
                        shippingMarketing.CountryIds.Add(countryId);
                    }
                }

                #endregion

                switch (shippingRewardType)
                {
                    case ShippingRewardType.ShippingDiscount:
                        var shippingDiscounts = new List<ShippingDiscount>();

                        if (discounts.IsNullOrEmpty())
                        {
                            hashtable["result"] = ActionJsonResult.Error;
                            hashtable["msg"] = "discounts data is null";
                            return Json(hashtable);
                        }
                        else
                        {
                            shippingDiscounts.AddRange(from discount in discounts.Split(";")
                                                       where !discount.IsNullOrEmpty() && discount.Contains('|')
                                                       select discount.Split('|')
                                                           into str
                                                           select new ShippingDiscount()
                                                              {
                                                                  Amount = str[0].ParseTo<decimal>(),
                                                                  Discount = 1M - str[1].ParseTo<decimal>() / 100.0M,
                                                              });
                        }
                        shippingDiscounts = shippingDiscounts.OrderByDescending(x => x.Discount).ToList();

                        ServiceFactory.MarketingService.SetShippingDiscountMarketing(shippingMarketing, shippingDiscounts);
                        break;
                    case ShippingRewardType.ShippingUpgrade:
                        if (upgrades.IsNullOrEmpty())
                        {
                            hashtable["result"] = ActionJsonResult.Error;
                            hashtable["msg"] = "Upgrades data is null";
                            return Json(hashtable);
                        }
                        else
                        {
                            var shippingUpgrades = new List<ShippingUpgrade>();
                            shippingUpgrades.AddRange(from upgrade in upgrades.Split(";")
                                                      where !upgrade.IsNullOrEmpty() && upgrade.Contains('|')
                                                      select upgrade.Split('|')
                                                          into str
                                                          select new ShippingUpgrade()
                                                           {
                                                               Marketingshippingid = shippingMarketing.Id,
                                                               Amount = str[0].ParseTo<decimal>(),
                                                               ShippingId = str[1].ParseTo<int>(),
                                                               Upshippingid = str[2].ParseTo<int>(),
                                                           });
                            shippingUpgrades = shippingUpgrades.OrderBy(x => x.Amount).ToList();
                            ServiceFactory.MarketingService.SetShippingUpgrade(shippingMarketing, shippingUpgrades);
                        }
                        break;
                    case ShippingRewardType.FreeShipping:
                    default:
                        ServiceFactory.MarketingService.SetFreeShippingMarketing(shippingMarketing, new FreeShipping
                        {
                            Id = freeShippingId,
                            Marketingshippingid = shippingMarketing.Id,
                            Amount = amount,
                            Baseshippingid = baseshippingid,
                            FreeShippingFee = freeShippingFee
                        });
                        break;
                }

                hashtable["result"] = ActionJsonResult.Success;

            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["msg"] = "活动不存在.";
                        break;
                }
            }
            catch (Exception exp)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }


        public ActionResult ShippingDelete(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var marketingId = form["marketingid"].ParseTo<int>();
                if (marketingId > 0)
                {
                    ServiceFactory.MarketingService.DeleteShippingMarketingById(marketingId);
                    hashtable["result"] = ActionJsonResult.Success;
                }
                else
                {
                    hashtable["result"] = ActionJsonResult.Failing;
                    hashtable["msg"] = "ID错误";
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["msg"] = "活动不存在.";
                        break;
                }
            }
            return Json(hashtable);
        }
        #endregion

        #region Coupon活动

        [HttpGet]
        public ActionResult CouponMarketing(int id = 1)
        {
            ViewBag.Page = id;
            return View("Coupon/CouponMarketing");
        }

        [HttpGet]
        public ActionResult CouponMarketingList(string keyword, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<MarketingSearchCriteria, object>
            {
                {MarketingSearchCriteria.MarketingType, MarketingType.SendCoupon}
            };
            var sorterCriteria = new List<Sorter<MarketingSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(MarketingSearchCriteria.Name, keyword);
            }
            PageData<VMarketingCoupon> cMarketingCoupon = ServiceFactory.MarketingService.FindCouponMarketings(page, pageSize, searchCriteria, sorterCriteria);
            return View("Coupon/CouponMarketingList", cMarketingCoupon);
        }

        [HttpGet]
        public ActionResult CouponMarketingEdit(int id = 0)
        {
            //场景
            var itemsMarketingType = new List<SelectListItem>
            {
                new SelectListItem{Text = "请选择",Value=(-1).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "注册",Value=((int)CouponMarketingRewardType.Register).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "生日",Value=((int)CouponMarketingRewardType.Birthday).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "Club每月coupon",Value=((int)CouponMarketingRewardType.Club).ToString(CultureInfo.InvariantCulture)}
            };
            ViewBag.CouponMarketingRewardType = new SelectList(itemsMarketingType, "Value", "Text");
            //客户类型
            var itemsCustomerType = new List<SelectListItem>
            {
                new SelectListItem{Text = "全部客户",Value=((int)MarketingCustomerType.AllCustomer).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "VIP客户",Value=((int)MarketingCustomerType.VipCustomer).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "Club客户",Value=((int)MarketingCustomerType.ClubCustomer).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "渠道商客户",Value=((int)MarketingCustomerType.ChannelCustomer).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "批量导入",Value=((int)MarketingCustomerType.ImportCustomer).ToString(CultureInfo.InvariantCulture)}
            };
            ViewBag.MarketingCustomerType = new SelectList(itemsCustomerType, "Value", "Text");
            //VIP等级信息
            ViewBag.CustomerGroupDescs = ServiceFactory.CustomerService.GetAllCustomerGroups().Select(GetCustomerGroupInfo).ToList();
            //Club等级
            ViewBag.ClubLevel = new Dictionary<ClubType, string>
            {
                { ClubType.One,"一期"},{ClubType.Two, "二期"}
            };
            //语种
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();

            var couponMarketing = new CouponMarketing();
            if (id != 0)
            {
                try
                {
                    couponMarketing = ServiceFactory.MarketingService.GetCouponMarketingById(id);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_MARKETING_NOT_EXIST":
                            ViewBag.ErrorMsg = "活动不存在！";
                            break;
                    }
                }
            }
            ViewBag.CouponMarketingId = id;
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("Coupon活动", Url.Content("/Marketing/CouponMarketing")));

            return View("Coupon/CouponMarketingEdit", couponMarketing);
        }

        [HttpGet]
        public ActionResult CouponMarketingDetail(int id, int page = 1)
        {
            const string customerObjInfo = "";
            const string languageInfo = "";
            var couponMarketing = ServiceFactory.MarketingService.GetCouponMarketingById(id);
            switch (couponMarketing.CustomerType)
            {
                case MarketingCustomerType.VipCustomer:
                    var customerGroups = ServiceFactory.CustomerService.GetAllCustomerGroups().Select(GetCustomerGroupInfo).ToList();
                    couponMarketing.CustomerVipIds.Aggregate(customerObjInfo, (current, vipId) => current + customerGroups.Find(m => m.Key == vipId).Value + " ");
                    break;
                case MarketingCustomerType.ClubCustomer:
                    couponMarketing.ClubLevels.Aggregate(customerObjInfo, (current, clubLevel) => current + (clubLevel == 1 ? "Club一期 " : "Club二期 "));
                    break;
            }
            ViewBag.CustomerObjInfo = customerObjInfo;

            couponMarketing.LanguageIds.Aggregate(languageInfo,
                (current, languageId) =>
                    current +
                    ServiceFactory.ConfigureService.GetAllValidLanguage()
                        .ToList()
                        .Find(m => m.LanguageId == languageId)
                        .ChineseName + " ");
            ViewBag.LanguageInfo = languageInfo;
            ViewBag.Page = page;
            return View("Coupon/CouponMarketingDetail", couponMarketing);
        }

        [HttpPost]
        public ActionResult CouponMarketingEdit(CouponMarketing couponMarketing, string customerVipIds, string clubLevels, string languageIds, int statue,
                                                string limitBeginDate, bool ispclub, bool ispchannel, string limitEndDate, string limitBeginTime = "16:00", string limitEndTime = "15:59")
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            //语种
            foreach (var languageid in languageIds.Split('|'))
            {
                couponMarketing.LanguageIds.Add(Convert.ToInt32(languageid));
            }
            //国家
            couponMarketing.CountryIds = new List<int>();
            foreach (var country in ServiceFactory.ConfigureService.GetAllValidCountry())
            {
                couponMarketing.CountryIds.Add(country.CountryId);
            }
            switch (couponMarketing.CustomerType)
            {
                case MarketingCustomerType.VipCustomer:
                    foreach (var customerVipId in customerVipIds)
                    {
                        couponMarketing.CustomerVipIds.Add(Convert.ToInt32(customerVipId));
                    }
                    break;
                case MarketingCustomerType.ClubCustomer:
                    foreach (var clubLevel in clubLevels)
                    {
                        couponMarketing.ClubLevels.Add(Convert.ToInt32(clubLevel));
                    }
                    break;
                case MarketingCustomerType.ImportCustomer:
                    foreach (string upload in Request.Files)
                    {
                        var file = Request.Files[upload];
                        string filePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportCustomer"),
                            upload + Path.GetExtension(file.FileName));
                        file.SaveAs(filePath);
                        couponMarketing.CustomerInfo = ExcelReadToDictionary(filePath);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                    break;
            }
            //币种初始化
            couponMarketing.CurrencyIds = new List<int>();
            try
            {
                couponMarketing.EffectiveBegin = Convert.ToDateTime(limitBeginDate + " " + limitBeginTime);
                couponMarketing.EffectiveEnd = Convert.ToDateTime(limitEndDate + " " + limitEndTime);
                couponMarketing.Status = statue == 1;
                couponMarketing.IsExcludeClub = ispclub;
                couponMarketing.IsExcludeChannels = ispchannel;
                if (couponMarketing.Id == 0)
                    ServiceFactory.MarketingService.AddCouponMarketing(couponMarketing);
                else
                    ServiceFactory.MarketingService.UpdateCouponMarketing(couponMarketing);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_CUSTOMERCLAIM_ISNULL":
                        hashtable["msg"] = "导入的客户不存在";
                        hashtable["error"] = true;
                        break;
                }
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult GetCouponInfo(string couponCode, string languageIds, int couponmarketId)
        {
            var hashtable = new Hashtable { { "error", false }, { "isnull", false } };
            var coupon = ServiceFactory.CouponService.GetCoupon(couponCode.Trim());
            if (coupon.IsNullOrEmpty())
            {
                hashtable["isnull"] = true;
                hashtable["error"] = true;
                hashtable.Add("errormsg", "当前Coupon不存在，请修改");
                return Json(hashtable);
            }
            hashtable.Add("amount", ServiceFactory.ConfigureService.GetCurrency(coupon.AmountCurrencyId).CurrencyCode + coupon.Amount.Value.ToString(CultureInfo.InvariantCulture));
            hashtable.Add("minamount", ServiceFactory.ConfigureService.GetCurrency(coupon.MinAmountCurrencyId).CurrencyCode + coupon.MinAmount.Value.ToString(CultureInfo.InvariantCulture));
            hashtable.Add("validate", coupon.LimitType == LimitType.Day ? "领取后" + coupon.LimitDay + "天"
                : (coupon.LimitBeginTime.HasValue ? coupon.LimitBeginTime.Value.ToString("yyyy.MM.dd HH:mm:ss") : "") + "-" + (coupon.LimitEndTime.HasValue ? coupon.LimitEndTime.Value.ToString("yyyy.MM.dd HH:mm:ss") : ""));
            hashtable.Add("pickdate", coupon.LimitType == LimitType.Day ?
                (coupon.PickBeginTime.HasValue ? coupon.PickBeginTime.Value.ToString("yyyy.MM.dd HH:mm:ss") : "") + "-" + (coupon.PickEndTime.HasValue ? coupon.PickEndTime.Value.ToString("yyyy.MM.dd HH:mm:ss") : "") : "");

            if ((coupon.LimitType == LimitType.BeginEnd && coupon.LimitEndTime < DateTime.Now) || (coupon.LimitType == LimitType.Day && coupon.PickEndTime < DateTime.Now))
            {
                hashtable["error"] = true;
                hashtable.Add("errormsg", "当前Coupon已经关闭，不能再绑定活动，请修改");
                return Json(hashtable);
            }

            var couponmarketing = ServiceFactory.MarketingService.GetCouponMarketingByCode(couponCode.Trim());
            if (!couponmarketing.IsNullOrEmpty() && couponmarketing.Id != couponmarketId)
            {
                hashtable["error"] = true;
                hashtable.Add("errormsg", "当前Coupon Code已经使用过，不允许再使用");
                return Json(hashtable);
            }

            string cunlanmsg = "";
            if (coupon.CountryIds != "All")
            {
                hashtable["error"] = true;
                cunlanmsg = "当前Coupon的使用国家不包含当前活动国家的范围；";
            }

            if (coupon.LanguageIds != "All")
            {
                if (languageIds == "" || languageIds.Split(new[] { '|', ',' }).All(languageId => coupon.LanguageIds.Split(',', '|').Contains(languageId)))
                    return Json(hashtable);
                hashtable["error"] = true;
                cunlanmsg = cunlanmsg + "当前Coupon的使用语种不包含当前活动语种的范围；";
            }
            if (cunlanmsg != "")
            {
                cunlanmsg = cunlanmsg + "请重新设置";
                hashtable.Add("errormsg", cunlanmsg);
            }

            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult CouponDelete(int id)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            try
            {
                ServiceFactory.MarketingService.DeleteCouponMarketingById(id);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "活动不存在！";
                        break;
                }
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult ChangeStatus(int id, int status)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            try
            {
                ServiceFactory.MarketingService.UpdateCouponMarketingStatus(id, !Convert.ToBoolean(status));
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "活动不存在！";
                        break;
                }
            }
            return Json(hashtable);
        }

        #region 辅助方法
        private KeyValuePair<int, string> GetCustomerGroupInfo(CustomerGroup customerGroup)
        {
            return new KeyValuePair<int, string>(customerGroup.Id, ServiceFactory.CustomerService.GetCustomerGroupDesc(customerGroup.Id, ServiceFactory.ConfigureService.EnglishLangId).GroupName + "(" + Convert.ToInt32((1 - customerGroup.Discount) * 100) + "%)");
        }

        /// <summary>
        /// 读取处理内容信息
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns></returns>
        private Dictionary<int, string> ExcelReadToDictionary(string file)
        {
            try
            {
                var custonrtInfo = new Dictionary<int, string>();
                using (var excelHelper = new ExcelHelper(file))
                {
                    var dt = excelHelper.ExcelToDataTable("MySheet", true);
                    foreach (DataRow row in dt.Rows)
                    {
                        string email = row["email"].ToString().Trim();
                        var customer = ServiceFactory.CustomerService.GetCustomerByEmail(email);
                        if (!customer.IsNullOrEmpty())
                        {
                            custonrtInfo.Add(customer.CustomerId, customer.Email);
                        }
                    }
                }
                return custonrtInfo;
            }
            catch
            {
                return null;
            }
        }

        #endregion
        #endregion

        #region 订单折扣活动
        [HttpGet]
        public ActionResult OrderDiscount()
        {
            return View("OrderDiscount/OrderDiscount");
        }

        [HttpGet]
        public ActionResult OrderDiscountList(string keyword, MarketingSorterCriteria? sorter)
        {
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();

            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<MarketingSearchCriteria, object>
            {
                {MarketingSearchCriteria.MarketingType, MarketingType.OrderDiscount}
            };
            var sorterCriteria = new List<Sorter<MarketingSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(MarketingSearchCriteria.Name, keyword);
            }
            if (sorter.HasValue)
            {
                var sort = new Sorter<MarketingSorterCriteria>(sorter.Value, true);
                sorterCriteria.Add(sort);
            }

            var findOrderDiscountMarketings = ServiceFactory.MarketingService.FindOrderDiscountMarketings(page, pageSize, searchCriteria, sorterCriteria);
            return View("OrderDiscount/OrderDiscountList", findOrderDiscountMarketings);
        }


        public ActionResult OrderDiscountDelete(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var marketingId = form["marketingid"].ParseTo<int>();
                if (marketingId > 0)
                {
                    ServiceFactory.MarketingService.DeleteOrderDiscountMarketingById(marketingId);
                    hashtable["result"] = ActionJsonResult.Success;
                }
                else
                {
                    hashtable["result"] = ActionJsonResult.Failing;
                    hashtable["msg"] = "ID错误";
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["msg"] = "活动不存在.";
                        break;
                }
            }
            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult OrderDiscountEdit(int id)
        {
            #region 公共信息设置
            //  初始化语种、国家等资料
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();
            ViewBag.CustomerGroupDescs = ServiceFactory.CustomerService.GetCustomerGroupDescsByLanguage(ServiceFactory.ConfigureService.EnglishLangId).ToList();
            ViewBag.Continents = ServiceFactory.ConfigureService.GetAllContinent().ToList();
            ViewBag.Countries = new Dictionary<int, List<Country>>();
            foreach (var continent in ViewBag.Continents)
            {
                ViewBag.Countries[continent.ContinentId] = ServiceFactory.ConfigureService.GetAllCountryByContinent(continent.ContinentId);
            }
            #endregion

            //订单折扣活动信息
            var orderDiscountMarketing = new OrderDiscountMarketing
            {
                MarketingType = MarketingType.OrderDiscount,
                IsExcludeChannels = true,
                IsExcludeClub = true,
                Status = true
            };
            if (id > 0)
            {
                try
                {
                    orderDiscountMarketing = ServiceFactory.MarketingService.GetOrderDiscountMarketingById(id);
                    orderDiscountMarketing.OrderAmountDiscounts.ForEach(x => x.Discount = (1M - x.Discount) * 100.0M);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_MARKETING_NOT_EXIST":
                            ViewBag.ErrorMsg = "活动不存在！";
                            break;
                    }
                }
            }
            ViewBag.OrderDiscountMarketing = orderDiscountMarketing;

            //ViewBag.OrderDiscounts = ServiceFactory.OrderDiscountService.FindAllOrderDiscounts(1,2,new Dictionary<OrderDiscountSearchCriteria, object>(), new List<Sorter<OrderDiscountSorterCriteria>>() );
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("订单折扣活动", Url.Content("/Marketing/OrderDiscount")));

            return View("OrderDiscount/OrderDiscountEdit");
        }

        public JsonResult OrderDiscountSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var marketingId = form["marketingId"].ParseTo(0);
                var customerType = form["FD_CustomerType"].ParseTo<int>().ToEnum<MarketingCustomerType>();
                var isExcludeClub = form["FD_IsExcludeClub"].ParseTo(0);
                var isExcludeChannels = form["FD_IsExcludeChannels"].ParseTo(0);
                var vipIds = form["FD_CustomerVipIds"];
                var discounts = form["discounts"];
                var languages = form["FD_LanguageIds"];
                var countryIds = form["FD_CountryIds"];
                var amountType = form["FD_AmountType"].ParseTo(0);
                var marketingName = form["FD_Name"];
                var begindate = form["FD_EffectiveBegin_Date"];
                var begintime = form["FD_EffectiveBegin_Time"];
                var enddate = form["FD_EffectiveEnd_Date"];
                var endtime = form["FD_EffectiveEnd_Time"];
                var status = form["FD_Status"].ParseTo(0);
                if (marketingName.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "活动名称不能为空！";
                    return Json(hashtable);
                }

                if (begindate.IsNullOrEmpty() || enddate.IsEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "有效期开始日期不能为空！";
                    return Json(hashtable);
                }
                var orderDiscountMarketing = new OrderDiscountMarketing
                {
                    Id = marketingId,
                    MarketingType = MarketingType.OrderDiscount,
                    CustomerType = customerType,
                    IsExcludeClub = isExcludeClub > 0,
                    IsExcludeChannels = isExcludeChannels > 0,
                    Name = marketingName,
                    AmountType = amountType.ParseTo<MarketingAmountType>(),
                    CountryIds = new List<int>(),
                    LanguageIds = new List<int>(),
                    CustomerVipIds = new List<int>(),//vip等级 
                    ClubLevels = new List<int>(),//去掉
                    CurrencyIds = new List<int>(),//去掉
                    CustomerInfo = new Dictionary<int, string>(),//导入客户
                    Status = status > 0,
                    EffectiveBegin = Convert.ToDateTime(begindate + " " + begintime),
                    EffectiveEnd = Convert.ToDateTime(enddate + " " + endtime)
                };

                #region idList

                switch (customerType)
                {
                    case MarketingCustomerType.ImportCustomer://导入客户
                        {
                            var file = Request.Files["file_importcustomers"];
                            if (file != null)
                            {
                                var filePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportCustomer"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName));
                                file.SaveAs(filePath);
                                orderDiscountMarketing.CustomerInfo = ExcelReadToDictionary(filePath);
                                if (System.IO.File.Exists(filePath))
                                    System.IO.File.Delete(filePath);
                            }
                        } break;
                    case MarketingCustomerType.VipCustomer://vip等级
                        if (!vipIds.IsNullOrEmpty())
                        {
                            var viplist = vipIds.Split<int>(",");
                            foreach (var v in viplist.Where(v => v > 0))
                            {
                                orderDiscountMarketing.CustomerVipIds.Add(v);
                            }
                        }
                        break;
                }

                //language ids
                if (!languages.IsNullOrEmpty())
                {
                    var languagelist = languages.Split<int>(",");
                    foreach (var l in languagelist.Where(l => l > 0))
                    {
                        orderDiscountMarketing.LanguageIds.Add(l);
                    }
                }

                //国家Ids
                if (!countryIds.IsNullOrEmpty())
                {
                    var countrylist = countryIds.Split<int>(",");
                    foreach (var countryId in countrylist.Where(countryId => countryId > 0))
                    {
                        orderDiscountMarketing.CountryIds.Add(countryId);
                    }
                }

                #endregion

                var orderAmountDiscounts = new List<OrderAmountDiscount>();

                if (discounts.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "discounts data is null";
                    return Json(hashtable);
                }
                else
                {
                    orderAmountDiscounts.AddRange(from coupon in discounts.Split(";")
                                                  where !coupon.IsNullOrEmpty() && coupon.Contains('|')
                                                  select coupon.Split('|')
                                                      into a
                                                      select new OrderAmountDiscount()
                                                   {
                                                       Amount = a[0].ParseTo<decimal>(),
                                                       Discount = 1M - a[1].ParseTo<decimal>() / 100.0M,
                                                   });
                }
                orderAmountDiscounts = orderAmountDiscounts.OrderByDescending(x => x.Discount).ToList();

                ServiceFactory.MarketingService.SetOrderDiscountMarketing(orderDiscountMarketing, orderAmountDiscounts);
                hashtable["result"] = ActionJsonResult.Success;

            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["msg"] = "活动不存在.";
                        break;
                }
            }
            catch (Exception exp)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

        #endregion

        #region 下单活动
        [HttpGet]
        public ActionResult PlaceOrder()
        {
            return View("PlaceOrder/PlaceOrder");
        }

        [HttpGet]
        public ActionResult PlaceOrderList(string keyword, MarketingSorterCriteria? sorter)
        {
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();

            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<MarketingSearchCriteria, object>
            {
                {MarketingSearchCriteria.MarketingType, MarketingType.PlaceOrder}
            };
            var sorterCriteria = new List<Sorter<MarketingSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(MarketingSearchCriteria.Name, keyword);
            }
            if (sorter.HasValue)
            {
                var sort = new Sorter<MarketingSorterCriteria>(sorter.Value, true);
                sorterCriteria.Add(sort);
            }

            var findPlaceOrderMarketings = ServiceFactory.MarketingService.FindPlaceOrderMarketings(page, pageSize, searchCriteria, sorterCriteria);
            return View("PlaceOrder/PlaceOrderList", findPlaceOrderMarketings);
        }

        public ActionResult PlaceOrderDelete(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };

            try
            {
                var marketingId = form["marketingid"].ParseTo<int>();
                if (marketingId > 0)
                {
                    ServiceFactory.MarketingService.DeletePlaceOrderMarketingById(marketingId);
                    hashtable["result"] = ActionJsonResult.Success;
                }
                else
                {
                    hashtable["result"] = ActionJsonResult.Failing;
                    hashtable["msg"] = "ID错误";
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["msg"] = "活动不存在.";
                        break;
                }
            }

            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult PlaceOrderEdit(int id)
        {
            #region 公共信息设置
            //  初始化语种、国家等资料
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();
            ViewBag.CustomerGroupDescs = ServiceFactory.CustomerService.GetCustomerGroupDescsByLanguage(ServiceFactory.ConfigureService.EnglishLangId).ToList();
            ViewBag.Continents = ServiceFactory.ConfigureService.GetAllContinent().ToList();
            ViewBag.Countries = new Dictionary<int, List<Country>>();
            ViewBag.Currencies = ServiceFactory.ConfigureService.GetAllValidCurrencies();
            foreach (var continent in ViewBag.Continents)
            {
                ViewBag.Countries[continent.ContinentId] = ServiceFactory.ConfigureService.GetAllCountryByContinent(continent.ContinentId);
            }
            #endregion

            //下单活动信息
            var placeOrderMarketing = new PlaceOrderMarketing
            {
                MarketingType = MarketingType.PlaceOrder,
                IsExcludeChannels = true,
                IsExcludeClub = true,
                Status = true,
                PlaceOrderRewardType = MarketingPlaceOrderResultType.Gift
            };
            if (id > 0)
            {
                try
                {
                    placeOrderMarketing = ServiceFactory.MarketingService.GetPlaceOrderMarketingById(id);
                    placeOrderMarketing.PlaceOrderRewardType = placeOrderMarketing.PlaceOrderDetails.Exists(x => !x.CouponCode.IsNullOrEmpty()) ?
                        MarketingPlaceOrderResultType.Coupon : MarketingPlaceOrderResultType.Gift;
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_MARKETING_NOT_EXIST":
                            ViewBag.ErrorMsg = "活动不存在！";
                            break;
                    }
                }
            }
            ViewBag.PlaceOrderMarketing = placeOrderMarketing;

            //ViewBag.OrderDiscounts = ServiceFactory.OrderDiscountService.FindAllOrderDiscounts(1,2,new Dictionary<OrderDiscountSearchCriteria, object>(), new List<Sorter<OrderDiscountSorterCriteria>>() );
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("下单活动", Url.Content("/Marketing/PlaceOrder")));

            return View("PlaceOrder/PlaceOrderEdit");
        }

        public JsonResult PlaceOrderSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var marketingId = form["marketingId"].ParseTo<int>(0);
                var placeOrderType = form["FD_PlaceOrderType"].ParseTo<int>().ToEnum<MarketingPlaceOrderResultType>();
                var customerType = form["FD_CustomerType"].ParseTo<int>().ToEnum<MarketingCustomerType>();
                var isExcludeClub = form["FD_IsExcludeClub"].ParseTo<int>(0);
                var isExcludeChannels = form["FD_IsExcludeChannels"].ParseTo<int>(0);
                var vipIds = form["FD_CustomerVipIds"];
                var gifts = form["gift"];
                var coupons = form["coupon"];
                var languages = form["FD_LanguageIds"];
                var currencies = form["FD_Currencies"];
                var countryIds = form["FD_CountryIds"];
                var amountType = form["FD_AmountType"].ParseTo<int>(0);
                var marketingName = form["FD_Name"];
                var begindate = form["FD_EffectiveBegin_Date"];
                var begintime = form["FD_EffectiveBegin_Time"];
                var enddate = form["FD_EffectiveEnd_Date"];
                var endtime = form["FD_EffectiveEnd_Time"];
                var status = form["FD_Status"].ParseTo<int>(0);
                if (marketingName.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "活动名称不能为空！";
                    return Json(hashtable);
                }

                if (begindate.IsNullOrEmpty() || enddate.IsEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = "有效期开始日期不能为空！";
                    return Json(hashtable);
                }
                var placeordermarketing = new PlaceOrderMarketing()
                {
                    Id = marketingId,
                    MarketingType = MarketingType.PlaceOrder,
                    CustomerType = customerType,
                    IsExcludeClub = isExcludeClub > 0,
                    IsExcludeChannels = isExcludeChannels > 0,
                    Name = marketingName,
                    AmountType = amountType.ParseTo<MarketingAmountType>(),
                    CountryIds = new List<int>(),
                    LanguageIds = new List<int>(),
                    CustomerVipIds = new List<int>(),//vip等级 
                    ClubLevels = new List<int>(),//去掉
                    CurrencyIds = new List<int>(),//去掉
                    CustomerInfo = new Dictionary<int, string>(),//导入客户
                    Status = status > 0,
                    EffectiveBegin = Convert.ToDateTime(begindate + " " + begintime),
                    EffectiveEnd = Convert.ToDateTime(enddate + " " + endtime)
                };

                #region idList

                switch (customerType)
                {
                    case MarketingCustomerType.ImportCustomer://导入客户
                        {
                            var file = Request.Files["file_importcustomers"];
                            if (file != null)
                            {
                                var filePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportCustomer"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName));
                                file.SaveAs(filePath);
                                placeordermarketing.CustomerInfo = ExcelReadToDictionary(filePath);
                                if (System.IO.File.Exists(filePath))
                                    System.IO.File.Delete(filePath);
                            }
                        } break;
                    case MarketingCustomerType.VipCustomer://vip等级
                        if (!vipIds.IsNullOrEmpty())
                        {
                            var viplist = vipIds.Split<int>(",");
                            foreach (var v in viplist.Where(v => v > 0))
                            {
                                placeordermarketing.CustomerVipIds.Add(v);
                            }
                        }
                        break;
                }

                //language ids
                if (!languages.IsNullOrEmpty())
                {
                    var languagelist = languages.Split<int>(",");
                    foreach (var l in languagelist.Where(l => l > 0))
                    {
                        placeordermarketing.LanguageIds.Add(l);
                    }
                }

                if (!currencies.IsNullOrEmpty())
                {
                    var currencylist = currencies.Split<int>(",");
                    foreach (var l in currencylist.Where(l => l > 0))
                    {
                        placeordermarketing.CurrencyIds.Add(l);
                    }
                }

                //国家Ids
                if (!countryIds.IsNullOrEmpty())
                {
                    var countrylist = countryIds.Split<int>(",");
                    foreach (var countryId in countrylist.Where(countryId => countryId > 0))
                    {
                        placeordermarketing.CountryIds.Add(countryId);
                    }
                }

                #endregion

                var placeOrderDetails = new List<PlaceOrderDetail>();
                switch (placeOrderType)
                {
                    case MarketingPlaceOrderResultType.Gift:
                        {
                            if (gifts.IsNullOrEmpty())
                            {
                                hashtable["result"] = ActionJsonResult.Error;
                                hashtable["msg"] = "gift data is null";
                                return Json(hashtable);
                            }
                            else
                            {
                                placeOrderDetails.AddRange(from gift in gifts.Split(';')
                                                           where !gift.IsNullOrEmpty() && gift.Contains('|')
                                                           select gift.Split('|')
                                                               into a
                                                               select new PlaceOrderDetail()
                                                               {
                                                                   Amount = a[0].ParseTo<decimal>(),
                                                                   GiftLevel = a[1],
                                                               });
                            }
                        }
                        break;
                    case MarketingPlaceOrderResultType.Coupon:
                        {
                            if (coupons.IsNullOrEmpty())
                            {
                                hashtable["result"] = ActionJsonResult.Error;
                                hashtable["msg"] = "coupon data is null";
                                return Json(hashtable);
                            }
                            else
                            {
                                placeOrderDetails.AddRange(from coupon in coupons.Split(";")
                                                           where !coupon.IsNullOrEmpty() && coupon.Contains('|')
                                                           select coupon.Split('|')
                                                               into a
                                                               select new PlaceOrderDetail()
                                                               {
                                                                   Amount = a[0].ParseTo<decimal>(),
                                                                   CouponCode = a[1],
                                                               });
                            }
                        }
                        break;
                }

                ServiceFactory.MarketingService.SetPlaceOrderMarketing(placeordermarketing, placeOrderDetails);
                hashtable["result"] = ActionJsonResult.Success;

            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_MARKETING_NOT_EXIST":
                        hashtable["msg"] = "活动不存在.";
                        break;
                }
            }
            catch (Exception exp)
            {
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

        #endregion

        #region 送礼活动
        [HttpGet]
        public ActionResult Gift()
        {
            return View("Gift/Gift");
        }

        [HttpGet]
        public ActionResult GiftList(MarketingSorterCriteria? sorter, string keyword, int? rewardType, int page = 1, int pageSize = 20)
        {
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList();

            var searchCriteria = new Dictionary<MarketingSearchCriteria, object>
            {
                {MarketingSearchCriteria.MarketingType, MarketingType.Gift}
            };

            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(MarketingSearchCriteria.Name, keyword);
            }
            if (rewardType.HasValue && rewardType.Value != 0)
            {
                searchCriteria.Add(MarketingSearchCriteria.RewardType, rewardType.Value);
            }

            var sorterCriteria = new List<Sorter<MarketingSorterCriteria>>();
            if (sorter.HasValue)
            {
                var sort = new Sorter<MarketingSorterCriteria>(sorter.Value, true);
                sorterCriteria.Add(sort);
            }

            var giftMarketing = ServiceFactory.MarketingService.FindGiftMarketings(page, pageSize, searchCriteria, sorterCriteria);
            return View("Gift/GiftList", giftMarketing);
        }

        [HttpGet]
        public ActionResult GiftEdit(int id = 0, int page = 1)
        {
            //场景
            var itemsMarketingType = new List<SelectListItem>
            {
                new SelectListItem{Text = "请选择",Value=(-1).ToString(CultureInfo.InvariantCulture)},
                new SelectListItem{Text = "注册",Value=((int)GiftMarketingRewardType.Register).ToString(CultureInfo.InvariantCulture)}
            };
            ViewBag.GiftMarketingRewardType = new SelectList(itemsMarketingType, "Value", "Text");
            //语种
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();

            int giftMarketingId = id;
            GiftMarketing giftMarketing = new GiftMarketing();
            if (id != 0)
                giftMarketing = ServiceFactory.MarketingService.GetGiftMarketingById(giftMarketingId);
            ViewBag.Page = page;
            return View("Gift/GiftEdit", giftMarketing);
        }

        [HttpPost]
        public ActionResult GiftEdit(GiftMarketing giftMarketing, string level, string limitBeginDate, string limitBeginTime, string limitEndDate, string limitEndTime, int status)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty }, { "url", "/Marketing/Gift" } };

            DateTime beginTime = Convert.ToDateTime(limitBeginDate + " " + limitBeginTime);
            DateTime endTime = Convert.ToDateTime(limitEndDate + " " + limitEndTime);

            #region 表单验证

            if (giftMarketing.Name.Length <= 2)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "活动名称必须不少于2个字符数";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }
            if (level == "-1")
            {
                hashtable["error"] = true;
                hashtable["msg"] = "请选择具体的送礼等级";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }
            if (limitBeginDate.IsNullOrEmpty())
            {
                hashtable["error"] = true;
                hashtable["msg"] = "请选择开始时间";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }
            if (limitEndDate.IsNullOrEmpty())
            {
                hashtable["error"] = true;
                hashtable["msg"] = "请选择结束时间";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }
            if (beginTime <= DateTime.Now)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "开始时间不能小于当前时间";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }
            if (endTime <= DateTime.Now)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "结束时间不能小于当前时间";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }
            if (beginTime > endTime)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "结束时间不能小于开始时间，请修改";
                hashtable["url"] = string.Empty;
                return Json(hashtable);
            }

            #endregion
            giftMarketing.GiftLevel = level;
            giftMarketing.EffectiveBegin = beginTime;
            giftMarketing.EffectiveEnd = endTime;
            giftMarketing.Status = status == 1;
            giftMarketing.CountryIds = new List<int>();
            giftMarketing.LanguageIds = new List<int>();
            giftMarketing.CustomerVipIds = new List<int>();//vip等级 
            giftMarketing.ClubLevels = new List<int>();//去掉
            giftMarketing.CurrencyIds = new List<int>();//去掉
            giftMarketing.CustomerInfo = new Dictionary<int, string>();//导入客户
            giftMarketing.IsExcludeClub = false;
            giftMarketing.IsExcludeChannels = false;
            var language = Request["language"];
            if (!language.IsNullOrEmpty())
            {
                giftMarketing.LanguageIds = language.Split(',').Select(m => Convert.ToInt32(m)).ToList();
            }
            ServiceFactory.MarketingService.UpdateGiftMarketing(giftMarketing);
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult UpdateGiftMarketingStatus(int id)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty } };
            try
            {
                var giftmarketing = ServiceFactory.MarketingService.GetGiftMarketingById(id);
                ServiceFactory.MarketingService.UpdateGiftMarketingStatus(id, !giftmarketing.Status);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
            }
            return Json(hashtable);
        }

        public ActionResult DeleteGiftMarketing(FormCollection form)
        {
            var hashtable = new Hashtable
            {
                { "error", false },
                { "msg", string.Empty } ,
                {"getlist", true},
                {"url", "/Marketing/GiftList"}
            };
            try
            {
                var ids = form["ids"];
                foreach (var id in ids.Split(','))
                {
                    ServiceFactory.MarketingService.DeleteGiftMarketingById(Convert.ToInt32(id));
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                hashtable["getlist"] = false;
            }
            return Json(hashtable);
        }
        #endregion

        #region SearchKeyword

        [HttpGet]
        public ActionResult SearchKeyword()
        {
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View("SearchKeyword/SearchKeyword");
        }

        [HttpGet]
        public ActionResult SearchKeywordInBoxList(int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<SearchKeywordCriteria, object>
            {
                {SearchKeywordCriteria.KeywordType, KeywordType.InBox}
            };
            var searchkeywords = ServiceFactory.ConfigureService.FindSearchKeyword(page, pageSize, searchCriteria, null);

            return View("SearchKeyword/SearchKeywordInBoxList", searchkeywords);
        }

        [HttpGet]
        public ActionResult SearchKeywordUnderBoxList(int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<SearchKeywordCriteria, object>
            {
                {SearchKeywordCriteria.KeywordType, KeywordType.UnderBox}
            };
            var searchkeywords = ServiceFactory.ConfigureService.FindSearchKeyword(page, pageSize, searchCriteria, null);
            return View("SearchKeyword/SearchKeywordUnderBoxList", searchkeywords);
        }

        [HttpGet]
        public ActionResult SearchKeywordInBoxEdit(int id)
        {
            var searchKeyword = ServiceFactory.ConfigureService.GetSearchKeyword(id);
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View("SearchKeyword/SearchKeywordInBoxEdit", searchKeyword);
        }

        [HttpPost]
        public ActionResult SearchKeywordInBoxEdit(string keywordname, int language, string link = "", int id = 0)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty } };
            SearchKeyword keyword = null;
            if (id == 0)
            {
                keyword = new SearchKeyword
                {
                    KeywordType = KeywordType.InBox,
                    KeywordName = keywordname,
                    KeywordUrl = link,
                    LanguageId = language,
                };
            }
            else
            {
                keyword = new SearchKeyword
                {
                    Id = id,
                    KeywordType = KeywordType.InBox,
                    KeywordName = keywordname,
                    KeywordUrl = link,
                    LanguageId = language,
                };
            }
            ServiceFactory.ConfigureService.SetSearchKeyword(keyword);
            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult SearchKeywordUnderBoxEdit(int id)
        {
            var searchKeyword = ServiceFactory.ConfigureService.GetSearchKeyword(id);
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View("SearchKeyword/SearchKeywordUnderBoxEdit", searchKeyword);
        }

        [HttpPost]
        public ActionResult SearchKeywordUnderBoxEdit(string keywordname, int language, string link = "", int id = 0)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty } };
            SearchKeyword keyword = null;
            if (id == 0)
            {
                keyword = new SearchKeyword
                {
                    KeywordType = KeywordType.UnderBox,
                    KeywordName = keywordname,
                    KeywordUrl = link,
                    LanguageId = language,
                };
            }
            else
            {
                keyword = new SearchKeyword
                {
                    Id = id,
                    KeywordType = KeywordType.UnderBox,
                    KeywordName = keywordname,
                    KeywordUrl = link,
                    LanguageId = language,
                };
            }
            ServiceFactory.ConfigureService.SetSearchKeyword(keyword);
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult DeleteSearchKeyword(int id)
        {
            var hashtable = new Hashtable
            {
                { "error", false },
                { "msg", string.Empty }
            };
            try
            {
                ServiceFactory.ConfigureService.DeleteSeachKeywordById(id);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                hashtable["msg"] = string.Empty;
            }
            return Json(hashtable);
        }
        #endregion
    }
}
