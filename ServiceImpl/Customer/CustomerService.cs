using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Cash;
using Com.Panduo.Service.Customer;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Unit = Com.Panduo.Service.Customer.Unit;

namespace Com.Panduo.ServiceImpl.Customer
{
    public class CustomerService : ICustomerService
    {
        public ICustomerDao CustomerDao { private get; set; }
        public ICustomerAvatarDao CustomerAvatarDao { private get; set; }
        public ICustomerPreferenceDao CustomerPreferenceDao { private get; set; }
        public ICustomerManagerDao CustomerManagerDao { private get; set; }
        public ICustomerAddressDao CustomerAddressDao { private get; set; }
        public static ICustomerGroupDao CustomerGroupDao { private get; set; }
        public ICustomerGroupDescDao CustomerGroupDescDao { private get; set; }
        public ISubscribeDao SubscribeDao { private get; set; }
        public IForgotPasswordDao ForgotPasswordDao { private get; set; }
        public ICustomerRemarkDao CustomerRemarkDao { private get; set; }
        public ICustomerHighRiskDao CustomerHighRiskDao { private get; set; }

        public string ERROR_EMAIL_EXIST
        {
            get { return "ERROR_EMAIL_EXIST"; }
        }

        public string ERROR_EMAIL_NOT_EXIST
        {
            get { return "ERROR_EMAIL_NOT_EXIST"; }
        }

        public string ERROR_CUSTOMER_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_NOT_EXIST"; }
        }

        public string ERROR_WRONG_PASSWORD
        {
            get { return "ERROR_WRONG_PASSWORD"; }
        }

        public string ERROR_FACEBOOK_ACCOUNT_PASSWORD
        {
            get { return "ERROR_FACEBOOK_ACCOUNT_PASSWORD"; }
        }

        public string ERROR_PASSWORD_CODE_TIMEOUT
        {
            get { return "ERROR_PASSWORD_CODE_TIMEOUT"; }
        }

        public string ERROR_WRONG_PASSWORD_CODE
        {
            get { return "ERROR_WRONG_PASSWORD_CODE"; }
        }

        public string ERROR_NO_SHIPPING_ADDRESS
        {
            get { return "ERROR_NO_SHIPPING_ADDRESS"; }
        }

        public string ERROR_NO_BILLING_ADDRESS
        {
            get { return "ERROR_NO_BILLING_ADDRESS"; }
        }

        public string ERROR_ADDRESS_NOT_EXIST
        {
            get { return "ERROR_ADDRESS_NOT_EXIST"; }
        }

        public string ERROR_OVER_MAX_REGISTER_COUNT
        {
            get { return "ERROR_OVER_MAX_REGISTER_COUNT"; }
        }

        public string ERROR_CUSTOMER_EXIST_BIND_FACEBOOK
        {
            get { return "ERROR_CUSTOMER_EXIST_BIND_FACEBOOK"; }
        }

        public string ERROR_FACEBOOK_EXIST_BIND_CUSTOMER
        {
            get { return "ERROR_FACEBOOK_EXIST_BIND_CUSTOMER"; }
        }

        public string EEOR_PASSWORD_USED
        {
            get { return "EEOR_PASSWORD_USED"; }
        }

        public string ERROR_WRONG_CUSTOMER
        {
            get { return "ERROR_WRONG_CUSTOMER"; }
        }

        public string ERROR_NOT_SUBSCRIBE
        {
            get { return "ERROR_NOT_SUBSCRIBE"; }
        }

        public string ERROR_DEFAULT_SHIPPING_ADDRESS
        {
            get { return "ERROR_DEFAULT_SHIPPING_ADDRESS"; }
        }

        public string ERROR_DEFAULT_BILLING_ADDRESS
        {
            get { return "ERROR_DEFAULT_BILLING_ADDRESS"; }
        }

        public string ERROR_ENCRYPT_CODE_NOT_EXIST
        {
            get { return "ERROR_ENCRYPT_CODE_NOT_EXIST"; }
        }

        public string ERROR_OVERTIME
        {
            get { return "ERROR_OVERTIME"; }
        }

        public int Register(Service.Customer.Customer customer)
        {
            if (CustomerDao.ExistObject("CustomerEmail", customer.Email))//判断email是否注册过
            {
                throw new BussinessException(ERROR_EMAIL_EXIST);
            }
            if (CustomerDao.GetRegisterCount(customer.RegisterInfo.RegisterIp) > 5)//当天统一Ip只能注册5个会员
            {
                throw new BussinessException(ERROR_OVER_MAX_REGISTER_COUNT);
            }

            var sourceType = string.Empty;
            var urlReferrer = customer.RegisterInfo.UrlReferrer;
            if (!urlReferrer.IsNullOrEmpty())
            {
                if (sourceType.IndexOf("facebook", StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    sourceType = "facebook";
                }
                else if (sourceType.IndexOf("google", StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    sourceType = "google";
                }
                else if (sourceType.IndexOf("mailchimp", StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    sourceType = "mailchimp";
                }
            }

            //随机取2位秘钥
            var key = NoExtendHelper.GetRandomString(2);
            customer.Password = ToCustomerPassword(customer.Password, key);

            var customerPo = GetCustomerPoFromVo(customer);

            return CustomerDao.AddObject(customerPo);
        }

        public bool Login(string email, string password, string ip)
        {
            var customerPo = CustomerDao.GetCustomerByEmail(email);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_EMAIL_NOT_EXIST);
            }

            if (!customerPo.Status)
                return false;

            var passwrods = FromCustomerPassword(customerPo.Password);
            var md5Password = string.Empty;
            var key = string.Empty;
            if (!passwrods.IsNullOrEmpty())
            {
                if (passwrods.Length > 1)
                {
                    md5Password = passwrods[0];
                    key = passwrods[1];
                }
                else
                {
                    md5Password = passwrods[0];
                }

            }

            var currentPassword = ToCustomerPassword(password, key);
            var comparePassword = GetCustomerPasswordKey(md5Password, key);
            if (currentPassword != comparePassword)
            {
                throw new BussinessException(ERROR_WRONG_PASSWORD);
            }
            //登陆次数和最后登录时间记录
            int loginCount = customerPo.TotalLoginCount.HasValue ? customerPo.TotalLoginCount.Value + 1 : 1;
            CustomerDao.UpdateCustomerLogin(customerPo.CustomerId, loginCount);

            return true;
        }

        public void Logout(int customerId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
        }

        public Service.Customer.Customer CheckFacebookAccount(string facebookId)
        {
            return GetCustomerVoFromPo(CustomerDao.GetCustomerByFacebookId(facebookId));
        }

        public void BindFacebookInfo(FacebookInfo facebookInfo)
        {
            var customerPo = CustomerDao.GetObject(facebookInfo.CustomerId);
            //通过客户Id判断客户是否存在
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            //判断要绑定的客户是否已经绑定了facebookId
            if (!customerPo.FacebookId.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_EXIST_BIND_FACEBOOK);
            }
            //判断要绑定的facebookId是否绑定了其他客户
            if (!CustomerDao.GetCustomerByFacebookId(facebookInfo.FaceBookId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_FACEBOOK_EXIST_BIND_CUSTOMER);
            }
            CustomerDao.AddFacebookInfo(facebookInfo);
        }

