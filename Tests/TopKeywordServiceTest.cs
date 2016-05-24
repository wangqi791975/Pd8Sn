using System;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.SEO;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class TopKeywordServiceTest : SpringTest
    {
        readonly TopKeywordSubject _topKeywordSubject = new TopKeywordSubject
        {
            LanguageId = 1,
            TopKeywordSubjectName = "popularkeyword"
        };

        readonly TopKeywordSubject _topKeywordSubjectupdate = new TopKeywordSubject
        {
            TopKeywordSubjectId = 2,
            LanguageId = 2,
            TopKeywordSubjectName = "wogaigaigai"
        };

        readonly TopKeyword _topKeyword = new TopKeyword
        {
            TopKeywordSubjectId = 1,
            TopKeywordName = "1-11111"
        };

        readonly TopKeyword _topKeywordupdate = new TopKeyword
        {
            TopKeyworId = 2,
            TopKeywordSubjectId = 3,
            TopKeywordName = "1-22222"
        };

        /// <summary>
        /// 添加主题（业务异常测试通过，功能测试通过）
        /// </summary>
        [Test]
        public void AddKeywordSubjectTest()
        {
            //添加主题（业务异常测试通过，功能测试通过）
            int a = ServiceFactory.TopKeywordService.AddKeywordSubject(_topKeywordSubject);
        }

        /// <summary>
        /// 修改主题（业务异常测试通过，功能测试通过）
        /// </summary>
        [Test]
        public void UpdateKeywordSubjectTest()
        {
            ServiceFactory.TopKeywordService.UpdateKeywordSubject(_topKeywordSubjectupdate);
        }

        /// <summary>
        /// 删除主题 (功能测试通过）
        /// </summary>
        [Test]
        public void DeleteKeywordSubjectTest()
        {
            ServiceFactory.TopKeywordService.DeleteKeywordSubject(2);
        }

        /// <summary>
        /// 获取主题 (功能测试通过）
        /// </summary>
        [Test]
        public void GetKeywordSubjectByIdTest()
        {
            ServiceFactory.TopKeywordService.GetKeywordSubjectById(1);
        }

        /// <summary>
        /// 分页获取主题 (功能测试通过）
        /// </summary>
        [Test]
        public void FindKeywordSubjectsTest()
        {
            var pages = ServiceFactory.TopKeywordService.FindKeywordSubjects(2, 5, null, null);
        }

        /// <summary>
        /// 添加关键词 (功能测试通过）
        /// </summary>
        [Test]
        public void AddKeywordTest()
        {
            var a = ServiceFactory.TopKeywordService.AddKeyword(_topKeyword);
        }

        /// <summary>
        /// 更新关键词（业务异常测试通过，功能测试通过）
        /// </summary>
        [Test]
        public void UpdateKeywordTest()
        {
            ServiceFactory.TopKeywordService.UpdateKeyword(_topKeywordupdate);
        }

        /// <summary>
        /// 删除关键词 （业务异常测试通过，功能测试通过）
        /// </summary>
        [Test]
        public void DeleteKeywordTest()
        {
            ServiceFactory.TopKeywordService.DeleteKeyword(2);
        }

        /// <summary>
        /// 通过id获取关键词（功能测试通过）
        /// </summary>
        [Test]
        public void GetKeywordTest()
        {
            var a = ServiceFactory.TopKeywordService.GetKeyword(1);
        }

        /// <summary>
        /// 通过主题id获取关键词（功能测试通过）
        /// </summary>
        [Test]
        public void GetKeywordsBySubjectId()
        {
            var a = ServiceFactory.TopKeywordService.GetKeywordsBySubjectId(1);
        }
    }
}