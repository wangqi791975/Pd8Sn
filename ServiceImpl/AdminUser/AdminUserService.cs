using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.AdminUser;
using Com.Panduo.Service;
using Com.Panduo.Service.AdminUser;
using Com.Panduo.ServiceImpl.AdminUser.Dao;

namespace Com.Panduo.ServiceImpl.AdminUser
{
    public class AdminUserService : IAdminUserService
    {
        public IAdminDao AdminDao { private get; set; }

        public IAdminRoleDao AdminRoleDao { private get; set; }

        public IAdminPasswordUsedDao AdminPasswordUsedDao { get; set; }

        public IAdminMenuDao AdminMenuDao { get; set; }

        public IAdminModuleDao AdminModuleDao { get; set; }

        public IAdminUserModuleDao AdminUserModuleDao { get; set; }

        #region IAdminUserService 成员

        public string ERROR_USER_EMAIL_EXIST
        {
            get { return "ERROR_USER_EMAIL_EXIST"; }
        }

        public string ERROR_USER_EXIST
        {
            get { return "ERROR_USER_EXIST"; }
        }

        public string ERROR_USER_NOT_EXIST
        {
            get { return "ERROR_USER_NOT_EXIST"; }
        }

        public string ERROR_USER_IS_ROOT
        {
            get { return "ERROR_USER_IS_ROOT"; }
        }

        public string ERROR_LOGIN_FAILED
        {
            get { return "ERROR_LOGIN_FAILED"; }
        }

        public string ERROR_WRONG_PASSWORD
        {
            get { return "ERROR_WRONG_PASSWORD"; }
        }

        public string ERROR_CANNOT_DELETE_ROOT_USER
        {
            get { return "ERROR_CANNOT_DELETE_ROOT_USER"; }
        }

        public string ERROR_CANNOT_OPERATE_ROOT_USER
        {
            get { return "ERROR_CANNOT_OPERATE_ROOT_USER"; }
        }

        public string ERROR_PASSWORD_HAS_USED
        {
            get { return "ERROR_PASSWORD_HAS_USED"; }
        }

        public string ERROR_PASSWORD_INCONFORMITY
        {
            get { return "ERROR_PASSWORD_INCONFORMITY"; }
        }

        public string ERROR_PASSWORD_NOT_SAME
        {
            get { return "ERROR_PASSWORD_NOT_SAME"; }
        }

        public int AddAdminUser(Service.AdminUser.AdminUser adminUser, string password)
        {
            if (AdminDao.ExistObject("AccountEmail", adminUser.AccountEmail))
            {
                throw new BussinessException(ERROR_USER_EMAIL_EXIST);
            }

            var adminPo = GetAdminPoFromVo(adminUser);
            adminPo.DateCreated = DateTime.Now;
            adminPo.IsRoot = false;
            adminPo.RoleId = adminUser.RoleId;
            adminPo.Status = (int)AdminUserStatus.Active;
            adminPo.DateLastLogin = null;
            adminPo.Password = password.ToMd5();

            return AdminDao.AddObject(adminPo);
        }

        public Service.AdminUser.AdminUser Login(string email, string password)
        {
            var adminPo = AdminDao.GetAdminUserByEmail(email);
            if (adminPo.IsNullOrEmpty() || adminPo.Status != (int)AdminUserStatus.Active)
            {
                throw new BussinessException(ERROR_USER_NOT_EXIST);
            }

            if (password.ToMd5() != adminPo.Password)
            {
                throw new BussinessException(ERROR_WRONG_PASSWORD);
            }

            adminPo.DateLastLogin = DateTime.Now;

            return GetAdminUser(adminPo.AdminId);
        }

        public void Logout(string loginId, string ip)
        {

        }

        public void UpdateAdminUser(Service.AdminUser.AdminUser adminUser)
        {
            var adminPo = AdminDao.GetObject(adminUser.AdminId);
            if (adminPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_USER_NOT_EXIST);
            }

