using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Web.Common;
using Microsoft.SqlServer.Server;

namespace Com.Panduo.Web.Controllers
{
    public class CouponController : Controller
    {
        #region Coupon设置
        [HttpGet]
        public ActionResult CouponSet()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CouponSetList(int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<CouponSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<CouponSorterCriteria>>();
            ViewBag.Page = page;
            var coupons = ServiceFactory.CouponService.FindAllCoupon(page, pageSize, searchCriteria, sorterCriteria);
            return View(coupons);
        }

        [HttpGet]
        public ActionResult CouponEdit(int id = 0, int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Id = id;
            ViewBag.AllCurrency = new SelectList(ServiceFactory.ConfigureService.GetAllCurrencies().Select(m => new SelectListItem() { Text = m.CurrencyCode, Value = m.CurrencyId.ToString() }), "Value", "Text"); ;
            var items = new List<SelectListItem>
            {
                new SelectListItem{Text = "物品总金额",Value=((int)AmountType.TotalAmount).ToString()},
                new SelectListItem{Text = "正价商品总额",Value=((int)AmountType.NormalAmount).ToString()}
            };
            ViewBag.CouponAmountType = new SelectList(items, "Value", "Text");
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.AllContinent = ServiceFactory.ConfigureService.GetAllContinent();
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("Coupon", Url.Content("/Coupon/CouponSet")));

            return View(ServiceFactory.CouponService.GetCoupon(id));
        }

        [HttpGet]
        public ActionResult CouponDetail(int id, int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            return View(ServiceFactory.CouponService.GetCoupon(id));
        }

