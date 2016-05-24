using System;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    /// CustomerServiceTest 的摘要说明
    /// </summary>
    [TestFixture]
    public class CustomerServiceTest : SpringTest
    {
        public CustomerServiceTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }



        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Test]
        public void TestMethod1()
        {
            var s = ServiceFactory.CustomerService;
            //
            // TODO: 在此处添加测试逻辑
            //int b = ServiceFactory.AdminUserService.AddAdminUser(new AdminUser { Email = "321" }, "123");

            #region 对象


            var customer = new Customer
            {
                FirstName = "wang",
                LastName = "qi",
                FullName = "wangqi",
                Password = "kaoyanjiayou".ToMd5(),
                Birthday = "1989-12-17",
                CustomerType = CustomerType.JewelryDiyFan,
                Country = 1,
                Gender = Gender.Male,
                Email = "1wangqi791975@126.com",
                Telphone = "057182791975",
                Cellphone = "15088615280",
            };

            var facebookInfo = new FacebookInfo
            {
                FaceBookId = "999999999",
                CustomerId = 2,
                CreateDateTime = DateTime.Now
            };

            var preference1 = new Preference
            {
                CustomerId = 2,
                SizeUnit = Unit.Metric,
                WeightUnit = Unit.Metric,
                CurrencyId = 1,
                LanguageId = 1,
                ListShowType = ListShowType.List,
                ListShowCount = (ListShowCount)10
            };

            var preference2 = new Preference
            {
                CustomerId = 2,
                SizeUnit = Unit.Imperial,
                WeightUnit = Unit.Imperial,
                CurrencyId = 2,
                LanguageId = 2,
                ListShowType = ListShowType.Gallery,
                ListShowCount = (ListShowCount)20
            };

            var addressadd = new Address
            {
                CustomerId = 2,
                FirstName = "qi",
                LastName = "qi",
                FullName = "qiqi",
                Gender = Gender.Female,
                Telphone = "057188888888",
                CompanyName = "CompanyName",
                Country = 1,
                Province = "zhejiang",
                City = "hangzhou",
                ZipCode = "310000",
                Street1 = "",
                Street2 = ""
            };

            var addressupdate = new Address
            {
                AddressId = 4,
                CustomerId = 2,
                FirstName = "mo",
                LastName = "mo",
                FullName = "momo",
                Gender = Gender.Female,
                Telphone = "057111111118",
                CompanyName = "CompanyName1",
                Country = 12,
                Province = "zhejiang1",
                City = "hangzhou1",
                ZipCode = "3100001",
                Street1 = "",
                Street2 = ""
            };



            #endregion

            #region 服务测试

            //注册 （业务异常测试通过，功能测试通过）
            //int register = ServiceFactory.CustomerService.Register(customer);


            //登录 (业务异常测试通过，功能测试通过)
            //bool login = ServiceFactory.CustomerService.Login(customer.Email,customer.Password,customer.RegisterIp);

            //登出 （业务异常测试通过）
            //ServiceFactory.CustomerService.Logout(1);

            //检测facebook账号 (业务异常测试通过，功能测试通过)
            //var customertest = ServiceFactory.CustomerService.CheckFacebookAccount(facebookInfo.FaceBookId);
            //if (customertest.IsNullOrEmpty())
            //{
            //    ServiceFactory.CustomerService.BindFacebookInfo(facebookInfo);
            //}

            //修改客户信息 (业务异常测试通过，功能测试通过)
            //customer.CustomerId = 2;
            //ServiceFactory.CustomerService.UpdateCustomerInfo(customer);

            //修改客户邮箱 (功能测试通过)
            //ServiceFactory.CustomerService.ChangeEmail(2, "188146068@qq.com");

            //校验密码 (业务异常测试通过，功能测试通过)
            //ServiceFactory.CustomerService.CheckPassword(2, "kaoyanjiayou".ToMd5());

            //修改密码 (业务异常测试通过，功能测试通过)
            //ServiceFactory.CustomerService.UpdatePassword(3, "kaoyanjia1you".ToMd5(), "wangqi".ToMd5());

            //找回密码 todo

            //验证密码串 todo

            //客户重置密码 (业务异常测试通过，功能测试通过)
            //ServiceFactory.CustomerService.ResetPassword(1,"wangqi".ToMd5());

            //客服重置密码  (业务异常测试通过，功能测试通过)
            //ServiceFactory.CustomerService.ResetPassword(1, 2);

            //通过邮箱地址获取客户（功能测试通过)
            //var testc = s.GetCustomerByEmail("2wangqi791975@126.com");

            //返回当天该Ip注册数（功能测试通过)
            //int a = s.GetRegisterCountByIP("111.111.111.111");

            //订阅/退订 (业务异常测试通过，功能测试通过)
            //s.Subscribe(1);
            //s.Subscribe(2);
            //s.UnSubscribe(1);
            //s.UnSubscribe(3);
            //s.UnSubscribe(2);

            //设置客户使用偏好 (功能测试通过)
            //s.SetPreference(preference2);

            //获取客户使用偏好 (功能测试通过)
            //var preferences = s.GetPreferenceByCustomerId(2);

            //添加地址 (业务异常测试通过，功能测试通过)
            //int id = s.AddAddress(addressadd,true,true);

            //修改地址 (业务异常测试通过，功能测试通过)
            //s.UpdateAddress(1, addressupdate);

            //删除地址 (业务异常测试通过，功能测试通过)
            //s.DeleteAddress(3,4);

            //设置默认货运/账单地址 (业务异常测试通过，功能测试通过)
            //s.SetShippingAddress(3, 4);
            //s.SetBillingAddress(2, 4);

            //通过地址Id获取地址 (功能测试通过)
            //var a = s.GetAddressById(4);

            //获取客户地址 (功能测试通过)
            //var a = s.GetAddressesByCustomerId(2);

            //获取默认货运/账单地址 (功能测试通过)
            //var a = s.GetDefaultBillingAddress(2);
            //var b = s.GetDefaultShippingAddress(2);

            //todo 客户等级单元测试



            #endregion
        }

        [Test]
        public void GetNextCustomerGroupTest()
        {
            var a = ServiceFactory.CustomerService.GetNextCustomerGroup(3);
        }

        [Test]
        public void EditCustomerManagerTest()
        {
            ServiceFactory.CustomerService.EditCustomerManager(new CustomerManager()
            {
                Avatar = null,
                ChineseName = null,
                CompanyEmail = "123",
                CustomerManagerId = 0,
                Name = "1qwe",
                ServiceEmail = "qweqe",
                Skype = "asdfasdf",
                Telphone = "adfasdf",
            });
        }

        [Test]
        public void EditCustomerAvatar()
        {
            ServiceFactory.CustomerService.EditCustomerAvatar(2, "/ImagesAvator/User/2-130709622904216558.png");
        }
    }
}
