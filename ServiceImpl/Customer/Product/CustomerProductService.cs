using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Entity.Product;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Product;
using Com.Panduo.ServiceImpl.Customer.Product.Dao;
using Com.Panduo.ServiceImpl.Product;
using Com.Panduo.ServiceImpl.Product.Dao;

namespace Com.Panduo.ServiceImpl.Customer.Product
{


    public class CustomerProductService : ICustomerProductService
    {
        public ICustomerProductDao CustomerProductDao { private get; set; }
        public IProductDao ProductDao { private get; set; }
        public ICustomerProductViewDao CustomerProductViewDao { private get; set; }

        public string ERROR_CUSTOMERPRODUCT_NOT_EXIST
        {
            get { return "ERROR_CUSTOMERPRODUCT_NOT_EXIST"; }
        }

        public string ERROR_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_PRODUCT_NOT_EXIST"; }
        }

        public int AddCustomerProduct(int customerId, int productId)
        {
            var customerProductPo = GetCustomerProductPo(customerId, productId);
            return CustomerProductDao.AddObject(customerProductPo);
        }

        public int AddCustomerProduct(int customerId, string productCode)
        {
            throw new NotImplementedException();
        }

        public void AddCustomerProducts(List<KeyValuePair<int, int>> customerProducts)
        {
            var customerProductPos = customerProducts.Select(customerProduct => GetCustomerProductPo(customerProduct.Key, customerProduct.Value));
            CustomerProductDao.AddObjects(customerProductPos);
        }

        public void AddCustomerProducts(List<CustomerProduct> customerProducts)
        {
            if (!customerProducts.IsNullOrEmpty())
            {
                CustomerProductDao.AddObjects(customerProducts.Select(GetCustomerProductPoFromVo));
            }
        }

        public void DeleteCustomerProduct(int id)
        {
            if (CustomerProductDao.GetObject(id).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMERPRODUCT_NOT_EXIST);
            }
            CustomerProductDao.DeleteObjectById(id);
        }

        public void RemoveCustomerProduct(int customerId, int productId)
        {
            CustomerProductDao.DeleteCustomerProduct(customerId, productId);
        }

        public void RemoveCustomerProducts(List<KeyValuePair<int, int>> customerProducts)
        {
            foreach (var customerProduct in customerProducts)
            {
                RemoveCustomerProduct(customerProduct.Key, customerProduct.Value);
            }
        }

        public Service.Product.Product GetCustomerProduct(int customerId, int productId)
        {
            if (CustomerProductDao.GetCustomerProduct(customerId, productId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CUSTOMERPRODUCT_NOT_EXIST);
            }
            var product = ProductDao.GetObject(productId);
            if (product.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PRODUCT_NOT_EXIST);
            }
            return ServiceFactory.ProductService.GetProductById(productId);
        }

        public List<CustomerProduct> GetCustomerProducts(int customerId)
        {
            return CustomerProductDao.GetCustomerProductPos(customerId).Select(GetCustomerProductVoFromPo).ToList();
        }

        public PageData<CustomerProduct> FindCustomerProducts(int customerId, int currentPage, int pageSize, IDictionary<CustomerProductSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerProductSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CustomerProductPo C");
            hqlHelper.AddWhere("CustomerId", HqlOperator.Eq, "CustomerId", customerId);
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CustomerProductSearchCriteria.KeyWrod:
                            hqlHelper.AddWhere(string.Format("(C.ProductModel Like {0})", ":keyWord"), HqlOperator.Exp, "keyWord", string.Format("%{0}%", item.Value));
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        //todo 排序条件
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.Id", false);
            }
            var pageDataPo = CustomerProductDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<CustomerProduct>();
            var voList = pageDataPo.Data.Select(GetCustomerProductVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        public PageData<CustomerProductView> FindCustomerProductsView(int customerId, int currentPage, int pageSize, IDictionary<CustomerProductSearchCriteria, object> searchCriteria,
            IList<Sorter<CustomerProductSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT C FROM CustomerProductViewPo C");
            hqlHelper.AddWhere("CustomerId", HqlOperator.Eq, "CustomerId", customerId);
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case CustomerProductSearchCriteria.KeyWrod:
                            hqlHelper.AddWhere(string.Format("(C.ProductModel Like {0})", ":keyWord"), HqlOperator.Exp, "keyWord", string.Format("%{0}%", item.Value));
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        //todo 排序条件
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("C.Id", false);
            }
            var pageDataPo = CustomerProductViewDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pagedataVo = new PageData<CustomerProductView>();
            var voList = pageDataPo.Data.Select(GetCustomerProductViewVoFromPo).ToList();

            pagedataVo.Pager = pageDataPo.Pager;
            pagedataVo.Data = voList;
            return pagedataVo;
        }

        #region 辅助方法

        internal static CustomerProduct GetCustomerProductVoFromPo(CustomerProductPo customerProductPo)
        {
            CustomerProduct customerProduct = null;
            if (!customerProductPo.IsNullOrEmpty())
            {
                customerProduct = new CustomerProduct
                {
                    Id = customerProductPo.Id,
                    CustomerId = customerProductPo.Id,
                    ProductId = customerProductPo.ProductId,
                    DateCreated = customerProductPo.DateCreated,
                    ProductModel = customerProductPo.ProductModel
                };
            }
            return customerProduct;
        }

        internal static CustomerProductPo GetCustomerProductPoFromVo(CustomerProduct customerProduct)
        {
            CustomerProductPo customerProductPo = null;
            if (!customerProduct.IsNullOrEmpty())
            {
                customerProductPo = new CustomerProductPo
                {
                    Id = customerProduct.Id,
                    CustomerId = customerProduct.CustomerId,
                    ProductId = customerProduct.ProductId,
                    DateCreated = customerProduct.DateCreated,
                    ProductModel = customerProduct.ProductModel
                };
            }
            return customerProductPo;
        }

        internal static CustomerProductPo GetCustomerProductPo(int customerId, int productId)
        {
            return new CustomerProductPo
            {
                CustomerId = customerId,
                ProductId = productId,
                DateCreated = DateTime.Now
            };
        }

        internal static CustomerProductView GetCustomerProductViewVoFromPo(CustomerProductViewPo customerProductViewPo)
        {
            CustomerProductView customerProductView = null;
            if (!customerProductViewPo.IsNullOrEmpty())
            {
                customerProductView = new CustomerProductView
                {
                    Id = customerProductViewPo.Id,
                    CustomerId = customerProductViewPo.CustomerId,
                    Image = customerProductViewPo.Image,
                    LanguageId = customerProductViewPo.LanguageId,
                    Name = customerProductViewPo.Name,
                    ProductId = customerProductViewPo.ProductId,
                    ProductModel = customerProductViewPo.ProductModel,
                    ProfitRate = customerProductViewPo.ProfitRate
                };
            }
            return customerProductView;
        }
        #endregion
    }
}