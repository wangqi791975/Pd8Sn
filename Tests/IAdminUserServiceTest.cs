using Com.Panduo.Service.AdminUser;
using System;
using System.Collections.Generic;
using Com.Panduo.Service;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    
    
    /// <summary>
    ///这是 IAdminUserServiceTest 的测试类，旨在
    ///包含所有 IAdminUserServiceTest 单元测试
    ///</summary>
     [TestFixture]
    public class AdminUserServiceTest : SpringTest
    { 
        /// <summary>
        ///AddAdminUser 的测试
        ///</summary>
        [Test]
        public void AddAdminUserTest()
        {
            
            var adminUser = new AdminUser
                {
                    Code="fdsf",
                    Email = "sttf_jack@126.com",
                    CreateDate = DateTime.Now,
                    IsRoot = true,
                    LastLoginDate = DateTime.Now,
                    Name = "aabd",
                    Remark = "adfd",
                    Role = new Role(){Id = 1},
                    Status = UserStatus.Active
                };

            adminUser.Id = ServiceFactory.AdminUserService.AddAdminUser(adminUser,"123456");

            Console.WriteLine(adminUser.Id);
        }

        /// <summary>
        ///ChangePassword 的测试
        ///</summary>
        [Test]
        public void ChangePasswordTest()
        {
            ServiceFactory.AdminUserService.ChangePassword(2,"123456","abc123");
        }

        /// <summary>
        ///DeleteAdminUser 的测试
        ///</summary>
        [Test]
        public void DeleteAdminUserTest()
        {
             ServiceFactory.AdminUserService.DeleteAdminUser(4);
        }

        /// <summary>
        ///DeleteAdminUsers 的测试
        ///</summary>
        [Test]
        public void DeleteAdminUsersTest()
        {
            
        }

        /// <summary>
        ///FindAdminUsers 的测试
        ///</summary>
        [Test]
        public void FindAdminUsersTest()
        {
            int currentPage = 1;
            int pageSize = 10;
            IDictionary<AdminUserSearchCriteria, object> searchCriteria = new Dictionary<AdminUserSearchCriteria, object>
                {
                    //{AdminUserSearchCriteria.Email, "jack@126.com"},
                    //{AdminUserSearchCriteria.RoleId, 3},
                    //{AdminUserSearchCriteria.UserCode, "00000001"}
                    
                };
            IList<Sorter<AdminUserSorterCriteria>> sorterCriteria = null;
            var pageData =ServiceFactory.AdminUserService.FindAdminUsers(currentPage, pageSize, searchCriteria, sorterCriteria);

            Console.WriteLine(pageData.Pager.TotalRowCount);
        }

        /// <summary>
        ///GetAdminUser 的测试
        ///</summary>
        [Test]
        public void GetAdminUserTest()
        {
            var adminUser = ServiceFactory.AdminUserService.GetAdminUser(1); 
        }

        /// <summary>
        ///GetAdminUserByCode 的测试
        ///</summary>
        [Test]
        public void GetAdminUserByCodeTest()
        {
            var adminUser = ServiceFactory.AdminUserService.GetAdminUserByCode("00000001");
        }

        /// <summary>
        ///GetAdminUsers 的测试
        ///</summary>
        [Test]
        public void GetAdminUsersTest()
        {
            var list = ServiceFactory.AdminUserService.GetAllAdminUsers();

            Console.WriteLine(list.Count);
        }

        /// <summary>
        ///GetAdminUsersByRole 的测试
        ///</summary>
        [Test]
        public void GetAdminUsersByRoleTest()
        {
            var list = ServiceFactory.AdminUserService.GetAdminUsersByRole(3);

            Console.WriteLine(list.Count);
        }

        /// <summary>
        ///GetCountOfAdminUsersByRole 的测试
        ///</summary>
        [Test]
        public void GetCountOfAdminUsersByRoleTest()
        {
            var count = ServiceFactory.AdminUserService.GetCountOfAdminUsersByRole(3);

            Console.WriteLine(count);
        }

        /// <summary>
        ///LockAdminUser 的测试
        ///</summary>
        [Test]
        public void LockAdminUserTest()
        {
             ServiceFactory.AdminUserService.LockAdminUser(2);
        }

        /// <summary>
        ///Login 的测试
        ///</summary>
        [Test]
        public void LoginTest()
        {
            ServiceFactory.AdminUserService.Login("00000002", "123456", "127.0.0.1");
        }

        /// <summary>
        ///Logout 的测试
        ///</summary>
        [Test]
        public void LogoutTest()
        {
            ServiceFactory.AdminUserService.Logout("00000002","210.135.215.35");
        }

        /// <summary>
        ///UnLockAdminUser 的测试
        ///</summary>
        [Test]
        public void UnLockAdminUserTest()
        {
            ServiceFactory.AdminUserService.UnLockAdminUser(2);
        }

        /// <summary>
        ///UpdateAdminUser 的测试
        ///</summary>
        [Test]
        public void UpdateAdminUserTest()
        {
            var adminUser = ServiceFactory.AdminUserService.GetAdminUser(2);
            adminUser.Remark += "-修改";
            adminUser.Email = "jack88@126.com";
            ServiceFactory.AdminUserService.UpdateAdminUser(adminUser);
        } 
    }
}
