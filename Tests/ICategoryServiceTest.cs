using System;
using System.Net;
using System.Web;
using Com.Panduo.Common;
using Com.Panduo.Service;
using NHibernate.Linq;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 ICategoryServiceTest 的测试类，旨在
    ///包含所有 ICategoryServiceTest 单元测试
    ///</summary>
    [TestFixture]
    public class CategoryServiceTest : SpringTest
    {

        /// <summary>
        ///GetCategory 的测试
        ///</summary>
        [Test]
        public void GetCategories()
        {
            Console.WriteLine(HttpUtility.UrlDecode("http://openapi.etsy.com/svc/oembed/?url=http%3a%2f%2fwww.etsy.com%2flisting%2f128235512%2fetsy-i-buy-from-real-people-tote-bag"));

            Console.WriteLine(HttpUtility.UrlDecode("https://www.pinterest.com/pin/create/button/?url=https%3A%2F%2Fwww.etsy.com%2Flisting%2F156901432%2Fglass-beaded-garden-art-on-mesquite%3Futm_source%3DPinterest%26utm_medium%3DPageTools%26utm_campaign%3DShare&media=https%3A%2F%2Fimg0.etsystatic.com%2F030%2F1%2F5287973%2Fil_570xN.528012210_8lf0.jpg&description=Glass+Beaded+Garden+Art+on+Mesquite+by+LTreatDesigns+on+Etsy"));

            Console.WriteLine(HttpUtility.UrlDecode("https://www.pinterest.com/pin/create/button/?url=http%3A%2F%2Fwww.flickr.com%2Fphotos%2Fkentbrew%2F6851755809%2F&media=http%3A%2F%2Ffarm8.staticflickr.com%2F7027%2F6851755809_df5b2051c9_z.jpg&description=Next%20stop%3A%20Pinterest"));

            Console.WriteLine(HttpUtility.UrlEncode("/pinterest.html?time=001"));
            var lstCategories = ServiceFactory.CategoryService.GetAllLeafCategories();

            lstCategories.ForEach(x => Console.WriteLine(string.Format("Id:{0}  Name:{1}  Parent:{2}", x.CategoryId, x.CategoryName, x.ParentId)));
        }

        /// <summary>
        ///GetCategoriesByXX 的测试
        ///</summary>
        [Test]
        public void GetCategoriesByXx()
        {
            //var list = ServiceFactory.CategoryService.GetAllSubCategories(146);
            //var list = ServiceFactory.CategoryService.GetCategoryBindedAllProperties(146);
            var list = ServiceFactory.CategoryService.GetCategoryLanguageById(146);

            Console.WriteLine(list.Count);
        }

        /// <summary>
        ///GetCategoriesByXX 的测试
        ///</summary>
        [Test]
        public void GetCategoriesByTop()
        {
            var list = ServiceFactory.CategoryService.GetTopSubCategoriesById(146, 5);

            Console.WriteLine(list.Count);
        }

        [Test]
        public void DeleteCategoryKeywordByCategoryId()
        {
            ServiceFactory.CategoryService.DeleteCategoryKeywordByCategoryId(339);
        }

        [Test]
        public void GetParentCategoryById()
        {
            var category = ServiceFactory.CategoryService.GetParentCategoryById(1952);
            Console.WriteLine(category.CategoryId + "->" + category.CategoryName);
        }

        [Test]
        public void SaveCategoriesImage()
        {
            WebClient myWebClient = new WebClient();
            var categories = ServiceFactory.CategoryService.GetAllCategories();
            int index = 0;
            foreach (var category in categories)
            {
                if (!category.CategoryImage.IsNullOrEmpty())
                {
                    var url = "http://www.8seasons.com/images/" + category.CategoryImage;
                    var newFileName = url.Substring(url.LastIndexOf("/") + 1);
                    var filePath = @"E:\Upload\0407_17\" + newFileName;
                    try
                    {
                        Console.WriteLine(index + "=>" + newFileName + "=>" + url);
                        myWebClient.DownloadFile(url, filePath);
                        category.CategoryImage = "/Upload/Category/" + newFileName;
                        ServiceFactory.CategoryService.SetCategoryBaseInfo(category);
                        index ++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ":" + url);
                    }
                }
                
            }
        }
    }
}
