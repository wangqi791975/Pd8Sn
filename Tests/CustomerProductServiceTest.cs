using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Service;
using NUnit.Framework;
namespace Com.Panduo.Tests
{
    public class CustomerProductServiceTest : SpringTest
    {
        [Test]
        public void AddCustomerProductTest()
        {
            var a = ServiceFactory.CustomerProductService.AddCustomerProduct(2, 241);
        }

        [Test]
        public void AddCustomerProductsTest()
        {
            var customerProducts = new List<KeyValuePair<int, int>>
            {
                new KeyValuePair<int, int>(2, 4),
                new KeyValuePair<int, int>(2, 5)
            };
            ServiceFactory.CustomerProductService.AddCustomerProducts(customerProducts);
        }

        [Test]
        public void RemoveCustomerProductTest()
        {
            ServiceFactory.CustomerProductService.RemoveCustomerProduct(2, 3);
        }

        [Test]
        public void RemoveCustomerProductsTest()
        {
            ServiceFactory.CustomerProductService.RemoveCustomerProducts(new List<KeyValuePair<int, int>>
            {
                new KeyValuePair<int, int>(2,4),
                new KeyValuePair<int, int>(2,5)
            });
        }

        [Test]
        public void GetCustomerProductTest()
        {
            var a = ServiceFactory.CustomerProductService.GetCustomerProduct(2, 241);
        }

        [Test]
        public void FindCustomerProductsTest()
        {
            var a = ServiceFactory.CustomerProductService.FindCustomerProducts(2, 1, 3, null, null);
        }
    }
}