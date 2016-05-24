using System;
using System.Collections.Generic;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Suggestion;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class SuggestionServiceTest : SpringTest
    {
        [Test]
        public void TestMethod1()
        {
            var s = ServiceFactory.SuggestionService;

            var suggestionDetail = new List<SuggestionDetail>
            {
                new SuggestionDetail{ObjectId = 3,Score = 5},
                new SuggestionDetail{ObjectId = 4,Score = 5},
                new SuggestionDetail{ObjectId = 5,Score = 5},
                new SuggestionDetail{ObjectId = 6,Score = 5},
                new SuggestionDetail{ObjectId = 7,Score = 5},
                new SuggestionDetail{ObjectId = 8,Score = 5},
                new SuggestionDetail{ObjectId = 9,Score = 5},
                new SuggestionDetail{ObjectId = 10,Score = 5},
                new SuggestionDetail{ObjectId = 11,Score = 5}
            };

            var suggestionAttachment = new List<SuggestionAttachment>
            {
                new SuggestionAttachment{Name = "fujianname1",Path = "~/path/"},
                new SuggestionAttachment{Name = "fujianname2",Path = "~/path/"}
            };

            var suggestionContent = new SuggestionContent
            {
                FullName = "wqwqwqwq",
                Email = "ewq@gmail.com",
                Content = "very good！！！",
                Details = suggestionDetail,
                CreateDateTime = DateTime.Now,
                AttachmentList = suggestionAttachment
            };

            //获取语种对应所有项 (功能测试通过）
            //var a = s.GetAllSuggestionItems(1);

            //获取语种对应所有对象(功能测试通过）
            //var a = s.GetSuggestionObjectsByItemId(2);

            //获取所有对象(功能测试通过）
            //var a = s.GetAllSuggestionObjects();

            //客户建议反馈(功能测试通过）
            //s.SuggestionFeedback(suggestionContent);
        }
    }
}