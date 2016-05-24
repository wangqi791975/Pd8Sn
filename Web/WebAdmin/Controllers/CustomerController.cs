using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.SystemMail;
using Com.Panduo.Web.Common;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.Web.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        [HttpGet]
        public ActionResult ModifyEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyEmail(string emial, string newemail, string conemail)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            if (newemail != conemail)
            {
                hashtable["msg"] = "输入值和新邮箱不一致，请修改！";
                hashtable["error"] = true;
            }

            if (!RegexHelper.IsEmail(newemail))
            {
                hashtable["msg"] = "邮箱格式不正确，必须是a@b的形式";
                hashtable["error"] = true;
            }
            var customer = ServiceFactory.CustomerService.GetCustomerByEmail(emial);
            if (customer.IsNullOrEmpty())
            {
                hashtable["msg"] = "当前邮箱在系统中不存在，请修改！";
                hashtable["error"] = true;
            }
            if (!Convert.ToBoolean(hashtable["error"]))
            {
                try
                {
                    ServiceFactory.CustomerService.ChangeEmail(customer.CustomerId, newemail);
                    ServiceFactory.MailService.AddEmailProduceLog(new EmailProduceLog
                    {
                        Attachment = emial,
                        DateAdded = DateTime.Now,
                        DateCreatedXml = DateTime.Now,
                        EmailType = MailType.CustomerRegister,
                        LanguageId = customer.RegisterLanguageId.HasValue ? customer.RegisterLanguageId.Value : ServiceFactory.ConfigureService.EnglishLangId,
                        HasAttachment = false,
                        IsCreatedXml = false,
                        KeyNo = customer.CustomerId.ToString(CultureInfo.InvariantCulture)
                    });
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_CUSTOMER_NOT_EXIST":
                            hashtable["msg"] = "客户不存在";
                            hashtable["error"] = true;
                            break;
                        case "ERROR_EMAIL_EXIST":
                            hashtable["msg"] = "要修改的邮箱已存在";
                            hashtable["error"] = true;
                            break;
                    }
                }
            }
            return Json(hashtable);
        }

        public ActionResult Avatar(int? id)
        {
            int page = id ?? 1;
            ViewBag.Page = page;
            return View();
        }

        public ActionResult AvatarList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = 20;

            //PageData<Customer> pageData = ServiceFactory.CustomerService.FindAllCustomers(page, pageSize);
            PageData<CustomerAvatar> pageData = ServiceFactory.CustomerService.FindAllCustomerAvatars(page, pageSize);

            ViewData.Model = pageData;
            return View();
        }

        #region 客户管理

        [HttpGet]
        public ActionResult CustomerManage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CustomerManageList(string keyword = "", int custype = 0, int lang = 0, int source = 0, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<CustomerSearchCriteria, object>
            {
                {CustomerSearchCriteria.KeyWord, keyword},
                {CustomerSearchCriteria.CustomerType, custype},
                {CustomerSearchCriteria.LanguageId, lang},
                {CustomerSearchCriteria.SourceType, source}
            };

            var customerManges = ServiceFactory.CustomerService.FindAllCustomers(page, pageSize, searchCriteria, new List<Sorter<CustomerSorterCriteria>>());
            return View(customerManges);
        }

        [HttpGet]
        public ActionResult CustomerManageDetail(int id, int page)
        {
            var customer = ServiceFactory.CustomerService.GetCustomerById(id);
            var customerDefaultAddress = ServiceFactory.CustomerService.GetDefaultShippingAddress(id);
            var customerAddress = ServiceFactory.CustomerService.GetAddressesByCustomerId(id).Where(m => m.AddressId != customerDefaultAddress.AddressId);
            var customerPreference = ServiceFactory.CustomerService.GetPreferenceByCustomerId(id);
            var customerNewsletter = ServiceFactory.NewsletterService.GetNewsletter(id);
            var customerOrders = ServiceFactory.OrderService.GetOrdersByCustomerId(id, 1, 10000, new Dictionary<OrderSearchCriteria, object>(), null).Data;
            var customerLastOrder = ServiceFactory.OrderService.GetCustomerLatestOrder(id, null);
            var customerReviewsCount = ServiceFactory.ReviewService.GetCustomerReivewsCount(id);

            ViewBag.CustomerDefaultAddress = customerDefaultAddress;
            ViewBag.CustomerAddress = customerAddress.ToList();
            ViewBag.CustomerPreference = customerPreference;
            ViewBag.CustomerNewsletter = customerNewsletter;
            ViewBag.CustomerOdersCount = customerOrders.Count;
            ViewBag.CustomerLastOrder = customerLastOrder.OrderCost.IsNullOrEmpty() ? 0 : customerLastOrder.OrderCost.TotalOrderAmt;
            ViewBag.CustomerReviewsCount = customerReviewsCount;
            ViewBag.Page = page;

            return View(customer);
        }

        [HttpPost]
        public void ChangeStatus(int id)
        {
            ServiceFactory.CustomerService.UpdateCustomerStatus(id);
        }

        [HttpGet]
        public ActionResult CustomerManagerRemark(int id)
        {
            ViewBag.CustomerId = id;
            var customerRemark = ServiceFactory.CustomerService.GetCustomerRemark(id);
            return View(customerRemark);
        }

        [HttpPost]
        public ActionResult CustomerManagerRemark(int customerId, string remark)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            try
            {
                ServiceFactory.CustomerService.SetCustomerRemark(customerId, remark, SessionHelper.CurrentAdminUser.AdminId);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "操作失败！";
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult Subscribe()
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            string customerIds = Request["chksubscribe"];
            if (!customerIds.IsNullOrEmpty())
            {
                var customerIdList = customerIds.Split(',');
                var newsletters = new List<Newsletter>();
                foreach (var customerId in customerIdList)
                {
                    var customer = ServiceFactory.CustomerService.GetCustomerById(Int32.Parse(customerId));
                    var newsletter = new Newsletter
                    {
                        CustomerId = customer.CustomerId,
                        Email = customer.Email,
                        FullName = customer.FullName,
                        IsUnNewsletter = false,
                        LanguageId = customer.RegisterLanguageId.HasValue ? customer.RegisterLanguageId.Value : 1,
                        NewsletterDateTime = DateTime.Now
                    };
                    newsletters.Add(newsletter);
                }
                ServiceFactory.NewsletterService.Subscribe(newsletters);
            }
            else
            {
                hashtable["error"] = true;
                hashtable["msg"] = "请选择需要订阅的客户！";
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult BindCustomerProduct()
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            foreach (string upload in Request.Files)
            {
                var file = Request.Files[upload];
                var path = HttpContext.Server.MapPath("../Upload/BindCustomerProduct");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (file != null)
                {
                    string fileName = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
                    string filePath = Path.Combine(path, fileName + Path.GetExtension(file.FileName));
                    file.SaveAs(filePath);
                    ServiceFactory.CustomerProductService.AddCustomerProducts(ExcelReadToCustomerProduct(filePath, hashtable));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }
            return Json(hashtable);
        }


        /// <summary>
        /// 读取客户绑定商品信息
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        private List<CustomerProduct> ExcelReadToCustomerProduct(string file, Hashtable hashtable)
        {
            try
            {
                var customerProducts = new List<CustomerProduct>();
                using (var excelHelper = new ExcelHelper(file))
                {
                    var dt = excelHelper.ExcelToDataTable("Sheet1", true);
                    foreach (DataRow row in dt.Rows)
                    {
                        string email = row[0].ToString().Trim();
                        int productId = Int32.Parse(row[1].ToString().Trim());
                        var customer = ServiceFactory.CustomerService.GetCustomerByEmail(email);
                        var product = ServiceFactory.ProductService.GetProductById(productId);
                        if (!customer.IsNullOrEmpty() && !product.IsNullOrEmpty())
                        {
                            var customerProduct = new CustomerProduct
                            {
                                CustomerId = customer.CustomerId,
                                DateCreated = DateTime.Now,
                                ProductId = productId,
                                ProductModel = ServiceFactory.ProductService.GetProductById(productId).ProductCode
                            };
                            customerProducts.Add(customerProduct);
                        }
                    }
                }
                return customerProducts;
            }
            catch
            {
                hashtable["error"] = true;
                hashtable["msg"] = "excel格式不正确";
                return null;
            }
        }
        #endregion

        #region 客户绑定产品
        [HttpGet]
        public ActionResult CustomerProduct(int id)
        {
            ViewBag.CustomerId = id;
            return View();
        }

        [HttpGet]
        public ActionResult CustomerProductList(string keyword, int id, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<CustomerProductSearchCriteria, object>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(CustomerProductSearchCriteria.KeyWrod, keyword);
            }
            PageData<CustomerProductView> customerProducts = ServiceFactory.CustomerProductService.FindCustomerProductsView(id, page, pageSize, searchCriteria, null);
            ViewBag.CustomerId = id;
            return View(customerProducts);
        }

        [HttpPost]
        public ActionResult DeleteCustomerProduct(FormCollection form)
        {
            var hashtable = new Hashtable
            {
                {"error", false},
                {"msg", string.Empty},
                {"getlist", true},
                {"url", ""}
            };
            try
            {
                var ids = form["ids"];
                if (!ids.IsNullOrEmpty())
                    foreach (var id in ids.Split(','))
                    {
                        ServiceFactory.CustomerProductService.DeleteCustomerProduct(Convert.ToInt32(id));
                    }
                else
                {
                    hashtable["error"] = true;
                    hashtable["msg"] = "请选择要删除的用户！";
                    hashtable["getlist"] = false;
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_CUSTOMERPRODUCT_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "客户产品不存在！";
                        hashtable["getlist"] = false;
                        break;
                }
            }
            return Json(hashtable);
        }

        #endregion
    }
}
