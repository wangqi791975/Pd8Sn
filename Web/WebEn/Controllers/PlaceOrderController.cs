using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using Com.Panduo.Common;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Coupon;
using Com.Panduo.Web.Models.Payment;
using Com.Panduo.Web.Models.ShoppingCart;

namespace Com.Panduo.Web.Controllers
{
    public class PlaceOrderController : BaseController
    {

        /// <summary>
        /// 当前客户的ShoppingCartId
        /// </summary>
        private int ShoppingCartId { get { return SessionHelper.ShoppingCartId; } }
        /// <summary>
        /// 当前语种
        /// </summary>
        private int LanguageId { get { return ServiceFactory.ConfigureService.SiteLanguageId; } }

        [HttpPost]
        public JsonResult CheckShoppingCart(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "checkouturl", UrlRewriteHelper.GetCheckoutUrl() }, { "msg", string.Empty } };
            try
            {
                var countryId = form["countryId"].ParseTo<int>();
                var posatlCode = form["posatlCode"];

                var isDisabled = ServiceFactory.ShoppingCartService.ValidateShoppingCartItem(ShoppingCartId);//如果又存在新的下架物品
                if (isDisabled)
                {
                    ServiceFactory.ShoppingCartService.UpdateShoppingCartUnAvailableProductStatus(ShoppingCartId);//先将remove_checked改为1
                }
                Country country = null;
                var shippingAddress = GetDefaultShippingAddress(out country, countryId, posatlCode);
                var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
                    PageHelper.GetCurrentLanguage().LanguageId, PageHelper.GetCurrentCurrency().CurrencyId, country.CountryId);

                var shipppingCriteria = new ShipppingCriteria
                {
                    CountryIsoCode2 = country.SimpleCode2,
                    City = shippingAddress.Province,
                    PostCode = shippingAddress.ZipCode,
                    GrossWeight = shoppingCartSummary.GrossWeight / 1000,
                    VolumeWeight = shoppingCartSummary.VolumeWeight / 1000,
                    ClubLevel = SessionHelper.CurrentCustomer.IsNullOrEmpty() ? 0 : SessionHelper.CurrentCustomer.ClubLevel,
                    TotalAmount = shoppingCartSummary.OriginalProductAmount
                };

                var shippingAmount = ServiceFactory.ShippingService.GetShippingAmount(CookieHelper.CustomerShippingId, shipppingCriteria);
                shoppingCartSummary.GrandTotal += shippingAmount.HandlingFeeForClub + shippingAmount.TotalShippingCost + shippingAmount.HandlingFeeForFreeShipping;

