
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Com.Panduo.Service;
using Com.Panduo.Service.SEO;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class MetaServiceTest : SpringTest
    {
        /// <summary>
        /// set meta首页信息，测试通过
        /// </summary>
        [Test]
        public void SetMetaHome()
        {
            var vo1 = new MetaHome
            {
                Breadcrumb = "bread",
                Description = "desc",
                Keywords = "key",
                LanguageId = 1,
                Title = "title",
                PageType = MetaHomePageType.NewIndex
            };
            var vo2 = new MetaHome
            {
                Breadcrumb = "bread222",
                Description = "desc222",
                Keywords = "key222",
                LanguageId = 2,
                Title = "title222",
                PageType = MetaHomePageType.ProductDetail
            };
            var vo3 = new MetaHome
            {
                Breadcrumb = "bread33",
                Description = "desc333",
                Keywords = "key333",
                LanguageId = 1,
                Title = "title333",
                PageType = MetaHomePageType.ProductDetail
            };
            var list = new List<MetaHome>();
            list.Add(vo1);
            list.Add(vo2);
            list.Add(vo3);
            ServiceFactory.MetaService.SetMetaHome(list);
        }

        /// <summary>
        /// 获取meta首页信息，测试通过
        /// </summary>
        [Test]
        public void GetMetaHomesByLanguageId()
        {
            var a = ServiceFactory.MetaService.GetMetaHomesByLanguageId(1);
            foreach (var b in a)
            {
                Console.WriteLine(b.Breadcrumb);
            }
        }
    }
}