            if (AdminDao.ExistObjectExceptId("AccountEmail", adminUser.AccountEmail, "AdminId", adminUser.AdminId))
            {
                throw new BussinessException(ERROR_USER_EMAIL_EXIST);
            }

            adminPo.AccountEmail = adminUser.AccountEmail;
            adminPo.Remark = adminUser.Remark;
            adminPo.RoleId = adminUser.RoleId;
            adminPo.Status = (int)adminUser.AdminUserStatus;
            adminPo.Name = adminUser.Name;
            adminPo.IsViewEmail = adminUser.IsViewEmail;

            AdminDao.UpdateObject(adminPo);
        }

        public void ChangePassword(int adminUserId, string oldPassword, string newPassword, string conPassword)
        {
            var adminPo = AdminDao.GetObject(adminUserId);
            if (adminPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_USER_NOT_EXIST);
            }

            if (oldPassword.ToMd5() != adminPo.Password)
            {
                throw new BussinessException(ERROR_WRONG_PASSWORD);
            }
            //半年前时间
            DateTime halfYeayAgo = DateTime.Now.AddDays(-182);
            var adminPasswordUsed = GetAdminPasswordUsedVoFromPo(AdminPasswordUsedDao.GetAdminPasswordUsed(newPassword.ToMd5(), halfYeayAgo));
            if (!adminPasswordUsed.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PASSWORD_HAS_USED);
            }
            if (!RegexHelper.IsWordChar(newPassword))
            {
                throw new BussinessException(ERROR_PASSWORD_INCONFORMITY);
            }
            if (conPassword != newPassword)
            {
                throw new BussinessException(ERROR_PASSWORD_NOT_SAME);
            }

            adminPo.Password = newPassword.ToMd5();
            adminPo.UpdatePasswordTime = DateTime.Now;
            //修改密码
            AdminDao.UpdateObject(adminPo);
            //记录修改密码日志
            AdminPasswordUsedDao.AddObject(new AdminPasswordUsedPo { AdminId = adminUserId, DateCreated = DateTime.Now, Password = newPassword.ToMd5() });
        }

        public AdminPasswordUsed GetAdminPasswordUsed(string newPassword)
        {
            //半年前时间
            DateTime halfYeayAgo = DateTime.Now.AddDays(-182);
            return GetAdminPasswordUsedVoFromPo(AdminPasswordUsedDao.GetAdminPasswordUsed(newPassword.ToMd5(), halfYeayAgo));
        }

        public void ResetPassword(int adminUserId)
        {
            var adminPo = AdminDao.GetObject(adminUserId);
            if (adminPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_USER_NOT_EXIST);
            }

            if (adminPo.IsRoot)
            {
                throw new BussinessException(ERROR_USER_IS_ROOT);
            }

            adminPo.Password = "123456".ToMd5();

            AdminDao.UpdateObject(adminPo);
        }

        public void DeleteAdminUser(int adminUserId)
        {
            var adminPo = AdminDao.GetObject(adminUserId);
            if (!adminPo.IsNullOrEmpty())
            {
                if (adminPo.IsRoot)
                {
                    throw new BussinessException(ERROR_CANNOT_DELETE_ROOT_USER);
                }

                AdminDao.DeleteObject(adminPo);
            }
        }

        public int DeleteAdminUsers(IEnumerable<int> adminUserIds)
        {
            var count = 0;
            if (!adminUserIds.IsNullOrEmpty())
            {
                foreach (var adminUserId in adminUserIds)
                {
                    DeleteAdminUser(adminUserId);
                    count++;
                }
            }
            return count;
        }

        public void LockAdminUser(int adminUserId)
        {
            var AdminPo = AdminDao.GetObject(adminUserId);
            if (AdminPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_USER_NOT_EXIST);
            }

            if (AdminPo.IsRoot)
            {
                throw new BussinessException(ERROR_CANNOT_OPERATE_ROOT_USER);
            }

            AdminPo.Status = (int)AdminUserStatus.Lock;

            AdminDao.UpdateObject(AdminPo);
        }

        public void UnLockAdminUser(int adminUserId)
        {
            var AdminPo = AdminDao.GetObject(adminUserId);
            if (AdminPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_USER_NOT_EXIST);
            }

            if (AdminPo.IsRoot)
            {
                throw new BussinessException(ERROR_CANNOT_OPERATE_ROOT_USER);
            }

            AdminPo.Status = (int)AdminUserStatus.Active;

            AdminDao.UpdateObject(AdminPo);
        }

        public Com.Panduo.Service.AdminUser.AdminUser GetAdminUser(int adminUserId)
        {
            var AdminPo = AdminDao.GetObject(adminUserId);
            var adminUser = GetAdminUserVoFromPo(AdminPo);

            return adminUser;
        }

        public Service.AdminUser.AdminUser GetAdminUserByEmail(string email)
        {
            return GetAdminUserVoFromPo(AdminDao.GetAdminUserByEmail(email));
        }

        public IList<Com.Panduo.Service.AdminUser.AdminUser> GetAllAdminUsers()
        {
            var adminUserList = new List<Com.Panduo.Service.AdminUser.AdminUser>();
            var AdminPoList = AdminDao.GetAll();
            if (!AdminPoList.IsNullOrEmpty())
            {
                Com.Panduo.Service.AdminUser.AdminUser adminUser = null;
                foreach (var AdminPo in AdminPoList)
                {
                    adminUser = GetAdminUserVoFromPo(AdminPo);
                    adminUserList.Add(adminUser);
                }
            }
            return adminUserList;
        }

        public IList<Com.Panduo.Service.AdminUser.AdminUser> GetAdminUsersByRole(int roleId)
        {
            var adminUserList = new List<Com.Panduo.Service.AdminUser.AdminUser>();
            var AdminPoList = AdminDao.GetAllAdminUserPosByRole(roleId);
            if (!AdminPoList.IsNullOrEmpty())
            {
                Com.Panduo.Service.AdminUser.AdminUser adminUser = null;
                foreach (var AdminPo in AdminPoList)
                {
                    adminUser = GetAdminUserVoFromPo(AdminPo);
                    if (!adminUser.IsNullOrEmpty())
                    {
                        adminUserList.Add(adminUser);
                    }
                }
            }
            return adminUserList;
        }

        public int GetCountOfAdminUsersByRole(int roleId)
        {
            return AdminDao.GetCountOfAdminUsersByRole(roleId);
        }

        public PageData<Service.AdminUser.AdminUser> FindAdminUsers(int currentPage, int pageSize, IDictionary<AdminUserSearchCriteria, object> searchCriteria, IList<Sorter<AdminUserSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("Select a From AdminPo a");

            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case AdminUserSearchCriteria.Email:
                            hqlHelper.AddWhere("a.AccountEmail", HqlOperator.Like, "Email", item.Value);
                            break;
                        case AdminUserSearchCriteria.IsRoot:
                            hqlHelper.AddWhere("a.IsRoot", HqlOperator.Eq, "IsRoot", item.Value);
                            break;
                        case AdminUserSearchCriteria.RoleId:
                            hqlHelper.AddWhere("a.RoleId", HqlOperator.Eq, "RoleId", item.Value);
                            break;
                        case AdminUserSearchCriteria.Status:
                            hqlHelper.AddWhere("a.Status", HqlOperator.Eq, "Status", item.Value);
                            break;
                        case AdminUserSearchCriteria.UserName:
                            hqlHelper.AddWhere("a.Name", HqlOperator.Like, "UserName", item.Value);
                            break;
                        case AdminUserSearchCriteria.CreateDateFrom:
                            hqlHelper.AddWhere("a.DateCreated", HqlOperator.Egt, "CreateDateFrom", item.Value.ToDateQueryFrom());
                            break;
                        case AdminUserSearchCriteria.CreateDateTo:
                            hqlHelper.AddWhere("a.DateCreated", HqlOperator.Lt, "CreateDateTo", item.Value.ToDateQueryTo());
                            break;
                    }
                }
            }

            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case AdminUserSorterCriteria.CreateDate:
                            hqlHelper.AddSorter("a.DateCreated", sorter.IsAsc);
                            break;
                        case AdminUserSorterCriteria.Email:
                            hqlHelper.AddSorter("a.AccountEmail", sorter.IsAsc);
                            break;
                        case AdminUserSorterCriteria.Id:
                            hqlHelper.AddSorter("a.AdminId", sorter.IsAsc);
                            break;
                        case AdminUserSorterCriteria.IsRoot:
                            hqlHelper.AddSorter("a.IsRoot", sorter.IsAsc);
                            break;
                        case AdminUserSorterCriteria.Status:
                            hqlHelper.AddSorter("a.Status", sorter.IsAsc);
                            break;
                        case AdminUserSorterCriteria.UserName:
                            hqlHelper.AddSorter("a.Name", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("a.DateCreated", false);
            }

            //3.执行查询并返回数据
            var pageDataPo = AdminDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);

            var pageDataVo = new PageData<Service.AdminUser.AdminUser>();
            var voList = new List<Service.AdminUser.AdminUser>();
            foreach (var po in pageDataPo.Data)
            {
                voList.Add(GetAdminUserVoFromPo(po));
            }

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;

            return pageDataVo;


        }

        public int UpdateAdminStatus(int adminId)
        {
            var adminUser = AdminDao.GetObject(adminId);
            if (adminUser.IsNullOrEmpty())
            {
                throw new BussinessException("ERROR_USER_NOT_EXIST");
            }
            var status = (int)AdminUserStatus.Active;
            if (adminUser.Status == (int)AdminUserStatus.Active)
                status = (int)AdminUserStatus.Lock;
            else
                status = (int)AdminUserStatus.Active;
            AdminDao.UpdateAdminStatus(adminId, status);
            return status;
        }

        public AdminModule GetAdminModule(string controller, string action)
        {
            return GetAdminModuleVoFromPo(AdminModuleDao.GetAdminModulePo(controller, action));
        }

        public IList<AdminMenu> GetAllAdminMenus()
        {
            List<AdminMenu> adminMenus;
            if (ImplCacheHelper.GetAllAdminMenus().IsNullOrEmpty())
            {
                adminMenus = AdminMenuDao.GetAll().Select(GetAdminMenuVoFromPo).Select(m => new AdminMenu { Code = m.Code, Name = m.Name, AdminModules = AdminModuleDao.GetAdminModulePos(m.Code).Select(GetAdminModuleVoFromPo).ToList() }).ToList();
                ImplCacheHelper.SetAllAdminMenus(adminMenus);
            }
            else
            {
                adminMenus = ImplCacheHelper.GetAllAdminMenus().ToList();
            }
            return adminMenus;
        }

        public IList<AdminUserModule> GetAdminUserModules(int adminId)
        {
            return AdminUserModuleDao.GetAdminUserModules(adminId).Select(GetAdminUserModuleVoFromPo).ToList();
        }

        public IList<int> SetAdminUserModules(List<AdminUserModule> adminUserModules, int adminId)
        {
            var adminUser = AdminDao.GetObject(adminId);
            if (adminUser.IsNullOrEmpty())
            {
                throw new BussinessException("ERROR_USER_NOT_EXIST");
            }
            AdminUserModuleDao.DeleteAdminUserModules(adminId);
            return AdminUserModuleDao.AddObjects(adminUserModules.Select(GetAdminUserModulePoFromVo));
        }

        public void UpdateAdminUser(int adminId, string name, string email, bool isValid, bool isViewEmail)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdminModule(int adminId, List<string> modules)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 辅助方法
        private static readonly string Const_User_Login_Type_In = "I";
        private static readonly string Const_User_Login_Type_Out = "O";

        internal static Com.Panduo.Service.AdminUser.AdminUser GetAdminUserVoFromPo(AdminPo po)
        {
            Com.Panduo.Service.AdminUser.AdminUser adminUser = null;
            if (po != null)
            {
                adminUser = new Com.Panduo.Service.AdminUser.AdminUser();
                ObjectHelper.CopyProperties(po, adminUser, new[] { "AdminUserStatus" });
                adminUser.AdminUserStatus = EnumHelper.ToEnum<AdminUserStatus>(po.Status);
            }
            return adminUser;
        }

        internal static AdminPo GetAdminPoFromVo(Com.Panduo.Service.AdminUser.AdminUser adminUser)
        {
            AdminPo po = null;
            if (!adminUser.IsNullOrEmpty())
            {
                po = new AdminPo
                {
                    AdminId = adminUser.AdminId,
                    Name = adminUser.Name,
                    AccountEmail = adminUser.AccountEmail,
                    Password = adminUser.Password,
                    RoleId = adminUser.RoleId,
                    IsViewEmail = adminUser.IsViewEmail,
                    IsRoot = adminUser.IsRoot,
                    Status = (int)adminUser.AdminUserStatus,
                    DateCreated = adminUser.DateCreated,
                    DateLastLogin = adminUser.DateLastLogin,
                    Remark = adminUser.Remark,
                    UpdatePasswordTime = adminUser.UpdatePasswordTime,
                };

            }
            return po;
        }

        internal static AdminPasswordUsed GetAdminPasswordUsedVoFromPo(AdminPasswordUsedPo adminPasswordUsedPo)
        {
            AdminPasswordUsed adminPasswordUsed = null;
            if (!adminPasswordUsedPo.IsNullOrEmpty())
            {
                adminPasswordUsed = new AdminPasswordUsed();
                adminPasswordUsed.CopyProperties(adminPasswordUsedPo, null);
            }
            return adminPasswordUsed;
        }

        internal static string GetUserCode(int count)
        {
            return count.ToString(System.Globalization.NumberFormatInfo.InvariantInfo).PadLeft(8, '0');
        }

        internal static AdminMenu GetAdminMenuVoFromPo(AdminMenuPo adminMenuPo)
        {
            AdminMenu adminMenu = null;
            if (!adminMenuPo.IsNullOrEmpty())
            {
                adminMenu = new AdminMenu
                {
                    Code = adminMenuPo.Code,
                    Name = adminMenuPo.Name
                };
            }
            return adminMenu;
        }

        internal static AdminModule GetAdminModuleVoFromPo(AdminModulePo adminModulePo)
        {
            AdminModule adminModule = null;
            if (!adminModulePo.IsNullOrEmpty())
            {
                adminModule = new AdminModule
                {
                    ModuleCode = adminModulePo.ModuleCode,
                    MenuCode = adminModulePo.MenuCode,
                    Name = adminModulePo.Name,
                    Controller = adminModulePo.Controller,
                    Action = adminModulePo.Action,
                    Sort = adminModulePo.Sort,
                };
            }
            return adminModule;
        }

        internal static AdminUserModule GetAdminUserModuleVoFromPo(AdminUserModulePo adminUserModulePo)
        {
            AdminUserModule adminUserModule = null;
            if (!adminUserModulePo.IsNullOrEmpty())
            {
                adminUserModule = new AdminUserModule
                {
                    Id = adminUserModulePo.Id,
                    AdminId = adminUserModulePo.AdminId,
                    ModuleCode = adminUserModulePo.ModuleCode
                };
            }
            return adminUserModule;
        }

        internal static AdminUserModulePo GetAdminUserModulePoFromVo(AdminUserModule adminUserModule)
        {
            AdminUserModulePo adminUserModulePo = null;
            if (!adminUserModule.IsNullOrEmpty())
            {
                adminUserModulePo = new AdminUserModulePo
                {
                    Id = adminUserModule.Id,
                    AdminId = adminUserModule.AdminId,
                    ModuleCode = adminUserModule.ModuleCode
                };
            }
            return adminUserModulePo;
        }
        #endregion
    }
}
