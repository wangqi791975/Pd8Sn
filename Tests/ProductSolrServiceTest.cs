using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public  class ProductSolrServiceTest : SpringTest
    {
        [Test]
        public void TestNormalSearch()
        {
            var currentPage = 1;
            var pageSize = 90;
            var searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.NormalArea},
                {ProductSearchCriteria.CategoryPath,1803},
                {ProductSearchCriteria.IgnoreProductIds,new List<int>(){111,222}},
            };

            var sorterCriteria = new List<Sorter<ProductSorterCriteria>>
            {
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.BestMatch,false), 
            };
            var isIncludeProperty = true;
            var isIncludeCategory = true;

            SearchProduct(currentPage,pageSize,searchCriteria,sorterCriteria,isIncludeProperty,isIncludeCategory);
        }

         [Test]
        public void TestSimilarSearch()
        {
            var currentPage = 1;
            var pageSize = 90;
            var searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.SimilarItem},
                {ProductSearchCriteria.SimilarProductId,244}
            };

            var sorterCriteria = new List<Sorter<ProductSorterCriteria>>
            {
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.BestMatch,false), 
            };
            var isIncludeProperty = true;
            var isIncludeCategory = true;

            SearchProduct(currentPage,pageSize,searchCriteria,sorterCriteria,isIncludeProperty,isIncludeCategory);
        }
        

        [Test]
        public void TestPromotionSearch()
        {
            var currentPage = 1;
            var pageSize = 90;
            var searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.Promotion},
                {ProductSearchCriteria.PromotionDiscount,0.4},

            };

            var sorterCriteria = new List<Sorter<ProductSorterCriteria>>
            {
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.SaleCount,false),
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.JoinDateNewToOld,false)
            };
            var isIncludeProperty = true;
            var isIncludeCategory = true;

            SearchProduct(currentPage, pageSize, searchCriteria, sorterCriteria, isIncludeProperty, isIncludeCategory);
        }

        [Test]
        public void TestNewArrivalSearch()
        {
            var currentPage = 1;
            var pageSize = 90;
            var searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.NewArrival},
                {ProductSearchCriteria.CategoryPath,2}

            };

            var sorterCriteria = new List<Sorter<ProductSorterCriteria>>
            {
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.JoinDateNewToOld,false),
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.LastModifyDate,false)
            };
            var isIncludeProperty = true;
            var isIncludeCategory = true;

            SearchProduct(currentPage, pageSize, searchCriteria, sorterCriteria, isIncludeProperty, isIncludeCategory);
        }

        [Test]
        public void TestKeywordSearch()
        {
            var currentPage = 1;
            var pageSize = 90;
            var keyword = "beads";
            var searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.NormalArea},
                //{ProductSearchCriteria.Keyword,keyword},
                {ProductSearchCriteria.CategoryId,1803},
                {ProductSearchCriteria.PropertyValueIds,new[]{50}}

            };

            var sorterCriteria = new List<Sorter<ProductSorterCriteria>>
            {
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.BestMatch,false)
            };
            var isIncludeProperty = true;
            var isIncludeCategory = true;

            SearchProduct(currentPage, pageSize, searchCriteria, sorterCriteria, isIncludeProperty, isIncludeCategory);
        }

        [Test]
        public void TestBatchMachSearch()
        {
            var currentPage = 1;
            var pageSize = 90; 
            var searchCriteria = new Dictionary<ProductSearchCriteria, object>
            {
                {ProductSearchCriteria.ProductSearchAreaType,ProductSearchAreaType.BestMatch},
                //{ProductSearchCriteria.Keyword,keyword},
                {ProductSearchCriteria.BestMatchProductId,17898}, 

            };

            var sorterCriteria = new List<Sorter<ProductSorterCriteria>>
            {
                new Sorter<ProductSorterCriteria>(ProductSorterCriteria.BestMatch,false)
            };
            var isIncludeProperty = true;
            var isIncludeCategory = true;

            SearchProduct(currentPage, pageSize, searchCriteria, sorterCriteria, isIncludeProperty, isIncludeCategory);
        }

        private void SearchProduct(int currentPage, int pageSize,
            IDictionary<ProductSearchCriteria, object> searchCriteria,
            IList<Sorter<ProductSorterCriteria>> sorterCriteria, bool isIncludeProperty, bool isIncludeCategory)
        {
            //先缓存类别
            ServiceFactory.CategoryService.GetAllCategories();

            var result = ServiceFactory.ProductService.SearchProducts(currentPage, pageSize, searchCriteria, sorterCriteria, isIncludeProperty, isIncludeCategory);

            var pageData = result.ProductPageData;
            var productCategories = result.ProductCategories;
            var poductProperties = result.ProductProperties;


            Console.WriteLine("总产品数:{0}", pageData.Pager.TotalRowCount);
            Console.WriteLine("类别产品数统计，共{0}", productCategories.Sum(c => c.Qty));
            foreach (var item in productCategories)
            {
                Console.WriteLine("{0}:{1}", item.Category.CategoryName,item.Qty); 
            }

            Console.WriteLine("属性产品数统计，共{0}", poductProperties.Sum(c=>c.Qty));
            foreach (var item in poductProperties)
            {
                Console.WriteLine("{0}:{1}", item.Property.PropertyName, item.Qty); 
                Console.WriteLine("下面输出{0}的属性值组:", item.Property.PropertyName);
                foreach (var groupItem in item.PropertyValueGroupQtys)
                {
                    Console.WriteLine("      属性组-{0}-{1}:{2}", groupItem.PropertyValueGroup.GroupId,groupItem.PropertyValueGroup.PropertyValueGroupName, groupItem.Qty);
                    foreach (var valueItem in groupItem.PropertyValueQtys)
                    {

                        Console.WriteLine("             属性组的属性值-{0}-{1}:{2}", valueItem.Key.PropertyValueId,valueItem.Key.PropertyValueName, valueItem.Value);
                    }
                }

                Console.WriteLine("下面输出{0}的属性值:", item.Property.PropertyName);
                foreach (var valueItem in item.PropertyValueQtys)
                {

                    Console.WriteLine("      属性值-{0}-{1}:{2}", valueItem.Key.PropertyValueId, valueItem.Key.PropertyValueName, valueItem.Value);
                }
            }
        }
    }
}
