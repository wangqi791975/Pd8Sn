using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.AccessControl;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Marketing.Criteria;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SystemMail;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Customer;
using System.Linq;
using System.Net;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common.Mvc.Routing;
using Com.Panduo.Web.Models.ShoppingCart;
using Panduo.Com.Email.SaveXml.Entity;
using Resources;

namespace Com.Panduo.Web.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/

        [HttpGet]
        public ActionResult Login(string code)
        {
            var url = Request[UrlParameterKey.RedirectUrl];
            //if (url.IsNullOrEmpty() && Request.UrlReferrer != null)
            //{
            //    url = Request.UrlReferrer.AbsoluteUri;
            //}
            ViewBag.RedirectUrl = url;
            ViewBag.IsShowForgotMsg = false;
            if (!code.IsNullOrEmpty())
            {
                ServiceFactory.CustomerService.UpdateForgotStatus(code);
                ViewBag.IsShowForgotMsg = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string validatecode, string redirectUrl)
        {
            try
            {
                #region 错误判断
                if (email.Trim() == "")
                {
                    ViewBag.ErrorEmail = Lang.ErrorEmailEmpty;
                    return View();
                }

                if (!RegexHelper.IsEmail(email.Trim()))
                {
                    ViewBag.ErrorEmail = Lang.ErrorEmailFormat;
                    return View();
                }

                if (ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
                {
                    ViewBag.ErrorEmail = Lang.ErrorEmailNotExist;
                    return View();
                }

                #endregion
                //客户端IP
                string ip = PageManager.GetClientIp();
                //登录
                ServiceFactory.CustomerService.Login(email, password, ip);
                //记录客户信息session
                SessionHelper.CurrentCustomer = ServiceFactory.CustomerService.GetCustomerByEmail(email);

                if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
                {
                    //记录客户使用偏好session
                    SessionHelper.CurrentCustomerPreference =
                        ServiceFactory.CustomerService.GetPreferenceByCustomerId(SessionHelper.CurrentCustomer.CustomerId);
                }
                if (password.Trim() == "" && CookieHelper.LoginErrorCount > 2 &&
                    validatecode.Trim().ToUpper() == string.Empty)
                {
                    ViewBag.ErrorPassword = Lang.ErrorPasswordEmpty;
                    ViewBag.ErrorValidateCode = Lang.ErrorEmptyVerificationCode;
                    return View();
                }
                if (password.Trim() == "" && CookieHelper.LoginErrorCount > 2 &&
                    validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                {
                    ViewBag.ErrorPassword = Lang.ErrorPasswordEmpty;
                    ViewBag.ErrorValidateCode = Lang.ErrorValidateCode;
                    return View();
                }
                if (password.Trim() == "")
                {
                    ViewBag.ErrorPassword = Lang.ErrorPasswordEmpty;
                    return View();
                }
                //当登录错误次数大于2的时候出现验证码并需要对验证码进行验证
                if (CookieHelper.LoginErrorCount > 2)
                {
                    if (validatecode.Trim().ToUpper() == "")
                    {
                        ViewBag.ErrorValidateCode = Lang.ErrorEmptyVerificationCode;
                        return View();
                    }
                    if (validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                    {
                        ViewBag.ErrorValidateCode = Lang.ErrorValidateCode;
                        return View();
                    }
                }
                if (redirectUrl.IsNullOrEmpty())
                {
                    return Redirect(UrlRewriteHelper.GetMyAccount());
                }
                return Redirect(redirectUrl);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_NOT_EXIST":
                        ViewBag.ErrorEmail = Lang.ErrorEmailNotExist;
                        CookieHelper.LoginErrorCount++;
                        break;
                    case "ERROR_WRONG_PASSWORD":
                        if (CookieHelper.LoginErrorCount > 2 &&
                            validatecode.Trim().ToUpper() == string.Empty)
                        {
                            ViewBag.ErrorValidateCode = Lang.ErrorEmptyVerificationCode;
                        }
                        ViewBag.ErrorPassword = Lang.ErrorPassword;
                        if (CookieHelper.LoginErrorCount > 2 &&
                            validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                        {
                            ViewBag.ErrorValidateCode = Lang.ErrorValidateCode;
                        }
                        CookieHelper.LoginErrorCount++;
                        break;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult WinLogin(string email, string password, string validatecode)
        {
            var message = new Dictionary<string, object>();
            bool isLoginSuccess = false;
            try
            {
                #region 错误判断
                if (email.Trim() == "")
                {
                    message.Add("ErrorEmail", Lang.ErrorEmailEmpty);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }

                if (!RegexHelper.IsEmail(email.Trim()))
                {
                    message.Add("ErrorEmail", Lang.ErrorEmailFormat);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }

                if (ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
                {
                    message.Add("ErrorEmail", Lang.ErrorEmailNotExist);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }

                if (password.Trim() == "")
                {
                    message.Add("ErrorPassword", Lang.ErrorPasswordEmpty);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                //当登录错误次数大于2的时候出现验证码并需要对验证码进行验证
                if (CookieHelper.LoginErrorCount > 2)
                {
                    if (validatecode.Trim().ToUpper() == string.Empty)
                    {
                        message.Add("ErrorValidateCode", Lang.ErrorEmptyVerificationCode);
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                    if (validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                    {
                        message.Add("ErrorValidateCode", Lang.ErrorValidateCode);
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion
                string ip = PageManager.GetClientIp();
                if (ServiceFactory.CustomerService.Login(email, password, ip))
                {
                    SessionHelper.CurrentCustomer = ServiceFactory.CustomerService.GetCustomerByEmail(email);
                    message.Add("Name", SessionHelper.CurrentCustomer.FullName);
                    message.Add("Email", SessionHelper.CurrentCustomer.Email);
                    isLoginSuccess = true;
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_NOT_EXIST":
                        message.Add("ErrorEmail", Lang.ErrorEmailNotExist);
                        CookieHelper.LoginErrorCount++;
                        break;
                    case "ERROR_WRONG_PASSWORD":
                        message.Add("ErrorPassword", Lang.ErrorPassword);
                        CookieHelper.LoginErrorCount++;
                        break;
                }
            }
            message.Add("IsLoginSuccess", isLoginSuccess);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Logout()
        {
            SessionHelper.CurrentCustomer = null;
        }

        [HttpPost]
        public int GetLoginErrCount()
        {
            return CookieHelper.LoginErrorCount;
        }

        [HttpPost]
        public string CheckIcon()
        {
            return SessionHelper.ValidateCode;
        }

        #region FaceBook相关
        [HttpPost]
        public ActionResult LoginWithFb()
        {
            var hashTable = new Hashtable { { "error", false } };

            var fbId = Request["id"] ?? string.Empty;
            if (fbId.IsNullOrEmpty())
            {
                hashTable.Add("msg", UrlRewriteHelper.GetLoginUrl());
                return Json(hashTable);
            }
            var customer = ServiceFactory.CustomerService.CheckFacebookAccount(fbId);
            if (customer.IsNullOrEmpty())
            {
                SessionHelper.CurrentFaceBookInfo = new FacebookInfo
                {
                    FaceBookId = Request["id"] ?? string.Empty,
                    FaceBookEmail = Request["email"] ?? string.Empty,
                    FaceBookFName = Request["first_name"] ?? string.Empty,
                    FaceBookLName = Request["last_name"] ?? string.Empty,
                    FaceBookName = Request["name"] ?? string.Empty,
                    FaceBookGender = Request["gender"] ?? string.Empty,
                    FaceBookLink = Request["link"] ?? string.Empty
                };
                hashTable.Add("msg", "/Account/Facebook");
                return Json(hashTable);
            }
            else
            {
                _setCustomerSession(customer, customer.Email);
                hashTable.Add("msg", UrlRewriteHelper.GetMyAccount());
                return Json(hashTable);
            }
        }

        [HttpGet]
        public ActionResult Facebook()
        {
            if (SessionHelper.CurrentFaceBookInfo.IsNullOrEmpty())
            {
                return Redirect(UrlRewriteHelper.GetLoginUrl());
            }

            ViewBag.Sitemaps = Sitemap.GetPublicSitemap("Band your 8seasons Account");

            return View(SessionHelper.CurrentFaceBookInfo);
        }

        [HttpPost]
        public ActionResult LoginBindFb()
        {
            #region 获取参数
            var jsonData = new Hashtable { { "err", true }, { "errEmail", null }, { "errPassword", null }, { "redirectUrl", null } };
            var email = Request["FD_Email"].Trim();
            var password = Request["FD_Password"].Trim();
            var redirectUrl = Request["HID_RedirectUrl"].Trim();
            #endregion

            #region 数据校验
            if (SessionHelper.CurrentFaceBookInfo.IsNullOrEmpty())
            {
                jsonData["redirectUrl"] = UrlRewriteHelper.GetLoginUrl();
                return Json(jsonData);
            }
            if (email.IsNullOrEmpty())
            {
                jsonData["errEmail"] = Lang.ErrorEmailEmpty;
                return Json(jsonData);
            }
            if (!RegexHelper.IsEmail(email))
            {
                jsonData["errEmail"] = Lang.ErrorEmailFormat;
                return Json(jsonData);
            }
            if (password.IsNullOrEmpty())
            {
                jsonData["errPassword"] = Lang.ErrorPasswordEmpty;
                return Json(jsonData);
            }
            #endregion

            #region 执行业务方法
            try
            {
                //  登陆
                string ip = PageManager.GetClientIp();
                ServiceFactory.CustomerService.Login(email, password, ip);
                //  绑定
                Customer customer = ServiceFactory.CustomerService.GetCustomerByEmail(email);
                var facebookInfo = SessionHelper.CurrentFaceBookInfo;
                facebookInfo.CustomerId = customer.CustomerId;
                facebookInfo.CreateDateTime = DateTime.Now;
                ServiceFactory.CustomerService.BindFacebookInfo(facebookInfo);
                //  设置session
                _setCustomerSession(customer, email);

                jsonData["err"] = false;
                jsonData["redirectUrl"] = redirectUrl.IsNullOrEmpty() ? UrlRewriteHelper.GetMyAccount() : redirectUrl;
                return Json(jsonData);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_NOT_EXIST":
                        jsonData["errEmail"] = Lang.ErrorEmailNotExist;
                        break;
                    case "ERROR_WRONG_PASSWORD":
                        jsonData["errPassword"] = Lang.ErrorPassword;
                        break;
                    default:
                        jsonData["errEmail"] = bussinessException.GetError();
                        break;
                }
                return Json(jsonData);
            }
            #endregion
        }

        [HttpPost]
        public ActionResult RegisterBindFb()
        {
            #region 获取参数
            var jsonData = new Hashtable { { "err", true }, { "errEmail", null }, { "errPassword", null }, { "errConfword", null }, { "redirectUrl", null } };
            var fullName = Request["FD_FullName"].Trim();
            var email = Request["FD_EmailReg"].Trim();
            var password = Request["FD_PasswordReg"].Trim();
            var comfPassword = Request["FD_ConfPassword"].Trim();
            var customerType = Request["FD_CustomerType"];
            var newsletters = Request["FD_ReceiveNewsletters"].ParseTo(false);
            var redirectUrl = Request["HID_RedirectUrl"].Trim();
            #endregion

            #region 数据校验
            if (SessionHelper.CurrentFaceBookInfo.IsNullOrEmpty())
            {
                jsonData["redirectUrl"] = UrlRewriteHelper.GetLoginUrl();
                return Json(jsonData);
            }
            if (email == "")
            {
                jsonData["errEmail"] = Lang.ErrorEmailEmpty;
                return Json(jsonData);
            }
            if (!RegexHelper.IsEmail(email))
            {
                jsonData["errEmail"] = Lang.ErrorEmailFormat;
                return Json(jsonData);
            }
            if (password == "")
            {
                jsonData["errPassword"] = Lang.ErrorPasswordEmpty;
                return Json(jsonData);
            }
            if (password.Length < 5)
            {
                jsonData["errPassword"] = Lang.ErrorShortPassword;
                return Json(jsonData);
            }
            if (password.Length > 32)
            {
                jsonData["errPassword"] = Lang.ErrorLongPassword;
                return Json(jsonData);
            }
            if (comfPassword == "")
            {
                jsonData["errConfword"] = Lang.ErrorConfPasswordEmpty;
                return Json(jsonData);
            }
            if (password != comfPassword)
            {
                jsonData["errConfword"] = Lang.ErrorPasswordNotMatch;
                return Json(jsonData);
            }
            #endregion

            #region 执行业务方法
            try
            {
                var registerInfo = new RegisterInfo
                {
                    RegisterIp = PageManager.GetClientIp(),
                    UserLanguage = Request.UserLanguages == null ? string.Empty : Request.UserLanguages.FirstOrDefault(),
                    UserAgent = Request.UserAgent,
                    UrlReferrer = Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString(),
                    SourceType = SourceType.W
                };
                var country = ServiceFactory.ConfigureService.GetCountryByIp(registerInfo.RegisterIp);
                var customer = new Customer
                {
                    FullName = fullName != "" ? fullName : "Customer",
                    Email = email,
                    Password = password,
                    CustomerType = customerType.ToEnum<CustomerType>(),
                    Country = country.CountryId,
                };

                customer.RegisterInfo = registerInfo;
                var customerId = ServiceFactory.CustomerService.Register(customer);
                //  绑定
                var facebookInfo = SessionHelper.CurrentFaceBookInfo;
                facebookInfo.CustomerId = customerId;
                facebookInfo.CreateDateTime = DateTime.Now;
                ServiceFactory.CustomerService.BindFacebookInfo(facebookInfo);
                //  设置session
                _setCustomerSession(null, email);
                if (newsletters)
                {
                    var subscribe = new Newsletter
                    {
                        CustomerId = customerId,
                        FullName = customer.FullName,
                        Email = customer.Email,
                        NewsletterDateTime = DateTime.Now,
                        LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                        IsUnNewsletter = false
                    };
                    ServiceFactory.NewsletterService.Subscribe(subscribe);
                }
                jsonData["err"] = false;
                jsonData["redirectUrl"] = redirectUrl.IsNullOrEmpty() ? "/Account/FacebookSuccess" : redirectUrl;
                return Json(jsonData);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_EXIST":
                        jsonData["errEmail"] = Lang.ErrorEmailExist;
                        break;
                    default:
                        jsonData["errEmail"] = bussinessException.GetError();
                        break;
                }
                return Json(jsonData);
            }
            #endregion
        }

        [HttpGet]
        public ActionResult FacebookSuccess()
        {
            if (SessionHelper.CurrentFaceBookInfo.IsNullOrEmpty())
            {
                return Redirect(UrlRewriteHelper.GetLoginUrl());
            }

            ViewBag.Sitemaps = Sitemap.GetPublicSitemap("Bind your Account Successfully");

            return View();
        }
        #endregion

        #region 注册
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer, string comfPassword, string validatecode, bool newsletter)
        {
            try
            {

                #region 错误判断

                if (customer.FullName.Trim().Length > 32)
                {
                    ViewBag.ErrorFullName = Lang.ErrorFullNameTooLong;
                    return View();
                }
                if (customer.Email.Trim() == "")
                {
                    ViewBag.ErrorEmail = Lang.ErrorEmailEmpty;
                    return View();
                }
                if (!RegexHelper.IsEmail(customer.Email.Trim()))
                {
                    ViewBag.ErrorEmail = Lang.ErrorEmailFormat;
                    return View();
                }
                if (!ServiceFactory.CustomerService.GetCustomerByEmail(customer.Email).IsNullOrEmpty())
                {
                    ViewBag.ErrorEmail.Add("ErrorEmail", Lang.ErrorEmailExist);
                    return Json(ViewBag.ErrorEmail, JsonRequestBehavior.AllowGet);
                }
                if (customer.Password.Trim() == "")
                {
                    ViewBag.ErrorPassword = Lang.ErrorPasswordEmpty;
                    return View();
                }
                if (customer.Password.Trim().Length < 5)
                {
                    ViewBag.ErrorPassword = Lang.ErrorShortPassword;
                    return View();
                }
                if (customer.Password.Trim().Length > 32)
                {
                    ViewBag.ErrorPassword = Lang.ErrorLongPassword;
                    return View();
                }
                if (comfPassword.Trim() == "")
                {
                    ViewBag.ErrorConfPassword = Lang.ErrorConfPasswordEmpty;
                    return View();
                }
                if (customer.Password != comfPassword)
                {
                    ViewBag.ErrorConfPassword = Lang.ErrorPasswordNotMatch;
                    return View();
                }
                if (!customer.CustomerType.HasValue)
                {
                    ViewBag.ErrorCustomerType = Lang.ErrorCustomerType;
                    return View();
                }
                //客户端IP
                string ip = PageManager.GetClientIp();
                //当天该IP地址注册数
                int registerIpCount = ServiceFactory.CustomerService.GetRegisterCountByIP(ip);
                //如果注册数超过3个需要验证码
                if (registerIpCount >= 3)
                {
                    if (validatecode.Trim().ToUpper() == string.Empty)
                    {
                        ViewBag.ErrorValidateCode = Lang.ErrorEmptyVerificationCode;
                        return View();
                    }
                    if (validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                    {
                        ViewBag.ErrorValidateCode = Lang.ErrorValidateCode;
                        return View();
                    }
                }
                #endregion

                var registerInfo = new RegisterInfo
                {
                    RegisterIp = PageManager.GetClientIp(),
                    UserLanguage = Request.UserLanguages == null ? string.Empty : Request.UserLanguages.FirstOrDefault(),
                    UserAgent = Request.UserAgent,
                    UrlReferrer = Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString(),
                    SourceType = SourceType.W
                };

                customer.RegisterInfo = registerInfo;
                customer.RegisterLanguageId = ServiceFactory.ConfigureService.SiteLanguageId;//注册语种
                var country = ServiceFactory.ConfigureService.GetCountryByIp(registerInfo.RegisterIp);

                customer.Country = country.CountryId;

                int customerId = ServiceFactory.CustomerService.Register(customer);

                ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                {
                    Attachment = "",
                    DateAdded = DateTime.Now,
                    DateCreatedXml = DateTime.Now,
                    EmailType = MailType.CustomerRegister,
                    LanguageId = customer.RegisterLanguageId.Value,
                    HasAttachment = false,
                    IsCreatedXml = false,
                    KeyNo = customerId.ToString(CultureInfo.InvariantCulture)
                }); //注册邮件

                var registerCoupon = ServiceFactory.MarketingService.SendCouponCodeForRegister(new CouponCriteria
                {
                    CustomerId = customerId,
                    LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                    RewardType = (int)CouponMarketingRewardType.Register
                });
                SessionHelper.CurrentCustomer = ServiceFactory.CustomerService.GetCustomerById(customerId);
                //订阅
                if (newsletter)
                {
                    var subscribe = new Newsletter
                    {
                        CustomerId = customerId,
                        FullName = customer.FullName,
                        Email = customer.Email,
                        NewsletterDateTime = DateTime.Now,
                        LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                        IsUnNewsletter = false
                    };
                    ServiceFactory.NewsletterService.Subscribe(subscribe);
                }
                return RedirectToAction("RegisterSuccess", "Account", registerCoupon);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_EXIST":
                        ViewBag.ErrorEmail = Lang.ErrorEmailExist;
                        break;
                    case "ERROR_OVER_MAX_REGISTER_COUNT":
                        ViewBag.ErrorIpCount = "";
                        break;
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult WinRegister(string email, string password, string name, CustomerType? customerType, string comfPassword, string validatecode, bool newsletter)
        {
            var message = new Dictionary<string, object>();
            bool isRegisterSuccess = false;
            try
            {
                var customer = new Customer
                {
                    Email = email,
                    Password = password,
                    FullName = name,
                    CustomerType = customerType
                };
                #region 错误判断
                if (customer.Email.Trim() == "")
                {
                    message.Add("ErrorEmail", Lang.ErrorEmailEmpty);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (!RegexHelper.IsEmail(customer.Email.Trim()))
                {
                    message.Add("ErrorEmail", Lang.ErrorEmailFormat);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (!ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
                {
                    message.Add("ErrorEmail", Lang.ErrorEmailExist);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (customer.Password.Trim() == "")
                {
                    message.Add("ErrorPassword", Lang.ErrorPasswordEmpty);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (customer.Password.Trim().Length < 5)
                {
                    message.Add("ErrorPassword", Lang.ErrorShortPassword);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (customer.Password.Trim().Length > 32)
                {
                    message.Add("ErrorPassword", Lang.ErrorLongPassword);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (comfPassword.Trim() == "")
                {
                    message.Add("ErrorConfPassword", Lang.ErrorConfPasswordEmpty);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (customer.Password != comfPassword)
                {
                    message.Add("ErrorConfPassword", Lang.ErrorPasswordNotMatch);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (!customer.CustomerType.HasValue)
                {
                    message.Add("ErrorCustomerType", Lang.ErrorCustomerType);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                //客户端IP
                string ip = PageManager.GetClientIp();
                //当天该IP地址注册数
                int registerIpCount = ServiceFactory.CustomerService.GetRegisterCountByIP(ip);
                //如果注册数超过3个需要验证码
                if (registerIpCount >= 3)
                {
                    if (validatecode.Trim().ToUpper() == string.Empty)
                    {
                        message.Add("ErrorValidateCode", Lang.ErrorEmptyVerificationCode);
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                    if (validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                    {
                        message.Add("ErrorValidateCode", Lang.ErrorValidateCode);
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion

                var registerInfo = new RegisterInfo
                {
                    RegisterIp = PageManager.GetClientIp(),
                    UserLanguage = Request.UserLanguages == null ? string.Empty : Request.UserLanguages.FirstOrDefault(),
                    UserAgent = Request.UserAgent,
                    UrlReferrer = Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString(),
                    SourceType = SourceType.W
                };

                customer.RegisterInfo = registerInfo;
                customer.RegisterLanguageId = ServiceFactory.ConfigureService.SiteLanguageId;//注册语种
                var country = ServiceFactory.ConfigureService.GetCountryByIp(registerInfo.RegisterIp);
                if (customer.FullName == "")
                    customer.FullName = "Customer";
                customer.Country = country.CountryId;

                int customerId = ServiceFactory.CustomerService.Register(customer);

                ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                {
                    Attachment = "",
                    DateAdded = DateTime.Now,
                    DateCreatedXml = DateTime.Now,
                    EmailType = MailType.CustomerRegister,
                    LanguageId = customer.RegisterLanguageId.Value,
                    HasAttachment = false,
                    IsCreatedXml = false,
                    KeyNo = customerId.ToString(CultureInfo.InvariantCulture)
                }); //注册邮件

                SessionHelper.CurrentCustomer = ServiceFactory.CustomerService.GetCustomerById(customerId);
                isRegisterSuccess = true;
                //订阅
                if (newsletter)
                {
                    var subscribe = new Newsletter
                    {
                        CustomerId = customerId,
                        FullName = customer.FullName,
                        Email = customer.Email,
                        NewsletterDateTime = DateTime.Now,
                        LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                        IsUnNewsletter = false
                    };
                    ServiceFactory.NewsletterService.Subscribe(subscribe);
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_EXIST":
                        message.Add("ErrorEmail", Lang.ErrorEmailExist);
                        break;
                    case "ERROR_OVER_MAX_REGISTER_COUNT":
                        message.Add("ErrorEmail", "");
                        break;
                }

            }
            message.Add("IsRegisterSuccess", isRegisterSuccess);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int GetRegisterCount()
        {
            return ServiceFactory.CustomerService.GetRegisterCountByIP(PageManager.GetClientIp());
        }

        public ActionResult RegisterSuccess(Coupon registerCoupon)
        {
            var customerGroups = ServiceFactory.CustomerService.GetAllCustomerGroups().ToList();
            ViewBag.RegCouponCurrency = ServiceFactory.ConfigureService.GetCurrency(registerCoupon.AmountCurrencyId);
            ViewBag.RegCouponAmount = registerCoupon.Amount;
            return View(customerGroups);
        }
        #endregion
        [HttpGet]
        public ActionResult ForgetPwd(bool? show)
        {
            ViewBag.SendMessage = false;
            ViewBag.ResetLink = show;
            ViewBag.Sitemaps = Sitemap.GetForgetPasswordSitemap();

            return View();
        }

        [HttpPost]
        public ActionResult ForgetPwd(string email, string validatecode)
        {
            var hashTable = new Hashtable { { "error", false }, { "email", string.Empty }, { "valcode", string.Empty }, { "url", string.Empty } };
            try
            {
                if (email.Trim() == "")
                {
                    hashTable["error"] = true;
                    hashTable["email"] = Lang.ErrorEnterEmail;
                    return Json(hashTable);
                }
                if (!RegexHelper.IsEmail(email.Trim()))
                {
                    hashTable["error"] = true;
                    hashTable["email"] = Lang.ErrorEmailFormat;
                    return Json(hashTable);
                }
                if (ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
                {
                    hashTable["error"] = true;
                    hashTable["email"] = Lang.ErrorCreateEmail;
                    return Json(hashTable);
                }
                if (validatecode.Trim().ToUpper() == string.Empty)
                {
                    hashTable["error"] = true;
                    hashTable["valcode"] = Lang.ErrorEmptyVerificationCode;
                    return Json(hashTable);
                }
                if (validatecode.Trim().ToUpper() != SessionHelper.ValidateCode)
                {
                    hashTable["error"] = true;
                    hashTable["valcode"] = Lang.ErrorValidateCode;
                    return Json(hashTable);
                }
                string code = ServiceFactory.CustomerService.FindPassword(email);
                var customer = ServiceFactory.CustomerService.GetCustomerByEmail(email);
                string reseturl = UrlRewriteHelper.GetResetPwdUrl() + "?code=" + System.Web.HttpUtility.UrlEncode(code);
                string cancelurl = UrlRewriteHelper.GetLoginUrl() + "?code=" + System.Web.HttpUtility.UrlEncode(code);
                ServiceFactory.MailService.ForgetPwdEmail(customer.FullName, email, reseturl, cancelurl);
                ViewBag.SendMessage = true;
                ViewBag.SendEmail = email;

                hashTable["url"] = "/Account/ForgetSuccess?email=" + email;
                return Json(hashTable);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_EMAIL_NOT_EXIST":
                        hashTable["error"] = true;
                        hashTable["email"] = Lang.ErrorCreateEmail;
                        break;
                }
            }
            return Json(hashTable);
        }

        [HttpGet]
        public ActionResult ForgetSuccess(string email)
        {
            ViewBag.CustomerEmail = email;
            ViewBag.Sitemaps = Sitemap.GetForgetPasswordSitemap();
            return View();
        }

        [HttpGet]
        public ActionResult ResetPwd(string code)
        {
            try
            {
                ViewBag.VerifyCode = code;
                string verifyCode = ServiceFactory.CustomerService.VerifyCode(code);
                int customerId = Int32.Parse(verifyCode.Split(',')[1]);
                ViewBag.Email = ServiceFactory.CustomerService.GetCustomerById(customerId).Email;
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_ENCRYPT_CODE_NOT_EXIST":
                        break;
                    case "ERROR_CUSTOMER_NOT_EXIST":
                        break;
                    case "ERROR_OVERTIME":
                        break;
                }
                return RedirectToAction("ForgetPwd", "Account", new { show = true });
            }
            return View();
        }


        public ActionResult ResetPwd(string code, string password, string confPassword)
        {
            try
            {
                ViewBag.VerifyCode = code;
                try
                {
                    string verifyCode = ServiceFactory.CustomerService.VerifyCode(code);
                    ViewBag.Email = ServiceFactory.CustomerService.GetCustomerById(Int32.Parse(verifyCode.Split(',')[1])).Email;
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.Message)
                    {
                        case "ERROR_ENCRYPT_CODE_NOT_EXIST":
                            break;
                        case "ERROR_CUSTOMER_NOT_EXIST":
                            break;
                        case "ERROR_OVERTIME":
                            break;
                    }
                    return RedirectToAction("ForgetPwd", "Account", new { show = true });
                }
                #region 错误验证
                if (password.Trim() == "")
                {
                    ViewBag.ErrorPassword = Lang.ErrorPasswordEmpty;
                    return View();
                }
                if (password.Trim().Length < 5)
                {
                    ViewBag.ErrorPassword = Lang.ErrorShortPassword;
                    return View();
                }
                if (password.Trim().Length > 32)
                {
                    ViewBag.ErrorPassword = Lang.ErrorLongPassword;
                    return View();
                }
                if (confPassword.Trim() == "")
                {
                    ViewBag.ErrorConfPassword = Lang.ErrorConfPasswordEmpty;
                    return View();
                }
                if (password != confPassword)
                {
                    ViewBag.ErrorConfPassword = Lang.ErrorPasswordNotMatch;
                    return View();
                }
                #endregion

                int forgetPasswordId = ServiceFactory.CustomerService.GetForgotPassword(code).Id;
                int customerId = Int32.Parse(ServiceFactory.CustomerService.VerifyCode(code).Split(',')[1]);
                ServiceFactory.CustomerService.ResetPassword(customerId, password, forgetPasswordId);
                ViewBag.IsSuccess = true;
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.Message)
                {
                    case "ERROR_CUSTOMER_NOT_EXIST":

                        return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult AccountSetting()
        {
            ViewBag.Sitemaps = Sitemap.GetMyAccountSettingSitemap();
            string ip = PageManager.GetClientIp();
            var country = ServiceFactory.ConfigureService.GetCountryByIp(ip);
            ViewBag.CurrentCountryName = country.CountryName;
            return View(SessionHelper.CurrentCustomer);
        }

        [HttpPost]
        public ActionResult SaveAvatar(string avatarImg)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };

            var customer = SessionHelper.CurrentCustomer;
            ServiceFactory.CustomerService.EditCustomerAvatar(customer.CustomerId, avatarImg);
            SessionHelper.CurrentCustomer.Avatar = avatarImg;

            return Json(hashtable);
        }

        [HttpPost]
        public void ChangePassword(string curPassword, string newPassword)
        {
            int customerId = SessionHelper.CurrentCustomer.CustomerId;
            ServiceFactory.CustomerService.UpdatePassword(customerId, curPassword, newPassword);
        }

        [HttpPost]
        public bool CheckPassword(string curPassword)
        {
            int customerId = SessionHelper.CurrentCustomer.CustomerId;
            return ServiceFactory.CustomerService.CheckPassword(customerId, curPassword);
        }

        [HttpPost]
        public JsonResult CheckEmail(string email, string type)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            switch (type)
            {
                case "register":
                    if (!ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = Lang.ErrorEmailExist;
                    }
                    break;
                case "login":
                    if (ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = Lang.ErrorEmailNotExist;
                    }
                    break;
            }
            return Json(hashtable);
        }

        [HttpPost]
        public void ProfileSetting(int customerId, Customer newCustomerInfo, int? countryId)
        {
            var customer = ServiceFactory.CustomerService.GetCustomerById(customerId);

            customer.Gender = newCustomerInfo.Gender;
            customer.FullName = newCustomerInfo.FullName;
            customer.Birthday = newCustomerInfo.Birthday;
            customer.Telphone = newCustomerInfo.Telphone;
            customer.Cellphone = newCustomerInfo.Cellphone;
            customer.Skype = newCustomerInfo.Skype;
            customer.CustomerType = newCustomerInfo.CustomerType;
            customer.PersonWebSite = newCustomerInfo.PersonWebSite;
            customer.Country = countryId;
            SessionHelper.CurrentCustomer = customer;
            ServiceFactory.CustomerService.UpdateCustomerInfo(customer);
        }

        [HttpGet]
        public ActionResult MyPreference()
        {
            int customerId = SessionHelper.CurrentCustomer.CustomerId;
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Currencies = ServiceFactory.ConfigureService.GetAllValidCurrencies();
            ViewBag.MyPreferences = SessionHelper.CurrentCustomerPreference;
            ViewBag.CurrentCurrency = SessionHelper.CurrentCurrency.CurrencyId;
            ViewBag.CUrrentLanguage = ServiceFactory.ConfigureService.SiteLanguageId;
            ViewBag.Sitemaps = Sitemap.GetMyAccountPreferenceSitemap();
            return View();
        }

        [HttpPost]
        public ActionResult MyPreference(int? lang, int? currency, Unit? linear, Unit? weight, ListShowType? page, ListShowCount? perpage)
        {
            int customerId = SessionHelper.CurrentCustomer.CustomerId;
            var preference = new Preference
            {
                CustomerId = customerId
            };
            if (linear.HasValue)
            {
                preference.SizeUnit = linear.Value;
                SessionHelper.CurrentCustomerPreference.SizeUnit = linear.Value;
                CookieHelper.CurrentCustomerPreference.SizeUnit = linear.Value;
            }
            if (weight.HasValue)
            {
                preference.WeightUnit = weight.Value;
                SessionHelper.CurrentCustomerPreference.WeightUnit = weight.Value;
                CookieHelper.CurrentCustomerPreference.WeightUnit = weight.Value;
            }
            if (currency.HasValue)
            {
                preference.CurrencyId = currency.Value;
                SessionHelper.CurrentCurrency = ServiceFactory.ConfigureService.GetCurrency(currency.Value);
                SessionHelper.CurrentCustomerPreference.CurrencyId = currency.Value;
                CookieHelper.CurrentCustomerPreference.CurrencyId = currency.Value;
            }
            if (lang.HasValue)
            {
                preference.LanguageId = lang.Value;
                SessionHelper.CurrentCustomerPreference.LanguageId = lang.Value;
                CookieHelper.CurrentCustomerPreference.LanguageId = lang.Value;
            }
            if (page.HasValue)
            {
                preference.ListShowType = page.Value;
                SessionHelper.CurrentCustomerPreference.ListShowType = page.Value;
                CookieHelper.CurrentCustomerPreference.ListShowType = page.Value;
            }
            if (perpage.HasValue)
            {
                preference.ListShowCount = perpage.Value;
                SessionHelper.CurrentCustomerPreference.ListShowCount = perpage.Value;
                CookieHelper.CurrentCustomerPreference.ListShowCount = perpage.Value;
            }
            ServiceFactory.CustomerService.SetPreference(preference);
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Currencies = ServiceFactory.ConfigureService.GetAllValidCurrencies();
            ViewBag.MyPreferences = ServiceFactory.CustomerService.GetPreferenceByCustomerId(customerId);
            ViewBag.CurrentCurrency = SessionHelper.CurrentCurrency.CurrencyId;
            ViewBag.CUrrentLanguage = ServiceFactory.ConfigureService.SiteLanguageId;
            ViewBag.Sitemaps = Sitemap.GetMyAccountPreferenceSitemap();
            return View();
        }

        [HttpGet]
        public ActionResult NewsLetter()
        {
            string email = SessionHelper.CurrentCustomer.Email;
            ViewBag.IsUnNewsletter = ServiceFactory.CustomerService.GetNewsletter(email).IsUnNewsletter;
            ViewBag.Sitemaps = Sitemap.GetMyAccountNewsletterSitemap();
            return View();
        }

        [HttpPost]
        public void NewsLetter(bool isnewletter)
        {
            int customerId = SessionHelper.CurrentCustomer.CustomerId;
            if (isnewletter)
            {
                ServiceFactory.CustomerService.Subscribe(customerId);
            }
            else
            {
                ServiceFactory.CustomerService.UnSubscribe(customerId);
            }
        }



        [HttpGet]
        public ActionResult MyProducts()
        {
            return View();
        }

        #region MyCoupon
        [HttpGet]
        public ActionResult MyCoupon(string ajaxtab, int page = 1, int pageSize = 20)
        {
            ViewBag.Sitemaps = Sitemap.GetMyAccountCouponSitemap();
            var customer = SessionHelper.CurrentCustomer;
            var activeSearchCriteria = new Dictionary<CustomerCouponSearchCriteria, object>
            {
                {CustomerCouponSearchCriteria.ActiveCoupon, CouponStatus.NotUsed}
            };
            var activeCoupons = ServiceFactory.CouponService.FindMyCustomerCoupon(customer.CustomerId, page, pageSize, activeSearchCriteria, new List<Sorter<CustomerCouponSorterCriteria>>());
            var inactiveSearchCriteria = new Dictionary<CustomerCouponSearchCriteria, object>
            {
                {CustomerCouponSearchCriteria.InActiveCoupon, ""},
            };
            var inactiveCoupons = ServiceFactory.CouponService.FindMyCustomerCoupon(customer.CustomerId, page, pageSize, inactiveSearchCriteria, new List<Sorter<CustomerCouponSorterCriteria>>());

            ViewBag.ActiveCoupons = activeCoupons;
            ViewBag.InactiveCoupons = inactiveCoupons;
            if (ajaxtab.IsNullOrEmpty())
                return View();
            if (ajaxtab == "active")
            {
                return View("MyCouponActive", activeCoupons);
            }
            if (ajaxtab == "inactive")
            {
                return View("MyCouponInActive", inactiveCoupons);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddCoupon(string couponCode)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            var customer = SessionHelper.CurrentCustomer;
            try
            {
                if (customer.Country != null)
                    ServiceFactory.CouponService.PickCustomerCoupon(couponCode, customer.CustomerId,
                        SessionHelper.CurrentCurrency.CurrencyId, customer.Country.Value,
                        ServiceFactory.ConfigureService.SiteLanguageId);
                else
                {
                    hashtable["msg"] = "Please set your country.";
                    hashtable["error"] = true;
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_CUSTOMER_NOT_EXIST":
                        hashtable["msg"] = Lang.ErrorCusNotExist;
                        hashtable["error"] = true;
                        break;
                    case "ERROR_COUPON_NOT_EXIST":
                        hashtable["msg"] = Lang.ErrorWrongCouCode;
                        hashtable["error"] = true;
                        break;
                    case "ERROR_CURRENCY_CANT_PICK":
                        hashtable["msg"] = Lang.ErrorWrongCouCode;
                        hashtable["error"] = true;
                        break;
                    case "ERROR_COUNTRY_CANT_PICK":
                        hashtable["msg"] = Lang.ErrorWrongCouCode;
                        hashtable["error"] = true;
                        break;
                    case "ERROR_LANGUAGE_CANT_PICK":
                        hashtable["msg"] = Lang.ErrorWrongCouCode;
                        hashtable["error"] = true;
                        break;
                    case "ERROR_COUPON_HAS_PICK":
                        hashtable["msg"] = Lang.ErrorCouCodeAlrAdd;
                        hashtable["error"] = true;
                        break;
                    case "ERROR_COUPON_HAS_EXPIRED":
                        hashtable["msg"] = Lang.ErrorCouponExpired;
                        hashtable["error"] = true;
                        break;
                }
            }
            return Json(hashtable);
        }
        #endregion

        #region Club
        [HttpGet]
        public ActionResult WelcomeInClub()
        {
            int customerId = SessionHelper.CurrentCustomer.IsNullOrEmpty() ? 0 : SessionHelper.CurrentCustomer.CustomerId;
            var clubShippingFee = ServiceFactory.ClubService.GetClubShippingFee(customerId);
            return View(clubShippingFee);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns>验证码</returns>
        public ActionResult GetValidatorGraphics()
        {
            var validateHelper = new ValidateCodeHelper();
            byte[] graphic = validateHelper.CreateValidateImage();
            SessionHelper.ValidateCode = validateHelper.ValidateCode;
            return File(graphic, @"image/jpeg");
        }
        #endregion

        /// <summary>
        /// 账户中心
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAccount()
        {
            var customer = SessionHelper.CurrentCustomer;
            var vo = new CustomerVo();
            if (!customer.IsNullOrEmpty())
            {
                vo.Customer = customer;
                vo.CustomerGroup = ServiceFactory.CustomerService.GetAllCustomerGroups();
                var h = vo.CustomerGroup.FirstOrDefault(x => x.Discount < vo.Customer.VipDiscount);
                vo.Balance = ServiceFactory.CashService.GetCustomerBalance(customer.CustomerId);
                var order =
                ServiceFactory.OrderService.GetCustomerLatestOrder(customer.CustomerId,
                    new Sorter<LatestOrderSorterCriteria>(LatestOrderSorterCriteria.OrderTime, false));
                if (!order.IsNullOrEmpty())
                {
                    vo.TaxNumber = order.CustomerTaxNumber;
                }

                //club
                var clubCoupon = ServiceFactory.CouponService.GetCouponCustomer(customer.CustomerId, CouponStatus.NotUsed,
                    CouponMarketingRewardType.Club);
                if (!clubCoupon.IsNullOrEmpty())
                {
                    vo.ClubCoupon = clubCoupon[0];
                }

                //Register
                var registerCoupon = ServiceFactory.CouponService.GetCouponCustomer(customer.CustomerId, CouponStatus.NotUsed,
                   CouponMarketingRewardType.Register);
                if (!registerCoupon.IsNullOrEmpty())
                {
                    vo.RegisterCoupon = registerCoupon[0];
                }

                if (customer.IsClub)
                {
                    vo.ClubCustomer = ServiceFactory.ClubService.GetClubByCustomerId(customer.CustomerId);
                }

                if (!h.IsNullOrEmpty())
                {
                    vo.NextNeedCost = h.MinAmount - (customer.HistoryAmount.HasValue ? customer.HistoryAmount.Value : 0.0M);
                    vo.NextDiscount = h.Discount;
                }
            }
            var list = ServiceFactory.ProductService.SearchProducts(1, 10, new Dictionary<ProductSearchCriteria, object>(){
            {ProductSearchCriteria.ProductSearchAreaType, ProductSearchAreaType.BestSeller}
            }, new List<Sorter<ProductSorterCriteria>>(), false, false);
            var productList = list.ProductPageData.Data;

            var productinfos = ServiceFactory.ProductService.GetProductInfos(productList,
                                                                     isIncludeProductStock: true,
                                                                     isIncludeProductImage: false,
                                                                     isIncludeProductProperty: false,
                                                                     isIncludeProductPrice: true,
                                                                     isJudgeHotSeller: true,
                                                                     isJudgeHasSimilarProuct: false);
            vo.ProductInfoList = productinfos;
            ViewBag.Sitemaps = Sitemap.GetMyAccountSitemap();
            return View(vo);
        }

        #region AddressBook
        public ActionResult AddressBook()
        {
            //  berad
            ViewBag.Sitemaps = Sitemap.GetMyAccountAddressBookSitemap();

            var addresses = ServiceFactory.CustomerService.GetAddressesByCustomerId(SessionHelper.CurrentCustomer.CustomerId).ToList();
            ViewBag.defaultId = SessionHelper.CurrentCustomer.ShippingAddress ?? 0;

            return View(addresses);
        }

        public ActionResult AddressBookDelete()
        {
            var hashTable = new Hashtable();
            hashTable.Add("error", false);

            var addressId = Request["AddressId"].ParseTo(0);
            if (addressId > 0)
            {
                try
                {
                    ServiceFactory.CustomerService.DeleteAddress(SessionHelper.CurrentCustomer.CustomerId, addressId);
                }
                catch (BussinessException bussinessException)
                {
                    hashTable.Add("msg", bussinessException.GetError());
                    hashTable["error"] = true;
                }
            }
            else
            {
                hashTable["error"] = true;
            }

            return Json(hashTable);
        }

        public ActionResult AddressBookSetDef()
        {
            var hashTable = new Hashtable();
            hashTable.Add("error", false);

            var addressId = Request["AddressId"].ParseTo(0);
            if (addressId > 0)
            {
                try
                {
                    ServiceFactory.CustomerService.SetShippingAddress(SessionHelper.CurrentCustomer.CustomerId, addressId);
                    SessionHelper.CurrentCustomer.ShippingAddress = addressId;
                }
                catch (BussinessException bussinessException)
                {
                    hashTable.Add("msg", bussinessException.GetError());
                    hashTable["error"] = true;
                }
            }
            else
            {
                hashTable["error"] = true;
            }

            return Json(hashTable);
        }

        public ActionResult AddressBookInfo()
        {
            var addressId = Request["AddressId"].ParseTo(0);

            var countryAll = ServiceFactory.ConfigureService.GetAllCountryLanguages();
            var countryLanguages = countryAll.Where(x => x.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).ToList();
            var commonCountries = ServiceFactory.ConfigureService.GetCommonCountry();
            var commonCountryLanguages = commonCountries.Select(x => new CountryLanguage { CountryId = x.CountryId, CountryName = countryLanguages.FirstOrDefault(w => w.CountryId == x.CountryId && w.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).CountryName }).ToList();
            var countryLanguage = new CountryLanguage();
            Address address = new Address();
            var isCheckedAddress = string.Empty;
            var isDisabledAddress = string.Empty;

            if (addressId <= 0)
            {
                Country country = ServiceFactory.ConfigureService.GetCountryByIp(PageManager.GetClientIp());
                var countryCurrents = countryLanguages.Where(x => x.CountryId == country.CountryId).ToList();
                countryLanguage = countryCurrents[0];
            }
            else
            {
                address = ServiceFactory.CustomerService.GetAddressById(addressId);
                countryLanguage = ServiceFactory.ConfigureService.GetCountryLanguage(address.Country, ServiceFactory.ConfigureService.SiteLanguageId);

                if (addressId == SessionHelper.CurrentCustomer.ShippingAddress)
                {
                    isCheckedAddress = "checked=checked";
                }
                var addressList = ServiceFactory.CustomerService.GetAddressesByCustomerId(SessionHelper.CurrentCustomer.CustomerId);
                if (addressList.Count <= 1 || addressId == SessionHelper.CurrentCustomer.ShippingAddress)
                {
                    isDisabledAddress = "disabled=disabled";
                }
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

            return View("AddressBookEdit", shoppingAddress);
        }

        public ActionResult AddressBookSave(FormCollection form)
        {
            var hashTable = new Hashtable { { "error", false }, { "msg", null } };

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
                    hashTable["msg"] = "Message:数据不合法!";
                    return Json(hashTable);
                }

                string fullName = string.Format("{0} {1}", firstName, lastName);
                if (ServiceFactory.ConfigureService.SiteLanguageId == 6)
                {
                    fullName = string.Format("{0} {1}", lastName, firstName);
                }

                Address address = new Address();
                address.AddressId = addressId;
                address.CustomerId = SessionHelper.CurrentCustomer.CustomerId;
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
                    ServiceFactory.CustomerService.UpdateAddress(SessionHelper.CurrentCustomer.CustomerId, address);
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
            }
            catch (BussinessException bussinessException)
            {
                hashTable["msg"] = bussinessException.GetError();
                hashTable["error"] = true;
            }

            return Json(hashTable);
        }
        #endregion

        /// <summary>
        /// 登陆或注册完，设置session。通用可以调用这个方法
        /// </summary>
        /// <param name="customer">customer实体</param>
        /// <param name="email">email</param>
        private void _setCustomerSession(Customer customer, string email)
        {
            SessionHelper.CurrentCustomer = !customer.IsNullOrEmpty() ? customer : ServiceFactory.CustomerService.GetCustomerByEmail(email);
            if (!SessionHelper.CurrentCustomer.IsNullOrEmpty())
            {
                //记录客户使用偏好session
                SessionHelper.CurrentCustomerPreference =
                    ServiceFactory.CustomerService.GetPreferenceByCustomerId(SessionHelper.CurrentCustomer.CustomerId);
                //todo 记录客户使用偏好session
            }
        }
    }
}