        public void UpdateCustomerInfo(Service.Customer.Customer customer)
        {
            int customerCount = CustomerDao.GetCustomerCount(customer.CustomerId);
            //判断客户数是否存在
            if (customerCount == 0)
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            CustomerDao.UpdateObject(GetCustomerPoFromVo(customer));
        }

        public void UpdateCustomerStatus(int customerId)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (!customer.IsNullOrEmpty())
                CustomerDao.UpdateCustomerStatus(customerId, !customer.Status);
        }

        public void EditCustomerAvatar(int customerId, string avatarPath)
        {
            CustomerDao.UpDateCustomerAvatar(customerId, avatarPath);
        }

        public void ChangeEmail(int customerId, string email)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            if (!CustomerDao.GetCustomerByEmail(email).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_EMAIL_EXIST);
            }
            CustomerDao.UpdateCustomerEmail(customerId, email);
        }

        public bool CheckPassword(int customerId, string password)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }

            var passwrods = FromCustomerPassword(customerPo.Password);
            var md5Password = string.Empty;
            var key = string.Empty;
            if (!passwrods.IsNullOrEmpty())
            {
                if (passwrods.Length > 1)
                {
                    md5Password = passwrods[0];
                    key = passwrods[1];
                }
                else
                {
                    md5Password = passwrods[0];
                }

            }

            var currentPassword = ToCustomerPassword(password, key);

            return currentPassword == md5Password;
        }

        public void UpdatePassword(int customerId, string oldPassword, string newPassword)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            if (!CheckPassword(customerId, oldPassword))
            {
                throw new BussinessException(ERROR_WRONG_PASSWORD);
            }

            //随机取2位秘钥
            var key = NoExtendHelper.GetRandomString(2);
            var newMd5Password = ToCustomerPassword(newPassword, key);

            CustomerDao.UpdateCustomerPassword(customerId, newMd5Password);
        }

        public string FindPassword(string email)
        {
            var customerPo = CustomerDao.GetCustomerByEmail(email);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_EMAIL_NOT_EXIST);
            }
            //密码串加密，记录找回密码日志
            var guid = Guid.NewGuid().ToString();
            var createDate = DateTime.Now;
            var expiredDate = createDate.AddHours(2);
            string encryptCode = CryptHelper.EncryptDes(guid + "," + customerPo.CustomerId + "," + expiredDate);
            var forgotPassword = new ForgotPasswordPo
            {
                CustomerId = customerPo.CustomerId,
                EncryptedString = encryptCode,
                DateCreated = createDate,
                DateExpired = expiredDate,
                Status = false
            };
            ForgotPasswordDao.AddObject(forgotPassword);
            return encryptCode;
        }

        public ForgotPassword GetForgotPassword(string encryptCode)
        {
            return GetForgotPasswordVoFromPo(ForgotPasswordDao.GetValidObjectByCode(encryptCode));
        }

        public string VerifyCode(string encryptCode)
        {
            if (ForgotPasswordDao.GetValidObjectByCode(encryptCode).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_ENCRYPT_CODE_NOT_EXIST);
            }
            var decCode = CryptHelper.DecryptDes(encryptCode);
            string guid = decCode.Split(',')[0];
            int customerId = Int32.Parse(decCode.Split(',')[1]);
            DateTime expiredDate = Convert.ToDateTime(decCode.Split(',')[2]);
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            //大于2小时失效
            if ((DateTime.Now - expiredDate).TotalHours >= 2)
            {
                throw new BussinessException(ERROR_OVERTIME);
            }
            return decCode;
        }

        public void UpdateForgotStatus(string encryptCode)
        {
            var forgot = ForgotPasswordDao.GetValidObjectByCode(encryptCode);
            int forgotPasswordId = 0;
            if (!forgot.IsNullOrEmpty())
                forgotPasswordId = ForgotPasswordDao.GetValidObjectByCode(encryptCode).Id;
            ForgotPasswordDao.UpdateForgotPassword(forgotPasswordId, DateTime.Now, true);
        }

        public void ResetPassword(int customerId, string newPassword, int forgotPasswordId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }

            //随机取2位秘钥
            var key = NoExtendHelper.GetRandomString(2);
            var newMd5Password = ToCustomerPassword(newPassword, key);

            CustomerDao.UpdateCustomerPassword(customerId, newMd5Password);
            //修改找回密码日志
            ForgotPasswordDao.UpdateForgotPassword(forgotPasswordId, DateTime.Now, true);
        }

        public void ResetPassword(int customerId, int adminId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }


            //随机取2位秘钥
            var key = NoExtendHelper.GetRandomString(2);
            var password = ToCustomerPassword("123456", key);//todo 读取配置文件

            CustomerDao.UpdateCustomerPassword(customerId, password);
        }

        public Service.Customer.Customer GetCustomerById(int customerId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            return GetCustomerVoFromPo(customerPo);
        }

        public Service.Customer.Customer GetCustomerByEmail(string email)
        {
            var customerPo = CustomerDao.GetCustomerByEmail(email);
            return GetCustomerVoFromPo(customerPo);
        }

        public PageData<Service.Customer.Customer> FindAllCustomers(int currentPage, int pageSize, IDictionary<CustomerSearchCriteria, object> searchCriteria, IList<Sorter<CustomerSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<Service.Customer.Customer>();
            var dataList = new List<Service.Customer.Customer>();
            var rowCount = 0;

            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@sortField", SqlDbType.NVarChar) {Value =  string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.NVarChar) {Value = "ASC"},
                new SqlParameter("@keyword", SqlDbType.NVarChar) {Value = searchCriteria.TryGet(CustomerSearchCriteria.KeyWord).ToSqlString()},
                new SqlParameter("@customerType", SqlDbType.Int) {Value = searchCriteria.TryGet(CustomerSearchCriteria.CustomerType).ToSqlString()},
                new SqlParameter("@languageId", SqlDbType.Int) {Value = searchCriteria.TryGet(CustomerSearchCriteria.LanguageId).ToSqlString()},
                new SqlParameter("@sourceType", SqlDbType.Int) {Value = searchCriteria.TryGet(CustomerSearchCriteria.SourceType).ToSqlString()},
            };

            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_customer_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                while (reader.Read())
                {
                    var customer = new Service.Customer.Customer
                    {
                        CustomerId = reader["customer_id"].ParseTo<int>(),
                        Email = reader["customer_email"].ToString(),
                        FullName = reader["full_name"].ToString(),
                        RegisterInfo = new RegisterInfo
                        {
                            SourceType = (SourceType)reader["source_type"].ParseTo<int>(),
                            DateCreated = reader["date_created"].ParseTo<DateTime>(),
                            UserLanguage = reader["register_useragent_language"].ToString(),
                            RegisterIp = reader["register_ip"].ToString(),

                        },
                        LastLoginDateTime = reader["date_login"].ParseTo<DateTime>(),
                        CustomerGroupId = reader["customer_group_id"].ParseTo<int>(),
                        RegisterLanguageId = reader["register_language_id"].ParseTo<int>(),
                        Status = reader["status"].ParseTo<bool>()
                    };
                    dataList.Add(customer);
                }
            }
            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }

        public PageData<Service.Customer.CustomerAvatar> FindAllCustomerAvatars(int currentPage, int pageSize)
        {
            var list = CustomerAvatarDao.FindAllCustomerAvatars(currentPage, pageSize);

            PageData<Service.Customer.CustomerAvatar> pageData = new PageData<Service.Customer.CustomerAvatar>();
            pageData.Data = list.Data.Select(x => GetCustomerAvatarFromPo((CustomerAvatarPo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        public PageData<Service.Customer.Customer> FindLongTimeNotUpdatedCustomers(int currentPage, int pageSize,
            IDictionary<CustomerSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<Service.Customer.Customer>();
            var dataList = new List<Service.Customer.Customer>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@keyword", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(CustomerSearchCriteria.KeyWord).ToSqlString()
                },
                new SqlParameter("@languageId", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(CustomerSearchCriteria.LanguageId)
                },              
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };

            //设置排序条件(暂时不需要提供)
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case CustomerSorterCriteria.CustomerId:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "Id";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }

            Service.Customer.Customer customer;
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                    "up_admin_shoppingcart_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                //分页数据 
                while (reader.Read())
                {
                    customer = new Service.Customer.Customer
                    {
                        CustomerId = reader["customer_id"].ParseTo<int>(),
                        Email = reader["customer_email"].ToString(),
                        FullName = reader["full_name"].ToString(),
                        RegisterInfo = new RegisterInfo
                        {
                            SourceType = (SourceType)reader["source_type"].ParseTo<int>(),
                            DateCreated = reader["date_created"].ParseTo<DateTime>(),
                            UserLanguage = reader["register_useragent_language"].ToString(),
                            RegisterIp = reader["register_ip"].ToString(),

                        },
                        LastLoginDateTime = reader["date_login"].ParseTo<DateTime>(),
                        CustomerGroupId = reader["customer_group_id"].ParseTo<int>(),
                        RegisterLanguageId = reader["register_language_id"].ParseTo<int>(),
                        Status = reader["status"].ParseTo<bool>()

                    };
                    dataList.Add(customer);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;

        }

        public int GetRegisterCountByIP(string ip)
        {
            return CustomerDao.GetRegisterCount(ip);
        }

        public Newsletter GetNewsletter(string email)
        {
            return NewsletterService.GetSubscribeVoFromPo(SubscribeDao.GetSubscribe(email));
        }

        public void Subscribe(int customerId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var newsletter = new Newsletter
            {
                CustomerId = customerId,
                FullName = customerPo.FullName,
                Email = customerPo.CustomerEmail,
                LanguageId = customerPo.RegisterLanguageId.HasValue ? (int)customerPo.RegisterLanguageId : 0,
                NewsletterDateTime = DateTime.Now,
                IsUnNewsletter = false
            };
            var subscribePo = NewsletterService.GetSubscribePoFromVo(newsletter);
            var subscribeId = SubscribeDao.GetSubscribeId(newsletter.LanguageId, newsletter.Email);
            if (subscribeId == 0)
            {
                SubscribeDao.AddObject(subscribePo);
            }
            else
            {
                var email = customerPo.CustomerEmail;
                const bool isNuNewsletter = false;
                SubscribeDao.UpdateSubscribeStatus(email, isNuNewsletter);
            }
        }

        public void UnSubscribe(int customerId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var email = customerPo.CustomerEmail;
            if (SubscribeDao.GetSubscribe(email).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_NOT_SUBSCRIBE);
            }
            const bool isNuNewsletter = true;
            SubscribeDao.UpdateSubscribeStatus(email, isNuNewsletter);
        }

        #region 高危客户

        /// <summary>
        /// 添加高危客户
        /// </summary>
        /// <param name="customerHighRisk"></param>
        public void AddCustomerHighRisk(CustomerHighRisk customerHighRisk)
        {
            var customer = CustomerDao.GetObject(customerHighRisk.CustomerId);
            if (customer == null)
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var customerHighRiskPo = CustomerHighRiskDao.GetCustomerHighRiskByCustomerEmail(customer.CustomerEmail);
            if (customerHighRiskPo != null)
            {
                throw new BussinessException("ERROR_EMAIL_EXIST");
            }

            customer.HighRisk = false;
            CustomerHighRiskDao.AddObject(GetCustomerHighRiskPoFromVo(customerHighRisk));
            CustomerDao.UpdateObject(customer);

        }

        /// <summary>
        /// 删除高危客户
        /// </summary>
        /// <param name="customerId"></param>
        public void DelCustomerHighRisk(int customerId)
        {
            var customer = CustomerDao.GetObject(customerId);
            if (customer == null)
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            customer.HighRisk = false;
            CustomerHighRiskDao.DeleteObjectById(customerId);
            CustomerDao.UpdateObject(customer);
        }

        /// <summary>
        /// 分页查询高危客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns></returns>
        public PageData<CustomerHighRisk> FindCustomerHighRisks(int currentPage, int pageSize,
            IDictionary<CustomerHighRiskSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerHighRiskSorterCriteria>> sorterCriteria)
        {

            var list = CustomerHighRiskDao.FindCustomerHighRisks(currentPage, pageSize, searchCriteria, sorterCriteria);
            PageData<CustomerHighRisk> pageData = new PageData<CustomerHighRisk>();
            pageData.Data = list.Data.Select(x => GetCustomerHighRiskVoFromPo((CustomerHighRiskPo)x)).ToList();
            pageData.Pager = list.Pager;

            return pageData;
        }

        #endregion

        #region 销售备注

        public CustomerRemark GetCustomerRemark(int customerId)
        {
            return GetCustomerRemarkVoFromPo(CustomerRemarkDao.GetCustomerRemarkPo(customerId));
        }

        public void SetCustomerRemark(int customerId, string remark, int adminId)
        {
            var customerRemark = CustomerRemarkDao.GetCustomerRemarkPo(customerId);
            if (customerRemark.IsNullOrEmpty())
            {
                CustomerRemarkDao.AddObject(new CustomerRemarkPo
                {
                    AdminId = adminId,
                    CustomerId = customerId,
                    DateCreated = DateTime.Now,
                    Remark = remark
                });
            }
            else
            {
                CustomerRemarkDao.UpdateCustomerRemarkPo(customerId, remark, adminId);
            }
        }

        #endregion

        public PageData<CustomerManager> FindAllManager(int currentPage, int pageSize, IDictionary<ManagerSearchCriteria, object> searchCriteria, IList<Sorter<ManagerSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT R FROM CustomerManagerPo R ");

            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case ManagerSearchCriteria.Name:
                            hqlHelper.AddWhere("R.Name", HqlOperator.Like, "Name", item.Value);
                            break;

                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {

                }
            }
            else
            {
                hqlHelper.AddSorter("R.CustomerManagerId", true);
            }

            var pageDataPo = CustomerManagerDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<CustomerManager>();
            var voList = pageDataPo.Data.Select(GetCustomerManagerVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public CustomerManager GetCustomerManager(int customerManagerId)
        {
            return GetCustomerManagerVoFromPo(CustomerManagerDao.GetObject(customerManagerId));
        }

        public int EditCustomerManager(CustomerManager customerManager)
        {
            int customerManagerId;
            if (customerManager.CustomerManagerId == 0)
            {
                customerManagerId = CustomerManagerDao.AddObject(GetCustomerManagerPoFromVo(customerManager));
            }
            else
            {
                CustomerManagerDao.UpdateObject(GetCustomerManagerPoFromVo(customerManager));
                customerManagerId = customerManager.CustomerManagerId;
            }
            return customerManagerId;
        }

        public void DeleteCustomerManager(int customerManagerId)
        {
            CustomerManagerDao.DeleteObjectById(customerManagerId);
        }

        public void SetPreference(Preference preference)
        {
            var preferencePos = GetCustomerPreferencePoFromVo(preference);
            foreach (var customerPreferencePo in preferencePos)
            {
                if (CustomerPreferenceDao.GetPreference(preference.CustomerId, customerPreferencePo.Key).IsNullOrEmpty())
                {
                    CustomerPreferenceDao.AddObject(customerPreferencePo);
                }
                else
                {
                    CustomerPreferenceDao.UpdatePreference(customerPreferencePo.CustomerId, customerPreferencePo.Key, customerPreferencePo.Value);
                }
            }
        }

        public Preference GetPreferenceByCustomerId(int customerId)
        {
            List<CustomerPreferencePo> customerPreferencePos = CustomerPreferenceDao.GetPreferences(customerId).ToList();
            return GetCustomerPreferenceVoFromPo(customerPreferencePos);
        }

        #region Address Book

        public int AddAddress(Address address, bool isDefaultShippingAddress, bool isDefaultBillingAddress)
        {
            var customerPo = CustomerDao.GetObject(address.CustomerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var addressPo = GetCustomerAddressPoFromVo(address);
            var addressId = CustomerAddressDao.AddObject(addressPo);
            if (isDefaultShippingAddress)
                CustomerDao.UpdateShippingDefaultAddress(address.CustomerId, addressId);
            if (isDefaultBillingAddress)
                CustomerDao.UpdateBillingDefaultAddress(address.CustomerId, addressId);
            return addressId;
        }

        public void UpdateAddress(int customerId, Address address)
        {
            if (customerId != address.CustomerId)
            {
                throw new BussinessException(ERROR_WRONG_CUSTOMER);
            }
            var customerPo = CustomerDao.GetObject(address.CustomerId);
            if (customerPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMER_NOT_EXIST);
            }
            var addressPo = CustomerAddressDao.GetObject(address.AddressId);
            if (addressPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_ADDRESS_NOT_EXIST);
            }
            CustomerAddressDao.UpdateObject(GetCustomerAddressPoFromVo(address, addressPo));
        }

        public void DeleteAddress(int customerId, int addressId)
        {
            var addressPo = CustomerAddressDao.GetObject(addressId);
            if (addressPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_ADDRESS_NOT_EXIST);
            }
            if (customerId != addressPo.CustomerId)
            {
                throw new BussinessException(ERROR_WRONG_CUSTOMER);
            }
            var customerPo = CustomerDao.GetObject(customerId);
            if (customerPo.DefaultBillingAddress != null && customerPo.DefaultShippingAddress == addressId)
            {
                throw new BussinessException(ERROR_DEFAULT_SHIPPING_ADDRESS);
            }
            CustomerAddressDao.DeleteObjectById(addressId);
        }

        public void SetShippingAddress(int customerId, int addressId)
        {
            var addressPo = CustomerAddressDao.GetObject(addressId);
            if (addressPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_ADDRESS_NOT_EXIST);
            }
            if (customerId != addressPo.CustomerId)
            {
                throw new BussinessException(ERROR_WRONG_CUSTOMER);
            }
            CustomerDao.UpdateShippingDefaultAddress(customerId, addressId);
        }

        public void SetBillingAddress(int customerId, int addressId)
        {
            var addressPo = CustomerAddressDao.GetObject(addressId);
            if (addressPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_ADDRESS_NOT_EXIST);
            }
            if (customerId != addressPo.CustomerId)
            {
                throw new BussinessException(ERROR_WRONG_CUSTOMER);
            }
            CustomerDao.UpdateBillingDefaultAddress(customerId, addressId);
        }

        public Address GetAddressById(int addressId)
        {
            return GetCustomerAddressVoFromPo(CustomerAddressDao.GetObject(addressId));
        }

        public IList<Address> GetAddressesByCustomerId(int customerId)
        {
            var customerAddressPos = CustomerAddressDao.GetAddresses(customerId);
            var addresses = customerAddressPos.Select(GetCustomerAddressVoFromPo).ToList();
            return addresses;
        }

        public Address GetDefaultShippingAddress(int customerId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            return GetCustomerAddressVoFromPo(CustomerAddressDao.GetObject(customerPo.DefaultShippingAddress.HasValue ? customerPo.DefaultShippingAddress.Value : 0));
        }

        public Address GetDefaultBillingAddress(int customerId)
        {
            var customerPo = CustomerDao.GetObject(customerId);
            return GetCustomerAddressVoFromPo(CustomerAddressDao.GetObject(customerPo.DefaultBillingAddress.HasValue ? customerPo.DefaultBillingAddress.Value : customerPo.DefaultShippingAddress.HasValue ? customerPo.DefaultShippingAddress.Value : 0));
        }
        #endregion


        public void SetCustomerGroupDesc(Dictionary<int, string> nameMap)
        {
            foreach (var variable in nameMap)
            {
                CustomerGroupDescDao.UpdateCustomerGroupName(variable.Key, variable.Value);
            }
        }

        public IList<CustomerGroup> GetAllCustomerGroups()
        {
            var customerGroupPos = CustomerGroupDao.GetAll();
            return customerGroupPos.Select(GetCustomerGroupVoFromPo).ToList();
        }

        public CustomerGroup GetCustomerGroupById(int customerGroupId)
        {
            return GetCustomerGroupVoFromPo(CustomerGroupDao.GetObject(customerGroupId));
        }

        public CustomerGroupDesc GetCustomerGroupDesc(int customerGroupId, int languageId)
        {
            return GetCustomerGroupDescVoFromPo(CustomerGroupDescDao.GetCustomerGroupDesc(customerGroupId, languageId));
        }

        public IList<CustomerGroupDesc> GetCustomerGroupDesc(int customerGroupId)
        {
            var customerGroupDescPos = CustomerGroupDescDao.GetAll();
            return customerGroupDescPos.Select(GetCustomerGroupDescVoFromPo).ToList();
        }

        public IList<CustomerGroupDesc> GetCustomerGroupDescsByLanguage(int languageId)
        {
            return CustomerGroupDescDao.GetCustomerGroupDescs(languageId).Select(GetCustomerGroupDescVoFromPo).ToList();
        }

        public CustomerGroup GetNextCustomerGroup(int customerGroupId)
        {
            var customerGrouPo = CustomerGroupDao.GetObject(customerGroupId);
            var customerGroup = customerGrouPo.IsNullOrEmpty() ? GetCustomerGroupVoFromPo(CustomerGroupDao.GetNextCustomerGroup(1)) : GetCustomerGroupVoFromPo(CustomerGroupDao.GetNextCustomerGroup(customerGrouPo.Percentage));
            customerGroup.CustomerGroupDesc = CustomerGroupDescDao.GetCustomerGroupDescsByCustomerGroupId(customerGroup.Id).Select(GetCustomerGroupDescVoFromPo).ToList();
            return customerGroup;
        }

        #region 辅助方法
        /// <summary>
        /// 转换为客户密码
        /// </summary>
        /// <param name="password">原密码</param>
        /// <param name="key">秘钥</param>
        /// <returns></returns>
        internal static string ToCustomerPassword(string password, string key)
        {
            var passwordValue = GetCustomerPasswordKey(password, key);


            return GetCustomerPasswordKey(passwordValue.ToMd5(), key);
        }

        internal static string GetCustomerPasswordKey(string password, string key)
        {
            var passwordValue = string.Format("{0}{1}{2}", password, key.IsNullOrEmpty() ? "" : ":", key);

            return passwordValue;
        }

        internal static string[] FromCustomerPassword(string password)
        {
            return password.Split(':');
        }

        internal static Service.Customer.Customer GetCustomerVoFromPo(CustomerPo customerPo)
        {
            Service.Customer.Customer customer = null;
            if (!customerPo.IsNullOrEmpty())
            {
                //  xiaoyong.lv
                decimal vipDiscount = 1;
                if (customerPo.CustomerGroupId.HasValue)
                {
                    var customerGroup = CustomerGroupDao.GetObject(customerPo.CustomerGroupId.Value);
                    if (!customerGroup.IsNullOrEmpty())
                        vipDiscount = customerGroup.Percentage;
                }
                customer = new Service.Customer.Customer
                {
                    CustomerId = customerPo.CustomerId,
                    FirstName = customerPo.FirstName,
                    LastName = customerPo.LastName,
                    FullName = customerPo.FullName,
                    Password = customerPo.Password,
                    Birthday = customerPo.Birthday.HasValue ? customerPo.Birthday.Value.ToString("MM/dd/yyyy") : "",
                    CustomerType = customerPo.DescribesType == null ? CustomerType.Others : (CustomerType)customerPo.DescribesType,
                    Country = customerPo.CountryId,
                    Gender = (Gender)Convert.ToChar(customerPo.Gender),
                    Avatar = customerPo.Avatar,
                    Email = customerPo.CustomerEmail,
                    Telphone = customerPo.PhoneNumber,
                    Cellphone = customerPo.CellPhone,
                    PersonWebSite = customerPo.PersonWebSite,
                    Skype = customerPo.Skype,
                    TotalLoginCount = customerPo.TotalLoginCount,
                    RegisterLanguageId = customerPo.RegisterLanguageId.HasValue ? customerPo.RegisterLanguageId : 0,
                    ShippingAddress = customerPo.DefaultShippingAddress,
                    BillingAddress = customerPo.DefaultBillingAddress,
                    IsDanger = customerPo.HighRisk,
                    LastLoginDateTime = customerPo.DateLogin.HasValue ? customerPo.DateLogin : DateTime.Now,
                    HistoryAmount = customerPo.OrderAmount,
                    CustomerGroupId = customerPo.CustomerGroupId,
                    ClubLevel = customerPo.ClubLevel,
                    IsVip = customerPo.CustomerGroupId.HasValue && customerPo.CustomerGroupId.Value > 0,
                    IsClub = !ServiceFactory.ClubService.GetValidClubByCustomerId(customerPo.CustomerId).IsNullOrEmpty(),
                    VipDiscount = vipDiscount,
                    IsRcd = customerPo.IsRcd.HasValue && customerPo.IsRcd.Value,
                    DateLastPlaceOrder = customerPo.DateLastPlaceOrder,
                    Status = customerPo.Status,
                    FaxNumber = customerPo.FaxNumber,
                    RegisterInfo = new RegisterInfo
                    {
                        DateCreated = customerPo.DateCreated.HasValue ? customerPo.DateCreated : DateTime.MinValue,
                        UserLanguage = customerPo.RegisterUserAgentLanguage,
                        RegisterIp = customerPo.RegisterIp,
                        SourceType = (SourceType)customerPo.SourceType,
                        UrlReferrer = customerPo.SourceUrl
                    },
                };
            }
            return customer;
        }

        internal static CustomerPo GetCustomerPoFromVo(Service.Customer.Customer customer)
        {
            CustomerPo customerPo = null;
            if (!customer.IsNullOrEmpty())
            {

                customerPo = new CustomerPo
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    FullName = customer.FullName,
                    Password = customer.Password,
                    DescribesType = customer.CustomerType == null ? 0 : (int)customer.CustomerType,
                    CountryId = customer.Country,
                    Gender = ((char)(customer.Gender.HasValue ? customer.Gender : Gender.Male)).ToString(CultureInfo.InvariantCulture),
                    Avatar = customer.Avatar,
                    CustomerEmail = customer.Email,
                    PhoneNumber = customer.Telphone,
                    CellPhone = customer.Cellphone,
                    PersonWebSite = customer.PersonWebSite,
                    Skype = customer.Skype,
                    DateCreated = customer.RegisterInfo.DateCreated,
                    RegisterUserAgentLanguage = customer.RegisterInfo.UserLanguage,
                    TotalLoginCount = customer.TotalLoginCount,
                    RegisterLanguageId = customer.RegisterLanguageId.HasValue ? customer.RegisterLanguageId : 0,
                    RegisterIp = customer.RegisterInfo.RegisterIp,
                    DefaultShippingAddress = customer.ShippingAddress,
                    DefaultBillingAddress = customer.BillingAddress,
                    HighRisk = customer.IsDanger,
                    DateLogin = customer.LastLoginDateTime,
                    OrderAmount = customer.HistoryAmount,
                    CustomerGroupId = customer.CustomerGroupId,
                    ClubLevel = customer.ClubLevel,
                    DateLastPlaceOrder = customer.DateLastPlaceOrder,
                    SourceType = (int)customer.RegisterInfo.SourceType,
                    SourceUrl = customer.RegisterInfo.UrlReferrer,
                    Status = true,
                    FaxNumber = customer.FaxNumber
                };
                if (customer.Birthday.IsNullOrEmpty())
                {
                    customerPo.Birthday = null;
                }
                else
                {
                    customerPo.Birthday = Convert.ToDateTime(customer.Birthday);
                }
            }
            return customerPo;
        }

        internal static List<CustomerPreferencePo> GetCustomerPreferencePoFromVo(Preference preference)
        {
            var customerPreferencePos = new List<CustomerPreferencePo>();
            if (!preference.IsNullOrEmpty())
            {
                if (preference.SizeUnit.HasValue)
                {
                    var preferenceSizeUnit = new CustomerPreferencePo
                    {
                        CustomerId = preference.CustomerId,
                        Key = "SizeUnit",
                        Value = ((int)preference.SizeUnit).ToString(CultureInfo.InvariantCulture)
                    };
                    customerPreferencePos.Add(preferenceSizeUnit);
                }

                if (preference.WeightUnit.HasValue)
                {
                    var preferenceWeightUnit = new CustomerPreferencePo
                    {
                        CustomerId = preference.CustomerId,
                        Key = "WeightUnit",
                        Value = ((int)preference.WeightUnit).ToString(CultureInfo.InvariantCulture)
                    };
                    customerPreferencePos.Add(preferenceWeightUnit);
                }

                if (preference.CurrencyId.HasValue)
                {
                    var preferenceCurrency = new CustomerPreferencePo
                    {
                        CustomerId = preference.CustomerId,
                        Key = "Currency",
                        Value = preference.CurrencyId.Value.ToString(CultureInfo.InvariantCulture)
                    };
                    customerPreferencePos.Add(preferenceCurrency);
                }

                if (preference.LanguageId.HasValue)
                {
                    var preferenceLanguage = new CustomerPreferencePo
                    {
                        CustomerId = preference.CustomerId,
                        Key = "Language",
                        Value = preference.LanguageId.Value.ToString(CultureInfo.InvariantCulture)
                    };
                    customerPreferencePos.Add(preferenceLanguage);
                }

                if (preference.ListShowType.HasValue)
                {
                    var preferenceListShowTyppe = new CustomerPreferencePo
                    {
                        CustomerId = preference.CustomerId,
                        Key = "ListShowType",
                        Value = ((int)preference.ListShowType).ToString(CultureInfo.InvariantCulture)
                    };
                    customerPreferencePos.Add(preferenceListShowTyppe);
                }

                if (preference.ListShowCount.HasValue)
                {
                    var preferenceListShowCount = new CustomerPreferencePo
                    {
                        CustomerId = preference.CustomerId,
                        Key = "ListShowCount",
                        Value = ((int)preference.ListShowCount).ToString(CultureInfo.InvariantCulture)
                    };
                    customerPreferencePos.Add(preferenceListShowCount);
                }
            }
            return customerPreferencePos;
        }

        internal static Preference GetCustomerPreferenceVoFromPo(List<CustomerPreferencePo> customerPreferencePos)
        {
            var preference = new Preference
            {
                CurrencyId = ServiceFactory.ConfigureService.GetCurrencyByCode(ServiceFactory.ConfigureService.CURRENCY_CODE_USD).CurrencyId,
                LanguageId = ServiceFactory.ConfigureService.EnglishLangId,
                ListShowType = ListShowType.List,
                ListShowCount = ListShowCount.S,
                SizeUnit = Unit.Metric,
                WeightUnit = Unit.Metric
            };
            if (!customerPreferencePos.IsNullOrEmpty())
            {
                foreach (var customerPreferencePo in customerPreferencePos)
                {
                    if (preference.CustomerId == 0)
                    {
                        preference.CustomerId = customerPreferencePo.CustomerId;
                    }
                    switch (customerPreferencePo.Key)
                    {
                        case "SizeUnit":
                            preference.CustomerId = customerPreferencePo.CustomerId;
                            preference.SizeUnit = (Unit)Convert.ToInt32(customerPreferencePo.Value);
                            break;
                        case "WeightUnit":
                            preference.WeightUnit = (Unit)Convert.ToInt32(customerPreferencePo.Value);
                            break;
                        case "Currency":
                            preference.CurrencyId = Convert.ToInt32(customerPreferencePo.Value);
                            break;
                        case "Language":
                            preference.LanguageId = Convert.ToInt32(customerPreferencePo.Value);
                            break;
                        case "ListShowType":
                            preference.ListShowType = (ListShowType)Convert.ToInt32(customerPreferencePo.Value);
                            break;
                        case "ListShowCount":
                            preference.ListShowCount = (ListShowCount)Convert.ToInt32(customerPreferencePo.Value);
                            break;
                    }
                }
            }
            return preference;
        }

        internal static Address GetCustomerAddressVoFromPo(CustomerAddressPo customerAddressPo)
        {
            Address address = null;
            if (!customerAddressPo.IsNullOrEmpty())
            {
                address = new Address
                {
                    AddressId = customerAddressPo.Id,
                    CustomerId = customerAddressPo.CustomerId,
                    FirstName = customerAddressPo.FirstName,
                    LastName = customerAddressPo.LastName,
                    FullName = customerAddressPo.FullName,
                    Gender = (Gender)Convert.ToChar(customerAddressPo.Gender),
                    Telphone = customerAddressPo.PhoneNumber,
                    CompanyName = customerAddressPo.Company,
                    Country = customerAddressPo.CountryId,
                    ProvinceId = customerAddressPo.ProvinceId,
                    Province = customerAddressPo.ProvinceName,
                    City = customerAddressPo.City,
                    ZipCode = customerAddressPo.ZipCode,
                    Street1 = customerAddressPo.StreetAddress,
                    Street2 = customerAddressPo.StreetAddress2
                };
            }
            return address;
        }

        internal static CustomerAddressPo GetCustomerAddressPoFromVo(Address address)
        {
            CustomerAddressPo customerAddressPo = null;
            if (!address.IsNullOrEmpty())
            {
                customerAddressPo = new CustomerAddressPo
                {
                    Id = address.AddressId,
                    CustomerId = address.CustomerId,
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    FullName = address.FullName,
                    Gender = ((char)address.Gender).ToString(CultureInfo.InvariantCulture),
                    PhoneNumber = address.Telphone,
                    Company = address.CompanyName,
                    CountryId = address.Country,
                    ProvinceId = address.ProvinceId,
                    ProvinceName = address.Province,
                    City = address.City,
                    ZipCode = address.ZipCode,
                    StreetAddress = address.Street1,
                    StreetAddress2 = address.Street2
                };
            }
            return customerAddressPo;
        }

        internal static CustomerAddressPo GetCustomerAddressPoFromVo(Address address, CustomerAddressPo customerAddressPo)
        {
            customerAddressPo.CustomerId = address.CustomerId;
            customerAddressPo.FirstName = address.FirstName;
            customerAddressPo.LastName = address.LastName;
            customerAddressPo.FullName = address.FullName;
            customerAddressPo.Gender = ((char)address.Gender).ToString(CultureInfo.InvariantCulture);
            customerAddressPo.PhoneNumber = address.Telphone;
            customerAddressPo.Company = address.CompanyName;
            customerAddressPo.CountryId = address.Country;
            customerAddressPo.ProvinceId = address.ProvinceId;
            customerAddressPo.ProvinceName = address.Province;
            customerAddressPo.City = address.City;
            customerAddressPo.ZipCode = address.ZipCode;
            customerAddressPo.StreetAddress = address.Street1;
            customerAddressPo.StreetAddress2 = address.Street2;
            return customerAddressPo;
        }

        internal static CustomerGroup GetCustomerGroupVoFromPo(CustomerGroupPo customerGroupPo)
        {
            CustomerGroup customerGroup = null;
            if (!customerGroupPo.IsNullOrEmpty())
            {
                customerGroup = new CustomerGroup
                {
                    Id = customerGroupPo.CustomerGroupId,
                    MinAmount = customerGroupPo.AmountMin,
                    MaxAmount = customerGroupPo.AmountMax,
                    Discount = customerGroupPo.Percentage
                };
            }

            return customerGroup;
        }

        internal static CustomerGroupPo GetCustomerGroupPoFromVo(CustomerGroup customerGroup)
        {
            CustomerGroupPo customerGroupPo = null;
            if (!customerGroup.IsNullOrEmpty())
            {
                customerGroupPo = new CustomerGroupPo
                {
                    CustomerGroupId = customerGroup.Id,
                    AmountMin = customerGroup.MinAmount,
                    AmountMax = customerGroup.MaxAmount,
                    Percentage = customerGroup.Discount
                };
            }
            return customerGroupPo;
        }

        internal static CustomerGroupDesc GetCustomerGroupDescVoFromPo(CustomerGroupDescPo customerGroupDescPo)
        {
            CustomerGroupDesc customerGroupDesc = null;
            if (!customerGroupDescPo.IsNullOrEmpty())
            {
                customerGroupDesc = new CustomerGroupDesc
                {
                    Id = customerGroupDescPo.Id,
                    CustomerGroupId = customerGroupDescPo.CustomerGroupId,
                    LanguageId = customerGroupDescPo.LanguageId,
                    GroupName = customerGroupDescPo.Name
                };
            }
            return customerGroupDesc;
        }

        internal static CustomerGroupDescPo GetCustomerGroupDescPoFromVo(CustomerGroupDesc customerGroupDesc)
        {
            CustomerGroupDescPo customerGroupDescPo = null;
            if (!customerGroupDesc.IsNullOrEmpty())
            {
                customerGroupDescPo = new CustomerGroupDescPo
                {
                    Id = customerGroupDesc.Id,
                    CustomerGroupId = customerGroupDesc.CustomerGroupId,
                    LanguageId = customerGroupDesc.LanguageId,
                    Name = customerGroupDesc.GroupName
                };
            }
            return customerGroupDescPo;
        }

        internal static ForgotPassword GetForgotPasswordVoFromPo(ForgotPasswordPo forgotPasswordPo)
        {
            ForgotPassword forgotPassword = null;
            if (!forgotPasswordPo.IsNullOrEmpty())
            {
                forgotPassword = new ForgotPassword
                {
                    Id = forgotPasswordPo.Id,
                    CustomerId = forgotPasswordPo.CustomerId,
                    EncryptedString = forgotPasswordPo.EncryptedString,
                    DateCreated = forgotPasswordPo.DateCreated,
                    DateExpired = forgotPasswordPo.DateExpired,
                    Status = forgotPasswordPo.Status,
                    DateUsed = forgotPasswordPo.DateUsed
                };
            }
            return forgotPassword;
        }

        internal static CustomerManager GetCustomerManagerVoFromPo(CustomerManagerPo customerManagerPo)
        {
            CustomerManager customerManager = null;
            if (!customerManagerPo.IsNullOrEmpty())
            {
                customerManager = new CustomerManager
                {
                    CustomerManagerId = customerManagerPo.CustomerManagerId,
                    Name = customerManagerPo.Name,
                    ChineseName = customerManagerPo.ChineseName,
                    Telphone = customerManagerPo.Telphone,
                    Skype = customerManagerPo.Skype,
                    Avatar = customerManagerPo.Avatar,
                    ServiceEmail = customerManagerPo.ServiceEmail,
                    CompanyEmail = customerManagerPo.CompanyEmail
                };
            }
            return customerManager;
        }

        internal static CustomerManagerPo GetCustomerManagerPoFromVo(CustomerManager customerManager)
        {
            CustomerManagerPo customerManagerPo = null;
            if (!customerManager.IsNullOrEmpty())
            {
                customerManagerPo = new CustomerManagerPo
                {
                    CustomerManagerId = customerManager.CustomerManagerId,
                    Name = customerManager.Name,
                    ChineseName = customerManager.ChineseName,
                    Telphone = customerManager.Telphone,
                    Skype = customerManager.Skype,
                    Avatar = customerManager.Avatar,
                    ServiceEmail = customerManager.ServiceEmail,
                    CompanyEmail = customerManager.CompanyEmail
                };
            }
            return customerManagerPo;
        }

        internal static CustomerAvatar GetCustomerAvatarFromPo(CustomerAvatarPo customerAvatarPo)
        {
            CustomerAvatar customerAvatar = new CustomerAvatar();
            if (!customerAvatarPo.IsNullOrEmpty())
            {
                ObjectHelper.CopyProperties(customerAvatarPo, customerAvatar, new string[] { });
                customerAvatar.AuditingStatus = EnumHelper.ToEnum<AuditingStatus>(customerAvatarPo.AuditingStatus);
            }
            return customerAvatar;
        }

        internal static CustomerHighRisk GetCustomerHighRiskVoFromPo(CustomerHighRiskPo customerHighRiskPo)
        {
            CustomerHighRisk customerHighRisk = null;
            if (!customerHighRiskPo.IsNullOrEmpty())
            {
                customerHighRisk = new CustomerHighRisk();
                ObjectHelper.CopyProperties(customerHighRiskPo, customerHighRisk, new string[] { });
            }
            return customerHighRisk;
        }

        internal static CustomerHighRiskPo GetCustomerHighRiskPoFromVo(CustomerHighRisk customerHighRisk)
        {
            CustomerHighRiskPo customerHighRiskPo = null;
            if (!customerHighRisk.IsNullOrEmpty())
            {
                customerHighRiskPo = new CustomerHighRiskPo();
                ObjectHelper.CopyProperties(customerHighRisk, customerHighRiskPo, new string[] { });
            }
            return customerHighRiskPo;
        }

        #region 销售备注

        internal static CustomerRemark GetCustomerRemarkVoFromPo(CustomerRemarkPo customerRemarkPo)
        {
            CustomerRemark customerRemark = null;
            if (!customerRemarkPo.IsNullOrEmpty())
            {
                customerRemark = new CustomerRemark
                {
                    Id = customerRemarkPo.Id,
                    CustomerId = customerRemarkPo.CustomerId,
                    Remark = customerRemarkPo.Remark,
                    DateCreated = customerRemarkPo.DateCreated,
                    AdminId = customerRemarkPo.AdminId
                };
            }
            return customerRemark;
        }
        #endregion
        #endregion
    }
}