        [HttpPost]
        public ActionResult CouponEdit(Coupon coupon, string languages, string countrys, string descs, string limitBeginDate, string limitBeginTime,
            string limitEndDate, string limitEndTime, string pickBeginDate, string pickBeginTime, string pickEndDate, string pickEndTime)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            if (coupon.CouponCode.IsNullOrEmpty())
            {
                hashtable["msg"] = "Coupon Code不允许为空，请设置！";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            if (!coupon.LimitCount.HasValue)
            {
                hashtable["msg"] = "领取次数不允许为空，请设置！";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            if (!coupon.Amount.HasValue)
            {
                hashtable["msg"] = "面额不允许为空，请设置！";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            if (!coupon.MinAmount.HasValue)
            {
                hashtable["msg"] = "最低消费金额不允许为空，如果不限制，请填入0！";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            if (languages.Trim().IsEmpty())
            {
                hashtable["msg"] = "请选择针对的语种，如果不限制，请勾选\"所有\"！";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            if (countrys.Trim().IsEmpty())
            {
                hashtable["msg"] = "请选择针对的国家，如果不限制，请勾选\"所有\"！";
                hashtable["error"] = true;
                return Json(hashtable);
            }
            if (coupon.LimitType == LimitType.BeginEnd)
            {
                if (limitBeginDate.IsNullOrEmpty())
                {
                    hashtable["msg"] = "开始时间不允许为空，请设置！";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                DateTime limitBeginDateTime = Convert.ToDateTime(limitBeginDate + " " + limitBeginTime);
                coupon.LimitBeginTime = limitBeginDateTime;
                if (!limitEndDate.IsEmpty())
                {
                    DateTime limitEndDateTime = Convert.ToDateTime(limitEndDate + " " + limitEndTime);
                    coupon.LimitEndTime = limitEndDateTime;
                }
                else
                {
                    coupon.LimitEndTime = null;
                }
                if (coupon.LimitBeginTime < DateTime.Now)
                {
                    hashtable["msg"] = "开始时间不能小于当前时间，请修改！";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (coupon.LimitEndTime.HasValue && coupon.LimitBeginTime.Value > coupon.LimitEndTime.Value)
                {
                    hashtable["msg"] = "结束时间不允许小于开始时间，请修改！";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
            }
            else
            {
                if (!coupon.LimitDay.HasValue)
                {
                    hashtable["msg"] = "请设置具体的使用天数，如果不限制请设置一个比较大的值！";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (pickBeginDate.IsEmpty())
                {
                    hashtable["msg"] = "领取周期开始日期不能为空！";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                else
                {
                    DateTime pickBeginDateTime = Convert.ToDateTime(pickBeginDate + " " + pickBeginTime);
                    coupon.PickBeginTime = pickBeginDateTime;
                }
                if (!pickEndDate.IsEmpty())
                {
                    DateTime pickEndDateTime = Convert.ToDateTime(pickEndDate + " " + pickEndTime);
                    coupon.PickEndTime = pickEndDateTime;
                }
                if (coupon.PickBeginTime.HasValue && coupon.PickEndTime.HasValue && coupon.PickBeginTime.Value > coupon.PickEndTime.Value)
                {
                    hashtable["msg"] = "领取周期开始日期不能小于结束时间！";
                    hashtable["error"] = true;
                }
            }
            coupon.LanguageIds = languages;
            coupon.CountryIds = countrys;
            coupon.CurrencyIds = "All";

            var descArr = Regex.Split(descs, "{;}");

            var couponDescs = descArr.Select(desc => Regex.Split(desc, "{:}")).Select(descspl => new CouponDesc
            {
                LanguageId = Convert.ToInt32(descspl[0]),
                Description = descspl[1]
            }).ToList();

            if (!(bool)hashtable["error"])
                if (coupon.CouponId == 0)
                {
                    try
                    {
                        coupon.Status = 1;
                        ServiceFactory.CouponService.CreateCoupon(coupon, couponDescs);
                    }
                    catch (BussinessException bussinessException)
                    {
                        switch (bussinessException.Message)
                        {
                            case "ERROR_COUPON_CODE_EXIST":
                                hashtable["msg"] = "当前Coupon Code和已经创建的Coupon Code重复，请修改！";
                                hashtable["error"] = true;
                                break;
                        }
                    }
                }
                else
                {
                    var couponbyid = ServiceFactory.CouponService.GetCoupon(coupon.CouponId);
                    var couponbycode = ServiceFactory.CouponService.GetCoupon(coupon.CouponCode);
                    if (!couponbycode.IsNullOrEmpty() && couponbyid.CouponId != couponbycode.CouponId)
                    {
                        hashtable["msg"] = "Coupon编号已存在！";
                        hashtable["error"] = true;
                    }
                    else
                    {
                        foreach (var couponDesc in couponDescs)
                        {
                            couponDesc.CouponId = coupon.CouponId;
                        }
                        ServiceFactory.CouponService.EditCoupon(coupon, couponDescs);
                    }
                }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult CheckMarketingCoupon(int id)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };

            var coupon = ServiceFactory.CouponService.GetCoupon(id);
            if (ServiceFactory.MarketingService.CheckMarketingCouponByCode(coupon.CouponCode))
            {
                hashtable["msg"] = "当前Coupon绑定了xxx的Coupon活动，不能删除！";
                hashtable["error"] = true;
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult DeleteCoupon(int id)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            ServiceFactory.CouponService.DeleteCoupon(id);
            return Json(hashtable);
        }

        #endregion

        #region CouponCustomer

        [HttpGet]
        public ActionResult CouponCustomer()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CouponCustomerEClose(int id)
        {
            return View(ServiceFactory.CouponService.GetCustomerCouponView(id));
        }

        [HttpGet]
        public ActionResult CouponCustomerEStart(int id)
        {
            return View(ServiceFactory.CouponService.GetCustomerCouponView(id));
        }

        [HttpGet]
        public ActionResult CouponCustomerEUse(int id)
        {
            return View(ServiceFactory.CouponService.GetCustomerCouponView(id));
        }

        [HttpPost]
        public ActionResult CouponCustomerEUse(int id, string usereason)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", "修改成功!" }, { "getlist", true } };

            if (usereason.Trim() == "")
            {
                hashtable["error"] = true;
                hashtable["msg"] = "修改失败，使用原因不能为空";
            }
            else
                ServiceFactory.CouponService.UseCustomerCoupon(id, SessionHelper.CurrentAdminUser.AdminId, usereason);

            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult CouponCustomerEClose(int id, string closereason)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", "修改成功!" }, { "getlist", true } };
            if (closereason.Trim() == "")
            {
                hashtable["error"] = true;
                hashtable["msg"] = "修改失败，关闭原因不能为空";
            }
            else
                ServiceFactory.CouponService.CloseCustomerCoupon(id, SessionHelper.CurrentAdminUser.AdminId, closereason);

            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult CouponCustomerEStart(int id, string endDate, string endTime)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", "修改成功!" }, { "getlist", true } };
            if (endDate.Trim() == "")
            {
                hashtable["error"] = true;
                hashtable["msg"] = "结束时间不能为空";
            }
            else
            {
                try
                {
                    DateTime endDateTime = Convert.ToDateTime(endDate + " " + endTime);
                    if (endDateTime <= DateTime.Now)
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = "结束时间不能小于当前时间";
                    }
                    else
                    {
                        ServiceFactory.CouponService.StartCustomerCoupon(id, SessionHelper.CurrentAdminUser.AdminId, endDateTime);
                    }
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_COUPONCUSTOMER_NOT_EXIST":
                            hashtable["error"] = true;
                            hashtable["msg"] = "修改失败，客户优惠券不存在";
                            break;
                        case "ERROR_BEGINTIME_GREATER_ENDTIME":
                            hashtable["error"] = true;
                            hashtable["msg"] = "修改失败，客户优惠券开始时间大于结束时间";
                            break;
                    }
                }
            }
            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult CouponCustomerList(int? status, string couponName, string emailId, string orderCode, int? leftDay)
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<CustomerCouponSearchCriteria, object>();
            if (status != null)
            {
                if (status != 0)
                {
                    if (status == (int)CouponStatus.PassDue)
                    {
                        searchCriteria.Add(CustomerCouponSearchCriteria.PassDue, DateTime.Now);
                    }
                    else
                    {
                        searchCriteria.Add(CustomerCouponSearchCriteria.Status, status);
                    }
                }

            }
            if (couponName != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.CouponName, couponName);
            }
            if (emailId != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.EmailId, emailId);
            }
            if (orderCode != null)
            {
                searchCriteria.Add(CustomerCouponSearchCriteria.OrderCode, orderCode);
            }
            var sorterCriteria = new List<Sorter<CustomerCouponSorterCriteria>>();
            if (leftDay != null)
            {
                sorterCriteria.Add(new Sorter<CustomerCouponSorterCriteria>(CustomerCouponSorterCriteria.LeftDay, leftDay == 1));
            }
            var couponCustomerView = ServiceFactory.CouponService.FindAllCustomerCouponView(page, pageSize, searchCriteria, sorterCriteria);
            return View(couponCustomerView);
        }

        public ActionResult SendCoupon(string customerEmail, string coupunCode)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", "发送成功!" } };
            try
            {
                ServiceFactory.CouponService.SendCoupon(coupunCode, customerEmail, SessionHelper.CurrentAdminUser.AdminId);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_COUPON_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "优惠券不存在";
                        break;
                    case "ERROR_CUSTOMER_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "客户不存在";
                        break;
                    case "ERROR_COUPON_PASS_DUE":
                        hashtable["error"] = true;
                        hashtable["msg"] = "当前Coupon Code已经过期，请更换为活动状态的Coupon Code";
                        break;
                    case "ERROR_COUPON_GT":
                        hashtable["error"] = true;
                        hashtable["msg"] = "该客户coupon领取超过设置次数";
                        break;
                }
            }
            return Json(hashtable);
        }
        #endregion
    }
}