                if (shoppingCartSummary.GrandTotal > 0)
                {
                    hashtable["result"] = ActionJsonResult.Success;
                    hashtable["totalAmount"] = shoppingCartSummary.GrandTotal;
                    hashtable["shippingAmount"] = shippingAmount.TotalShippingCost;
                    hashtable["productCount"] = shoppingCartSummary.TotalQuantity;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = ex.Message;
            }
            return Json(hashtable);//, JsonRequestBehavior.AllowGet
        }

        [OutputCache(Duration = 0)]
        public ActionResult CheckOut()
        {
            int addressId = Request["address_id"].ParseTo(0);//地址ID
            int shippingId = Request["shipping_id"].ParseTo(0);//配送方式ID
            int modify = Request["modify"].ParseTo(0);//是否是修改地址状态
            int reportType = Request["report_type"].ParseTo(-1);
            string reportCurrencyCode = Request["report_currency_code"] ?? string.Empty;
            decimal reportProductMoney = Request["report_product_money"].ParseTo(0M);
            decimal reportShippingMoney = Request["report_shipping_money"].ParseTo(0M);

            bool updateAddressCookie = false;
            if (addressId > 0)
            {
                var addressTemp = ServiceFactory.CustomerService.GetAddressById(addressId);
                if (addressTemp != null && addressTemp.CustomerId == ShoppingCartId)
                {
                    CookieHelper.CustomerAddressId = addressId;
                    updateAddressCookie = true;
                }
            }
            if (addressId <= 0 || updateAddressCookie == false)
            {
                addressId = SessionHelper.CurrentCustomer.ShippingAddress.ParseTo(0);
                if (addressId == 0)
                {
                    addressId = CookieHelper.CustomerAddressId;
                }
            }
            if (shippingId <= 0)
            {
                shippingId = CookieHelper.CustomerShippingId;//最后一次下单完成时可以设置该值
            }



            //得到客户的所有地址
            var shippingAddresses = ServiceFactory.CustomerService.GetAddressesByCustomerId(ShoppingCartId);

            //得到客户当前的收货地址
            IList<Address> addresses = shippingAddresses.Where(x => x.AddressId == addressId).ToList();
            Address address = null;
            Country country = null;
            if (addresses.IsNullOrEmpty())
            {
                country = ServiceFactory.ConfigureService.GetCountryByIp(PageManager.GetClientIp()); ;
            }
            else
            {
                address = addresses[0];
                country = ServiceFactory.ConfigureService.GetCountryById(address.Country);
            }
            if (country == null)
                country = ServiceFactory.ConfigureService.GetCountryByIp(PageManager.GetClientIp());
            if (address == null)
            {
                address = new Address { AddressId = 0, Province = string.Empty, ZipCode = string.Empty };
            }
            CookieHelper.CustomerCountryId = country.CountryId;//记住配送方式ID

            //验证购物车数据
            /*
            var form = new FormCollection();
            form.Add("countryId", country.CountryId.ToString());
            form.Add("posatlCode", "");
            var validate = CheckShoppingCart(form);
            var table = validate.ToString().FromJson<IDictionary<string, string>>();
            if (table["result"].Equals(ActionJsonResult.Failing))
            {
                return Content("ERROR_CART_IS_EMPTY");
            }
            */

            //得到所有国家
            var countryAll = ServiceFactory.ConfigureService.GetAllCountryLanguages();
            var countryLanguages = countryAll.Where(x => x.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).ToList();
            var commonCountries = ServiceFactory.ConfigureService.GetCommonCountry();
            var commonCountryLanguages = commonCountries.Select(x => new CountryLanguage { CountryId = x.CountryId, CountryName = countryLanguages.FirstOrDefault(w => w.CountryId == x.CountryId && w.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).CountryName }).ToList();
            var countryCurrents = countryLanguages.Where(x => x.CountryId == country.CountryId).ToList();
            var countryCurrent = countryCurrents[0];

            ShoppingAddress shoppingAddress = new ShoppingAddress
            {
                CountryLanguages = countryLanguages,
                CommonCountryLanguages = commonCountryLanguages,
                Address = new Address(),
                CountryLanguage = countryCurrent,
                IsCheckedAddress = string.Empty,
                IsDisabledAddress = string.Empty
            };



            var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
                PageHelper.GetCurrentLanguage().LanguageId, PageHelper.GetCurrentCurrency().CurrencyId, country.CountryId);


            ShipppingCriteria shipppingCriteria = new ShipppingCriteria();
            shipppingCriteria.CountryIsoCode2 = country.SimpleCode2;
            shipppingCriteria.City = address.Province;
            shipppingCriteria.PostCode = address.ZipCode;
            shipppingCriteria.GrossWeight = shoppingCartSummary.GrossWeight / 1000;
            shipppingCriteria.VolumeWeight = shoppingCartSummary.VolumeWeight / 1000;
            shipppingCriteria.ClubLevel = SessionHelper.CurrentCustomer.ClubLevel;
            shipppingCriteria.TotalAmount = shoppingCartSummary.OriginalProductAmount;
            var shippingAmounts = ServiceFactory.ShippingService.GetShippingAmounts(shipppingCriteria);
            var hasShippingAmounts = shippingAmounts.Where(x => x.ShippingId == shippingId).ToList();//该配送方式ID是否在满足条件的配送方式列表中
            if (shippingAmounts.Count > 0 && (shippingId == 0 || hasShippingAmounts.Count == 0))
            {
                var shippingAmount = shippingAmounts.Where(x => x.IsDefault == true).ToList();
                if (shippingAmount.Count > 0)
                {
                    shippingId = shippingAmount[0].ShippingId;
                }
            }
            CookieHelper.CustomerShippingId = shippingId;//记住配送方式ID

            var shippingLanguages = ServiceFactory.ShippingService.GetAllShippingDescs(ServiceFactory.ConfigureService.SiteLanguageId);

            var currencies = ServiceFactory.ConfigureService.GetAllValidCurrencies();

            var paypalInfo = SessionHelper.PaypalExpressPayInfo;//paypal快速付款信息

            CustomsNo customsNo = ServiceFactory.ShippingService.GetCustomsNo(shippingId, country.CountryId);

            ViewBag.AddressId = addressId;
            ViewBag.Modify = modify;
            ViewBag.Country = country;
            ViewBag.ShippingId = shippingId;
            ViewBag.ShippingAddresses = shippingAddresses;
            ViewBag.ShoppingCartSummary = shoppingCartSummary;
            ViewBag.ShippingAmounts = shippingAmounts;
            ViewBag.ShippingLanguages = shippingLanguages;
            ViewBag.Currencies = currencies;
            ViewBag.ReportType = reportType;
            ViewBag.ReportCurrencyCode = reportCurrencyCode;
            ViewBag.ReportProductMoney = reportProductMoney;
            ViewBag.ReportShippingMoney = reportShippingMoney;
            ViewBag.CustomsNo = customsNo;
            ViewBag.PaypalInfo = paypalInfo;

            return View(shoppingAddress);
        }

        public ActionResult AddressInfo(int id)
        {
            //得到所有国家
            var countryAll = ServiceFactory.ConfigureService.GetAllCountryLanguages();
            var countryLanguages = countryAll.Where(x => x.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).ToList();
            var commonCountries = ServiceFactory.ConfigureService.GetCommonCountry();
            //var commonCountryLanguages = commonCountries.Select(x => new CountryLanguage { CountryId = x.CountryId, CountryName = countryLanguages.FirstOrDefault(w => w.CountryId == x.CountryId).CountryName }).ToList();
            var commonCountryLanguages = commonCountries.Select(x => new CountryLanguage { CountryId = x.CountryId, CountryName = countryLanguages.FirstOrDefault(w => w.CountryId == x.CountryId && w.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).CountryName }).ToList();
            Country country = null;
            var countryLanguage = new CountryLanguage();
            if (id <= 0)
            {
                var shippingAddress = GetDefaultShippingAddress(out country, 0, "");
                var countryCurrents = countryLanguages.Where(x => x.CountryId == country.CountryId).ToList();
                countryLanguage = countryCurrents[0];
            }

            Address address = new Address();
            var isCheckedAddress = string.Empty;
            var isDisabledAddress = string.Empty;
            if (id > 0)
            {
                address = ServiceFactory.CustomerService.GetAddressById(id);
                countryLanguage = ServiceFactory.ConfigureService.GetCountryLanguage(address.Country, ServiceFactory.ConfigureService.SiteLanguageId);

                //该地址是否与默认地址一致
                if (id == SessionHelper.CurrentCustomer.ShippingAddress)
                {
                    isCheckedAddress = "checked=checked";
                }

                //查询客户有多少个地址，如果只有一个，那么(Set as Default Shipping Address)选中并不能操作
                var addressList = ServiceFactory.CustomerService.GetAddressesByCustomerId(ShoppingCartId);
                if (addressList.Count <= 1 || id == SessionHelper.CurrentCustomer.ShippingAddress)
                {
                    isDisabledAddress = "disabled=disabled";
                }


            }
            if (address == null)
            {
                address = new Address();
            }
            ShoppingAddress shoppingAddress = new ShoppingAddress
            {
                CountryLanguages = countryLanguages,
                CommonCountryLanguages = commonCountryLanguages,
                Address = address,
                CountryLanguage = countryLanguage,
                IsCheckedAddress = isCheckedAddress,
                IsDisabledAddress = isDisabledAddress
            };

            return View("Partial/Address", shoppingAddress);
        }



        [OutputCache(Duration = 0)]
        public JsonResult AddAddress(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                int addressId = form["address_id"].ParseTo(-1);
                string firstName = form["first_name"].Trim() ?? string.Empty;
                string lastName = form["last_name"].Trim() ?? string.Empty;
                string streetAddress = form["street_address"].Trim() ?? string.Empty;
                string streetAddress2 = form["street_address2"].Trim() ?? string.Empty;
                int countryId = form["country_id"].ParseTo(-1);
                int provinceId = form["province_id"].ParseTo(0);
                string provinceName = form["province_name"].Trim() ?? string.Empty;
                string city = form["city"].Trim() ?? string.Empty;
                string zipCode = form["zip_code"].Trim() ?? string.Empty;
                string phoneNumber = form["phone_number"].Trim() ?? string.Empty;
                bool isDefaultShippingAddress = form["is_default_shipping_address"].ParseTo(false);

                if (addressId < 0 || countryId < 0 || firstName.Equals("") || firstName.Length < 2 || firstName.Length > 50 || lastName.Equals("") || lastName.Length < 2 || lastName.Length > 50 || streetAddress.Equals("") || streetAddress.Length < 5 || streetAddress.Length > 200 || countryId == 0 || provinceName.Equals("") || provinceName.Length < 3 || provinceName.Length > 100 || city.Equals("") || city.Length < 3 || city.Length > 100 || zipCode.Equals("") || zipCode.Length < 3 || zipCode.Length > 30 || phoneNumber.Equals("") || phoneNumber.Length < 3 || phoneNumber.Length > 50)
                {
                    hashtable["msg"] = "Message:数据不合法!";
                    return Json(hashtable);
                }

                string fullName = string.Format("{0} {1}", firstName, lastName);
                if (ServiceFactory.ConfigureService.SiteLanguageId == 6)
                {
                    fullName = string.Format("{0} {1}", lastName, firstName);
                }

                Address address = new Address();
                address.AddressId = addressId;
                address.CustomerId = ShoppingCartId;
                address.FirstName = firstName;
                address.LastName = lastName;
                address.FullName = fullName; ;
                address.CompanyName = string.Empty;
                address.Street1 = streetAddress;
                address.Street2 = streetAddress2;
                address.Country = countryId;
                address.ProvinceId = provinceId;
                address.Province = provinceName;
                address.City = city;
                address.ZipCode = zipCode;
                address.Telphone = phoneNumber;

                int result = 0;
                if (addressId > 0)
                {
                    ServiceFactory.CustomerService.UpdateAddress(ShoppingCartId, address);
                    result = address.AddressId;
                }
                else
                {
                    result = ServiceFactory.CustomerService.AddAddress(address, isDefaultShippingAddress, false);

                }
                if (isDefaultShippingAddress)
                {
                    SessionHelper.CurrentCustomer.ShippingAddress = result;
                }
                hashtable["result"] = ActionJsonResult.Success;
                hashtable["msg"] = result;
            }
            catch (BussinessException ex)
            {
                //记录日志
                hashtable["result"] = ex.GetError();
                hashtable["msg"] = ex.Message;
            }
            return Json(hashtable);
        }

        [OutputCache(Duration = 0)]
        public JsonResult GetProvinces(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                int countryId = form["country_id"].ParseTo(0);
                var provinces = ServiceFactory.ConfigureService.GetAllProvinceByCountryId(countryId);
                var provinceLanguages = ServiceFactory.ConfigureService.GetAllProvinceLanguages();
                if (!provinces.IsNullOrEmpty())
                {
                    hashtable["msg"] = provinces.Select(x => new Province { ProvinceId = x.ProvinceId, ProvinceName = provinceLanguages.FirstOrDefault(w => w.Province == x.ProvinceId && w.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).ProvinceName }).ToList();
                }
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

        [OutputCache(Duration = 0)]
        public JsonResult DelAddress()
        {
            var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                int addressId = Convert.ToInt32(Request["address_id"]);
                ServiceFactory.CustomerService.DeleteAddress(ShoppingCartId, addressId);
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

        public ActionResult ItemsReview()
        {
            int addressId = Request["address_id"].ParseTo(0);
            int shippingId = Request["shipping_id"].ParseTo(0);
            int countryId = Request["country_id"].ParseTo(0);
            int reportType = Request["report_type"].ParseTo(-1);
            string reportCurrencyCode = Request["report_currency_code"] ?? string.Empty;
            decimal reportProductMoney = Request["report_product_money"].ParseTo(0M);
            decimal reportShippingMoney = Request["report_shipping_money"].ParseTo(0M);
            string customsNoNumber = Request["customs_no_number"] ?? string.Empty;
            string orderRemark = Request["order_remark"] ?? string.Empty;

            if (reportType < 0)
            {
                reportCurrencyCode = string.Empty;
                reportProductMoney = 0;
                reportShippingMoney = 0;
            }

            var page = Request[UrlParameterKey.Page].ParseTo(1); //当前页
            var pageSize = 20; //页大小

            if (Request[UrlParameterKey.Page] == null)
            {
                if (reportType < 0)
                {
                    return RedirectToAction("CheckOut", "PlaceOrder");
                }
                if (addressId <= 0)
                {
                    return Content("ERROR_ADDRESS");
                }
            }

            //得到客户的所有地址
            var shippingAddresses = ServiceFactory.CustomerService.GetAddressesByCustomerId(ShoppingCartId);

            //得到客户当前的收货地址
            IList<Address> addresses = shippingAddresses.Where(x => x.AddressId == addressId).ToList();
            var address = new Address();
            if (addresses.Count > 0)
            {
                address = addresses[0];
            }
            else if (Request[UrlParameterKey.Page] == null)
            {
                return Content("ERROR_ADDRESS");
            }

            var sortMode = Request[UrlParameterKey.Sort].ParseTo<int>().ToEnum<ShoppingCartSorterCriteria>();//sort.ToEnum<ShoppingCartSorterCriteria>();
            var sorter = new List<Sorter<ShoppingCartSorterCriteria>>
            {
                new Sorter<ShoppingCartSorterCriteria> {Key = sortMode, IsAsc = true}
            };

            var search = new Dictionary<ShoppingCartSearchCriteria, object>()
            {
                {ShoppingCartSearchCriteria.LanguageId, LanguageId}
            };
            var pageData = ServiceFactory.ShoppingCartService.FindVShoppingCartItems(ShoppingCartId,
                page, pageSize, search, sorter);
            if (Request.IsAjaxRequest())
            {
                return View("Partial/ShoppingCartList", pageData);
            }
            if (pageData.Data.Count <= 0)
            {
                return Redirect(UrlRewriteHelper.GetShoppingCartUrl());
            }

            var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
                PageHelper.GetCurrentLanguage().LanguageId, PageHelper.GetCurrentCurrency().CurrencyId, countryId);

            //满足条件的Coupon
            var couponCustomers = ServiceFactory.CouponService.GetUsableCoupons(ShoppingCartId, new Dictionary<AmountType, decimal> { { AmountType.NormalAmount, shoppingCartSummary.NoDiscountProductAmount }, { AmountType.TotalAmount, shoppingCartSummary.GrandTotal } }, countryId, PageHelper.GetCurrentCurrency().CurrencyId, PageHelper.GetCurrentLanguage().LanguageId);
            var couponOrders = new List<CouponCustomerVo>();
            foreach (var coupon in couponCustomers)
            {
                Currency currency = ServiceFactory.ConfigureService.GetCurrency(coupon.AmountCurrencyId ?? 1);
                string symbolLeft = string.Empty;
                string first = string.Empty;
                if (currency != null)
                {
                    symbolLeft = PageHelper.GetCurrentCurrency().SymbolLeft;
                }
                coupon.Amount = PageHelper.ExchangeMoneyByUsd(PageHelper.ExchangeMoneyToUsd(coupon.Amount ?? 0, currency), PageHelper.GetCurrentCurrency());

                var couponCustomerVo = new CouponCustomerVo
                {
                    CouponCustomer = coupon,
                    SymbolLeft = symbolLeft,
                    CurrentShortDate = PageHelper.ToCurrentShortDate(coupon.LimitEndTime)

                };
                couponOrders.Add(couponCustomerVo);
            }
            //Coupon排序
            couponOrders = couponOrders.OrderByDescending(x => x.CouponCustomer.Amount).ThenBy(x => x.CouponCustomer.LimitEndTime).ToList();


            //获取所有币种信息
            var currencies = ServiceFactory.ConfigureService.GetAllValidCurrencies();

            var country = ServiceFactory.ConfigureService.GetCountryById(countryId);




            ShipppingCriteria shipppingCriteria = new ShipppingCriteria();
            shipppingCriteria.CountryIsoCode2 = country.SimpleCode2;
            shipppingCriteria.City = address.Province;
            shipppingCriteria.PostCode = address.ZipCode;
            shipppingCriteria.GrossWeight = shoppingCartSummary.GrossWeight / 1000;
            shipppingCriteria.VolumeWeight = shoppingCartSummary.VolumeWeight / 1000;
            shipppingCriteria.ClubLevel = SessionHelper.CurrentCustomer.ClubLevel;
            shipppingCriteria.TotalAmount = shoppingCartSummary.OriginalProductAmount;

            var shippingAmount = ServiceFactory.ShippingService.GetShippingAmount(CookieHelper.CustomerShippingId, shipppingCriteria);
            shoppingCartSummary.GrandTotal += shippingAmount.HandlingFeeForClub + shippingAmount.TotalShippingCost + shippingAmount.HandlingFeeForFreeShipping;

            ViewBag.AddressId = addressId;
            ViewBag.ShippingId = shippingId;
            ViewBag.CountryId = countryId;
            ViewBag.ReportType = reportType;
            ViewBag.ReportCurrencyCode = reportCurrencyCode;
            ViewBag.ReportProductMoney = reportProductMoney;
            ViewBag.reportShippingMoney = reportShippingMoney;
            ViewBag.CustomsNoNumber = customsNoNumber;
            ViewBag.OrderRemark = orderRemark;
            ViewBag.PageData = pageData;
            ViewBag.ShoppingCartSummary = shoppingCartSummary;
            ViewBag.ShippingAmount = shippingAmount;
            ViewBag.CouponOrders = couponOrders;
            ViewBag.Currencies = currencies;

            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResult GetUserCoupons()
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", true } };
            int countryId = Request["country_id"].ParseTo(0);
            var country = ServiceFactory.ConfigureService.GetCountryById(countryId);

            var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
                PageHelper.GetCurrentLanguage().LanguageId, PageHelper.GetCurrentCurrency().CurrencyId, country.CountryId);
            //满足条件的Coupon
            var couponCustomers = ServiceFactory.CouponService.GetUsableCoupons(ShoppingCartId, new Dictionary<AmountType, decimal> { { AmountType.NormalAmount, shoppingCartSummary.NoDiscountProductAmount }, { AmountType.TotalAmount, shoppingCartSummary.GrandTotal } }, countryId, PageHelper.GetCurrentCurrency().CurrencyId, PageHelper.GetCurrentLanguage().LanguageId);
            var couponOrders = new List<CouponCustomerVo>();
            foreach (var coupon in couponCustomers)
            {
                Currency currency = ServiceFactory.ConfigureService.GetCurrency(coupon.AmountCurrencyId ?? 1);
                string symbolLeft = string.Empty;
                string first = string.Empty;
                if (currency != null)
                {
                    symbolLeft = PageHelper.GetCurrentCurrency().SymbolLeft;
                }
                coupon.Amount = PageHelper.ExchangeMoneyByUsd(PageHelper.ExchangeMoneyToUsd(coupon.Amount ?? 0, currency), PageHelper.GetCurrentCurrency());

                var couponCustomerVo = new CouponCustomerVo
                {
                    CouponCustomer = coupon,
                    SymbolLeft = symbolLeft,
                    CurrentShortDate = PageHelper.ToCurrentShortDate(coupon.LimitEndTime)

                };
                couponOrders.Add(couponCustomerVo);
            }
            //Coupon排序
            couponOrders = couponOrders.OrderByDescending(x => x.CouponCustomer.Amount).ThenBy(x => x.CouponCustomer.LimitEndTime).ToList();
            if (couponOrders.Count > 0)
            {
                hashtable["msg"] = couponOrders;
                hashtable["error"] = false;
            }
            return Json(hashtable);
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult Submit()
        {
            int addressId = Request["address_id"].ParseTo(0);//地址ID
            int shippingId = Request["shipping_id"].ParseTo(0);//配送方式ID
            int reportType = Request["report_type"].ParseTo(0);
            string reportCurrencyCode = Request["report_currency_code"] ?? string.Empty;
            decimal reportProductMoney = Request["report_product_money"].ParseTo(0M);
            decimal reportShippingMoney = Request["report_shipping_money"].ParseTo(0M);
            string customsNoNumber = Request["customs_no_number"] ?? string.Empty;
            string orderRemark = Request["order_remark"] ?? string.Empty;
            int outOfStockWaitType = Request["out_of_stock_wait_type"].ParseTo(1);
            int couponCustomerId = Request["coupon_customer_id"].ParseTo(0);
            try
            {
                var checkoutDraft = new CheckoutDraft
                {
                    ShoppingCartId = ShoppingCartId,
                    ClubLevel = SessionHelper.CurrentCustomer.ClubLevel,
                    ReceivingAddressId = addressId,
                    BillAddressId = addressId,
                    ShippingId = shippingId,
                    OrderSource = 0,
                    LanguageCode = ServiceFactory.ConfigureService.SiteLanguageCode,
                    CurrencyCode = PageHelper.GetCurrentCurrency().CurrencyCode,
                    ReportType = reportType,
                    ReportCurrencyCode = reportCurrencyCode,
                    ReportProductMoney = reportProductMoney,
                    ReportShippingMoney = reportShippingMoney,
                    CustomsNoType = CustomsNoType.CnpjNo,
                    CustomsNoNumber = customsNoNumber,
                    OrderRemark = orderRemark,
                    OutOfStockWaitType = EnumHelper.ToEnum<OutOfStockWaitType>(outOfStockWaitType),
                    CouponCustomerId = couponCustomerId,
                    OrderIpAddress = PageManager.GetClientIp()
                };

                var orderNumber = ServiceFactory.OrderService.PlaceOrderByCustomer(checkoutDraft);

                SessionHelper.LastOrderNumber = orderNumber;//记录订单号

                #region 下单成功后对于Paypal快速支付的需要执行扣款操作
                if (SessionHelper.PaypalExpressPayInfo != null && !SessionHelper.PaypalExpressPayInfo.Token.IsNullOrEmpty())
                {
                    //客户在购物车点击了check out 并完成了与paypal的通讯的
                    var hasBalance = ServiceFactory.CashService.GetCustomerBalance(SessionHelper.CurrentCustomer.CustomerId) > 0;
                    if (hasBalance)
                    {
                        //客户有余额的需要客户到支付中心去勾选是否使用Cash支付以后点击Pay Now按钮完成扣款
                        return RedirectToAction("Payment", "CheckOut");
                    }
                    else
                    {
                        //客户没有Cash余额的直接支付订单
                        var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNumber);

                        var orderAmount = ServiceFactory.OrderService.GetOrderCostByOrderId‎(order.OrderId);

                        var debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(order.CustomerId);

                        var payAmountUsd = orderAmount.NeedToPayAmt + debtCashUsd;

                        //汇率转换为订单币种对应的金额进行支付
                        var payCurrencyCode = ServiceFactory.PaymentService.IsCurrencyUseUsdForPaypal(order.Currency) ? PageHelper.CURRENCY_CODE_USD : order.Currency;

                        var currency = CacheHelper.GetCurrencyByCode(payCurrencyCode);

                        var paymentAmount = payAmountUsd * (string.Equals(PageHelper.CURRENCY_CODE_USD, payCurrencyCode, StringComparison.InvariantCultureIgnoreCase) ? 1.00M : order.ExchangeRate);//汇率转换为订单币种对应的金额

                        paymentAmount = PageHelper.GetRoundValue(paymentAmount, currency.DecimalPlaces);

                        var paymentParm = new DoPaypalExpressCheckoutPaymentParm
                        {
                            CurrencyCode = payCurrencyCode,
                            PaymentAmount = paymentAmount,
                            OrderNo = order.OrderNo,
                            OrderId = order.OrderId
                        };

                        //调用支付控制器的扣款方法
                        var result = PaymentController.DoPaypalExpressCheckoutPayment(paymentParm);

                        //扣款结果处理
                        switch (result.PaypalExpressResultType)
                        {
                            case PaypalExpressResultType.Success:
                                return RedirectToAction("Succeed", "CheckOut");//todo 待完成，这里要跳转到paypal支付成功页面
                                break;
                            case PaypalExpressResultType.MaxAmountOutOfRange:
                            case PaypalExpressResultType.PayPalException:
                            case PaypalExpressResultType.PaySuccess:
                            default:
                                //jsonData.Data = (int)result.PaypalExpressResultType;
                                //jsonData.Message = result.PayPalExpressCheckoutUrl;
                                if (!result.PayPalExpressCheckoutUrl.IsNullOrEmpty())
                                {
                                    //失败是由于等待提交引起的还要跳转到paypal扣款页面
                                    return Redirect(result.PayPalExpressCheckoutUrl);
                                }
                                else
                                {
                                    //其他情况的扣款失败就只显示下单成功,但是会传递一个失败的原因到那个页面
                                    return RedirectToAction("Payment", "CheckOut", new RouteValueDictionary { { "PaypalExpressErrorType", (int)result.PaypalExpressResultType } });
                                }
                        }
                    }
                }
                #endregion

                return RedirectToAction("Payment", "CheckOut");
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 判断当前客户是否可以继续向购物车添加产品
        /// </summary>
        /// <returns></returns>
        public bool CheckCurrentProdSeveral()
        {
            //客户未登录操作add to cart时，进行判断，若已经超过20款商品，则出现登录注册弹框。
            //例如，若第21款商品是backorder商品，则在backorder弹框中，点击Add to Cart的时候出现登录注册弹框
            if (ShoppingCartId < 0)
            {
                var prodSeveral = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
                if (prodSeveral >= ConfigHelper.MaxNotLoggedShoppingCartItemCount) return false;
            }
            return true;
        }

        private Address GetDefaultShippingAddress(out Country country, int countryId = 0, string zipCode = "")
        {
            country = null;
            if (countryId > 0)
            {
                country = ServiceFactory.ConfigureService.GetCountryById(countryId);
            }

            var shippingAddress = new Address();
            if (ShoppingCartId > 0)
            {
                shippingAddress = ServiceFactory.CustomerService.GetDefaultShippingAddress(ShoppingCartId);
            }
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty() && (shippingAddress == null || shippingAddress.AddressId == 0))
            {
                shippingAddress =
                    ServiceFactory.CustomerService.GetAddressById(SessionHelper.CurrentCustomer.ShippingAddress.ParseTo(0));
            }

            if (shippingAddress == null || shippingAddress.AddressId == 0)
                shippingAddress = new Address { AddressId = 0, Province = string.Empty, ZipCode = zipCode };

            if (country == null && shippingAddress.Country > 0)
            {
                country = ServiceFactory.ConfigureService.GetCountryById(shippingAddress.Country);
            }

            if (country != null) return shippingAddress;

            country = ServiceFactory.ConfigureService.GetCountryByIp(PageManager.GetClientIp());

            return shippingAddress;
        }

    }
}
