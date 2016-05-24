using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.AdminUser;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SystemMail;
using Com.Panduo.ServiceImpl.SystemMail;
using Com.Panduo.Web.Common;
using Panduo.Com.Email.SaveXml.Entity;
using Spring.Globalization.Formatters;

namespace Com.Panduo.Web.Controllers
{
    public class AdminUserController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            var url = Request["redirectUrl"];
            ViewBag.RedirectUrl = url;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false }, { "url", "" } };
            try
            {
                var currentAdminUser = ServiceFactory.AdminUserService.Login(email, password);
                if (currentAdminUser.UpdatePasswordTime.HasValue && (DateTime.Now - currentAdminUser.UpdatePasswordTime.Value).Days > ServiceConfig.AdminModifyTime)
                {
                    hashtable["url"] = "/AdminUser/ModifyPassword";
                }
                else
                {
                    hashtable["url"] = "/Home/Index";
                }
                SessionHelper.CurrentAdminUser = currentAdminUser;
                SessionHelper.CurrentAdminUserModules = ServiceFactory.AdminUserService.GetAdminUserModules(currentAdminUser.AdminId).ToList();
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_USER_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户名不存在！";
                        break;
                    case "ERROR_WRONG_PASSWORD":
                        hashtable["error"] = true;
                        hashtable["msg"] = "密码错误！";
                        break;
                }
            }
            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            if (!SessionHelper.CurrentAdminUser.IsNullOrEmpty())
            {
                SessionHelper.CurrentAdminUser = null;
            }
            return RedirectToAction("Login", "AdminUser");
        }

        [HttpGet]
        public ActionResult ModifyPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyPassword(string oldPassword, string newPassword, string conPassword)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            try
            {
                var currentAdminUser = SessionHelper.CurrentAdminUser;
                ServiceFactory.AdminUserService.ChangePassword(currentAdminUser.AdminId, oldPassword, newPassword, conPassword);
                SessionHelper.CurrentAdminUser.UpdatePasswordTime = DateTime.Now;
                ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                {
                    Attachment = newPassword,
                    DateAdded = DateTime.Now,
                    DateCreatedXml = DateTime.Now,
                    EmailType = MailType.PasswordUpdated,
                    LanguageId = ServiceFactory.ConfigureService.EnglishLangId,
                    HasAttachment = false,
                    IsCreatedXml = false,
                    KeyNo = SessionHelper.CurrentAdminUser.AdminId.ToString(CultureInfo.InvariantCulture)
                });
                
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_USER_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户名不存在！";
                        break;
                    case "ERROR_WRONG_PASSWORD":
                        hashtable["error"] = true;
                        hashtable["msg"] = "旧密码不正确，请重新输入！";
                        break;
                    case "ERROR_PASSWORD_HAS_USED":
                        hashtable["error"] = true;
                        hashtable["msg"] = "半年内不能使用重复的密码！";
                        break;
                    case "ERROR_PASSWORD_INCONFORMITY":
                        hashtable["error"] = true;
                        hashtable["msg"] = "密码必须为数字与字母结合！";
                        break;
                    case "ERROR_PASSWORD_NOT_SAME":
                        hashtable["error"] = true;
                        hashtable["msg"] = "两次输入的密码不一致，请修改！";
                        break;
                }
            }
            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult AdminSetting(int page = 1, int pageSize = 20)
        {
            var adminUsers = ServiceFactory.AdminUserService.FindAdminUsers(page, pageSize, null, null);
            return View(adminUsers);
        }

        [HttpGet]
        public ActionResult AdminUserAuth(int id, int page = 1)
        {
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("管理员设置", Url.Content("/AdminUser/AdminSetting")));
            ViewBag.AdminUser = ServiceFactory.AdminUserService.GetAdminUser(id);
            ViewBag.AdminUserModuleCodes = ServiceFactory.AdminUserService.GetAdminUserModules(id).Select(m => m.ModuleCode).ToList();
            ViewBag.Page = page;
            var adminMenus = ServiceFactory.AdminUserService.GetAllAdminMenus();
            return View(adminMenus.ToList());
        }

        [HttpPost]
        public ActionResult AdminUserAuth()
        {
            int adminId = Request["adminId"].ToInt();
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            var modules = Request["module"].Split(',');
            var adminUserModules = modules.Select(moduleCode => new AdminUserModule
            {
                AdminId = adminId,
                ModuleCode = moduleCode
            }).ToList();
            try
            {
                ServiceFactory.AdminUserService.SetAdminUserModules(adminUserModules, adminId);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_USER_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户名不存在！";
                        break;
                }
            }
            return Json(hashtable);
        }

        [HttpPost]
        public bool ChangeAdminStatus(int id)
        {
            return ServiceFactory.AdminUserService.UpdateAdminStatus(id) == (int)AdminUserStatus.Active;
        }

        [HttpGet]
        public ActionResult AdminUserEdit(int page = 1, int id = 0)
        {
            ViewBag.Page = page;
            ViewBag.Id = id;
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("管理员设置", Url.Content("/AdminUser/AdminSetting")));

            var adminUser = new AdminUser { IsViewEmail = true, AdminUserStatus = AdminUserStatus.Active };
            if (id != 0)
            {
                adminUser = ServiceFactory.AdminUserService.GetAdminUser(id);
            }
            return View(adminUser);
        }

        [HttpPost]
        public ActionResult AdminUserEdit(AdminUser adminUser)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            if (adminUser.AdminId == 0)
            {
                try
                {
                    if (adminUser.Name.IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户姓名不能为空！";
                        return Json(hashtable);
                    }
                    if (adminUser.AccountEmail.IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户邮箱不能为空！";
                        return Json(hashtable);
                    }
                    ServiceFactory.AdminUserService.AddAdminUser(adminUser, "123456");
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_USER_EMAIL_EXIST":
                            hashtable["error"] = true;
                            hashtable["msg"] = "邮箱已存在！";
                            return Json(hashtable);
                            break;
                    }
                }
            }
            else
            {
                try
                {
                    if (adminUser.Name.IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户姓名不能为空！";
                        return Json(hashtable);
                    }
                    if (adminUser.AccountEmail.IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = "用户邮箱不能为空！";
                        return Json(hashtable);
                    }
                    ServiceFactory.AdminUserService.UpdateAdminUser(adminUser);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_USER_NOT_EXIST":
                            hashtable["error"] = true;
                            hashtable["msg"] = "用户名已存在！";
                            return Json(hashtable);
                            break;
                        case "ERROR_USER_EMAIL_EXIST":
                            hashtable["error"] = true;
                            hashtable["msg"] = "邮箱已存在！";
                            return Json(hashtable);
                            break;
                    }
                }
            }
            return Json(hashtable);
        }
        #region 辅助方法

        #endregion
    }
}
